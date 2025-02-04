using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TelupstreamDMUAPI.confs
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

        public static class common
        {
            public static class datalist
            {
                public static int sizeperpage
                    => __configures.GetSection("common:datalist:sizeperpage").Get<int>();
            }
        }

        public static class adk
        {
            public static string adbpath
                => __configures.GetSection("adk:adbpath").Get<string>();
        }

        public static class storage
        {
            public static string dbpath
                => __configures.GetSection("storage:dbpath").Get<string>();
        }

        public static class authorize
        {
            public static string signuser
                => __configures.GetSection("authorize:signuser").Get<string>();

            public static string signpassword
                => __configures.GetSection("authorize:signpassword").Get<string>();
        }
    }
}
