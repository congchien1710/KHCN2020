using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KHCN.Data.Entities.System
{
    [Table("CMS_RoleFunction")]
    public class CMS_RoleFunction : BaseEntity
    {
        [Display(Name = "Tên chức năng")]
        public int IdFunction { get; set; }

        [Display(Name = "Tên nhóm người dùng")]
        public int IdRole { get; set; }
    }
}