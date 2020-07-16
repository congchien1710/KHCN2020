using System.ComponentModel.DataAnnotations;

namespace KHCN.Data.ViewModels
{
    public class LoginModel
    {
        [Key]
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Bạn phải nhập tài khoản")]
        public string UserName { set; get; }

        [Required(ErrorMessage = "Bạn phải nhập mật khẩu")]
        [Display(Name = "Mật khẩu")]
        public string Password { set; get; }

        [Display(Name = "Nhớ mật khẩu")]
        public bool RememberMe { get; set; }

        public string ReturnURL { get; set; }
    }

    public class LoginViewModel
    {
        [Key]
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Bạn phải nhập tài khoản")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Bạn phải nhập mật khẩu")]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        public string RequestPath { get; set; }
    }
}
