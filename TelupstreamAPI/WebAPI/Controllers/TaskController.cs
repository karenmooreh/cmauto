using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TelupstreamAPI.WebAPI.Controllers
{
    [ApiController]
    [Route("task")]
    public class TaskController : ControllerBase
    {
        [HttpGet]
        [Route("add")]
        public Models.Task.result_add add(string appid, 
            string billingno,
            string phonenum,
            string amount, 
            string? notify_uri, 
            string? param, 
            string sign) {
            Models.Task.result_add __result = new Models.Task.result_add() { ret = false };

            if (confs.settings.auths.Any(a => a.appid == appid)) {
                var __authinfo = confs.settings.auths.SingleOrDefault(s => s.appid == appid);
                Dictionary<string, string> __params = new Dictionary<string, string>();
                __params.Add("appid", appid);
                __params.Add("billingno", billingno);
                __params.Add("phonenum", phonenum);
                __params.Add("amount", amount);
                if (!string.IsNullOrEmpty(notify_uri)) __params.Add("notify_uri", notify_uri);
                if (!string.IsNullOrEmpty(param)) __params.Add("param", param);
                string __signstr = ServCore.Sign(__params, __authinfo.appkey);
                
                if(__signstr == sign) {
                    bool __ret = ServCore.Singleton.AddTask(__authinfo.appid, __authinfo.appkey,
                        billingno, phonenum, amount, param, notify_uri, sign);

                    __result.ret = __ret;
                    __result.code = __ret ? 0x00 : -0x01;
                    __result.msg = $"新增充值任务{(__ret ? "成功" : "失败：队列或库中已有相同订单号任务已受理或等待受理")}";
                } else {
                    __result.ret = false;
                    __result.code = -0x01;
                    __result.msg = "新增充值任务失败：签名数据异常";
                }
            } else {
                __result.ret = false;
                __result.code = -0x01;
                __result.msg = "新增充值任务失败：未知的AppId";
            }
            return __result;
        }

        [HttpGet]
        [Route("query")]
        public Models.Task.result_query query(string appid, 
            string? billingno, string? phonenum, 
            string? param, string sign) {
            Models.Task.result_query __result = new Models.Task.result_query() { ret = false };

            if (confs.settings.auths.Any(a => a.appid == appid))
            {
                if (!(string.IsNullOrEmpty(billingno) && string.IsNullOrEmpty(phonenum)))
                {
                    var __authinfo = confs.settings.auths.SingleOrDefault(s => s.appid == appid);
                    Dictionary<string, string> __params = new Dictionary<string, string>();
                    __params.Add("appid", appid);
                    if (!string.IsNullOrEmpty(billingno)) __params.Add("billingno", billingno);
                    if (!string.IsNullOrEmpty(phonenum)) __params.Add("phonenum", phonenum);
                    if (!string.IsNullOrEmpty(param)) __params.Add("param", param);
                    string __signstr = ServCore.Sign(__params, __authinfo.appkey);

                    if(__signstr == sign) {
                        string[] __billingnos = null;
                        if (!string.IsNullOrEmpty(billingno))
                            __billingnos = Regex.Split(billingno, ",", RegexOptions.IgnorePatternWhitespace);

                        if (ServCore.Singleton.PresetTasks.Any(t => 0x01 == 0x01
                            && null != __billingnos && __billingnos.Length > 0x00 ? __billingnos.Contains(t.billingno) : true
                            && !string.IsNullOrEmpty(phonenum) ? t.phonenum == phonenum : true))
                        {

                            var __matchedtasks = ServCore.Singleton.PresetTasks.Where(t => 0x01 == 0x01
                            && null != __billingnos && __billingnos.Length > 0x00 ? __billingnos.Contains(t.billingno) : true
                            && !string.IsNullOrEmpty(phonenum) ? t.phonenum == phonenum : true).ToList();

                            if (null != __matchedtasks && __matchedtasks.Count > 0x00)
                            {
                                foreach (var __task in __matchedtasks)
                                {
                                    __result.data.Add(new Models.Task.result_query.cls_data()
                                    {
                                        billingno = __task.billingno,
                                        phonenum = __task.phonenum,
                                        amount = __task.amount,
                                        result = 0x00
                                    });
                                }
                                __result.ret = true;
                                __result.code = 0x00;
                                __result.msg = "查询成功";
                            }
                            else
                            {
                                __result.code = -0x01;
                                __result.msg = "查询失败：无匹配数据";
                            }
                        }
                        else {
                            EFCore.Contexts.TelupstreamStorages __storage = new EFCore.Contexts.TelupstreamStorages();
                            if (__storage.tasks.Any(t => 0x01 == 0x01 
                                && null != __billingnos && __billingnos.Length>0x00 ? __billingnos.Contains(t.billingno) : true
                                && !string.IsNullOrEmpty(phonenum) ? t.phonenum == phonenum : true)) {
                                var __tasks = __storage.tasks.Where(t => 0x01 == 0x01
                                    && null != __billingnos && __billingnos.Length > 0x00 ? __billingnos.Contains(t.billingno) : true
                                    && !string.IsNullOrEmpty(phonenum) ? t.phonenum == phonenum : true).ToList();
                                if (null != __tasks && __tasks.Count > 0x00)
                                {
                                    foreach(var __task in __tasks)
                                    {
                                        __result.data.Add(new Models.Task.result_query.cls_data() { 
                                            billingno = __task.billingno,
                                            phonenum = __task.phonenum,
                                            amount = __task.amount.ToString(),
                                            timestamp = __task.regtime.ToString("yyyyMMddHHmmssfff"),
                                            tradeno = __task.tradeno,
                                            result = __task.status
                                        });
                                    }
                                    __result.ret = true;
                                    __result.code = 0x00;
                                    __result.msg = "查询成功";
                                }
                                else
                                {
                                    __result.code = -0x01;
                                    __result.msg = "查询失败：无匹配数据";
                                }
                            }
                            else
                            {
                                __result.code = -0x01;
                                __result.msg = "查询失败：无匹配数据";
                            }
                        }

                    } else {
                        __result.code = -0x01;
                        __result.msg = "本次查询无效：签名数据异常";
                    }
                } else {
                    __result.code = -0x01;
                    __result.msg = "本次查询无效：billingno/tradeno/phonenum三者不能全部为空";
                }
            }
            else
            {
                __result.code = -0x01;
                __result.msg = "本次查询无效：未知的AppId";
            }

            return __result;
        }

        [HttpGet]
        [Route("intercept")]
        public Models.Task.result_intercept intercept(string appid, string billingno, string param, string sign) {
            Models.Task.result_intercept __result = new Models.Task.result_intercept() { ret = false, billingno = billingno };

            if (confs.settings.auths.Any(a => a.appid == appid)) {
                var __authinfo = confs.settings.auths.SingleOrDefault(s => s.appid == appid);
                Dictionary<string, string> __params = new Dictionary<string, string>();
                __params.Add("appid", appid);
                if (!string.IsNullOrEmpty(billingno)) __params.Add("billingno", billingno);
                if (!string.IsNullOrEmpty(param)) __params.Add("param", param);
                string __signstr = ServCore.Sign(__params, __authinfo.appkey);

                if (__signstr == sign)
                {
                    __result.status = 0x01;
                    __result.timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

                    __result.ret = true;
                    __result.code = 0x00;
                    __result.msg = "拦截成功";
                }
                else
                {
                    __result.ret = false;
                    __result.status = -0x01;
                    __result.timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                    __result.code = -0x01;
                    __result.msg = "本次拦截无效：签名数据异常";
                }
            }
            else
            {
                __result.code = -0x01;
                __result.msg = "新增充值任务失败：未知的AppId";
            }

            return __result;
        }


    }
}
