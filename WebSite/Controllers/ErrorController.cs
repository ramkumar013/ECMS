using ECMS.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class ErrorController : CMSBaseController
    {
        public ActionResult Index()
        {
            return View(this.GetErrorHandlerView());
        }

        public ActionResult NotFound()
        {
            return View(this.GetErrorHandlerView());
        }
    }

}
