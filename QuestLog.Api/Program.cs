using MediatR;
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

var builder = WebApplication.CreateBuilder(args);

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
            policy.WithOrigins("http://localhost:3000")
                .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
                .WithHeaders("Authorization", "Content-Type", "Accept");
        });
});

builder.Services.AddAuthorization();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); 
    app.UseSwaggerUI(options => 
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "QuestLog API V1");
        options.RoutePrefix = string.Empty; 
    });
}

//app.UseHttpsRedirection();

app.UseCors(allowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();