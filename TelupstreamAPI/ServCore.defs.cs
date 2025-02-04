using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelupstreamAPI
{
    internal partial class ServCore
    {
        private OpenNetworkPlatform.cores.services.webapi.IntegratedWebAPIService<ServCore> __webapiserv;

        private ConcurrentQueue<Models.PresetTask> __con_presettasks;

        private static ServCore __singleton;

        private bool __status;

        private Thread __thd_taskaccepting;
        
    }
}
