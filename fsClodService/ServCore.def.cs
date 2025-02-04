using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenNetworkPlatform.cores.services.webapi;

namespace fsClodService
{
    public partial class ServCore
    {
        public const string const_appid = "15c6f4a66cca4d96952b9e58527edb26";
        public const string const_appkey = "a31a2f0aac07d62a947967c57d029782";

        private const int __default_block = 0x01;

        public class billing
        {
            public string no { get; set; }
            public string tradeno { get; set; }
            public string phonenum { get; set; }
            public double amount { get; set; }
            public string notifyuri { get; set; }
            public string param { get; set; }
            public int status { get; set; }
            public string timestamp { get; set; }
        }

        private static ServCore __singleton;

        private bool __status;

        private IntegratedWebAPIService<ServCore> __serv_webapi;

        private ConcurrentQueue<billing> __con_billings;
        private ConcurrentDictionary<string, billing> __con_histories;
        private ConcurrentQueue<string> __con_billingnotifies;

        private Thread __thd_billingnotifing;
    }
}
