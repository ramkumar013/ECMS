using System;
using System.Collections.Generic;
using System.Linq;
using ECMS.Core.Interfaces;
using ECMS.Core.Entities;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using System.IO;
using MongoDB.Driver.Linq;
namespace ECMS.Services
{
    public class ECMSViewRepository : IViewRepository
    {
        private static MongoDatabase _db = null;
        static ECMSViewRepository()
        {
            _db = MongoHelper.GetMongoDB();
            BsonClassMap.RegisterClassMap<ECMSView>(cm =>
            {
                cm.AutoMap();
                cm.SetIdMember(cm.GetMemberMap(c => c.Id));
                cm.IdMemberMap.SetIdGenerator(MongoDB.Bson.Serialization.IdGenerators.GuidGenerator.Instance);
            });
        }
        public void Save(ECMSView view_)
        {   
            File.WriteAllText(GetViewPath(view_), view_.ViewHtml);
            _db.GetCollection<ECMSView>(typeof(ECMSView).ToString()).Save<ECMSView>(view_);
        }

        public ECMSView Get(ECMSView view_)
        {
            view_ = _db.GetCollection<ECMSView>(typeof(ECMSView).ToString()).AsQueryable().Where(x => x.Id == view_.Id).FirstOrDefault<ECMSView>();
            view_.ViewHtml = File.ReadAllText(GetViewPath(view_));
            return view_;
        }

        public void Update(ECMSView view_)
        {
            Save(view_);
        }

        public void Delete(ECMSView view_)
        {
            File.Delete(GetViewPath(view_));
        }

        private static string GetViewPath(ECMSView view_)
        {
            string dirPath = ECMS.Core.ECMSSettings.GetCurrentBySiteId(view_.SiteId).AppBasePath + "Views\\" + view_.SiteId + "\\" + Convert.ToInt32(view_.ViewType);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            return dirPath + "\\" + view_.ViewName + ".cshtml"; ;
        }


        public List<ECMSView> GetAll(int siteId_)
        {
            return _db.GetCollection<ECMSView>(typeof(ECMSView).ToString()).AsQueryable<ECMSView>().Where(x => x.SiteId == siteId_).ToList<ECMSView>();
        }
    }
}
