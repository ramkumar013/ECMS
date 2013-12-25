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
        public abstract ContentItem GetById(ValidUrl url_, ContentViewType viewType_);

        public abstract ContentItem GetByUrl(ValidUrl url_, ContentViewType viewType_);

        public abstract void Save(ContentItem content_, ContentViewType viewType_);

        public abstract void Delete(ContentItem content_, ContentViewType viewType_);

        public abstract ContentItemHead GetHeadContentByViewName(ValidUrl url_, ContentViewType viewType_);

        public abstract void Save(ContentItem content_, ECMSView view_);

        public abstract ContentItem GetContentForEditing(ECMSView view_);

        public abstract ContentItem GetContentForEditing(ValidUrl url_, ContentViewType viewType_);
    }
}
