using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalhostLy.Model.Data
{
    [Table("Links")]
    public class LinkData
    {
        public int Id { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Кто создал
        /// </summary>
        public Guid Author { get; set; }

        /// <summary>
        /// Коротка ссылка
        /// </summary>
        public string ShortLink { get; set; }

        /// <summary>
        /// Исходная ссылка
        /// </summary>
        public string OriginalLink { get; set; }

        /// <summary>
        /// Количество переходов
        /// </summary>
        public int LinkNavigations { get; set; }
    }
}
