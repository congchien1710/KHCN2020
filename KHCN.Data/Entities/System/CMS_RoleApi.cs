using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KHCN.Data.Entities.System
{
    [Table("CMS_RoleApi")]
    public class CMS_RoleApi : BaseEntity
    {
        [Display(Name = "Tên Api")]
        public int IdApi { get; set; }

        [Display(Name = "Tên nhóm người dùng")]
        public int IdRole { get; set; }
    }
}