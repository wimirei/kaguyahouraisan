using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace thrucommunity.Migrations
{
    /// <inheritdoc />
    public partial class WBAWC4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "No4thCondition",
                table: "Replays",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "No4thCondition",
                table: "Replays");
        }
    }
}
