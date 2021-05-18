using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SporeServer.Migrations
{
    public partial class UnlockedAchievements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UnlockedAchievements",
                columns: table => new
                {
                    UnlockedAchievementId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AuthorId = table.Column<long>(type: "bigint", nullable: false),
                    AchievementId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnlockedAchievements", x => x.UnlockedAchievementId);
                    table.ForeignKey(
                        name: "FK_UnlockedAchievements_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "0bb7a089-fb1a-4d8a-9369-73d3eca4a6d1");

            migrationBuilder.CreateIndex(
                name: "IX_UnlockedAchievements_AuthorId",
                table: "UnlockedAchievements",
                column: "AuthorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UnlockedAchievements");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "2124a11d-81f6-4e3e-bad2-1b589c948cdc");
        }
    }
}
