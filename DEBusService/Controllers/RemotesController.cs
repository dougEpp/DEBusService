using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DEBusService.Controllers
{
    public class RemotesController : Controller
    {
        // GET: Remotes
        public JsonResult ValidatePhone(string phone)
        {
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}