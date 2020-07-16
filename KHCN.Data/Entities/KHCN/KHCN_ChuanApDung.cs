using KHCN.Data.Entities.System;
using System.ComponentModel.DataAnnotations;

namespace KHCN.Data.Entities.KHCN
{
    public class KHCN_ChuanApDung : BaseEntity
    {
        [Display(Name = "Tên chuẩn")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; }
    }
}