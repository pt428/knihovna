using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Knihovna.Migrations
{
    /// <inheritdoc />
    public partial class removeAppUserFromBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_AspNetUsers_UserWhoBorrowedId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_AspNetUsers_UserWhoReservedId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_UserWhoBorrowedId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_UserWhoReservedId",
                table: "Books");

            migrationBuilder.AlterColumn<string>(
                name: "UserWhoReservedId",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserWhoBorrowedId",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserWhoReservedId",
                table: "Books",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserWhoBorrowedId",
                table: "Books",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Books_UserWhoBorrowedId",
                table: "Books",
                column: "UserWhoBorrowedId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_UserWhoReservedId",
                table: "Books",
                column: "UserWhoReservedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_AspNetUsers_UserWhoBorrowedId",
                table: "Books",
                column: "UserWhoBorrowedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_AspNetUsers_UserWhoReservedId",
                table: "Books",
                column: "UserWhoReservedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
