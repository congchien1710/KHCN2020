using KHCN.Data.Entities.System;
using System;
using System.ComponentModel.DataAnnotations;

namespace KHCN.Data.Entities.KHCN
{
    public class KHCN_HoSoDieuChinhKhac : BaseEntity
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
        public Nullable<DateTime> NgayLapToTrinhXinPDDieuChinh { get; set; }

        [Display(Name = "Quyết định phê duyệt điều chỉnh")]
        public string QDPheDuyetDieuChinh { get; set; }

        [Display(Name = "Ngày tháng")]
        public Nullable<DateTime> NgayQDPheDuyetDieuChinh { get; set; }
    }
}