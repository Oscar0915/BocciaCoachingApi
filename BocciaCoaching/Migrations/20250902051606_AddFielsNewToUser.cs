using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BocciaCoaching.Migrations
{
    /// <inheritdoc />
    public partial class AddFielsNewToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "User",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "User",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BoxNumber",
                table: "EvaluationDetailStrength",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Observations",
                table: "EvaluationDetailStrength",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "ScoreObtained",
                table: "EvaluationDetailStrength",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "EvaluationDetailStrength",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "TargetDistance",
                table: "EvaluationDetailStrength",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ThrowOrder",
                table: "EvaluationDetailStrength",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "User");

            migrationBuilder.DropColumn(
                name: "BoxNumber",
                table: "EvaluationDetailStrength");

            migrationBuilder.DropColumn(
                name: "Observations",
                table: "EvaluationDetailStrength");

            migrationBuilder.DropColumn(
                name: "ScoreObtained",
                table: "EvaluationDetailStrength");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "EvaluationDetailStrength");

            migrationBuilder.DropColumn(
                name: "TargetDistance",
                table: "EvaluationDetailStrength");

            migrationBuilder.DropColumn(
                name: "ThrowOrder",
                table: "EvaluationDetailStrength");
        }
    }
}
