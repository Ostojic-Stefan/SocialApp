using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RefactorForUserProfileImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "UserProfiles");

            migrationBuilder.AddColumn<Guid>(
                name: "ProfileImageId",
                table: "UserProfiles",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_ProfileImageId",
                table: "UserProfiles",
                column: "ProfileImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Images_ProfileImageId",
                table: "UserProfiles",
                column: "ProfileImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Images_ProfileImageId",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_ProfileImageId",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "ProfileImageId",
                table: "UserProfiles");

            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "UserProfiles",
                type: "VARCHAR(200)",
                nullable: true);
        }
    }
}
