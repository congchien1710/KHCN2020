using KHCN.Data.DTOs.System;
using System;
using System.ComponentModel.DataAnnotations;

namespace KHCN.Data.DTOs.KHCN
{
    public class KHCN_ThoiGianThucHien_DTO : BaseEntity_DTO
    {
        [Display(Name = "Mã số nhiệm vụ")]
        [MinLength(1)]
        [Required(ErrorMessage = "Bắt buộc")]
        public string MaSoNhiemVu { get; set; }

        [Display(Name = "Tên nhiệm vụ")]
        [MinLength(1)]
        [Required(ErrorMessage = "Bắt buộc")]
        public string TenNhiemVu { get; set; }

        [Display(Name = "Thời gian bắt đầu")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string TGBatDau { get; set; }

        [Display(Name = "Thời gian kết thúc")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string TGKetThuc { get; set; }

        [Display(Name = "Thời gian gia hạn mới nhất")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string TGGiaHanMoiNhat { get; set; }

        [Display(Name = "Tiến trình gia hạn")]
        public int? TienTrinhGiaHan { get; set; }

        [Display(Name = "Tổng thời gian thực hiện (tháng)")]
        [Required(ErrorMessage = "Bắt buộc")]
        public decimal TongTGThucHien { get; set; }
    }
}