using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace P03
{
    public class Jiema
    {
        private const string __apibase = $"http://47.123.6.54:9090/";
        private const string __regex_verifycode = $"(?<=请输入验证码：).*?(?=确认订购)";

        public Jiema(string phonenum, string userid, string itemid, string userkey) {
            __phonenum = phonenum;
            __userid = userid;
            __itemid = itemid;
            __userkey = userkey;

        }

        private string __userid;
        private string __itemid;
        private string __userkey;
        private string __phonenum;

        private class result_receiveCode
        {
            public string status { get; set; }
            public string code { get; set; }
            public string desc { get; set; }
            public double amount { get; set; }
            public string uid { get; set; }
            public int state { get; set; }
            public int maxTime { get; set; }
            public string areaCode { get; set; }
            public string bizOrderId { get; set; }
            public int carrierType { get; set; }
            public double itemFacePrice { get; set; }
            public string itemId { get; set; }
            public string itemName { get; set; }
            public string serialno { get; set; }
        }

        private class result_queryBizOrder
        {
            public class cls_data
            {
                public string id { get; set; }
                public string serialno { get; set; }
                public double amount { get; set; }
                public int amt { get; set; }
                public string status { get; set; }
                public string statusDesc { get; set; }
                public string itemId { get; set; }
                public string outOrderNo { get; set; }
                public string gmtCreate { get; set; }
                public string gmtModify { get; set; }
                public string? faildCode { get; set; }
                public int? timeCost { get; set; }
                public int payState { get; set; }
                public string? memo { get; set; }
                public string? ext { get; set; }
            }

            public string status { get; set; }
            public string desc { get; set; }
            public string code { get; set; }
            public cls_data? data { get; set; }
        }

        public bool Take(out string serialno, int timeout_receive = 30000)
        {
            bool __result = false;

            serialno = string.Empty;


            bool __ret = false;
            string __param_dtCreate = DateTime.Now.ToString("yyyyMMddHHmmss");
            serialno = $"{__phonenum}_{DateTime.Now.ToString("yyyyMMddHHmmss")}";

            Dictionary<string, string> __postdata = new Dictionary<string, string>() {
                { "userId", __userid },
                { "itemId", __itemid },
                { "uid", __phonenum },
                { "serialno", serialno },
                { "dtCreate", __param_dtCreate },
            };

            __postdata.Add("sign", __sign(__postdata));

            string __response = Http.request(
                new Http.requestparam($"{__apibase}cardCharge/receiveCode?{__getsearch(__postdata)}")
                {
                    method = HttpMethod.Get,
                    accept = $"{Http.requestparam.contenttype_application_json};charset=UTF-8",
                },
                out __ret);
            if (__ret && !string.IsNullOrEmpty(__response))
            {
                result_receiveCode __respobj = null;
                try { __respobj = JsonSerializer.Deserialize<result_receiveCode>(__response); } catch { }
                if (null != __respobj && __respobj.status == "success" && __respobj.code == "00")
                {
                    __result = true;
                }
            }

            return __result;
        }

        public string QueryCode(string serialno,int timeout_query = 30000)
        {
            string __result = string.Empty;

            Dictionary<string, string> __postdata = new Dictionary<string, string>() {
                    { "userId", __userid },
                    { "serialno", serialno },
                };
            __postdata.Add("sign", __sign(__postdata));

            bool __ret = false;
            string __response = Http.request(new Http.requestparam($"{__apibase}cardCharge/queryBizOrder?{__getsearch(__postdata)}")
            {
                accept = $"{Http.requestparam.contenttype_application_json};charset=UTF-8",
                method = HttpMethod.Get
            }, out __ret);

            string __message = string.Empty;
            if (__ret && !string.IsNullOrEmpty(__response))
            {
                result_queryBizOrder __queryresult = null;
                try { __queryresult = JsonSerializer.Deserialize<result_queryBizOrder>(__response); } catch { }
                if (null != __queryresult && __queryresult.status == "success"
                    && null != __queryresult.data && !string.IsNullOrEmpty(__queryresult.data.memo))
                {
                    __message = __queryresult.data.memo;
                }
            }


            if (!string.IsNullOrEmpty(__message) && Regex.IsMatch(__message, __regex_verifycode, RegexOptions.None))
                __result = Regex.Match(__message, __regex_verifycode, RegexOptions.None).Value;

            return __result;
        }

        private string __sign(Dictionary<string, string> data)
        {
            string __result = string.Empty;

            var __sortedpostdata = data.OrderBy(p => p.Key).ToDictionary(p => p.Key, p => p.Value);
            StringBuilder __spbuilder = new StringBuilder();
            foreach (var __param in __sortedpostdata) __spbuilder.Append(__param.Value);
            __spbuilder.Append(__userkey);
            __result = md5(__spbuilder.ToString());

            return __result;
        }

        private string __getsearch(Dictionary<string, string> data)
        {
            string __result = string.Empty;

            foreach (var __param in data)
                __result += $"&{__param.Key}={__param.Value}";
            if (__result.Length > 0x00) __result = __result.Substring(0x01);

            return __result;
        }
        public static string md5(string proclaimed)
        {
            return Regex.Replace(
                BitConverter.ToString(
                    MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(proclaimed))),
                "\\-", string.Empty, RegexOptions.None).ToLower();
        }
    }
}
