using ECMS.Core;
using ECMS.Core.Entities;
using ECMS.Core.Framework;
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
            if (this.CurrentUrl!=null)
            {
                return "~/Views/" + this.CurrentUrl.SiteId + "/Ecms-Error-Handler.cshtml";    
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
                try
                {
                    if (this.HttpContext != null)
                    {
                        if (this.HttpContext.Request != null && HttpContext.Request.Cookies["ECMS-Preview-Mode"] != null)
                        {
                            return ContentViewType.PREVIEW;
                        }
                        else if (this.HttpContext.Request != null && HttpContext.Request.QueryString["vm"] != null)
                        {
                            ContentViewType viewType = (ContentViewType)Enum.Parse(typeof(ContentViewType), this.HttpContext.Request.QueryString["vm"].ToString(), true);
                            return viewType;
                        }
                        else
                        {
                            return ContentViewType.PUBLISH;
                        }
                    }
                    else
                    {
                        return ContentViewType.PUBLISH;
                    }
                }
                catch (Exception ex)
                {
                    LogEventInfo info = new LogEventInfo(LogLevel.Error, ECMSSettings.DEFAULT_LOGGER, ex.ToString());
                    if (CurrentUrl != null)
                    {
                        info.Properties.Add("URL", CurrentUrl.FriendlyUrl);
                    }
                    else
                    {
                        if (ControllerContext != null && ControllerContext.HttpContext != null && ControllerContext.HttpContext.Request.Url != null)
                        {
                            info.Properties.Add("URL", ControllerContext.HttpContext.Request.Url);
                        }
                    }
                    DependencyManager.Logger.Log(info);
                    return ContentViewType.PUBLISH;
                }
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
                    DefaultUserProfileService service = new DefaultUserProfileService(ConfigurationManager.ConnectionStrings["mongodb"].ConnectionString);
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