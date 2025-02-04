using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenNetworkPlatform.cores.services.webapi;
using TelupstreamDMUAPI.AdbService;

namespace TelupstreamDMUAPI
{
    public partial class ServiceCore
    {
        public const string CONST_LOGTARGET_SYSTEM0 = "SYSTEM0";

        private bool __status;

        private static ServiceCore __singleton;
        
        private IntegratedWebAPIService<ServiceCore> __serv_webapi;

        private AdbManagedService __serv_adbserv;
    }
}
