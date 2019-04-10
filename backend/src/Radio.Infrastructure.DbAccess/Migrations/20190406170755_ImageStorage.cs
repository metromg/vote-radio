using Microsoft.EntityFrameworkCore.Migrations;

namespace Radio.Infrastructure.DbAccess.Migrations
{
    public partial class ImageStorage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ContentLength",
                table: "Image",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ContentLength",
                table: "Image",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}
