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
    [Route("device")]
    [EnableCors("DMUMWEBAPISERV-CORSPOLICY-ALLOWDOMAINS")]
    public class DeviceController : ControllerBase
    {
        [HttpGet]
        [Route("list")]
        public Models.models_device.devicelist_result list(int? pageindex, int? pagesize,
            string? devicesn, string? tag, int? status)
        {
            Models.models_device.devicelist_result __result = new Models.models_device.devicelist_result();

            DateTime __datetime_tryparseobject;

            EFCore.Contexts.MainContext __mcnt = new EFCore.Contexts.MainContext();

            Expression<Func<EFCore.Models.device, bool>> __queryexpress = (t => 0x01 == 0x01
                && (!string.IsNullOrEmpty(devicesn) ? t.sn.Contains(devicesn) : true)
                && (!string.IsNullOrEmpty(tag) ? t.tag.Contains(tag) : true)
                && (status.HasValue && status.Value >= 0x00 ? t.status == status.Value : true)
            );

            __result.pagesize = pagesize.HasValue ? pagesize.Value : confs.settings.common.datalist.sizeperpage;
            __result.records = __mcnt.devices.Count(__queryexpress);
            __result.pagecount = __result.records / __result.pagesize +
                (__result.records % __result.pagesize > 0x00 ? 0x01 : 0x00);
            __result.pageindex = pageindex.HasValue ?
                (pageindex.Value < 0x01 ? 0x01 :
                pageindex.Value >= __result.records ? __result.pagecount : pageindex.Value)
                : 0x01;

            var __queryresult = __mcnt.devices.Where(__queryexpress)
                .OrderByDescending(t => t.regtime)
                .Skip((__result.pageindex - 0x01) * __result.pagesize).Take(__result.pagesize).ToList();

            if(null != __queryresult)
            {
                foreach(var __record in __queryresult)
                {
                    Models.models_device.devicelist_result.cls_devicedata __device = new Models.models_device.devicelist_result.cls_devicedata() { 
                        id = __record.id,
                        sn = __record.sn,
                        tag = !string.IsNullOrEmpty(__record.tag) ? __record.tag : string.Empty,
                        todaycount = __record.todaycount,
                        worktime_checkin = __record.worktime_checkin.HasValue ? 
                            __record.worktime_checkin.Value.ToString() : string.Empty,
                        worktime_checkout = __record.worktime_checkout.HasValue ?
                            __record.worktime_checkout.Value.ToString() : string.Empty,
                        status = __record.status,
                        regtime = __record.regtime.ToString()
                    };
                    __result.data.Add(__device);
                }
            }

            __result.result = true;

            return __result;
        }
    }
}
