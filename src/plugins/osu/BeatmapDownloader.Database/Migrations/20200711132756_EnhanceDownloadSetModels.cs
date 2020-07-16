using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeatmapDownloader.Database.Migrations
{
    public partial class EnhanceDownloadSetModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "DownloadedBeatmapSets",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "DownloadedBeatmapSets",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DownloadUrl",
                table: "DownloadedBeatmapSets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullPath",
                table: "DownloadedBeatmapSets",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartedAt",
                table: "DownloadedBeatmapSets",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "DownloadedBeatmapSets",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "DownloadedBeatmapSets");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "DownloadedBeatmapSets");

            migrationBuilder.DropColumn(
                name: "DownloadUrl",
                table: "DownloadedBeatmapSets");

            migrationBuilder.DropColumn(
                name: "FullPath",
                table: "DownloadedBeatmapSets");

            migrationBuilder.DropColumn(
                name: "StartedAt",
                table: "DownloadedBeatmapSets");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "DownloadedBeatmapSets");
        }
    }
}
