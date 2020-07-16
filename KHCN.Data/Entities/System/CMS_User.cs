using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KHCN.Data.Entities.System
{
    [Table("CMS_User")]
    public class CMS_User : BaseEntity
    {
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string UserName { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string Password { get; set; }

        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string FullName { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Bắt buộc")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Điện thoại")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string Mobile { get; set; }

        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; }

        [Display(Name = "Mã kích hoạt")]
        public string ActiveCode { get; set; }

        [Display(Name = "Refresh Token")]
        public string RefreshToken { get; set; }
    }
}