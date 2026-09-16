using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BocciaCoaching.Migrations
{
    /// <inheritdoc />
    public partial class InitialGuidMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LevelEvent",
                columns: table => new
                {
                    LevelEventId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    NameLevel = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelEvent", x => x.LevelEventId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MicrocycleType",
                columns: table => new
                {
                    MicrocycleTypeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ShortCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MicrocycleType", x => x.MicrocycleTypeId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ModuleError",
                columns: table => new
                {
                    ModuleErrorId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleError", x => x.ModuleErrorId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NotificationType",
                columns: table => new
                {
                    NotificationTypeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationType", x => x.NotificationTypeId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Rol",
                columns: table => new
                {
                    RolId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.RolId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SubscriptionType",
                columns: table => new
                {
                    SubscriptionTypeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PriceInCents = table.Column<int>(type: "int", nullable: false),
                    AnnualPriceInCents = table.Column<int>(type: "int", nullable: true),
                    StripeProductId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StripeMonthlyPriceId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StripeAnnualPriceId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Features = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TeamLimit = table.Column<int>(type: "int", nullable: true),
                    AthleteLimit = table.Column<int>(type: "int", nullable: true),
                    MonthlyEvaluationLimit = table.Column<int>(type: "int", nullable: true),
                    HasAdvancedStatistics = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    HasPremiumChat = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDefault = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionType", x => x.SubscriptionTypeId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Dni = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FirstName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Country = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Image = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Category = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Seniority = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LogError",
                columns: table => new
                {
                    LogErrorId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ModuleErrorId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Location = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ErrorMessage = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogError", x => x.LogErrorId);
                    table.ForeignKey(
                        name: "FK_LogError_ModuleError_ModuleErrorId",
                        column: x => x.ModuleErrorId,
                        principalTable: "ModuleError",
                        principalColumn: "ModuleErrorId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CoachMicrocycleTypeDistribution",
                columns: table => new
                {
                    CoachMicrocycleTypeDistributionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CoachId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MicrocycleTypeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FisicaGeneral = table.Column<double>(type: "double", nullable: false),
                    FisicaEspecial = table.Column<double>(type: "double", nullable: false),
                    Tecnica = table.Column<double>(type: "double", nullable: false),
                    Tactica = table.Column<double>(type: "double", nullable: false),
                    Teorica = table.Column<double>(type: "double", nullable: false),
                    Psicologica = table.Column<double>(type: "double", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoachMicrocycleTypeDistribution", x => x.CoachMicrocycleTypeDistributionId);
                    table.ForeignKey(
                        name: "FK_CoachMicrocycleTypeDistribution_MicrocycleType_MicrocycleTyp~",
                        column: x => x.MicrocycleTypeId,
                        principalTable: "MicrocycleType",
                        principalColumn: "MicrocycleTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoachMicrocycleTypeDistribution_User_CoachId",
                        column: x => x.CoachId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    EventId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    NameEvent = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DescriptionEvent = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Location = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Country = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    LevelEventId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_Event_LevelEvent_LevelEventId",
                        column: x => x.LevelEventId,
                        principalTable: "LevelEvent",
                        principalColumn: "LevelEventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Event_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MicrocycleTypeDayDefault",
                columns: table => new
                {
                    MicrocycleTypeDayDefaultId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MicrocycleTypeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CoachId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    DayOfWeek = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ThrowPercentage = table.Column<double>(type: "double", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MicrocycleTypeDayDefault", x => x.MicrocycleTypeDayDefaultId);
                    table.ForeignKey(
                        name: "FK_MicrocycleTypeDayDefault_MicrocycleType_MicrocycleTypeId",
                        column: x => x.MicrocycleTypeId,
                        principalTable: "MicrocycleType",
                        principalColumn: "MicrocycleTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MicrocycleTypeDayDefault_User_CoachId",
                        column: x => x.CoachId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NotificationMessage",
                columns: table => new
                {
                    NotificationMessageId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Message = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Image = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SenderId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ReceiverId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    NotificationTypeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ReferenceId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationMessage", x => x.NotificationMessageId);
                    table.ForeignKey(
                        name: "FK_NotificationMessage_NotificationType_NotificationTypeId",
                        column: x => x.NotificationTypeId,
                        principalTable: "NotificationType",
                        principalColumn: "NotificationTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationMessage_User_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationMessage_User_SenderId",
                        column: x => x.SenderId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Session",
                columns: table => new
                {
                    SessionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Session", x => x.SessionId);
                    table.ForeignKey(
                        name: "FK_Session_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    SubscriptionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SubscriptionTypeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StripeSubscriptionId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StripeCustomerId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    NextRenewalDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CanceledAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsTrial = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TrialEndDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsAnnual = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PricePaidInCents = table.Column<int>(type: "int", nullable: true),
                    Currency = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Notes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.SubscriptionId);
                    table.ForeignKey(
                        name: "FK_Subscription_SubscriptionType_SubscriptionTypeId",
                        column: x => x.SubscriptionTypeId,
                        principalTable: "SubscriptionType",
                        principalColumn: "SubscriptionTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscription_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    TeamId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    NameTeam = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CoachId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Image = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Bc1 = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Bc2 = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Bc3 = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Bc4 = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Pairs = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Teams = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Country = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Region = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.TeamId);
                    table.ForeignKey(
                        name: "FK_Team_User_CoachId",
                        column: x => x.CoachId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserRol",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RolId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DateCreation = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRol", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRol_Rol_RolId",
                        column: x => x.RolId,
                        principalTable: "Rol",
                        principalColumn: "RolId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRol_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Achievement",
                columns: table => new
                {
                    AchievementId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Ranked = table.Column<int>(type: "int", nullable: false),
                    EventId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Achievement", x => x.AchievementId);
                    table.ForeignKey(
                        name: "FK_Achievement_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    PaymentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SubscriptionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StripePaymentIntentId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StripeInvoiceId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AmountInCents = table.Column<int>(type: "int", nullable: false),
                    Currency = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PaymentMethod = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PaymentDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RefundedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RefundedAmountInCents = table.Column<int>(type: "int", nullable: true),
                    FailureReason = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FailureCode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Metadata = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReceiptNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payment_Subscription_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscription",
                        principalColumn: "SubscriptionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payment_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AssessDirection",
                columns: table => new
                {
                    AssessDirectionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EvaluationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    State = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TeamId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CoachId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessDirection", x => x.AssessDirectionId);
                    table.ForeignKey(
                        name: "FK_AssessDirection_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssessDirection_User_CoachId",
                        column: x => x.CoachId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AssessStrength",
                columns: table => new
                {
                    AssessStrengthId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EvaluationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    State = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TeamId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CoachId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessStrength", x => x.AssessStrengthId);
                    table.ForeignKey(
                        name: "FK_AssessStrength_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssessStrength_User_CoachId",
                        column: x => x.CoachId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DailyWellness",
                columns: table => new
                {
                    DailyWellnessId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AthleteId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TeamId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    AssessmentDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Sleep = table.Column<int>(type: "int", nullable: false),
                    Stress = table.Column<int>(type: "int", nullable: false),
                    Fatigue = table.Column<int>(type: "int", nullable: false),
                    MusclePain = table.Column<int>(type: "int", nullable: false),
                    TotalScore = table.Column<int>(type: "int", nullable: false),
                    AverageScore = table.Column<double>(type: "double", nullable: false),
                    Observations = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyWellness", x => x.DailyWellnessId);
                    table.ForeignKey(
                        name: "FK_DailyWellness_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DailyWellness_User_AthleteId",
                        column: x => x.AthleteId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Macrocycle",
                columns: table => new
                {
                    MacrocycleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AthleteId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AthleteName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Notes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CoachId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TeamId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Macrocycle", x => x.MacrocycleId);
                    table.ForeignKey(
                        name: "FK_Macrocycle_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Macrocycle_User_AthleteId",
                        column: x => x.AthleteId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Macrocycle_User_CoachId",
                        column: x => x.CoachId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SaremasEvaluation",
                columns: table => new
                {
                    SaremasEvaluationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TeamId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CoachId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EvaluationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    State = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TotalScore = table.Column<int>(type: "int", nullable: true),
                    AverageScore = table.Column<double>(type: "double", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaremasEvaluation", x => x.SaremasEvaluationId);
                    table.ForeignKey(
                        name: "FK_SaremasEvaluation_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaremasEvaluation_User_CoachId",
                        column: x => x.CoachId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeamUser",
                columns: table => new
                {
                    IdTeamUser = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TeamId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DateCreation = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamUser", x => x.IdTeamUser);
                    table.ForeignKey(
                        name: "FK_TeamUser_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AthletesToEvaluatedDirection",
                columns: table => new
                {
                    AthletesToEvaluatedDirectionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CoachId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AthleteId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AssessDirectionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AthletesToEvaluatedDirection", x => x.AthletesToEvaluatedDirectionId);
                    table.ForeignKey(
                        name: "FK_AthletesToEvaluatedDirection_AssessDirection_AssessDirection~",
                        column: x => x.AssessDirectionId,
                        principalTable: "AssessDirection",
                        principalColumn: "AssessDirectionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AthletesToEvaluatedDirection_User_AthleteId",
                        column: x => x.AthleteId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AthletesToEvaluatedDirection_User_CoachId",
                        column: x => x.CoachId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DirectionStatistics",
                columns: table => new
                {
                    DirectionStatisticsId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EffectivenessPercentage = table.Column<double>(type: "double", nullable: false),
                    AccuracyPercentage = table.Column<double>(type: "double", nullable: false),
                    EffectiveThrow = table.Column<int>(type: "int", nullable: false),
                    FailedThrow = table.Column<int>(type: "int", nullable: false),
                    ShortThrow = table.Column<int>(type: "int", nullable: false),
                    MediumThrow = table.Column<int>(type: "int", nullable: false),
                    LongThrow = table.Column<double>(type: "double", nullable: false),
                    ShortEffectivenessPercentage = table.Column<double>(type: "double", nullable: false),
                    MediumEffectivenessPercentage = table.Column<double>(type: "double", nullable: false),
                    LongEffectivenessPercentage = table.Column<double>(type: "double", nullable: false),
                    ShortThrowAccuracy = table.Column<int>(type: "int", nullable: false),
                    MediumThrowAccuracy = table.Column<int>(type: "int", nullable: false),
                    LongThrowAccuracy = table.Column<int>(type: "int", nullable: false),
                    ShortAccuracyPercentage = table.Column<double>(type: "double", nullable: false),
                    MediumAccuracyPercentage = table.Column<double>(type: "double", nullable: false),
                    LongAccuracyPercentage = table.Column<double>(type: "double", nullable: false),
                    TotalDeviatedRight = table.Column<int>(type: "int", nullable: false),
                    TotalDeviatedLeft = table.Column<int>(type: "int", nullable: false),
                    DeviatedRightPercentage = table.Column<double>(type: "double", nullable: false),
                    DeviatedLeftPercentage = table.Column<double>(type: "double", nullable: false),
                    ShortDeviatedRight = table.Column<int>(type: "int", nullable: false),
                    ShortDeviatedLeft = table.Column<int>(type: "int", nullable: false),
                    MediumDeviatedRight = table.Column<int>(type: "int", nullable: false),
                    MediumDeviatedLeft = table.Column<int>(type: "int", nullable: false),
                    LongDeviatedRight = table.Column<int>(type: "int", nullable: false),
                    LongDeviatedLeft = table.Column<int>(type: "int", nullable: false),
                    AssessDirectionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AthleteId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectionStatistics", x => x.DirectionStatisticsId);
                    table.ForeignKey(
                        name: "FK_DirectionStatistics_AssessDirection_AssessDirectionId",
                        column: x => x.AssessDirectionId,
                        principalTable: "AssessDirection",
                        principalColumn: "AssessDirectionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DirectionStatistics_User_AthleteId",
                        column: x => x.AthleteId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EvaluationDetailDirection",
                columns: table => new
                {
                    EvaluationDetailDirectionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    BoxNumber = table.Column<int>(type: "int", nullable: false),
                    ThrowOrder = table.Column<int>(type: "int", nullable: false),
                    TargetDistance = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    ScoreObtained = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Observations = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AthleteId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AssessDirectionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CoordinateX = table.Column<double>(type: "double", nullable: false),
                    CoordinateY = table.Column<double>(type: "double", nullable: false),
                    DeviatedRight = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeviatedLeft = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsStrength = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsCadence = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDirection = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsTrajectory = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluationDetailDirection", x => x.EvaluationDetailDirectionId);
                    table.ForeignKey(
                        name: "FK_EvaluationDetailDirection_AssessDirection_AssessDirectionId",
                        column: x => x.AssessDirectionId,
                        principalTable: "AssessDirection",
                        principalColumn: "AssessDirectionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EvaluationDetailDirection_User_AthleteId",
                        column: x => x.AthleteId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AthletesToEvaluated",
                columns: table => new
                {
                    AthletesToEvaluatedId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CoachId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AthleteId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AssessStrengthId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AthletesToEvaluated", x => x.AthletesToEvaluatedId);
                    table.ForeignKey(
                        name: "FK_AthletesToEvaluated_AssessStrength_AssessStrengthId",
                        column: x => x.AssessStrengthId,
                        principalTable: "AssessStrength",
                        principalColumn: "AssessStrengthId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AthletesToEvaluated_User_AthleteId",
                        column: x => x.AthleteId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AthletesToEvaluated_User_CoachId",
                        column: x => x.CoachId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EvaluationDetailStrength",
                columns: table => new
                {
                    EvaluationDetailStrengthId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    BoxNumber = table.Column<int>(type: "int", nullable: false),
                    ThrowOrder = table.Column<int>(type: "int", nullable: false),
                    TargetDistance = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    ScoreObtained = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Observations = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AthleteId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AssessStrengthId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CoordinateX = table.Column<double>(type: "double", nullable: false),
                    CoordinateY = table.Column<double>(type: "double", nullable: false),
                    IsStrength = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsCadence = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDirection = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsTrajectory = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluationDetailStrength", x => x.EvaluationDetailStrengthId);
                    table.ForeignKey(
                        name: "FK_EvaluationDetailStrength_AssessStrength_AssessStrengthId",
                        column: x => x.AssessStrengthId,
                        principalTable: "AssessStrength",
                        principalColumn: "AssessStrengthId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EvaluationDetailStrength_User_AthleteId",
                        column: x => x.AthleteId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StrengthStatistics",
                columns: table => new
                {
                    StrengthStatisticsId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EffectivenessPercentage = table.Column<double>(type: "double", nullable: false),
                    AccuracyPercentage = table.Column<double>(type: "double", nullable: false),
                    EffectiveThrow = table.Column<int>(type: "int", nullable: false),
                    FailedThrow = table.Column<int>(type: "int", nullable: false),
                    ShortThrow = table.Column<int>(type: "int", nullable: false),
                    MediumThrow = table.Column<int>(type: "int", nullable: false),
                    LongThrow = table.Column<double>(type: "double", nullable: false),
                    ShortEffectivenessPercentage = table.Column<double>(type: "double", nullable: false),
                    MediumEffectivenessPercentage = table.Column<double>(type: "double", nullable: false),
                    LongEffectivenessPercentage = table.Column<double>(type: "double", nullable: false),
                    ShortThrowAccuracy = table.Column<int>(type: "int", nullable: false),
                    MediumThrowAccuracy = table.Column<int>(type: "int", nullable: false),
                    LongThrowAccuracy = table.Column<int>(type: "int", nullable: false),
                    ShortAccuracyPercentage = table.Column<double>(type: "double", nullable: false),
                    MediumAccuracyPercentage = table.Column<double>(type: "double", nullable: false),
                    LongAccuracyPercentage = table.Column<double>(type: "double", nullable: false),
                    AssessStrengthId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AthleteId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StrengthStatistics", x => x.StrengthStatisticsId);
                    table.ForeignKey(
                        name: "FK_StrengthStatistics_AssessStrength_AssessStrengthId",
                        column: x => x.AssessStrengthId,
                        principalTable: "AssessStrength",
                        principalColumn: "AssessStrengthId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StrengthStatistics_User_AthleteId",
                        column: x => x.AthleteId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MacrocycleEvent",
                columns: table => new
                {
                    MacrocycleEventId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MacrocycleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Level = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Location = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Notes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MacrocycleEvent", x => x.MacrocycleEventId);
                    table.ForeignKey(
                        name: "FK_MacrocycleEvent_Macrocycle_MacrocycleId",
                        column: x => x.MacrocycleId,
                        principalTable: "Macrocycle",
                        principalColumn: "MacrocycleId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MacrocyclePeriod",
                columns: table => new
                {
                    MacrocyclePeriodId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MacrocycleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StageCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Weeks = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MacrocyclePeriod", x => x.MacrocyclePeriodId);
                    table.ForeignKey(
                        name: "FK_MacrocyclePeriod_Macrocycle_MacrocycleId",
                        column: x => x.MacrocycleId,
                        principalTable: "Macrocycle",
                        principalColumn: "MacrocycleId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Mesocycle",
                columns: table => new
                {
                    MesocycleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MacrocycleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Weeks = table.Column<int>(type: "int", nullable: false),
                    Objective = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mesocycle", x => x.MesocycleId);
                    table.ForeignKey(
                        name: "FK_Mesocycle_Macrocycle_MacrocycleId",
                        column: x => x.MacrocycleId,
                        principalTable: "Macrocycle",
                        principalColumn: "MacrocycleId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Microcycle",
                columns: table => new
                {
                    MicrocycleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MacrocycleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MicrocycleTypeId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    WeekNumber = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PeriodName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MesocycleName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HasPeakPerformance = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LoadPercentage = table.Column<double>(type: "double", nullable: false),
                    TrainingDistribution = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Microcycle", x => x.MicrocycleId);
                    table.ForeignKey(
                        name: "FK_Microcycle_Macrocycle_MacrocycleId",
                        column: x => x.MacrocycleId,
                        principalTable: "Macrocycle",
                        principalColumn: "MacrocycleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Microcycle_MicrocycleType_MicrocycleTypeId",
                        column: x => x.MicrocycleTypeId,
                        principalTable: "MicrocycleType",
                        principalColumn: "MicrocycleTypeId",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SaremasAthleteEvaluation",
                columns: table => new
                {
                    SaremasAthleteEvaluationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SaremasEvalId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AthleteId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AthleteName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaremasAthleteEvaluation", x => x.SaremasAthleteEvaluationId);
                    table.ForeignKey(
                        name: "FK_SaremasAthleteEvaluation_SaremasEvaluation_SaremasEvalId",
                        column: x => x.SaremasEvalId,
                        principalTable: "SaremasEvaluation",
                        principalColumn: "SaremasEvaluationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaremasAthleteEvaluation_User_AthleteId",
                        column: x => x.AthleteId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SaremasThrow",
                columns: table => new
                {
                    SaremasThrowId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SaremasEvalId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AthleteId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ThrowNumber = table.Column<int>(type: "int", nullable: false),
                    Diagonal = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TechnicalComponent = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ScoreObtained = table.Column<int>(type: "int", nullable: false),
                    Observations = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FailureTags = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    WhiteBallX = table.Column<double>(type: "double", nullable: true),
                    WhiteBallY = table.Column<double>(type: "double", nullable: true),
                    ColorBallX = table.Column<double>(type: "double", nullable: true),
                    ColorBallY = table.Column<double>(type: "double", nullable: true),
                    EstimatedDistance = table.Column<double>(type: "double", nullable: true),
                    LaunchPointX = table.Column<double>(type: "double", nullable: true),
                    LaunchPointY = table.Column<double>(type: "double", nullable: true),
                    DistanceToLaunchPoint = table.Column<double>(type: "double", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaremasThrow", x => x.SaremasThrowId);
                    table.ForeignKey(
                        name: "FK_SaremasThrow_SaremasEvaluation_SaremasEvalId",
                        column: x => x.SaremasEvalId,
                        principalTable: "SaremasEvaluation",
                        principalColumn: "SaremasEvaluationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaremasThrow_User_AthleteId",
                        column: x => x.AthleteId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MicrocycleDay",
                columns: table => new
                {
                    MicrocycleDayId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MicrocycleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DayOfWeek = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ThrowPercentage = table.Column<double>(type: "double", nullable: false),
                    IsCustom = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MicrocycleDay", x => x.MicrocycleDayId);
                    table.ForeignKey(
                        name: "FK_MicrocycleDay_Microcycle_MicrocycleId",
                        column: x => x.MicrocycleId,
                        principalTable: "Microcycle",
                        principalColumn: "MicrocycleId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TrainingSession",
                columns: table => new
                {
                    TrainingSessionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MicrocycleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DayOfWeek = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    PhotoEvidence1 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhotoEvidence2 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhotoEvidence3 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhotoEvidence4 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ThrowPercentage = table.Column<double>(type: "double", nullable: false),
                    TotalThrowsBase = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingSession", x => x.TrainingSessionId);
                    table.ForeignKey(
                        name: "FK_TrainingSession_Microcycle_MicrocycleId",
                        column: x => x.MicrocycleId,
                        principalTable: "Microcycle",
                        principalColumn: "MicrocycleId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SessionPart",
                columns: table => new
                {
                    SessionPartId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TrainingSessionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionPart", x => x.SessionPartId);
                    table.ForeignKey(
                        name: "FK_SessionPart_TrainingSession_TrainingSessionId",
                        column: x => x.TrainingSessionId,
                        principalTable: "TrainingSession",
                        principalColumn: "TrainingSessionId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SessionSection",
                columns: table => new
                {
                    SessionSectionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SessionPartId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NumberOfThrows = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsOwnDiagonal = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Observation = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionSection", x => x.SessionSectionId);
                    table.ForeignKey(
                        name: "FK_SessionSection_SessionPart_SessionPartId",
                        column: x => x.SessionPartId,
                        principalTable: "SessionPart",
                        principalColumn: "SessionPartId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "ModuleError",
                columns: new[] { "ModuleErrorId", "Description", "Name" },
                values: new object[] { new Guid("00000000-0000-0000-0000-0000000000a1"), "Módulo general", "General" });

            migrationBuilder.InsertData(
                table: "NotificationType",
                columns: new[] { "NotificationTypeId", "CreatedAt", "Description", "Name", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-0000000000b1"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Notificación general", "General", true, null },
                    { new Guid("00000000-0000-0000-0000-0000000000b2"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Invitación para unirse a un equipo", "Invitación a equipo", true, null }
                });

            migrationBuilder.InsertData(
                table: "Rol",
                columns: new[] { "RolId", "Description" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), "Admin" },
                    { new Guid("00000000-0000-0000-0000-000000000002"), "Coach" },
                    { new Guid("00000000-0000-0000-0000-000000000003"), "Atleta" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Achievement_EventId",
                table: "Achievement",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessDirection_CoachId",
                table: "AssessDirection",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessDirection_TeamId",
                table: "AssessDirection",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessStrength_CoachId",
                table: "AssessStrength",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessStrength_TeamId",
                table: "AssessStrength",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_AthletesToEvaluated_AssessStrengthId",
                table: "AthletesToEvaluated",
                column: "AssessStrengthId");

            migrationBuilder.CreateIndex(
                name: "IX_AthletesToEvaluated_AthleteId",
                table: "AthletesToEvaluated",
                column: "AthleteId");

            migrationBuilder.CreateIndex(
                name: "IX_AthletesToEvaluated_CoachId",
                table: "AthletesToEvaluated",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_AthletesToEvaluatedDirection_AssessDirectionId",
                table: "AthletesToEvaluatedDirection",
                column: "AssessDirectionId");

            migrationBuilder.CreateIndex(
                name: "IX_AthletesToEvaluatedDirection_AthleteId",
                table: "AthletesToEvaluatedDirection",
                column: "AthleteId");

            migrationBuilder.CreateIndex(
                name: "IX_AthletesToEvaluatedDirection_CoachId",
                table: "AthletesToEvaluatedDirection",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_CoachMicrocycleTypeDistribution_CoachId",
                table: "CoachMicrocycleTypeDistribution",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_CoachMicrocycleTypeDistribution_MicrocycleTypeId",
                table: "CoachMicrocycleTypeDistribution",
                column: "MicrocycleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyWellness_AthleteId_AssessmentDate",
                table: "DailyWellness",
                columns: new[] { "AthleteId", "AssessmentDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DailyWellness_TeamId",
                table: "DailyWellness",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectionStatistics_AssessDirectionId",
                table: "DirectionStatistics",
                column: "AssessDirectionId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectionStatistics_AthleteId",
                table: "DirectionStatistics",
                column: "AthleteId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationDetailDirection_AssessDirectionId",
                table: "EvaluationDetailDirection",
                column: "AssessDirectionId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationDetailDirection_AthleteId",
                table: "EvaluationDetailDirection",
                column: "AthleteId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationDetailStrength_AssessStrengthId",
                table: "EvaluationDetailStrength",
                column: "AssessStrengthId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationDetailStrength_AthleteId",
                table: "EvaluationDetailStrength",
                column: "AthleteId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_LevelEventId",
                table: "Event",
                column: "LevelEventId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_UserId",
                table: "Event",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LogError_ModuleErrorId",
                table: "LogError",
                column: "ModuleErrorId");

            migrationBuilder.CreateIndex(
                name: "IX_Macrocycle_AthleteId",
                table: "Macrocycle",
                column: "AthleteId");

            migrationBuilder.CreateIndex(
                name: "IX_Macrocycle_CoachId",
                table: "Macrocycle",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_Macrocycle_TeamId",
                table: "Macrocycle",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_MacrocycleEvent_MacrocycleId",
                table: "MacrocycleEvent",
                column: "MacrocycleId");

            migrationBuilder.CreateIndex(
                name: "IX_MacrocyclePeriod_MacrocycleId",
                table: "MacrocyclePeriod",
                column: "MacrocycleId");

            migrationBuilder.CreateIndex(
                name: "IX_Mesocycle_MacrocycleId",
                table: "Mesocycle",
                column: "MacrocycleId");

            migrationBuilder.CreateIndex(
                name: "IX_Microcycle_MacrocycleId",
                table: "Microcycle",
                column: "MacrocycleId");

            migrationBuilder.CreateIndex(
                name: "IX_Microcycle_MicrocycleTypeId",
                table: "Microcycle",
                column: "MicrocycleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MicrocycleDay_MicrocycleId",
                table: "MicrocycleDay",
                column: "MicrocycleId");

            migrationBuilder.CreateIndex(
                name: "IX_MicrocycleTypeDayDefault_CoachId",
                table: "MicrocycleTypeDayDefault",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_MicrocycleTypeDayDefault_MicrocycleTypeId",
                table: "MicrocycleTypeDayDefault",
                column: "MicrocycleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationMessage_NotificationTypeId",
                table: "NotificationMessage",
                column: "NotificationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationMessage_ReceiverId",
                table: "NotificationMessage",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationMessage_SenderId",
                table: "NotificationMessage",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_SubscriptionId",
                table: "Payment",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_UserId",
                table: "Payment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SaremasAthleteEvaluation_AthleteId",
                table: "SaremasAthleteEvaluation",
                column: "AthleteId");

            migrationBuilder.CreateIndex(
                name: "IX_SaremasAthleteEvaluation_SaremasEvalId",
                table: "SaremasAthleteEvaluation",
                column: "SaremasEvalId");

            migrationBuilder.CreateIndex(
                name: "IX_SaremasEvaluation_CoachId",
                table: "SaremasEvaluation",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_SaremasEvaluation_TeamId",
                table: "SaremasEvaluation",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_SaremasThrow_AthleteId",
                table: "SaremasThrow",
                column: "AthleteId");

            migrationBuilder.CreateIndex(
                name: "IX_SaremasThrow_SaremasEvalId",
                table: "SaremasThrow",
                column: "SaremasEvalId");

            migrationBuilder.CreateIndex(
                name: "IX_Session_UserId",
                table: "Session",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SessionPart_TrainingSessionId",
                table: "SessionPart",
                column: "TrainingSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionSection_SessionPartId",
                table: "SessionSection",
                column: "SessionPartId");

            migrationBuilder.CreateIndex(
                name: "IX_StrengthStatistics_AssessStrengthId",
                table: "StrengthStatistics",
                column: "AssessStrengthId");

            migrationBuilder.CreateIndex(
                name: "IX_StrengthStatistics_AthleteId",
                table: "StrengthStatistics",
                column: "AthleteId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_SubscriptionTypeId",
                table: "Subscription",
                column: "SubscriptionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_UserId",
                table: "Subscription",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_CoachId",
                table: "Team",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamUser_TeamId",
                table: "TeamUser",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamUser_UserId",
                table: "TeamUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingSession_MicrocycleId",
                table: "TrainingSession",
                column: "MicrocycleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRol_RolId",
                table: "UserRol",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRol_UserId",
                table: "UserRol",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Achievement");

            migrationBuilder.DropTable(
                name: "AthletesToEvaluated");

            migrationBuilder.DropTable(
                name: "AthletesToEvaluatedDirection");

            migrationBuilder.DropTable(
                name: "CoachMicrocycleTypeDistribution");

            migrationBuilder.DropTable(
                name: "DailyWellness");

            migrationBuilder.DropTable(
                name: "DirectionStatistics");

            migrationBuilder.DropTable(
                name: "EvaluationDetailDirection");

            migrationBuilder.DropTable(
                name: "EvaluationDetailStrength");

            migrationBuilder.DropTable(
                name: "LogError");

            migrationBuilder.DropTable(
                name: "MacrocycleEvent");

            migrationBuilder.DropTable(
                name: "MacrocyclePeriod");

            migrationBuilder.DropTable(
                name: "Mesocycle");

            migrationBuilder.DropTable(
                name: "MicrocycleDay");

            migrationBuilder.DropTable(
                name: "MicrocycleTypeDayDefault");

            migrationBuilder.DropTable(
                name: "NotificationMessage");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "SaremasAthleteEvaluation");

            migrationBuilder.DropTable(
                name: "SaremasThrow");

            migrationBuilder.DropTable(
                name: "Session");

            migrationBuilder.DropTable(
                name: "SessionSection");

            migrationBuilder.DropTable(
                name: "StrengthStatistics");

            migrationBuilder.DropTable(
                name: "TeamUser");

            migrationBuilder.DropTable(
                name: "UserRol");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "AssessDirection");

            migrationBuilder.DropTable(
                name: "ModuleError");

            migrationBuilder.DropTable(
                name: "NotificationType");

            migrationBuilder.DropTable(
                name: "Subscription");

            migrationBuilder.DropTable(
                name: "SaremasEvaluation");

            migrationBuilder.DropTable(
                name: "SessionPart");

            migrationBuilder.DropTable(
                name: "AssessStrength");

            migrationBuilder.DropTable(
                name: "Rol");

            migrationBuilder.DropTable(
                name: "LevelEvent");

            migrationBuilder.DropTable(
                name: "SubscriptionType");

            migrationBuilder.DropTable(
                name: "TrainingSession");

            migrationBuilder.DropTable(
                name: "Microcycle");

            migrationBuilder.DropTable(
                name: "Macrocycle");

            migrationBuilder.DropTable(
                name: "MicrocycleType");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
