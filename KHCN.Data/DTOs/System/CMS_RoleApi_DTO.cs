using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KHCN.Data.DTOs.System
{
    public class CMS_RoleApi_DTO : BaseEntity_DTO
    {
        [Display(Name = "Tên Api")]
        public int IdApi { get; set; }

        [Display(Name = "Tên Api")]
        public string NameApi { get; set; }

        [Display(Name = "Tên nhóm người dùng")]
        public int IdRole { get; set; }

        [Display(Name = "Tên nhóm người dùng")]
        public string NameRole { get; set; }
    }
}