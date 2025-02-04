using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelupstreamDMUAPI.Logger
{
    public partial class Logger
    {
        private static Thread __thd_logging;

        private static bool __status;

        private static ConcurrentQueue<log> __con_logsqueue;
    }
}
