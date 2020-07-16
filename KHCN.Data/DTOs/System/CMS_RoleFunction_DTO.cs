using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KHCN.Data.DTOs.System
{
    public class CMS_RoleFunction_DTO : BaseEntity_DTO
    {
        [Display(Name = "Tên chức năng")]
        public int IdFunction { get; set; }

        [Display(Name = "Tên chức năng")]
        public string NameFunction { get; set; }

        [Display(Name = "Tên nhóm người dùng")]
        public int IdRole { get; set; }

        [Display(Name = "Tên nhóm người dùng")]
        public string NameRole { get; set; }
    }
}