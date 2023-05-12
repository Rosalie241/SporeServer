using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SporeServer.Migrations
{
    /// <inheritdoc />
    public partial class Archetypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SporeServerAssetArcheType",
                columns: table => new
                {
                    ArcheTypeId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AssetId = table.Column<long>(type: "bigint", nullable: false),
                    ArcheType = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SporeServerAssetArcheType", x => x.ArcheTypeId);
                    table.ForeignKey(
                        name: "FK_SporeServerAssetArcheType_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "AssetId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "08aa056b-8619-4322-a6c7-26b8c812fdc1", "86cb002d-8139-462c-8246-35508bc94ba2" });

            migrationBuilder.CreateIndex(
                name: "IX_SporeServerAssetArcheType_AssetId",
                table: "SporeServerAssetArcheType",
                column: "AssetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SporeServerAssetArcheType");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "e90e461b-9070-43cd-bf04-0153367cabc5", "2afed3c2-1034-4c26-8b2d-4a15271cf76b" });
        }
    }
}
