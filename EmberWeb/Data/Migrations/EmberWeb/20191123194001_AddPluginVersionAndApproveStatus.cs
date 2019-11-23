using Microsoft.EntityFrameworkCore.Migrations;

namespace EmberWeb.Data.Migrations.EmberWeb
{
    public partial class AddPluginVersionAndApproveStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Approve",
                table: "Plugins",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "EmberVersion",
                table: "Plugins",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LatestVersion",
                table: "Plugins",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceUrl",
                table: "Plugins",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approve",
                table: "Plugins");

            migrationBuilder.DropColumn(
                name: "EmberVersion",
                table: "Plugins");

            migrationBuilder.DropColumn(
                name: "LatestVersion",
                table: "Plugins");

            migrationBuilder.DropColumn(
                name: "SourceUrl",
                table: "Plugins");
        }
    }
}
