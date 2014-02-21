using ECMS.Core;
using ECMS.Core.Utilities;
using ECMS.Services;
using ECMS.Services.ContentRepository;
using NLog;
using NLog.Config;
using NLog.Interface;
using NLog.Targets;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ECMS.Core.Extensions;

namespace ECMS.WebV2
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private Guid _loggerName = Guid.Empty;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            DependencyManager.ViewRepository = new ECMSViewRepository();
            //DependencyManager.URLRepository = new ValidUrlFileRepository();
            DependencyManager.URLRepository = new ValidUrlMongoDBRepository();
            //DependencyManager.ContentRepository = new FileSystemRepository();
            DependencyManager.ContentRepository = new MongoDBRepository();
            DependencyManager.CachingService = new InProcCachingService();
            DependencyManager.Logger = ECMSLogger.Instance;
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }
        public override void Init()
        {
            this.BeginRequest += MvcApplication_BeginRequest;
            this.EndRequest += MvcApplication_EndRequest;
            base.Init();
        }
        void MvcApplication_EndRequest(object sender, EventArgs e)
        {
            MemoryTarget memTarget = (MemoryTarget)LogManager.Configuration.FindTargetByName(Convert.ToString(HttpContext.Current.Items["LoggerName"]));
            if (memTarget != null)
            {
                memTarget.Dispose();
                LogManager.Configuration.LoggingRules.RemoveAt(LogManager.Configuration.LoggingRules.Count - 1);
                LogManager.Configuration.RemoveTarget(memTarget.Name);
                LogManager.Configuration.Reload();
            }
        }

        void MvcApplication_BeginRequest(object sender, EventArgs e)
        {
            MemoryTarget _logTarget = null;
            _loggerName = Guid.NewGuid();
            HttpContext.Current.Items.Add("LoggerName", _loggerName);
            _logTarget = new MemoryTarget();
            _logTarget.Name = _loggerName.ToString();

            LoggingRule rule = new LoggingRule(_loggerName.ToString(), _logTarget);
            rule.EnableLoggingForLevel(LogLevel.Debug);

            LogManager.Configuration.LoggingRules.Add(rule);

            LogManager.Configuration.Reload();
        }

        void Application_Error(object sender, EventArgs e)
        {
            try
            {
                var httpContext = ((MvcApplication)sender).Context;
                var currentController = string.Empty;
                var currentAction = string.Empty;
                var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));
                Logger logger = LogManager.GetLogger(_loggerName.ToString());

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

                var ex = Server.GetLastError();
                var controller = new ErrorController();
                var routeData = new RouteData();
                var action = "Index";
                if (ex != null)
                {
                    LogEventInfo info = new LogEventInfo(LogLevel.Error, ECMSSettings.DEFAULT_LOGGER, ex.ToString());
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
                MemoryTarget memTarget = (MemoryTarget)LogManager.Configuration.AllTargets[LogManager.Configuration.AllTargets.Count - 1];
                if (memTarget != null)
                {
                    controller.ViewData["Logs"] = memTarget.Logs;
                }

                ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
            }
            catch (Exception ex)
            {
                LogEventInfo info = new LogEventInfo(LogLevel.Error, ECMSSettings.DEFAULT_LOGGER, ex.ToString());
                DependencyManager.Logger.Log(info);
            }
        }

        public override string GetOutputCacheProviderName(HttpContext context)
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["UseFakePageOutputCache"]))
            {
                return "FakePageOutputCache";
            }
            return base.GetOutputCacheProviderName(context);
        }
        public override string GetVaryByCustomString(HttpContext context, string custom)
        {
            if (custom.ToLower() == "actionresultcache")
            {
                if (context.Request.IsAuthenticated)
                {
                    return Guid.NewGuid().ToString();
                }
                if (Request.QueryString["vm"] != null || Request.Headers["ecmsrefresh"] != null)
                {
                    return Guid.NewGuid().ToString();
                }
                else
                {
                    // we want to include querystring and form in the vary by param. 
                    // TODO : Remove some extra querystring param.
                    return Request.RawUrl + Request.QueryString.ToString();
                }
            }
            return base.GetVaryByCustomString(context, custom);
        }
    }
}