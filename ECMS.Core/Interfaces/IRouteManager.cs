using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using ECMS.Core.Entities;

namespace ECMS.Core.Interfaces
{
    public interface IRouteManager
    {
        List<AppRoute> GetAllRoutes(int siteId_);
    }
}
