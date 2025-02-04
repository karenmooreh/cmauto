using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinacSDKCore.NET.Models
{
    public class BaseRequest
    {
        [Argument(ArgumentAttribute.Type.Shared)]
        public string Action { get; set; }
        [Argument(ArgumentAttribute.Type.Shared)]
        public string Date { get; set; }
        [Argument(ArgumentAttribute.Type.Shared)]
        public string Version { get; set; }
        [Argument(ArgumentAttribute.Type.Shared)]
        public string AccessKeyId { get; set; }
        [Argument(ArgumentAttribute.Type.Shared)]
        public string SignatureMethod { get; set; }
        [Argument(ArgumentAttribute.Type.Shared)]
        public string Signature { get; set; }

        public BaseRequest() {
            this.Date = __getDatetimeUTCNowISO8601();
            this.Version = GlobalPresettings.APIRequestPresetValues.Version;
        }

        private static string __getDatetimeUTCNowISO8601()
        {
            string __result = string.Empty;
            var __dtNow = DateTime.Now;
            DateTime __dtUtcNow = new DateTime(__dtNow.Year, __dtNow.Month, __dtNow.Day,
                __dtNow.Hour, __dtNow.Minute, __dtNow.Second,
                DateTimeKind.Local);
            __result = __dtUtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss K");
            __result = __result.Substring(0x00, __result.LastIndexOf(":")) +
                __result.Substring(__result.LastIndexOf(":") + 0x01);

            return __result;
        }
    }
}
