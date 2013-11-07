using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace WebApp.AppCode
{
    public class ECMSViewHelper
    {
        public static string Eval(string expression)
        {
            return Razor.Parse(expression, expression);
        }

        public static string GetHref(string url_,string text, string attributes_)
        {
            // TODO : Check if url is active or not.
            return string.Format("<a href={0}>{1}</a>", url_, text);
        }
    }
}