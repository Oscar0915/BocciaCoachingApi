using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BocciaCoaching.Migrations
{
    /// <inheritdoc />
    public partial class RenameNotificationMessageFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationMessage_User_AthleteId",
                table: "NotificationMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationMessage_User_CoachId",
                table: "NotificationMessage");

            migrationBuilder.RenameColumn(
                name: "CoachId",
                table: "NotificationMessage",
                newName: "SenderId");

            migrationBuilder.RenameColumn(
                name: "AthleteId",
                table: "NotificationMessage",
                newName: "ReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationMessage_CoachId",
                table: "NotificationMessage",
                newName: "IX_NotificationMessage_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationMessage_AthleteId",
                table: "NotificationMessage",
                newName: "IX_NotificationMessage_ReceiverId");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "AssessStrength",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "AssessStrength",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AssessStrength",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AssessStrength",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationMessage_User_ReceiverId",
                table: "NotificationMessage",
                column: "ReceiverId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationMessage_User_SenderId",
                table: "NotificationMessage",
                column: "SenderId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationMessage_User_ReceiverId",
                table: "NotificationMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationMessage_User_SenderId",
                table: "NotificationMessage");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AssessStrength");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AssessStrength");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "NotificationMessage",
                newName: "CoachId");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "NotificationMessage",
                newName: "AthleteId");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationMessage_SenderId",
                table: "NotificationMessage",
                newName: "IX_NotificationMessage_CoachId");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationMessage_ReceiverId",
                table: "NotificationMessage",
                newName: "IX_NotificationMessage_AthleteId");

            migrationBuilder.UpdateData(
                table: "AssessStrength",
                keyColumn: "State",
                keyValue: null,
                column: "State",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "AssessStrength",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AssessStrength",
                keyColumn: "Description",
                keyValue: null,
                column: "Description",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "AssessStrength",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationMessage_User_AthleteId",
                table: "NotificationMessage",
                column: "AthleteId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationMessage_User_CoachId",
                table: "NotificationMessage",
                column: "CoachId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
