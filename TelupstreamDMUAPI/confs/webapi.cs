using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TelupstreamDMUAPI.confs
{
    internal class webapi
    {
        private const string __const_settingsfile = "confs/webapi.json";

        private static IConfiguration __configures;
        private static string __workpath;

        static webapi()
        {
            __workpath = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            __configures = new ConfigurationBuilder()
                .SetBasePath(__workpath)
                .AddJsonFile(__const_settingsfile, false, true)
                .Build();
        }


        public static bool Enable
        {
            get
            {
                return __configures
                        .GetSection("Enable")
                        .Get<bool>();
            }
        }
        public static class BaseSettings
        {

            public static string BindUrls
            {
                get
                {
                    return __configures
                            .GetSection("BaseSettings:BindUrls")
                            .Get<string>();
                }
            }


            public static class OpenAPI
            {
                public static bool SwaggerSwitch
                {
                    get
                    {
                        return __configures
                                .GetSection("BaseSettings:OpenAPI:SwaggerSwitch")
                                .Get<bool>();
                    }
                }

                public static bool ShowLogging
                {
                    get
                    {
                        return __configures
                                .GetSection("BaseSettings:OpenAPI:ShowLogging")
                                .Get<bool>();
                    }
                }

                public class Document
                {
                    public string Name { get; set; }
                    public string Version { get; set; }
                    public string Title { get; set; }
                    public string Description { get; set; }
                    public string TermsOfService { get; set; }
                }
                public class SecurityDefinition
                {
                    public string Key { get; set; }
                    public string Description { get; set; }
                    public string Name { get; set; }
                    public string BearerFormat { get; set; }
                    public string Scheme { get; set; }
                }



                public static Document[] Documents
                {
                    get
                    {
                        return __configures
                            .GetSection("BaseSettings:OpenAPI:Documents")
                            .Get<Document[]>();
                    }
                }

                public static string InjectionXMLCommentsFile
                {
                    get
                    {
                        return Path.Combine(__workpath, __configures
                                .GetSection("BaseSettings:OpenAPI:InjectionXmlCommentsFile")
                                .Get<string>());
                    }
                }

                public static SecurityDefinition[] SecurityDefinitions
                {
                    get
                    {
                        return __configures
                            .GetSection("BaseSettings:OpenAPI:SecurityDefinitions")
                            .Get<SecurityDefinition[]>();
                    }
                }
            }

            public static class Cors
            {
                public static string Policy
                {
                    get
                    {
                        return __configures
                                .GetSection("BaseSettings:Cors:Policy")
                                .Get<string>();
                    }
                }
                public static string[] TrustOrigins
                {
                    get
                    {
                        return __configures
                                .GetSection("BaseSettings:Cors:TrustOrigins")
                                .Get<string[]>();
                    }
                }

                public static int ExpiresDays
                {
                    get
                    {
                        return __configures
                                .GetSection("BaseSettings:Cors:ExpiresDays")
                                .Get<int>();
                    }
                }

                public static string SecretKey
                {
                    get
                    {
                        return __configures
                                .GetSection("BaseSettings:Cors:SecretKey")
                                .Get<string>();
                    }
                }

                public static string TokenIssuer
                {
                    get
                    {
                        return __configures
                                .GetSection("BaseSettings:Cors:TokenIssuer")
                                .Get<string>();
                    }
                }

                public static string TokenAudience
                {
                    get
                    {
                        return __configures
                                .GetSection("BaseSettings:Cors:TokenAudience")
                                .Get<string>();
                    }
                }
            }

            public static class MBSAccess
            {
                public static string Policy
                {
                    get
                    {
                        return __configures
                                .GetSection("BaseSettings:MBSAccess:Policy")
                                .Get<string>();
                    }
                }
                public static string[] TrustOrigins
                {
                    get
                    {
                        return __configures
                                .GetSection("BaseSettings:MBSAccess:TrustOrigins")
                                .Get<string[]>();
                    }
                }

                public static int ExpiresDays
                {
                    get
                    {
                        return __configures
                                .GetSection("BaseSettings:MBSAccess:ExpiresDays")
                                .Get<int>();
                    }
                }

                public static string SecretKey
                {
                    get
                    {
                        return __configures
                                .GetSection("BaseSettings:MBSAccess:SecretKey")
                                .Get<string>();
                    }
                }

                public static string AccessIssuer
                {
                    get
                    {
                        return __configures
                                .GetSection("BaseSettings:MBSAccess:AccessIssuer")
                                .Get<string>();
                    }
                }

                public static string AccessAudience
                {
                    get
                    {
                        return __configures
                                .GetSection("BaseSettings:MBSAccess:AccessAudience")
                                .Get<string>();
                    }
                }
            }
        }
    }
}
