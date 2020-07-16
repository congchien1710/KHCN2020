using KHCN.Data.Entities.System;
using System;
using System.ComponentModel.DataAnnotations;

namespace KHCN.Data.Entities.KHCN
{
    public class KHCN_GiaiDoanSanPham : BaseEntity
    {
        [Display(Name = "Loại giai đoạn")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string MaGiaiDoan { get; set; }

        [Display(Name = "Tên giai đoạn")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string TenGiaiDoan { get; set; }

        [Display(Name = "Thời gian bắt đầu")]
        [Required(ErrorMessage = "Bắt buộc")]
        public DateTime ThoiGianBatDau { get; set; }

        [Display(Name = "Thời gian kết thúc")]
        [Required(ErrorMessage = "Bắt buộc")]
        public DateTime ThoiGianKetThuc { get; set; }

        [Display(Name = "Số lượng")]
        public int SoLuong { get; set; }

        [Display(Name = "Chi phí NVL (BOM)")]
        public decimal ChiPhiNVL { get; set; }

        [Display(Name = "Mã số nhiệm vụ")]
        public string MaSoNhiemVu { get; set; }

        [Display(Name = "Tên nhiệm vụ")]
        public string TenNhiemVu { get; set; }
    }
}