using System.Security.Cryptography;
using System.Text;
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

namespace fsFirstCauseEmulator
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //CTL_AMOUNT.ItemsSource = new comboitem[] {
            //    new comboitem(){ display = "￥10.00", value = "10" },
            //    new comboitem(){ display = "￥30.00", value = "30" },
            //};
        }

        internal class comboitem
        {
            public string display { get; set; }
            public string value { get; set; }
        }

        private void CTL_BILLINGNO_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CTL_BILLINGNO.Text = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }

        private void CTL_BTNSUBMIT_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show(this,"是否确认提交该订单到云端接口？", "操作提示", MessageBoxButton.YesNo, MessageBoxImage.Question)
                == MessageBoxResult.Yes)
            {
                Dictionary<string, string> __params = new Dictionary<string, string>() {
                    { "appid", "15c6f4a66cca4d96952b9e58527edb26" },
                    { "billingno", CTL_BILLINGNO.Text },
                    { "phonenum", CTL_PHONENUM.Text },
                    { "amount", null == (ComboBoxItem)CTL_AMOUNT.SelectedItem ?
                        CTL_AMOUNT.Text : 
                        CTL_AMOUNT.Text != ((ComboBoxItem)CTL_AMOUNT.SelectedItem).Content.ToString() ? 
                            CTL_AMOUNT.Text : ((ComboBoxItem)CTL_AMOUNT.SelectedItem).Content.ToString() },
                    { "notify_uri", string.Empty },
                    { "param", "abcd1234" },
                };
                string __signstr = __sign(__params, "a31a2f0aac07d62a947967c57d029782");
                __params.Add("sign", __signstr);
                string __requesturi = $"{CTL_APIBASE.Text}?{__getsearch(__params)}";

                CTL_BTNSUBMIT.IsEnabled = false;

                ThreadPool.QueueUserWorkItem(o => {

                    string __uri = o as string;
                    string __response = null;
                    bool __ret = false;
                    __response = Http.request(new Http.requestparam(__uri), out __ret);

                    this.Dispatcher.BeginInvoke(() =>
                    {
                        CTL_RESPONSE.Text = __response;
                        CTL_BTNSUBMIT.IsEnabled = true;
                        MessageBox.Show(this, 
                            __ret ? "订单数据已提交到云端接口" : "订单数据提交失败，请检查接口服务是否存在", "操作提示",
                            MessageBoxButton.OK, 
                            __ret ? MessageBoxImage.Information : MessageBoxImage.Error);
                    });
                }, __requesturi);

                
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


        public static string __md5crypto(string proclaimed)
        {
            return Regex.Replace(
                BitConverter.ToString(
                    MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(proclaimed))),
                "\\-", string.Empty, RegexOptions.None).ToLower();
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

    }
}