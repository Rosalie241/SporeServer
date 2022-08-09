using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SporeServer.Migrations
{
    public partial class FirstUserLockoutEnabled : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "LockoutEnabled", "SecurityStamp" },
                values: new object[] { "e90e461b-9070-43cd-bf04-0153367cabc5", true, "2afed3c2-1034-4c26-8b2d-4a15271cf76b" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "LockoutEnabled", "SecurityStamp" },
                values: new object[] { "c536d83c-2c6c-448a-a82b-e1a58812c84a", false, "f11be4da-ad08-43dd-980a-cd8b6080ddcc" });
        }
    }
}
