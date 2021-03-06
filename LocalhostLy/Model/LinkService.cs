﻿using LocalhostLy.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ThePlatform.Common.Execution;
using ThePlatform.Core.Data;
using EntityFramework.Extensions;
using EntityFramework.Batch;

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
                    ShortLink = CreateShortLink(),
                };

                db.Links.Add(link);
                db.SaveChanges();

                return new CommandResult<LinkData>(link, r);
            }
        }

        static Regex m_UrlValidator = new Regex(@"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$", RegexOptions.Compiled);
        bool ValidateUrl(string a_Url)
        {
            return m_UrlValidator.IsMatch(a_Url);
        }

        string CreateShortLink()
        {
            // for a 48-bit base, omits l/L, 1, i/I, o/O, 0
            var map = new char[] {
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K',
                'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W',
                'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g',
                'h', 'j', 'k', 'm', 'n', 'p', 'q', 'r', 's', 't',
                'u', 'v', 'x', 'y', 'z', '2', '3', '4',
            };

            var inp = Math.Abs(Guid.NewGuid().GetHashCode());

            var b = map.Count();
            // value -> character
            var toChar = map.Select((v, i) => new { Value = v, Index = i }).ToDictionary(i => i.Index, i => i.Value);
            var res = "";
            if (inp == 0)
            {
                return "" + toChar[0];
            }
            while (inp > 0)
            {
                // encoded least-to-most significant
                var val = (int)(inp % b);
                inp = inp / b;
                res += toChar[val];
            }

            return res;
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
                db.Links.Where(x => x.Id == a_Id).Update(l => new LinkData() { LinkNavigations = l.LinkNavigations + 1 });
                db.SaveChanges();
            }
        }

        public void RemoveLinkOfUser(Guid a_UserId)
        {
            using (var db = new DataContext())
            {
                db.Links.Where(x => x.Author == a_UserId).Delete();
            }
        }
    }
}
