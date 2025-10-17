using BocciaCoaching.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace BocciaCoaching.Data
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<UserRol> UserRols { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<AthletesToEvaluated> AthletesToEvaluated { get; set; }
        public DbSet<EvaluationDetailStrength> EvaluationDetailStrengths { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamUser> TeamsUsers { get; set; }
        public DbSet<AssessStrength> AssessStrengths { get; set; }
        public DbSet<ModuleError> ModuleError { get; set; }
        public DbSet<LogError> LogError { get; set; }
        public DbSet<NotificationType> NotificationType { get; set; }
        public DbSet<NotificationMessage> NotificationMessage { get; set; }

        public DbSet<Event> Event { get; set; }
        public DbSet<LevelEvent> LevelEvent { get; set; }
        public DbSet<Achievement> Achievement { get; set; }

        #region Required
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
        #endregion

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
