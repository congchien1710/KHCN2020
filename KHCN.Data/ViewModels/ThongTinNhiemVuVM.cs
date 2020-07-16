using System;

namespace KHCN.Data.ViewModels
{
    public class ThongTinNhiemVuVM
    {
        public int IdNhiemVu { get; set; }

        public string MaSoNhiemVu { get; set; }

        public string TenNhiemVu { get; set; }

        public int IdLoaiNhiemVu { get; set; }

        public string LoaiNhiemVu { get; set; }

        public int IdNganh { get; set; }

        public string TenNganh { get; set; }

        public int IdCapQuanLy { get; set; }

        public string CapQuanLy { get; set; }

        public int IdDonViChuTri { get; set; }

        public string DonViChuTri { get; set; }

        public int NamPheDuyet { get; set; }

        public int NamHoanThanh { get; set; }

        public int IdChuNhiemNV { get; set; }

        public string ChuNhiemNV { get; set; }

        public string EmailChuNhiem { get; set; }

        public int IdTienDoThucHien { get; set; }

        public string TienDoThucHien { get; set; }

        public DateTime TGBatDau { get; set; }

        public DateTime TGKetThuc { get; set; }

        public DateTime TGGiaHanMoiNhat { get; set; }

        public int TienTrinhGiaHan { get; set; }

        public decimal TongTGThucHien { get; set; }

        public Nullable<decimal> DuToanDuocDuyet { get; set; }

        public Nullable<decimal> DuToanDieuChinh { get; set; }

        public int TienTrinhDieuChinh { get; set; }

        public Nullable<decimal> GiaTriQuyetToan { get; set; }

        public string NguonKinhPhi { get; set; }
    }
}