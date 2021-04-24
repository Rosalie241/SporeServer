using Microsoft.EntityFrameworkCore.Migrations;

namespace SporeServer.Migrations
{
    public partial class Tags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "AssetId",
                table: "Assets",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "600000000, 1")
                .OldAnnotation("SqlServer:Identity", "6000000, 1");

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Assets",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Assets");

            migrationBuilder.AlterColumn<long>(
                name: "AssetId",
                table: "Assets",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "6000000, 1")
                .OldAnnotation("SqlServer:Identity", "600000000, 1");
        }
    }
}
