using Microsoft.EntityFrameworkCore.Migrations;

namespace KHCN.Data.Migrations
{
    public partial class _14071 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "KHCN_TaiLieuDinhKemDel",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "KHCN_TaiLieuDinhKem",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Guid",
                table: "KHCN_TaiLieuDinhKemDel");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "KHCN_TaiLieuDinhKem");
        }
    }
}
