using KHCN.Data.DTOs.System;
using System.ComponentModel.DataAnnotations;

namespace KHCN.Data.DTOs.KHCN
{
    public class KHCN_GiaiDoan_DTO : BaseEntity_DTO
    {
        [Display(Name = "Mã giai đoạn")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string Code { get; set; }

        [Display(Name = "Tên giai đoạn")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; }
    }
}