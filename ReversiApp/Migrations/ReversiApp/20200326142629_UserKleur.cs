using Microsoft.EntityFrameworkCore.Migrations;

namespace ReversiApp.Migrations.ReversiApp
{
    public partial class UserKleur : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Kleur",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Kleur",
                table: "AspNetUsers");
        }
    }
}
