using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace WebInterface.Models
{
    /// <summary>
    /// Управляет идентификаторами пользователей
    /// </summary>
    /// <remarks>
    /// Идентификатор пользователя живет в Cookie. Считается, что куку никто не удаляет.
    /// </remarks>
    public static class Users
    {
        /// <summary>
        /// Проверить и установить куку с идентификатором пользователя
        /// </summary>
        /// <param name="a_Controller"></param>
        public static void EnsureUserId()
        {
            if (HttpContext.Current.Request.Cookies["UserId"] == null)
            {
                HttpContext.Current.Request.Cookies.Add(new HttpCookie("UserId", Guid.NewGuid().ToString())
                {
                    Expires = DateTime.Today.AddYears(1000),
                });
            }
        }

        /// <summary>
        /// Получить текущий идентификатор пользователя
        /// </summary>
        /// <param name="a_Controller"></param>
        /// <returns></returns>
        public static Guid UserId()
        {
            var userCookie = HttpContext.Current.Request.Cookies["UserId"];
            if (userCookie == null || string.IsNullOrEmpty(userCookie.Value)) return Guid.Empty;
            return Guid.Parse(userCookie.Value);
        }
    }
}