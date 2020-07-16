using Microsoft.EntityFrameworkCore.Migrations;

namespace KHCN.Data.Migrations
{
    public partial class _12073 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdTrinhDo",
                table: "KHCN_ThanhVienDeTai");

            migrationBuilder.DropColumn(
                name: "TrinhDo",
                table: "KHCN_ThanhVienDeTai");

            migrationBuilder.AlterColumn<string>(
                name: "TenNhiemVu",
                table: "KHCN_ThoiGianThucHien",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaSoNhiemVu",
                table: "KHCN_ThoiGianThucHien",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ChucDanh",
                table: "KHCN_ThanhVienDeTai",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CapBac",
                table: "KHCN_ThanhVienDeTai",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenNhiemVu",
                table: "KHCN_SanPham",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenNganh",
                table: "KHCN_SanPham",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenGiaiDoan",
                table: "KHCN_SanPham",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaSoNhiemVu",
                table: "KHCN_SanPham",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DonViCheTao",
                table: "KHCN_SanPham",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DonViCha",
                table: "KHCN_PhongBan",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenNganh",
                table: "KHCN_NhiemVu",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DonViChuTri",
                table: "KHCN_NhiemVu",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CapQuanLy",
                table: "KHCN_NhiemVu",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenNhiemVu",
                table: "KHCN_KinhPhiThucHien",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NguonKinhPhi",
                table: "KHCN_KinhPhiThucHien",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "MaSoNhiemVu",
                table: "KHCN_KinhPhiThucHien",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenNhiemVu",
                table: "KHCN_HoSoXetDuyet",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaSoNhiemVu",
                table: "KHCN_HoSoXetDuyet",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenNhiemVu",
                table: "KHCN_HoSoQuyetToan",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaSoNhiemVu",
                table: "KHCN_HoSoQuyetToan",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenNhiemVu",
                table: "KHCN_HoSoNghiemThu",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaSoNhiemVu",
                table: "KHCN_HoSoNghiemThu",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenNhiemVu",
                table: "KHCN_HoSoDieuChinhThoiGian",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaSoNhiemVu",
                table: "KHCN_HoSoDieuChinhThoiGian",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenNhiemVu",
                table: "KHCN_HoSoDieuChinhKinhPhi",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaSoNhiemVu",
                table: "KHCN_HoSoDieuChinhKinhPhi",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenNhiemVu",
                table: "KHCN_HoSoDieuChinhKhac",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaSoNhiemVu",
                table: "KHCN_HoSoDieuChinhKhac",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenGiaiDoan",
                table: "KHCN_GiaiDoanSanPham",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenNhiemVu",
                table: "KHCN_GiaiDoanSanPham",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DonViCha",
                table: "KHCN_PhongBan");

            migrationBuilder.DropColumn(
                name: "TenNhiemVu",
                table: "KHCN_GiaiDoanSanPham");

            migrationBuilder.AlterColumn<string>(
                name: "TenNhiemVu",
                table: "KHCN_ThoiGianThucHien",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "MaSoNhiemVu",
                table: "KHCN_ThoiGianThucHien",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ChucDanh",
                table: "KHCN_ThanhVienDeTai",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "CapBac",
                table: "KHCN_ThanhVienDeTai",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "IdTrinhDo",
                table: "KHCN_ThanhVienDeTai",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrinhDo",
                table: "KHCN_ThanhVienDeTai",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "TenNhiemVu",
                table: "KHCN_SanPham",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "TenNganh",
                table: "KHCN_SanPham",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "TenGiaiDoan",
                table: "KHCN_SanPham",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "MaSoNhiemVu",
                table: "KHCN_SanPham",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "DonViCheTao",
                table: "KHCN_SanPham",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "TenNganh",
                table: "KHCN_NhiemVu",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "DonViChuTri",
                table: "KHCN_NhiemVu",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "CapQuanLy",
                table: "KHCN_NhiemVu",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "TenNhiemVu",
                table: "KHCN_KinhPhiThucHien",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "NguonKinhPhi",
                table: "KHCN_KinhPhiThucHien",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaSoNhiemVu",
                table: "KHCN_KinhPhiThucHien",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "TenNhiemVu",
                table: "KHCN_HoSoXetDuyet",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "MaSoNhiemVu",
                table: "KHCN_HoSoXetDuyet",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "TenNhiemVu",
                table: "KHCN_HoSoQuyetToan",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "MaSoNhiemVu",
                table: "KHCN_HoSoQuyetToan",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "TenNhiemVu",
                table: "KHCN_HoSoNghiemThu",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "MaSoNhiemVu",
                table: "KHCN_HoSoNghiemThu",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "TenNhiemVu",
                table: "KHCN_HoSoDieuChinhThoiGian",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "MaSoNhiemVu",
                table: "KHCN_HoSoDieuChinhThoiGian",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "TenNhiemVu",
                table: "KHCN_HoSoDieuChinhKinhPhi",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "MaSoNhiemVu",
                table: "KHCN_HoSoDieuChinhKinhPhi",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "TenNhiemVu",
                table: "KHCN_HoSoDieuChinhKhac",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "MaSoNhiemVu",
                table: "KHCN_HoSoDieuChinhKhac",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "TenGiaiDoan",
                table: "KHCN_GiaiDoanSanPham",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
