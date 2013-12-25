using ECMS.Core.Entities;
using ECMS.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECMS.Services.ContentRepository
{
    public class MongoDBRepository : ContentRepositoryBase
    {
        public override ContentItem GetById(ValidUrl url_, ContentViewType viewType_)
        {
            throw new NotImplementedException();
        }

        public override ContentItem GetByUrl(ValidUrl url_, ContentViewType viewType_)
        {
            throw new NotImplementedException();
        }

        public override void Save(ContentItem content_, ContentViewType viewType_)
        {
            throw new NotImplementedException();
        }

        public override void Delete(ContentItem content_, ContentViewType viewType_)
        {
            throw new NotImplementedException();
        }

        public override ContentItemHead GetHeadContentByViewName(ValidUrl url_, ContentViewType viewType_)
        {
            throw new NotImplementedException();
        }

        public override void Save(ContentItem content_, Core.Entities.ECMSView view_)
        {
            throw new NotImplementedException();
        }

        public override ContentItem GetContentForEditing(ECMSView view_)
        {
            throw new NotImplementedException();
        }

        public override ContentItem GetContentForEditing(ValidUrl url_, ContentViewType viewType_)
        {
            throw new NotImplementedException();
        }
    }
}
