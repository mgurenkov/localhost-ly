using LocalhostLy.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ThePlatform.Common.Execution;
using ThePlatform.Core.Data;

namespace LocalhostLy.Model
{
    public interface ILinkService
    {
        /// <summary>
        /// Создать новую ссылку
        /// </summary>
        /// <param name="a_OriginalUrl">Адрес, который необходимо сократить</param>
        /// <param name="a_UserId">Пользователь</param>
        /// <returns></returns>
        CommandResult<LinkData> Create(string a_OriginalUrl, Guid a_UserId);

        /// <summary>
        /// Найти ссылку по которкому адресу
        /// </summary>
        /// <param name="a_ShortLink"></param>
        /// <returns></returns>
        LinkData Find(string a_ShortLink);

        /// <summary>
        /// Получить ссылки, созданные определенным пользователем
        /// </summary>
        /// <param name="a_UserId"></param>
        /// <returns></returns>
        LinkData[] Find(Guid a_UserId);

        /// <summary>
        /// Увеличить счетчик посещений
        /// </summary>
        /// <param name="a_Id"></param>
        void IncrementVisits(int a_Id);
    }

    public class LinkService : ILinkService
    {
        public CommandResult<LinkData> Create(string a_OriginalUrl, Guid a_UserId)
        {
            var r = new OperationResult();

            if (string.IsNullOrWhiteSpace(a_OriginalUrl))
            {
                r.AddError("OriginalLink", "Не указана ссылка");
                return new CommandResult<LinkData>(null, r);
            }

            if (!ValidateUrl(a_OriginalUrl))
            {
                r.AddError("OriginalLink", "Указана некорректная ссылка");
                return new CommandResult<LinkData>(null, r);
            }

            using (var db = new DataContext())
            {
                var link = new LinkData()
                {
                    Author = a_UserId,
                    CreatedAt = DateTime.Now,
                    LinkNavigations = 0,
                    OriginalLink = a_OriginalUrl,
                };

                db.Links.Add(link);
                db.SaveChanges();

                link.ShortLink = string.Format("l{0}", a_OriginalUrl.GetHashCode() ^ link.Id.GetHashCode());
                db.SaveChanges();

                return new CommandResult<LinkData>(link, r);
            }
        }

        static Regex m_UrlValidator = new Regex(@"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$", RegexOptions.Compiled);
        bool ValidateUrl(string a_Url)
        {
            return m_UrlValidator.IsMatch(a_Url);
        }

        public LinkData Find(string a_ShortLink)
        {
            using (var db = new DataContext())
            {
                return db.Links.FirstOrDefault(x => x.ShortLink == a_ShortLink);
            }
        }

        public LinkData[] Find(Guid a_UserId)
        {
            using (var db = new DataContext())
            {
                return db.Links.Where(x => x.Author == a_UserId).ToArray();
            }
        }

        public void IncrementVisits(int a_Id)
        {
            using (var db = new DataContext())
            {
                // статистика должна обновлять прямым запросом в базу данных, 
                // иначе при работе через объектную модель между загрузкой данных и запросом на обновление кто-то еще может данные поменять.
                db.Database.ExecuteSqlCommand("update Links set LinkNavigations = LinkNavigations + 1 where id = @p0", a_Id);
            }
        }
    }
}
