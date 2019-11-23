using Microsoft.EntityFrameworkCore.Migrations;

namespace EmberWeb.Data.Migrations.EmberWeb
{
    public partial class AddPluginDownloadUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DownloadUrl",
                table: "PluginVersions",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DownloadUrl",
                table: "Plugins",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownloadUrl",
                table: "PluginVersions");

            migrationBuilder.DropColumn(
                name: "DownloadUrl",
                table: "Plugins");
        }
    }
}
