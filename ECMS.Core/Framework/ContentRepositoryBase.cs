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
        public static dynamic VerifyContentWithDefault(dynamic contentItem_) // TODO :  Decide Type
        {
            //contentItem_.Title = contentItem_.Title + ".. ok pass";
            //contentItem_.Keywords = contentItem_.Keywords + ".. ok pass";
            //contentItem_.Description = contentItem_.Description + ".. ok pass";
            //return contentItem_;

            throw new NotImplementedException();
        }

        public abstract ContentBase GetById(Guid id_);

        public abstract ContentBase GetByUrl(string incomingUrl_);

        public abstract void Save(ContentBase content_);

        public abstract void Delete(ContentBase content_);
    }
}
