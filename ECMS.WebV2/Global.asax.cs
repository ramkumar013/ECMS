using ECMS.Core;
using ECMS.Services;
using ECMS.Services.ContentRepository;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
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
        private string _loggerName = string.Empty;
        protected void Application_Start()
        {
            DependencyManager.ViewRepository = new ECMSViewRepository();
            DependencyManager.URLRepository = new ValidUrlMongoDBRepository();
            DependencyManager.ContentRepository = new MongoDBRepository();
            DependencyManager.CachingService = new InProcCachingService();
            DependencyManager.Logger = ECMSLogger.Instance;
            AreaRegistration.RegisterAllAreas();
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


        void Application_Error(object sender, EventArgs e)
        {
            try
            {
                var httpContext = ((MvcApplication)sender).Context;
                var currentController = string.Empty;
                var currentAction = string.Empty;
                var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));
                Logger logger = LogManager.GetLogger(_loggerName);

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
                MemoryTarget memTarget = LogManager.Configuration.AllTargets[LogManager.Configuration.AllTargets.Count - 1] as MemoryTarget;
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

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Directory.GetParent(Path.GetDirectoryName(path)).FullName;
            }
        }


        void Application_End()
        {
            try
            {
                HttpRuntime runtime = (HttpRuntime)typeof(System.Web.HttpRuntime).InvokeMember("_theRuntime",
                                                                                           BindingFlags.NonPublic
                                                                                           | BindingFlags.Static
                                                                                           | BindingFlags.GetField,
                                                                                           null,
                                                                                           null,
                                                                                           null);

                if (runtime == null)
                    return;

                string shutDownMessage = (string)runtime.GetType().InvokeMember("_shutDownMessage",
                                                                                 BindingFlags.NonPublic
                                                                                 | BindingFlags.Instance
                                                                                 | BindingFlags.GetField,
                                                                                 null,
                                                                                 runtime,
                                                                                 null);

                string shutDownStack = (string)runtime.GetType().InvokeMember("_shutDownStack",
                                                                               BindingFlags.NonPublic
                                                                               | BindingFlags.Instance
                                                                               | BindingFlags.GetField,
                                                                               null,
                                                                               runtime,
                                                                               null);

                File.AppendAllText(AssemblyDirectory + "\\Logs\\ECMSLogs.txt", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss:ffff") + " : shutDownMessage--" + shutDownMessage + " : shutDownStack--" + shutDownStack);
            }
            catch (Exception ex)
            {
                File.AppendAllText(AssemblyDirectory + "\\Logs\\ECMSLogs.txt", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss:ffff") + " : Error while shutdown " + ex.ToString());
            }
        }
        void MvcApplication_BeginRequest(object sender, EventArgs e)
        {
            try
            {
                if (HttpContext.Current.Request.QueryString["vm"] != null && Convert.ToInt32(HttpContext.Current.Request.QueryString["vm"]) == 10)
                {
                    MemoryTarget _logTarget = null;
                    _loggerName = Guid.NewGuid().ToString().Replace("-", string.Empty);
                    HttpContext.Current.Items.Add("LoggerName", _loggerName);
                    _logTarget = new MemoryTarget();
                    _logTarget.Name = _loggerName.ToString();

                    LoggingRule rule = new LoggingRule(_loggerName, _logTarget);
                    rule.EnableLoggingForLevel(LogLevel.Debug);
                    rule.EnableLoggingForLevel(LogLevel.Trace);
                    rule.EnableLoggingForLevel(LogLevel.Info);
                    rule.EnableLoggingForLevel(LogLevel.Warn);
                    rule.EnableLoggingForLevel(LogLevel.Error);
                    rule.EnableLoggingForLevel(LogLevel.Fatal);

                    LogManager.Configuration.LoggingRules.Add(rule);

                    LogManager.Configuration.Reload();
                }
            }
            catch (Exception ex)
            {
                LogEventInfo info = new LogEventInfo(LogLevel.Error, ECMSSettings.DEFAULT_LOGGER, ex.ToString());
                ECMSLogger.Instance.Log(info);
            }
        }

        void MvcApplication_EndRequest(object sender, EventArgs e)
        {
            try
            {
                var loggerName = Convert.ToString(HttpContext.Current.Items["LoggerName"]);
                if (!string.IsNullOrEmpty(loggerName))
                {
                    var rule = LogManager.Configuration.LoggingRules.Where(x => x.NameMatches(loggerName)).FirstOrDefault();
                    if (rule != null && rule.Targets.Count > 0)
                    {
                        MemoryTarget target = rule.Targets.Where(x => x.Name == _loggerName).FirstOrDefault() as MemoryTarget;
                        if (target != null)
                        {
                            target.Dispose();
                            LogManager.Configuration.LoggingRules.Remove(rule);
                            LogManager.Configuration.RemoveTarget(target.Name);
                            LogManager.Configuration.Reload();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogEventInfo info = new LogEventInfo(LogLevel.Error, ECMSSettings.DEFAULT_LOGGER, ex.ToString());
                ECMSLogger.Instance.Log(info);
            }
        }
    }
}