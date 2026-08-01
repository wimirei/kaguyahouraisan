using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace thrucommunity.Migrations
{
    /// <inheritdoc />
    public partial class FinalsIN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "INFinal",
                table: "Replays",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "INFinal",
                table: "Replays");
        }
    }
}
