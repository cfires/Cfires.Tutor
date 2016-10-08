using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cfires.Tutor.WebApp.Controllers
{
    public class TutorsController : BaseController
    {
        // GET: Tutors
        public ActionResult Index()
        {
            return View();
        }
    }
}