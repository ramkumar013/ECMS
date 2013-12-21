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
        private static MongoDatabase _db = null;
        static ValidUrlMongoDBRepository()
        {
            
            _db = MongoHelper.GetMongoDB();
            BsonClassMap.RegisterClassMap<ValidUrl>(cm =>
            {
                cm.AutoMap();
                cm.SetIdMember(cm.GetMemberMap(c => c.Id));
                cm.IdMemberMap.SetIdGenerator(MongoDB.Bson.Serialization.IdGenerators.NullIdChecker.Instance);
            });

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


        public Tuple<long, List<ValidUrl>> FindAndGetAll(int siteId_, string searchField, string searchOperator, string sortField, string sortDirection_, int pageNo_, int records_, bool isSearchRq_)
        {
            Tuple<long, List<ValidUrl>> result = null;
            if (isSearchRq_)
            {
                throw new NotImplementedException();    
            }
            else
            {
                var query = (from c in _db.GetCollection<ValidUrl>(GetCollName(siteId_)).AsQueryable()
                             orderby c.LastModified
                             select c)
                            .Skip((pageNo_ - 1) * records_)
                            .Take(records_);
                var list = query.ToList();
                result = new Tuple<long, List<ValidUrl>>(GetTotalUrlCount(siteId_), list);
            }
            return result;
        }


        public long GetTotalUrlCount(int siteId_)
        {
            return _db.GetCollection<ValidUrl>(GetCollName(siteId_)).Count();
        }
    }
}

