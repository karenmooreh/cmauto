using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.DeviceCommands;
using AdvancedSharpAdbClient.Models;
using OpenQA.Selenium.Appium.Android;
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
        public static ServCore __singleton;

        private string __phonefile;
        private bool __status;

        internal enum devstatus
        {
            unknow,
            init,
            idle,
            working
        }

        internal class androiddevice
        {
            public string id { get; set; }
            public string name { get; set; }
            public string cloudid { get; set; }
            public string cloudregion { get; set; }
            public string cloudimageid { get; set; }
            public string paypwd { get; set; }
            public AndroidDriver driver { get; set; }
            public int samenumcount { get; set; }
            public string workserialno { get; set; }
            public string workphonenum { get; set; }
            public devstatus status { get; set; }
            public Thread thd_working { get; set; }
            public Thread thd_cmccmonitoring { get; set; }
            public models.task currenttask { get; set; }
            public AdbClient adbclient { get; set; }
            public DeviceClient devclient { get; set; }
            public DeviceData adbdevicedata { get; set; }
            public AppStatus cmccstatus { get; set; }
            public string lastphone { get; set; }
            public bool isenvexception { get; set; }
            public bool isdevicelimited { get; set; }
            public bool istaskflowinterrupted { get; set; }
            public bool needjumpguide { get; set; }
        }

        private ConcurrentQueue<models.task> __con_localwaittingtasks;
        private ConcurrentDictionary<string, models.task> __con_finishtasks;
        private ConcurrentQueue<string> __con_phonenumsrepository;
        private AdbServer __serv_adbserver;
        private ConcurrentQueue<models.log> __con_logs;

        private ChinacSDKCore.NET.ChinacCloudMobileClient __ccmclient;

        private OpenNetworkPlatform.cores.services.webapi.IntegratedWebAPIService<ServCore> __serv_webapi;

        private androiddevice __device1;
        private androiddevice __device2;

        private Thread __thd_cloudbillingsgetting;
        private Thread __thd_worksallocating;
        private Thread __thd_logging;
    }
}
