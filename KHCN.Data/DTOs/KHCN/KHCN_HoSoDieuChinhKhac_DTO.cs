using KHCN.Data.DTOs.System;
using System;
using System.ComponentModel.DataAnnotations;

namespace KHCN.Data.DTOs.KHCN
{
    public class KHCN_HoSoDieuChinhKhac_DTO : BaseEntity_DTO
    {
        [Display(Name = "Mã số nhiệm vụ")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string MaSoNhiemVu { get; set; }

        [Display(Name = "Tên nhiệm vụ")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string TenNhiemVu { get; set; }

        [Display(Name = "Tờ trình xin phê duyệt điều chỉnh")]
        public string ToTrinhXinPDDieuChinh { get; set; }

        [Display(Name = "Ngày tháng")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string NgayLapToTrinhXinPDDieuChinh { get; set; }

        [Display(Name = "Quyết định phê duyệt điều chỉnh")]
        public string QDPheDuyetDieuChinh { get; set; }

        [Display(Name = "Ngày tháng")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string NgayQDPheDuyetDieuChinh { get; set; }
    }
}