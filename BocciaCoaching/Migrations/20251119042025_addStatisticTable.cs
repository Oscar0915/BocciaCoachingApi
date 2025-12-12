using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BocciaCoaching.Migrations
{
    /// <inheritdoc />
    public partial class addStatisticTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StrengthStatistics",
                columns: table => new
                {
                    StrengthStatisticsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
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
                    AssessStrengthId = table.Column<int>(type: "int", nullable: false),
                    AthleteId = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_StrengthStatistics_AssessStrengthId",
                table: "StrengthStatistics",
                column: "AssessStrengthId");

            migrationBuilder.CreateIndex(
                name: "IX_StrengthStatistics_AthleteId",
                table: "StrengthStatistics",
                column: "AthleteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StrengthStatistics");
        }
    }
}
