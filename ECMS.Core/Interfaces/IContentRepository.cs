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
        ContentItem GetById(ValidUrl url_);
        ContentItem GetByUrl(ValidUrl url_);
        void Save(ContentItem content_);
        void Delete(ContentItem content_);
        ContentItemHead GetHeadContentByViewName(ValidUrl url_);
    }
}
