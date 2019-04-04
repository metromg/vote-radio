using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Radio.Infrastructure.DbAccess.Migrations
{
    public partial class UserIdentifierOnVotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserIdentifier",
                table: "Vote",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserIdentifier",
                table: "Vote");
        }
    }
}
