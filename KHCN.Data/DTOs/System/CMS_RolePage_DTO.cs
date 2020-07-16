using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KHCN.Data.DTOs.System
{
    public class CMS_RolePage_DTO : BaseEntity_DTO
    {
        [Display(Name = "Tên trang")]
        public int IdPage { get; set; }

        [Display(Name = "Tên trang")]
        public string NamePage { get; set; }

        [Display(Name = "Tên nhóm người dùng")]
        public int IdRole { get; set; }

        [Display(Name = "Tên nhóm người dùng")]
        public string NameRole { get; set; }
    }
}