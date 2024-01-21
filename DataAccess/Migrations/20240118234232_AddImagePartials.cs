using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddImagePartials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Images",
                newName: "ThumbnailImagePath");

            migrationBuilder.AddColumn<string>(
                name: "FullscreenImagePath",
                table: "Images",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OriginalImagePath",
                table: "Images",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullscreenImagePath",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "OriginalImagePath",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "ThumbnailImagePath",
                table: "Images",
                newName: "Location");
        }
    }
}
