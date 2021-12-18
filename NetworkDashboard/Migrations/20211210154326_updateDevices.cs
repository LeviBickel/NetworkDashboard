using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetworkDashboard.Migrations
{
    public partial class updateDevices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "NetworkDevices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "NetworkDevices");
        }
    }
}
