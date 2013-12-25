using ECMS.Core.Entities;
using ECMS.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECMS.Core.Interfaces
{
    public interface IContentRepository
    {
        ContentItem GetById(ValidUrl url_,ContentViewType viewType_);
        ContentItem GetByUrl(ValidUrl url_, ContentViewType viewType_);
        void Save(ContentItem content_, ContentViewType viewType_);
        void Save(ContentItem content_, ECMSView view_);
        void Delete(ContentItem content_, ContentViewType viewType_);
        ContentItemHead GetHeadContentByViewName(ValidUrl url_, ContentViewType viewType_);
        ContentItem GetContentForEditing(ECMSView view_);
    }
}
