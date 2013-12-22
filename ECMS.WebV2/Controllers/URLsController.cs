using ECMS.Core;
using ECMS.Core.Entities;
//using Lib.Web.Mvc.JQuery.JqGrid;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ECMS.WebV2
{
    [Authorize]
    public class UrlsController : CMSBaseController
    {

        public ActionResult Index()
        {
            ViewBag.TotalRows = DependencyManager.URLRepository.GetTotalUrlCount(ECMSSettings.Current.SiteId);
            return View("~/Views/Admin/Urls-Grid.cshtml");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public string GetAll()
        {
            
            int pageNo = 1;
            int.TryParse(Request.Form["page"], out pageNo);

            int noOfRecords = 5;
            int.TryParse(Request.Form["rows"], out noOfRecords);

            string searchOp = Request.Form["searchOper"];
            string searchField = Request.Form["searchField"];
            string searchString = Request.Form["searchString"];
            string sortField = Request.Form["sidx"];
            string sortdirection = Request.Form["asc"];
            bool isSearchRq = Convert.ToBoolean(Request.Form["_search"]);

            Tuple<long, List<ValidUrl>> result = DependencyManager.URLRepository.FindAndGetAll(ECMSSettings.Current.SiteId, searchField,searchString, searchOp, sortField, sortdirection, pageNo, noOfRecords, isSearchRq);
            var urls = result.Item2;
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"page\":");
            sb.Append(pageNo);
            sb.Append(",\"total\":");
            sb.Append((result.Item1 / noOfRecords) + 1);
            sb.Append(",\"records\":");
            sb.Append(result.Item1);
            sb.Append(",\"rows\":[");
            foreach (var url in urls)
            {
                sb.Append("{\"cell\":[");
                sb.Append("\"\",\"");
                sb.Append(url.FriendlyUrl);
                sb.Append("\",\"");
                sb.Append(url.View);
                sb.Append("\",\"");
                sb.Append(url.Index);
                sb.Append("\",\"");
                sb.Append(url.Active);
                sb.Append("\",\"");
                sb.Append(url.StatusCode); 
                sb.Append("\",\"");
                sb.Append(url.ChangeFrequency);
                sb.Append("\",\"");
                sb.Append(url.SitemapPriority);
                sb.Append("\",\"");
                sb.Append(url.LastModified);
                sb.Append("\",\"");
                sb.Append(url.LastModifiedBy);
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
        public ActionResult UpdateUrl(ValidUrl url_)
        {
            string result = string.Empty;
            try
            {
                url_.LastModified = DateTime.Now;
                url_.LastModifiedBy = this.CMSUser.UserName;
                url_.SitemapPriority = float.Parse(url_.SitemapPriority.ToString("N1"));
                url_.Action = ECMSSettings.Current.DefaultURLRewriteAction;
                if (url_.Id == Guid.Empty)
                {
                    url_.Id = Guid.NewGuid();
                    DependencyManager.URLRepository.Save(url_);
                }
                else
                {
                    DependencyManager.URLRepository.Update(url_);
                }

                result = "Url Updated Successfully.";
            }
            catch (Exception ex)
            {
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.StatusCode = 500;
                Response.StatusDescription = "Failed : " + ex.Message;
            }
            return Json(result);
        }

        //[AcceptVerbs(HttpVerbs.Post)]
        //public JsonResult UrlsV2()
        //{
        //    JqGridResponse response = new JqGridResponse()
        //    {
        //        TotalPagesCount = 1,
        //        PageIndex = 1,
        //        TotalRecordsCount = 1,
        //        //Footer data
        //        UserData = string.Empty
        //    };
        //    response.Records.AddRange(from product in DependencyManager.URLRepository.GetAll(0, true)
        //                              select new JqGridRecord<ValidUrl>(Convert.ToString(product.Id), product));

        //    return new JqGridJsonResult() { Data = response };
        //}
    }
}
