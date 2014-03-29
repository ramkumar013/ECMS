using ECMS.Core;
using ECMS.Core.Entities;
using ECMS.Core.Framework;
using ECMS.Core.Utilities;
using ECMS.Services;
using ECMS.WebV2.Locale;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ECMS.WebV2
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
            return "~/Views/" + this.CurrentUrl.SiteId + "/" + (short)this.ViewType + "/" + this.CurrentUrl.View + ".cshtml";
        }

        public string GetErrorHandlerView()
        {
            if (this.CurrentUrl != null)
            {
                return "~/Views/" + this.CurrentUrl.SiteId + "/Ecms-Error-Handler.cshtml";
            }
            else if (GetSiteIdFromContext() > -1)
            {
                return "~/Views/" + GetSiteIdFromContext() + "/Ecms-Error-Handler.cshtml";
            }
            else
            {
                return "~/Views/Shared/Ecms-Error-Handler.cshtml";
            }
        }

        public int GetSiteIdFromContext()
        {
            return Convert.ToInt32(ControllerContext.HttpContext.Items["SiteId"]);
        }

        public int GetErrorStatusCode()
        {
            if (this.HttpContext != null && ControllerContext.HttpContext.Items["ResponseStatusCode"] != null)
            {
                return Convert.ToInt32(ControllerContext.HttpContext.Items["ResponseStatusCode"]);
            }
            else {
                return 500;
            }
        }

        public string GetErrorMessage()
        {
            // why not store tags in resouce file:
            // http://stackoverflow.com/questions/1588790/best-practices-tips-for-storing-html-tags-in-resource-files

            //StringBuilder builder = new StringBuilder(ECMSResources.ResourceManager.GetString(ECMSSettings.HTTPERROR_LOCALE_PREFIX + GetErrorStatusCode().ToString()));
            //builder.Replace("{11}", "<h2>");
            //builder.Replace("{12}", "</h2>");
            //builder.Replace("{21}", "<a href=\"/\">");
            //builder.Replace("{22}", "</a>");
            //return builder.ToString();

            return string.Empty;
        }
        public ContentViewType ViewType
        {
            get
            {
                return Utility.CurrentViewType(this.ControllerContext.HttpContext);
            }
        }
        public DeviceType DeviceType {
            get {
                return DeviceType.WEB;
            }
        }

        public ECMSMember CMSUser
        {
            get
            {
                ECMSMember member = DependencyManager.CachingService.Get<ECMSMember>("LoggedInUser");
                if (member == null && HttpContext.User != null && HttpContext.User.Identity.IsAuthenticated)
                {
                    DefaultUserProfileService service = new DefaultUserProfileService(SecurityHelper.Decrypt(ConfigurationManager.ConnectionStrings["mongodb"].ConnectionString, true));
                    member = service.GetProfileByUserName(HttpContext.User.Identity.Name);
                    if (member != null)
                    {
                        DependencyManager.CachingService.Set<ECMSMember>("LoggedInUser", member);
                    }
                }
                return member;
            }
        }

        public string GetControllerView(string viewName_)
        {
            return Convert.ToString("~/views/admin/" + this.RouteData.Values["controller"]) + "-" + viewName_ + ".cshtml";
        }
    }
}