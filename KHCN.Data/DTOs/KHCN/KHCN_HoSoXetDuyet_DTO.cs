using KHCN.Data.DTOs.System;
using System;
using System.ComponentModel.DataAnnotations;

namespace KHCN.Data.DTOs.KHCN
{
    public class KHCN_HoSoXetDuyet_DTO : BaseEntity_DTO
    {
        [Display(Name = "Mã số nhiệm vụ")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string MaSoNhiemVu { get; set; }

        [Display(Name = "Tên nhiệm vụ")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string TenNhiemVu { get; set; }

        [Display(Name = "Phiếu đăng ký")]
        public string PhieuDangKy { get; set; }

        [Display(Name = "Ngày tháng")]
        [Required(ErrorMessage = "Bắt buộc")]
        public Nullable<DateTime> NgayDangKy { get; set; }

        [Display(Name = "Quyết định thành lập HĐXD cấp Viện")]
        public string QDLapHDXDCapVien { get; set; }

        [Display(Name = "Ngày tháng")]
        [Required(ErrorMessage = "Bắt buộc")]
        public Nullable<DateTime> NgayQDLapHDXDCapVien { get; set; }

        [Display(Name = "Biên bản họp HĐXD cấp Viện")]
        public string BBHopHDXDCapVien { get; set; }

        [Display(Name = "Ngày tháng")]
        [Required(ErrorMessage = "Bắt buộc")]
        public Nullable<DateTime> NgayHopHDXDCapVien { get; set; }

        [Display(Name = "Quyết định Thành lập HĐXD cấp Tập đoàn")]
        public string QDLapHDXDCapTapDoan { get; set; }

        [Display(Name = "Ngày tháng")]
        [Required(ErrorMessage = "Bắt buộc")]
        public Nullable<DateTime> NgayQDLapHDXDCapTapDoan { get; set; }

        [Display(Name = "Biên bản họp HĐXD cấp Tập đoàn")]
        public string BBHopHDXDCapTapDoan { get; set; }

        [Display(Name = "Ngày tháng")]
        [Required(ErrorMessage = "Bắt buộc")]
        public Nullable<DateTime> NgayHopHDXDCapTapDoan { get; set; }

        [Display(Name = "Tờ trình xin phê duyệt chủ trương")]
        public string ToTrinhXinPDChuTruong { get; set; }

        [Display(Name = "Ngày tháng")]
        [Required(ErrorMessage = "Bắt buộc")]
        public Nullable<DateTime> NgayTrinhXinPDChuTruong { get; set; }

        [Display(Name = "Tờ trình xin phê duyệt nhiệm vụ")]
        public string ToTrinhXinPDNhiemVu { get; set; }

        [Display(Name = "Ngày tháng")]
        [Required(ErrorMessage = "Bắt buộc")]
        public Nullable<DateTime> NgayTrinhXinPDNhiemVu { get; set; }

        [Display(Name = "Quyết định phê duyệt")]
        public string QDPheDuyet { get; set; }

        [Display(Name = "Ngày tháng")]
        [Required(ErrorMessage = "Bắt buộc")]
        public Nullable<DateTime> NgayQDPheDuyet { get; set; }

        [Display(Name = "Quyết định giao nhiệm vụ")]
        public string QDGiaoNhiemVu { get; set; }

        [Display(Name = "Ngày tháng")]
        [Required(ErrorMessage = "Bắt buộc")]
        public Nullable<DateTime> NgayQDGiaoNhiemVu { get; set; }

        [Display(Name = "Thuyết minh nhiệm vụ")]
        public string ThuyetMinhNhiemVu { get; set; }

        [Display(Name = "Ngày tháng")]
        [Required(ErrorMessage = "Bắt buộc")]
        public Nullable<DateTime> NgayThuyetMinhNhiemVu { get; set; }

        [Display(Name = "Hồ sơ dự toán")]
        public string HoSoDuToan { get; set; }

        [Display(Name = "Ngày tháng")]
        [Required(ErrorMessage = "Bắt buộc")]
        public Nullable<DateTime> NgayLapHoSoDuToan { get; set; }

        [Display(Name = "Tài liệu MRD")]
        public string TaiLieuMRD { get; set; }

        [Display(Name = "Ngày tháng")]
        [Required(ErrorMessage = "Bắt buộc")]
        public Nullable<DateTime> NgayLapTaiLieuMRD { get; set; }

        [Display(Name = "Tài liệu PRD")]
        public string TaiLieuPRD { get; set; }

        [Display(Name = "Ngày tháng")]
        [Required(ErrorMessage = "Bắt buộc")]
        public Nullable<DateTime> NgayLapTaiLieuPRD { get; set; }
    }
}