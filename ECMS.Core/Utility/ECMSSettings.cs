using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ECMS.Core
{
    public class ECMSSettings
    {
        #region Constants
        public const string DEFAULT_LOGGER = "default";
        public const string HTTPERROR_LOCALE_PREFIX = "HTTPError."; 
        #endregion

        #region Static Properties
        public static Dictionary<int, ECMSSettings> CMSSettingsList { get; set; }
        #endregion

        #region Instance Properties
        public string CDNPath { get; set; }
        public string AppBasePath { get; set; }
        public string HostAliases { get; set; }
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
            CMSSettingsList = new Dictionary<int, ECMSSettings>();
            string[] dirinfo = Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory + "\\configs");
            foreach (string dir in dirinfo)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dir);
                CMSSettingsList.Add(Convert.ToInt32(dirInfo.Name), LoadAppSettings(dirInfo));
            }
        }

        private static ECMSSettings LoadAppSettings(DirectoryInfo dirInfo_)
        {
            ECMSSettings setting = new ECMSSettings();
            try
            {
                using (DataSet ds = new DataSet("Configuration"))
                {
                    ds.ReadXml(dirInfo_.FullName + "\\site.config");
                    setting.CDNPath = Convert.ToString(ds.Tables["configuration"].Rows[0]["CDNPath"]) + dirInfo_.Name;
                    setting.AppBasePath = AppDomain.CurrentDomain.BaseDirectory;
                    setting.HostAliases = Convert.ToString(ds.Tables["configuration"].Rows[0]["HttpAliases"]);
                } 
            }
            catch (Exception ex)
            {
                DependencyManager.Logger.Debug(string.Format("Error while reading config file at : {0}", dirInfo_.FullName) + "\r\n" + ex.ToString());
            }
            return setting;
        }  
        #endregion

        /// <summary>
        /// Site id must be injected either from app_begin_request or action filter to make ECMSSettings of current portal available using this static property.
        /// </summary>
        public static ECMSSettings Current { 


            
            get{
                if (HttpContext.Current != null && HttpContext.Current.Items["siteid"]!=null)
                {
                    return CMSSettingsList[Convert.ToInt32(HttpContext.Current.Items["siteid"])];
                }
                else 
                {
                    return null;
                }
            }
        }

        public static ECMSSettings GetCurrentBySiteId(int siteId_)
        {
            return CMSSettingsList[siteId_];
        }
    }
}
