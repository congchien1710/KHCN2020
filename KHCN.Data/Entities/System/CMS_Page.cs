using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KHCN.Data.Entities.System
{
    [Table("CMS_Page")]
    public class CMS_Page : BaseEntity
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

        [Display(Name = "Module")]
        [Required(ErrorMessage = "Bắt buộc")]
        public Nullable<int> IdModule { get; set; }

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