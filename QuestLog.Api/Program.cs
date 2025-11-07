using Microsoft.EntityFrameworkCore;
using QuestLog.Application.Feature.Users.Commands; 
using QuestLog.Domain.Interfaces;
using QuestLog.Infrastructure.Data;
using QuestLog.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<QuestLogDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(CreateUserCommand).Assembly)
);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IAvatarRepository, AvatarRepository>();



builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "QuestLog API",
        Version = "v1",
        Description = "API для гейміфікованого планера завдань"
    });
});


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); 
    app.UseSwaggerUI(options => 
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "QuestLog API V1");
        options.RoutePrefix = string.Empty; 
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
