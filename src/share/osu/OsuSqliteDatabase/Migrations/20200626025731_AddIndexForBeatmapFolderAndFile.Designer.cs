﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OsuSqliteDatabase.Database;

namespace OsuSqliteDatabase.Migrations
{
    [DbContext(typeof(OsuDatabaseContext))]
    [Migration("20200626025731_AddIndexForBeatmapFolderAndFile")]
    partial class AddIndexForBeatmapFolderAndFile
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5");

            modelBuilder.Entity("OsuSqliteDatabase.Model.OsuDatabase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("AccountLocked")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BeatmapCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FolderCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Permission")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PlayerName")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UnlockedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("Version")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PlayerName");

                    b.ToTable("OsuDatabases");
                });

            modelBuilder.Entity("OsuSqliteDatabase.Model.OsuDatabaseBeatmap", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("ApproachRate")
                        .HasColumnType("REAL");

                    b.Property<string>("Artist")
                        .HasColumnType("TEXT");

                    b.Property<string>("ArtistUnicode")
                        .HasColumnType("TEXT");

                    b.Property<string>("AudioFileName")
                        .HasColumnType("TEXT");

                    b.Property<int>("AudioPreviewTime")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BeatmapId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BeatmapSetId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("BeatmapSkinIgnored")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("BeatmapSoundIgnored")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BytesOfBeatmapEntry")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CatchTheBeatRankRating")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CircleCount")
                        .HasColumnType("INTEGER");

                    b.Property<double>("CircleSize")
                        .HasColumnType("REAL");

                    b.Property<string>("Creator")
                        .HasColumnType("TEXT");

                    b.Property<string>("Difficult")
                        .HasColumnType("TEXT");

                    b.Property<int>("DrainTime")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FileName")
                        .HasColumnType("TEXT");

                    b.Property<string>("FolderName")
                        .HasColumnType("TEXT");

                    b.Property<double>("HPDrain")
                        .HasColumnType("REAL");

                    b.Property<bool>("IsOsz2")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LatestModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LatestPlayedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LatestUpdateAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("LocalOffset")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MD5Hash")
                        .HasColumnType("TEXT");

                    b.Property<int>("ManiaRankRating")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ManiaScrollSpeed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("NotPlayed")
                        .HasColumnType("INTEGER");

                    b.Property<int>("OnlineOffset")
                        .HasColumnType("INTEGER");

                    b.Property<int>("OsuDatabaseId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("OverallDifficulty")
                        .HasColumnType("REAL");

                    b.Property<int>("RankStatus")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RuleSet")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SliderCount")
                        .HasColumnType("INTEGER");

                    b.Property<double>("SliderVelocity")
                        .HasColumnType("REAL");

                    b.Property<string>("Source")
                        .HasColumnType("TEXT");

                    b.Property<int>("SpinnerCount")
                        .HasColumnType("INTEGER");

                    b.Property<double>("StackLeniency")
                        .HasColumnType("REAL");

                    b.Property<int>("StandardRankRating")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("StoryboardDisabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Tags")
                        .HasColumnType("TEXT");

                    b.Property<int>("TaikoRankRating")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ThreadId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TimingPointCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<string>("TitleFont")
                        .HasColumnType("TEXT");

                    b.Property<string>("TitleUnicode")
                        .HasColumnType("TEXT");

                    b.Property<int>("TotalTime")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("VideoDisabled")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("VisualOverrided")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("Artist");

                    b.HasIndex("ArtistUnicode");

                    b.HasIndex("BeatmapId");

                    b.HasIndex("BeatmapSetId");

                    b.HasIndex("CatchTheBeatRankRating");

                    b.HasIndex("Creator");

                    b.HasIndex("Difficult");

                    b.HasIndex("ManiaRankRating");

                    b.HasIndex("NotPlayed");

                    b.HasIndex("OsuDatabaseId");

                    b.HasIndex("RankStatus");

                    b.HasIndex("StandardRankRating");

                    b.HasIndex("TaikoRankRating");

                    b.HasIndex("Title");

                    b.HasIndex("TitleUnicode");

                    b.HasIndex("FolderName", "FileName");

                    b.HasIndex("CircleCount", "SliderCount", "SpinnerCount");

                    b.HasIndex("ApproachRate", "CircleSize", "HPDrain", "OverallDifficulty");

                    b.ToTable("OsuDatabaseBeatmap");
                });

            modelBuilder.Entity("OsuSqliteDatabase.Model.OsuDatabaseBeatmapStarRating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Moderators")
                        .HasColumnType("INTEGER");

                    b.Property<int>("OsuDatabaseBeatmapId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RuleSet")
                        .HasColumnType("INTEGER");

                    b.Property<double>("StarRating")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("Moderators");

                    b.HasIndex("OsuDatabaseBeatmapId");

                    b.HasIndex("StarRating");

                    b.ToTable("OsuDatabaseBeatmapStarRating");
                });

            modelBuilder.Entity("OsuSqliteDatabase.Model.OsuDatabaseTimings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("BeatPreMinute")
                        .HasColumnType("REAL");

                    b.Property<bool>("Inherited")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Offset")
                        .HasColumnType("REAL");

                    b.Property<int>("OsuDatabaseBeatmapId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("BeatPreMinute");

                    b.HasIndex("OsuDatabaseBeatmapId");

                    b.ToTable("OsuDatabaseTimings");
                });

            modelBuilder.Entity("OsuSqliteDatabase.Model.OsuDatabaseBeatmap", b =>
                {
                    b.HasOne("OsuSqliteDatabase.Model.OsuDatabase", "OsuDatabase")
                        .WithMany("Beatmaps")
                        .HasForeignKey("OsuDatabaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OsuSqliteDatabase.Model.OsuDatabaseBeatmapStarRating", b =>
                {
                    b.HasOne("OsuSqliteDatabase.Model.OsuDatabaseBeatmap", "OsuDatabaseBeatmap")
                        .WithMany("StarRatings")
                        .HasForeignKey("OsuDatabaseBeatmapId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OsuSqliteDatabase.Model.OsuDatabaseTimings", b =>
                {
                    b.HasOne("OsuSqliteDatabase.Model.OsuDatabaseBeatmap", "OsuDatabaseBeatmap")
                        .WithMany("OsuDatabaseTimings")
                        .HasForeignKey("OsuDatabaseBeatmapId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
