using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveImageType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageType",
                table: "Images");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImageType",
                table: "Images",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
