using Microsoft.EntityFrameworkCore.Migrations;

namespace KHCN.Data.Migrations
{
    public partial class fix1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MoTa",
                table: "KHCN_TienDoThucHien");

            migrationBuilder.DropColumn(
                name: "Ten",
                table: "KHCN_TienDoThucHien");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "KHCN_TienDoThucHien",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "KHCN_TienDoThucHien",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "KHCN_TienDoThucHien");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "KHCN_TienDoThucHien");

            migrationBuilder.AddColumn<string>(
                name: "MoTa",
                table: "KHCN_TienDoThucHien",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ten",
                table: "KHCN_TienDoThucHien",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
