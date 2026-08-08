using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace thrucommunity.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Replays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nickname = table.Column<string>(type: "text", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false),
                    Game = table.Column<int>(type: "integer", nullable: false),
                    ShotType = table.Column<string>(type: "text", nullable: false),
                    Survival = table.Column<bool>(type: "boolean", nullable: false),
                    Scoring = table.Column<bool>(type: "boolean", nullable: false),
                    Score = table.Column<long>(type: "bigint", nullable: false),
                    DeathCount = table.Column<int>(type: "integer", nullable: false),
                    NoMiss = table.Column<bool>(type: "boolean", nullable: false),
                    NoBomb = table.Column<bool>(type: "boolean", nullable: false),
                    NoThirdCondition = table.Column<bool>(type: "boolean", nullable: false),
                    ReplayFileName = table.Column<string>(type: "text", nullable: false),
                    ReplayFilePath = table.Column<string>(type: "text", nullable: false),
                    ReplayLink = table.Column<string>(type: "text", nullable: false),
                    ReplayDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SubmissionStatus = table.Column<int>(type: "integer", nullable: false),
                    Proven = table.Column<bool>(type: "boolean", nullable: false),
                    SubmittedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Replays", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Replays");
        }
    }
}
