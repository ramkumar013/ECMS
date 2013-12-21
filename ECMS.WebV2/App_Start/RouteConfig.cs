﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ECMS.WebV2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            
            routes.MapRoute(
               name: "Default",
               url: "admin/{controller}/{action}/{id}",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "DefaultWithViewName",
               url: "admin/ECMSView/{action}/{viewName}",
               defaults: new { controller = "Home", action = "Index", viewName = UrlParameter.Optional }
           );   


            routes.MapRoute(
                name: "UrlRewriteRoute",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

           
        }
    }
}