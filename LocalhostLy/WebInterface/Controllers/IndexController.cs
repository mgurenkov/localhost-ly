using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebInterface.Controllers
{
    public class IndexController : Controller
    {
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }
    }
}