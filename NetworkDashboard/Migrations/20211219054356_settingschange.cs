using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetworkDashboard.Migrations
{
    public partial class settingschange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "CrossPlatformSettings",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CrossPlatformSettings",
                table: "CrossPlatformSettings",
                column: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CrossPlatformSettings",
                table: "CrossPlatformSettings");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "CrossPlatformSettings");
        }
    }
}
