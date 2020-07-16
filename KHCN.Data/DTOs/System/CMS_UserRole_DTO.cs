using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KHCN.Data.DTOs.System
{
    public class CMS_UserRole_DTO : BaseEntity_DTO
    {
        [Display(Name = "Tên người dùng")]
        public int IdUser { get; set; }

        [Display(Name = "Tên người dùng")]
        public string NameUser { get; set; }

        [Display(Name = "Tên nhóm người dùng")]
        public int IdRole { get; set; }

        [Display(Name = "Tên nhóm người dùng")]
        public string NameRole { get; set; }
    }
}