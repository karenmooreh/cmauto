using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelupstreamDMUAPI.EFCore.Contexts
{
    public class MainContext : DbContext
    {
        #region overrides
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={confs.settings.storage.dbpath};");
        }
        #endregion

        public DbSet<Models.task> tasks { get; set; }
        public DbSet<Models.device> devices { get; set; }
        public DbSet<Models.log> logs { get; set; }
    }
}
