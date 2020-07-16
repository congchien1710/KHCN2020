using KHCN.Data.DTOs.System;
using System;
using System.ComponentModel.DataAnnotations;

namespace KHCN.Data.DTOs.KHCN
{
    public class KHCN_KinhPhiThucHien_DTO : BaseEntity_DTO
    {
        [Display(Name = "Mã số nhiệm vụ")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string MaSoNhiemVu { get; set; }

        [Display(Name = "Tên nhiệm vụ")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string TenNhiemVu { get; set; }

        [Display(Name = "Dự toán được duyệt (VNĐ)")]
        [Required(ErrorMessage = "Bắt buộc")]
        public decimal DuToanDuocDuyet { get; set; }

        [Display(Name = "Dự toán điều chỉnh (VNĐ)")]
        public Nullable<decimal> DuToanDieuChinh { get; set; }

        [Display(Name = "Tiến trình điều chỉnh")]
        public Nullable<int> TienTrinhDieuChinh { get; set; }

        [Display(Name = "Giá trị quyết toán (VNĐ)")]
        [Required(ErrorMessage = "Bắt buộc")]
        public decimal GiaTriQuyetToan { get; set; }

        [Display(Name = "Nguồn kinh phí")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string NguonKinhPhi { get; set; }
    }
}