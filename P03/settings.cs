using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace P03
{
    internal class settings
    {
        private const string __const_settingsfile = "settings.json";

        private static IConfiguration __configures;
        private static string __workpath;


        static settings()
        {
            __workpath = System.IO.Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            __configures = new ConfigurationBuilder()
                .SetBasePath(__workpath)
                .AddJsonFile(__const_settingsfile, false, true)
                .Build();
        }

        public static class configs
        {
            public static int singleinterval
                => __configures.GetSection("configs:singleinterval").Get<int>();
        }

        public static class proxyserv
        {
            public static string secretid
                => __configures.GetSection("proxyserv:secretid").Get<string>();

            public static string secretkey
                => __configures.GetSection("proxyserv:secretkey").Get<string>();

            public static string authuser
                => __configures.GetSection("proxyserv:authuser").Get<string>();
            
            public static string password
                => __configures.GetSection("proxyserv:password").Get<string>();
        }
    }
}
