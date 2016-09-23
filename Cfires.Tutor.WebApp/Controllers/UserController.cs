using Cfires.Tutor.BLL;
using Cfires.Tutor.Common;
using Cfires.Tutor.Model;
using Cfires.Tutor.WebApp.Models;
using Microsoft.Owin.Security;
using System;
using System.Security.Claims;
using System.Web.Mvc;

namespace Cfires.Tutor.WebApp.Controllers
{
    public class UserController : BaseController
    {
        public UserController() { }

        UserService _userService = new UserService();

        #region 登录
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

            if (viewModel.Captcha.Equals(captcha, StringComparison.InvariantCultureIgnoreCase))
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
        #endregion

        #region 注册
        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Register(string returnUrl = null)
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(UserRegisterViewModel viewModel, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                _userService.Create(viewModel.AsUser());
                var user = _userService.GetByEmail(viewModel.Email);
                Login(user);
                return RedirectToAction("Index", "Home");
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(viewModel);
        }

        #endregion

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
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Email));
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