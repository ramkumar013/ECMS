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
            return View("~/Views/"+this.CurrentUrl.SiteId  + this.CurrentUrl.View);
        }
    }
}
