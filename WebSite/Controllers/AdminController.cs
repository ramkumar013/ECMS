using ECMS.Core;
using ECMS.Core.Entities;
using Lib.Web.Mvc.JQuery.JqGrid;
using Newtonsoft.Json.Linq;
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
        public string Urls()
        {
            List<ValidUrl> urls = DependencyManager.URLRepository.GetAll(0, true);
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"page\":");
            sb.Append(1);
            sb.Append(",\"total\":");
            sb.Append(urls.Count);
            sb.Append(",\"records\":");
            sb.Append(urls.Count);
            sb.Append(",\"rows\":[");
            foreach (var url in urls)
            {
                sb.Append("{\"cell\":[");
                sb.Append("\"\",\"");
                sb.Append(url.FriendlyUrl);
                sb.Append("\",\"");
                sb.Append(url.View);
                sb.Append("\",\"");
                sb.Append((Convert.ToBoolean(url.Index) ? "Yes" : "No"));
                sb.Append("\",\"");
                sb.Append((Convert.ToBoolean(url.Active) ? "Active" : "InActive"));
                sb.Append("\",\"");
                sb.Append(url.LastModified);
                sb.Append("\",\"");
                sb.Append(url.StatusCode);
                sb.Append("\",\"");
                sb.Append(url.Action);              
                sb.Append("\"],\"id\":\"");
                sb.Append(url.Id);
                sb.Append("\"},");
            }
            sb.Remove(sb.ToString().LastIndexOf(","), 1);
            sb.Append("]}");

            return sb.ToString();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UrlsV2()
        {
            JqGridResponse response = new JqGridResponse()
            {
                TotalPagesCount = 1,
                PageIndex = 1,
                TotalRecordsCount = 1,
                //Footer data
                UserData = string.Empty
            };
            response.Records.AddRange(from product in DependencyManager.URLRepository.GetAll(0, true)
                                      select new JqGridRecord<ValidUrl>(Convert.ToString(product.Id), product));

            return new JqGridJsonResult() { Data = response };
        }
    }
}
