using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetIM.IMServer.Controllers
{
    public class IMController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}