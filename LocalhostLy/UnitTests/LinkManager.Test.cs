using LocalhostLy.Model;
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
        static Guid m_TestUser = new Guid("11111111111111111111111111111111");

        [TestInitialize]
        public void Init()
        {
            var service = new LinkService();
            service.RemoveLinkOfUser(m_TestUser);
        }

        [TestMethod]
        public void NewLink()
        {
            var service = new LinkService();

            Assert.AreEqual(0, service.Find(m_TestUser).Length);

            // проверим валидацию
            // пустую строчку нельзя передавать
            var r = service.Create("", m_TestUser);
            Assert.IsTrue(r.Result.HasErrors);

            // и некорректный адрес тоже
            r = service.Create("htttp://yandex.ru", m_TestUser);
            Assert.IsTrue(r.Result.HasErrors);

            // а сейчас все должно сработать
            r = service.Create("http://yandex.ru", m_TestUser);
            Assert.IsTrue(r.Result.OK);
            Assert.IsNotNull(r.Object);

            Assert.AreEqual(1, service.Find(m_TestUser).Length);

            var linkByShort = service.Find(r.Object.ShortLink);
            Assert.IsNotNull(linkByShort);
        }

        [TestMethod]
        public void Increment()
        {
            // тестирование инкремента счетчика

            var service = new LinkService();

            Assert.AreEqual(0, service.Find(m_TestUser).Length);
            var l1 = service.Create("http://yandex1.ru", m_TestUser).Object;
            var l2 = service.Create("http://yandex2.ru", m_TestUser).Object;
            var l3 = service.Create("http://yandex3.ru", m_TestUser).Object;

            Assert.AreEqual(3, service.Find(m_TestUser).Length);

            // обновим счетчики (количество раз по номеру объекта)
            service.IncrementVisits(l2.Id);
            service.IncrementVisits(l3.Id);
            service.IncrementVisits(l1.Id);
            service.IncrementVisits(l2.Id);
            service.IncrementVisits(l3.Id);
            service.IncrementVisits(l3.Id);

            // загрузим обновленную информацию и проверим
            var links = service.Find(m_TestUser).ToDictionary(x => x.Id);
            Assert.AreEqual(1, links[l1.Id].LinkNavigations);
            Assert.AreEqual(2, links[l2.Id].LinkNavigations);
            Assert.AreEqual(3, links[l3.Id].LinkNavigations);
        }
    }
}
