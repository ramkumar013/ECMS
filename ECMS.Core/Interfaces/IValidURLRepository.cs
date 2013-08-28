using ECMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECMS.Core.Interfaces
{
    public interface IValidURLRepository
    {
        ValidUrl GetByFriendlyUrl(int siteId_,string friendlyurl_);
        ValidUrl GetById(int siteId_, string friendlyurl_);
        ValidUrl GetByFriendlyUrl(int siteId_, string friendlyurl_, bool useCache_);
        ValidUrl GetById(int siteId_, string friendlyurl_, bool useCache_);
        void Save(ValidUrl url_);
        void Update(ValidUrl url_);
        void Delete(ValidUrl url_);
    }
}
