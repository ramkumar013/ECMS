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
using ECMS.Services.ContentRepository;
using WebApp.Controllers;
namespace WebApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            DependencyManager.URLRepository = new ValidUrlFileRepository();
            DependencyManager.ContentRepository = new FileSystemRepository();
            DependencyManager.CachingService = new InProcCachingService();
            DependencyManager.Logger = new NLog.Interface.LoggerAdapter(NLog.LogManager.GetLogger("default"));

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }


        //public override void Init()
        //{
        //    base.Init();
        //    //this.Error += Application_Error;
        //}

        void Application_Error(object sender, EventArgs e)
        {
            //HttpApplication app = (HttpApplication)sender;
            //app.Context.RewritePath("/Template/HandleServerError");

            var httpContext = ((MvcApplication)sender).Context;
            var currentController = " ";
            var currentAction = " ";
            var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));

            if (currentRouteData != null)
            {
                if (currentRouteData.Values["controller"] != null && !String.IsNullOrEmpty(currentRouteData.Values["controller"].ToString()))
                {
                    currentController = currentRouteData.Values["controller"].ToString();
                }

                if (currentRouteData.Values["action"] != null && !String.IsNullOrEmpty(currentRouteData.Values["action"].ToString()))
                {
                    currentAction = currentRouteData.Values["action"].ToString();
                }

                LogEventInfo info = new LogEventInfo(LogLevel.Error, ECMSSettings.DEFAULT_LOGGER, currentController + "--" + currentAction);
                DependencyManager.Logger.Log(info);
            }

            //var ex = Server.GetLastError();
            //var controller = new ErrorController();
            //var routeData = new RouteData();
            //var action = "Index";
            //if (ex != null)
            //{
            //    LogEventInfo info = new LogEventInfo(LogLevel.Error, ECMSSettings.DEFAULT_LOGGER, ex.ToString());
            //    DependencyManager.Logger.Log(info);
            //}
            //if (ex is HttpException)
            //{
            //    var httpEx = ex as HttpException;

            //    switch (httpEx.GetHttpCode())
            //    {
            //        case 404:
            //            action = "NotFound";
            //            break;
            //        default:
            //            action = "Index";
            //            break;
            //    }
            //}

            //httpContext.ClearError();
            //httpContext.Response.Clear();
            //httpContext.Response.StatusCode = ex is HttpException ? ((HttpException)ex).GetHttpCode() : 500;
            //httpContext.Response.TrySkipIisCustomErrors = true;

            //routeData.Values["controller"] = "Error";
            //routeData.Values["action"] = action;

            //controller.ViewData.Model = new HandleErrorInfo(ex, currentController, currentAction);
            //((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
        }
    }
}