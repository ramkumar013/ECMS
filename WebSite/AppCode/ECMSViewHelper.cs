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
    }
}