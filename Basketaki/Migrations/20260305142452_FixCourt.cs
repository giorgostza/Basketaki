using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basketaki.Migrations
{
    /// <inheritdoc />
    public partial class FixCourt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Court_CourtId",
                table: "Matches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Court",
                table: "Court");

            migrationBuilder.RenameTable(
                name: "Court",
                newName: "Courts");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Courts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Courts",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Courts",
                table: "Courts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Courts_CourtId",
                table: "Matches",
                column: "CourtId",
                principalTable: "Courts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Courts_CourtId",
                table: "Matches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Courts",
                table: "Courts");

            migrationBuilder.RenameTable(
                name: "Courts",
                newName: "Court");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Court",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Court",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Court",
                table: "Court",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Court_CourtId",
                table: "Matches",
                column: "CourtId",
                principalTable: "Court",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
