using LocalhostLy.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            throw new NotImplementedException();
        }

        public LinkData Find(string a_ShortLink)
        {
            throw new NotImplementedException();
        }

        public LinkData[] Find(Guid a_UserId)
        {
            throw new NotImplementedException();
        }

        public void IncrementVisits(int a_Id)
        {
            throw new NotImplementedException();
        }
    }
}
