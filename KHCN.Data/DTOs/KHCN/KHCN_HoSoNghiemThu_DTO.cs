using KHCN.Data.DTOs.System;
using System;
using System.ComponentModel.DataAnnotations;

namespace KHCN.Data.DTOs.KHCN
{
    public class KHCN_HoSoNghiemThu_DTO : BaseEntity_DTO
    {
        [Display(Name = "Mã số nhiệm vụ")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string MaSoNhiemVu { get; set; }

        [Display(Name = "Tên nhiệm vụ")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string TenNhiemVu { get; set; }

        [Display(Name = "Phiếu đề nghị nghiệm thu cấp Viện")]
        public string PhieuDNNghiemThuCapVien { get; set; }

        [Display(Name = "Ngày tháng")]
        [Required(ErrorMessage = "Bắt buộc")]
        public Nullable<DateTime> NgayLapPhieuDNNghiemThuCapVien { get; set; }

        [Display(Name = "Quyết định thành lập HĐNT cấp Viện")]
        public string QDLapHDNTCapVien { get; set; }

        [Display(Name = "Ngày tháng")]
        [Required(ErrorMessage = "Bắt buộc")]
        public Nullable<DateTime> NgayQDLapHDNTCapVien { get; set; }

        [Display(Name = "Biên bản HĐNT cấp Viện")]
        public string BBHopHDNTCapVien { get; set; }

        [Display(Name = "Ngày tháng")]
        [Required(ErrorMessage = "Bắt buộc")]
        public Nullable<DateTime> NgayHopHDXDCapVien { get; set; }

        [Display(Name = "Công văn đề nghị nghiệm thu cấp Tập đoàn")]
        public string CongVanDNNTCapTapDoan { get; set; }

        [Display(Name = "Ngày tháng")]
        [Required(ErrorMessage = "Bắt buộc")]
        public Nullable<DateTime> NgayLapCongVanDNNTCapTapDoan { get; set; }

        [Display(Name = "Quyết định thành lập HĐNT cấp Tập đoàn")]
        public string QDLapHDNTCapTapDoan { get; set; }

        [Display(Name = "Ngày tháng")]
        [Required(ErrorMessage = "Bắt buộc")]
        public Nullable<DateTime> NgayQDLapHDNTCapTapDoan { get; set; }

        [Display(Name = "Biên bản HĐNT cấp Tập đoàn")]
        public string BBHopHDNTCapTapDoan { get; set; }

        [Display(Name = "Ngày tháng")]
        [Required(ErrorMessage = "Bắt buộc")]
        public Nullable<DateTime> NgayHopHDNTCapTapDoan { get; set; }

        [Display(Name = "Quyết định công nhận kết quả")]
        public string QDCongNhanKetQua { get; set; }

        [Display(Name = "Ngày tháng")]
        [Required(ErrorMessage = "Bắt buộc")]
        public Nullable<DateTime> NgayQDCongNhanKetQua { get; set; }
    }
}