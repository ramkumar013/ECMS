using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft;
using Newtonsoft.Json;
using ECMS.Core;
using ECMS.Core.Utilities;
using ECMS.Services.Logging;
using ECMS.Core.Framework;
using ECMS.WebV2;
namespace WebSite.Controllers
{
    public class TemplateController : CMSBaseController
    {
        public ActionResult Index()
        {
            return Compose();
        }

        public ActionResult Guid()
        {
            return View("~/Views/Guid.cshtml");
        }

        [ViewExecutionActionFilter]
        public ActionResult Compose()
        {
            var model = DependencyManager.ContentRepository.GetById(this.CurrentUrl, this.ViewType);            
            ViewResult result= View(this.GetView(),null, model);
            
            return result;
        }

        public ActionResult HandleServerError()
        {
            this.ControllerContext.HttpContext.Response.StatusCode = this.GetErrorStatusCode();
            ViewBag.ErrorMessage = this.GetErrorMessage();
            return View(this.GetErrorHandlerView());
        }
    }
}
