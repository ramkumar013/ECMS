using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECMS.Core
{
    public class AppRoute
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}
