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
        public override Core.Entities.ContentItem GetById(Core.Entities.ValidUrl url_, ContentViewType viewType_)
        {
            throw new NotImplementedException();
        }

        public override Core.Entities.ContentItem GetByUrl(Core.Entities.ValidUrl url_, ContentViewType viewType_)
        {
            throw new NotImplementedException();
        }

        public override void Save(Core.Entities.ContentItem content_, ContentViewType viewType_)
        {
            throw new NotImplementedException();
        }

        public override void Delete(Core.Entities.ContentItem content_, ContentViewType viewType_)
        {
            throw new NotImplementedException();
        }

        public override Core.Entities.ContentItemHead GetHeadContentByViewName(Core.Entities.ValidUrl url_, ContentViewType viewType_)
        {
            throw new NotImplementedException();
        }
    }
}
