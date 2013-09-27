using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
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
            var model = new JavaScriptSerializer().Deserialize("{ \"FirstName\" : \"Vishal\", \"LastName\" : \"Sharma\", \"FlatNo\" : \"2104A\", \"Models\" : [{\"Make\":\"Maruti\",\"Model\":\"Alto\"},{\"Make\":\"Ranault\",\"Model\":\"Duster\"}]  }", typeof(System.Object));
            //return View("~/Views/" + this.CurrentUrl.SiteId + this.CurrentUrl.View + ".cshtml", new MvcHelpers.ReflectionDynamicObject() { RealObject = model });
            return View("~/Views/" + this.CurrentUrl.SiteId + this.CurrentUrl.View + ".cshtml", model);
        }
    }
}
