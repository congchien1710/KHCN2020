using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace KHCN.Data.Migrations
{
    public partial class _2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CMS_Function",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IdParent = table.Column<int>(nullable: true),
                    IdModule = table.Column<int>(nullable: false),
                    Controller = table.Column<string>(nullable: false),
                    Action = table.Column<string>(nullable: true),
                    ControllerAction = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsApi = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMS_Function", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CMS_Module",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMS_Module", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CMS_Page",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Key = table.Column<string>(nullable: false),
                    IdParent = table.Column<int>(nullable: true),
                    IdModule = table.Column<int>(nullable: false),
                    Icon = table.Column<string>(nullable: true),
                    OrderHint = table.Column<int>(nullable: true),
                    IsAdmin = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMS_Page", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CMS_Role",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsAdmin = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMS_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CMS_RoleFunction",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IdFunction = table.Column<int>(nullable: false),
                    IdRole = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMS_RoleFunction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CMS_RolePage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IdPage = table.Column<int>(nullable: false),
                    IdRole = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMS_RolePage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CMS_User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    FullName = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: false),
                    Mobile = table.Column<string>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    ActiveCode = table.Column<string>(nullable: true),
                    RefreshToken = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMS_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CMS_UserRole",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IdUser = table.Column<int>(nullable: false),
                    IdRole = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMS_UserRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KHCN_CapBac",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHCN_CapBac", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KHCN_CapQuanLy",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHCN_CapQuanLy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KHCN_ChuanApDung",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHCN_ChuanApDung", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KHCN_ChucDanh",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHCN_ChucDanh", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KHCN_DonViCheTao",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHCN_DonViCheTao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KHCN_GiaiDoan",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHCN_GiaiDoan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KHCN_GiaiDoanSanPham",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    MaGiaiDoan = table.Column<string>(nullable: false),
                    TenGiaiDoan = table.Column<string>(nullable: true),
                    ThoiGianBatDau = table.Column<DateTime>(nullable: false),
                    ThoiGianKetThuc = table.Column<DateTime>(nullable: false),
                    SoLuong = table.Column<int>(nullable: false),
                    ChiPhiNVL = table.Column<decimal>(nullable: false),
                    MaSoNhiemVu = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHCN_GiaiDoanSanPham", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KHCN_HoSoDieuChinhKhac",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    MaSoNhiemVu = table.Column<string>(nullable: true),
                    TenNhiemVu = table.Column<string>(nullable: true),
                    ToTrinhXinPDDieuChinh = table.Column<string>(nullable: true),
                    NgayLapToTrinhXinPDDieuChinh = table.Column<DateTime>(nullable: true),
                    QDPheDuyetDieuChinh = table.Column<string>(nullable: true),
                    NgayQDPheDuyetDieuChinh = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHCN_HoSoDieuChinhKhac", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KHCN_HoSoDieuChinhKinhPhi",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    MaSoNhiemVu = table.Column<string>(nullable: true),
                    TenNhiemVu = table.Column<string>(nullable: true),
                    ToTrinhXinPDDieuChinh = table.Column<string>(nullable: true),
                    NgayLapToTrinhXinPDDieuChinh = table.Column<DateTime>(nullable: true),
                    KinhPhiSauKhiDieuChinh = table.Column<decimal>(nullable: false),
                    QDPheDuyetDieuChinh = table.Column<string>(nullable: true),
                    NgayQDPheDuyetDieuChinh = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHCN_HoSoDieuChinhKinhPhi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KHCN_HoSoDieuChinhThoiGian",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    MaSoNhiemVu = table.Column<string>(nullable: true),
                    TenNhiemVu = table.Column<string>(nullable: true),
                    ToTrinhXinPDDieuChinh = table.Column<string>(nullable: true),
                    NgayLapToTrinhXinPDDieuChinh = table.Column<DateTime>(nullable: true),
                    QDPheDuyetDieuChinh = table.Column<string>(nullable: true),
                    NgayQDPheDuyetDieuChinh = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHCN_HoSoDieuChinhThoiGian", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KHCN_HoSoNghiemThu",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    MaSoNhiemVu = table.Column<string>(nullable: true),
                    TenNhiemVu = table.Column<string>(nullable: true),
                    PhieuDNNghiemThuCapVien = table.Column<string>(nullable: true),
                    NgayLapPhieuDNNghiemThuCapVien = table.Column<DateTime>(nullable: true),
                    QDLapHDNTCapVien = table.Column<string>(nullable: true),
                    NgayQDLapHDNTCapVien = table.Column<DateTime>(nullable: true),
                    BBHopHDNTCapVien = table.Column<string>(nullable: true),
                    NgayHopHDXDCapVien = table.Column<DateTime>(nullable: true),
                    CongVanDNNTCapTapDoan = table.Column<string>(nullable: true),
                    NgayLapCongVanDNNTCapTapDoan = table.Column<DateTime>(nullable: true),
                    QDLapHDNTCapTapDoan = table.Column<string>(nullable: true),
                    NgayQDLapHDNTCapTapDoan = table.Column<DateTime>(nullable: true),
                    BBHopHDNTCapTapDoan = table.Column<string>(nullable: true),
                    NgayHopHDNTCapTapDoan = table.Column<DateTime>(nullable: true),
                    QDCongNhanKetQua = table.Column<string>(nullable: true),
                    NgayQDCongNhanKetQua = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHCN_HoSoNghiemThu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KHCN_HoSoQuyetToan",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    MaSoNhiemVu = table.Column<string>(nullable: true),
                    TenNhiemVu = table.Column<string>(nullable: true),
                    PhieuNhapKhoSP = table.Column<string>(nullable: true),
                    NgayLapPhieuNhapKhoSP = table.Column<DateTime>(nullable: true),
                    QDPheDuyetHSQT = table.Column<string>(nullable: true),
                    NgayQDPheDuyetHSQT = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHCN_HoSoQuyetToan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KHCN_HoSoXetDuyet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    MaSoNhiemVu = table.Column<string>(nullable: true),
                    TenNhiemVu = table.Column<string>(nullable: true),
                    PhieuDangKy = table.Column<string>(nullable: true),
                    NgayDangKy = table.Column<DateTime>(nullable: true),
                    QDLapHDXDCapVien = table.Column<string>(nullable: true),
                    NgayQDLapHDXDCapVien = table.Column<DateTime>(nullable: true),
                    BBHopHDXDCapVien = table.Column<string>(nullable: true),
                    NgayHopHDXDCapVien = table.Column<DateTime>(nullable: true),
                    QDLapHDXDCapTapDoan = table.Column<string>(nullable: true),
                    NgayQDLapHDXDCapTapDoan = table.Column<DateTime>(nullable: true),
                    BBHopHDXDCapTapDoan = table.Column<string>(nullable: true),
                    NgayHopHDXDCapTapDoan = table.Column<DateTime>(nullable: true),
                    ToTrinhXinPDChuTruong = table.Column<string>(nullable: true),
                    NgayTrinhXinPDChuTruong = table.Column<DateTime>(nullable: true),
                    ToTrinhXinPDNhiemVu = table.Column<string>(nullable: true),
                    NgayTrinhXinPDNhiemVu = table.Column<DateTime>(nullable: true),
                    QDPheDuyet = table.Column<string>(nullable: true),
                    NgayQDPheDuyet = table.Column<DateTime>(nullable: true),
                    QDGiaoNhiemVu = table.Column<string>(nullable: true),
                    NgayQDGiaoNhiemVu = table.Column<DateTime>(nullable: true),
                    ThuyetMinhNhiemVu = table.Column<string>(nullable: true),
                    NgayThuyetMinhNhiemVu = table.Column<DateTime>(nullable: true),
                    HoSoDuToan = table.Column<string>(nullable: true),
                    NgayLapHoSoDuToan = table.Column<DateTime>(nullable: true),
                    TaiLieuMRD = table.Column<string>(nullable: true),
                    NgayLapTaiLieuMRD = table.Column<DateTime>(nullable: true),
                    TaiLieuPRD = table.Column<string>(nullable: true),
                    NgayLapTaiLieuPRD = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHCN_HoSoXetDuyet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KHCN_KinhPhiThucHien",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    MaSoNhiemVu = table.Column<string>(nullable: true),
                    TenNhiemVu = table.Column<string>(nullable: true),
                    DuToanDuocDuyet = table.Column<decimal>(nullable: true),
                    DuToanDieuChinh = table.Column<decimal>(nullable: true),
                    TienTrinhDieuChinh = table.Column<int>(nullable: false),
                    GiaTriQuyetToan = table.Column<decimal>(nullable: true),
                    NguonKinhPhi = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHCN_KinhPhiThucHien", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KHCN_LoaiNhiemVu",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHCN_LoaiNhiemVu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KHCN_Nganh",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHCN_Nganh", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KHCN_NhiemVu",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    MaSoNhiemVu = table.Column<string>(nullable: true),
                    TenNhiemVu = table.Column<string>(nullable: false),
                    IdLoaiNhiemVu = table.Column<int>(nullable: false),
                    LoaiNhiemVu = table.Column<string>(nullable: true),
                    IdNganh = table.Column<int>(nullable: false),
                    TenNganh = table.Column<string>(nullable: true),
                    IdCapQuanLy = table.Column<int>(nullable: false),
                    CapQuanLy = table.Column<string>(nullable: true),
                    IdDonViChuTri = table.Column<int>(nullable: false),
                    DonViChuTri = table.Column<string>(nullable: true),
                    NamPheDuyet = table.Column<int>(nullable: false),
                    NamHoanThanh = table.Column<int>(nullable: false),
                    IdChuNhiemNV = table.Column<int>(nullable: false),
                    ChuNhiemNV = table.Column<string>(nullable: true),
                    EmailChuNhiem = table.Column<string>(nullable: true),
                    IdTienDoThucHien = table.Column<int>(nullable: false),
                    TienDoThucHien = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHCN_NhiemVu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KHCN_PhongBan",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    DonViChaId = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHCN_PhongBan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KHCN_SanPham",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    MaSanPham = table.Column<string>(nullable: false),
                    SerialNumber = table.Column<string>(nullable: true),
                    TenSanPham = table.Column<string>(nullable: false),
                    MaSoNhiemVu = table.Column<string>(nullable: true),
                    TenNhiemVu = table.Column<string>(nullable: true),
                    IdNganh = table.Column<int>(nullable: false),
                    TenNganh = table.Column<string>(nullable: true),
                    IdDonViCheTao = table.Column<int>(nullable: false),
                    DonViCheTao = table.Column<string>(nullable: true),
                    MaGiaiDoan = table.Column<string>(nullable: false),
                    TenGiaiDoan = table.Column<string>(nullable: true),
                    IdChuanApDung = table.Column<int>(nullable: false),
                    ChuanApDung = table.Column<string>(nullable: true),
                    TinhNangChinh = table.Column<string>(nullable: false),
                    SanPhamTuongDuong = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHCN_SanPham", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KHCN_TaiLieuDinhKem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    LoaiTaiLieu = table.Column<int>(nullable: false),
                    MaSoNhiemVu = table.Column<string>(nullable: true),
                    MaSanPham = table.Column<string>(nullable: true),
                    Keyword = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    RelativePath = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    DisplayFileName = table.Column<string>(nullable: true),
                    FileSize = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHCN_TaiLieuDinhKem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KHCN_TaiLieuDinhKemDel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    LoaiTaiLieu = table.Column<int>(nullable: false),
                    MaSoNhiemVu = table.Column<string>(nullable: true),
                    MaSanPham = table.Column<string>(nullable: true),
                    Keyword = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    RelativePath = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    DisplayFileName = table.Column<string>(nullable: true),
                    FileSize = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHCN_TaiLieuDinhKemDel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KHCN_ThanhVienDeTai",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    MaNV = table.Column<string>(nullable: false),
                    HoTen = table.Column<string>(nullable: false),
                    IdDonVi = table.Column<int>(nullable: false),
                    DonVi = table.Column<string>(nullable: true),
                    IdPhongBan = table.Column<int>(nullable: false),
                    PhongBan = table.Column<string>(nullable: true),
                    IdCapBac = table.Column<int>(nullable: false),
                    CapBac = table.Column<string>(nullable: true),
                    IdChucDanh = table.Column<int>(nullable: false),
                    ChucDanh = table.Column<string>(nullable: true),
                    TrinhDo = table.Column<int>(nullable: false),
                    IdTrinhDo = table.Column<string>(nullable: true),
                    NgaySinh = table.Column<DateTime>(nullable: false),
                    NgayKyHD = table.Column<DateTime>(nullable: false),
                    GioiTinh = table.Column<int>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    NhiemVu1 = table.Column<string>(nullable: true),
                    TrangThaiNV1 = table.Column<int>(nullable: false),
                    NhiemVu2 = table.Column<string>(nullable: true),
                    TrangThaiNV2 = table.Column<int>(nullable: false),
                    NhiemVu3 = table.Column<string>(nullable: true),
                    TrangThaiNV3 = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHCN_ThanhVienDeTai", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KHCN_ThoiGianThucHien",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    MaSoNhiemVu = table.Column<string>(nullable: true),
                    TenNhiemVu = table.Column<string>(nullable: true),
                    TGBatDau = table.Column<DateTime>(nullable: false),
                    TGKetThuc = table.Column<DateTime>(nullable: false),
                    TGGiaHanMoiNhat = table.Column<DateTime>(nullable: false),
                    TienTrinhGiaHan = table.Column<int>(nullable: false),
                    TongTGThucHien = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHCN_ThoiGianThucHien", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KHCN_TienDoThucHien",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    Ten = table.Column<string>(nullable: false),
                    MoTa = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHCN_TienDoThucHien", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KHCN_TrinhDo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHCN_TrinhDo", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CMS_Function");

            migrationBuilder.DropTable(
                name: "CMS_Module");

            migrationBuilder.DropTable(
                name: "CMS_Page");

            migrationBuilder.DropTable(
                name: "CMS_Role");

            migrationBuilder.DropTable(
                name: "CMS_RoleFunction");

            migrationBuilder.DropTable(
                name: "CMS_RolePage");

            migrationBuilder.DropTable(
                name: "CMS_User");

            migrationBuilder.DropTable(
                name: "CMS_UserRole");

            migrationBuilder.DropTable(
                name: "KHCN_CapBac");

            migrationBuilder.DropTable(
                name: "KHCN_CapQuanLy");

            migrationBuilder.DropTable(
                name: "KHCN_ChuanApDung");

            migrationBuilder.DropTable(
                name: "KHCN_ChucDanh");

            migrationBuilder.DropTable(
                name: "KHCN_DonViCheTao");

            migrationBuilder.DropTable(
                name: "KHCN_GiaiDoan");

            migrationBuilder.DropTable(
                name: "KHCN_GiaiDoanSanPham");

            migrationBuilder.DropTable(
                name: "KHCN_HoSoDieuChinhKhac");

            migrationBuilder.DropTable(
                name: "KHCN_HoSoDieuChinhKinhPhi");

            migrationBuilder.DropTable(
                name: "KHCN_HoSoDieuChinhThoiGian");

            migrationBuilder.DropTable(
                name: "KHCN_HoSoNghiemThu");

            migrationBuilder.DropTable(
                name: "KHCN_HoSoQuyetToan");

            migrationBuilder.DropTable(
                name: "KHCN_HoSoXetDuyet");

            migrationBuilder.DropTable(
                name: "KHCN_KinhPhiThucHien");

            migrationBuilder.DropTable(
                name: "KHCN_LoaiNhiemVu");

            migrationBuilder.DropTable(
                name: "KHCN_Nganh");

            migrationBuilder.DropTable(
                name: "KHCN_NhiemVu");

            migrationBuilder.DropTable(
                name: "KHCN_PhongBan");

            migrationBuilder.DropTable(
                name: "KHCN_SanPham");

            migrationBuilder.DropTable(
                name: "KHCN_TaiLieuDinhKem");

            migrationBuilder.DropTable(
                name: "KHCN_TaiLieuDinhKemDel");

            migrationBuilder.DropTable(
                name: "KHCN_ThanhVienDeTai");

            migrationBuilder.DropTable(
                name: "KHCN_ThoiGianThucHien");

            migrationBuilder.DropTable(
                name: "KHCN_TienDoThucHien");

            migrationBuilder.DropTable(
                name: "KHCN_TrinhDo");
        }
    }
}
