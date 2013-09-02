using ECMS.Core.Interfaces;
using ECMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECMS.Services.ContentRepository
{
    public class FileSystemRepository : IContentRepository
    {
        public ContentBase GetById(Guid id_)
        {
            throw new NotImplementedException();
        }

        public ContentBase GetByUrl(string incomingUrl_)
        {
            throw new NotImplementedException();
        }

        public void Save(ContentBase content_)
        {
            throw new NotImplementedException();
        }

        public void Delete(ContentBase content_)
        {
            throw new NotImplementedException();
        }
    }
}
