using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TelupstreamAPI.EFCore.Contexts
{
    public class TelupstreamStorages : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL($"server={confs.settings.efcore.dbaddr};database={confs.settings.efcore.dbname};user={confs.settings.efcore.dbuser};password={confs.settings.efcore.dbpasswd}");
        }

        public DbSet<Models.task> tasks { get; set; }
    }
}
