using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace fsLocalService.confs
{
    internal class settings
    {
        private const bool __debugmode = false;

        public const string const_android_packages_cmcc = "com.greenpoint.android.mc10086.activity";
        public const string const_android_activity_cmcc = "com.mc10086.cmcc.base.StartPageActivity";
        public const string const_android_appiumserver_address = "127.0.0.1";
        public const int const_android_appiumserver_port = 0x1273;
        public const string const_android_appiumserver_sessionpath = "/wd/hub";

        public const string const_platform_jiema_baseuri = "http://47.123.6.54:9090";
        public const string const_platform_jiema_userid = "260";
        public const string const_platform_jiema_itemid = "57";
        public const string const_platform_jiema_userkey = "advd3su4mnizmwguzjstp12e9b8v9d7wdjllhode9zux1qqxm8k54s6exsnb7phn";
        public const string const_platform_jiema_defaultfile = "confs/testphones.txt";
        public const string const_local_rechargehistoryfile = "confs/history.csv";


        public const string CloudBaseUri = __debugmode ?
            "http://127.0.0.1:7115" :
            "http://telupstream-api.mingshenggroup.cn"
            ;

        private const string __const_settingsfile = "confs/settings.json";

        private static IConfiguration __configures;
        private static string __workpath;

        static settings()
        {
            __workpath = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            __configures = new ConfigurationBuilder()
                .SetBasePath(__workpath)
                .AddJsonFile(__const_settingsfile, false, true)
                .Build();
        }

        public static class cloud
        {
            public static int getbillingsinterval
                => __configures.GetSection("cloud:getbillingsinterval").Get<int>();
            public static int getbillingsblock
                => __configures.GetSection("cloud:getbillingsblock").Get<int>();
        }


        public static class devices
        {
            public class device
            {
                public string name { get; set; }
                public string serial { get; set; }
                public string paypwd { get; set; }
                public string cloudimageid { get; set; }
            }

            public static device device1
                => __configures.GetSection("devices:device1").Get<device>();
            public static device device2
                => __configures.GetSection("devices:device2").Get<device>();
        }

        public static class adb
        {
            public static string path
                => __configures.GetSection("adb:path").Get<string>();
        }

        public static class chinac
        {
            public static string apikeyid
                => __configures.GetSection("chinac:apikeyid").Get<string>();

            public static string apikeysecret
                => __configures.GetSection("chinac:apikeysecret").Get<string>();
        }
    }
}
