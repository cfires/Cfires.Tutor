using Cfires.Tutor.BLL;
using Cfires.Tutor.Common;
using Cfires.Tutor.Model;
using Cfires.Tutor.WebApp.Controllers.Base;
using Cfires.Tutor.WebApp.Models;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Cfires.Tutor.WebApp.Controllers
{
    public class UserController : CustomControllerBase
    {
        public UserController() { }

        UserService _userService = new UserService();

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// 登录表单提交
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(UserLoginViewModel viewModel, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            string captcha = Session["captcha"] == null ? string.Empty : Session["captcha"].ToString();
            Session["captcha"] = null;
            if (string.IsNullOrWhiteSpace(captcha))
            {
                ModelState.AddModelError("Captcha", "请填写验证码");
            }
            else if (viewModel.Captcha.Equals(captcha, StringComparison.InvariantCultureIgnoreCase))
            {
                ModelState.AddModelError("Captcha", "验证码错误");
            }

            var user = _userService.GetByEmail(viewModel.Email);

            Login(user);

            if (user != null && SecurityHelper.DecryptAES(user.Password) == viewModel.Password)
            {
                return RedirectToLocal(returnUrl);
            }
            else
            {
                ModelState.AddModelError("Validate", "用户名或密码错误");
                return View();
            }
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// 注册表单提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register(UserRegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _userService.Create(viewModel.AsUser());
            }

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// 验证待注册邮箱是否已被注册
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public ActionResult _ValidateEmailRepeat(string email)
        {
            var user = _userService.GetByEmail(email);
            if (user != null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        #region private

        private void Login(Base_User user)
        {
            var identity = new ClaimsIdentity("App");
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Name));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Role, user.Type.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.GroupSid, user.Type.ToString()));
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);
        }

        /// <summary>
        /// 重定向
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}