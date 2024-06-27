using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Knihovna.Migrations
{
    /// <inheritdoc />
    public partial class updateReader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReaderId",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReaderId1",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_ReaderId",
                table: "Books",
                column: "ReaderId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_ReaderId1",
                table: "Books",
                column: "ReaderId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Readers_ReaderId",
                table: "Books",
                column: "ReaderId",
                principalTable: "Readers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Readers_ReaderId1",
                table: "Books",
                column: "ReaderId1",
                principalTable: "Readers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Readers_ReaderId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Readers_ReaderId1",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_ReaderId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_ReaderId1",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "ReaderId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "ReaderId1",
                table: "Books");
        }
    }
}
