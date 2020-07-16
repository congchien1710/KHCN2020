using KHCN.Data.Entities.KHCN;
using KHCN.Data.Entities.System;
using Microsoft.EntityFrameworkCore;

namespace KHCN.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        //public AppDbContext()
        //{
        //}

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseNpgsql(_connString);
        //}

        //private readonly string _connString = "User ID =postgres;Password=Admin@2020;Server=localhost;Port=5432;Database=KHCN;Integrated Security=true;Pooling=true;";

        public DbSet<CMS_User> CMS_User { get; set; }
        public DbSet<CMS_Role> CMS_Role { get; set; }
        public DbSet<CMS_UserRole> CMS_UserRole { get; set; }
        public DbSet<CMS_Function> CMS_Function { get; set; }
        public DbSet<CMS_Api> CMS_Api { get; set; }
        public DbSet<CMS_RoleFunction> CMS_RoleFunction { get; set; }
        public DbSet<CMS_RoleApi> CMS_RoleApi { get; set; }
        public DbSet<CMS_Page> CMS_Page { get; set; }
        public DbSet<CMS_RolePage> CMS_RolePage { get; set; }
        public DbSet<CMS_Module> CMS_Module { get; set; }


        public DbSet<KHCN_CapQuanLy> KHCN_CapQuanLy { get; set; }
        public DbSet<KHCN_NhiemVu> KHCN_NhiemVu { get; set; }
        public DbSet<KHCN_HoSoDieuChinhThoiGian> KHCN_HoSoDieuChinhThoiGian { get; set; }
        public DbSet<KHCN_HoSoNghiemThu> KHCN_HoSoNghiemThu { get; set; }
        public DbSet<KHCN_HoSoQuyetToan> KHCN_HoSoQuyetToan { get; set; }
        public DbSet<KHCN_HoSoXetDuyet> KHCN_HoSoXetDuyet { get; set; }
        public DbSet<KHCN_KinhPhiThucHien> KHCN_KinhPhiThucHien { get; set; }
        public DbSet<KHCN_LoaiNhiemVu> KHCN_LoaiNhiemVu { get; set; }
        public DbSet<KHCN_Nganh> KHCN_Nganh { get; set; }
        public DbSet<KHCN_ThoiGianThucHien> KHCN_ThoiGianThucHien { get; set; }
        public DbSet<KHCN_GiaiDoan> KHCN_GiaiDoan { get; set; }
        public DbSet<KHCN_TienDoThucHien> KHCN_TienDoThucHien { get; set; }
        public DbSet<KHCN_DonViCheTao> KHCN_DonViCheTao { get; set; }
        public DbSet<KHCN_ThanhVienDeTai> KHCN_ThanhVienDeTai { get; set; }
        public DbSet<KHCN_CapBac> KHCN_CapBac { get; set; }
        public DbSet<KHCN_ChucDanh> KHCN_ChucDanh { get; set; }
        public DbSet<KHCN_TrinhDo> KHCN_TrinhDo { get; set; }
        public DbSet<KHCN_PhongBan> KHCN_PhongBan { get; set; }
        public DbSet<KHCN_ChuanApDung> KHCN_ChuanApDung { get; set; }
        public DbSet<KHCN_HoSoDieuChinhKhac> KHCN_HoSoDieuChinhKhac { get; set; }
        public DbSet<KHCN_HoSoDieuChinhKinhPhi> KHCN_HoSoDieuChinhKinhPhi { get; set; }
        public DbSet<KHCN_GiaiDoanSanPham> KHCN_GiaiDoanSanPham { get; set; }
        public DbSet<KHCN_SanPham> KHCN_SanPham { get; set; }
        public DbSet<KHCN_TaiLieuDinhKem> KHCN_TaiLieuDinhKem { get; set; }
        public DbSet<KHCN_TaiLieuDinhKemDel> KHCN_TaiLieuDinhKemDel { get; set; }

    }
}
