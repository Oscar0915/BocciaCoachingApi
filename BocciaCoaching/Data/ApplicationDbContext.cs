﻿using BocciaCoaching.Models.Entities;
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

        // Direction Assessment entities
        public DbSet<AssessDirection> AssessDirections { get; set; }
        public DbSet<EvaluationDetailDirection> EvaluationDetailDirections { get; set; }
        public DbSet<AthletesToEvaluatedDirection> AthletesToEvaluatedDirection { get; set; }
        public DbSet<DirectionStatistics> DirectionStatistics { get; set; }

        // Subscription entities
        public DbSet<SubscriptionType> SubscriptionTypes { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Payment> Payments { get; set; }

        // SAREMAS+ Assessment entities
        public DbSet<SaremasEvaluation> SaremasEvaluations { get; set; }
        public DbSet<SaremasAthleteEvaluation> SaremasAthleteEvaluations { get; set; }
        public DbSet<SaremasThrow> SaremasThrows { get; set; }

        // Macrocycle entities
        public DbSet<Macrocycle> Macrocycles { get; set; }
        public DbSet<MacrocycleEvent> MacrocycleEvents { get; set; }
        public DbSet<MacrocyclePeriod> MacrocyclePeriods { get; set; }
        public DbSet<Mesocycle> Mesocycles { get; set; }
        public DbSet<Microcycle> Microcycles { get; set; }

        // Training Session entities
        public DbSet<TrainingSession> TrainingSessions { get; set; }
        public DbSet<SessionPart> SessionParts { get; set; }
        public DbSet<SessionSection> SessionSections { get; set; }

        // Microcycle Type entities
        public DbSet<MicrocycleType> MicrocycleTypes { get; set; }
        public DbSet<MicrocycleTypeDayDefault> MicrocycleTypeDayDefaults { get; set; }
        public DbSet<CoachMicrocycleTypeDay> CoachMicrocycleTypeDays { get; set; }

        // Microcycle Day (días/porcentajes por instancia de microciclo)
        public DbSet<MicrocycleDay> MicrocycleDays { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Microcycle → MicrocycleType (nullable, SetNull on delete del tipo)
            modelBuilder.Entity<Microcycle>()
                .HasOne(m => m.MicrocycleType)
                .WithMany()
                .HasForeignKey(m => m.MicrocycleTypeId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);

            // MicrocycleDay → Microcycle (Cascade: al borrar el microciclo se borran sus días)
            modelBuilder.Entity<MicrocycleDay>()
                .HasOne(d => d.Microcycle)
                .WithMany(m => m.Days)
                .HasForeignKey(d => d.MicrocycleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
