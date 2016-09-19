using Cfires.Tutor.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cfires.Tutor.WebApp.Controllers
{
    public class CommonController : Controller
    {
        #region 验证码

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowCaptcha()
        {
            var captcha = CaptchaUtility.Generate(6);
            Session["captcha"] = captcha;
            byte[] buff = CaptchaUtility.Draw(captcha);
            return File(buff, "image/jpeg");
        }

        #endregion
    }
}