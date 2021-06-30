using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SRCU
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
           );

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Account", action = "Index", id = UrlParameter.Optional }
            //);

            routes.MapRoute(
                    name: "BranchSchoolBag",
                    url: "{area}/{controller}/{action}/{id}",
                    defaults: new { area = "Branch", controller = "Branch", action = "BranchSchoolBag", id = 10 }
                );

            routes.MapRoute(
                    name: "SchoolBag",
                    url: "{area}/{controller}/{action}/{id}",
                    defaults: new { controller = "Admin", action = "SchoolBag", id = 0 }
                );
        }
    }
}
