using ECMS.Core;
using ECMS.Core.Entities;
using Lib.Web.Mvc.JQuery.JqGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View("~/Views/Admin/UrlGrid.cshtml");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Urls()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"page\":\"");
            sb.Append("1");
            sb.Append("\",\"total\":");
            sb.Append("1");
            sb.Append(",\"records\":\"");
            sb.Append(2);
            sb.Append("\",\"rows\":[");
            foreach (var url in DependencyManager.URLRepository.GetAll(0,true))
            {
                sb.Append("{\"id\":\"");
                sb.Append(url.Id);
                sb.Append("\",");
                sb.Append("\"cell\":[");
                if (true)
                {
                    sb.Append("\"\",");
                }
                sb.Append("\"");
                sb.Append(url.Id);
                sb.Append("\",\"<a class='Breadcrumb' href='http://");
                sb.Append("www.explotravel.com");
                sb.Append(url.FriendlyUrl);
                sb.Append("?preview=1&gcms-srv' target='_blank'>Preview</a>\",\"<a class='Breadcrumb' href='http://");
                sb.Append("www.explotravel.com");
                sb.Append(url.FriendlyUrl);
                sb.Append("' target='_blank'>Publish</a>\",\"");
                sb.Append(url.FriendlyUrl);
                sb.Append("\",\"");
                sb.Append(url.Action);
                sb.Append("\",\"");
                sb.Append((Convert.ToBoolean(url.Active) ? "Active" : "InActive"));
                sb.Append("\",\"");
                sb.Append(url.StatusCode);
                sb.Append("\",\"");
                sb.Append((Convert.ToBoolean(url.Index) ? "Yes" : "No"));
                sb.Append("\"]},");
            }
            sb.Remove(sb.ToString().LastIndexOf(","), 1);
            sb.Append("]}");

            JqGridResponse response = new JqGridResponse()
            {
                TotalPagesCount = 1,
                PageIndex = 1,
                TotalRecordsCount = 1,
                //Footer data
                UserData = string.Empty
            };
            response.Records.AddRange(from product in DependencyManager.URLRepository.GetAll(0,true)
                                      select new JqGridRecord<ValidUrl>(Convert.ToString(product.Id), product));

            return new JqGridJsonResult() { Data = response };
            //return Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }
    }
}
