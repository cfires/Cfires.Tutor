using Cfires.Tutor.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cfires.Tutor.WebApp.Controllers
{
    public class ManageUserController : Controller
    {
        UserService _userService = new UserService();

        // GET: ManageUser
        public ActionResult Index()
        {
            var userList = _userService.GetUserList();
            return View();
        }
    }
}