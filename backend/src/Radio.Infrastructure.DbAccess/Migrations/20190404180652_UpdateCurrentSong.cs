using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Radio.Infrastructure.DbAccess.Migrations
{
    public partial class UpdateCurrentSong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "EndsAtTime",
                table: "CurrentSong",
                nullable: false,
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndsAtTime",
                table: "CurrentSong",
                nullable: false,
                oldClrType: typeof(DateTimeOffset));
        }
    }
}
