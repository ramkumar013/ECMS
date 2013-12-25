using ECMS.Core;
using ECMS.Core.Utilities;
using ECMS.Services;
using ECMS.Services.ContentRepository;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ECMS.WebV2
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            DependencyManager.URLRepository = new ValidUrlMongoDBRepository();
            DependencyManager.ContentRepository = new FileSystemRepository();
            DependencyManager.CachingService = new InProcCachingService();
            DependencyManager.Logger = new NLog.Interface.LoggerAdapter(NLog.LogManager.GetLogger("default"));

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }


        void Application_Error(object sender, EventArgs e)
        {
            try
            {
                var httpContext = ((MvcApplication)sender).Context;
                var currentController = string.Empty;
                var currentAction = string.Empty;
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
                    info.Properties.Add("ClientIP", Utility.GetClientIP());
                    DependencyManager.Logger.Log(info);
                }

                var ex = Server.GetLastError();
                var controller = new ErrorController();
                var routeData = new RouteData();
                var action = "Index";
                if (ex != null)
                {
                    LogEventInfo info = new LogEventInfo(LogLevel.Error, ECMSSettings.DEFAULT_LOGGER, ex.ToString());
                    info.Properties.Add("ClientIP", Utility.GetClientIP());
                    DependencyManager.Logger.Log(info);
                }
                if (ex is HttpException)
                {
                    var httpEx = ex as HttpException;

                    switch (httpEx.GetHttpCode())
                    {
                        case 404:
                            action = "NotFound";
                            break;
                        default:
                            action = "Index";
                            break;
                    }
                }

                httpContext.ClearError();
                httpContext.Response.Clear();
                httpContext.Response.StatusCode = ex is HttpException ? ((HttpException)ex).GetHttpCode() : 500;
                httpContext.Response.TrySkipIisCustomErrors = true;

                routeData.Values["controller"] = "Error";
                routeData.Values["action"] = action;

                controller.ViewData.Model = new HandleErrorInfo(ex, currentController, currentAction);
                ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
            }
            catch (Exception ex)
            {
                LogEventInfo info = new LogEventInfo(LogLevel.Error, ECMSSettings.DEFAULT_LOGGER, ex.ToString());
                info.Properties.Add("ClientIP", Utility.GetClientIP());
                DependencyManager.Logger.Log(info);
            }
        }
    }
}