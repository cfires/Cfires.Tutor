using AutoMapper;
using Cfires.Tutor.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Cfires.Tutor.WebApp.Models
{
    public class UserRegisterViewModel
    {
        [Required(ErrorMessage = "必填")]
        [Display(Name = "类型")]
        public string Type { get; set; }

        [Required(ErrorMessage = "请输入联系人")]
        [Display(Name = "联系人")]
        public string Name { get; set; }

        [Required(ErrorMessage = "请输入邮箱")]
        [Display(Name = "电子邮件")]
        [EmailAddress(ErrorMessage = "请输入正确的邮箱格式")]
        [Remote("_ValidateEmailRepeat", "User", ErrorMessage = "该邮箱已被注册")]
        public string Email { get; set; }

        [Required(ErrorMessage = "请输入密码")]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Required(ErrorMessage = "请再次输入密码")]
        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "输入密码不一致")]
        public string ConfimPassword { get; set; }
    }

    /// <summary>
    /// 模型扩展
    /// </summary>
    public static class UserRegisterViewModelExtensions
    {
        static UserRegisterViewModelExtensions()
        {
            //创建映射配置
            Mapper.Initialize(x => x.CreateMap<Base_User, UserRegisterViewModel>());
            Mapper.Initialize(x => x.CreateMap<UserRegisterViewModel, Base_User>());
        }

        /// <summary>
        /// 转换为数据模型
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Base_User AsUser(this UserRegisterViewModel source)
        {
            return Mapper.Map<Base_User>(source);
        }
    }
}
