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
        }
    }
}
