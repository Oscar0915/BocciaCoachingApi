using BocciaCoaching.Data;
using BocciaCoaching.Infrastructure.Api;
using BocciaCoaching.Infrastructure.Auth;
using BocciaCoaching.Repositories.Auth;
using BocciaCoaching.Services.Auth;
using BocciaCoaching.Models.Configuration;
using BocciaCoaching.Repositories;
using BocciaCoaching.Repositories.AssesstStrength;
using BocciaCoaching.Repositories.AssessDirection;
using BocciaCoaching.Repositories.AssessSaremas;
using BocciaCoaching.Repositories.Interfaces;
using BocciaCoaching.Repositories.Interfaces.IAssesstStrength;
using BocciaCoaching.Repositories.Interfaces.IAssessDirection;
using BocciaCoaching.Repositories.Interfaces.IAssessSaremas;
using BocciaCoaching.Repositories.Interfaces.IMacrocycle;
using BocciaCoaching.Repositories.Interfaces.IMicrocycleType;
using BocciaCoaching.Repositories.Interfaces.IRol;
using BocciaCoaching.Repositories.Interfaces.ITeams;
using BocciaCoaching.Repositories.Interfaces.ITrainingSession;
using BocciaCoaching.Repositories.Interfaces.IWellness;
using BocciaCoaching.Repositories.Macrocycle;
using BocciaCoaching.Repositories.MicrocycleType;
using BocciaCoaching.Repositories.Rol;
using BocciaCoaching.Repositories.TrainingSession;
using BocciaCoaching.Repositories.NotificationTypes;
using BocciaCoaching.Repositories.Statistic;
using BocciaCoaching.Repositories.Statistic.Interfce;
using BocciaCoaching.Repositories.Teams;
using BocciaCoaching.Repositories.Wellness;
using BocciaCoaching.Services;
using BocciaCoaching.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Stripe;
using SubscriptionService = BocciaCoaching.Services.SubscriptionService;

// Deshabilitar reloadOnChange ANTES de que CreateBuilder inicie los file watchers.
// Esto evita el error de inotify en Linux cuando se alcanza el límite de instancias.
Environment.SetEnvironmentVariable("DOTNET_hostBuilder__reloadConfigOnChange", "false");

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ApplicationName = "BocciaCoaching"
});

// Reemplazar todos los sources de configuración para asegurar reloadOnChange = false
// Se usa Host.ConfigureAppConfiguration para evitar el error de WebHost con nombre de aplicación cambiado
builder.Host.ConfigureAppConfiguration((ctx, config) =>
{
    config.Sources.Clear();
    config.SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
          .AddJsonFile($"appsettings.{ctx.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: false)
          .AddEnvironmentVariables();
    if (args != null)
        config.AddCommandLine(args);
});

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200", "https://localhost:4200", "https://bocciacoaching.com", "http://localhost:3000", "https://app-bocciacoaching.onrender.com")
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

// La versión del servidor MySQL se detecta UNA sola vez al arrancar. Antes, ServerVersion.AutoDetect
// estaba dentro del lambda de AddDbContext, que se ejecuta por cada DbContext (es decir, por cada
// request): abría una conexión extra a la BD en cada petición solo para consultar la versión.
// Opcional: fijar "Database:ServerVersion" (p. ej. "8.0.36-mysql") para no conectar ni al arrancar.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Falta la cadena de conexión 'DefaultConnection'.");
var configuredServerVersion = builder.Configuration["Database:ServerVersion"];
var mySqlServerVersion = string.IsNullOrWhiteSpace(configuredServerVersion)
    ? ServerVersion.AutoDetect(connectionString)
    : ServerVersion.Parse(configuredServerVersion);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, mySqlServerVersion));

// Configurar EmailSettings
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// Configurar StripeSettings
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("StripeSettings"));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        // JSON indentado solo en desarrollo: en producción aumenta el tamaño de las respuestas sin aportar nada.
        options.JsonSerializerOptions.WriteIndented = builder.Environment.IsDevelopment();
    })
    // Errores de validación con el mismo contrato { success, message, data } en /api/v1.
    .AddStandardValidationResponse();

// Manejo global de errores: log + respuesta estándar + código HTTP correcto (200 en rutas legacy).
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddHealthChecks();

builder.Services.AddMemoryCache();

// SignalR
builder.Services.AddSignalR();

//Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<IFirebaseAuthenticationService, FirebaseAuthenticationService>();
// File storage
builder.Services.AddSingleton<BocciaCoaching.Services.IFileStorageService, BocciaCoaching.Services.DiskFileStorageService>();

