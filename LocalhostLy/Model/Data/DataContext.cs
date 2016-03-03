using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalhostLy.Model.Data
{
    class DataContext : DbContext 
    {
        public DbSet<LinkData> Links { get; set; }
    }
}
