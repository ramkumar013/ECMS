using ECMS.Services;
using ECMS.WebV2.AppCode;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECMS.WebV2.Controllers
{
    public class UtilityController : Controller
    {
        //
        // GET: /Utility/

        public ActionResult EncryptConnectionString()
        {
            var encryptedval = SecurityHelper.Encrypt(ConfigurationManager.ConnectionStrings["mongodb"].ConnectionString, true);

            var decyptedVal = SecurityHelper.Decrypt(encryptedval, true);
            return View();
        }
    }
}
