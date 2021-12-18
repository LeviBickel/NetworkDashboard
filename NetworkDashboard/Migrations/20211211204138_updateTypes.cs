using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetworkDashboard.Migrations
{
    public partial class updateTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ping",
                table: "MonitorTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ping",
                table: "MonitorTypes");
        }
    }
}
