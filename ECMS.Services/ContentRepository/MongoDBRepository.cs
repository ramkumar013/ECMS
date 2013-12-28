using ECMS.Core.Entities;
using ECMS.Core.Framework;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.IO;
using CsvHelper;
namespace ECMS.Services.ContentRepository
{
    public class MongoDBRepository : ContentRepositoryBase
    {
        private static MongoDatabase _db = null;
        private const string COLLNAME = "PageContent";
        private const string ARC_COLLNAME = "ARCPageContent";
        static MongoDBRepository()
        {
            _db = MongoHelper.GetMongoDB();

            BsonClassMap.RegisterClassMap<ContentItemHead>(cm => cm.AutoMap());

            BsonClassMap.RegisterClassMap<ContentItem>(cm =>
            {
                cm.AutoMap();
                cm.SetIdMember(cm.GetMemberMap(c => c.Id));
                cm.IdMemberMap.SetIdGenerator(MongoDB.Bson.Serialization.IdGenerators.GuidGenerator.Instance);
                //cm.UnmapProperty(c => c.Url.Action);
                //cm.UnmapProperty(c => c.Url.Active);
                //cm.UnmapProperty(c => c.Url.ChangeFrequency);
                //cm.UnmapProperty(c => c.Url.LastModified);
                //cm.UnmapProperty(c => c.Url.LastModifiedBy);
                //cm.UnmapProperty(c => c.Url.SitemapPriority);
                //cm.UnmapProperty(c => c.Url.StatusCode);
                //cm.UnmapProperty(c => c.Url.Index);
                //cm.MapMember(c => c.Head);
                //cm.MapMember(c => c.Body);
                //cm.MapMember(c => c.LastModifiedBy);
                //cm.MapMember(c => c.LastModifiedOn);
                //cm.MapMember(c => c.ContentId);
                //cm.MapMember(c => c.ViewType);
                //cm.MapMember(c => c.Url.Id);
                //cm.MapMember(c => c.Url.View);
            }); 
        }

        public override ContentItem GetById(ValidUrl url_, ContentViewType viewType_)
        {
            //ContentItem item = _db.GetCollection<ContentItem>(COLLNAME).AsQueryable<ContentItem>().Where(x => x.Url.Id == url_.Id && x.ContentView.ViewType == viewType_).FirstOrDefault<ContentItem>();
            ContentItem item = _db.GetCollection<ContentItem>(COLLNAME).Find(Query.And(Query.EQ("Url.Id", url_.Id), Query.EQ("ViewType", Convert.ToInt32(viewType_)))).FirstOrDefault<ContentItem>();
            
            if (item == null)
            {
                item = _db.GetCollection<ContentItem>(COLLNAME).Find(Query.And(Query.EQ("ContentView.SiteId", url_.SiteId), Query.EQ("ContentView.ViewName", url_.View), Query.EQ("ContentView.ViewType", Convert.ToInt32(viewType_)))).FirstOrDefault<ContentItem>();
            }

            //TODO : Optimize this
            if (item != null)
            {
                using (StringReader streamReader = new StringReader(item.Body[0].ToString()))
                {
                    using (var csv = new CsvReader(streamReader))
                    {
                        csv.Read();
                        item.Body = JObject.FromObject(csv.GetRecord<object>());
                    }
                }
            }
            return item;
        }

        public override ContentItem GetByUrl(ValidUrl url_, ContentViewType viewType_)
        {
            throw new NotImplementedException();
        }

        public override void Save(ContentItem content_, ContentViewType viewType_)
        {
            //ContentItem previousItem = _db.GetCollection<ContentItem>(COLLNAME).AsQueryable<ContentItem>().Where(x => x.ContentId ==  content_.ContentId && x.ViewType == viewType_).FirstOrDefault<ContentItem>();
            ContentItem previousItem = _db.GetCollection<ContentItem>(COLLNAME).Find(Query.And(Query.EQ("ContentId", content_.ContentId), Query.EQ("ViewType", Convert.ToInt32(viewType_)))).FirstOrDefault<ContentItem>();
            
            if (previousItem != null)
            {
                previousItem.ContentId = Guid.Empty;
                _db.GetCollection<ContentItem>(ARC_COLLNAME).Save<ContentItem>(previousItem);
            }
            _db.GetCollection<ContentItem>(COLLNAME).Save<ContentItem>(content_);
        }

        public override void Delete(ContentItem content_, ContentViewType viewType_)
        {
            throw new NotImplementedException();
        }

        public override ContentItemHead GetHeadContentByViewName(ValidUrl url_, ContentViewType viewType_)
        {
            throw new NotImplementedException();
            //_db.GetCollection<ContentItem>(COLLNAME).AsQueryable().Where(x=>x.ViewType==viewType_ && x.)
        }

        public override void Save(ContentItem content_, ECMSView view_)
        {
            content_.ContentView = view_;
            Save(content_, view_.ViewType);
        }

        public override ContentItem GetContentForEditing(ECMSView view_)
        {
            //return _db.GetCollection<ContentItem>(COLLNAME).AsQueryable<ContentItem>().Where(x => x.Url != null && x.Url.SiteId == view_.SiteId && x.Url.View == view_.ViewName && Convert.ToInt32(x.ViewType) == Convert.ToInt32(view_.ViewType)).FirstOrDefault<ContentItem>();
            ContentItem item = _db.GetCollection<ContentItem>(COLLNAME).Find(Query.And(Query.EQ("ContentView.SiteId", view_.SiteId), Query.EQ("ContentView.ViewName", view_.ViewName), Query.EQ("ContentView.ViewType", Convert.ToInt32(view_.ViewType)))).FirstOrDefault<ContentItem>();
            if (item!=null)
            {
                item.Body = item.Body[0];
            }
            return item;
        }

        public override ContentItem GetContentForEditing(ValidUrl url_, ContentViewType viewType_)
        {
            ContentItem item = _db.GetCollection<ContentItem>(COLLNAME).AsQueryable<ContentItem>().Where(x => x.Url.Id == url_.Id && x.Url.View == url_.View && x.ContentView.ViewType == viewType_).FirstOrDefault<ContentItem>();
            if (item != null)
            {
                item.Body = item.Body[0];
            }
            return item;
        }
    }
}
