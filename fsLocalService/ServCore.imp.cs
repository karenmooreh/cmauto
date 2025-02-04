using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.DeviceCommands;
using AdvancedSharpAdbClient.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace fsLocalService
{
    public partial class ServCore
    {
        private void __constructor_ServCore(Dictionary<string, string> args)
        {
            __singleton = this;

            __con_localwaittingtasks = new System.Collections.Concurrent.ConcurrentQueue<models.task>();
            __con_finishtasks = new System.Collections.Concurrent.ConcurrentDictionary<string, models.task>();
            __con_phonenumsrepository = new System.Collections.Concurrent.ConcurrentQueue<string>();
            __con_logs = new System.Collections.Concurrent.ConcurrentQueue<models.log>();
            __phonefile = args.ContainsKey("pf")
                && !string.IsNullOrEmpty(args["pf"])
                && File.Exists($"{(Path.IsPathRooted(args["pf"]) ? 
                args["pf"] : Path.Combine(Path.GetDirectoryName(typeof(ServCore).Assembly.Location), args["pf"]))}") ?
                Path.IsPathRooted(args["pf"]) ? args["pf"] :
                    Path.Combine(Path.GetDirectoryName(typeof(ServCore).Assembly.Location), args["pf"]) : 
                    confs.settings.const_platform_jiema_defaultfile;

            __ccmclient = new ChinacSDKCore.NET.ChinacCloudMobileClient(
                confs.settings.chinac.apikeyid, confs.settings.chinac.apikeysecret);

            __serv_webapi = new OpenNetworkPlatform.cores.services.webapi.IntegratedWebAPIService<ServCore>();


            __device1 = new androiddevice()
            {
                id = confs.settings.devices.device1.serial,
                name = confs.settings.devices.device1.name,
                paypwd = confs.settings.devices.device1.paypwd,
                cloudimageid = confs.settings.devices.device1.cloudimageid,
            };
            __device2 = new androiddevice()
            {
                id = confs.settings.devices.device2.serial,
                name = confs.settings.devices.device2.name,
                paypwd = confs.settings.devices.device2.paypwd,
                cloudimageid = confs.settings.devices.device2.cloudimageid,
            };
        }


        private void __start()
        {
            if (!__status)
            {
                __status = true;

                (__thd_logging = new Thread(new ThreadStart(__thdmtd_logging)) { IsBackground = true }).Start();

                __writelog($">local webapi starting...", ConsoleColor.Cyan);
                __serv_webapi.Start();
                __writelog($">local webapi started.", ConsoleColor.Green);
                __writelog($">press enter key to continue...");
                Console.ReadLine();

                __writelog($">cloud service verifing...", ConsoleColor.Cyan);
                bool __ret = false;
                string __response = Http.request(new Http.requestparam($"{confs.settings.CloudBaseUri}/test/hello")
                {
                    method = HttpMethod.Get,
                    contenttype = Http.requestparam.contenttype_application_json
                }, out __ret);
                if(__ret && !string.IsNullOrEmpty(__response))
                {
                    __writelog($">cloud service response data: {__response}", ConsoleColor.Green);
                }
                else
                {
                    __writelog(">cloud service unavailable.", ConsoleColor.Red);
                }

                if (__status)
                {
                    #region jiema init

                    if (File.Exists(__phonefile)) {
                        string[] __phonenums = Regex.Split(File.ReadAllText(__phonefile), "\\r*\\n", RegexOptions.IgnorePatternWhitespace);
                        foreach (string __phonenum in __phonenums)
                            if (!string.IsNullOrEmpty(__phonenum) && Regex.IsMatch(__phonenum, @"^1[3456789]\d{9}$"))
                                __con_phonenumsrepository.Enqueue(__phonenum);
                    }

                    #endregion

                    #region adb server init

                    var __adbservresult = __adb_initserver();

                    #endregion

                    #region chinac cloud api client init

                    bool __cloudinitresult = __chinac_cloudinit();

                    #endregion

                    #region device option
                    //device1
                    if (true &&
                        !string.IsNullOrEmpty(confs.settings.devices.device1.serial)) {
                        //ThreadPool.QueueUserWorkItem(new WaitCallback(__device_initcmccapp), __device1);

                        string __lastphonefile = Path.Combine(Path.GetDirectoryName(typeof(ServCore).Assembly.Location), $"confs/{__device1.id.Replace(":", "-")}.last");
                        if (File.Exists(__lastphonefile))
                            __device1.lastphone = File.ReadAllText(__lastphonefile).Trim();

                        //if(__adbservresult == StartServerResult.Started || __adbservresult == StartServerResult.AlreadyRunning)
                        //{
                        {
                            __adb_connectadbclient(__device1);
                        }
                        //}
                        __device1.thd_cmccmonitoring = new Thread(
                            new ParameterizedThreadStart(__thdmtd_cmccmonitoring)) { IsBackground = true };
                        __device1.thd_cmccmonitoring.Start(__device1);

                        __device1.thd_working = new Thread(
                            new ParameterizedThreadStart(
                                __thdmtd_working
                                //__thdmtd_workingoncloud
                                )) { IsBackground = true };
                        __device1.thd_working.Start(__device1);

                    }

                    //device2
                    if (false &&
                        !string.IsNullOrEmpty(confs.settings.devices.device2.serial))
                    {
                        string __lastphonefile = Path.Combine(Path.GetDirectoryName(typeof(ServCore).Assembly.Location), $"confs/{__device2.id.Replace(":", "-")}.last");
                        if (File.Exists(__lastphonefile))
                            __device2.lastphone = File.ReadAllText(__lastphonefile).Trim();

                        //if(__adbservresult == StartServerResult.Started || __adbservresult == StartServerResult.AlreadyRunning)
                        //{
                        {
                            __adb_connectadbclient(__device2);
                        }
                        //}
                        __device2.thd_cmccmonitoring = new Thread(
                            new ParameterizedThreadStart(__thdmtd_cmccmonitoring))
                        { IsBackground = true };
                        __device2.thd_cmccmonitoring.Start(__device2);

                        __device2.thd_working = new Thread(
                            new ParameterizedThreadStart(
                                __thdmtd_working
                                //__thdmtd_workingoncloud
                                ))
                        { IsBackground = true };
                        __device2.thd_working.Start(__device2);
                    }

                    #endregion

                    #region query from cloud

                    (__thd_cloudbillingsgetting = new Thread(
                        new ThreadStart(__thdmtd_cloudbillingsgetting))
                    { IsBackground = true }).Start();

                    #endregion

                    #region works allocating

                    //(__thd_worksallocating = new Thread(
                    //    new ThreadStart(__thdmtd_worksallocating))
                    //{ IsBackground = true }).Start();

                    #endregion
                }
                else
                {
                    __writelog(">local service start failed.", ConsoleColor.Red);
                }
            }
        }

        private void __thdmtd_androidinit(object device)
        {

        }

        private StartServerResult __adb_initserver()
        {
            __serv_adbserver = new AdvancedSharpAdbClient.AdbServer();
            StartServerResult __result = __serv_adbserver.StartServer(confs.settings.adb.path, false);
            if (__result == StartServerResult.Started || __result == StartServerResult.AlreadyRunning)
                __writelog($"adb server running, result: {__result}", ConsoleColor.Cyan);
            else __writelog($"adb server start fail, result: {__result}", ConsoleColor.Red);
            return __result;
        }

        private void __device_initcmccapp(object dev)
        {
            androiddevice __device = dev as androiddevice;
            __writelog($">device[{__device.id}] framework initialing...", ConsoleColor.Green);

            Stopwatch __timewatcher = new Stopwatch();

            if (__device != null)
            {
                var __driveropts = new AppiumOptions()
                {
                    AutomationName = AutomationName.AndroidUIAutomator2,
                    PlatformName = "Android",
                    DeviceName = __device.id,
                };
                __driveropts.AddAdditionalAppiumOption("appPackage", confs.settings.const_android_packages_cmcc);
                __driveropts.AddAdditionalAppiumOption("appActivity", confs.settings.const_android_activity_cmcc);
                __driveropts.AddAdditionalAppiumOption("noReset", true);

                __device.driver = new OpenQA.Selenium.Appium.Android.AndroidDriver(new Uri(
                    $"http://{confs.settings.const_android_appiumserver_address}:{confs.settings.const_android_appiumserver_port}{confs.settings.const_android_appiumserver_sessionpath}"),
                    __driveropts, TimeSpan.FromSeconds(0x64)
                    );
                __device.driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0x0a);
                __device.status = devstatus.init;

                __writelog($">device[{__device.id}] cmcc official app started.", ConsoleColor.Green);

                bool __chapterresult = false;
                __chapterresult = __scriptchapter_defaultactivity_checkresignneeded(__device);

                if (__chapterresult)
                {
                    string __workphone = null;
                    if (__con_phonenumsrepository.TryDequeue(out __workphone))
                    {
                        string __serialno = __device.workserialno = null;

                        while (string.IsNullOrEmpty(__serialno))
                        {
                            __serialno = __platformjiema_requestnewphone(__workphone);
                            if (!string.IsNullOrEmpty(__serialno))
                            {
                                __device.workserialno = __serialno;
                                __device.workphonenum = __workphone;
                                __writelog($"device[{__device.id}] phone[{__workphone}] request success", ConsoleColor.Green);
                            }
                            else
                            {
                                __writelog($"device[{__device.id}] phone[{__workphone}] request fail,wait for next request", ConsoleColor.Red);
                                Thread.Sleep(0x7530);
                            }
                        }

                        if (!string.IsNullOrEmpty(__device.workserialno) && !string.IsNullOrEmpty(__device.workphonenum))
                        {
                            __writelog($"jiema platform phone worked, workdata[serialno: {__device.workserialno}, phonenum: {__device.workphonenum}]", ConsoleColor.Green);
                            __chapterresult = __scriptchapter_securityactivity_cmccsignin(__device);
                        }
                        else
                        {
                            __writelog($"jiema platform exception, workdata[serialno: {__device.workserialno}, phonenum: {__device.workphonenum}]", ConsoleColor.Red);
                        }
                    }
                    else
                    {
                        __writelog($"phonenum not enough", ConsoleColor.Red);
                    }
                }

                __device.status = devstatus.idle;
            }
            else __device.status = devstatus.unknow;
        }

        private void __adb_connectadbclient(androiddevice device)
        {
            device.adbclient = new AdvancedSharpAdbClient.AdbClient();
            IPEndPoint __adbservaddr = (IPEndPoint)__serv_adbserver.EndPoint;
            string __connresult =
                device.adbclient.Connect(
                    confs.settings.devices.device1.serial.Split(":")[0x00],
                    int.Parse(confs.settings.devices.device1.serial.Split(":")[0x01]));
            __writelog($">device[{device.id}] adbclient connect result: {__connresult}", ConsoleColor.Yellow);
            var __devices = device.adbclient.GetDevices();
            if (__devices.Any(d => d.Serial == device.id))
            {
                device.adbdevicedata = __devices.FirstOrDefault(d => d.Serial == device.id);
                device.devclient = device.adbclient.CreateDeviceClient(device.adbdevicedata);
                __writelog($">device[{device.id}] got adb device data[name: {device.adbdevicedata.Name}, model: {device.adbdevicedata.Model}]", ConsoleColor.Green);
            }
        }


        private bool __appium_waitforelement(AndroidDriver driver, string xpath, int timeoutseconds, out AppiumElement foundelement) {
            bool __result = false;
            foundelement = null;

            Stopwatch __timewatcher = new Stopwatch();
            __timewatcher.Start();
            while(!__result && __timewatcher.Elapsed.TotalSeconds < timeoutseconds)
            {
                try {
                    foundelement = driver.FindElement(By.XPath(xpath));
                } catch { }
                if(null != foundelement)
                    __result = true;
                else 
                    Thread.Sleep(0x0a);
            }
            __timewatcher.Stop();

            return __result;
        }

        private bool __adb_waitforelement(androiddevice device, string xpath, int timeoutseconds, out AdvancedSharpAdbClient.DeviceCommands.Models.Element foundelement)
        {
            bool __result = false;
            foundelement = null;

            Stopwatch __timewatcher = new Stopwatch();
            __timewatcher.Start();
            while(!__result && __timewatcher.Elapsed.TotalSeconds < timeoutseconds)
            {
                try {
                    foundelement = device.devclient.AdbClient.FindElement(device.adbdevicedata, xpath, TimeSpan.FromSeconds(timeoutseconds / 0x03));
                } catch { }
                if (null != foundelement) __result = true;
                else Thread.Sleep(0x0a);
            }
            __timewatcher.Stop();

            return __result;
        }

        private void __thdmtd_cloudbillingsgetting()
        {
            while (__status)
            {
                string __response = null;
                bool __ret = false;

                __response = Http.request(new Http.requestparam(
                    $"{confs.settings.CloudBaseUri}/local/freshbillings?stocknum={(confs.settings.cloud.getbillingsblock > 0x00 ? confs.settings.cloud.getbillingsblock : 0x0a)}"),
                    out __ret);
                if (__ret && !string.IsNullOrEmpty(__response)) {
                    models.result_freshbillings __result = null;
                    try { __result = JsonSerializer.Deserialize<models.result_freshbillings>(__response); } catch { }
                    if(null != __result)
                    {
                        if(__result.count > 0x00)
                        {
                            __writelog($">gotted {__result.count} fresh billings,enqueued.", ConsoleColor.Cyan);
                            foreach(var __billing in __result.data)
                            {
                                models.task __task = new models.task() { 
                                    billingno = __billing.billingno,
                                    phonenum = __billing.phonenum,
                                    amount = __billing.amount,
                                    tasktradeno = Guid.NewGuid().ToString("N"),
                                    status = 0x02,
                                    isptradeno = null,
                                    timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff")
                                };
                                __con_localwaittingtasks.Enqueue(__task);

                                __response = Http.request(new Http.requestparam($"{confs.settings.CloudBaseUri}/local/taskresponse?billingno={__task.billingno}&tradeno={__task.tasktradeno}&status={__task.status}"), out __ret);
                                if(__ret && !string.IsNullOrEmpty(__response))
                                {
                                    models.result_taskresponse __tresult = null;
                                    try { __tresult = JsonSerializer.Deserialize<models.result_taskresponse>(__response); } catch { }
                                    if(null != __tresult)
                                    {
                                        __writelog($"task[{__task.tasktradeno}, billingno: {__task.billingno}, amount: {__task.amount}] notify callback successed", ConsoleColor.Green);
                                    }
                                    else
                                    {
                                        __writelog($"task[{__task.tasktradeno}, billingno: {__task.billingno}, amount: {__task.amount}] notify callback failed. unsupported data format datagram,check cloud service status", ConsoleColor.Yellow);
                                    }
                                }
                                else
                                {
                                    __writelog($">task[{__task.tasktradeno}, billingno: {__task.billingno}, amount: {__task.amount}] notify callback failed, cloud service unavailable", ConsoleColor.Red);
                                }
                            }
                        }
                        else
                        {
                            __writelog($">there is none fresh billings currently,wait for next request...");
                        }
                    }
                    else
                    {
                        __writelog($">unsupported data format datagram,check cloud service status", ConsoleColor.Yellow);
                    }
                }
                else
                {
                    __writelog($">get freshbillings failed,cloud service unavailable at {DateTime.Now.ToString()}", ConsoleColor.Red);
                }

                Thread.Sleep(confs.settings.cloud.getbillingsinterval);
            }
        }

        private void __thdmtd_deviceworking(object dev)
        {

        }

        private void __thdmtd_worksallocating()
        {
            while (true) {
                androiddevice __idledev;
                models.task __dequeuetask;
                if (__con_localwaittingtasks.TryDequeue(out __dequeuetask) 
                    && __getidledevice(out __idledev)) {
                    
                    if(null == __idledev.thd_working)
                        __idledev.thd_working = new Thread(
                            new ParameterizedThreadStart(__thdmtd_deviceworking))
                            { IsBackground = true };
                    __idledev.status = devstatus.working;
                    __idledev.currenttask = __dequeuetask;
                    __idledev.thd_working.Start(__idledev);

                }
                Thread.Sleep(0x64);
            }
        }

        private bool __getidledevice(out androiddevice idledevice)
        {
            bool __result = false;
            idledevice = null;

            if (__device1.status == devstatus.idle || __device2.status == devstatus.idle)
            {
                idledevice = __device1.status == devstatus.idle ? __device1 : __device2;
                __result = true;
            }

            return __result;
        }

        private void __writelog(string log, ConsoleColor color = ConsoleColor.White)
        {
            
            __con_logs.Enqueue(new models.log() { 
                logcontext = log,
                color = color,
                time = DateTime.Now
            });
        }

        private void __thdmtd_logging()
        {
            while (__status)
            {
                if (__con_logs.Count > 0x00)
                {
                    models.log __displaylog = null;
                    if(__con_logs.TryDequeue(out __displaylog))
                    {
                        if (__displaylog.color != ConsoleColor.White) Console.ForegroundColor = __displaylog.color;
                        Console.WriteLine($"[{__displaylog.time.ToString()}]{__displaylog.logcontext}");
                        if (__displaylog.color != ConsoleColor.White) Console.ForegroundColor = ConsoleColor.White;
                    }
                }

                Thread.Sleep(0x64);
            }
        }

    }
}
