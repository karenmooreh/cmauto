using OpenQA.Selenium.DevTools.V129.Debugger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace P03
{
    internal class kdlapiproxy
    {
        private const string apiuri_dps = "https://dps.kdlapi.com/api/getdps";
        private const string apiuri_auth = "https://auth.kdlapi.com/api/get_secret_token";

        private const string __areacode_jiangsu = "32";
        private static readonly string[] __areacodes_jiangsucities = new string[] {
            "320102","320104","320105","320106","320111","320113","320114","320115",
            "320116","320117","320118","320205","320206","320211","320213","320214",
            "320302","320303","320305","320311","320312","320321","320322","320324",
            "320381","320382","320402","320404","320411","320412","320413","320481",
            "320505","320506","320507","320508","320509","320581","320582","320583",
            "320585","320612","320613","320614","320623","320681","320682","320685",
            "320703","320706","320707","320722","320723","320724","320803","320804",
            "320812","320813","320826","320830","320831","320902","320903","320904",
            "320921","320922","320923","320924","320925","320981","321002","321003",
            "321012","321023","321081","321084","321102","321111","321112","321181",
            "321182","321183","321202","321203","321204","321281","321282","321283",
            "321302","321311","321322","321323","321324"
        };

        internal class result_get_secret_token
        {
            public class cls_data
            {
                public string secret_token { get; set; }
                public int expire { get; set; }
            }

            public int code { get; set; }
            public string msg { get; set; }
            public cls_data data { get; set; }
        }
        internal class result_getdps
        {
            public class cls_data
            {
                public int count { get; set; }
                public int dedup_count { get; set; }
                public int order_left_count { get; set; }
                public List<string> proxy_list { get; set; }
                public cls_data()
                {
                    proxy_list = new List<string>();
                }
            }
            public int code { get; set; }
            public string msg { get; set; }
            public cls_data data { get; set; }
            public result_getdps()
            {
                this.data = new cls_data();
            }
        }

        public kdlapiproxy()
        {
            bool __ret = false;
            string __response = Http.request(new Http.requestparam(apiuri_auth) {
                method = HttpMethod.Post,
                data = $"secret_id={settings.proxyserv.secretid}&secret_key={settings.proxyserv.secretkey}"
            }, out __ret);
            if(__ret && !string.IsNullOrEmpty(__response)) {
                result_get_secret_token __result = null;
                try { __result = JsonSerializer.Deserialize<result_get_secret_token>(__response); } catch { }
                if(null != __result && __result.code == 0x00)
                {
                    __token = __result.data.secret_token;
                }
            }
        }

        public string token => __token;
        private string __token = string.Empty;

        public IPEndPoint getproxy()
        {
            IPEndPoint __result = null;
            bool __ret = false;
            Random __rnd = new Random(DateTime.Now.Millisecond);

            while (null == __result)
            {
                string __search = $"secret_id={settings.proxyserv.secretid}&num=1&format=json&area={__areacode_jiangsu}&signature={__token}";
                string __response = Http.request(new Http.requestparam($"{apiuri_dps}?{__search}")
                {
                    method = HttpMethod.Get,
                }, out __ret);

                if (__ret && !string.IsNullOrEmpty(__response))
                {
                    result_getdps __resp = null;
                    try { __resp = JsonSerializer.Deserialize<result_getdps>(__response); } catch { }
                    if (null != __resp && null != __resp.data && __resp.data.count > 0x00)
                    {
                        string __proxy = __resp.data.proxy_list[0x00];
                        string[] __pdata = __proxy.Split(':');
                        if (__pdata.Length >= 0x02)
                        {
                            try { __result = new IPEndPoint(IPAddress.Parse(__pdata[0x00]), int.Parse(__pdata[0x01])); } catch { }
                            if (null != __result) break;
                        }
                    }
                }
                Thread.Sleep(__rnd.Next(0x64, 0x03e8));
            }

            return __result;
        }
    }
}
