using BocciaCoaching.Data;
using BocciaCoaching.Repositories;
using BocciaCoaching.Repositories.AssesstStrength;
using BocciaCoaching.Repositories.Interfaces;
using BocciaCoaching.Repositories.Interfaces.IAssesstStrength;
using BocciaCoaching.Repositories.Interfaces.ITeams;
using BocciaCoaching.Repositories.Teams;
using BocciaCoaching.Services;
using BocciaCoaching.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.WithOrigins("https://bocciacoaching.com", "http://localhost:4200", "https://www.bocciacoaching.com")
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

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddMemoryCache();

//Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IAssessStrengthService, AssessStrengthService>();

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



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();
app.UseRouting(); 
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
