using KHCN.Data.DTOs.System;
using System;
using System.ComponentModel.DataAnnotations;

namespace KHCN.Data.DTOs.KHCN
{
    public class KHCN_PhongBan_DTO : BaseEntity_DTO
    {
        [Display(Name = "Tên phòng ban")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Đơn vị cha")]
        public Nullable<int> DonViChaId { get; set; }

        [Display(Name = "Đơn vị cha")]
        public string DonViCha { get; set; }

        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; }
    }
}