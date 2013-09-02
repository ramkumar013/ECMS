using ECMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.App_Code
{
    public class CMSBaseController : Controller
    {
        public ContentBase PageContent { get; set; }
        public ValidUrl CurrentUrl {
            get {
                return (ControllerContext.HttpContext.Items["validUrl"] != null ? ControllerContext.HttpContext.Items["validUrl"] as ValidUrl : null);
            }
        }
    }
}