using System.Web;
using System.Web.Mvc;
using WebApp.AppCode;
namespace ECMS.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}