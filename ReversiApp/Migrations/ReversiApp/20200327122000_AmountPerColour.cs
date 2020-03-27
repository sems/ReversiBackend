using Microsoft.EntityFrameworkCore.Migrations;

namespace ReversiApp.Migrations.ReversiApp
{
    public partial class AmountPerColour : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SerializedAmountOfBlack",
                table: "Spel",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SerializedAmountOfWhite",
                table: "Spel",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SerializedAmountOfBlack",
                table: "Spel");

            migrationBuilder.DropColumn(
                name: "SerializedAmountOfWhite",
                table: "Spel");
        }
    }
}
