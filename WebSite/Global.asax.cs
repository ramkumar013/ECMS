using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ECMS.Core;
using ECMS.Services;
using NLog;
namespace WebApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            DependencyManager.URLRepository = new ValidUrlFileRepository();
            DependencyManager.CachingService = new InProcCachingService();
            DependencyManager.Logger = new NLog.Interface.LoggerAdapter(NLog.LogManager.GetLogger("default"));
        }


        public override void Init()
        {
            base.Init();
            this.Error += Application_Error;
        }

        void Application_Error(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            app.Context.RewritePath("/Template/HandleServerError");
        }
    }
}