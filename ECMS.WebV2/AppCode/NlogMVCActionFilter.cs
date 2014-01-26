using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;
using NLog.Config;
using NLog.Targets;
using System.Diagnostics;
using System.Web.Routing;
namespace MvcApplication1.AppCode
{
    public class NlogMVCActionFilter : ActionFilterAttribute
    {
        private string _loggerName = string.Empty;
        Stopwatch _stopWatch = null;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _stopWatch = new Stopwatch();
            _stopWatch.Start();
            Log("OnActionExecuting", filterContext.RouteData, filterContext.HttpContext);
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Log("OnActionExecuted", filterContext.RouteData, filterContext.HttpContext);
        }
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            Log("OnResultExecuting", filterContext.RouteData, filterContext.HttpContext);
        }
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            Log("OnResultExecuted", filterContext.RouteData, filterContext.HttpContext);
            _stopWatch.Stop();
        }

        private void Log(string methodName, RouteData routeData, HttpContextBase context_)
        {
            string message = null;
            if (context_.Items["LoggerName"]!=null)
            {
                _loggerName = ((System.Guid)context_.Items["LoggerName"]).ToString();
                Logger logger = LogManager.GetLogger(_loggerName);
                if (logger != null)
                {
                    logger.Debug("Inside OnActionExecuted");
                }
                var controllerName = routeData.Values["controller"];
                var actionName = routeData.Values["action"];
                message = String.Format("{0} controller:{1} action:{2}, duration:{3}", methodName, controllerName, actionName, _stopWatch.ElapsedMilliseconds.ToString());
            }
            Debug.WriteLine(message, "Action Filter Log");
        }
    }
}