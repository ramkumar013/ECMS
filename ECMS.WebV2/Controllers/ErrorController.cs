using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ECMS.WebV2
{
    public class ErrorController : CMSBaseController
    {
        public ActionResult Index()
        {
            //StringBuilder builder = new StringBuilder();
            //builder.Append("\"\r\n");
            //builder.Append("time,level,name,message\r\n");
            //foreach (var item in ((IList<string>)ViewData["Logs"]))
            //{
            //    builder.Append(item.Replace("|", ","));
            //    builder.Append("\r\n");
            //}
            //builder.Append("\"");
            //ViewData["CsvLogs"] = builder.ToString();
            return View(this.GetErrorHandlerView());
        }

        public ActionResult NotFound()
        {
            return View(this.GetErrorHandlerView());
        }
    }

}
