using ECMS.Core;
using ECMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebApp.Locale.en_us;

namespace WebSite.App_Code
{
    public class CMSBaseController : Controller
    {
        public ContentItem PageContent { get; set; }
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
            if (this.CurrentUrl!=null)
            {
                return "~/Views/" + this.CurrentUrl.SiteId + "Ecms-Error-Handler.cshtml";    
            }
            else
            {
                return "~/Views/Ecms-Error-Handler.cshtml";    
            }
        }

        public int GetErrorStatusCode()
        {
            if (this.HttpContext != null && this.HttpContext.Items["ResponseStatusCode"] != null)
            {
                return Convert.ToInt32(this.HttpContext.Items["ResponseStatusCode"]);
            }
            else {
                return 500;
            }
        }

        public string GetErrorMessage()
        {
            // why not store tags in resouce file:
            // http://stackoverflow.com/questions/1588790/best-practices-tips-for-storing-html-tags-in-resource-files

            StringBuilder builder = new StringBuilder(ECMSResources.ResourceManager.GetObject(ECMSSettings.HTTPERROR_LOCALE_PREFIX + GetErrorStatusCode().ToString()).ToString());
            builder.Replace("{11}", "<h2>");
            builder.Replace("{12}", "</h2>");
            builder.Replace("{21}", "<a href=\"//>");
            builder.Replace("{22}", "</a");
            return builder.ToString();
        }
    }
}