using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ModifyNotificationToIncludeRecipientAndSender : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_UserProfiles_UserProfileId",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "UserProfileId",
                table: "Notifications",
                newName: "SenderUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_UserProfileId",
                table: "Notifications",
                newName: "IX_Notifications_SenderUserId");

            migrationBuilder.AddColumn<Guid>(
                name: "RecipientUserId",
                table: "Notifications",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_RecipientUserId",
                table: "Notifications",
                column: "RecipientUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_UserProfiles_RecipientUserId",
                table: "Notifications",
                column: "RecipientUserId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_UserProfiles_SenderUserId",
                table: "Notifications",
                column: "SenderUserId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_UserProfiles_RecipientUserId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_UserProfiles_SenderUserId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_RecipientUserId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "RecipientUserId",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "SenderUserId",
                table: "Notifications",
                newName: "UserProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_SenderUserId",
                table: "Notifications",
                newName: "IX_Notifications_UserProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_UserProfiles_UserProfileId",
                table: "Notifications",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
