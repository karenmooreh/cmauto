using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TelupstreamAPI
{
    internal partial class ServCore
    {
        public ServCore() => __constructor_ServCore();

        public void Start() => __start();

        public static ServCore Singleton => __singleton;

        public ConcurrentQueue<Models.PresetTask> PresetTasks => __con_presettasks;
        
        public bool AddTask(string appid, string appkey, 
            string billingno, string phonenum, string amount, string param, string notifyuri, string sign) 
            => __addtask(appid, appkey, billingno, phonenum, amount, param, notifyuri, sign);




        public static string Sign(Dictionary<string, string> data, string appkey)
        {
            string __result = string.Empty;

            var __sortedpostdata = data.OrderBy(p => p.Key).ToDictionary(p => p.Key, p => p.Value);
            StringBuilder __spbuilder = new StringBuilder();
            foreach (var __param in __sortedpostdata) __spbuilder.Append(__param.Value);
            __spbuilder.Append(appkey);
            __result = MD5Crypto(__spbuilder.ToString());

            return __result;
        }

        public static string GetSearch(Dictionary<string, string> data)
        {
            string __result = string.Empty;

            foreach (var __param in data)
                if (!string.IsNullOrEmpty(__param.Key) && !string.IsNullOrEmpty(__param.Value) && __param.Key != "sign")
                    __result += $"&{__param.Key}={__param.Value}";
            if (__result.Length > 0x00) __result = __result.Substring(0x01);

            return __result;
        }

        public static string MD5Crypto(string proclaimed)
        {
            return Regex.Replace(
                BitConverter.ToString(
                    MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(proclaimed))),
                "\\-", string.Empty, RegexOptions.None).ToLower();
        }

        
    }
}
