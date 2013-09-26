using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.App_Code;

namespace WebSite.Controllers
{
    public class TemplateController : CMSBaseController
    {
        //
        // GET: /Content/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Compose()
        {
            dynamic model = new { FirstName = "Vishal", LastName = "Sharma", FlatNo = "2104A", FirstName1 = "Vishal", LastName1 = "Sharma", FlatNo1 = "2104A", FirstName2 = "Vishal", LastName2 = "Sharma", FlatNo2 = "2104A", FirstName3 = "Vishal", LastName3 = "Sharma", FlatNo3 = "2104A", FirstName4 = "Vishal", LastName4 = "Sharma", FlatNo4 = "2104A", FirstName5 = "Vishal", LastName5 = "Sharma", FlatNo5 = "2104A", FirstName6 = "Vishal", LastName6 = "Sharma", FlatNo6 = "2104A" };
            return View("~/Views/" + this.CurrentUrl.SiteId + this.CurrentUrl.View + ".cshtml", new MvcHelpers.ReflectionDynamicObject() { RealObject = model });
        }
    }
}
