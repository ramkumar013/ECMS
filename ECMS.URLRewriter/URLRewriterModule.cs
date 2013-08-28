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
            throw new NotImplementedException();
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += context_BeginRequest;
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            string url = string.Empty;
            int siteId=-1;
            try
            {
                url = HttpContext.Current.Request.Url.AbsolutePath;
                ValidUrl validUrl = DependencyManager.URLRepository.GetByFriendlyUrl(siteId, url);
                if (validUrl != null)
                {
                    HttpContext.Current.Items.Add("validUrl",validUrl);
                    switch (validUrl.StatusCode)
                    {
                        case 200:
                            //TODO:
                            break;
                        case 301:
                            //TODO:
                            break;
                        case 302:
                            //TODO:
                            break;
                        case 404:                            
                        default:
                            //TODO:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            throw new NotImplementedException();
        }
    }
}
