using ECMS.Core.Framework;
using ECMS.Core.Utilities;
using NLog;
using System.Web;

namespace ECMS.Core
{
    public class ECMSLogger
    {
        private ECMSLogger()
        { }
        private static ECMSLogger _ecmsLogger = null;
        public static ECMSLogger Instance
        {
            get
            {
                if (_ecmsLogger != null)
                {
                    return _ecmsLogger;
                }
                else
                {
                    _ecmsLogger = new ECMSLogger();
                    return _ecmsLogger;
                }
            }
        }
        public void Log(LogEventInfo info)
        {
            if (HttpContext.Current != null && HttpContext.Current.Items["LoggerName"] != null && Utility.CurrentViewType(new HttpContextWrapper(HttpContext.Current)) == ContentViewType.PREVIEW)
            {
                string _loggerName = ((System.Guid)HttpContext.Current.Items["LoggerName"]).ToString();
                Logger contextualLogger = LogManager.GetLogger(_loggerName);
                if (contextualLogger != null)
                {
                    contextualLogger.Debug(info);
                }
            }
            Logger logger = LogManager.GetLogger(ECMSSettings.DEFAULT_LOGGER);
            info.Properties.Add("ClientIP", Utility.GetClientIP());
            logger.Log(info);
        }
    }
}
