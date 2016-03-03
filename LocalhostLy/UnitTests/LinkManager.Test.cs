using LocalhostLy.Model.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class LinkManager
    {
        [TestMethod]
        public void NewLink()
        {
            using (var db = new DataContext())
            {
                db.Links.Add(new LinkData()
                {
                    Author = Guid.NewGuid(),
                    CreatedAt = DateTime.Now,
                    LinkNavigations = 20,
                    OriginalLink = "3431114",
                    ShortLink = "333311133"
                });

                db.SaveChanges();
            }
        }
    }
}
