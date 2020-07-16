using KHCN.Data.Entities.System;
using System;
using System.ComponentModel.DataAnnotations;

namespace KHCN.Data.Entities.KHCN
{
    public class KHCN_HoSoQuyetToan : BaseEntity
    {
        [Display(Name = "Mã số nhiệm vụ")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string MaSoNhiemVu { get; set; }

        [Display(Name = "Tên nhiệm vụ")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string TenNhiemVu { get; set; }

        [Display(Name = "Phiếu nhập kho sản phẩm")]
        public string PhieuNhapKhoSP { get; set; }

        [Display(Name = "Ngày tháng")]
        public Nullable<DateTime> NgayLapPhieuNhapKhoSP { get; set; }

        [Display(Name = "Quyết định phê duyệt Hồ sơ quyết toán")]
        public string QDPheDuyetHSQT { get; set; }

        [Display(Name = "Ngày tháng")]
        public Nullable<DateTime> NgayQDPheDuyetHSQT { get; set; }
    }
}