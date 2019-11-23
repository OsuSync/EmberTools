using Microsoft.EntityFrameworkCore.Migrations;

namespace EmberWeb.Data.Migrations.EmberWeb
{
    public partial class AddPluginDeletedField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approve",
                table: "Plugins");

            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "Plugins",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Plugins",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approved",
                table: "Plugins");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Plugins");

            migrationBuilder.AddColumn<bool>(
                name: "Approve",
                table: "Plugins",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
