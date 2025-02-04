using System.Diagnostics;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.WebRequestMethods;

namespace JiemaGUIToolTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            submit.IsEnabled = false;
            string __phonenum = phonenum.Text;
            ThreadPool.QueueUserWorkItem(o => {
                string __phone = o as string;
                string __serialno = string.Empty;
                string __message = string.Empty;

                __serialno = __platformjiema_requestnewphone(__phone);


                this.Dispatcher.BeginInvoke(() => {
                    message.Text = $"phone number {__phone} accepted, serial no: {__serialno}\r\nstart receiving verify code...";
                });

                __message = __platformjiema_requestquery(__serialno);


                this.Dispatcher.BeginInvoke(() => {
                    message.Text = __message;
                    MessageBox.Show("success");
                    submit.IsEnabled = true;
                });
            },__phonenum);
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
                if (!string.IsNullOrEmpty(__param.Key) && !string.IsNullOrEmpty(__param.Value))
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

        public const string const_platform_jiema_baseuri = "http://47.123.6.54:9090";
        public const string const_platform_jiema_userid = "260";
        public const string const_platform_jiema_itemid = "57";
        public const string const_platform_jiema_userkey = "advd3su4mnizmwguzjstp12e9b8v9d7wdjllhode9zux1qqxm8k54s6exsnb7phn";

        public class jiemaapi_result_receiveCode
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

        public class jiemaapi_result_queryBizOrder
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
                public string? failCode { get; set; }
                public int? timeCost { get; set; }
                public int payState { get; set; }
                public string memo { get; set; }
                public string? ext { get; set; }
            }
            public string status { get; set; }
            public string desc { get; set; }
            public string code { get; set; }
            public cls_data data { get; set; }
        }

        private string __platformjiema_requestnewphone(string phonenum)
        {
            string __result_serialno = null;
            string __temp_serialno = $"{Guid.NewGuid().ToString("N").Substring(0x00, 0x4)}{phonenum}{DateTime.Now.ToString("yyyyMMddHHmmssfff")}";

            Dictionary<string, string> __params = new Dictionary<string, string>() {
                { "userId", const_platform_jiema_userid },
                { "itemId", const_platform_jiema_itemid },
                { "uid", phonenum },
                { "needReady", "1" },
                { "overKeep", "1" },
                { "serialno", __temp_serialno },
                { "dtCreate", DateTime.Now.ToString("yyyyMMddHHmmss") }
            };
            string __signstr = __sign(__params, const_platform_jiema_userkey);
            __params.Add("sign", __signstr);
            string __requesturi = $"{const_platform_jiema_baseuri}/cardCharge/receiveCode?{__getsearch(__params)}";
            bool __ret = false;
            string __response = Http.request(new Http.requestparam(__requesturi, HttpMethod.Get)
            {
                accept = "application/json;charset=UTF-8"
            }, out __ret);
            if (__ret && !string.IsNullOrEmpty(__response))
            {
                jiemaapi_result_receiveCode __respobj = null;
                try { __respobj = JsonSerializer.Deserialize<jiemaapi_result_receiveCode>(__response); } catch { }
                if (null != __respobj)
                    __result_serialno = __respobj.serialno;
            }
            else __result_serialno = null;

            return __result_serialno;
        }

        private string __platformjiema_requestquery(string serialno)
        {
            string __verifycode = null;

            Dictionary<string, string> __params = new Dictionary<string, string>()
            {
                { "userId", const_platform_jiema_userid },
                { "serialno", serialno }
            };
            string __signstr = __sign(__params, const_platform_jiema_userkey);
            __params.Add("sign", __signstr);

            string __requesturi = $"{const_platform_jiema_baseuri}/cardCharge/queryBizOrder?{__getsearch(__params)}";
            string __tempcode = null;

            while (string.IsNullOrEmpty(__tempcode))
            {
                bool __ret = false;
                string __response = Http.request(new Http.requestparam(__requesturi)
                {
                    method = HttpMethod.Get,
                    accept = $"application/json;charset=UTF-8"
                }, out __ret);
                if (__ret && !string.IsNullOrEmpty(__response))
                {
                    jiemaapi_result_queryBizOrder __respobj = null;
                    try { __respobj = JsonSerializer.Deserialize<jiemaapi_result_queryBizOrder>(__response); } catch { }
                    if (null != __respobj
                        && __respobj.code == "00"
                        && null != __respobj.data
                        && __respobj.data.status == "2"
                        && !string.IsNullOrEmpty(__respobj.data.memo))
                    {
                        __verifycode = __respobj.data.memo;
                        break;
                        //if (Regex.IsMatch(__respobj.data.memo, "(?<=您的验证码为).*?(?=，该验证码)", RegexOptions.None))
                        //{
                        //    __verifycode = Regex.Match(__respobj.data.memo, "(?<=您的验证码为).*?(?=，该验证码)", RegexOptions.None).Value;
                        //    break;
                        //}
                        //else if (Regex.IsMatch(__respobj.data.memo, "(?<=验证码为：).*?(?=。尊敬的用户)", RegexOptions.None))
                        //{
                        //    __verifycode = Regex.Match(__respobj.data.memo, "(?<=验证码为：).*?(?=。尊敬的用户)", RegexOptions.None).Value;
                        //    break;
                        //}
                        //else
                        //{
                        //    Thread.Sleep(0x3e8);
                        //}
                    }
                    else
                    {

                        Thread.Sleep(0x3e8);
                    }
                }
                else
                {
                    Thread.Sleep(0x3e8);
                }
            }

            return __verifycode;
        }
    }
}