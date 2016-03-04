using LocalhostLy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebInterface.Models;

namespace WebInterface.Controllers
{
    public class IndexController : Controller
    {
        [Route("")]
        public ActionResult Index()
        {
            Users.EnsureUserId();
            return View();
        }

        [Route("{link}")]
        public ActionResult RedirectToOriginal(string Link)
        {
            var service = Registry.LinkService();
            var link = service.Find(Link);

            if (link == null) return HttpNotFound();

            service.IncrementVisits(link.Id);
            Response.Redirect(link.OriginalLink, true);
            return Redirect(link.OriginalLink);
        }
    }
}