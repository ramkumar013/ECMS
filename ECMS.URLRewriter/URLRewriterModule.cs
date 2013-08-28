using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ECMS.Core;
using ECMS.Core.Entities;

namespace ECMS.HttpModules
{
    public class URLRewriter : IHttpModule
    {
        
        public void Dispose()
        {
            
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += context_BeginRequest;
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            string url = string.Empty;
            int siteId=1; // TODO : Remove HardCoding
            
            try
            {
                HttpContext context = HttpContext.Current;
                url = context.Request.Url.AbsolutePath;
                ValidUrl validUrl = DependencyManager.URLRepository.GetByFriendlyUrl(siteId, url);
                if (validUrl != null)
                {
                    context.Items.Add("validUrl", validUrl);
                    switch (validUrl.StatusCode)
                    {
                        case 200:
                            context.RewritePath("/Template/Compose");
                            break;
                        case 301:
                            throw new NotImplementedException();
                            break;
                        case 302:
                            throw new NotImplementedException();
                            break;
                        case 404:                            
                        default:
                            throw new NotImplementedException();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
