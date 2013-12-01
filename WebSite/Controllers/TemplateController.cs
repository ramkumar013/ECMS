using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebSite.App_Code;
using Newtonsoft;
using Newtonsoft.Json;
using ECMS.Core;
using ECMS.Core.Utility;
using ECMS.Services.Logging;
namespace WebSite.Controllers
{
    public class TemplateController : CMSBaseController
    {
        public ActionResult Index()
        {
            return Compose();
        }

        [ViewExecutionActionFilter]
        public ActionResult Compose()
        {
            var model = DependencyManager.ContentRepository.GetById(this.CurrentUrl);
            return View(this.GetView(), model);
        }

        //public ActionResult HandleServerError()
        //{
        //    this.ControllerContext.HttpContext.Response.StatusCode = this.GetErrorStatusCode();
        //    ViewBag.ErrorMessage = this.GetErrorMessage();
        //    return View(this.GetErrorHandlerView());
        //}
    }
}
