using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SouqBooks.Migrations
{
    public partial class renamecompanyname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "companies",
                newName: "CompanyName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CompanyName",
                table: "companies",
                newName: "Name");
        }
    }
}
