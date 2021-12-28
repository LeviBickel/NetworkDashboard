using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetworkDashboard.Migrations
{
    public partial class SNMPSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SNMPAuth",
                table: "CrossPlatformSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SNMPPriv",
                table: "CrossPlatformSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SNMPUsername",
                table: "CrossPlatformSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SNMPAuth",
                table: "CrossPlatformSettings");

            migrationBuilder.DropColumn(
                name: "SNMPPriv",
                table: "CrossPlatformSettings");

            migrationBuilder.DropColumn(
                name: "SNMPUsername",
                table: "CrossPlatformSettings");
        }
    }
}
