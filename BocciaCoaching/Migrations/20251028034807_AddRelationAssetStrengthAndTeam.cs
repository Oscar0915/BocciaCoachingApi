using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BocciaCoaching.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationAssetStrengthAndTeam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          
           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssessStrength_Team_TeamId",
                table: "AssessStrength");

            migrationBuilder.DropIndex(
                name: "IX_AssessStrength_TeamId",
                table: "AssessStrength");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "AssessStrength");
        }
    }
}
