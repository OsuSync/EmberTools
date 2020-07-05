using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeatmapDownloader.Database.Migrations
{
    public partial class InitializeMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DownloadedBeatmapSets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BeatmapId = table.Column<int>(nullable: false),
                    BeatmapSetId = table.Column<int>(nullable: false),
                    DownloadProviderId = table.Column<string>(nullable: true),
                    DownloadProviderName = table.Column<string>(nullable: true),
                    DownloadTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DownloadedBeatmapSets", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DownloadedBeatmapSets_DownloadProviderId",
                table: "DownloadedBeatmapSets",
                column: "DownloadProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_DownloadedBeatmapSets_DownloadProviderName",
                table: "DownloadedBeatmapSets",
                column: "DownloadProviderName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DownloadedBeatmapSets");
        }
    }
}
