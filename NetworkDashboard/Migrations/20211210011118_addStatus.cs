using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetworkDashboard.Migrations
{
    public partial class addStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsConnected",
                table: "NetworkDevices",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsConnected",
                table: "NetworkDevices");
        }
    }
}
