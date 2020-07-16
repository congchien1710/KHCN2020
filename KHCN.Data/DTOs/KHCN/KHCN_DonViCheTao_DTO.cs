using KHCN.Data.DTOs.System;
using System.ComponentModel.DataAnnotations;

namespace KHCN.Data.DTOs.KHCN
{
    public class KHCN_DonViCheTao_DTO : BaseEntity_DTO
    {
        [Display(Name = "Mã đơn vị")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string Code { get; set; }

        [Display(Name = "Tên đơn vị")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; }
    }
}