using Microsoft.EntityFrameworkCore.Migrations;

namespace ReversiApp.Migrations.ReversiApp
{
    public partial class SpelRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SpelId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Spel",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Omschrijving = table.Column<string>(nullable: true),
                    Token = table.Column<string>(nullable: true),
                    SerializedBord = table.Column<string>(nullable: true),
                    AandeBeurt = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spel", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SpelId",
                table: "AspNetUsers",
                column: "SpelId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Spel_SpelId",
                table: "AspNetUsers",
                column: "SpelId",
                principalTable: "Spel",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Spel_SpelId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Spel");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SpelId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SpelId",
                table: "AspNetUsers");
        }
    }
}
