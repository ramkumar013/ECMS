using ECMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECMS.Core.Interfaces
{
    public interface IContentRepository
    {
        ContentBase GetById(Guid id_);
        ContentBase GetByUrl(string incomingUrl_);
        void Save(ContentBase content_);
        void Delete(ContentBase content_);
    }
}
