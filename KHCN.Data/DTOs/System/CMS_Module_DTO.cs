using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KHCN.Data.DTOs.System
{
    public class CMS_Module_DTO : BaseEntity_DTO
    {
        [Display(Name = "Tên module")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; }
    }
}