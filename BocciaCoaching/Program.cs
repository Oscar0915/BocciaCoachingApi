using BocciaCoaching.Data;
using BocciaCoaching.Models.Configuration;
using BocciaCoaching.Repositories;
using BocciaCoaching.Repositories.AssesstStrength;
using BocciaCoaching.Repositories.Interfaces;
using BocciaCoaching.Repositories.Interfaces.IAssesstStrength;
using BocciaCoaching.Repositories.Interfaces.ITeams;
using BocciaCoaching.Repositories.NotificationTypes;
using BocciaCoaching.Repositories.Statistic;
using BocciaCoaching.Repositories.Statistic.Interfce;
using BocciaCoaching.Repositories.Teams;
using BocciaCoaching.Services;
using BocciaCoaching.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

// Configurar WebApplicationOptions para evitar el error de inotify
var options = new WebApplicationOptions
{
    Args = args,
    ContentRootPath = Directory.GetCurrentDirectory()
};

var builder = WebApplication.CreateBuilder(options);

// Deshabilitar el monitoreo de archivos de configuración para evitar el límite de inotify
builder.Configuration.Sources.Clear();
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: false)
    .AddEnvironmentVariables();

if (args != null)
{
    builder.Configuration.AddCommandLine(args);
}

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200", "https://localhost:4200", "https://bocciacoaching.com", "http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials(); // Necesario para SignalR
        });
    
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

// Configurar EmailSettings
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddMemoryCache();

// SignalR
builder.Services.AddSignalR();

//Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IAssessStrengthService, AssessStrengthService>();
builder.Services.AddScoped<BocciaCoaching.Services.Interfaces.INotificationService, BocciaCoaching.Services.NotificationService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IStatisticsService, StatisticsService>();

/*
Repositories - Repositorios
*/

// Team - Equipos
builder.Services.AddScoped<ITeamValidationRepository, TeamValidationRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();

//User - Usuario
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Assess Strength - Prueba de fuerza 
builder.Services.AddScoped<IAssessStrengthRepository, AssessStrengthRepository>();
builder.Services.AddScoped<IValidationsAssetsStrength, ValidatiosStrenthRepository>();

// Statistic - Estadisticas
builder.Services.AddScoped<IStatisticAssessStrength, StatisticAssessStrength>();

// NotificationType - Tipos de notificación
builder.Services.AddScoped<INotificationTypeRepository, NotificationTypeRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();
app.UseRouting(); 

// Use CORS - permitir credenciales para SignalR
app.UseCors("AllowSpecificOrigins");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Mapear SignalR Hub
app.MapHub<BocciaCoaching.Hubs.ChatHub>("/chatHub");

app.Run();
