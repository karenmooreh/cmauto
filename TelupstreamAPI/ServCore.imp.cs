using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelupstreamAPI.Common;

namespace TelupstreamAPI
{
    internal partial class ServCore
    {
        private void __constructor_ServCore()
        {
            __singleton = this;

            __con_presettasks = new System.Collections.Concurrent.ConcurrentQueue<Models.PresetTask>();
        }

        private void __start()
        {
            if (!__status)
            {
                __status = true;

                __webapiserv = new OpenNetworkPlatform.cores.services.webapi.IntegratedWebAPIService<ServCore>();
                __webapiserv.Start();

                __thd_taskaccepting = new Thread(new ThreadStart(__thdmtd_taskaccepting)) { IsBackground = true };
                __thd_taskaccepting.Start();
            }
        }

        private bool __addtask(string appid, string appkey, 
            string billingno, string phonenum, string amount, string param,string notifyuri, string sign) {
            bool __result = false;

            if (!((new EFCore.Contexts.TelupstreamStorages()).tasks.Any(t => t.billingno == billingno)))
            {
                if (!__con_presettasks.Any(t => t.billingno == billingno))
                {
                    __con_presettasks.Enqueue(new Models.PresetTask() {
                        appid = appid,
                        appkey = appkey,
                        phonenum = phonenum,
                        billingno = billingno,
                        amount = amount,
                        notifyuri = notifyuri,
                        param = param,
                    });
                    __result = true;
                }
            }

            #region test

            //if (!string.IsNullOrEmpty(notifyuri))
            //{
            //    //test thread pool codes
            //    ThreadPool.QueueUserWorkItem(o =>
            //    {
            //        Thread.Sleep(60000);
            //        bool __ret = false;

            //        string __timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            //        string __tradeno = $"{appid.Substring(0x00, 0x06)}{sign.Substring(0x00, 0x06)}{__timestamp}";

            //        Dictionary<string, string> __params = new Dictionary<string, string>() {
            //            { "billingno", billingno },
            //            { "tradeno", __tradeno },
            //            { "phonenum", phonenum },
            //            { "amount", amount },
            //            { "result", "true" },
            //            { "code", "0" },
            //            { "timestamp", __timestamp },
            //        };
            //        if (!string.IsNullOrEmpty(param)) __params.Add("param", param);
            //        string __signstr = ServCore.Sign(__params, appkey);

            //        Http.request(new Http.requestparam($"{notifyuri}?billingno={billingno}&tradeno={__tradeno}&phonenum={phonenum}&amount={amount}&result=true&code=0&timestamp={__timestamp}&param={param}&sign={__signstr}")
            //        { method = HttpMethod.Get }, out __ret);
            //    });
            //}

            #endregion

            return __result;
        }


        private void __thdmtd_taskaccepting()
        {
            while (__status)
            {
                if (__con_presettasks.Count > 0x00) {
                    Models.PresetTask __outtask;
                    if(__con_presettasks.TryDequeue(out __outtask)) {
                        string __timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        EFCore.Contexts.TelupstreamStorages __storage = new EFCore.Contexts.TelupstreamStorages();
                        EFCore.Models.task __storagetask = new EFCore.Models.task()
                        {
                            id = Guid.NewGuid().ToString("N"),
                            tradeno = MD5Crypto($"{__outtask.appid.Substring(0x00,0x03)}{__outtask.phonenum}{__timestamp}"),
                            appid = __outtask.appid,
                            billingno = __outtask.billingno,
                            phonenum = __outtask.phonenum,
                            amount = double.Parse(__outtask.amount),
                            notifyuri = __outtask.notifyuri,
                            param = __outtask.param,
                            status = 0x01,
                            regtime = DateTime.Now
                        };
                        __storage.Add(__storagetask);

                        try { __storage.SaveChanges(); } catch { }

                        if (!string.IsNullOrEmpty(__outtask.notifyuri)) {
                            Dictionary<string, string> __params = new Dictionary<string, string>() {
                                { "billingno", __storagetask.billingno },
                                { "tradeno", __storagetask.tradeno },
                                { "phonenum", __storagetask.phonenum },
                                { "amount", __outtask.amount },
                                { "result", "true" },
                                { "code", "0" },
                                { "timestamp", __timestamp },
                            };
                            if (!string.IsNullOrEmpty(__outtask.param)) __params.Add("param", __outtask.param);
                            string __signstr = Sign(__params, __outtask.appkey);
                            __params.Add("sign", __signstr);
                            string __searchstr = GetSearch(__params);

                            bool __httpret = false;
                            Http.request(new Http.requestparam($"{__outtask.notifyuri}?{__searchstr}"), out __httpret);

                            if (__httpret)
                            {
                                //...
                            } else {

                            }
                        }
                    }
                }

                Thread.Sleep(0x64);
            }
        }
    }
}
