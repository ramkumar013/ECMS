using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECMS.Core
{
    public class ECMSSettings
    {
        public const string DEFAULT_LOGGER = "default";
        public const string HTTPERROR_LOCALE_PREFIX = "HTTPError.";
        public static Dictionary<int, ECMSSettings> AppSettings = null;

        public string CDNPath { get; set; }
        static ECMSSettings()
        {
            BeginLoadAppSettings();
        }

        private static void BeginLoadAppSettings()
        {
            AppSettings = new Dictionary<int, ECMSSettings>();
            string[] dirinfo = Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory);
            foreach (string dir in dirinfo)
            {
                AppSettings.Add(Convert.ToInt32(new DirectoryInfo(dir).Name), new ECMSSettings());
            }
        }

        private static ECMSSettings LoadAppSettings(string dirPath_)
        {
            //ECMSSettings setting = new ECMSSettings();
            //return setting;
            throw new NotImplementedException();
        }
    }
}
