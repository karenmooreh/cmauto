using Microsoft.Win32;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Net;
using System.Reflection;
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

namespace P03
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            __importedphonenums = new ObservableCollection<Models.PhoneNum>();
            __unworkingqueue = new ConcurrentQueue<Models.PhoneNum>();
            __workingpool = new ConcurrentDictionary<string, Models.PhoneNum>();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CTL_DATA.ItemsSource = __importedphonenums;

            //test
            CTL_USERID.Text = "260";
            CTL_ITEMID.Text = "57";
            CTL_USERKEY.Text = "advd3su4mnizmwguzjstp12e9b8v9d7wdjllhode9zux1qqxm8k54s6exsnb7phn";

            ThreadPool.QueueUserWorkItem(o =>
            {
                this.Dispatcher.BeginInvoke(() => {
                    CTL_LOG.Text = $"[{DateTime.Now.ToString()}]正在初始化代理模块...";
                });
                __kdlapimanager = new kdlapiproxy();
                this.Dispatcher.BeginInvoke(() => {
                    CTL_LOG.Text = $"[{DateTime.Now.ToString()}]代理模块初始化成功，Token：{__kdlapimanager.token}";
                });
            });
        }

        private ObservableCollection<Models.PhoneNum> __importedphonenums;
        private ConcurrentQueue<Models.PhoneNum> __unworkingqueue;
        private ConcurrentDictionary<string, Models.PhoneNum> __workingpool;

        private kdlapiproxy __kdlapimanager;

        private int __runlimit = 0x0a;

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            //string __ss = new Jiema("18705196432", "260", "57", "advd3su4mnizmwguzjstp12e9b8v9d7wdjllhode9zux1qqxm8k54s6exsnb7phn")
            //    .TakeandWait();
            IPEndPoint __ipep = __kdlapimanager.getproxy();

            //Proxy __webproxy = new Proxy();
            //ChromeOptions __opts = new ChromeOptions();
            //__opts.AddArgument($"--user-agent=Mozilla/5.0 (Linux; Android 10; HLK-AL10 Build/HONORHLK-AL10; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/97.0.4692.98 Mobile Safari/537.36 T7/13.64 BDOS/1.0 (HarmonyOS 2.2.0) SP-engine/3.5.0 baiduboxapp/13.64.4.10 (Baidu; P1 10) NABar/1.0");
            //__opts.AddArgument($"--proxy-server={settings.proxyserv.authuser}:{settings.proxyserv.password}@{__ipep.Address.ToString()}:{__ipep.Port}");

            //ChromeDriver __webdv = new ChromeDriver(
            ////System.IO.Path.Combine($"{System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}", "drivers"),
            //__opts);

            //EdgeOptions __edgeopts = new EdgeOptions() { 
            //    Proxy = new Proxy() { 
            //        Kind = ProxyKind.Manual,
            //        HttpProxy = $"{settings.proxyserv.authuser}:{settings.proxyserv.password}@{__ipep.Address.ToString()}:{__ipep.Port}",
            //        SslProxy = $"{settings.proxyserv.authuser}:{settings.proxyserv.password}@{__ipep.Address.ToString()}:{__ipep.Port}",
            //        SocksProxy = $"{settings.proxyserv.authuser}:{settings.proxyserv.password}@{__ipep.Address.ToString()}:{__ipep.Port}",
            //        SocksUserName = settings.proxyserv.authuser,
            //        SocksPassword = settings.proxyserv.password,
            //        SocksVersion = 0x05
            //    }
            //};
            //IWebDriver __webdv = new EdgeDriver(__edgeopts);
            FirefoxProfile __ffprof = new FirefoxProfile();
            FirefoxOptions __ffopts = new FirefoxOptions();
            __ffprof.SetPreference($"network.proxy.type", 0x01);
            __ffprof.SetPreference($"network.proxy.http", __ipep.Address.ToString());
            __ffprof.SetPreference($"network.proxy.http_port", __ipep.Port);
            __ffprof.SetPreference($"network.proxy.ssl", __ipep.Address.ToString());
            __ffprof.SetPreference($"network.proxy.ssl_port", __ipep.Port);
            __ffprof.SetPreference($"network.proxy.username", settings.proxyserv.authuser);
            __ffprof.SetPreference($"network.proxy.password", settings.proxyserv.password);

            __ffprof.SetPreference($"general.useragent.override", "Mozilla/5.0 (Linux; Android 10; HLK-AL10 Build/HONORHLK-AL10; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/97.0.4692.98 Mobile Safari/537.36 T7/13.64 BDOS/1.0 (HarmonyOS 2.2.0) SP-engine/3.5.0 baiduboxapp/13.64.4.10 (Baidu; P1 10) NABar/1.0");

            __ffprof.SetPreference($"network.proxy.autoconfig_url.include_path", false);
            __ffprof.SetPreference($"signon.autologin.proxy", true);
            __ffprof.SetPreference($"network.automatic-ntlm-auth.allow-proxies", true);
            __ffprof.SetPreference($"network.proxy.autoconfig_url", $"http://{settings.proxyserv.authuser}:{settings.proxyserv.password}@{__ipep.Address.ToString()}:{__ipep.Port}");

            __ffopts.Profile = __ffprof;
            __ffopts.AcceptInsecureCertificates = true;

            IWebDriver __webdv = new FirefoxDriver(__ffopts);


            //WebDriverWait __wait = new WebDriverWait(__webdv, TimeSpan.FromSeconds(0x0f));
            //__wait.Until(WebDeiverIsPresent);
            IAlert __alter = __webdv.SwitchTo().Alert();
            __alter.SendKeys(settings.proxyserv.authuser);
            __alter.SendKeys(Keys.Tab);
            __alter.SendKeys(settings.proxyserv.password);
            __alter.Accept();

            __webdv.Manage().Window.Position = new System.Drawing.Point(50,50);
            __webdv.Navigate().GoToUrl("https://ip138.com");



        }

        private static bool WebDeiverIsPresent(IWebDriver WebDriver)
        {
            try { WebDriver.SwitchTo().Alert(); return true; } catch {  return false; }
        }

        private void Test2_Click(object sender, RoutedEventArgs e)
        {
            IWebDriver __driver = new ChromeDriver(new ChromeOptions() { });
            __driver.Navigate().GoToUrl(Global.CONST_PAGEURL);
            var __dom_dinggou = __driver.FindElement(By.XPath(Global.const_xpaths[Global.CONST_XPATHKEY_WOYAODINGYUE]));
            __dom_dinggou.Click();
            var __dom_phonenum = __driver.FindElement(By.XPath(Global.const_xpaths[Global.CONST_XPATHKEY_SHOUJIHAOMA]));
            __dom_phonenum.SendKeys($"13584004251");
            var __dom_checked = __driver.FindElement(By.XPath(Global.const_xpaths[Global.CONST_XPATHKEY_YUEDUTONGYI]));
            __dom_checked.Click();
            var __dom_getcode = __driver.FindElement(By.XPath(Global.const_xpaths[Global.CONST_XPATHKEY_HUOQUYANZHENGMA]));
            __dom_getcode.Click();

            var __dom_code = __driver.FindElement(By.XPath(Global.const_xpaths[Global.CONST_XPATHKEY_TIANXIEYANZHENGMA]));
            __dom_code.SendKeys($"123456");

            var __dom_sure = __driver.FindElement(By.XPath(Global.const_xpaths[Global.CONST_XPATHKEY_QUERENDINGGOU]));
            __dom_sure.Click();

            
            __driver.Quit();

        }

        private Thread __thd_workingmonitor;
        private void CTL_START_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(CTL_USERID.Text) ||
                string.IsNullOrEmpty(CTL_ITEMID.Text) ||
                string.IsNullOrEmpty(CTL_USERKEY.Text) ||
                string.IsNullOrEmpty(CTL_PHONENUMSFILE.Text))
            {
                MessageBox.Show(this, "当前配置未填写完整，不能启动任务，请检查后重试", "任务操作提示",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show(this, "是否确认开始本次任务？", "任务操作提示",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                int.TryParse(CTL_RUNLIMIT.Text, out __runlimit);

                (__thd_workingmonitor = new Thread(
                    new ThreadStart(__thdmtd_workingmonitoring))
                { IsBackground = true }).Start();
            }
        }

        private void CTL_IMPORT_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog __ofd = new OpenFileDialog() {
                Title = "请选择需要导入的号码文件",
                Filter = "标准分割符数据文件(*.csv)|*.csv"
            };

            if(__ofd.ShowDialog() == true)
            {
                __importedphonenums.Clear();
                __unworkingqueue.Clear();
                __workingpool.Clear();

                __userid = CTL_USERID.Text;
                __userkey = CTL_USERKEY.Text;
                __itemid = CTL_ITEMID.Text;

                string __filecontent = System.IO.File.ReadAllText(CTL_PHONENUMSFILE.Text = __ofd.FileName);
                string[] __filerows = Regex.Split(__filecontent, @"\r*\n", RegexOptions.IgnorePatternWhitespace);
                int __sncount = 0x00;
                foreach(var __row in __filerows) {
                    string[] __rowdata = __row.Split(',');
                    if (!string.IsNullOrEmpty(__rowdata[0x00]) && Regex.IsMatch(__rowdata[0x00].Trim(), @"^1[3456789]\d{9}$")) {
                        string __phonenum = __rowdata[0x00].Trim();
                        if (!__importedphonenums.Any(p => p.phonenum == __phonenum))
                        {
                            __sncount++;
                            var __phonedata = new Models.PhoneNum() {
                                sn = __sncount.ToString().PadLeft(0x05, '0'),
                                snum = __sncount,
                                phonenum = __phonenum,
                                status = "待取号",
                                serialno = "-",
                                log = "-",
                                code = "-",
                                workthread = new Thread(new ParameterizedThreadStart(__thdmtd_phoneworking)) { IsBackground = true }
                            };
                            __importedphonenums.Add(__phonedata);
                            __unworkingqueue.Enqueue(__phonedata);
                        }
                    }
                }
            }
        }

        private string __userid = string.Empty,
            __itemid = string.Empty, __userkey = string.Empty;

        private void CTL_EXPORT_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog __sfd = new SaveFileDialog() { 
                Title = "导出本次数据文件",
                Filter = "标准分隔符数据文件（*.csv）|*.csv"
            };
            if (__sfd.ShowDialog() == true)
            {
                var __exportdata = __importedphonenums.OrderBy(p => p.success).ToList();
                StringBuilder __expbuilder = new StringBuilder();
                __expbuilder.AppendLine($"号码,结果,流水号,验证码,处理时间,用户ID：{__userid},商品ID：{__itemid},用户密钥：{__userkey}");
                foreach(var __linedata in __exportdata)
                    __expbuilder.AppendLine($"{__linedata.phonenum},{__linedata.status},{__linedata.serialno},{(__linedata.success?__linedata.code:"")},{__linedata.finishtime.ToString()}");
                
                if (System.IO.File.Exists(__sfd.FileName))
                    System.IO.File.Delete(__sfd.FileName);

                System.IO.File.AppendAllText(__sfd.FileName, __expbuilder.ToString());

                MessageBox.Show(this, "数据导出成功", "操作提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void __thdmtd_workingmonitoring()
        {
            this.Dispatcher.BeginInvoke(() => {
                CTL_USERID.IsEnabled = CTL_ITEMID.IsEnabled =
                CTL_USERKEY.IsEnabled = CTL_IMPORT.IsEnabled =
                CTL_START.IsEnabled = CTL_RUNLIMIT.IsEnabled = false;

                CTL_EXPORT.Visibility = Visibility.Collapsed;
            });

            int __workedcount = 0x00;

            while (__unworkingqueue.Count != 0x00)
            {
                if (__workingpool.Count < __runlimit) {
                    Models.PhoneNum __phone;
                    if(__unworkingqueue.TryDequeue(out __phone))
                    {
                        __workingpool.TryAdd(__phone.phonenum, __phone);
                        __phone.workthread.Start(__phone);
                        __workedcount++;
                        this.Dispatcher.BeginInvoke(() => {
                            CTL_LOG.Text = $"[{DateTime.Now.ToString()}]号码 {__phone.phonenum} 已入列处理中，当前{__workedcount} / {__importedphonenums.Count}";
                        });
                    }
                } else Thread.Sleep(settings.configs.singleinterval);
            }


            while (__workingpool.Count > 0x00) {

                this.Dispatcher.BeginInvoke(() => {
                    CTL_LOG.Text = $"[{DateTime.Now.ToString()}]等待最后剩余 {__workingpool.Count} 个号码完成操作...";
                });
                Thread.Sleep(0x03e8);
            }

            this.Dispatcher.BeginInvoke(() =>
            {
                CTL_LOG.Text = $"[{DateTime.Now.ToString()}]所有号码已全部处理完毕，总计入列处理 {__workedcount} 个，其中 {__importedphonenums.Count(p => p.success)} 个号码提交成功，{__importedphonenums.Count(p => !p.success)} 个号码未成功。";

                CTL_USERID.IsEnabled = CTL_ITEMID.IsEnabled =
                CTL_USERKEY.IsEnabled = CTL_IMPORT.IsEnabled =
                CTL_START.IsEnabled = CTL_RUNLIMIT.IsEnabled = true;

                CTL_EXPORT.Visibility = Visibility.Visible;
            });
        }

        private void __thdmtd_phoneworking(object obj)
        {
            Models.PhoneNum __phone = obj as Models.PhoneNum;
            __phone.log = "正在启动取号接口，请稍候...";
            __phone.status = "取号中";

            Jiema __jiema = new Jiema(__phone.phonenum, __userid, __itemid, __userkey);

            string __serialno = null;
            int __failedcount = 0x00;
            while (string.IsNullOrEmpty(__serialno) && __failedcount < 0x64) {
                bool __ret = __jiema.Take(out __serialno);
                if (__ret) break;
                else {
                    __failedcount++;
                    __serialno = null;
                    __phone.log = $"第 {__failedcount} 次取号失败，稍候将重新尝试取号...";
                    Thread.Sleep(0x03e8);
                }
            }
            if(!string.IsNullOrEmpty(__serialno) && __failedcount < 0x64)
            {
                __phone.status = "取号成功";
                __phone.serialno = __serialno;

                IPEndPoint __apiproxy = __kdlapimanager.getproxy();
                ChromeOptions __chromeoptions = new ChromeOptions()
                {
                    Proxy = new Proxy()
                    {
                        Kind = ProxyKind.Manual,
                        IsAutoDetect = false,
                        HttpProxy = $"{settings.proxyserv.authuser}:{settings.proxyserv.password}@{__apiproxy.Address.ToString()}:{__apiproxy.Port}",
                        SslProxy = $"{settings.proxyserv.authuser}:{settings.proxyserv.password}@{__apiproxy.Address.ToString()}:{__apiproxy.Port}",
                    },

                };
                __chromeoptions.AddArgument($"--user-agent={UserAgentsLib.getone()}");
                __chromeoptions.AddArgument($"--proxy-server=http://{settings.proxyserv.authuser}:{settings.proxyserv.password}@{__apiproxy.Address.ToString()}:{__apiproxy.Port}");
                IWebDriver __driver = new ChromeDriver();
                __driver.Manage().Window.Position =
                    new System.Drawing.Point((__phone.snum % 0x0a) * 0x32, (__phone.snum / 0x0a) * 0x32);

                string __code = null;
                int __rollcount = 0x00;
                while (string.IsNullOrEmpty(__code) && __rollcount < 0x0a) {
                    __rollcount++;
                    __phone.log = $"[第 {__rollcount} 轮]正在前往活动页面填报数据，请稍候...";
                    __driver.Navigate().GoToUrl(Global.CONST_PAGEURL);
                    var __dom_dinggou = __driver.FindElement(By.XPath(Global.const_xpaths[Global.CONST_XPATHKEY_WOYAODINGYUE]));
                    __dom_dinggou.Click();
                    var __dom_phonenum = __driver.FindElement(By.XPath(Global.const_xpaths[Global.CONST_XPATHKEY_SHOUJIHAOMA]));
                    __dom_phonenum.SendKeys(__phone.phonenum);
                    var __dom_checked = __driver.FindElement(By.XPath(Global.const_xpaths[Global.CONST_XPATHKEY_YUEDUTONGYI]));
                    __dom_checked.Click();
                    var __dom_getcode = __driver.FindElement(By.XPath(Global.const_xpaths[Global.CONST_XPATHKEY_HUOQUYANZHENGMA]));
                    __dom_getcode.Click();

                    __phone.status = "等待取码";

                    bool __queryresult = false;
                    int __querycount = 0x00;
                    while (__querycount < 0x48 && !__queryresult)
                    {
                        __querycount++;
                        __phone.log = $"[第 {__rollcount} 轮第 {__querycount} 次]正在查询验证码接收结果...";
                        __code = __jiema.QueryCode(__serialno);

                        if (!string.IsNullOrEmpty(__code)) {
                            __queryresult = true;
                            break;
                        } else Thread.Sleep(0x03e8);
                    }

                    if (__queryresult)
                    {
                        __phone.status = "已取码";
                        __phone.log = $"[第 {__rollcount} 轮第 {__querycount} 次]正确获取到验证码：{__code}";
                        break;
                    }
                    else Thread.Sleep(0x03e8);
                }
                if (!string.IsNullOrEmpty(__code) && __rollcount <= 0x0a) {

                    var __dom_code = __driver.FindElement(By.XPath(Global.const_xpaths[Global.CONST_XPATHKEY_TIANXIEYANZHENGMA]));
                    __dom_code.SendKeys(__code);

                    var __dom_sure = __driver.FindElement(By.XPath(Global.const_xpaths[Global.CONST_XPATHKEY_QUERENDINGGOU]));
                    __dom_sure.Click();

                    bool __successfound = false;
                    int __foundcount = 0x00;
                    while (!__successfound && __failedcount < 0x64)
                    {
                        __foundcount++;
                        __successfound = __webdriver_findelement(__driver, Global.const_xpaths[Global.CONST_XPATHKEY_SUCCESS]);
                        Thread.Sleep(0x3e8);
                    }

                    if (__successfound)
                    {
                        __phone.success = true;
                        __phone.status = "提交成功";
                        __phone.log = $"已向活动页面提交数据成功";
                    }
                    else
                    {
                        __phone.success = false;
                        __phone.status = "提交超时";
                        __phone.log = "最终提交时网络超时，请手工或在下个批次中处理";
                    }
                }
                else
                {
                    __phone.log = $"已尝试 {__rollcount} 轮但并未正确获取到验证码，本号码已取消操作...";
                }

                __driver.Quit();
                __driver.Dispose();
            }
            else
            {
                __phone.status = "取号失败";
                __phone.log = $"取号失败，经过100次尝试后，已取消该号码取号.";
            }

            __phone.finishtime = DateTime.Now;
            Models.PhoneNum __rmphone;
            __workingpool.TryRemove(__phone.phonenum, out __rmphone);
        }

        public bool __webdriver_findelement(IWebDriver driver, string xpath)
        {
            bool __result = false;

            try {
                var __foundelement = driver.FindElement(By.XPath(xpath));
                if (null != __foundelement) __result = true;
            } catch { }

            return __result;
        }
    }
}