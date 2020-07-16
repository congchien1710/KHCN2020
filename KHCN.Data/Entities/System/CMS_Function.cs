using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KHCN.Data.Entities.System
{
    [Table("CMS_Function")]
    public class CMS_Function : BaseEntity
    {
        [Display(Name = "Tên chức năng")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Chức năng cha")]
        public Nullable<int> IdParent { get; set; }

        [Display(Name = "Module")]
        [Required(ErrorMessage = "Bắt buộc")]
        public Nullable<int> IdModule { get; set; }

        [Display(Name = "Controller")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string Controller { get; set; }

        [Display(Name = "Action")]
        public string Action { get; set; }

        [Display(Name = "Controller-Action")]
        public string ControllerAction { get; set; }

        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; }
    }
}