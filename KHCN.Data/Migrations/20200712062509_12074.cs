using Microsoft.EntityFrameworkCore.Migrations;

namespace KHCN.Data.Migrations
{
    public partial class _12074 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdTrinhDo",
                table: "KHCN_ThanhVienDeTai",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TrinhDo",
                table: "KHCN_ThanhVienDeTai",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdTrinhDo",
                table: "KHCN_ThanhVienDeTai");

            migrationBuilder.DropColumn(
                name: "TrinhDo",
                table: "KHCN_ThanhVienDeTai");
        }
    }
}
