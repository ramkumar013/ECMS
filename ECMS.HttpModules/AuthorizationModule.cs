using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ECMS.HttpModules
{
    public class AuthorizationModule : IHttpModule
    {
        public void Dispose()
        {
            
        }

        public void Init(HttpApplication context)
        {
            context.AuthorizeRequest += context_AuthorizeRequest;
        }

        void context_AuthorizeRequest(object sender, EventArgs e)
        {
            //TODO: Add code here to restrict access based on IP.
            throw new NotImplementedException();
        }
    }
}
