using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Knihovna.Migrations
{
    /// <inheritdoc />
    public partial class Reserved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserWhoReservedId",
                table: "Books",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_UserWhoReservedId",
                table: "Books",
                column: "UserWhoReservedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_AspNetUsers_UserWhoReservedId",
                table: "Books",
                column: "UserWhoReservedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_AspNetUsers_UserWhoReservedId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_UserWhoReservedId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "UserWhoReservedId",
                table: "Books");
        }
    }
}
