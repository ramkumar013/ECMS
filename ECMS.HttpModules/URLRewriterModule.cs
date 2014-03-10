using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ECMS.Core;
using ECMS.Core.Entities;
using NLog;
using ECMS.Core.Utilities;
using System.Configuration;
using ECMS.Core.Framework;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace ECMS.HttpModules
{
    public class URLRewriter : IHttpModule
    {

        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            string callingmehtodname = new StackFrame(1, true).GetMethod().Name;
            context.BeginRequest += context_BeginRequest;
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            string url = string.Empty;
            short siteId = -1;
            HttpContext context = HttpContext.Current;
            bool isPublish = true;
            try
            {
                siteId = Convert.ToInt16(Utility.GetSiteId(context.Request.Url.DnsSafeHost.ToLower()));
                context.Items.Add("siteid", siteId);
                if (siteId < 0 || !IsValidUrlForRewrite(new HttpContextWrapper(HttpContext.Current)))
                {
                    return;
                }
                url = context.Request.Url.AbsolutePath;
                isPublish = Utility.CurrentViewType(new HttpContextWrapper(HttpContext.Current)) == ContentViewType.PUBLISH;
                ValidUrl validUrl = DependencyManager.URLRepository.GetByFriendlyUrl(siteId, url, isPublish);
                if (validUrl != null)
                {
                    DependencyManager.Logger.Log(new LogEventInfo(LogLevel.Debug, ECMSSettings.DEFAULT_LOGGER, validUrl.Id + "....Found "));
                    validUrl.SiteId = siteId;
                    context.Items.Add("validUrl", validUrl);
                    switch (validUrl.StatusCode)
                    {
                        case 200:
                        default:
                            context.RewritePath(validUrl.Action);
                            break;
                        case 301:
                            context.Response.RedirectLocation = validUrl.Action;
                            context.Response.RedirectPermanent(validUrl.Action, true);
                            break;
                        case 302:
                            context.Response.RedirectLocation = validUrl.Action;
                            context.Response.Redirect(validUrl.Action, true);
                            break;
                        case 404:
                            HandleError(context, validUrl.SiteId, 404);
                            break;
                    }
                }
                else
                {
                    DependencyManager.Logger.Log(new LogEventInfo(LogLevel.Debug, ECMSSettings.DEFAULT_LOGGER, url + ":: Not Found "));
                    HandleError(context, siteId, 404);
                }
            }
            catch (KeyNotFoundException ex)
            {
                Exception temp = ex;
                while (temp != null)
                {
                    LogEventInfo info = new LogEventInfo(LogLevel.Error, ECMSSettings.DEFAULT_LOGGER, url + "::" + ex.ToString());
                    DependencyManager.Logger.Log(info);
                }
                HandleError(context, siteId, 404);
            }
            catch (Exception ex)
            {
                Exception temp = ex;
                while (temp != null)
                {
                    LogEventInfo info = new LogEventInfo(LogLevel.Error, ECMSSettings.DEFAULT_LOGGER, url + "::" + ex.ToString());
                    DependencyManager.Logger.Log(info);
                }
                HandleError(context, siteId, 500);
            }
        }

        private static void HandleError(HttpContext context_, int siteId_, int statusCode_)
        {
            context_.Items.Add("ResponseStatusCode", statusCode_);
            context_.RewritePath("\\Template\\HandleServerError");
            context_.Response.TrySkipIisCustomErrors = true;
        }

        public static bool IsValidUrlForRewrite(HttpContextBase httpContext)
        {
            UrlValidationConfig configSection = (UrlValidationConfig)ConfigurationManager.GetSection("UrlValidationConfig");
            string url = httpContext.Request.Url.AbsolutePath.ToLower();
            bool result = true;
            foreach (UrlValidationElement item in configSection.ConfigCollection)
            {
                string[] strs = null;
                switch (item.Action)
                {
                    case "contains":
                        strs = item.InvalidValue.Split(new char[] { ',' });
                        result = !strs.Any(x => url.Contains(x.ToLower()));
                        break;
                    case "equal":
                        strs = item.InvalidValue.Split(new char[] { ',' });
                        result = !strs.Any(x => url == x.ToLower());
                        break;
                    default:
                        break;
                }
                if (!result)
                {
                    break;
                }
            }
            return result;
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
    }
}
