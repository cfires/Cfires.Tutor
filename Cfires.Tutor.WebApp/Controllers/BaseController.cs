using Cfires.Tutor.BLL;
using Cfires.Tutor.Common;
using Cfires.Tutor.Model;
using Microsoft.Owin.Security;
using System;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Cfires.Tutor.WebApp.Controllers
{
    /// <summary>
    ///  Controller基类
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseController()
            : base()
        {
            this.ValidateRequest = false;
        }

        protected IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        /// <summary>
        /// 当前用户
        /// </summary>
        public Base_User CurrentUser
        {
            get
            {
                var user = AuthenticationManager.User;

                ExceptionHelper.IsNull(user, "获取不到登录票据！");

                if (!user.Identity.IsAuthenticated)
                    return null;

                Base_User userInfo;

                var useridClaimType = user.FindFirst(ClaimTypes.NameIdentifier);
                var usertypeClaimType = user.FindFirst(ClaimTypes.GroupSid);

                UserType ut = EnumHelper.Parse<UserType>(usertypeClaimType.Value);
                int userID = int.Parse(useridClaimType.Value);

                UserService _userService = new UserService();

                switch (ut)
                {
                    case UserType.Student:
                    case UserType.Tutor:
                    default:
                        userInfo = _userService.Get(userID);
                        break;
                }

                ExceptionHelper.IsNull(userInfo, "获取不到登录用户！");

                return userInfo;
            }
        }
    }
}