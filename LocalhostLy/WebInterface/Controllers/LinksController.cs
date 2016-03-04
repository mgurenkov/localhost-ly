using LocalhostLy.Model;
using LocalhostLy.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ThePlatform.Core.Data;
using WebInterface.Models;

namespace WebInterface.Controllers
{
    [RoutePrefix("api/links")]
    public class LinksController : ApiController
    {
        [Route("")]
        [HttpPost]
        public CommandResult<LinkData> CreateLink(string OriginalLink)
        {
            var service = Registry.LinkService();
            return service.Create(OriginalLink, Users.UserId());
        }

        [Route("")]
        [HttpGet]
        public LinkData[] List()
        {
            var service = Registry.LinkService();
            return service.Find(Users.UserId());
        }
    }
}
