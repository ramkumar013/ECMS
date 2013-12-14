using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using  ECMS.Core.Interfaces;
using ECMS.Core.Entities;
using System.Web;
using System.Reflection;
using System.Web.Configuration;
using System.IO;
using Newtonsoft.Json.Linq;
namespace ECMS.Core.Utilities
{
    public class Utility
    {
        public static int GetSiteId(string host)
        {
            var settings = ECMSSettings.CMSSettingsList.Values.ToList();
            foreach (KeyValuePair<int, ECMSSettings> pair in ECMSSettings.CMSSettingsList)
            {
                if (pair.Value.HostAliases.Split(new char[] { ',' }).Any(x => x == host))
                {
                    return pair.Key;
                }
            }
            return -1;
        }
        public static ValidUrl GetValidUrlFromContext(HttpContextBase contextBase_)
        {
            return (contextBase_.Items["validUrl"] != null ? contextBase_.Items["validUrl"] as ValidUrl : null);
        }

        public static string GetClientIP()
        {
            if (HttpContext.Current != null)
            {
                return "255.255.255.255";
            }
            if (HttpContext.Current.Request.ServerVariables["HTTP_TRUE_CLIENT_IP"] != null)
            {
                return HttpContext.Current.Request.ServerVariables["HTTP_TRUE_CLIENT_IP"];
            }
            if (HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] != null)
            {
                return HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
            }

            return HttpContext.Current.Request.UserHostAddress;
        }
    }
}
