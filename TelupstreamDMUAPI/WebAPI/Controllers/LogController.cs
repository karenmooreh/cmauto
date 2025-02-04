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
    [Route("log")]
    [EnableCors("DMUMWEBAPISERV-CORSPOLICY-ALLOWDOMAINS")]
    public class LogController : ControllerBase
    {
        [HttpGet]
        [Route("list")]
        public Models.models_log.loglist_result list(int? pageindex, int? pagesize,
            string? source, int? sourcetype, string? intro, string? details)
        {
            Models.models_log.loglist_result __result = new Models.models_log.loglist_result();

            DateTime __datetime_tryparseobject;

            EFCore.Contexts.MainContext __mcnt = new EFCore.Contexts.MainContext();

            Expression<Func<EFCore.Models.log, bool>> __queryexpress = (t => 0x01 == 0x01
                && (!string.IsNullOrEmpty(source) ? t.source.Contains(source) : true)
                && (sourcetype.HasValue ? t.type == sourcetype : true)
                && (!string.IsNullOrEmpty(intro) ? t.intro.Contains(intro) : true)
                && (!string.IsNullOrEmpty(details) ? t.details.Contains(details) : true)
            );

            __result.pagesize = pagesize.HasValue ? pagesize.Value : confs.settings.common.datalist.sizeperpage;
            __result.records = __mcnt.logs.Count(__queryexpress);
            __result.pagecount = __result.records / __result.pagesize +
                (__result.records % __result.pagesize > 0x00 ? 0x01 : 0x00);
            __result.pageindex = pageindex.HasValue ?
                (pageindex.Value < 0x01 ? 0x01 :
                pageindex.Value >= __result.records ? __result.pagecount : pageindex.Value)
                : 0x01;

            var __queryresult = __mcnt.logs.Where(__queryexpress)
                .OrderByDescending(t => t.regtime)
                .Skip((__result.pageindex - 0x01) * __result.pagesize).Take(__result.pagesize).ToList();

            if(null != __queryresult)
            {
                foreach(var __record in __queryresult)
                {
                    Models.models_log.loglist_result.cls_logdata __log = new Models.models_log.loglist_result.cls_logdata() { 
                        id = __record.id,
                        source = __record.source,
                        type = __record.type,
                        intro = __record.intro,
                        details = __record.details,
                        regtime = __record.regtime.ToString()
                    };

                    __result.data.Add(__log);
                }
            }

            __result.result = true;
            __result.timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            return __result;
        }
    }
}
