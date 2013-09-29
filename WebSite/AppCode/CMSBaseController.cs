using ECMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.App_Code
{
    public class CMSBaseController : Controller
    {
        public ContentBase PageContent { get; set; }
        public ValidUrl CurrentUrl {
            get {
                return (ControllerContext.HttpContext.Items["validUrl"] != null ? ControllerContext.HttpContext.Items["validUrl"] as ValidUrl : null);
            }
        }

        public string GetView()
        {
            return "~/Views/" + this.CurrentUrl.SiteId + this.CurrentUrl.View + ".cshtml";
        }

        public string GetErrorHandlerView()
        {
            return "~/Views/" + this.CurrentUrl.SiteId + "Ecms-Error-Handler.cshtml";
        }

        public int GetErrorStatusCode()
        {
            if (this.HttpContext != null && this.HttpContext.Items[""] != null)
            {
                return Convert.ToInt32(this.HttpContext.Items["ResponseStatusCode"]);
            }
            else {
                return 500;
            }

        }
    }
}