using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace thrucommunity.Migrations
{
    /// <inheritdoc />
    public partial class Profiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nickname = table.Column<string>(type: "text", nullable: false),
                    L1CCcount = table.Column<int>(type: "integer", nullable: false),
                    LNMcount = table.Column<int>(type: "integer", nullable: false),
                    LNBcount = table.Column<int>(type: "integer", nullable: false),
                    LNNcount = table.Column<int>(type: "integer", nullable: false),
                    LNNNcount = table.Column<int>(type: "integer", nullable: false),
                    LNBNxcount = table.Column<int>(type: "integer", nullable: false),
                    ThirdPlaceCount = table.Column<int>(type: "integer", nullable: false),
                    SecondPlaceCount = table.Column<int>(type: "integer", nullable: false),
                    FirstPlaceCount = table.Column<int>(type: "integer", nullable: false),
                    WRcount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
