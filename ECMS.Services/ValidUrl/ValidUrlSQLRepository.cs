using ECMS.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECMS.Core.Entities;

namespace ECMS.Services
{
    public class ValidUrlSQLRepository : IValidURLRepository
    {
        public ValidUrl GetByFriendlyUrl(int siteId_, string friendlyurl_)
        {
            throw new NotImplementedException();
        }

        public ValidUrl GetById(Guid urlId_)
        {
            throw new NotImplementedException();
        }

        public ValidUrl GetByFriendlyUrl(int siteId_, string friendlyurl_, bool isPublish_)
        {
            throw new NotImplementedException();
        }

        public ValidUrl GetById(int siteId_, Guid urlId_, bool isPublish_)
        {
            throw new NotImplementedException();
        }

        public List<ValidUrl> GetAll(int siteId_, bool isPublish_)
        {
            throw new NotImplementedException();
        }

        public long GetTotalUrlCount(int siteId_)
        {
            throw new NotImplementedException();
        }

        public Tuple<long, List<ValidUrl>> FindAndGetAll(int siteId_, string searchField, string searchString_, string searchOperator, string sortField, string sortDirection_, int pageNo_, int records_, bool isSearchRq_)
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
