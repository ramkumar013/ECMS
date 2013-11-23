using ECMS.Core;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
namespace WebApp.AppCode
{
    public class ECMSViewHelper
    {
        public static string Eval(string expression)
        {
            var task = DependencyManager.CachingService.Get<Task>("Task." + expression.GetHashCode().ToString());
            if (task.IsCompleted == false)
            {
                task.Wait();
            }
            TemplateService service = new TemplateService();
            return service.Run(DependencyManager.CachingService.Get<ITemplate>(expression.GetHashCode().ToString()), null);
        }

        public static string GetHref(string url_,string text, string attributes_)
        {
            // TODO : Check if url is active or not.
            return string.Format("<a href={0}>{1}</a>", url_, text);
        }
    }
}