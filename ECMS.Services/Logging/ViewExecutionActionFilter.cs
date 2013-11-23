using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace ECMS.Services.Logging
{
    public class ViewExecutionActionFilter : ActionFilterAttribute
    {
        Stopwatch stopWatch = new Stopwatch();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            stopWatch.Start();
            Log("OnActionExecuting", filterContext.RouteData);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //Log("OnActionExecuted", filterContext.RouteData);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            stopWatch.Stop();
            Log("OnResultExecuted", filterContext.RouteData);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            //Log("OnResultExecuting", filterContext.RouteData);
        }

        private void Log(string methodName, RouteData routeData)
        {
            var controllerName = routeData.Values["controller"];
            var actionName = routeData.Values["action"];
            var message = String.Format("{0} controller:{1} action:{2}, duration:{3}", methodName, controllerName, actionName, stopWatch.ElapsedMilliseconds.ToString());
            Debug.WriteLine(message, "Action Filter Log");
        }
    }
}
