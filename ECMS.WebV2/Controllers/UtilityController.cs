using ECMS.WebV2.AppCode;
using System;
using System.Collections.Generic;
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
            EncryptDecrypt.EncryptConnString();
            return View();
        }

    }
}
