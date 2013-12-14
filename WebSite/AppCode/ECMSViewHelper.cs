using ECMS.Core;
using ECMS.Core.Entities;
using ECMS.HttpModules;
using NLog;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
namespace WebApp.AppCode
{
    public class ECMSViewHelper
    {
        public static string Eval(string expression)
        {
            try
            {
                string hashCode = expression.GetHashCode().ToString();
                var task = DependencyManager.CachingService.Get<Task>("Task." + hashCode);
                if (task != null && !task.IsCompleted)
                {
                    task.Wait();
                }

                TemplateService service = new TemplateService();
                return service.Run(DependencyManager.CachingService.Get<ITemplate>(hashCode), null);
            }
            catch (Exception ex)
            {
                ValidUrl validurl = Utility.GetValidUrlFromContext(new HttpContextWrapper(HttpContext.Current));
                string url = validurl != null ? validurl.FriendlyUrl + "::" + validurl.Id.ToString() + "::" : string.Empty;
                LogEventInfo info = new LogEventInfo(LogLevel.Error, ECMSSettings.DEFAULT_LOGGER, url + ex.ToString());
                DependencyManager.Logger.Log(info);
                return string.Empty;
            }
        }

        public static string GetHref(string url_,string text, string attributes_)
        {
            // TODO : Check if url is active or not.
            return string.Format("<a href={0}>{1}</a>", url_, text);
        }
    }
}