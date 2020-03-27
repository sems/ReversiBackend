using Microsoft.EntityFrameworkCore.Migrations;

namespace ReversiApp.Migrations.ReversiApp
{
    public partial class BeurtSpel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Beurt",
                table: "Spel",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Beurt",
                table: "Spel");
        }
    }
}
