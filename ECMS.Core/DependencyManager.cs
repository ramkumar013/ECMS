using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECMS.Core.Interfaces;
using NLog.Interface;
using ECMS.Core.Framework;

namespace ECMS.Core
{
    public class DependencyManager
    {
        public static IValidURLRepository URLRepository { get; set; }

        public static ILogger Logger { get; set; }

        public static ICachingService CachingService { get; set; }

        public static ContentRepositoryBase ContentRepository { get; set; }

    }
}
