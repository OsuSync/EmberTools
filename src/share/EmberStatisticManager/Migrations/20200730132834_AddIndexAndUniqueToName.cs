using Microsoft.EntityFrameworkCore.Migrations;

namespace EmberStatisticDatabase.Migrations
{
    public partial class AddIndexAndUniqueToName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RegisteredFormats_Name",
                table: "RegisteredFormats",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RegisteredFormats_Name",
                table: "RegisteredFormats");
        }
    }
}
