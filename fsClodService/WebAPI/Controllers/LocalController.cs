using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fsClodService.WebAPI.Controllers
{
    [ApiController]
    [Route("local")]
    [EnableCors("FSVDMUMWEBAPISERV-CORSPOLICY-ALLOWDOMAINS")]
    public class LocalController : ControllerBase
    {
        [HttpGet]
        [Route("freshbillings")]
        public Models.models_notify.result_freshbillings freshbillings(int? stocknum)
        {
            Models.models_notify.result_freshbillings __result = new Models.models_notify.result_freshbillings();

            var __billings = ServCore.Singleton.GetUntaskBillings(stocknum);
            if(null != __billings && __billings.Count > 0x00)
            {
                __result.count = __billings.Count;
                foreach (var __billing in __billings)
                    __result.data.Add(new Models.models_notify.result_freshbillings.result_freshbilling() { 
                        billingno = __billing.no,
                        phonenum = __billing.phonenum,
                        amount = __billing.amount,
                        timestamp = __billing.timestamp,
                    });
            }
            __result.ret = true;

            return __result;
        }

        [HttpGet]
        [Route("taskresponse")]
        public Models.models_notify.result_taskresponse taskresponse(string billingno, string tradeno, int status)
        {
            Models.models_notify.result_taskresponse __result = new Models.models_notify.result_taskresponse();

            if (ServCore.Singleton.Histories.ContainsKey(billingno))
            {
                var __billing = ServCore.Singleton.Histories[billingno];
                __billing.tradeno = tradeno;
                __billing.status = status;
                __billing.timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                ServCore.Singleton.BillingNotifies.Enqueue(__billing.no);

                __result.ret = true;
                __result.code = 0x01;
                __result.msg = "success";
            }
            else
            {
                __result.ret = false;
                __result.code = -0x01;
                __result.msg = "billingno not exists.";
            }

            return __result;
        }
    }
}
