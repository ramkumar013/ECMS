using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace ECMS.Core
{
    interface IRouteManager
    {
        List<Route> GetAllRoutes(int siteId_);
    }
}
