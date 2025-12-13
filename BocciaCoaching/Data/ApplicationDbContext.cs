﻿﻿using BocciaCoaching.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BocciaCoaching.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {

        public DbSet<User> Users { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<UserRol> UserRoles { get; set; }
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
        public DbSet<StrengthStatistics> StrengthStatistics { get; set; }

        // Chat entities
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
