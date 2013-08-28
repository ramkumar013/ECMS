using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECMS.Core.Interfaces;

namespace ECMS.Core
{
    public class DependencyManager
    {
        public static IRouteManager RouteManager { get; set; }
    }
}
