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

        /// <summary>
        /// Save url specific content.
        /// </summary>
        /// <param name="content_"></param>
        void Save(ContentItem content_, ContentViewType viewType_);
        /// <summary>
        /// Save default content to a view.
        /// </summary>
        /// <param name="content_"></param>
        /// <param name="viewType_"></param>
        void Save(ContentItem content_, ECMSView view_);
        
        void Delete(ContentItem content_, ContentViewType viewType_);
        ContentItemHead GetHeadContentByViewName(ValidUrl url_, ContentViewType viewType_);
        ContentItem GetContentForEditing(ECMSView view_);
        ContentItem GetContentForEditing(ValidUrl url_, ContentViewType viewType_);
    }
}
