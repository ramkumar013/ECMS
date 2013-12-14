using ECMS.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ECMS.Core
{
    public class ECMSContextActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            int siteid = Utility.GetSiteId(filterContext.Controller.ControllerContext.HttpContext.Request.Url.Host);
            if (siteid>-1)
            {
                filterContext.Controller.ControllerContext.HttpContext.Items.Add("siteid", siteid);
            }
        }
    }
}
