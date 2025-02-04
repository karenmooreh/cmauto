using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace fsClodService.WebAPI.Controllers
{
    [ApiController]
    [Route("task")]
    [EnableCors("FSVDMUMWEBAPISERV-CORSPOLICY-ALLOWDOMAINS")]
    public class TaskController : ControllerBase
    {

        [HttpGet]
        [Route("add")]
        public Models.models_task.result_add add(string appid, string billingno, string phonenum, double amount, string? notify_uri, string? param, string sign)
        {
            Models.models_task.result_add __result = new Models.models_task.result_add();

            if (appid == ServCore.const_appid)
            {
                Dictionary<string, string> __params = new Dictionary<string, string>() {
                    { "appid", appid },
                    { "billingno", billingno },
                    { "phonenum", phonenum },
                    { "amount", amount.ToString() },
                    { "notify_uri", notify_uri },
                    { "param", param },
                };

                string __signstr = ServCore.Sign(__params, ServCore.const_appkey);
                if(__signstr == sign) {
                    if(ServCore.Singleton.BillingEnqueue(billingno, phonenum, amount, notify_uri, param) == 0x01)
                    {
                        __result.ret = true;
                        __result.code = 0x00;
                        __result.msg = "订单提交成功，已加入任务队列";
                    }
                    else
                    {
                        __result.ret = false;
                        __result.code = -0x01;
                        __result.msg = "该订单已在本系统任务队列中存在，请不要重复提交";
                    }
                }
                else
                {
                    __result.ret = false;
                    __result.code = -0x01;
                    __result.msg = "签名数据异常，请检查后重试";
                }
            }
            else
            {
                __result.ret = false;
                __result.code = -0x01;
                __result.msg = "未知的应用id，请检查后重试";
            }

            return __result;
        }

        [HttpGet]
        [Route("query")]
        public Models.models_task.result_query query(string appid, string billingno, string? param, string sign)
        {
            Models.models_task.result_query __result = new Models.models_task.result_query();

            if (appid == ServCore.const_appid)
            {

                Dictionary<string, string> __params = new Dictionary<string, string>() {
                    { "appid", appid },
                    { "billingno", billingno },
                    { "param", param },
                };
                string __signstr = ServCore.Sign(__params, ServCore.const_appkey);
                if (__signstr == sign)
                {
                    if(ServCore.Singleton.Billings.Any(b=>b.no == billingno) || ServCore.Singleton.Histories.ContainsKey(billingno))
                    {
                        ServCore.billing __billing = ServCore.Singleton.Billings.Any(b => b.no == billingno) ?
                            ServCore.Singleton.Billings.SingleOrDefault(b => b.no == billingno) :
                            ServCore.Singleton.Histories[billingno];
                        __result.ret = true;
                        __result.data.Add(new Models.models_task.result_query.result_query_data() { 
                            billingno = __billing.no,
                            amount = __billing.amount,
                            phonenum = __billing.phonenum,
                            tradeno = __billing.tradeno,
                            status = __billing.status,
                            timestamp = __billing.timestamp,
                            code = 0x00
                        });
                        __result.code = 0x00;
                        __result.msg = "success";
                    }
                    else
                    {
                        __result.ret = false;
                        __result.code = -0x01;
                        __result.msg = "未查询到订单信息";
                    }
                }
                else
                {
                    __result.ret = false;
                    __result.code = -0x01;
                    __result.msg = "签名数据异常，请检查后重试";
                }
            }
            else
            {
                __result.ret = false;
                __result.code = -0x01;
                __result.msg = "未知的应用id，请检查后重试";
            }

            return __result;
        }


    }
}
