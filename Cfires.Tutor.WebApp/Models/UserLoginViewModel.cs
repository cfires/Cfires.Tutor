using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cfires.Tutor.WebApp.Models
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "请输入邮箱")]
        [Display(Name = "邮箱：")]
        [EmailAddress(ErrorMessage = "请输入正确的邮箱")]
        public string Email { get; set; }

        [Required(ErrorMessage = "请输入密码")]
        [DataType(DataType.Password)]
        [Display(Name = "密码：")]
        public string Password { get; set; }

        [Required(ErrorMessage = "请输入验证码")]
        [Display(Name = "验证码：")]
        public string Captcha { get; set; }

        [Display(Name = "记住我?")]
        public bool RememberMe { get; set; }
    }
}
