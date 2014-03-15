using ECMS.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ECMS.Core
{
    public class ECMSContextActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContextBase context = filterContext.Controller.ControllerContext.HttpContext;
            if (context.Items["SiteId"] == null)
            {
                int siteid = Utility.GetSiteId(context.Request.Url.Host);
                if (siteid > -1)
                {
                    context.Items.Add("SiteId", siteid);
                }
            }
        }
    }
}
