using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace thrucommunity.Migrations
{
    /// <inheritdoc />
    public partial class MMR : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "scoringpoints",
                table: "Players",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "survivalpoints",
                table: "Players",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "scoringpoints",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "survivalpoints",
                table: "Players");
        }
    }
}
