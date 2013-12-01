using ECMS.Core.Entities;
using ECMS.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECMS.Services
{
    public class RouteRepository : IRouteManager
    {
        public List<AppRoute> GetAllRoutes(int siteId_)
        {
            List<AppRoute> routes = new List<AppRoute>();
            routes.Add(new AppRoute() { Action = "Index", Controller = "Error", Name = "ErrorHandler", Url = "error/index" });
            return routes;
        }
    }
}
