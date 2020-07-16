﻿// <auto-generated />
using System;
using BeatmapDownloader.Database.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BeatmapDownloader.Database.Migrations
{
    [DbContext(typeof(BeatmapDownloaderDatabaseContext))]
    [Migration("20200711132756_EnhanceDownloadSetModels")]
    partial class EnhanceDownloadSetModels
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5");

            modelBuilder.Entity("BeatmapDownloader.Database.Model.DownloadBeatmapSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BeatmapId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BeatmapSetId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CompletedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("DownloadProviderId")
                        .HasColumnType("TEXT");

                    b.Property<string>("DownloadProviderName")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DownloadTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("DownloadUrl")
                        .HasColumnType("TEXT");

                    b.Property<string>("FullPath")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("DownloadProviderId");

                    b.HasIndex("DownloadProviderName");

                    b.ToTable("DownloadedBeatmapSets");
                });
#pragma warning restore 612, 618
        }
    }
}