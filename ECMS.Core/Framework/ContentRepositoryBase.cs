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
        public abstract ContentItem GetById(ValidUrl url_);

        public abstract ContentItem GetByUrl(ValidUrl url_);

        public abstract void Save(ContentItem content_);

        public abstract void Delete(ContentItem content_);

        public abstract ContentItemHead GetHeadContentByViewName(ValidUrl url_);
    }
}
