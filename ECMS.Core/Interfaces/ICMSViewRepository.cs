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
        ECMSView Get(ECMSView view_);
        List<ECMSView> GetAll(ECMSView view_);
        void Update(ECMSView view_);
        void Delete(ECMSView view_);
    }
}
