using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
        public static void EnsureUserId(Controller a_Controller)
        {
            if (a_Controller.Request.Cookies["UserId"] == null)
            {
                a_Controller.Response.Cookies.Add(new HttpCookie("UserId", Guid.NewGuid().ToString())
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
        public static Guid UserId(Controller a_Controller)
        {
            var userCookie = a_Controller.Request.Cookies["UserId"];
            if (userCookie == null || string.IsNullOrEmpty(userCookie.Value)) return Guid.Empty;
            return Guid.Parse(userCookie.Value);
        }
    }
}