﻿using ECMS.Core.Entities;
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
        ValidUrl GetById(Guid urlId_);
        ValidUrl GetByFriendlyUrl(int siteId_, string friendlyurl_, bool isPublish_);
        ValidUrl GetById(int siteId_, Guid urlId_, bool isPublish_);
        List<ValidUrl> GetAll(int siteId_, bool isPublish_);
        long GetTotalUrlCount(int siteId_);
        Tuple<long, List<ValidUrl>> FindAndGetAll(int siteId_, string searchField, string searchString_, string searchOperator, string sortField, string sortDirection_, int pageNo_, int records_, bool isSearchRq_);
        void Save(ValidUrl url_);
        void Update(ValidUrl url_);
        void Delete(ValidUrl url_);
    }
}
