using System;
using System.Collections.Generic;
using System.Linq;
using ECMS.Core.Interfaces;
using ECMS.Core.Entities;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using System.IO;
using MongoDB.Driver.Linq;
using ECMS.Core.Framework;
using MongoDB.Driver.Builders;
namespace ECMS.Services
{
    public class ECMSViewRepository : IViewRepository
    {
        private static MongoDatabase _db = null;
        private  const string COLLNAME = "EcmsViewContent";
        private  const string ARC_COLLNAME = "ARCEcmsViewContent";
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
            File.WriteAllText(GetViewPath(view_), view_.Html);
            _db.GetCollection<ECMSView>(COLLNAME).Save<ECMSView>(view_);
        }

        //public ECMSView Get(ECMSView view_)
        //{
           
        //}


        public void Update(ECMSView view_)
        {
            // archieve publish view changes only.
            if (view_.ViewType == ContentViewType.PUBLISH)
            {
                // first archieve the content.
                ECMSView previousPublishedView = _db.GetCollection<ECMSView>(COLLNAME).AsQueryable().Where(x => x.Id == view_.Id).ToList<ECMSView>().Where(x => x.ViewType == ContentViewType.PUBLISH).FirstOrDefault<ECMSView>();
                if (previousPublishedView != null)
                {
                    previousPublishedView.Id = Guid.Empty;
                    previousPublishedView.LastModifiedBy = view_.LastModifiedBy;
                    previousPublishedView.LastModifiedOn = view_.LastModifiedOn;
                    _db.GetCollection<ECMSView>(ARC_COLLNAME).Save<ECMSView>(previousPublishedView);                    
                }

                // save the original content as well.
                Save(view_);

                //then update the same on preview mode.
                ECMSView previewView = _db.GetCollection<ECMSView>(COLLNAME).Find(Query.And(Query.EQ("ViewType", ContentViewType.PREVIEW), Query.EQ("ViewName", view_.ViewName))).FirstOrDefault<ECMSView>();
                if (previewView != null)
                {
                    previewView.LastModifiedBy = view_.LastModifiedBy;
                    previewView.LastModifiedOn = view_.LastModifiedOn;
                    previewView.Html = view_.Html;
                    Save(previewView);
                }
                else
                {
                    // preview mode content not found then simple create a preview mode.
                    view_.Id = Guid.Empty;
                    view_.ViewType = ContentViewType.PREVIEW;
                    Save(view_);
                }
            }
            else
            {
                // if preview then simple save as it is.
                Save(view_);
            }
        }

        public void Delete(ECMSView view_)
        {
            //File.Delete(GetViewPath(view_));
            throw new NotImplementedException();
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
            return _db.GetCollection<ECMSView>(COLLNAME).AsQueryable<ECMSView>().Where(x => x.SiteId == siteId_).ToList<ECMSView>();
        }

        public List<ECMSView> GetAllArchieved(int siteId_, string viewName_)
        {
            return _db.GetCollection<ECMSView>(ARC_COLLNAME).AsQueryable<ECMSView>().Where(x => x.SiteId == siteId_ && x.ViewName == viewName_).ToList<ECMSView>();
        }

        public ECMSView GetArchieved(int siteId_, Guid id_)
        {
            return _db.GetCollection<ECMSView>(ARC_COLLNAME).AsQueryable<ECMSView>().Where(x => x.SiteId == siteId_ && x.Id == id_).FirstOrDefault();
        }


        public ECMSView GetById(Guid id_)
        {
            ECMSView view = _db.GetCollection<ECMSView>(COLLNAME).AsQueryable().Where(x => x.Id == id_).FirstOrDefault<ECMSView>();
            view.Html = File.ReadAllText(GetViewPath(view));
            return view;
        }

        public ECMSView GetByViewName(string viewName_)
        {
            ECMSView view = _db.GetCollection<ECMSView>(COLLNAME).AsQueryable().Where(x => x.ViewName == viewName_).FirstOrDefault<ECMSView>();
            view.Html = File.ReadAllText(GetViewPath(view));
            return view;
        }
    }
}
