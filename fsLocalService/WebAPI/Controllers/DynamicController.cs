using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fsLocalService.WebAPI.Controllers
{
    [ApiController]
    [EnableCors("ISPWEBAPISERV-CORSPOLICY-ALLOWDOMAINS")]
    [Route("dynamic")]
    public class DynamicController : ControllerBase
    {
        [HttpGet]
        [Route("tasksrealtimehistory")]
        public string tasksrealtimehistory()
        {
            string __result = string.Empty;

            StringBuilder __hbuilder = new StringBuilder();
            foreach(var __task in ServCore.Singleton.FinishedTasks)
            {
                __hbuilder.AppendLine($",{{\"billingno\": \"{__task.Value.billingno}\", \"phonenum\": \"{__task.Value.phonenum}\", \"amount\":\"{__task.Value.amount.ToString()}\", \"finishedtime\": \"{__task.Value.finishedtimestamp}\"}}");
            }
            if (__hbuilder.Length > 0x00) __hbuilder.Remove(0x00, 0x01);
            __hbuilder.AppendLine("]");
            __hbuilder.Insert(0x00,"[");
            __result = __hbuilder.ToString();

            return __result;
        }

        [HttpGet]
        [Route("test")]
        public string test()
        {
            return "success";
        }
    }
}
