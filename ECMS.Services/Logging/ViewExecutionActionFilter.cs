﻿using System;
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
        Stopwatch _stopWatch = null;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _stopWatch= new Stopwatch();
            _stopWatch.Start();
            Log("OnActionExecuting", filterContext.RouteData);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Log("OnActionExecuted", filterContext.RouteData);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            Log("OnResultExecuted", filterContext.RouteData);
            _stopWatch.Stop();
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            Log("OnResultExecuting", filterContext.RouteData);
        }

        private void Log(string methodName, RouteData routeData)
        {
            var controllerName = routeData.Values["controller"];
            var actionName = routeData.Values["action"];
            var message = String.Format("{0} controller:{1} action:{2}, duration:{3}", methodName, controllerName, actionName, _stopWatch.ElapsedMilliseconds.ToString());
            Debug.WriteLine(message, "Action Filter Log");
        }
    }
}
