using ECMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECMS.Core.Interfaces
{
    public interface IViewRepository
    {
        void Save(ECMSView view_);
        ECMSView GetById(Guid id);
        ECMSView GetByViewName(string viewName_);
        List<ECMSView> GetAll(int siteId_);
        List<ECMSView> GetAllArchieved(int siteId_,string viewName_);
        ECMSView GetArchieved(int siteId_, Guid id_);
        void Update(ECMSView view_);
        void Delete(ECMSView view_);
    }
}
