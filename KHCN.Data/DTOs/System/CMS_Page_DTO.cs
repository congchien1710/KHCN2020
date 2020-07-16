using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KHCN.Data.DTOs.System
{
    public class CMS_Page_DTO : BaseEntity_DTO
    {
        [Display(Name = "Tên page")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Key")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string Key { get; set; }

        [Display(Name = "Page cha")]
        public Nullable<int> IdParent { get; set; }

        [Display(Name = "Page cha")]
        public string NameParent { get; set; }

        [Display(Name = "Module")]
        [Required(ErrorMessage = "Bắt buộc")]
        public Nullable<int> IdModule { get; set; }

        [Display(Name = "Module")]
        public string NameModule { get; set; }

        [Display(Name = "Icon")]
        public string Icon { get; set; }

        [Display(Name = "Thứ tự")]
        public Nullable<int> OrderHint { get; set; }

        [Display(Name = "Trang quản trị")]
        public bool IsAdmin { get; set; }

        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; }
    }
}