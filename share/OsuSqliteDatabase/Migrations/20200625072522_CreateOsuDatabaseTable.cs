using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OsuSqliteDatabase.Migrations
{
    public partial class CreateOsuDatabaseTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OsuDatabases",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Version = table.Column<int>(nullable: false),
                    AccountLocked = table.Column<bool>(nullable: false),
                    UnlockedAt = table.Column<DateTime>(nullable: false),
                    PlayerName = table.Column<string>(nullable: true),
                    BeatmapCount = table.Column<int>(nullable: false),
                    Permission = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OsuDatabases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OsuDatabaseBeatmap",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OsuDatabaseId = table.Column<int>(nullable: false),
                    BytesOfBeatmapEntry = table.Column<int>(nullable: false),
                    Artist = table.Column<string>(nullable: true),
                    ArtistUnicode = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    TitleUnicode = table.Column<string>(nullable: true),
                    Creator = table.Column<string>(nullable: true),
                    Difficult = table.Column<string>(nullable: true),
                    AudioFileName = table.Column<string>(nullable: true),
                    MD5Hash = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    RankStatus = table.Column<int>(nullable: false),
                    CircleCount = table.Column<int>(nullable: false),
                    SliderCount = table.Column<int>(nullable: false),
                    SpinnerCount = table.Column<int>(nullable: false),
                    LatestModifiedAt = table.Column<DateTime>(nullable: false),
                    ApproachRate = table.Column<double>(nullable: false),
                    CircleSize = table.Column<double>(nullable: false),
                    HPDrain = table.Column<double>(nullable: false),
                    OverallDifficulty = table.Column<double>(nullable: false),
                    SliderVelocity = table.Column<double>(nullable: false),
                    TimingPointCount = table.Column<int>(nullable: false),
                    BeatmapId = table.Column<int>(nullable: false),
                    BeatmapSetId = table.Column<int>(nullable: false),
                    ThreadId = table.Column<int>(nullable: false),
                    StandardRankRating = table.Column<int>(nullable: false),
                    TaikoRankRating = table.Column<int>(nullable: false),
                    CatchTheBeatRankRating = table.Column<int>(nullable: false),
                    ManiaRankRating = table.Column<int>(nullable: false),
                    LocalOffset = table.Column<int>(nullable: false),
                    StackLeniency = table.Column<double>(nullable: false),
                    RuleSet = table.Column<int>(nullable: false),
                    Score = table.Column<string>(nullable: true),
                    Tags = table.Column<string>(nullable: true),
                    OnlineOffset = table.Column<int>(nullable: false),
                    TitleFont = table.Column<string>(nullable: true),
                    NotPlayed = table.Column<bool>(nullable: false),
                    IsOsz2 = table.Column<bool>(nullable: false),
                    FolderName = table.Column<string>(nullable: true),
                    LatestUpdatedAt = table.Column<DateTime>(nullable: false),
                    BeatmapSoundIgnored = table.Column<bool>(nullable: false),
                    BeatmapSkinIgnored = table.Column<bool>(nullable: false),
                    StoryboardDisabled = table.Column<bool>(nullable: false),
                    VideoDisabled = table.Column<bool>(nullable: false),
                    VisualOverrided = table.Column<bool>(nullable: false),
                    ManiaScrollSpeed = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OsuDatabaseBeatmap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OsuDatabaseBeatmap_OsuDatabases_OsuDatabaseId",
                        column: x => x.OsuDatabaseId,
                        principalTable: "OsuDatabases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OsuDatabaseBeatmapStarRating",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OsuDatabaseBeatmapId = table.Column<int>(nullable: false),
                    Mode = table.Column<int>(nullable: false),
                    Moderators = table.Column<int>(nullable: false),
                    StarRating = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OsuDatabaseBeatmapStarRating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OsuDatabaseBeatmapStarRating_OsuDatabaseBeatmap_OsuDatabaseBeatmapId",
                        column: x => x.OsuDatabaseBeatmapId,
                        principalTable: "OsuDatabaseBeatmap",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OsuDatabaseTimings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OsuDatabaseBeatmapId = table.Column<int>(nullable: false),
                    BeatPreMinute = table.Column<double>(nullable: false),
                    Offset = table.Column<double>(nullable: false),
                    Inherited = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OsuDatabaseTimings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OsuDatabaseTimings_OsuDatabaseBeatmap_OsuDatabaseBeatmapId",
                        column: x => x.OsuDatabaseBeatmapId,
                        principalTable: "OsuDatabaseBeatmap",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OsuDatabaseBeatmap_Artist",
                table: "OsuDatabaseBeatmap",
                column: "Artist");

            migrationBuilder.CreateIndex(
                name: "IX_OsuDatabaseBeatmap_ArtistUnicode",
                table: "OsuDatabaseBeatmap",
                column: "ArtistUnicode");

            migrationBuilder.CreateIndex(
                name: "IX_OsuDatabaseBeatmap_BeatmapId",
                table: "OsuDatabaseBeatmap",
                column: "BeatmapId");

            migrationBuilder.CreateIndex(
                name: "IX_OsuDatabaseBeatmap_BeatmapSetId",
                table: "OsuDatabaseBeatmap",
                column: "BeatmapSetId");

            migrationBuilder.CreateIndex(
                name: "IX_OsuDatabaseBeatmap_CatchTheBeatRankRating",
                table: "OsuDatabaseBeatmap",
                column: "CatchTheBeatRankRating");

            migrationBuilder.CreateIndex(
                name: "IX_OsuDatabaseBeatmap_Creator",
                table: "OsuDatabaseBeatmap",
                column: "Creator");

            migrationBuilder.CreateIndex(
                name: "IX_OsuDatabaseBeatmap_Difficult",
                table: "OsuDatabaseBeatmap",
                column: "Difficult");

            migrationBuilder.CreateIndex(
                name: "IX_OsuDatabaseBeatmap_ManiaRankRating",
                table: "OsuDatabaseBeatmap",
                column: "ManiaRankRating");

            migrationBuilder.CreateIndex(
                name: "IX_OsuDatabaseBeatmap_NotPlayed",
                table: "OsuDatabaseBeatmap",
                column: "NotPlayed");

            migrationBuilder.CreateIndex(
                name: "IX_OsuDatabaseBeatmap_OsuDatabaseId",
                table: "OsuDatabaseBeatmap",
                column: "OsuDatabaseId");

            migrationBuilder.CreateIndex(
                name: "IX_OsuDatabaseBeatmap_RankStatus",
                table: "OsuDatabaseBeatmap",
                column: "RankStatus");

            migrationBuilder.CreateIndex(
                name: "IX_OsuDatabaseBeatmap_StandardRankRating",
                table: "OsuDatabaseBeatmap",
                column: "StandardRankRating");

            migrationBuilder.CreateIndex(
                name: "IX_OsuDatabaseBeatmap_TaikoRankRating",
                table: "OsuDatabaseBeatmap",
                column: "TaikoRankRating");

            migrationBuilder.CreateIndex(
                name: "IX_OsuDatabaseBeatmap_Title",
                table: "OsuDatabaseBeatmap",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_OsuDatabaseBeatmap_TitleUnicode",
                table: "OsuDatabaseBeatmap",
                column: "TitleUnicode");

            migrationBuilder.CreateIndex(
                name: "IX_OsuDatabaseBeatmap_CircleCount_SliderCount_SpinnerCount",
                table: "OsuDatabaseBeatmap",
                columns: new[] { "CircleCount", "SliderCount", "SpinnerCount" });

            migrationBuilder.CreateIndex(
                name: "IX_OsuDatabaseBeatmap_ApproachRate_CircleSize_HPDrain_OverallDifficulty",
                table: "OsuDatabaseBeatmap",
                columns: new[] { "ApproachRate", "CircleSize", "HPDrain", "OverallDifficulty" });

            migrationBuilder.CreateIndex(
                name: "IX_OsuDatabaseBeatmapStarRating_Moderators",
                table: "OsuDatabaseBeatmapStarRating",
                column: "Moderators");

            migrationBuilder.CreateIndex(
                name: "IX_OsuDatabaseBeatmapStarRating_OsuDatabaseBeatmapId",
                table: "OsuDatabaseBeatmapStarRating",
                column: "OsuDatabaseBeatmapId");

            migrationBuilder.CreateIndex(
                name: "IX_OsuDatabaseBeatmapStarRating_StarRating",
                table: "OsuDatabaseBeatmapStarRating",
                column: "StarRating");

            migrationBuilder.CreateIndex(
                name: "IX_OsuDatabases_PlayerName",
                table: "OsuDatabases",
                column: "PlayerName");

            migrationBuilder.CreateIndex(
                name: "IX_OsuDatabaseTimings_BeatPreMinute",
                table: "OsuDatabaseTimings",
                column: "BeatPreMinute");

            migrationBuilder.CreateIndex(
                name: "IX_OsuDatabaseTimings_OsuDatabaseBeatmapId",
                table: "OsuDatabaseTimings",
                column: "OsuDatabaseBeatmapId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OsuDatabaseBeatmapStarRating");

            migrationBuilder.DropTable(
                name: "OsuDatabaseTimings");

            migrationBuilder.DropTable(
                name: "OsuDatabaseBeatmap");

            migrationBuilder.DropTable(
                name: "OsuDatabases");
        }
    }
}
