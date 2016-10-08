using Cfires.Tutor.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cfires.Tutor.WebApp.Controllers
{
    public class ManageUserController : BaseController
    {
        UserService _userService = new UserService();

        // GET: ManageUser
        public ActionResult Index(int pageIndex = 1)
        {
            var userList = _userService.GetUserList(pageIndex, 20);
            return View();
        }
    }
}