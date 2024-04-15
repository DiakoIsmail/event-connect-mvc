using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventConnect.Migrations
{
    /// <inheritdoc />
    public partial class LikeNameChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PosyName",
                table: "Likes",
                newName: "PostName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PostName",
                table: "Likes",
                newName: "PosyName");
        }
    }
}
