using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Knihovna.Migrations
{
    /// <inheritdoc />
    public partial class userWhoBorrowed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserWhoBorrowedId",
                table: "Books",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_UserWhoBorrowedId",
                table: "Books",
                column: "UserWhoBorrowedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_AspNetUsers_UserWhoBorrowedId",
                table: "Books",
                column: "UserWhoBorrowedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_AspNetUsers_UserWhoBorrowedId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_UserWhoBorrowedId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "UserWhoBorrowedId",
                table: "Books");
        }
    }
}
