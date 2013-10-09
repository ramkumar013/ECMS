using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECMS.Core
{
    public class ECMSSettings
    {
        #region Constants
        public const string DEFAULT_LOGGER = "default";
        public const string HTTPERROR_LOCALE_PREFIX = "HTTPError."; 
        #endregion

        #region Static Properties
        public static Dictionary<int, ECMSSettings> AppSettings = null; 
        #endregion

        #region Instance Properties
        public string CDNPath { get; set; } 
        #endregion

        #region Static Constructor
        static ECMSSettings()
        {
            BeginLoadAppSettings();
        }
        #endregion

        #region Private Static Methods
        public static void BeginLoadAppSettings()
        {
            //TODO: expose this method on http.
            AppSettings = new Dictionary<int, ECMSSettings>();
            string[] dirinfo = Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory);
            foreach (string dir in dirinfo)
            {
                AppSettings.Add(Convert.ToInt32(new DirectoryInfo(dir).Name), new ECMSSettings());
            }
        }

        private static ECMSSettings LoadAppSettings(string dirPath_)
        {
            ECMSSettings setting = new ECMSSettings();
            DataSet ds = new DataSet("Configuration");
            ds.ReadXml(dirPath_);
            setting.CDNPath = Convert.ToString(ds.Tables["configuration"].Rows[0]["CDNPath"]);
            return setting;
        }  
        #endregion
       
    }
}
