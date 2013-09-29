﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ECMS.Core;
using ECMS.Core.Entities;
using NLog;

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
            HttpContext context = HttpContext.Current;
            try
            {
                if (!Utility.IsValidUrlForRewrite(new HttpContextWrapper(HttpContext.Current)))
                {
                    return;
                }
                url = context.Request.Url.AbsolutePath;
                ValidUrl validUrl = DependencyManager.URLRepository.GetByFriendlyUrl(siteId, url);
                if (validUrl != null)
                {
                    validUrl.SiteId = siteId;
                    context.Items.Add("validUrl", validUrl);
                    switch (validUrl.StatusCode)
                    {
                        case 200:
                        default:
                            context.RewritePath(validUrl.Action);
                            break;
                        case 301:
                            context.Response.RedirectLocation = validUrl.Action;
                            context.Response.RedirectPermanent(validUrl.Action, true);
                            break;
                        case 302:
                             context.Response.RedirectLocation = validUrl.Action;
                             context.Response.Redirect(validUrl.Action, true);
                             break;
                        case 404:
                             HandleError(context, validUrl.SiteId, 404);
                             break;
                    }
                }
            }
            catch (Exception ex)
            {
                DependencyManager.Logger.Log(LogLevel.Error, ex.ToString());
                HandleError(context, siteId, 500);
            }
        }

        private static void HandleError(HttpContext context_, int siteId_, int statusCode_)
        {
            context_.Items.Add("ResponseStatusCode", statusCode_);
            context_.RewritePath("\\Template\\HandleServerError");
            context_.Response.TrySkipIisCustomErrors = true;
        }
    }
}
