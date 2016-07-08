using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace TimePro.Filter
{
    public class Filter
    {
        public class LoginFilter : AuthorizeAttribute, IAuthorizationFilter
        {

            public override void OnAuthorization(AuthorizationContext filterContext)
            {
                string username = (string)filterContext.HttpContext.Session["username"];
                string password = (string)filterContext.HttpContext.Session["password"];

                if (username == null || password == null || filterContext.HttpContext.Session.Mode == 0)
                {
                    filterContext.Result = new RedirectResult("../Users/Login");
                }
                return;
            }
        }
    }
}