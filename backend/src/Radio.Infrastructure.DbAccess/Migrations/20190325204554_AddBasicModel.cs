using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Radio.Infrastructure.DbAccess.Migrations
{
    public partial class AddBasicModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Album",
                table: "Song",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Artist",
                table: "Song",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CoverImageId",
                table: "Song",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DurationInSeconds",
                table: "Song",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Song",
                maxLength: 1024,
                nullable: false);

            migrationBuilder.CreateTable(
                name: "CurrentSong",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SongId = table.Column<Guid>(nullable: false),
                    VoteCount = table.Column<int>(nullable: false),
                    EndsAtTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentSong", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrentSong_Song_SongId",
                        column: x => x.SongId,
                        principalTable: "Song",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "File",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Data = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VotingCandidate",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SongId = table.Column<Guid>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VotingCandidate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VotingCandidate_Song_SongId",
                        column: x => x.SongId,
                        principalTable: "Song",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ContentType = table.Column<string>(maxLength: 64, nullable: false),
                    ContentLength = table.Column<int>(nullable: false),
                    FileId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Image_File_FileId",
                        column: x => x.FileId,
                        principalTable: "File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vote",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    VotingCandidateId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vote_VotingCandidate_VotingCandidateId",
                        column: x => x.VotingCandidateId,
                        principalTable: "VotingCandidate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Song_CoverImageId",
                table: "Song",
                column: "CoverImageId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrentSong_SongId",
                table: "CurrentSong",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_FileId",
                table: "Image",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Vote_VotingCandidateId",
                table: "Vote",
                column: "VotingCandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_VotingCandidate_SongId",
                table: "VotingCandidate",
                column: "SongId");

            migrationBuilder.AddForeignKey(
                name: "FK_Song_Image_CoverImageId",
                table: "Song",
                column: "CoverImageId",
                principalTable: "Image",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Song_Image_CoverImageId",
                table: "Song");

            migrationBuilder.DropTable(
                name: "CurrentSong");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "Vote");

            migrationBuilder.DropTable(
                name: "File");

            migrationBuilder.DropTable(
                name: "VotingCandidate");

            migrationBuilder.DropIndex(
                name: "IX_Song_CoverImageId",
                table: "Song");

            migrationBuilder.DropColumn(
                name: "Album",
                table: "Song");

            migrationBuilder.DropColumn(
                name: "Artist",
                table: "Song");

            migrationBuilder.DropColumn(
                name: "CoverImageId",
                table: "Song");

            migrationBuilder.DropColumn(
                name: "DurationInSeconds",
                table: "Song");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Song");
        }
    }
}
