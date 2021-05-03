using Microsoft.EntityFrameworkCore.Migrations;

namespace SporeServer.Migrations
{
    public partial class RelativeUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageFile2Url",
                table: "Assets",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageFile3Url",
                table: "Assets",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageFile4Url",
                table: "Assets",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageFileUrl",
                table: "Assets",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModelFileUrl",
                table: "Assets",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThumbFileUrl",
                table: "Assets",
                type: "longtext",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "d0226572-70ee-4635-b8bc-ff9fcac509ac");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFile2Url",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "ImageFile3Url",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "ImageFile4Url",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "ImageFileUrl",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "ModelFileUrl",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "ThumbFileUrl",
                table: "Assets");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "edc241e0-d51e-4451-903f-99c6021db111");
        }
    }
}
