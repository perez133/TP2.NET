using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Gauniv.WebServer.Migrations
{
    /// <inheritdoc />
    public partial class Categorie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Games_GameId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_GameId",
                table: "Categories");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Categories");

            migrationBuilder.CreateTable(
                name: "CategoryGame",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "integer", nullable: false),
                    GamesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryGame", x => new { x.CategoriesId, x.GamesId });
                    table.ForeignKey(
                        name: "FK_CategoryGame_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryGame_Games_GamesId",
                        column: x => x.GamesId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryGame_GamesId",
                table: "CategoryGame",
                column: "GamesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryGame");

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "Categories",
                type: "integer",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "GameId", "Nom" },
                values: new object[,]
                {
                    { 1, null, "Action" },
                    { 2, null, "RPG" },
                    { 3, null, "Stratégie" }
                });

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "Id", "Description", "Nom", "Payload", "Prix", "UserId" },
                values: new object[,]
                {
                    { 1, "Un RPG futuriste en monde ouvert.", "Cyberpunk 2077", new byte[0], 59.99m, null },
                    { 2, "Un RPG épique avec une grande histoire.", "The Witcher 3", new byte[0], 39.99m, null },
                    { 3, "Un jeu de stratégie en temps réel.", "Age of Empires IV", new byte[0], 49.99m, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_GameId",
                table: "Categories",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Games_GameId",
                table: "Categories",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id");
        }
    }
}
