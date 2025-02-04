using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.DeviceCommands;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.BiDi.Modules.Input;
using OpenQA.Selenium.DevTools.V129.DeviceAccess;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace fsLocalService
{
    public partial class ServCore
    {
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

            Dictionary<string,string> __params = new Dictionary<string, string>() {
                { "userId", confs.settings.const_platform_jiema_userid },
                { "itemId", confs.settings.const_platform_jiema_itemid },
                { "uid", phonenum },
                { "needReady", "1" },
                { "overKeep", "1" },
                { "serialno", __temp_serialno },
                { "dtCreate", DateTime.Now.ToString("yyyyMMddHHmmss") }
            };
            string __signstr = __sign(__params, confs.settings.const_platform_jiema_userkey);
            __params.Add("sign", __signstr);
            string __requesturi = $"{confs.settings.const_platform_jiema_baseuri}/cardCharge/receiveCode?{__getsearch(__params)}";
            bool __ret = false;
            string __response = Http.request(new Http.requestparam(__requesturi, HttpMethod.Get) { 
                accept = "application/json;charset=UTF-8"
            }, out __ret);
            if (__ret && !string.IsNullOrEmpty(__response))
            {
                jiemaapi_result_receiveCode __respobj = null;
                try { __respobj = JsonSerializer.Deserialize<jiemaapi_result_receiveCode>(__response); } catch { }
                if(null != __respobj)
                    __result_serialno = __respobj.serialno;
            }
            else __result_serialno = null;

            return __result_serialno;
        }

        private string __platformjiema_requestquery(string serialno, int waitforseconds = 0x3c)
        {
            string __verifycode = null;

            Dictionary<string, string> __params = new Dictionary<string, string>()
            {
                { "userId", confs.settings.const_platform_jiema_userid },
                { "serialno", serialno }
            };
            string __signstr = __sign(__params, confs.settings.const_platform_jiema_userkey);
            __params.Add("sign", __signstr);

            string __requesturi = $"{confs.settings.const_platform_jiema_baseuri}/cardCharge/queryBizOrder?{__getsearch(__params)}";
            string __tempcode = null;

            Stopwatch __timewatcher = new Stopwatch();
            __timewatcher.Start();

            while (string.IsNullOrEmpty(__tempcode) && __timewatcher.Elapsed.TotalSeconds < waitforseconds)
            {
                bool __ret = false;
                string __response = Http.request(new Http.requestparam(__requesturi)
                {
                    method = HttpMethod.Get,
                    accept = $"application/json;charset=UTF-8"
                }, out __ret);
                if (__ret && !string.IsNullOrEmpty(__response)) {
                    jiemaapi_result_queryBizOrder __respobj = null;
                    try { __respobj = JsonSerializer.Deserialize<jiemaapi_result_queryBizOrder>(__response); } catch { }
                    if(null != __respobj 
                        && __respobj.code == "00" 
                        && null != __respobj.data
                        && __respobj.data.status == "2"
                        && !string.IsNullOrEmpty(__respobj.data.memo))
                    {
                        if(Regex.IsMatch(__respobj.data.memo, "(?<=您的验证码为).*?(?=，该验证码)", RegexOptions.None))
                        {
                            __verifycode = Regex.Match(__respobj.data.memo, "(?<=您的验证码为).*?(?=，该验证码)", RegexOptions.None).Value;
                            __writelog($">jiema serialno[{serialno}] got verify code {__verifycode}.", ConsoleColor.Green);
                            break;
                        }
                        else if(Regex.IsMatch(__respobj.data.memo, "(?<=验证码为：).*?(?=。尊敬的用户)", RegexOptions.None))
                        {
                            __verifycode = Regex.Match(__respobj.data.memo, "(?<=验证码为：).*?(?=。尊敬的用户)", RegexOptions.None).Value;
                            __writelog($">jiema serialno[{serialno}] got verify code {__verifycode}.", ConsoleColor.Green);
                            break;
                        }
                        else
                        {
                            __writelog($">jiema serialno[{serialno}] unsupported regex match,source message:\r\n{__respobj.data.memo}", ConsoleColor.Green);
                            Thread.Sleep(0x3e8);
                        }
                    }
                    else
                    {
                        __writelog($">jiema serialno[{serialno}] waiting for verify code...", ConsoleColor.Green);

                        Thread.Sleep(0x3e8);
                    }
                }else {
                    __writelog($">jiema serialno[{serialno}] get nothing, wait for next request...", ConsoleColor.Yellow);
                    Thread.Sleep(0x3e8);
                }
            }

            __timewatcher.Stop();

            return __verifycode;
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

        private bool __scriptchapter_defaultactivity_checkresignneeded(androiddevice device)
        {
            bool __result = true;

            AppiumElement __foundtarget;
            bool __needchangeaccount = false;

            if (__appium_waitforelement(device.driver,
                "(//android.widget.ImageView[@resource-id=\"com.greenpoint.android.mc10086.activity:id/custom_tab_icon\"])[1]",
                0x14, out __foundtarget))
            {
                __foundtarget.Click();
                Thread.Sleep(0x3e8 * 0x05);
            }

            if (__appium_waitforelement(device.driver,
                //default activity login button
                "//android.widget.TextView[@resource-id=\"com.greenpoint.android.mc10086.activity:id/tv_title_login\"]",
                0x14, out __foundtarget))
            {
                __writelog($">device[{device.id}] cmcc official app button \"wode\" clicked.", ConsoleColor.Green);

                __needchangeaccount = true;
                __foundtarget.Click();
                Thread.Sleep(0x3e8);
            }
            else if (__appium_waitforelement(device.driver,
                //default activity change account button
                "//android.widget.TextView[@resource-id=\"com.greenpoint.android.mc10086.activity:id/tv_title_account_change\"]",
                0x14, out __foundtarget))
            {
                AppiumElement __tempelement;
                if (__appium_waitforelement(device.driver,
                    "//android.widget.TextView[@resource-id=\"com.greenpoint.android.mc10086.activity:id/tv_title_account\"]",
                    0x14, out __tempelement))
                {
                    string __currentaccount = __tempelement.Text;
                    string[] __currentaccount_headend = Regex.Split(__currentaccount, "\\*\\*\\*\\*", RegexOptions.None);
                    if (string.IsNullOrEmpty(device.workphonenum) ||
                        device.samenumcount >= 0x03 ||
                        !(device.workphonenum.StartsWith(__currentaccount_headend[0x00]) &&
                        device.workphonenum.EndsWith(__currentaccount_headend[0x01])))
                    {

                        __needchangeaccount = true;
                        __foundtarget.Click();
                        Thread.Sleep(0x03e8);
                        
                        if(__appium_waitforelement(device.driver,
                            "//android.widget.RelativeLayout[@resource-id=\"com.greenpoint.android.mc10086.activity:id/other_account_login\"]",
                            0x14, out __foundtarget))
                        {
                            __foundtarget.Click();
                            Thread.Sleep(0x03e8);
                        }

                    }
                }
                else if (device.samenumcount >= 0x03)
                {
                    __needchangeaccount = true;
                    __foundtarget.Click();
                    Thread.Sleep(0x3e8);
                }
            }

            return __result;
        }

        private bool __scriptchapter_securityactivity_cmccsignin(androiddevice device)
        {
            bool __result = false;


            device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input tap 400 400");
            Thread.Sleep(0x3e8 * 0x02);

            device.adbclient.ClearInput(device.adbdevicedata, 0x14);
            Thread.Sleep(0x3e8 * 0x02);

            device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input text {device.workphonenum}");
            Thread.Sleep(0x03e8 * 0x02);

            device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input tap 80 620");
            Thread.Sleep(0x3e8 * 0x02);

            device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input tap 610 525");
            Thread.Sleep(0x3e8*0x02);

            string __vcode = __platformjiema_requestquery(device.workserialno);

            device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input tap 200 500");
            Thread.Sleep(0x3e8 * 0x02);

            device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input text {__vcode}");
            Thread.Sleep(0x03e8 * 0x02);

            device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input tap 350 750");
            Thread.Sleep(0x3e8 * 0x02);

            return __result;
        }


        private void __thdmtd_cmccmonitoring(object dev)
        {
            androiddevice __device = dev as androiddevice;
            if(null != __device)
            {
                while (__status)
                {
                    if (!__device.isenvexception && !__device.isdevicelimited)
                    {
                        var __appstatus = AdvancedSharpAdbClient.Models.AppStatus.Stopped;
                        try
                        {
                            __appstatus = __device.adbclient.GetAppStatus(__device.adbdevicedata,
                                confs.settings.const_android_packages_cmcc);
                            if (__appstatus != __device.cmccstatus)
                            {
                                __writelog($">device[{__device.id}] cmcc app status changed, status: {__device.cmccstatus} -> {__appstatus}",
                                    ConsoleColor.Yellow);
                                __device.cmccstatus = __appstatus;
                            }
                        }
                        catch (Exception ex)
                        {
                            __writelog($">device[{__device.id}] adb error, message: {ex.Message}",
                                ConsoleColor.Yellow);
                        }
                    }
                    Thread.Sleep(0x64);
                }
            }
        }

        private void __thdmtd_working(object dev)
        {
            androiddevice __device = dev as androiddevice;
            
            if(null != __device)
            {
                int __reinstallcounter = 0x00;
                while (__status)
                {

                    #region when same phone worked 3 times or first run, request new phone to jiema platform and resign 
                    bool __resignneeded = __proc_rollnewphonenum(__device);


                    #endregion

                    bool __needfinishquit = true;
                    if (__reinstallcounter > 0x03 || __device.isenvexception || __device.isdevicelimited) 
                    {
                        //reinstall cloudphone system
                        __writelog($">device[{__device.id}] reinstall counter large than 3 times, request to reinstall.", ConsoleColor.Yellow);
                        __chinac_reinstall(__device);
                        __writelog($">device[{__device.id}] is reinstalling, wait for 3 minutes...", ConsoleColor.Green);
                        Thread.Sleep(0x02bf20);
                        __writelog($">device[{__device.id}] is reinstalled.");
                        __needfinishquit = false;
                        __reinstallcounter = 0x00;
                        __device.workphonenum = string.Empty;
                        __device.samenumcount = 0x03;
                        __device.isenvexception = false;
                        __device.isdevicelimited = false;

                        {
                            var __response_createcloudphoneadb = __ccmclient.CreateCloudPhoneAdb(new ChinacSDKCore.NET.Models.CreateCloudPhoneAdbRequest() { 
                                CloudPhoneIds = new List<string>() { __device.cloudid },
                                Region = __device.cloudregion
                            });
                            if (null != __response_createcloudphoneadb &&
                                __response_createcloudphoneadb.code == 10000)
                            {
                                __writelog($">device[{__device.id}] create adb successed.", ConsoleColor.Green);

                                __adb_connectadbclient(__device);
                            }
                            else
                            {
                                __writelog($">device[{__device.id}] create adb failed, humanul operate waitting...", ConsoleColor.Red);
                                Console.ReadLine();
                            }

                        }

                        continue;
                    }
                    else if (true && 
                        __resignneeded) 
                    {
                        #region resign operations
                        bool __ret = false;

                        while (!__ret)
                        {
                            Thread.Sleep(0x03e8 * 0x03);
                            if (__appium_startcmcc(__device))
                            {
                                __ret = __appium_startsignin(__device);

                                if (__ret && null != __device.driver)
                                {
                                    __chinac_onekeyhome(__device);
                                    __reinstallcounter++;
                                    __writelog($">device[{__device.id}] signin successed, homed.", ConsoleColor.Green);
                                    break;
                                }
                                else if (__device.samenumcount >= 0x03)
                                {
                                    __writelog($">device[{__device.id}] current phonenum[{__device.workphonenum}] signin failed, jumped.", ConsoleColor.Green);
                                    break;
                                }
                            }

                        }

                        #endregion
                    }
                    else if (true
                        && null != __device.currenttask && __device.currenttask.status == 0x03
                        ) 
                    {

                        #region check last task trade result

                        //__appium_startcmcc(__device);
                        //__appium_lasttradequery(__device);

                        __writelog($">device[{__device.id}] finish order and not submit result.", ConsoleColor.Cyan);
                        __device.currenttask.status = 0x04;

                        #endregion
                    }
                    else
                    {
                        #region device scramble billing and run task
                        models.task __dequeuetask = null;
                        while (!__con_localwaittingtasks.TryDequeue(out __dequeuetask))
                        {
                            __writelog($">device[{__device.id}] waiting for billing task...", ConsoleColor.Yellow);
                            Thread.Sleep(0x03e8);
                        }
                        __writelog($">device[{__device.id}] got task[billingno: {__dequeuetask.billingno}, phonenum: {__dequeuetask.phonenum}],task running...", ConsoleColor.Cyan);
                        __device.currenttask = __dequeuetask;

                        while (!__appium_startcmcc(__device)) Thread.Sleep(0x03e8);
                        var __taskret = __appium_tasking(__device);

                        if (!__taskret && __device.istaskflowinterrupted)
                        {
                            __con_localwaittingtasks.Enqueue(__device.currenttask);
                            __writelog($">device[{__device.id}] return the task[billingno: {__device.currenttask.billingno}] back to wait pool.", ConsoleColor.Yellow);
                            __device.currenttask = null;
                            __device.istaskflowinterrupted = false;
                        }

                        #endregion
                    }

                    //if (null != __device.driver)
                    //{
                        //__device.driver.Quit();
                        //__device.driver.Dispose();
                        //__device.driver = null;

                    __chinac_onekeyhome(__device);
                    __device.adbclient.ExecuteShellCommand(__device.adbdevicedata, $"api sdk/UTIL/CLEAN_ALL_APP?cleanTop=true");
                    Thread.Sleep(0x3e8 * 0x03);
                    //}

                    Thread.Sleep(0x64);
                }
            }
        }

        private void __thdmtd_workingoncloud(object dev)
        {
            androiddevice __device = dev as androiddevice;

            if(null != __device)
                while (__status) {
                    #region when same phone worked 3 times or first run, request new phone to jiema platform and resign 
                    bool __resignneeded = false;
                    if (true
                        && (string.IsNullOrEmpty(__device.workphonenum) || __device.samenumcount >= 0x03)
                        ) {
                        string __newphone = string.Empty;
                        while (string.IsNullOrEmpty(__newphone)) {
                            if (__con_phonenumsrepository.TryDequeue(out __newphone)) {
                                __device.workphonenum = __newphone;
                                __writelog($">device1 changing to new work phone[{__newphone}]...", ConsoleColor.Yellow);
                                break;
                            } else {
                                __writelog($">phone number not enough in local stock repository", ConsoleColor.Red);
                                Thread.Sleep(0x3e8 * 0x0a);
                            }
                        }

                        string __newserialno = string.Empty;
                        if (__device.lastphone != __device.workphonenum)
                        {
                            int __jiemanewphonefailcounter = 0x00;
                            while (string.IsNullOrEmpty(__newserialno))
                            {
                                __newserialno = __platformjiema_requestnewphone(__newphone);
                                if (!string.IsNullOrEmpty(__newserialno))
                                {
                                    __writelog($"device{__device.id} work phone number '{__device.workphonenum}' changed to '{__newphone}'", ConsoleColor.Cyan);
                                    __device.workserialno = __newserialno;
                                    __device.samenumcount = 0x00;
                                    break;
                                }
                                else
                                {
                                    __jiemanewphonefailcounter++;
                                    __writelog($">request to interface 'receiveCode' from platform jiema failed,retry later...", ConsoleColor.Red);
                                    Thread.Sleep(0x3e8 * 0x0a);
                                }
                                if (__jiemanewphonefailcounter > 0x05)
                                {
                                    __device.samenumcount = 0x03;
                                    break;
                                }
                            }
                            if (__device.samenumcount >= 0x03) {
                                continue;
                            }
                        }

                        __resignneeded = true;
                    }
                    #endregion

                    #region run real workflow

                    if(true
                        && __resignneeded) {
                        __chinac_startcmcc(__device);
                        Thread.Sleep(0x0BB8);
                        __adb_startsignin(__device);
                    }
                    else
                    {

                    }

                    #endregion
                }
        }

        private bool __appium_startcmcc(androiddevice device)
        {
            bool __result = false;
            var __driveropts = new AppiumOptions()
            {
                AutomationName = AutomationName.AndroidUIAutomator2,
                PlatformName = "Android",
                DeviceName = device.id,
            };
            __driveropts.AddAdditionalAppiumOption("appPackage", confs.settings.const_android_packages_cmcc);
            __driveropts.AddAdditionalAppiumOption("appActivity", confs.settings.const_android_activity_cmcc);
            __driveropts.AddAdditionalAppiumOption("noReset", true);

            try
            {
                device.driver = new OpenQA.Selenium.Appium.Android.AndroidDriver(new Uri(
                    $"http://{confs.settings.const_android_appiumserver_address}:{confs.settings.const_android_appiumserver_port}{confs.settings.const_android_appiumserver_sessionpath}"),
                    __driveropts, TimeSpan.FromSeconds(0x64)
                    );
                device.driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0x0a);
                __result = true;
            }
            catch(Exception ex) {
                __writelog($">local appium server error: {ex.Message}", ConsoleColor.Red);
            }

            return __result;
        }

        private bool __chinac_startcmcc(androiddevice device) {
            bool __result = false;
            ChinacSDKCore.NET.ChinacCloudMobileClient __ccmclient =
                new ChinacSDKCore.NET.ChinacCloudMobileClient(confs.settings.chinac.apikeyid, confs.settings.chinac.apikeysecret);

            ChinacSDKCore.NET.Models.GetCmdApiSignatureRequest __request_getcmdapisignature =
                new ChinacSDKCore.NET.Models.GetCmdApiSignatureRequest() { 
                    Ids = new List<string>() { device.cloudid },
                    Region = device.cloudregion
                };
            ChinacSDKCore.NET.Models.GetCmdApiSignatureResponse __response_getcmdapisignature =
                __ccmclient.GetCmdApiSignature(__request_getcmdapisignature);

            if (null != __response_getcmdapisignature
                && __response_getcmdapisignature.code == 10000) {

                ChinacSDKCore.NET.Models.StartAppRequest __request_startapp =
                    new ChinacSDKCore.NET.Models.StartAppRequest()
                    {
                        RToken = __response_getcmdapisignature.data.RToken,
                        Id = device.cloudid,
                        PackageName = confs.settings.const_android_packages_cmcc,
                    };
                ChinacSDKCore.NET.Models.StartAppResponse __response_startapp =
                    __ccmclient.StartApp(__request_startapp, __response_getcmdapisignature.data.ApiUrl, __response_getcmdapisignature.data.Signature);

                if (null != __response_startapp && __response_startapp.ResponseCode == 100000)
                    __result = true;
            }

            return __result;
        }

        private bool __adb_startsignin(androiddevice device)
        {
            bool __flowsuccessed = false;

            __writelog($">device[{device.id}] start chinac cloud phone signin processing...", ConsoleColor.Green);

            AdvancedSharpAdbClient.DeviceCommands.Models.Element __foundelement = null;

            DeviceClient __devclient = device.adbclient.CreateDeviceClient(device.adbdevicedata);
            string __xml = __devclient.DumpScreenString();

            if(__adb_waitforelement(device,
                "//node[@content-desc=\"账号切换\" and @class=\"android.widget.LinearLayout\"]",
                0x05, out __foundelement)) 
            {

                #region run on main pad
                if (__adb_waitforelement(device,
                    "//node[@text=\"切换\" and @resource-id=\"com.greenpoint.android.mc10086.activity:id/tv_title_account_change\"]",
                    0x03,out __foundelement))
                {
                    __foundelement.Click();
                    Thread.Sleep(0x03e8 * 0x03);

                    string __curphoneno = string.Empty;
                    if(__adb_waitforelement(device,
                        "//node[@resource-id=\"com.greenpoint.android.mc10086.activity:id/tv_current_phone_number\"]",
                        0x03, out __foundelement))
                        __curphoneno = Regex.Replace(__foundelement.Text, "当前号码：", string.Empty, RegexOptions.None);

                    if(__adb_waitforelement(device,
                        "//node[@text=\"其他账号登录\" and @class=\"android.widget.TextView\"]",
                        0x03,out __foundelement))
                    {
                        __foundelement.Click();
                        Thread.Sleep(0x03e8 * 0x05);
                    }
                }
                else if (__adb_waitforelement(device,
                    "//node[@text=\"登录\" and @class=\"android.widget.TextView\"]",
                    0x03, out __foundelement))
                {
                    __foundelement.Click();
                    Thread.Sleep(0x03e8 * 0x05);
                }
                #endregion

                #region run turn to login pad

                AdvancedSharpAdbClient.DeviceCommands.Models.Element __anchorelement = null;
                if(__adb_waitforelement(device,
                    "//node[@text=\"欢迎登录中国移动\"]",
                    0x05, out __anchorelement))
                {
                    if(__adb_waitforelement(device,
                        "//node[@resource-id=\"com.greenpoint.android.mc10086.activity:id/user_phoneno_edt\"]",
                        0x03, out __foundelement))
                    {
                        string __runshell = $"input tap {((__foundelement.Bounds.Left + __foundelement.Bounds.Right) / 0x02)} {((__foundelement.Bounds.Top + __foundelement.Bounds.Bottom) / 0x02)}";
                        device.adbclient.ExecuteShellCommand(device.adbdevicedata, __runshell);
                        Thread.Sleep(0x03e8);
                        device.adbclient.ClearInput(device.adbdevicedata, 0x0b);
                        Thread.Sleep(0x03e8 * 0x03);
                        device.adbclient.SendText(device.adbdevicedata, device.workphonenum);
                        Thread.Sleep(0x03e8 * 0x03);
                        __runshell = $"input tap {((__anchorelement.Bounds.Left + __anchorelement.Bounds.Right) / 0x02)} {((__anchorelement.Bounds.Top + __anchorelement.Bounds.Bottom) / 0x02)}";
                        device.adbclient.ExecuteShellCommand(device.adbdevicedata, __runshell);
                        Thread.Sleep(0x03e8 * 0x03);

                        if(__adb_waitforelement(device,
                            "//node[@resource-id=\"com.greenpoint.android.mc10086.activity:id/iv_service_protocol_check\"]",
                            0x03,out __foundelement))
                        {
                            __foundelement.Click();
                            Thread.Sleep(0x03e8 * 0x03);

                            if (__adb_waitforelement(device,
                                "//node[@resource-id=\"com.greenpoint.android.mc10086.activity:id/login_checksms_btn\" and @class=\"android.widget.Button\"]",
                                0x03, out __foundelement))
                            {
                                __foundelement.Click();
                                Thread.Sleep(0x03e8);


                                string __vcode = __platformjiema_requestquery(device.workserialno);
                                if (string.IsNullOrEmpty(__vcode))
                                {
                                    device.samenumcount = 0x03;
                                    __writelog($">device[{device.id}] workphonenum[{device.workphonenum}] get vcode timeout, roll next workphonenum.");
                                    return false;
                                }

                                //...
                                if(__adb_waitforelement(device,
                                    "//node[@resource-id=\"com.greenpoint.android.mc10086.activity:id/user_login_smspassword_edt\"]",
                                    0x03, out __foundelement))
                                {
                                    __foundelement.SendText(__vcode);
                                    Thread.Sleep(0x03e8 * 0x03);

                                    if (__adb_waitforelement(device,
                                        "//node[@content-desc=\"登录\" and @class=\"android.widget.Button\"]",
                                        0x03, out __foundelement))
                                    {
                                        __foundelement.Click();
                                        Thread.Sleep(0x03e8 * 0x05);

                                        __flowsuccessed = true;
                                    }
                                }

                            }
                        }



                    }
                }


                #endregion
            }

            #region unexcept actions
            


            else if (__adb_waitforelement(device,
                "//node[@content-desc=\"查阅并同意完整的《服务协议》，《儿童隐私保护须知》及《隐私政策》\"]",
                0x03, out __foundelement))
            {
                __writelog($">device[{device.id}] found unexcept content: cmcc argeeuments action.", ConsoleColor.Yellow);
                __foundelement.Click();
                Thread.Sleep(0x03e8);
                if(__adb_waitforelement(device,
                    "//node[@text=\"同意\" and @class=\"android.widget.Button\"]",
                    0x03, out __foundelement))
                {
                    __foundelement.Click();
                    Thread.Sleep(0x03e8 * 0x03);
                }
            }
            
            if(__adb_waitforelement(device,
                "//node[@text=\"授权提醒\"]",
                0x03, out __foundelement))
            {
                __writelog($">device[{device.id}] found unexcept content: cmcc authorize action.", ConsoleColor.Yellow);
                if (__adb_waitforelement(device,
                    "//node[@text=\"确认\" and @class=\"android.widget.TextView\"]",
                    0x03, out __foundelement))
                {
                    __foundelement.Click();
                    Thread.Sleep(0x03e8 * 0x03);
                }
            }
            
            if(__adb_waitforelement(device,
                "//node[@text=\"“中国移动”将获取您的粘贴内容，用于分享、识别，您允许这样做吗？\"]",
                0x03, out __foundelement))
            {
                __writelog($">device[{device.id}] found unexcept content: cmcc want manage clipboard.", ConsoleColor.Yellow);
                if(__adb_waitforelement(device,
                    "//node[@text=\"允许\" and @class=\"android.widget.Button\"]",
                    0x03, out __foundelement))
                {
                    __foundelement.Click();
                    Thread.Sleep(0x03e8 * 0x03);
                }
            }
            #region unhandler except actions
            else
            {
                __writelog($">device[{device.id}] found unexcept content: cmcc want manage clipboard.", ConsoleColor.Red);
            }
            #endregion

            #endregion

            return __flowsuccessed;
        }

        private bool __chinac_stopcmcc(androiddevice device)
        {
            bool __result = false;
            ChinacSDKCore.NET.ChinacCloudMobileClient __ccmclient =
                new ChinacSDKCore.NET.ChinacCloudMobileClient(confs.settings.chinac.apikeyid, confs.settings.chinac.apikeysecret);

            ChinacSDKCore.NET.Models.GetCmdApiSignatureRequest __request_getcmdapisignature =
                new ChinacSDKCore.NET.Models.GetCmdApiSignatureRequest()
                {
                    Ids = new List<string>() { device.cloudid },
                    Region = device.cloudregion
                };
            ChinacSDKCore.NET.Models.GetCmdApiSignatureResponse __response_getcmdapisignature =
                __ccmclient.GetCmdApiSignature(__request_getcmdapisignature);

            if(null != __response_getcmdapisignature
                && __response_getcmdapisignature.code == 10000) {

                ChinacSDKCore.NET.Models.StopAppRequest __request_stopapp =
                    new ChinacSDKCore.NET.Models.StopAppRequest() { 
                        RToken = __response_getcmdapisignature.data.RToken,
                        Id = device.cloudid,
                        PackageName = confs.settings.const_android_packages_cmcc
                    };
                ChinacSDKCore.NET.Models.StopAppResponse __response_stopapp =
                    __ccmclient.StopApp(__request_stopapp, __response_getcmdapisignature.data.ApiUrl, __response_getcmdapisignature.data.Signature);

                if (null != __response_stopapp && __response_stopapp.ResponseCode == 100000)
                    __result = true;

            }

            return __result;
        }

        private bool __proc_rollnewphonenum(androiddevice device)
        {
            bool __result = false; if (true
                        && (string.IsNullOrEmpty(device.workphonenum) || device.samenumcount >= 0x03)
                        )
            {
                __goto_renewphone:

                string __newphone = string.Empty;
                while (string.IsNullOrEmpty(__newphone))
                {
                    if (__con_phonenumsrepository.TryDequeue(out __newphone))
                    {
                        device.workphonenum = __newphone;
                        __writelog($">device1 changing to new work phone[{__newphone}]...", ConsoleColor.Yellow);
                        break;
                    }
                    else
                    {
                        __writelog($">phone number not enough in local stock repository", ConsoleColor.Red);
                        Thread.Sleep(0x3e8 * 0x0a);
                    }
                }

                string __newserialno = string.Empty;
                if (device.lastphone != device.workphonenum)
                {
                    int __failcounter = 0x00;
                    while (string.IsNullOrEmpty(__newserialno))
                    {
                        __newserialno = __platformjiema_requestnewphone(__newphone);
                        if (!string.IsNullOrEmpty(__newserialno))
                        {
                            __writelog($"device{device.id} work phone number '{device.workphonenum}' changed to '{__newphone}'", ConsoleColor.Cyan);

                            device.workserialno = __newserialno;
                            device.samenumcount = 0x00;

                            break;
                        }
                        else
                        {
                            __failcounter++;
                            __writelog($">request to interface 'receiveCode' from platform jiema failed,retry later...", ConsoleColor.Red);
                            Thread.Sleep(0x3e8 * 0x0a);
                        }
                        if (__failcounter >= 0x05)
                        {
                            device.samenumcount = 0x03;
                            break;
                        }
                    }
                    if (device.samenumcount >= 0x03)
                    {
                        __writelog($">device[{device.id}] workphonenum[{device.workphonenum}] request to interface 'receiveCode' failed,roll next new phonenum...", ConsoleColor.Red);
                        goto __goto_renewphone;
                    }
                }

                __result = true;
            }
            return __result;
        }

        private bool __appium_startsignin(androiddevice device)
        {
            bool __result = false;
            Point __tapcoord = Point.Empty;

            __writelog($">device[{device.id}] start signin processing...", ConsoleColor.Green);

            AppiumElement __foundtarget = null;
            Random __rndengine = new Random(DateTime.Now.Millisecond);

            Dictionary<string, bool> __unexact_tokens = new Dictionary<string, bool>() {
                { "agreementpad", false },
                { "authorizepad", false },
                { "clipboardpad", false },
                { "qiuhaopingpad", false },
                { "adspad", false },
            };

            while (true)
            {
                if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                {
                    device.samenumcount = 0x03;
                    return false;
                }

                if (
                    ((__unexact_tokens["agreementpad"] && __unexact_tokens["authorizepad"] && __unexact_tokens["clipboardpad"]) ||
                    (!__unexact_tokens["agreementpad"] && !__unexact_tokens["authorizepad"] && !__unexact_tokens["clipboardpad"])) &&
                    __appium_waitforelement(device.driver,
                        "//android.widget.TextView[@resource-id=\"com.greenpoint.android.mc10086.activity:id/tv_title_login\"]",
                        0x02, out __foundtarget))
                {
                    if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                    {
                        device.samenumcount = 0x03;
                        return false;
                    }
                    try { __foundtarget.Click(); } catch { continue; }
                    Thread.Sleep(0x3e8 * 0x03 + __rndengine.Next(0x00, 0x03e8 * 0x01));
                    break;
                }
                else if (
                    ((__unexact_tokens["agreementpad"] && __unexact_tokens["authorizepad"] && __unexact_tokens["clipboardpad"]) ||
                    (!__unexact_tokens["agreementpad"] && !__unexact_tokens["authorizepad"] && !__unexact_tokens["clipboardpad"])) &&
                    __appium_waitforelement(device.driver,
                        "//android.widget.LinearLayout[@content-desc=\"账号切换\"]",
                        0x02, out __foundtarget) || 
                    __appium_waitforelement(device.driver,
                        "//android.widget.TextView[@resource-id=\"com.greenpoint.android.mc10086.activity:id/tv_title_account_change\"]",
                        0x02, out __foundtarget))
                {
                    if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                    {
                        device.samenumcount = 0x03;
                        return false;
                    }
                    AppiumElement __tempelement = null;
                    if(
                        //__appium_waitforelement(device.driver,
                        //"//android.widget.TextView[@resource-id=\"com.greenpoint.android.mc10086.activity:id/tv_current_phone_number\"]",
                        //0x03, out __tempelement) ||
                        __appium_waitforelement(device.driver,
                            "//android.widget.TextView[@resource-id=\"com.greenpoint.android.mc10086.activity:id/tv_title_account\"]",
                            0x03, out __tempelement))
                    {
                        if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                        {
                            device.samenumcount = 0x03;
                            return false;
                        }
                        string __currentphonenum = __tempelement.Text;
                        string[] __currentphonedat = Regex.Split(__currentphonenum, "\\*\\*\\*\\*", RegexOptions.None);
                        if(__currentphonedat.Length>=0x02 &&
                            device.workphonenum.StartsWith(__currentphonedat[0x00]) &&
                            device.workphonenum.EndsWith(__currentphonedat[0x01]))
                        {
                            return true;
                        }
                    }

                    try { __foundtarget.Click(); } catch { continue; }
                    Thread.Sleep(0x03e8 * 0x02 + __rndengine.Next(0x00, 0x03e8 * 0x01));

                    if(__appium_waitforelement(device.driver,
                        "//android.widget.RelativeLayout[@resource-id=\"com.greenpoint.android.mc10086.activity:id/other_account_login\"]",
                        0x14, out __foundtarget))
                    {
                        if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                        {
                            device.samenumcount = 0x03;
                            return false;
                        }
                        try { __foundtarget.Click(); } catch { continue; }
                        Thread.Sleep(0x03e8 * 0x03 + __rndengine.Next(0x00, 0x03e8 * 0x01));
                        break;
                    }
                }
                else
                {
                    #region unexcept actions

                    if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                    {
                        device.samenumcount = 0x03;
                        return false;
                    }
                    if (!__unexact_tokens["agreementpad"] &&
                        __appium_waitforelement(device.driver,
                        "//android.widget.CheckBox[@content-desc=\"查阅并同意完整的《服务协议》，《儿童隐私保护须知》及《隐私政策》\"]",
                        0x02, out __foundtarget))
                    {
                        if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                        {
                            device.samenumcount = 0x03;
                            return false;
                        }
                        __writelog($">device[{device.id}] found cmcc agreements pad, process it.", ConsoleColor.Yellow);
                        __foundtarget.Click();
                        Thread.Sleep(0x03e8 * 0x02 + __rndengine.Next(0x00, 0x03e8 * 0x01));
                        if(__appium_waitforelement(device.driver,
                            "//android.widget.Button[@resource-id=\"com.greenpoint.android.mc10086.activity:id/btn_agree\"]",
                            0x03, out __foundtarget))
                        {
                            if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                            {
                                device.samenumcount = 0x03;
                                return false;
                            }
                            __foundtarget.Click();
                            __writelog($">device[{device.id}] found cmcc agreements pad, processed.", ConsoleColor.Green);
                            Thread.Sleep(0x03e8 * 0x02 + __rndengine.Next(0x00, 0x03e8 * 0x01));
                            __unexact_tokens["agreementpad"] = true;
                            continue;
                        }
                    }



                    if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                    {
                        device.samenumcount = 0x03;
                        return false;
                    }
                    if (!__unexact_tokens["authorizepad"] && 
                        __appium_waitforelement(device.driver,
                        "//android.widget.TextView[@resource-id=\"com.greenpoint.android.mc10086.activity:id/dialog_title\"]",
                        0x02, out __foundtarget))
                    {
                        if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                        {
                            device.samenumcount = 0x03;
                            return false;
                        }
                        __writelog($">device[{device.id}] found cmcc authorize pad, process it.", ConsoleColor.Yellow);
                        if (__appium_waitforelement(device.driver,
                            "//android.widget.TextView[@resource-id=\"com.greenpoint.android.mc10086.activity:id/right_btn\"]",
                            0x03, out __foundtarget))
                        {
                            if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                            {
                                device.samenumcount = 0x03;
                                return false;
                            }
                            __foundtarget.Click();
                            __writelog($">device[{device.id}] found cmcc authorize pad, processed.", ConsoleColor.Green);
                            Thread.Sleep(0x03e8 * 0x02 + __rndengine.Next(0x00, 0x03e8 * 0x01));
                            __unexact_tokens["authorizepad"] = true;
                            continue;
                        }
                    }


                    if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                    {
                        device.samenumcount = 0x03;
                        return false;
                    }
                    if (!__unexact_tokens["clipboardpad"] &&
                        __appium_waitforelement(device.driver,
                        "//android.widget.TextView[@text=\"“中国移动”将获取您的粘贴内容，用于分享、识别，您允许这样做吗？\"]",
                        0x02, out __foundtarget))
                    {
                        if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                        {
                            device.samenumcount = 0x03;
                            return false;
                        }
                        __writelog($">device[{device.id}] found cmcc clipboard request, processed.", ConsoleColor.Green);
                        if (__appium_waitforelement(device.driver,
                            "//android.widget.Button[@resource-id=\"com.greenpoint.android.mc10086.activity:id/dialog_btn2\"]",
                            0x02, out __foundtarget))
                        {
                            if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                            {
                                device.samenumcount = 0x03;
                                return false;
                            }
                            __foundtarget.Click();
                            __writelog($">device[{device.id}] found cmcc clipboard request, processed.", ConsoleColor.Green);
                            Thread.Sleep(0x03e8 * 0x02 + __rndengine.Next(0x00, 0x03e8 * 0x01));
                            __unexact_tokens["clipboardpad"] = true;
                            continue;
                        }
                    }


                    if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                    {
                        device.samenumcount = 0x03;
                        return false;
                    }
                    if (!__unexact_tokens["qiuhaopingpad"] &&
                        __appium_waitforelement(device.driver,
                        "//android.widget.TextView[@text=\"我们的进步离不开您的支持与鼓励!\"]",
                        0x02, out __foundtarget))
                    {
                        if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                        {
                            device.samenumcount = 0x03;
                            return false;
                        }
                        __writelog($">device[{device.id}] found cmcc qiuhaoping pad, process it.", ConsoleColor.Yellow);
                        if (__appium_waitforelement(device.driver,
                            "//android.widget.Button[@content-desc=\"关闭\"]",
                            0x02, out __foundtarget))
                        {
                            if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                            {
                                device.samenumcount = 0x03;
                                return false;
                            }
                            __foundtarget.Click();
                            __writelog($">device[{device.id}] found cmcc qiuhaoping pad, processed.", ConsoleColor.Green);
                            Thread.Sleep(0x03e8 * 0x02 + __rndengine.Next(0x00, 0x03e8 * 0x01));
                            __unexact_tokens["qiuhaopingpad"] = true;
                            continue;
                        }
                    }

                    if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                    {
                        device.samenumcount = 0x03;
                        return false;
                    }
                    if (!__unexact_tokens["adspad"] &&
                        __appium_waitforelement(device.driver,
                        "//android.widget.ImageView[@content-desc=\"关闭\"]",
                        0x02, out __foundtarget))
                    {
                        if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                        {
                            device.samenumcount = 0x03;
                            return false;
                        }
                        __writelog($">device[{device.id}] found ads content: cmcc month no recharge alert action.", ConsoleColor.Yellow);
                        __foundtarget.Click();
                        Thread.Sleep(0x03e8 * 0x02 + __rndengine.Next(0x00, 0x03e8 * 0x01));
                        __unexact_tokens["adspad"] = true;
                        continue;
                    }

                    if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                    {
                        device.samenumcount = 0x03;
                        return false;
                    }
                    if (__appium_waitforelement(device.driver,
                        "//android.widget.Button[@text=\"暂不更新\"]",
                        0x02, out __foundtarget))
                    {
                        if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                        {
                            device.samenumcount = 0x03;
                            return false;
                        }
                        __writelog($">device[{device.id}] found ads content: cmcc update alert action.", ConsoleColor.Yellow);
                        __foundtarget.Click();
                        Thread.Sleep(0x03e8 * 0x02);
                    }


                    else
                    {
                        __writelog($">device[{device.id}] found unhandled cmcc unexcept action, please check and fix it.", ConsoleColor.Red);
                    }

                    #endregion
                }
                Thread.Sleep(0x03e8 * 0x02);
            }

            #region cmcc app security page
            Thread.Sleep(0x03e8 * 0x03);

            if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground) {
                device.samenumcount = 0x03;
                return false;
            }

            __tapcoord = __randompoint(340, 415, 5, 5);
            __writelog($">debuginfo1: {__tapcoord.X} {__tapcoord.Y}");
            device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input tap {__tapcoord.X} {__tapcoord.Y}");
            Thread.Sleep(0x3e8 * 0x02 + __rndengine.Next(0x00, 0x03e8 * 0x01));
            __tapcoord = __randompoint(340, 415, 5, 5);
            __writelog($">debuginfo1: {__tapcoord.X} {__tapcoord.Y}");
            device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input tap {__tapcoord.X} {__tapcoord.Y}");
            Thread.Sleep(0x3e8 * 0x02 + __rndengine.Next(0x00, 0x03e8 * 0x01));

            if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
            {
                device.samenumcount = 0x03;
                return false;
            }

            device.adbclient.ClearInput(device.adbdevicedata, 0x14);
            Thread.Sleep(0x3e8 * 0x02 + __rndengine.Next(0x00, 0x03e8 * 0x01));


            //char[] __phonenums = device.workphonenum.ToCharArray();
            //foreach(var __num in __phonenums)
            //{
            //    #region tap keyboard to input phonenum
            //    if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
            //    {
            //        device.samenumcount = 0x03;
            //        return false;
            //    }
            //    __tap_originalkeyboard_num(device, __num, __rndengine.Next(0x01f4, 0x03e8 * 0x01));
            //    #endregion
            //}
            if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
            {
                device.samenumcount = 0x03;
                return false;
            }
            device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input text {device.workphonenum}");
            Thread.Sleep(0x03e8 * 0x03 + __rndengine.Next(0x00, 0x03e8 * 0x01));

            if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
            {
                device.samenumcount = 0x03;
                return false;
            }
            device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input tap 655 810");
            Thread.Sleep(0x03e8 * 0x03 + __rndengine.Next(0x00, 0x03e8 * 0x01));

            //if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
            //{
            //    device.samenumcount = 0x03;
            //    return false;
            //}

            if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
            {
                device.samenumcount = 0x03;
                return false;
            }
            device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input keyevent 61");
            Thread.Sleep(0x3e8 * 0x02 + __rndengine.Next(0x00, 0x03e8 * 0x01));

            if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
            {
                device.samenumcount = 0x03;
                return false;
            }

            __tapcoord = __randompoint(80, 620, 5, 5);
            device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input tap {__tapcoord.X} {__tapcoord.Y}");
            Thread.Sleep(0x3e8 * 0x02 + __rndengine.Next(0x00, 0x03e8 * 0x01));

            if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
            {
                device.samenumcount = 0x03;
                return false;
            }

            __tapcoord = __randompoint(610, 525, 5, 5);
            device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input tap {__tapcoord.X} {__tapcoord.Y}");
            Thread.Sleep(0x3e8 * 0x02 + __rndengine.Next(0x00, 0x03e8 * 0x01));

            if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
            {
                device.samenumcount = 0x03;
                return false;
            }

            string __vcode = __platformjiema_requestquery(device.workserialno);
            if (string.IsNullOrEmpty(__vcode))
            {
                __writelog($">device[{device.id}] workphonenum[{device.workphonenum}] get verifycode timeout,roll next new workphonenum.", 
                    ConsoleColor.Red);
                device.samenumcount = 0x03;
                return false;
            }

            if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
            {
                device.samenumcount = 0x03;
                return false;
            }

            __tapcoord = __randompoint(200, 500, 5, 5);
            device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input tap {__tapcoord.X} {__tapcoord.Y}");
            Thread.Sleep(0x3e8 * 0x02 + __rndengine.Next(0x00, 0x03e8 * 0x01));

            if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
            {
                device.samenumcount = 0x03;
                return false;
            }

            device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input text {__vcode}");
            Thread.Sleep(0x03e8 * 0x05 + __rndengine.Next(0x00, 0x03e8 * 0x01));

            if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
            {
                device.samenumcount = 0x03;
                return false;
            }

            __tapcoord = __randompoint(350, 750, 5, 5);
            device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input tap {__tapcoord.X} {__tapcoord.Y}");
            Thread.Sleep(0x3e8 * 0x08 + __rndengine.Next(0x00, 0x03e8 * 0x01));

            if(__appium_waitforelement(device.driver,
                "//android.widget.TextView[@resource-id=\"com.greenpoint.android.mc10086.activity:id/dialog_limit_count_des\"]",
                0x05, out __foundtarget))
            {
                device.isdevicelimited = true;
            }

            #endregion

            #region write deviceid.last

            string __devidlastfile = Path.Combine(
                Path.GetDirectoryName(typeof(ServCore).Assembly.Location), $"confs/{device.id.Replace(":", "-")}.last");
            if (File.Exists(__devidlastfile)) File.Delete(__devidlastfile);
            File.WriteAllText(__devidlastfile, device.workphonenum);

            device.needjumpguide = __result = true;

            #endregion

            __writelog($">device[{device.id}] resignin process finished.", ConsoleColor.Green);

            return __result;
        }

        private bool __appium_tasking(androiddevice device)
        {
            bool __result = false;
            AppiumElement __foundtarget = null;
            Point __tapcoord = Point.Empty;
            Random __rndengine = new Random(DateTime.Now.Millisecond);
            int __busyretrycounter = 0x00;

            __writelog($">device[{device.id}] is tasking...", ConsoleColor.Cyan);
            Thread.Sleep(0x03e8 * 0x05);

            if (device.needjumpguide)
            {
                __writelog($">device[{device.id}] is try to clean guide pad...", ConsoleColor.Cyan);

                __tapcoord = __randompoint(60, 1220, 5, 5);
                device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input tap {__tapcoord.X} {__tapcoord.Y}");
                Thread.Sleep(0x03e8 * 0x03 + __rndengine.Next(0x00, 0x03e8 * 0x01));
                __tapcoord = __randompoint(60, 1220, 5, 5);
                device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input tap {__tapcoord.X} {__tapcoord.Y}");
                Thread.Sleep(0x03e8 * 0x03 + __rndengine.Next(0x00, 0x03e8 * 0x01));
                __tapcoord = __randompoint(60, 1220, 5, 5);
                device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input tap {__tapcoord.X} {__tapcoord.Y}");
                Thread.Sleep(0x03e8 * 0x03 + __rndengine.Next(0x00, 0x03e8 * 0x01));
                __tapcoord = __randompoint(60, 1220, 5, 5);
                device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input tap {__tapcoord.X} {__tapcoord.Y}");

                device.needjumpguide = false;

                Thread.Sleep(0x03e8 * 0x05 + __rndengine.Next(0x00, 0x03e8 * 0x03));
            }

        __goto_retasking:

            if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
            {
                device.istaskflowinterrupted = true;
                return false;
            }

            if (__appium_waitforelement(device.driver,
                "//android.widget.Button[@text=\"充值·缴费\"]",
                0x08, out __foundtarget))
            {
                if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground) {
                    device.istaskflowinterrupted = true;
                    return false;
                }
                try { __foundtarget.Click(); } catch { return false; }
                Thread.Sleep(0x3e8 * 0x03 + __rndengine.Next(0x00, 0x03e8 * 0x02));

                #region input recharge phone number
                if (__appium_waitforelement(device.driver,
                    "//android.widget.TextView[@text=\"尊敬的用户您好，当前充值服务繁忙，后台正在为您努力重试中，您可手动返回或重新登录APP重试！\"]",
                    0x03, out __foundtarget))
                {
                    if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                    {
                        device.istaskflowinterrupted = true;
                        return false;
                    }
                    if (__appium_waitforelement(device.driver,
                        "//android.widget.TextView[@resource-id=\"btnOuter\"]",
                        0x03, out __foundtarget))
                    {
                        if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                        {
                            device.istaskflowinterrupted = true;
                            return false;
                        }
                        __foundtarget.Click();
                        __busyretrycounter++;
                        Thread.Sleep(0x03e8 * 0x02 + __rndengine.Next(0x00, 0x03e8 * __busyretrycounter));
                        goto __goto_retasking;
                    }
                }
                else if (__appium_waitforelement(device.driver,
                    "//android.widget.TextView[@text=\"您当月给他人充值次数已满，请次月再试\"]",
                    0x03, out __foundtarget)) {
                    device.samenumcount = 0x03;
                    device.istaskflowinterrupted = true;
                    return false;
                }
                else if (__appium_waitforelement(device.driver,
                    "//android.webkit.WebView[@text=\"充值中心\"]/android.view.View/android.view.View[1]/android.view.View[1]/android.view.View[1]/android.widget.TextView[1]",
                    0x03, out __foundtarget))
                {
                    if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                    {
                        device.istaskflowinterrupted = true;
                        return false;
                    }
                    try { __foundtarget.Click(); } catch { return false; }
                    Thread.Sleep(0x03e8* 0x02 + __rndengine.Next(0x00, 0x03e8 * 0x02));
                    if (__appium_waitforelement(device.driver,
                        "//android.webkit.WebView[@text=\"充值中心\"]/android.view.View/android.view.View[1]/android.view.View[1]/android.view.View[1]/android.widget.TextView[1]",
                        0x03, out __foundtarget))
                    {
                        if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                        {
                            device.istaskflowinterrupted = true;
                            return false;
                        }
                        __foundtarget.Click();
                        Thread.Sleep(0x03e8* 0x02 + __rndengine.Next(0x00, 0x03e8 * 0x02));
                        if (__appium_waitforelement(device.driver,
                            "//android.widget.EditText", 0x05, out __foundtarget))
                        {
                            if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                            {
                                device.istaskflowinterrupted = true;
                                return false;
                            }
                            device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input text {device.currenttask.phonenum}");
                            //var __nums = device.currenttask.phonenum.ToCharArray();
                            //foreach(var __num in __nums)
                            //{
                            //    __tap_originalkeyboard_num(device, __num, __rndengine.Next(0x01f4, 0x03e8));
                            //}
                            //__foundtarget.SendKeys(device.currenttask.phonenum);
                            Thread.Sleep(0x03e8 * 0x02 + __rndengine.Next(0x00, 0x03e8 * 0x03));
                            //device.adbclient.ExecuteShellCommand(device.adbdevicedata, "input keyevent 61");
                            //Thread.Sleep(0x03e8);
                        }
                    }
                }
                #endregion

                #region chose the recharge amount
                string __rechargebtnindex =
                    device.currenttask.amount == 0x0a ? "1" :
                    device.currenttask.amount == 0x14 ? "2" :
                    device.currenttask.amount == 0x1e ? "3" :
                    device.currenttask.amount == 0x32 ? "4" :
                    device.currenttask.amount == 0x64 ? "5" :
                    device.currenttask.amount == 0xc8 ? "6" : "7";
                if(__appium_waitforelement(device.driver,
                    $"//android.webkit.WebView[@text=\"充值中心\"]/android.view.View/android.view.View[1]/android.view.View[3]/android.view.View[{__rechargebtnindex}]",
                    0x03, out __foundtarget))
                {
                    if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                    {
                        device.istaskflowinterrupted = true;
                        return false;
                    }
                    __foundtarget.Click();
                    Thread.Sleep(0x3e8 * 0x02 + __rndengine.Next(0x00, 0x03e8 * 0x01));
                    if (__rechargebtnindex == "7")
                    {
                        if(__appium_waitforelement(device.driver,
                            "//android.widget.TextView[@text=\"自定义\"]/..",
                            0x03, out __foundtarget))
                        {
                            if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                            {
                                device.istaskflowinterrupted = true;
                                return false;
                            }
                            //__foundtarget.Click();
                            __tapcoord = __randompoint(135, 860, 5, 5);
                            device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input tap {__tapcoord.X} {__tapcoord.Y}");
                            Thread.Sleep(0x03e8 * 0x02 + __rndengine.Next(0x00, 0x03e8 * 0x01));
                            device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input text {device.currenttask.amount.ToString()}");

                            //var __amnums = device.currenttask.amount.ToString().Trim().ToCharArray();
                            //foreach (var __num in __amnums)
                            //{
                            //    __tap_originalkeyboard_num(device, __num, __rndengine.Next(0x01f4, 0x03e8));
                            //}

                            //__foundtarget.SendKeys(device.currenttask.amount.ToString());
                            Thread.Sleep(0x03e8 * 0x02 + __rndengine.Next(0x00, 0x03e8 * 0x02));
                            //__tapcoord = __randompoint(700, 300, 0, 5);
                            //device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input tap {__tapcoord.X} {__tapcoord.Y}");
                            //Thread.Sleep(0x03e8 * 0x02 + __rndengine.Next(0x00, 0x03e8 * 0x03));

                            if(__appium_waitforelement(device.driver,
                                "//android.widget.TextView[@text=\"立即支付\"]/../..",
                                0x03, out __foundtarget))
                            {
                                if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                                {
                                    device.istaskflowinterrupted = true;
                                    return false;
                                }
                                __foundtarget.Click();
                                Thread.Sleep(0x03e8 * 0x02 + __rndengine.Next(0x00, 0x03e8 * 0x03));

                                
                            }
                        }
                    }
                }
                #endregion

                #region click recharge button

                //if(__appium_waitforelement(device.driver,
                //    "//android.webkit.WebView[@text=\"充值中心\"]/android.view.View/android.view.View[1]/android.view.View[7]/android.view.View[2]/android.widget.TextView",
                //    0x03, out __foundtarget)) {
                //    __foundtarget.Click();
                //    Thread.Sleep(0x03e8 * 0x03);
                //    if(__appium_waitforelement(device.driver,
                //        "//android.widget.TextView[@text=\"确认\"]",
                //        0x05, out __foundtarget))
                //    {
                //        __foundtarget.Click();
                //        Thread.Sleep(0x03e8 * 0x03);
                //    }
                //}
                if (__appium_waitforelement(device.driver,
                                    "//android.widget.TextView[@text=\"确认被充值号码\"]",
                                    0x03, out __foundtarget))
                {
                    if (__appium_waitforelement(device.driver,
                        "//android.widget.EditText[@resource-id=\"other-charge-input-again\"]",
                        0x03, out __foundtarget))
                    {
                        if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                        {
                            device.istaskflowinterrupted = true;
                            return false;
                        }
                        __foundtarget.Click();
                        Thread.Sleep(0x03e8 * 0x02 + __rndengine.Next(0x00, 0x03e8 * 0x01));
                        if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                        {
                            device.istaskflowinterrupted = true;
                            return false;
                        }
                        Console.WriteLine($">current task phone number: {device.currenttask.phonenum}");

                        //device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input text {device.currenttask.phonenum}");
                        var __nums = device.currenttask.phonenum.ToCharArray();
                        foreach(var __num in __nums) {
                            device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input text {__num}");
                            Thread.Sleep(__rndengine.Next(0xfa, 0x02ee));
                        }
                        Thread.Sleep(0x03e8 * 0x03 + __rndengine.Next(0x00, 0x03e8 * 0x01));

                        if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                        {
                            device.istaskflowinterrupted = true;
                            return false;
                        }
                        if (__appium_waitforelement(device.driver,
                            "//android.widget.TextView[@text=\"确认\"]",
                            0x03, out __foundtarget))
                        {
                            if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                            {
                                device.istaskflowinterrupted = true;
                                return false;
                            }
                            __foundtarget.Click();
                            Thread.Sleep(0x03e8 * 0x02 + __rndengine.Next(0x00, 0x03e8 * 0x03));
                        }
                    }
                }
                else
                {
                    if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                    {
                        device.istaskflowinterrupted = true;
                        return false;
                    }
                    __tapcoord = __randompoint(365, 985, 5, 5);
                    device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input tap {__tapcoord.X} {__tapcoord.Y}");
                    Thread.Sleep(0x03e8 * 0x02 + __rndengine.Next(0x00, 0x03e8 * 0x02));

                    if (__appium_waitforelement(device.driver,
                        "//android.widget.TextView[@text=\"确认\"]",
                        0x05, out __foundtarget))
                    {
                        if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                        {
                            device.istaskflowinterrupted = true;
                            return false;
                        }
                        __foundtarget.Click();
                        Thread.Sleep(0x03e8 * 0x02 + __rndengine.Next(0x00, 0x03e8 * 0x02));
                        //else
                        //{
                        //    __writelog($">unknow error, current billing return false...", ConsoleColor.Red);
                        //    return false;
                        //}
                    }
                }

                if (__appium_waitforelement(device.driver,
                    "//android.widget.TextView[@text=\"系统检测到您的充值环境异常，无法进行正常充值，请重新登录或切换网络重试\"]",
                    0x03, out __foundtarget))
                {
                    __writelog($">system check exception environments, reinstall will be start...", ConsoleColor.Red);
                    device.isenvexception = true;
                    device.istaskflowinterrupted = true;
                    return false;
                }
                else if (__appium_waitforelement(device.driver,
                    "//android.widget.TextView[@text=\"用户状态异常\"]",
                    0x03, out __foundtarget))
                {
                    __writelog($">user state unexception.", ConsoleColor.Red);
                    return false;
                }

                #endregion
                if (__appium_waitforelement(device.driver,
                    "//android.widget.TextView[@resource-id=\"zw_captcha_basic_bg_1\"]",
                    0x05, out __foundtarget)) {
                    bool __allwaysexists = true;
                    if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                    {
                        device.istaskflowinterrupted = true;
                        return false;
                    }
                    do {
                        Thread.Sleep(0x3e8 + __rndengine.Next(0x00, 0x03e8 * 0x01));
                        __allwaysexists = __appium_waitforelement(device.driver,
                            "//android.widget.TextView[@resource-id=\"zw_captcha_basic_bg_1\"]",
                            0x0a, out __foundtarget);
                        __writelog($">device[{device.id}] found captcha verify action in tasking.", ConsoleColor.DarkYellow);
                    } while (__allwaysexists);
                }

                #region select recharge channel alipay and jump to alipay
                //check local recharge times
                //您当天给他人充值下单次数已达上限，如未支付可选择继续支付或取消订单后重新下单，如已支付请明天再试。
                
                if(__appium_waitforelement(device.driver,
                        "(//android.widget.RadioButton[@text=\"\"])[3]",
                        0x03, out __foundtarget))
                {
                    if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                    {
                        device.istaskflowinterrupted = true;
                        return false;
                    }
                    __foundtarget.Click();
                    Thread.Sleep(0x03e8 + __rndengine.Next(0x00, 0x03e8 * 0x01));

                    if(__appium_waitforelement(device.driver,
                        "//android.widget.Button[@text=\"立即付款\"]",
                        0x03, out __foundtarget))
                    {
                        if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                        {
                            device.istaskflowinterrupted = true;
                            return false;
                        }
                        __foundtarget.Click();
                        Thread.Sleep(0x03e8 * 0x2a + __rndengine.Next(0x00, 0x03e8 * 0x0a));
                    }
                }
                else if (__appium_waitforelement(device.driver,
                    "//android.widget.TextView[@text=\"您今天给他人充值次数已满，请明天再试\"]",
                    0x05, out __foundtarget)
                    )
                {
                    device.samenumcount = 0x03;
                    device.istaskflowinterrupted = true;
                    return false;
                }
                else if (__appium_waitforelement(device.driver,
                    "//android.widget.TextView[@text=\"您当月给他人充值次数已满，请次月再试\"]",
                    0x05, out __foundtarget)
                    )
                {
                    device.samenumcount = 0x03;
                    device.istaskflowinterrupted = true;
                    return false;
                }
                else if(__appium_waitforelement(device.driver,
                    "//android.widget.TextView[@text=\"您当天给他人充值下单次数已达上限，如未支付可选择继续支付或取消订单后重新下单，如已支付请明天再试\"]",
                    0x05, out __foundtarget))
                {
                    device.samenumcount = 0x03;
                    device.istaskflowinterrupted = true;
                    return false;
                } else if(__appium_waitforelement(device.driver,
                    "//android.widget.TextView[@text=\"尊敬的用户，请输入正确的移动手机号码。\"]",
                    0x05,out __foundtarget))
                {
                    return false;
                }

                #endregion

                #region jumped to alipay security activity
                __tapcoord = __randompoint(260, 1120, 5, 5);
                device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input tap {__tapcoord.X} {__tapcoord.Y}");
                Thread.Sleep(0x03e8 * 0x0a + __rndengine.Next(0x00, 0x03e8 * 0x03));

                for (var i = 0x00; i < 0x06; i++)
                {
                    __tapcoord = __randompoint(600, 1220, 5, 5);
                    device.adbclient.ExecuteShellCommand(device.adbdevicedata, "input tap 600 1220");
                    Thread.Sleep(0x03e8 + __rndengine.Next(0x00, 0x03e8 * 0x01));
                }

                var __pwdarray = device.paypwd.ToCharArray();
                for(var i = 0x00; i < __pwdarray.Length; i++)
                {
                    var __curpwdchar = __pwdarray[i];
                    switch (__curpwdchar)
                    {
                        case '0':
                            __tapcoord = __randompoint(360, 1220, 5, 5);
                            break;
                        case '1':
                            __tapcoord = __randompoint(120, 870, 5, 5);
                            //__keycord = "120 870";
                            break;
                        case '2':
                            //__keycord = "360 870";
                            __tapcoord = __randompoint(360, 870, 5, 5);
                            break;
                        case '3':
                            //__keycord = "600 870";
                            __tapcoord = __randompoint(600, 870, 5, 5);
                            break;
                        case '4':
                            //__keycord = "120 985";
                            __tapcoord = __randompoint(120, 985, 5, 5);
                            break;
                        case '5':
                            //__keycord = "360 985";
                            __tapcoord = __randompoint(360, 985, 5, 5);
                            break;
                        case '6':
                            //__keycord = "600 985";
                            __tapcoord = __randompoint(600, 985, 5, 5);
                            break;
                        case '7':
                            //__keycord = "120 1110";
                            __tapcoord = __randompoint(120, 1110, 5, 5);
                            break;
                        case '8':
                            //__keycord = "360 1110";
                            __tapcoord = __randompoint(360, 1110, 5, 5);
                            break;
                        case '9':
                            //__keycord = "600 1110";
                            __tapcoord = __randompoint(600, 1110, 5, 5);
                            break;
                    }
                    device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input tap {__tapcoord.X} {__tapcoord.Y}");
                    Thread.Sleep(0x03e8 * 0x01 + __rndengine.Next(0x00, 0x03e8 * 0x01));
                }

                Thread.Sleep(0x03e8 * 0x03 + __rndengine.Next(0x00, 0x03e8 * 0x01));

                int __count = 0x00;
                do {
                    Thread.Sleep(0x03e8 * 0x02 + __rndengine.Next(0x00, 0x03e8 * 0x01));
                    __tapcoord = __randompoint(650, 100, 5, 5);
                    device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input tap {__tapcoord.X} {__tapcoord.Y}");
                    __count++;
                } while (
                    __count < 0x02
                    //__appium_waitforelement(device.driver,
                    //    "//android.widget.TextView[@text=\"交易已完成\"]",
                    //    0x05, out __foundtarget)
                );


                //if(true 
                //    && __appium_waitforelement(device.driver,
                //    "//android.widget.Button[@resource-id=\"backSubmit\"]",
                //    0x05, out __foundtarget)) {
                //    __foundtarget.Click();
                //    Thread.Sleep(0x03e8 * 0x03 + __rndengine.Next(0x00, 0x03e8 * 0x01));
                //}

                #endregion

                if (false)
                {

                }

                #region final to make task data

                device.currenttask.status = 0x03;
                __result = true;
                if (!__con_finishtasks.ContainsKey(device.currenttask.billingno))
                {
                    device.currenttask.finishedtimestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    __con_finishtasks.TryAdd(device.currenttask.billingno, device.currenttask);
                }
                File.AppendAllText(confs.settings.const_local_rechargehistoryfile, $"{device.currenttask.billingno},{device.currenttask.phonenum},{device.currenttask.amount},{device.currenttask.finishedtimestamp}\r\n");

                #endregion
            }
            else
            {
                if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                {
                    device.istaskflowinterrupted = true;
                    return false;
                }
                if (__appium_waitforelement(device.driver,
                    "//android.widget.TextView[@resource-id=\"com.greenpoint.android.mc10086.activity:id/dialog_title\"]",
                    0x02, out __foundtarget))
                {
                    if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                    {
                        device.istaskflowinterrupted = true;
                        return false;
                    }
                    __writelog($">device[{device.id}] found cmcc authorize pad, process it.", ConsoleColor.Yellow);
                    if (__appium_waitforelement(device.driver,
                        "//android.widget.TextView[@resource-id=\"com.greenpoint.android.mc10086.activity:id/right_btn\"]",
                        0x03, out __foundtarget))
                    {
                        if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                        {
                            device.istaskflowinterrupted = true;
                            return false;
                        }
                        __foundtarget.Click();
                        __writelog($">device[{device.id}] found cmcc authorize pad, processed.", ConsoleColor.Green);
                        Thread.Sleep(0x03e8 * 0x03);

                    }
                    goto __goto_retasking;
                }

                else if (__appium_waitforelement(device.driver,
                    "//android.widget.TextView[@text=\"“中国移动”将获取您的粘贴内容，用于分享、识别，您允许这样做吗？\"]",
                    0x02, out __foundtarget))
                {
                    if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                    {
                        device.istaskflowinterrupted = true;
                        return false;
                    }
                    __writelog($">device[{device.id}] found cmcc clipboard request, processed.", ConsoleColor.Green);
                    if (__appium_waitforelement(device.driver,
                        "//android.widget.Button[@resource-id=\"com.greenpoint.android.mc10086.activity:id/dialog_btn2\"]",
                        0x03, out __foundtarget))
                    {
                        __foundtarget.Click();
                        __writelog($">device[{device.id}] found cmcc clipboard request, processed.", ConsoleColor.Green);
                        Thread.Sleep(0x03e8 * 0x03);


                    }
                    goto __goto_retasking;
                }

                else if (__appium_waitforelement(device.driver,
                    "//android.widget.ImageView[@content-desc=\"关闭\"]",
                    0x02, out __foundtarget))
                {
                    if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                    {
                        device.istaskflowinterrupted = true;
                        return false;
                    }
                    __writelog($">device[{device.id}] found unexcept content: cmcc month no recharge alert action.", ConsoleColor.Yellow);
                    __foundtarget.Click();
                    Thread.Sleep(0x03e8 * 0x03);

                    goto __goto_retasking;
                }

                else if (__appium_waitforelement(device.driver,
                        "//android.widget.TextView[@text=\"我们的进步离不开您的支持与鼓励!\"]",
                        0x02, out __foundtarget))
                {
                    if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                    {
                        device.istaskflowinterrupted = true;
                        return false;
                    }
                    __writelog($">device[{device.id}] found cmcc qiuhaoping pad, process it.", ConsoleColor.Yellow);
                    if (__appium_waitforelement(device.driver,
                        "//android.widget.Button[@content-desc=\"关闭\"]",
                        0x02, out __foundtarget))
                    {
                        if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                        {
                            device.istaskflowinterrupted = true;
                            return false;
                        }
                        __foundtarget.Click();
                        __writelog($">device[{device.id}] found cmcc qiuhaoping pad, processed.", ConsoleColor.Green);
                        Thread.Sleep(0x03e8 * 0x03);

                    }
                    goto __goto_retasking;
                }

                else if (__appium_waitforelement(device.driver,
                    "//android.widget.Button[@text=\"充值·缴费\"]",
                    0x02, out __foundtarget))
                {

                    goto __goto_retasking;
                }

                else
                {
                    if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                    {
                        device.istaskflowinterrupted = true;
                        return false;
                    }
                    __tapcoord = __randompoint(360, 540, 5, 5);
                    device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input tap {__tapcoord.X} {__tapcoord.Y}");
                    Thread.Sleep(0x03e8 * 0x02);
                    if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                    {
                        device.istaskflowinterrupted = true;
                        return false;
                    }
                    __tapcoord = __randompoint(360, 540, 5, 5);
                    device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input tap {__tapcoord.X} {__tapcoord.Y}");
                    Thread.Sleep(0x03e8 * 0x02);
                    if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                    {
                        device.istaskflowinterrupted = true;
                        return false;
                    }
                    __tapcoord = __randompoint(360, 540, 5, 5);
                    device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input tap {__tapcoord.X} {__tapcoord.Y}");
                    Thread.Sleep(0x03e8 * 0x02);
                    if (device.cmccstatus != AdvancedSharpAdbClient.Models.AppStatus.Foreground)
                    {
                        device.istaskflowinterrupted = true;
                        return false;
                    }
                    __tapcoord = __randompoint(360, 540, 5, 5);
                    device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input tap {__tapcoord.X} {__tapcoord.Y}");
                    Thread.Sleep(0x03e8 * 0x02);
                    __writelog($">device[{__device1.id}] can not found cmcc recharge entry, failed.", ConsoleColor.Red);

                    goto __goto_retasking;
                }
            }

            return __result;
        }


        private void __appium_lasttradequery(androiddevice device)
        {
            AppiumElement __foundtarget = null;

            #region jump to mine activity

            if (__appium_waitforelement(device.driver,
                "(//android.widget.ImageView[@resource-id=\"com.greenpoint.android.mc10086.activity:id/custom_tab_icon\"])[5]",
                0x05, out __foundtarget))
            {
                __foundtarget.Click();
                Thread.Sleep(0x03e8);
                if (__appium_waitforelement(device.driver,
                    "(//android.widget.ImageView[@resource-id=\"com.greenpoint.android.mc10086.activity:id/iv_mine_icon\"])[12]",
                    0x05, out __foundtarget))
                {
                    __foundtarget.Click();
                    Thread.Sleep(0x03e8 * 0x05);

                    AppiumElement __tempelement = null;
                    bool __firstbillingfound = false;
                    do
                    {
                        Thread.Sleep(0x3e8);
                        __firstbillingfound = __appium_waitforelement(device.driver,
                            "//android.view.View[@resource-id=\"md-orderlist\"]/android.widget.ListView[1]/android.view.View/android.widget.TextView[1]",
                            0x03, out __tempelement);
                        ;
                    } while (!__firstbillingfound);
                    string __billingstate = __tempelement.Text.Replace("充值类-话费充值", string.Empty);
                    if(null != device.currenttask)
                        switch (__billingstate)
                        {
                            case "已完成":
                                device.currenttask.status = 0x04;
                                break;
                            case "超时未付":
                                device.currenttask.status = -0x01;
                                break;
                            default:
                                device.currenttask.status = -0x01;
                                break;
                        }

                    do
                    {
                        //click first billing in mine billings list
                        device.adbclient.ExecuteShellCommand(device.adbdevicedata,
                            "input tap 350 580");
                        Thread.Sleep(0x03e8 * 0x05);
                    } while (!__appium_waitforelement(device.driver,
                        "//android.view.View[@text=\"充值成功\"]",
                        0x03, out __foundtarget));

                    if (__appium_waitforelement(device.driver,
                        "//android.view.View[@resource-id=\"detail\"]/android.widget.TextView[1]",
                        0x03, out __foundtarget))
                    {
                        string __isptradeno = Regex.Replace(__foundtarget.Text, "订单编号：", string.Empty, RegexOptions.None).Trim();

                        if (null != device.currenttask)
                        {
                            device.currenttask.isptradeno = __isptradeno;
                            var __task = device.currenttask;

                            if (device.currenttask.status < 0x00) {
                                __task.isptradeno = null;
                                __task.status = 0x02;
                                __con_localwaittingtasks.Enqueue(__task);

                                __writelog($">device[{device.id}] task[{__task}] recharge fail,re-enqueue and tasking later.", ConsoleColor.DarkYellow);
                            }
                            else
                            {
                                bool __ret = false;
                                string __response = Http.request(new Http.requestparam(
                                    $"{confs.settings.CloudBaseUri}/local/taskresponse?billingno={__task.billingno}&tradeno={__task.isptradeno}&status={__task.status}"),
                                    out __ret);
                                __writelog($">device[{device.id}] task[{__task}] recharge successed, callbacked to cloud.", ConsoleColor.DarkYellow);
                            }


                            device.currenttask = null;
                        }

                        device.samenumcount++;
                    }
                }
            }

            #endregion
        }

        private bool __chinac_reinstall(androiddevice device)
        {
            bool __result = false;

            if (!string.IsNullOrEmpty(device.cloudid)
                && !string.IsNullOrEmpty(device.cloudimageid))
            {
                ChinacSDKCore.NET.Models.ReinstallCloudPhoneRequest __request_reinstallcloudphone =
                    new ChinacSDKCore.NET.Models.ReinstallCloudPhoneRequest() { 
                        CloudPhones = new List<ChinacSDKCore.NET.Models.ReinstallCloudPhoneRequest.CloudPhone>() { 
                            new ChinacSDKCore.NET.Models.ReinstallCloudPhoneRequest.CloudPhone() {
                                Id = device.cloudid,
                                Region = device.cloudregion
                            },
                        },
                        ImageId = device.cloudimageid,
                        Remark = $"{DateTime.Now.ToString("yyyyMMddHHmmss")}",
                        ParamType = "random"
                    };
                ChinacSDKCore.NET.Models.ReinstallCloudPhoneResponse __response_reinstallcloudphone =
                    __ccmclient.ReinstallCloudPhone(__request_reinstallcloudphone);
                if (__response_reinstallcloudphone.code == 10000)
                    __result = true;
            }

            return __result;
        }

        private bool __chinac_onekeyhome(androiddevice device)
        {
            bool __result = false;

            ChinacSDKCore.NET.Models.GetCmdApiSignatureRequest __request_getcmdapisignature =
                new ChinacSDKCore.NET.Models.GetCmdApiSignatureRequest() { 
                    Ids = new List<string>() { device.cloudid },
                    Region = device.cloudregion
                };
            ChinacSDKCore.NET.Models.GetCmdApiSignatureResponse __response_getcmdapisignature =
                __ccmclient.GetCmdApiSignature(__request_getcmdapisignature);
            if(null != __response_getcmdapisignature &&
                __response_getcmdapisignature.code == 10000)
            {
                ChinacSDKCore.NET.Models.ClickHomeRequest __request_clickhome =
                    new ChinacSDKCore.NET.Models.ClickHomeRequest() { 
                        RToken = __response_getcmdapisignature.data.RToken,
                        Id = device.cloudid
                    };
                ChinacSDKCore.NET.Models.ClickHomeResponse __response_clickhome =
                    __ccmclient.ClickHome(__request_clickhome, 
                    __response_getcmdapisignature.data.ApiUrl,
                    __response_getcmdapisignature.data.Signature);
                if(null != __response_clickhome &&
                    __response_clickhome.ResponseCode == 100000)
                {
                    __result = true;
                    __writelog($">device[{device.id}] invoke chinac onkey-home successed.",
                        ConsoleColor.Green);
                }
                else
                {
                    __writelog($">device[{device.id}] invoke chinac onkey-home failed, message: {__response_getcmdapisignature.message}",
                        ConsoleColor.Red);
                }
            }
            else
            {
                __writelog($">device[{device.id}] invoke chinac onkey-home failed, message: {__response_getcmdapisignature.message}",
                    ConsoleColor.Red);
            }

            return __result;
        }

        private bool __chinac_cloudinit()
        {
            bool __result = false;

            ChinacSDKCore.NET.Models.ListCloudPhoneRequest __request_listcloudphone =
                new ChinacSDKCore.NET.Models.ListCloudPhoneRequest() { };
            var __response_listcloudphone = __ccmclient.ListCloudPhone(__request_listcloudphone);
            if(null != __response_listcloudphone 
                && __response_listcloudphone.code == 10000 
                && null != __response_listcloudphone.data
                && null != __response_listcloudphone.data.Page
                && __response_listcloudphone.data.Page.TotalCount > 0x00
                && null != __response_listcloudphone.data.List
                && __response_listcloudphone.data.List.Count > 0x00) {

                if (!string.IsNullOrEmpty(__device1.id) && __response_listcloudphone.data.List.Any(p => p.Name == __device1.name))
                {
                    var __cphone = __response_listcloudphone.data.List.SingleOrDefault(p => p.Name == __device1.name);
                    __device1.cloudid = __cphone.Id;
                    __device1.cloudregion = __cphone.Region;
                }
                if(!string.IsNullOrEmpty(__device2.id) && __response_listcloudphone.data.List.Any(p=>p.Name == __device2.name))
                {
                    var __cphone = __response_listcloudphone.data.List.SingleOrDefault(p => p.Name == __device2.name);
                    __device2.cloudid = __cphone.Id;
                    __device2.cloudregion = __cphone.Region;
                }

                __result = true;
            }

            return __result;
        }

        private Point __randompoint(int centreX, int centreY, int offsetX, int offsetY)
        {
            Random __rndengine = new Random(DateTime.Now.Millisecond);
            return new Point(
                centreX + (__rndengine.Next(0x00, offsetX * 0x02 + 0x01) - offsetX),
                centreY + (__rndengine.Next(0x00, offsetY * 0x02 + 0x01) - offsetY));
        }

        private void __tap_originalkeyboard_num(androiddevice device, char num, int sleepinterval = 0x01f4)
        {
            //1,200 900
            //2,360 900
            //3,520 900
            //4,200 1005
            //5,360 1005
            //6,520 1005
            //7,200 1115
            //8,360 1115
            //9,520 1115
            //0,360 1120
            //d,655 810

            Point __tapcoord = Point.Empty;
            switch (num)
            {
                case '0':
                    __tapcoord = __randompoint(360, 1220, 5, 5);
                    break;
                case '1':
                    __tapcoord = __randompoint(200, 900, 5, 5);
                    break;
                case '2':
                    __tapcoord = __randompoint(360, 900, 5, 5);
                    break;
                case '3':
                    __tapcoord = __randompoint(520, 900, 5, 5);
                    break;
                case '4':
                    __tapcoord = __randompoint(200, 1005, 5, 5);
                    break;
                case '5':
                    __tapcoord = __randompoint(360, 1005, 5, 5);
                    break;
                case '6':
                    __tapcoord = __randompoint(520, 1005, 5, 5);
                    break;
                case '7':
                    __tapcoord = __randompoint(200, 1115, 5, 5);
                    break;
                case '8':
                    __tapcoord = __randompoint(360, 1115, 5, 5);
                    break;
                case '9':
                    __tapcoord = __randompoint(520, 1115, 5, 5);
                    break;
            }
            device.adbclient.ExecuteShellCommand(device.adbdevicedata, $"input tap {__tapcoord.X} {__tapcoord.Y}");
            Thread.Sleep(sleepinterval);
        }
    }
}
