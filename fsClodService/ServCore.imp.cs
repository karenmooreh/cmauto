using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using OpenNetworkPlatform.cores.services.webapi;

namespace fsClodService
{
    public partial class ServCore
    {
        private void __constructor_ServCore()
        {
            __singleton = this;

            __con_billings = new System.Collections.Concurrent.ConcurrentQueue<billing>();
            __con_histories = new System.Collections.Concurrent.ConcurrentDictionary<string, billing>();
            __con_billingnotifies = new System.Collections.Concurrent.ConcurrentQueue<string>();
        }

        private void __start()
        {
            if (!__status)
            {
                __status = true;

                __serv_webapi = new IntegratedWebAPIService<ServCore>();
                __serv_webapi.Start();

                (__thd_billingnotifing = new Thread(
                    new ThreadStart(__thdmtd_billingnotifing))
                { IsBackground = true }).Start();
            }
        }

        private int __billingenqueue(string billingno, string phonenum, double amount, string notifyuri, string param) {
            int __result = 0x00;

            if (!__con_billings.Any(b => b.no == billingno) && !__con_histories.ContainsKey(billingno))
            {
                __con_billings.Enqueue(new billing()
                {
                    no = billingno,
                    phonenum = phonenum,
                    amount = amount,
                    notifyuri = notifyuri,
                    param = param,
                });
                __result = 0x01;
            }
            else __result = -0x01;

            return __result;
        }

        private List<billing> __getuntaskbillings(int? block)
        {
            List<billing> __billings = new List<billing>();

            int __block = block.HasValue && block.Value > 0x00 ? block.Value : __default_block;
            for(int i = 0x00; i < __block; i++)
            {
                billing __dequeuebilling;
                if (__con_billings.TryDequeue(out __dequeuebilling))
                {
                    __dequeuebilling.status = 0x01;
                    __dequeuebilling.timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    __billings.Add(__dequeuebilling);
                    __con_histories.TryAdd(__dequeuebilling.no, __dequeuebilling);
                }
                else continue;
            }

            return __billings;
        }

        private void __thdmtd_billingnotifing()
        {
            while (__status) {
                if (__con_billingnotifies.Count > 0x00)
                {
                    string __dequeuebillingno;
                    if(__con_billingnotifies.TryDequeue(out __dequeuebillingno))
                    {
                        if(!string.IsNullOrEmpty(__dequeuebillingno) && __con_histories.Any(b=>b.Value.no == __dequeuebillingno))
                        {
                            var __billing = __con_histories.Values.SingleOrDefault(b => b.no == __dequeuebillingno);
                            if (null != __billing && !string.IsNullOrEmpty(__billing.notifyuri)) {
                                bool __ret = false;

                                Dictionary<string, string> __params = new Dictionary<string, string>() {
                                    { "billingno", __billing.no },
                                    { "tradeno", __billing.tradeno },
                                    { "phonenum", __billing.phonenum },
                                    { "amount", __billing.amount.ToString() },
                                    { "result", "true" },
                                    { "code", "0" },
                                    { "timestamp", __billing.timestamp },
                                    { "param", __billing.param },
                                };
                                string __signstr = __sign(__params, ServCore.const_appkey);
                                __params.Add("sign", __signstr);

                                string __response = Http.request(new Http.requestparam(
                                    $"{__billing.notifyuri}?{__getsearch(__params)}"), out __ret);
                                if(__ret && !string.IsNullOrEmpty(__response))
                                {
                                    Console.WriteLine($"[{DateTime.Now.ToString()}]billing[no: {__billing.no}, status: {__billing.status}] notify callbacked.");
                                }

                            }
                        }
                    }
                } else Thread.Sleep(0x64);
            }
        }


        private static string __sign(Dictionary<string, string> data, string appkey)
        {
            string __result = string.Empty;

            var __sortedpostdata = data.OrderBy(p => p.Key).ToDictionary(p => p.Key, p => p.Value);
            StringBuilder __spbuilder = new StringBuilder();
            foreach (var __param in __sortedpostdata) __spbuilder.Append(__param.Value);
            __spbuilder.Append(appkey);
            __result = __md5crypto(__spbuilder.ToString());

            return __result;
        }

        private static string __getsearch(Dictionary<string, string> data)
        {

            string __result = string.Empty;

            foreach (var __param in data)
                if (!string.IsNullOrEmpty(__param.Key) && !string.IsNullOrEmpty(__param.Value) && __param.Key != "sign")
                    __result += $"&{__param.Key}={__param.Value}";
            if (__result.Length > 0x00) __result = __result.Substring(0x01);

            return __result;
        }

        public static string __md5crypto(string proclaimed)
        {
            return Regex.Replace(
                BitConverter.ToString(
                    MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(proclaimed))),
                "\\-", string.Empty, RegexOptions.None).ToLower();
        }
    }
}
