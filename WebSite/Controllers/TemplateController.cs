using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Controllers
{
    public class TemplateController : Controller
    {
        //
        // GET: /Content/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Compose()
        {
            return View();
        }

    }
}
