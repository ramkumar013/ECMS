using ECMS.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECMS.Core.Entities;
using System.Configuration;
using MongoDB.Driver;
using FluentMongo.Linq;
using MongoDB.Driver.Builders;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using ECMS.Core;
namespace ECMS.Services
{
    public class ValidUrlMongoDBRepository : IValidURLRepository
    {
        private static string _mongoHostIP = ConfigurationManager.ConnectionStrings["MongoDBHost"].ConnectionString;
        private static int _mongoHostPort = Convert.ToInt32(ConfigurationManager.ConnectionStrings["MongoDBPort"].ConnectionString);
        private static string _dbName = ConfigurationManager.AppSettings["MongoDBName"];
        private static MongoServer _mongoServer = null;
        private static MongoDatabase _db = null;
        static ValidUrlMongoDBRepository()
        {
            MongoServerSettings serverSettings = new MongoServerSettings();
            serverSettings.ConnectionMode = ConnectionMode.Automatic;
            serverSettings.ConnectTimeout = new TimeSpan(0, 0, 10);
            serverSettings.MaxConnectionIdleTime = new TimeSpan(0, 30, 0);
            serverSettings.MaxConnectionLifeTime = new TimeSpan(0, 30, 0);
            serverSettings.MaxConnectionPoolSize = 10;
            serverSettings.MinConnectionPoolSize = 1;
            serverSettings.SocketTimeout = new TimeSpan(0, 0, 10);
            serverSettings.WriteConcern = WriteConcern.Acknowledged;
            serverSettings.WaitQueueSize = 10;
            serverSettings.WaitQueueTimeout = new TimeSpan(0, 0, 10);
            serverSettings.Server = new MongoServerAddress(_mongoHostIP, _mongoHostPort);
            _mongoServer = new MongoServer(serverSettings);

            BsonClassMap.RegisterClassMap<ValidUrl>(cm =>
            {
                cm.AutoMap();
                cm.SetIdMember(cm.GetMemberMap(c => c.Id));
                cm.IdMemberMap.SetIdGenerator(MongoDB.Bson.Serialization.IdGenerators.NullIdChecker.Instance);
            });

            _db = _mongoServer.GetDatabase(_dbName);


            //TODO: Add appropriate indexes on each collection
            //foreach (var item in ECMSSettings.CMSSettingsList)
            //{
            //    _db.GetCollection<ValidUrl>(GetCollName(item.Key)).EnsureIndex();
            //} 

            
        }
        public ValidUrl GetByFriendlyUrl(int siteId_, string friendlyurl_)
        {
            return _db.GetCollection<ValidUrl>(GetCollName(siteId_)).AsQueryable().Where(x => x.FriendlyUrl == friendlyurl_.ToLower()).FirstOrDefault();
        }

        public ValidUrl GetById(int urlId_)
        {
            throw new NotImplementedException();
        }

        public ValidUrl GetByFriendlyUrl(int siteId_, string friendlyurl_, bool useCache_)
        {
            throw new NotImplementedException();
        }

        public ValidUrl GetById(int siteId_, string friendlyurl_, bool useCache_)
        {
            throw new NotImplementedException();
        }

        public List<ValidUrl> GetAll(int siteId_, bool useCache_)
        {
            return _db.GetCollection<ValidUrl>(GetCollName(siteId_)).AsQueryable().ToList<ValidUrl>();
        }

        public void Save(ValidUrl url_)
        {
            WriteConcernResult result =  _db.GetCollection<ValidUrl>(GetCollName(url_.SiteId)).Save<ValidUrl>(url_);
        }

        public void Update(ValidUrl url_)
        {
            Save(url_);
        }

        public void Delete(ValidUrl url_)
        {
            WriteConcernResult result = _db.GetCollection<ValidUrl>(GetCollName(url_.SiteId)).Remove(Query.EQ("Id", url_.Id),RemoveFlags.Single);
        }

        private static string GetCollName(int siteId_)
        {
            return string.Format("ValidUrl{0}", siteId_.ToString());
        }
    }
}

