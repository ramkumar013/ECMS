using ECMS.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECMS.Core.Entities;

namespace ECMS.Services.ValidUrlService
{
    public class ValidUrlFileRepository : IValidURLRepository
    {
        public ValidUrl GetByFriendlyUrl(int siteId_, string friendlyurl_)
        {
            throw new NotImplementedException();
        }

        public ValidUrl GetById(int siteId_, string friendlyurl_)
        {
            throw new NotImplementedException();
        }

        public ValidUrl GetByFriendlyUrl(int siteId_, string friendlyurl_, bool useCache_)
        {
            throw new NotImplementedException();
        }

        public ValidUrl GetById(int siteId_, string friendlyurl_, bool useCache_)
        {
            throw new NotImplementedException();
        }

        public void Save(ValidUrl url_)
        {
            throw new NotImplementedException();
        }

        public void Update(ValidUrl url_)
        {
            throw new NotImplementedException();
        }

        public void Delete(ValidUrl url_)
        {
            throw new NotImplementedException();
        }
    }
}
