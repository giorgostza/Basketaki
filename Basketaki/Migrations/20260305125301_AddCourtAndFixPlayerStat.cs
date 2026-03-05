using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basketaki.Migrations
{
    /// <inheritdoc />
    public partial class AddCourtAndFixPlayerStat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerStats_Players_PlayerId",
                table: "PlayerStats");

            migrationBuilder.DropIndex(
                name: "IX_PlayerStats_PlayerId",
                table: "PlayerStats");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "PlayerStats");

            migrationBuilder.AddColumn<int>(
                name: "CourtId",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Court",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Court", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_CourtId_MatchDate",
                table: "Matches",
                columns: new[] { "CourtId", "MatchDate" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Court_CourtId",
                table: "Matches",
                column: "CourtId",
                principalTable: "Court",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Court_CourtId",
                table: "Matches");

            migrationBuilder.DropTable(
                name: "Court");

            migrationBuilder.DropIndex(
                name: "IX_Matches_CourtId_MatchDate",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "CourtId",
                table: "Matches");

            migrationBuilder.AddColumn<int>(
                name: "PlayerId",
                table: "PlayerStats",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerStats_PlayerId",
                table: "PlayerStats",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerStats_Players_PlayerId",
                table: "PlayerStats",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id");
        }
    }
}
