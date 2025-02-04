using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TelupstreamAPI.confs
{
    internal class settings
    {
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

        public class cls_auth
        {
            public string appid { get; set; }
            public string appkey { get; set; }
        }

        public static cls_auth[] auths
            => __configures.GetSection("auths").Get<cls_auth[]>();

        public static class efcore
        {
            public static string dbaddr
                => __configures.GetSection("efcore:dbaddr").Get<string>();

            public static string dbname
                => __configures.GetSection("efcore:dbname").Get<string>();

            public static string dbuser
                => __configures.GetSection("efcore:dbuser").Get<string>();

            public static string dbpasswd
                => __configures.GetSection("efcore:dbpasswd").Get<string>();

        }
    }
}
