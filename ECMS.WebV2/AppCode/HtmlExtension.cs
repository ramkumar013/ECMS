﻿using ECMS.Core;
using ECMS.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace ECMS.WebV2.AppCode
{
 
    public static class MvcExtensions
    {
        public static MvcHtmlString SSI(this HtmlHelper htmlHelper, string partialViewName)
        {
            //var viewName = string.Format("~/Views/{0}/{1}/{2}.cshtml", ECMSSettings.Current.SiteId, (int)Utility.CurrentViewType(new HttpContextWrapper(HttpContext.Current)), partialViewName);
            //return htmlHelper.Partial(viewName);
            throw new Exception("This method is deprecated. Use Html.Include instead.");
        }

        public static MvcHtmlString Include(this HtmlHelper htmlHelper, string partialViewName)
        {
            var viewName = string.Format("~/Views/{0}/{1}/{2}.cshtml", ECMSSettings.Current.SiteId, (int)Utility.CurrentViewType(new HttpContextWrapper(HttpContext.Current)), partialViewName);
            return htmlHelper.Partial(viewName);
        }

        public static MvcHtmlString DebuggerView(this HtmlHelper htmlhelper)
        {
            return htmlhelper.Partial("~/Views/Shared/DebugMessage.cshtml");
        }
    }
}