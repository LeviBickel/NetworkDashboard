using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetworkDashboard.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MonitorTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypeDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitorTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NetworkDevices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IPAddress = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetworkDevices", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonitorTypes");

            migrationBuilder.DropTable(
                name: "NetworkDevices");
        }
    }
}
