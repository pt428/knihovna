using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Knihovna.Migrations
{
    /// <inheritdoc />
    public partial class Appuserinlibraryuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "LibraryUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LibraryUsers_AppUserId",
                table: "LibraryUsers",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryUsers_AspNetUsers_AppUserId",
                table: "LibraryUsers",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LibraryUsers_AspNetUsers_AppUserId",
                table: "LibraryUsers");

            migrationBuilder.DropIndex(
                name: "IX_LibraryUsers_AppUserId",
                table: "LibraryUsers");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "LibraryUsers");
        }
    }
}
