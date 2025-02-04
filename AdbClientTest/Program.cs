// See https://aka.ms/new-console-template for more information
using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.DeviceCommands;
using AdvancedSharpAdbClient.DeviceCommands.Models;
using AdvancedSharpAdbClient.Models;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

//string __result = string.Empty;
//var __dtNow = DateTime.Now;
//DateTime __dtUtcNow = new DateTime(__dtNow.Year, __dtNow.Month, __dtNow.Day,
//    __dtNow.Hour, __dtNow.Minute, __dtNow.Second,
//    DateTimeKind.Local);
//__result = __dtUtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss K");
//__result = __result.Substring(0x00, __result.LastIndexOf(":")) +
//    __result.Substring(__result.LastIndexOf(":") + 0x01);

Console.WriteLine("Hello, World!");

string __adbfile = "C:\\Program Files\\Android\\adt-bundle-windows-x86_64-20140702\\sdk\\platform-tools\\adb.exe";

AdbServer __adbserv = new AdbServer();
StartServerResult __stresult = __adbserv.StartServer(__adbfile, false);
Console.WriteLine($">adb server start result: {__stresult}");

if(__stresult == StartServerResult.Started || __stresult == StartServerResult.AlreadyRunning)
{
    AdbClient __adbclient = new AdbClient();
    string __connresult = __adbclient.Connect("103.36.199.202", 5555);
    var __devices = __adbclient.GetDevices();
    DeviceData __dev1;
    if (__devices.Any(d => d.Serial == "103.36.199.202:5555"))
    {
        __dev1 = __devices.SingleOrDefault(d => d.Serial == "103.36.199.202:5555");
        var __devclient = __dev1.CreateDeviceClient();

        //__devclient.AdbClient.SendText(__dev1, "12345678");
        //__devclient.AdbClient.ExecuteShellCommand(__dev1,"input text 13584004251");

        Element __foundelement = null;

        #region dump xml

        if (false)
        {
            XmlDocument __xdoc = __devclient.DumpScreen();
            ;

        }

        if (false)
        {
            string __dumpxml = __devclient.DumpScreenString();
            ;
        }

        if (false)
        {
            using (System.IO.MemoryStream __memstm = new MemoryStream())
            {
                __devclient.AdbClient.Pull(__devclient.Device, "/sdcard/window_dump.xml", __memstm,
                    new Action<SyncProgressChangedEventArgs>(o => { }));
                byte[] __tempbuff = __memstm.ToArray();
                string __xml = Encoding.UTF8.GetString(__tempbuff);
                ;
            }
        }

        #endregion

        #region

        if (false)
        {
            __devclient.AdbClient.StartApp(__devclient.Device, "com.greenpoint.android.mc10086.activity");
            Thread.Sleep(0x03e8 * 0x05);
        }

        #endregion

        #region

        if (true)
        {
            #region startup agreements operations
            if (true)
            {

                if (__adb_waitforelement(__devclient.AdbClient, __devclient.Device,
                    "//node[@content-desc=\"查阅并同意完整的《服务协议》，《儿童隐私保护须知》及《隐私政策》\"]",
                    0x05, out __foundelement))
                {
                    __foundelement.Click();
                    Thread.Sleep(0x03e8);
                    if (__adb_waitforelement(__devclient.AdbClient, __devclient.Device,
                        "//node[@text=\"同意\" and @class=\"android.widget.Button\"]",
                        0x03, out __foundelement))
                    {
                        __foundelement.Click();
                        Thread.Sleep(0x03e8 * 0x05);
                    }
                }

                else if (__adb_waitforelement(__devclient.AdbClient, __devclient.Device,
                    "//node[@text=\"授权提醒\"]",
                    0x05, out __foundelement))
                {
                    if (__adb_waitforelement(__devclient.AdbClient, __devclient.Device,
                        "//node[@text=\"确认\" and @class=\"android.widget.TextView\"]",
                        0x03, out __foundelement))
                    {
                        __foundelement.Click();
                        Thread.Sleep(0x03e8 * 0x03);
                    }
                }

                else if (__adb_waitforelement(__devclient.AdbClient, __devclient.Device,
                    "//node[@text=\"“中国移动”将获取您的粘贴内容，用于分享、识别，您允许这样做吗？\"]",
                    0x05, out __foundelement))
                {
                    
                    if (__adb_waitforelement(__devclient.AdbClient, __devclient.Device,
                        "//node[@text=\"允许\" and @class=\"android.widget.Button\"]",
                        0x03, out __foundelement))
                    {
                        __foundelement.Click();
                        Thread.Sleep(0x03e8 * 0x03);
                    }
                }
            }
            #endregion

            #region signin process
            if (false)
            {
                if(__adb_waitforelement(__devclient.AdbClient, __devclient.Device,
                    "//node[@content-desc=\"账号切换\" and @class=\"android.widget.LinearLayout\"]",
                    0x05, out __foundelement)) 
                {
                    if(__adb_waitforelement(__devclient.AdbClient, __devclient.Device,
                        "//node[@text=\"登录\" and @class=\"android.widget.TextView\"]",
                        0x03, out __foundelement))
                    {
                        __foundelement.Click();
                        Thread.Sleep(0x03e8 * 0x05);
                    }else if(
                        __adb_waitforelement(__devclient.AdbClient, __devclient.Device,
                        "//node[@text=\"切换\" and @resource-id=\"com.greenpoint.android.mc10086.activity:id/tv_title_account_change\"]",
                        0x05, out __foundelement))
                    {
                        __foundelement.Click();

                        
                        Thread.Sleep(0x03e8 * 0x05);

                        if(__adb_waitforelement(__devclient.AdbClient, __devclient.Device,
                            "//node[@resource-id=\"com.greenpoint.android.mc10086.activity:id/tv_current_phone_number\"]",
                            0x03, out __foundelement))
                        {
                            string __curphoneno = Regex.Replace(__foundelement.Text, "当前号码：", string.Empty, RegexOptions.None);
                        }

                        if(__adb_waitforelement(__devclient.AdbClient, __devclient.Device,
                            "//node[@text=\"其他账号登录\" and @class=\"android.widget.TextView\"]",
                            0x03, out __foundelement))
                        {
                            __foundelement.Click();
                            Thread.Sleep(0x03e8 * 0x05);
                        }

                    }
                }

                #region signin pad
                Element __anchorelement = null;
                if(__adb_waitforelement(__devclient.AdbClient, __devclient.Device,
                    "//node[@text=\"欢迎登录中国移动\"]",
                    0x05, out __anchorelement))
                {
                    if(__adb_waitforelement(__devclient.AdbClient, __devclient.Device,
                        "//node[@resource-id=\"com.greenpoint.android.mc10086.activity:id/user_phoneno_edt\"]",
                        0x05, out __foundelement))
                    {
                        string __runshell = $"input tap {((__foundelement.Bounds.Left + __foundelement.Bounds.Right) / 0x02)} {((__foundelement.Bounds.Top + __foundelement.Bounds.Bottom) / 0x02)}";
                        __devclient.AdbClient.ExecuteShellCommand(__devclient.Device, __runshell);
                        ;
                        Thread.Sleep(0x03e8);
                        __devclient.AdbClient.ClearInput(__devclient.Device, 0x0b);
                        Thread.Sleep(0x03e8 * 0x03);
                        __foundelement.SendText("18260071042");
                        Thread.Sleep(0x03e8 * 0x03);
                        __runshell = $"input tap {((__anchorelement.Bounds.Left + __anchorelement.Bounds.Right) / 0x02)} {((__anchorelement.Bounds.Top + __anchorelement.Bounds.Bottom) / 0x02)}";
                        Thread.Sleep(0x03e8 * 0x03);

                        if(__adb_waitforelement(__devclient.AdbClient, __devclient.Device,
                            "//node[@resource-id=\"com.greenpoint.android.mc10086.activity:id/iv_service_protocol_check\"]",
                            0x03, out __foundelement))
                        {
                            __foundelement.Click();
                            Thread.Sleep(0x03e8);
                        }

                        if(__adb_waitforelement(__devclient.AdbClient, __devclient.Device,
                            "//node[@resource-id=\"com.greenpoint.android.mc10086.activity:id/login_checksms_btn\" and @class=\"android.widget.Button\"]",
                            0x03, out __foundelement))
                        {
                            __foundelement.Click();
                            Thread.Sleep(0x03e8 * 0x03);
                        }

                        if (true)
                        {
                            if(__adb_waitforelement(__devclient.AdbClient, __devclient.Device,
                                "//node[@content-desc=\"登录\" and @class=\"android.widget.Button\"]",
                                0x03, out __foundelement))
                            {
                                __foundelement.Click();
                                Thread.Sleep(0x03e8 * 0x05);
                            }
                        }
                    }
                }

                #endregion
            }
            #endregion

            #region drop first guide mask layout
            if (false)
            {
                if(__adb_waitforelement(__devclient.AdbClient, __devclient.Device,
                    "/hierarchy/node/node[@class=\"android.widget.ImageView\"]",
                    0x05, out __foundelement))
                {
                    //var __attrs = __foundelement.Attributes;
                    //__foundelement.Attributes["enabled"] = "false";

                    __foundelement.Click();
                    Thread.Sleep(0x03e8);
                }
            }
            #endregion

            #region close ads
            if (false)
            {
                if(__adb_waitforelement(__devclient.AdbClient, __devclient.Device,
                    "//node[@content-desc=\"广告\" and @resource-id=\"com.greenpoint.android.mc10086.activity:id/ad_image\"]",
                    0x05, out __foundelement))
                {
                    if(__adb_waitforelement(__devclient.AdbClient,__devclient.Device,
                        "//node[@resource-id=\"com.greenpoint.android.mc10086.activity:id/close_btn\" and @content-desc=\"关闭\"]",
                        0x05, out __foundelement))
                    {
                        __foundelement.Click();
                        Thread.Sleep(0x03e8 * 0x03);
                    }
                }
            }
            #endregion

            #region recharge

            if (true)
            {

                if(__adb_waitforelement(__devclient.AdbClient, __devclient.Device,
                    "//node[@text=\"充值·缴费\"]",
                    0x05, out __foundelement))
                {
                    __foundelement.Click();
                    ;
                }

                //tap to recharge
                //string __runshell = $"input tap 270 580";
                //__devclient.AdbClient.ExecuteShellCommand(__devclient.Device, __runshell);
                //Thread.Sleep(0x03e8 * 0x05);

            }

            #endregion
        }

        #endregion

    }
}

Console.ReadLine();



static bool __adb_waitforelement(IAdbClient device,
    DeviceData devdata,
    string xpath, int timeoutseconds, 
    out Element foundelement)
{
    bool __result = false;
    foundelement = null;

    Stopwatch __timewatcher = new Stopwatch();
    __timewatcher.Start();
    while (!__result && __timewatcher.Elapsed.TotalSeconds < timeoutseconds)
    {
        try
        {
            foundelement = device.FindElement(devdata, xpath, TimeSpan.FromSeconds(timeoutseconds / 0x03));
        }
        catch { }
        if (null != foundelement) __result = true;
        else Thread.Sleep(0x0a);
    }
    __timewatcher.Stop();

    return __result;
}