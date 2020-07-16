using KHCN.Data.Entities.System;
using System;
using System.ComponentModel.DataAnnotations;

namespace KHCN.Data.Entities.KHCN
{
    public class KHCN_ThoiGianThucHien : BaseEntity
    {
        [Display(Name = "Mã số nhiệm vụ")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string MaSoNhiemVu { get; set; }

        [Display(Name = "Tên nhiệm vụ")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string TenNhiemVu { get; set; }

        [Display(Name = "Thời gian bắt đầu")]
        [Required(ErrorMessage = "Bắt buộc")]
        public DateTime TGBatDau { get; set; }

        [Display(Name = "Thời gian kết thúc")]
        [Required(ErrorMessage = "Bắt buộc")]
        public DateTime TGKetThuc { get; set; }

        [Display(Name = "Thời gian gia hạn mới nhất")]
        [Required(ErrorMessage = "Bắt buộc")]
        public DateTime TGGiaHanMoiNhat { get; set; }

        [Display(Name = "Tiến trình gia hạn")]
        public int TienTrinhGiaHan { get; set; }

        [Display(Name = "Tổng thời gian thực hiện (tháng)")]
        [Required(ErrorMessage = "Bắt buộc")]
        public decimal TongTGThucHien { get; set; }
    }
}