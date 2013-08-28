using ECMS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebSite
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //int siteId = -1; // TODO;

            //DependencyManager.RouteManager.GetAllRoutes(siteId).ForEach(
            //    x => routes.MapRoute(
            //        name:x.Name,
            //        url: x.Url,
            //        defaults: new { Controller = x.Controller, action = x.Action }
            //        ));

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Page", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}