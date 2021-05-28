using Microsoft.EntityFrameworkCore.Migrations;

namespace SporeServer.Migrations
{
    public partial class OriginalAssetId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OriginalAssetId",
                table: "Assets",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "b52c135f-6747-4e95-8ec3-2499f79d916a");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalAssetId",
                table: "Assets");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "c183bfa3-c5a6-40c2-897d-b59b0a27192b");
        }
    }
}
