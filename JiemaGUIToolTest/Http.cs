using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JiemaGUIToolTest
{
    internal class Http
    {
        public class requestparam
        {
            public const string contenttype_application_json = "application/json";
            public const string contenttype_application_x_www_form_urlencoded = "application/x-www-form-urlencoded";
            public const string contenttype_text_html = "text/html";
            public int timeout { get; set; }
            public string uri { get; set; }
            public HttpMethod method { get; set; }
            public string data { get; set; }
            public string authtoken { get; set; }
            public string contenttype { get; set; }
            public string accept { get; set; }
            public requestparam(string uri,
                HttpMethod method = null, string data = null,
                string authtoken = null, string contenttype = null, string accept = null)
            {
                this.uri = uri;
                this.method = null != method ? method : HttpMethod.Get;
                this.data = data;
                this.contenttype = contenttype;
                this.authtoken = authtoken;
                this.accept = accept;
            }
        }

        public static string request(requestparam param, out bool resultsign)
        {
            string __result = null;
            resultsign = false;

            if (null != param)
            {
                HttpWebRequest __request = WebRequest.Create(param.uri) as HttpWebRequest;
                __request.Method = param.method.Method;
                __request.Accept = "text/plain";
                __request.KeepAlive = true;
                if (param.timeout > 0x00) __request.Timeout = param.timeout;
                if (!string.IsNullOrEmpty(param.contenttype))
                    __request.ContentType = param.contenttype;
                if (!string.IsNullOrEmpty(param.accept))
                    __request.Accept = param.accept;
                if (!string.IsNullOrEmpty(param.authtoken))
                    __request.Headers.Add("Authorization", $"Bearer {param.authtoken}");
                if (!string.IsNullOrEmpty(param.data))
                    using (var __reqstm = __request.GetRequestStream())
                    {
                        byte[] __tempbuff = Encoding.UTF8.GetBytes(param.data);
                        __reqstm.Write(__tempbuff, 0x00, __tempbuff.Length);
                    }
                try
                {
                    using (StreamReader __stmreader = new StreamReader(
                        (__request.GetResponse() as HttpWebResponse).GetResponseStream()))
                    {
                        __result = __stmreader.ReadToEnd();
                        resultsign = true;
                    }
                }
                catch { __result = string.Empty; }
            }

            return __result;
        }

        public static byte[] download(requestparam param, out bool resultsign)
            => __download(param, out resultsign);
        private static byte[] __download(requestparam param, out bool resultsign)
        {
            byte[] __result = null;
            resultsign = false;

            if (null != param)
            {
                HttpWebRequest __request = WebRequest.Create(param.uri) as HttpWebRequest;
                __request.Method = param.method.Method;
                if (param.timeout > 0x00) __request.Timeout = param.timeout;
                if (!string.IsNullOrEmpty(param.contenttype))
                    __request.ContentType = param.contenttype;
                if (!string.IsNullOrEmpty(param.authtoken))
                    __request.Headers.Add("Authorization", $"Bearer {param.authtoken}");
                if (!string.IsNullOrEmpty(param.data))
                    using (var __reqstm = __request.GetRequestStream())
                    {
                        byte[] __tempbuff = Encoding.UTF8.GetBytes(param.data);
                        __reqstm.Write(__tempbuff, 0x00, __tempbuff.Length);
                    }
                try
                {
                    using (MemoryStream __memstm = new MemoryStream())
                    {
                        using (Stream __netstm =
                            (__request.GetResponse() as HttpWebResponse).GetResponseStream())
                        {
                            int __vernier = 0x00;
                            byte[] __tempbuff = new byte[0x400];
                            while ((__vernier = __netstm.Read(__tempbuff, 0x00, __tempbuff.Length)) > 0x00)
                                __memstm.Write(__tempbuff, 0x00, __vernier);
                        }
                        __result = __memstm.ToArray();
                    }
                    resultsign = true;
                }
                catch { __result = null; }
            }

            return __result;
        }
    }
}
