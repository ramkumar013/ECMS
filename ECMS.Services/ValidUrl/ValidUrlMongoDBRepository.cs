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
using MongoDB.Bson;
using System.Reflection;
using System.IO;
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
            return GetByFriendlyUrl(siteId_, friendlyurl_, true);
        }

        public ValidUrl GetById(Guid urlId_)
        {
            throw new NotImplementedException();
        }

        public ValidUrl GetByFriendlyUrl(int siteId_, string friendlyurl_, bool isPublish_)
        {
            if (isPublish_)
            {
                return _db.GetCollection<ValidUrl>(GetCollName(siteId_)).AsQueryable().Where(x => x.SiteId == siteId_ && x.FriendlyUrl == friendlyurl_.ToLower() && x.Active == true).FirstOrDefault();
            }
            else
            {
                return _db.GetCollection<ValidUrl>(GetCollName(siteId_)).AsQueryable().Where(x => x.SiteId == siteId_ && x.FriendlyUrl == friendlyurl_.ToLower()).FirstOrDefault();
            }
        }

        public ValidUrl GetById(int siteId_, Guid urlId_, bool isPublish_)
        {
            // when we are picking by id then donot use active in the where clause as it may slow the performance.
            return _db.GetCollection<ValidUrl>(GetCollName(siteId_)).AsQueryable().Where(x => x.SiteId == siteId_ && x.Id == urlId_).FirstOrDefault();
        }
        public List<ValidUrl> GetAll(int siteId_, bool isPublish_)
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


        public Tuple<long, List<ValidUrl>> FindAndGetAll(int siteId_, string searchField, string searchString_,string searchOperator, string sortField, string sortDirection_, int pageNo_, int records_, bool isSearchRq_)
        {
            Tuple<long, List<ValidUrl>> result = null;
            if (isSearchRq_)
            {
                QueryBuilder<ValidUrl> builder = new QueryBuilder<ValidUrl>();
                IMongoQuery query = Query.EQ("SiteId",siteId_);
                
                switch (searchOperator)
                {
                    case "cn":
                        throw new NotImplementedException();    
                        break;
                    case "bw":
                        query = builder.And(query, Query.Matches(searchField, new BsonRegularExpression("^" + searchString_, "i")));
                        break;
                    case "ew":
                        query = builder.And(query, Query.Matches(searchField, new BsonRegularExpression(searchString_ + "$", "i")));
                        break;                    
                    case "lt":
                        query = builder.And(query, Query.LT(searchField, searchString_));
                        break;
                    case "gt":
                        query = builder.And(query, Query.GT(searchField, searchString_));
                        break;
                    case "ne":
                        query = builder.And(query, Query.NE(searchField, searchString_));
                        break;
                    case "eq":                        
                    default:
                        query = builder.And(query, Query.EQ(searchField, searchString_));
                        break;
                }

                var filterDocuments = _db.GetCollection<ValidUrl>(GetCollName(siteId_)).Find(query).AsQueryable().OrderByDescending(y => y.LastModified);
                //.Skip((pageNo_-1*records_)).Take(records_).ToList<ValidUrl>();
                result = new Tuple<long, List<ValidUrl>>(filterDocuments.Count(), filterDocuments.Skip((pageNo_ - 1 * records_)).Take(records_).ToList<ValidUrl>());
            }
            else
            {
                var query = (from c in _db.GetCollection<ValidUrl>(GetCollName(siteId_)).AsQueryable()
                             orderby c.LastModified descending
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

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Directory.GetParent(Path.GetDirectoryName(path)).FullName;
            }
        }
    }
}

