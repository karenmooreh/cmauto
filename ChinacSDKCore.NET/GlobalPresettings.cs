using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinacSDKCore.NET
{
    public static class GlobalPresettings
    {
        public const string Chinac_APIBaseUri_Newversion = $"https://api.chinac.com/v2/";
        public const string Chinac_APIBaseUri_Oldversion = $"https://api.chinac.com/";


        public static class APIRequestPresetKeys
        {
            public static string Action => $"Action";
            public static string Date => $"Date";
            public static string Version => $"Version";
            public static string AccessKeyId => $"AccessKeyId";
            public static string SignatureMethod => $"SignatureMethod";
            public static string Signature => $"Signature";
        }
        public static class  APIRequestPresetValues
        {
            public static string Version => $"1.0";
            public static string SignatureMethod_HMAC_SHA256 => $"HMAC-SHA256";
        }

        public static class HttpHeaderPresetValues
        {
            public static string PresetValue_ContentType 
                => $"application/json;charset=UTF-8";
            public static string PresetValue_Date 
                => DateTime.SpecifyKind(
                    DateTime.Parse(DateTime.Now.ToString(), CultureInfo.InvariantCulture), 
                        DateTimeKind.Utc).ToString("o");
        }


    }
}
