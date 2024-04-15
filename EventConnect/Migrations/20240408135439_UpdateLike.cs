using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventConnect.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLike : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PosyName",
                table: "Likes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PosyName",
                table: "Likes");
        }
    }
}
