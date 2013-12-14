using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECMS.Core.Framework
{
    public enum ContentViewType : short
    {
        NONE = 0,
        PREVIEW = 10,
        PUBLISH = 20
    }

    public enum DeviceType : short
    {
        NONE = 0,
        WEB = 10,
        TABLET = 20,
        MOBILE = 30
    }
}
