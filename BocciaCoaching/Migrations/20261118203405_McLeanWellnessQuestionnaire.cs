using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BocciaCoaching.Migrations
{
    /// <inheritdoc />
    public partial class McLeanWellnessQuestionnaire : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Mood",
                table: "DailyWellness",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ReadinessLevel",
                table: "DailyWellness",
                type: "varchar(10)",
                maxLength: 10,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "ScaleVersion",
                table: "DailyWellness",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<bool>(
                name: "VetoApplied",
                table: "DailyWellness",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mood",
                table: "DailyWellness");

            migrationBuilder.DropColumn(
                name: "ReadinessLevel",
                table: "DailyWellness");

            migrationBuilder.DropColumn(
                name: "ScaleVersion",
                table: "DailyWellness");

            migrationBuilder.DropColumn(
                name: "VetoApplied",
                table: "DailyWellness");
        }
    }
}
