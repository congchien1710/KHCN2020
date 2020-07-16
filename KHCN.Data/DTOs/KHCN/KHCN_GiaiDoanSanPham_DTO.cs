using KHCN.Data.DTOs.System;
using System;
using System.ComponentModel.DataAnnotations;

namespace KHCN.Data.DTOs.KHCN
{
    public class KHCN_GiaiDoanSanPham_DTO : BaseEntity_DTO
    {
        [Display(Name = "Loại giai đoạn")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string MaGiaiDoan { get; set; }

        [Display(Name = "Tên giai đoạn")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string TenGiaiDoan { get; set; }

        [Display(Name = "Thời gian bắt đầu")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string ThoiGianBatDau { get; set; }

        [Display(Name = "Thời gian kết thúc")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string ThoiGianKetThuc { get; set; }

        [Display(Name = "Số lượng")]
        [Required(ErrorMessage = "Bắt buộc")]
        public int SoLuong { get; set; }

        [Display(Name = "Chi phí NVL (BOM)")]
        [Required(ErrorMessage = "Bắt buộc")]
        public decimal ChiPhiNVL { get; set; }

        [Display(Name = "Mã số nhiệm vụ")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string MaSoNhiemVu { get; set; }

        [Display(Name = "Tên nhiệm vụ")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string TenNhiemVu { get; set; }
    }
}