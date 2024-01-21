using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixPolymorphicAssiciation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Posts_ImageableId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_UserProfiles_ImageableId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_ImageableId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ImageableId",
                table: "Images");

            migrationBuilder.AddColumn<Guid>(
                name: "PostId",
                table: "Images",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserProfileId",
                table: "Images",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_PostId",
                table: "Images",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_UserProfileId",
                table: "Images",
                column: "UserProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Posts_PostId",
                table: "Images",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_UserProfiles_UserProfileId",
                table: "Images",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Posts_PostId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_UserProfiles_UserProfileId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_PostId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_UserProfileId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "UserProfileId",
                table: "Images");

            migrationBuilder.AddColumn<Guid>(
                name: "ImageableId",
                table: "Images",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Images_ImageableId",
                table: "Images",
                column: "ImageableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Posts_ImageableId",
                table: "Images",
                column: "ImageableId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_UserProfiles_ImageableId",
                table: "Images",
                column: "ImageableId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
