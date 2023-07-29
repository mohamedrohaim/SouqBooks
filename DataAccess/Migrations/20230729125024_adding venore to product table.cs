using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SouqBooks.Migrations
{
    public partial class addingvenoretoproducttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "applicationUserId",
                table: "products",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_products_applicationUserId",
                table: "products",
                column: "applicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_products_AspNetUsers_applicationUserId",
                table: "products",
                column: "applicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_AspNetUsers_applicationUserId",
                table: "products");

            migrationBuilder.DropIndex(
                name: "IX_products_applicationUserId",
                table: "products");

            migrationBuilder.DropColumn(
                name: "applicationUserId",
                table: "products");
        }
    }
}
