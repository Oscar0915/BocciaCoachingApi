using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BocciaCoaching.Data
{
    /// <summary>
    /// ES: Factory usada por las herramientas de EF Core en tiempo de diseño
    /// (por ejemplo, "dotnet ef migrations add" / "database update").
    /// Lee la cadena de conexión de appsettings.json y usa una versión de
    /// servidor fija (evita el AutoDetect adicional).
    /// EN: Design-time factory for EF Core tooling. Reads the connection string
    /// from appsettings.json and uses a fixed server version.
    /// </summary>
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? "server=localhost;database=bocciacoaching;user=root;password=root;";

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            optionsBuilder.UseMySql(
                connectionString,
                new MariaDbServerVersion(new Version(10, 11, 0))
            );

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}

