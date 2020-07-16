using KHCN.Data.Entities.System;
using System;
using System.ComponentModel.DataAnnotations;

namespace KHCN.Data.Entities.KHCN
{
    public class KHCN_KinhPhiThucHien : BaseEntity
    {
        [Display(Name = "Mã số nhiệm vụ")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string MaSoNhiemVu { get; set; }

        [Display(Name = "Tên nhiệm vụ")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string TenNhiemVu { get; set; }

        [Display(Name = "Dự toán được duyệt (VNĐ)")]
        public Nullable<decimal> DuToanDuocDuyet { get; set; }

        [Display(Name = "Dự toán điều chỉnh (VNĐ)")]
        public Nullable<decimal> DuToanDieuChinh { get; set; }

        [Display(Name = "Tiến trình điều chỉnh")]
        public int TienTrinhDieuChinh { get; set; }

        [Display(Name = "Giá trị quyết toán (VNĐ)")]
        public Nullable<decimal> GiaTriQuyetToan { get; set; }

        [Display(Name = "Nguồn kinh phí")]
        public string NguonKinhPhi { get; set; }
    }
}