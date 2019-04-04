﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Radio.Infrastructure.DbAccess;

namespace Radio.Infrastructure.DbAccess.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:PostgresExtension:uuid-ossp", ",,")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Radio.Core.Domain.MasterData.Model.File", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Data");

                    b.HasKey("Id");

                    b.ToTable("File");
                });

            modelBuilder.Entity("Radio.Core.Domain.MasterData.Model.Image", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ContentLength");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<Guid>("FileId");

                    b.HasKey("Id");

                    b.HasIndex("FileId");

                    b.ToTable("Image");
                });

            modelBuilder.Entity("Radio.Core.Domain.MasterData.Model.Song", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Album")
                        .HasMaxLength(256);

                    b.Property<string>("Artist")
                        .HasMaxLength(256);

                    b.Property<Guid?>("CoverImageId");

                    b.Property<int>("DurationInSeconds");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(1024);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("CoverImageId");

                    b.ToTable("Song");
                });

            modelBuilder.Entity("Radio.Core.Domain.Playback.Model.CurrentSong", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("EndsAtTime");

                    b.Property<Guid>("SongId");

                    b.Property<int>("VoteCount");

                    b.HasKey("Id");

                    b.HasIndex("SongId");

                    b.ToTable("CurrentSong");
                });

            modelBuilder.Entity("Radio.Core.Domain.Voting.Model.Vote", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("UserIdentifier");

                    b.Property<Guid>("VotingCandidateId");

                    b.HasKey("Id");

                    b.HasIndex("VotingCandidateId");

                    b.ToTable("Vote");
                });

            modelBuilder.Entity("Radio.Core.Domain.Voting.Model.VotingCandidate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DisplayOrder");

                    b.Property<Guid>("SongId");

                    b.HasKey("Id");

                    b.HasIndex("SongId");

                    b.ToTable("VotingCandidate");
                });

            modelBuilder.Entity("Radio.Core.Domain.MasterData.Model.Image", b =>
                {
                    b.HasOne("Radio.Core.Domain.MasterData.Model.File", "File")
                        .WithMany()
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Radio.Core.Domain.MasterData.Model.Song", b =>
                {
                    b.HasOne("Radio.Core.Domain.MasterData.Model.Image", "CoverImage")
                        .WithMany()
                        .HasForeignKey("CoverImageId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("Radio.Core.Domain.Playback.Model.CurrentSong", b =>
                {
                    b.HasOne("Radio.Core.Domain.MasterData.Model.Song", "Song")
                        .WithMany()
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Radio.Core.Domain.Voting.Model.Vote", b =>
                {
                    b.HasOne("Radio.Core.Domain.Voting.Model.VotingCandidate", "VotingCandidate")
                        .WithMany("Votes")
                        .HasForeignKey("VotingCandidateId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Radio.Core.Domain.Voting.Model.VotingCandidate", b =>
                {
                    b.HasOne("Radio.Core.Domain.MasterData.Model.Song", "Song")
                        .WithMany()
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
