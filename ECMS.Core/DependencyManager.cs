using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECMS.Core.Interfaces;
using NLog.Interface;
using ECMS.Core.Framework;
using ECMS.Core.Extensions;

namespace ECMS.Core
{
    public class DependencyManager
    {
        public static IValidURLRepository URLRepository { get; set; }

        public static ECMSLogger Logger { get; set; }

        public static ICachingService CachingService { get; set; }

        public static ContentRepositoryBase ContentRepository { get; set; }

        public static IViewRepository ViewRepository { get; set; }

    }
}
