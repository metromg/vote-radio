using Microsoft.EntityFrameworkCore.Migrations;

namespace Radio.Infrastructure.DbAccess.Migrations
{
    public partial class VotingCandidateActivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "VotingCandidate",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "VotingCandidate");
        }
    }
}
