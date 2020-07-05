using Microsoft.EntityFrameworkCore.Migrations;

namespace OsuSqliteDatabase.Migrations
{
    public partial class AddIndexForBeatmapFolderAndFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_OsuDatabaseBeatmap_FolderName_FileName",
                table: "OsuDatabaseBeatmap",
                columns: new[] { "FolderName", "FileName" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OsuDatabaseBeatmap_FolderName_FileName",
                table: "OsuDatabaseBeatmap");
        }
    }
}
