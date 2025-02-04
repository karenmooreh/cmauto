using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fsLocalService
{
    public partial class ServCore
    {
        public ServCore(Dictionary<string, string> args) => __constructor_ServCore(args);
        public void Start() => __start();

        public static ServCore Singleton => __singleton;
        public ConcurrentDictionary<string, models.task> FinishedTasks => __con_finishtasks;
    }
}
