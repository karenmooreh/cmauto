using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TelupstreamDMUAPI.WebAPI.Models
{
    public static class models_task
    {
        public class tasklist_result : webapi_result
        {
            public class cls_taskdata
            {
                public string id { get; set; }
                public string billingno { get; set; }
                public string accepttime { get; set; }
                public string topup_phonenum { get; set; }
                public float topup_amount { get; set; }
                public string task_id { get; set; }
                public string deviceid { get; set; }
                public string devicesn { get; set; }
                public string task_phonenum { get; set; }
                public int task_status { get; set; }
                public string isp_tradeno { get; set; }
                public string task_runtime { get; set; }
            }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public new List<cls_taskdata> data { get; set; }

            public int pageindex { get; set; }
            public int pagesize { get; set; }
            public int pagecount { get; set; }
            public int records { get; set; }

            public tasklist_result()
            {
                this.data = new List<cls_taskdata>();
            }
        }
    }
}
