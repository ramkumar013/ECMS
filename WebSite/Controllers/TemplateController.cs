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
namespace WebSite.Controllers
{
    public class TemplateController : CMSBaseController
    {
        public ActionResult Index()
        {
            return Compose();
        }

        public ActionResult Compose()
        {
            //var model = ECMSUtility.ParseExpression(DependencyManager.ContentRepository.GetById(this.CurrentUrl.Id), this.CurrentUrl, this.HttpContext);

            var model = JsonConvert.DeserializeObject("{ \"FirstName\" : \"Vishal\", \"LastName\" : \"Sharma\", \"FlatNo\" : \"2104A\", \"Models\" : [{\"Make\":\"Maruti\",\"Model\":\"Alto\", \"Year\":\"@DateTime.Now.Year.ToString()\"},{\"Make\":\"Ranault\",\"Model\":\"Duster\", \"Year\":\"@DateTime.Now.Year.ToString()\"}]  }");
            return View(this.GetView(), model);
        }

        public ActionResult HandleServerError()
        {
            this.ControllerContext.HttpContext.Response.StatusCode = this.GetErrorStatusCode();
            ViewBag.ErrorMessage = this.GetErrorMessage();
            return View(this.GetErrorHandlerView());
        }
    }
}
