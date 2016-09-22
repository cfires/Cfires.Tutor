using Cfires.Tutor.BLL;
using Cfires.Tutor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cfires.Tutor.WebApp.Mvc
{
    public abstract class BaseWebPage : BaseWebPage<object>
    { }

    public abstract class BaseWebPage<TModel> : WebViewPage<TModel>
    {

        public Base_User CurrentUser
        {
            get
            {
                var user = new UserService().GetByEmail(User.Identity.Name);
                return user;
            }
        }
    }
}