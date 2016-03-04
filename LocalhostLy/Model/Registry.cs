using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalhostLy.Model
{
    /// <summary>
    /// Локатор ресурсов и реестр
    /// </summary>
    public static class Registry
    {
        public static ILinkService LinkService()
        {
            return new LinkService();
        }
    }
}
