using ECMS.Core;
using ECMS.Core.Entities;
using ECMS.Core.Utilities;
using NLog;
using RazorEngine.Templating;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Data;
using System.Web.Mvc;
using System.Globalization;

namespace ECMS.WebV2
{
    public class ViewHelper
    {
        private static RegexOptions ro = new RegexOptions();
        private static Regex hrefRegex = null;
        private static MvcHtmlString emptyMVCHtmlString = new MvcHtmlString(string.Empty);
        static ViewHelper()
        {
            ro = RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled;

            hrefRegex = new Regex("<a.*?(href)=[\"'](?<url>.*?)(\"\\s|'\\s|\">|'>)(?<=>)(?<name>.*?[^<])</a>", ro);
        }


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

        public static MvcHtmlString GetHref(string url_, string hrefTemplate_)
        {
            try
            {
                ValidUrl vu = DependencyManager.URLRepository.GetByFriendlyUrl(1, url_);
                if (vu != null && vu.StatusCode == 200)
                {
                    return new MvcHtmlString(string.Format(hrefTemplate_, vu.FriendlyUrl));
                }
                else
                {
                    Match match = hrefRegex.Match(hrefTemplate_);
                    if (match != null && match.Groups != null && match.Groups.Count > 0 && match.Groups["name"] != null)
                    {
                        return new MvcHtmlString(match.Groups["name"].Value);
                    }
                    else
                    {
                        return emptyMVCHtmlString;
                    }
                }
            }
            catch (Exception ex)
            {
                LogEventInfo info = new LogEventInfo(LogLevel.Error, ECMSSettings.DEFAULT_LOGGER, ex.ToString());
                DependencyManager.Logger.Log(info);
                return emptyMVCHtmlString;
            }
        }

        public static MvcHtmlString GetBaseDataVal(string expression_)
        {
            try
            {
                string[] temp = expression_.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                string tableName = temp[0];
                string lookUpColumnName = temp[1];
                string valueToLookUp = temp[2];
                string returnCol = temp[3];
                var result = from item in ECMSSettings.Current.DomainData.Tables[tableName].AsEnumerable()
                             where item[lookUpColumnName].Equals(valueToLookUp)
                             select item[returnCol];

                return new MvcHtmlString(result.FirstOrDefault().ToString());
            }
            catch (Exception ex)
            {
                LogEventInfo info = new LogEventInfo(LogLevel.Error, ECMSSettings.DEFAULT_LOGGER, ex.ToString());
                DependencyManager.Logger.Log(info);
                return emptyMVCHtmlString;
            }
        }

        

        public static MvcHtmlString GetQueryString(string p1)
        {
            return new MvcHtmlString(HttpContext.Current.Request.QueryString[p1]);
        }
    }
}