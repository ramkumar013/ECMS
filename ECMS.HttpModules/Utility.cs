using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ECMS.HttpModules
{
    public class Utility
    {
        private static UrlValidationConfig configSection=null;
        static Utility()
        {
            configSection = (UrlValidationConfig)ConfigurationManager.GetSection("UrlValidationConfig");
        }
        public static bool IsValidUrlForRewrite(HttpContextBase httpContext)
        {
            string url = httpContext.Request.Url.AbsolutePath.ToLower();
            bool result = true;
            foreach (UrlValidationElement item in configSection.ConfigCollection)
            {
                switch (item.Action)
                {
                    case "contains":
                        string[] strs = item.InvalidValue.Split( new char[]{','});
                        result = !strs.Any(x => url.Contains(x.ToLower()));
                        break;
                    case "action":
                        result = url == item.InvalidValue;
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
    }
}
