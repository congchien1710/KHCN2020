using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KHCN.Data.Entities.System
{
    [Table("CMS_RolePage")]
    public class CMS_RolePage : BaseEntity
    {
        [Display(Name = "Tên trang")]
        public int IdPage { get; set; }

        [Display(Name = "Tên nhóm người dùng")]
        public int IdRole { get; set; }
    }
}