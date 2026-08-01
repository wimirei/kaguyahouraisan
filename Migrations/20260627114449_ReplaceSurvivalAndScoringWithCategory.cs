using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace thrucommunity.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceSurvivalAndScoringWithCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Scoring",
                table: "Replays");

            migrationBuilder.DropColumn(
                name: "Survival",
                table: "Replays");

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "Replays",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Replays");

            migrationBuilder.AddColumn<bool>(
                name: "Scoring",
                table: "Replays",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Survival",
                table: "Replays",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
