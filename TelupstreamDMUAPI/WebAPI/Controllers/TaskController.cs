using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TelupstreamDMUAPI.WebAPI.Controllers
{
    [ApiController]
    [Route("task")]
    [EnableCors("DMUMWEBAPISERV-CORSPOLICY-ALLOWDOMAINS")]
    public class TaskController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("list")]
        public Models.models_task.tasklist_result list(int? pageindex, int? pagesize, string? billingno,
            string? accepttime_start, string? accepttime_end, string? topup_phonenum, double? topup_amount,
            string? task_id, string? task_phonenum, string? devicesn, int? task_status, string? isp_tradeno,
            string? task_runtime_start, string? task_runtime_end) {
            Models.models_task.tasklist_result __result = new Models.models_task.tasklist_result() { result = false };

            DateTime __datetime_tryparseobject;

            EFCore.Contexts.MainContext __mcnt = new EFCore.Contexts.MainContext();

            Expression<Func<EFCore.Models.task, bool>> __queryexpress = (t => 0x01 == 0x01
                && (!string.IsNullOrEmpty(billingno) ? t.billingno == billingno : true)
                && (!string.IsNullOrEmpty(accepttime_start) && DateTime.TryParse(accepttime_start, out __datetime_tryparseobject) ? t.accepttime >= DateTime.Parse(accepttime_start) : true)
                && (!string.IsNullOrEmpty(accepttime_end) && DateTime.TryParse(accepttime_end, out __datetime_tryparseobject) ? t.accepttime <= DateTime.Parse(accepttime_end) : true)
                && (!string.IsNullOrEmpty(topup_phonenum) ? t.topup_phonenum.Contains(topup_phonenum) : true)
                && (topup_amount.HasValue ? t.topup_amount == topup_amount.Value : true)
                && (!string.IsNullOrEmpty(task_id) ? t.task_id.Contains(task_id) : true)
                && (!string.IsNullOrEmpty(task_phonenum) ? t.task_phonenum.Contains(task_phonenum) : true)
                && (!string.IsNullOrEmpty(devicesn) ? t.devicesn.Contains(devicesn) : true)
                && (task_status.HasValue && task_status.Value >= 0x00 ? t.task_status == task_status.Value : true)
                && (!string.IsNullOrEmpty(isp_tradeno) ? t.isp_tradeno.Contains(isp_tradeno) : true)
                && (!string.IsNullOrEmpty(task_runtime_start) && DateTime.TryParse(task_runtime_start, out __datetime_tryparseobject) ? t.task_runtime >= DateTime.Parse(task_runtime_start) : true)
                && (!string.IsNullOrEmpty(task_runtime_end) && DateTime.TryParse(task_runtime_end, out __datetime_tryparseobject) ? t.task_runtime <= DateTime.Parse(task_runtime_end) : true)
            );

            

            __result.pagesize = pagesize.HasValue ? pagesize.Value : confs.settings.common.datalist.sizeperpage;
            __result.records = __mcnt.tasks.Count(__queryexpress);
            __result.pagecount = __result.records / __result.pagesize +
                (__result.records % __result.pagesize > 0x00 ? 0x01 : 0x00);
            __result.pageindex = pageindex.HasValue ? 
                (pageindex.Value < 0x01 ? 0x01 : 
                pageindex.Value >= __result.records ? __result.pagecount : pageindex.Value)
                : 0x01;

            var __queryresult = __mcnt.tasks.Where(__queryexpress)
                .OrderByDescending(t => t.regtime)
                .Skip((__result.pageindex - 0x01) * __result.pagesize).Take(__result.pagesize).ToList();

            if(null != __queryresult)
            {
                foreach(var __record in __queryresult)
                {
                    Models.models_task.tasklist_result.cls_taskdata __redata = new Models.models_task.tasklist_result.cls_taskdata() {
                        id = __record.id,
                        billingno = __record.billingno,
                        accepttime = __record.accepttime.HasValue ? __record.accepttime.Value.ToString() : string.Empty,
                        topup_phonenum = __record.topup_phonenum,
                        topup_amount = (float)(Math.Round(__record.topup_amount, 0x02, MidpointRounding.AwayFromZero)),
                        task_id = !string.IsNullOrEmpty(__record.task_id) ? __record.task_id : string.Empty,
                        deviceid = !string.IsNullOrEmpty(__record.deviceid) ? __record.deviceid : string.Empty,
                        devicesn = !string.IsNullOrEmpty(__record.devicesn) ? __record.devicesn : string.Empty,
                        task_phonenum = !string.IsNullOrEmpty(__record.task_phonenum) ? __record.task_phonenum: string.Empty,
                        task_status = __record.task_status,
                        isp_tradeno = !string.IsNullOrEmpty(__record.isp_tradeno) ? __record.isp_tradeno : string.Empty,
                        task_runtime = __record.task_runtime.HasValue ? __record.task_runtime.Value.ToString() : string.Empty
                    };
                    __result.data.Add(__redata);
                }

            }

            __result.result = true;

            return __result;
        }
    }
}
