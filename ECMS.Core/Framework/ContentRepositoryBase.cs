using ECMS.Core.Entities;
using ECMS.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECMS.Core.Framework
{
    public abstract class ContentRepositoryBase : IContentRepository
    {
        public static dynamic VerifyContentWithDefault(object obj_) // TODO :  Decide Type
        {
            throw new NotImplementedException();
        }

        public abstract ContentBase GetById(Guid id_);

        public abstract ContentBase GetByUrl(string incomingUrl_);

        public abstract void Save(ContentBase content_);

        public abstract void Delete(ContentBase content_);
    }
}
