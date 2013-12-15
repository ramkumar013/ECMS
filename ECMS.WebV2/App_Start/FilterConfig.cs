using ECMS.Core;
using System.Web;
using System.Web.Mvc;

namespace ECMS.WebV2
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ECMSContextActionFilter());
        }
    }
}