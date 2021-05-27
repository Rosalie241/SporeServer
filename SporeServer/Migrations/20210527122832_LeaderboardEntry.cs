using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SporeServer.Migrations
{
    public partial class LeaderboardEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeaderboardEntries",
                columns: table => new
                {
                    EntryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AssetId = table.Column<long>(type: "bigint", nullable: false),
                    AuthorId = table.Column<long>(type: "bigint", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PercentageCompleted = table.Column<int>(type: "int", nullable: false),
                    TimeInMilliseconds = table.Column<long>(type: "bigint", nullable: false),
                    CaptainId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaderboardEntries", x => x.EntryId);
                    table.ForeignKey(
                        name: "FK_LeaderboardEntries_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeaderboardEntries_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "AssetId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeaderboardEntries_Assets_CaptainId",
                        column: x => x.CaptainId,
                        principalTable: "Assets",
                        principalColumn: "AssetId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "c183bfa3-c5a6-40c2-897d-b59b0a27192b");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderboardEntries_AssetId",
                table: "LeaderboardEntries",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderboardEntries_AuthorId",
                table: "LeaderboardEntries",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderboardEntries_CaptainId",
                table: "LeaderboardEntries",
                column: "CaptainId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeaderboardEntries");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "82b0da2b-cfd9-47e2-833b-a79f4c1428e2");
        }
    }
}
