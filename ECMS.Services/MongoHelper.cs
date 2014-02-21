using ECMS.Core;
using MongoDB.Driver;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECMS.Services
{
    public class MongoHelper
    {
        public static MongoDatabase GetMongoDB()
        {
            string _mongoHostIP = ConfigurationManager.ConnectionStrings["MongoDBHost"].ConnectionString;
            int _mongoHostPort = Convert.ToInt32(ConfigurationManager.ConnectionStrings["MongoDBPort"].ConnectionString);
            string _dbName = ConfigurationManager.AppSettings["MongoDBName"];
            MongoServer _mongoServer = null;
            try
            {
                var dbCredential = MongoCredential.CreateMongoCRCredential(ConfigurationManager.AppSettings["MongoDBName"], "ecmsweb", "ecms@w3b");

                _mongoServer = new MongoServer(
                    new MongoServerSettings
                    {
                        Server = new MongoServerAddress(_mongoHostIP, _mongoHostPort),
                        Credentials = new[]
                            {
                                dbCredential
                            },
                        ConnectionMode = ConnectionMode.Automatic,
                        ConnectTimeout = new TimeSpan(0, 10, 0),
                        MaxConnectionIdleTime = new TimeSpan(0, 120, 0),
                        MaxConnectionLifeTime = new TimeSpan(0, 120, 0),
                        MaxConnectionPoolSize = 100,
                        MinConnectionPoolSize = 10,
                        SocketTimeout = new TimeSpan(0, 10, 0),
                        WriteConcern = WriteConcern.Acknowledged,
                        WaitQueueSize = 10,
                        WaitQueueTimeout = new TimeSpan(0, 10, 0),
                    });
                return _mongoServer.GetDatabase(_dbName);
            }
            catch (Exception ex)
            {
                DependencyManager.Logger.Log(new LogEventInfo(LogLevel.Error, ECMSSettings.DEFAULT_LOGGER, "Error while connecting to MongoDB : " + ex.ToString()));
                throw ex;
            }
        }
    }
}
