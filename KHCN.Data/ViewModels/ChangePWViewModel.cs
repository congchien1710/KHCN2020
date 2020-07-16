using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KHCN.Data.ViewModels
{
    public class ChangePWViewModel
    {
        public int UserId { get; set; }

        [Display(Name = "Mật khẩu hiện tại")]
        [Required(ErrorMessage = "(*) Bạn phải nhập mật khẩu hiện tại")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Display(Name = "Mật khẩu mới")]
        [Required(ErrorMessage = "(*) Bạn phải nhập mật khẩu mới")]
        [DataType(DataType.Password)]
        public string NewPassWord { get; set; }

        [Display(Name = "Nhập lại mật khẩu mới")]
        [Required(ErrorMessage = "(*) Bạn phải nhập mật khẩu xác nhận")]
        [CompareAttribute("NewPassWord", ErrorMessage = "Mật khẩu nhập lại không khớp")]
        [DataType(DataType.Password)]
        public string ReNewPassWord { get; set; }
    }
}
