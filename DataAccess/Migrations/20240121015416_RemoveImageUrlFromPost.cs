using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveImageUrlFromPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Posts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Posts",
                type: "VARCHAR(200)",
                nullable: false,
                defaultValue: "");
        }
    }
}
