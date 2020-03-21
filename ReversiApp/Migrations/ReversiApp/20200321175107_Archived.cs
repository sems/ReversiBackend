using Microsoft.EntityFrameworkCore.Migrations;

namespace ReversiApp.Migrations.ReversiApp
{
    public partial class Archived : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Archived",
                table: "AspNetUsers");
        }
    }
}
