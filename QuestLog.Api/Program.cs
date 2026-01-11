using Microsoft.EntityFrameworkCore;
using QuestLog.Application.Feature.Users.Commands;
using QuestLog.Domain.Interfaces;
using QuestLog.Infrastructure.Data;
using QuestLog.Infrastructure.Repositories;
using QuestLog.Infrastructure.Services;
using QuestLog.Api.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using FluentValidation;
using QuestLog.Application.Common.Behaviors;
using System.Text.Json.Serialization;
using QuestLog.Application.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var allowedOrigins = builder.Configuration["CorsSettings:AllowedOrigins"]?
                         .Split(',', StringSplitOptions.RemoveEmptyEntries)
                         .Select(o => o.Trim())
                         .ToArray() 
                     ?? new[] { "http://localhost:3000" };

var allowSpecificOrigins = "_allowSpecificOrigins";

builder.Services.AddDbContext<QuestLogDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]!))
        };
    });
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(CreateUserCommand).Assembly)
);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>(); 
builder.Services.AddScoped<IAvatarRepository, AvatarRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "QuestLog API",
        Version = "v1",
        Description = "API для гейміфікованого планера завдань"
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token : Bearer {your_token}\n\nПриклад: Bearer eyJhbGciOiJIUzI1Ni..."
    });
    
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowSpecificOrigins,
        policy =>
        {
            policy
                .WithOrigins(allowedOrigins)
                .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
                .WithHeaders("Authorization", "Content-Type", "Accept")
                .AllowCredentials();
        });
});

builder.Services.AddAuthorization();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IAiService, OpenRouterService>();
builder.Services.AddScoped<ITaskDifficultyEvaluator, AiTaskDifficultyEvaluator>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<QuestLogDbContext>();
        
        // Ця команда перевіряє, чи є база. 
        // Якщо немає — створює її. 
        // Якщо є — накатує нові міграції.
        context.Database.Migrate(); 
        Console.WriteLine("Database migrated successfully.");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger(); 
    app.UseSwaggerUI(options => 
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "QuestLog API V1");
        options.RoutePrefix = string.Empty; 
    });
//}

//app.UseHttpsRedirection();

app.UseCors(allowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();