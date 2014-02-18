using MongoDB.Driver;
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

            MongoServerSettings serverSettings = new MongoServerSettings();
           
            var dbCredential = MongoCredential.CreateMongoCRCredential(ConfigurationManager.AppSettings["MongoDBName"], "ecmsweb", "ecms@w3b");

            _mongoServer = new MongoServer(
                new MongoServerSettings
                {
                    Server = new MongoServerAddress(_mongoHostIP, _mongoHostPort),
                    Credentials = new[]
                            {
                                dbCredential,
                            },
                    ConnectionMode = ConnectionMode.Automatic,
                    ConnectTimeout = new TimeSpan(0, 0, 10),
                    MaxConnectionIdleTime = new TimeSpan(0, 30, 0),
                    MaxConnectionLifeTime = new TimeSpan(0, 120, 0),
                    MaxConnectionPoolSize = 10,
                    MinConnectionPoolSize = 1,
                    SocketTimeout = new TimeSpan(0, 0, 10),
                    WriteConcern = WriteConcern.Acknowledged,
                    WaitQueueSize = 10,
                    WaitQueueTimeout = new TimeSpan(0, 0, 10),
                });

            //serverSettings.ConnectionMode = ConnectionMode.Automatic;
            //serverSettings.ConnectTimeout = new TimeSpan(0, 0, 10);
            //serverSettings.MaxConnectionIdleTime = new TimeSpan(0, 30, 0);
            //serverSettings.MaxConnectionLifeTime = new TimeSpan(0, 30, 0);
            //serverSettings.MaxConnectionPoolSize = 10;
            //serverSettings.MinConnectionPoolSize = 1;
            //serverSettings.SocketTimeout = new TimeSpan(0, 0, 10);
            //serverSettings.WriteConcern = WriteConcern.Acknowledged;
            //serverSettings.WaitQueueSize = 10;
            //serverSettings.WaitQueueTimeout = new TimeSpan(0, 0, 10);
            //serverSettings.Server = new MongoServerAddress(_mongoHostIP, _mongoHostPort);
            //_mongoServer = new MongoServer(serverSettings);
            return _mongoServer.GetDatabase(_dbName);
        }
    }
}
