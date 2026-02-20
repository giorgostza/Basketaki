using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basketaki.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    JerseyNumber = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Seasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CoachName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Leagues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SeasonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leagues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Leagues_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerSeasonTeams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    SeasonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerSeasonTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerSeasonTeams_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerSeasonTeams_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerSeasonTeams_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamSeasonLeagues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    LeagueId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamSeasonLeagues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamSeasonLeagues_Leagues_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "Leagues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamSeasonLeagues_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MatchDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeagueId = table.Column<int>(type: "int", nullable: false),
                    HomeTeamSeasonLeagueId = table.Column<int>(type: "int", nullable: false),
                    AwayTeamSeasonLeagueId = table.Column<int>(type: "int", nullable: false),
                    HomeScore = table.Column<int>(type: "int", nullable: false),
                    AwayScore = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Leagues_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "Leagues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Matches_TeamSeasonLeagues_AwayTeamSeasonLeagueId",
                        column: x => x.AwayTeamSeasonLeagueId,
                        principalTable: "TeamSeasonLeagues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_TeamSeasonLeagues_HomeTeamSeasonLeagueId",
                        column: x => x.HomeTeamSeasonLeagueId,
                        principalTable: "TeamSeasonLeagues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlayerStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerSeasonTeamId = table.Column<int>(type: "int", nullable: false),
                    MatchId = table.Column<int>(type: "int", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    Rebounds = table.Column<int>(type: "int", nullable: false),
                    Assists = table.Column<int>(type: "int", nullable: false),
                    Steals = table.Column<int>(type: "int", nullable: false),
                    Blocks = table.Column<int>(type: "int", nullable: false),
                    Fouls = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerStats_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerStats_PlayerSeasonTeams_PlayerSeasonTeamId",
                        column: x => x.PlayerSeasonTeamId,
                        principalTable: "PlayerSeasonTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlayerStats_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Leagues_SeasonId",
                table: "Leagues",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_AwayTeamSeasonLeagueId",
                table: "Matches",
                column: "AwayTeamSeasonLeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_HomeTeamSeasonLeagueId",
                table: "Matches",
                column: "HomeTeamSeasonLeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_LeagueId",
                table: "Matches",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerSeasonTeams_PlayerId_SeasonId",
                table: "PlayerSeasonTeams",
                columns: new[] { "PlayerId", "SeasonId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerSeasonTeams_SeasonId",
                table: "PlayerSeasonTeams",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerSeasonTeams_TeamId",
                table: "PlayerSeasonTeams",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerStats_MatchId",
                table: "PlayerStats",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerStats_PlayerId",
                table: "PlayerStats",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerStats_PlayerSeasonTeamId",
                table: "PlayerStats",
                column: "PlayerSeasonTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamSeasonLeagues_LeagueId",
                table: "TeamSeasonLeagues",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamSeasonLeagues_TeamId_LeagueId",
                table: "TeamSeasonLeagues",
                columns: new[] { "TeamId", "LeagueId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerStats");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "PlayerSeasonTeams");

            migrationBuilder.DropTable(
                name: "TeamSeasonLeagues");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Leagues");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Seasons");
        }
    }
}
