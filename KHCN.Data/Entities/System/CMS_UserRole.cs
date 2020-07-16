using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KHCN.Data.Entities.System
{
    [Table("CMS_UserRole")]
    public class CMS_UserRole : BaseEntity
    {
        [Display(Name = "Tên người dùng")]
        public int IdUser { get; set; }

        [Display(Name = "Tên nhóm người dùng")]
        public int IdRole { get; set; }
    }
}