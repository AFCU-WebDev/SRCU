﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace SRCUBagTracking.Controllers
{
     //[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CheckSessionOutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["user"] == null)
            {
                FormsAuthentication.SignOut();
                filterContext.Result =
               new RedirectToRouteResult(new RouteValueDictionary   
            {  
                {"action", "Login" },  
                {"controller", "Account" },
                {"Area", String.Empty},
                {"returnUrl", filterContext.HttpContext.Request.RawUrl}  
             });

                return;
            }
        }            
        }
    }