// Servicio de email: usa la API REST de Hostinger Mail (token Bearer) si UseApi = true,
// de lo contrario usa SMTP mediante MailKit (EmailService).
var emailSettingsSection = builder.Configuration.GetSection("EmailSettings").Get<EmailSettings>() ?? new EmailSettings();
if (emailSettingsSection.UseApi)
{
    builder.Services.AddHttpClient<IEmailService, BocciaCoaching.Services.HostingerMailService>((sp, client) =>
    {
        var settings = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<EmailSettings>>().Value;
        client.BaseAddress = new Uri(settings.ApiBaseUrl);
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", settings.ApiToken);
        client.DefaultRequestHeaders.Accept.Add(
            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
    });
}
else
{
    builder.Services.AddScoped<IEmailService, EmailService>();
}
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IAssessStrengthService, AssessStrengthService>();
builder.Services.AddScoped<IAssessDirectionService, AssessDirectionService>();
builder.Services.AddScoped<BocciaCoaching.Services.Interfaces.INotificationService, BocciaCoaching.Services.NotificationService>();
builder.Services.AddScoped<IStatisticsService, StatisticsService>();

// Subscription Services
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<IStripePaymentService, StripePaymentServiceSimplified>();

// SAREMAS+ Services
builder.Services.AddScoped<IAssessSaremasService, AssessSaremasService>();

// Daily Wellness Services
builder.Services.AddScoped<IWellnessService, WellnessService>();

// Macrocycle Services
builder.Services.AddScoped<IMacrocycleService, MacrocycleService>();

// Training Session Services
builder.Services.AddScoped<ITrainingSessionService, TrainingSessionService>();

// MicrocycleType Services
builder.Services.AddScoped<IMicrocycleTypeService, MicrocycleTypeService>();

// Rol Services - Roles
builder.Services.AddScoped<IRolService, RolService>();

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

// Assess Direction - Prueba de control de dirección
builder.Services.AddScoped<IAssessDirectionRepository, AssessDirectionRepository>();
builder.Services.AddScoped<IValidationsAssetsDirection, ValidationsDirectionRepository>();

// Statistic - Estadisticas
builder.Services.AddScoped<IStatisticAssessStrength, StatisticAssessStrength>();

// NotificationType - Tipos de notificación
builder.Services.AddScoped<INotificationTypeRepository, NotificationTypeRepository>();

// Subscription - Suscripciones
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();

// Assess SAREMAS+ - Prueba SAREMAS+
builder.Services.AddScoped<IAssessSaremasRepository, AssessSaremasRepository>();
builder.Services.AddScoped<IValidationsAssessSaremas, ValidationsAssessSaremasRepository>();

// Daily Wellness - Test diario de bienestar
builder.Services.AddScoped<IWellnessRepository, WellnessRepository>();

// Macrocycle - Macrociclos
builder.Services.AddScoped<IMacrocycleRepository, MacrocycleRepository>();

// Training Session - Sesiones de entrenamiento
builder.Services.AddScoped<ITrainingSessionRepository, TrainingSessionRepository>();

// MicrocycleType - Tipos de microciclo
builder.Services.AddScoped<IMicrocycleTypeRepository, MicrocycleTypeRepository>();

// Rol - Roles
builder.Services.AddScoped<IRolRepository, RolRepository>();

// Autenticación: JWT (access token corto) + refresh tokens rotativos guardados como hash.
// Todas las APIs exigen token salvo las marcadas con [AllowAnonymous].
builder.Services.AddJwtAuthentication(builder.Configuration, builder.Environment);
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BocciaCoaching API",
        Version = "v1",
        Description = "Rutas REST en /api/v1. Las rutas antiguas (/api/Team, /api/Wellness, ...) aparecen como obsoletas."
    });

    // Incluye los comentarios /// de controladores y DTOs en la documentación.
    var xmlFile = Path.Combine(AppContext.BaseDirectory, $"{typeof(Program).Assembly.GetName().Name}.xml");
    if (System.IO.File.Exists(xmlFile))
        c.IncludeXmlComments(xmlFile);

    // Botón "Authorize" en Swagger: pega el accessToken (sin el prefijo "Bearer").
    var bearerScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Access token obtenido en POST /api/v1/auth/login",
        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
    };
    c.AddSecurityDefinition("Bearer", bearerScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement { [bearerScheme] = Array.Empty<string>() });
});



var app = builder.Build();

// IP real del cliente detrás del proxy (necesaria para el rate limiting por IP).
app.UseForwardedHeaders();

// Debe ir primero para capturar las excepciones de todo el pipeline.
app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Servir archivos estáticos (wwwroot)
app.UseStaticFiles();

app.UseRouting();

// Use CORS - permitir credenciales para SignalR
app.UseCors("AllowSpecificOrigins");

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health").AllowAnonymous();
app.MapControllers();

app.Run();
