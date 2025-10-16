using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BocciaCoaching.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationNotificationType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NotificationTypeId",
                table: "NotificationMessage",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationMessage_NotificationTypeId",
                table: "NotificationMessage",
                column: "NotificationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationMessage_NotificationType_NotificationTypeId",
                table: "NotificationMessage",
                column: "NotificationTypeId",
                principalTable: "NotificationType",
                principalColumn: "NotificationTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationMessage_NotificationType_NotificationTypeId",
                table: "NotificationMessage");

            migrationBuilder.DropIndex(
                name: "IX_NotificationMessage_NotificationTypeId",
                table: "NotificationMessage");

            migrationBuilder.DropColumn(
                name: "NotificationTypeId",
                table: "NotificationMessage");
        }
    }
}
