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
namespace ECMS.Core.Utility
{
    public class ECMSUtility
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
    }
}
