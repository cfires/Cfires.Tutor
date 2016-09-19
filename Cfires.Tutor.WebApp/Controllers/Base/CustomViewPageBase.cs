using Cfires.Tutor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cfires.Tutor.WebApp.Controllers.Base
{
    public abstract class CustomViewPageBase : CustomViewPageBase<object>
    { }

    public abstract class CustomViewPageBase<TModel> : WebViewPage<TModel>
    {
        public Base_User CurrentUser
        {
            get
            {
                return ((CustomControllerBase)this.ViewContext.Controller).CurrentUser;
            }
        }
    }
}