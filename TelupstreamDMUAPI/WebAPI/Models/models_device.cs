using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TelupstreamDMUAPI.WebAPI.Models
{
    public static class models_device
    {
        public class devicelist_result : webapi_result
        {
            public class cls_devicedata
            {
                public string id { get; set; }
                public string sn { get; set; }
                public string tag { get; set; }
                public int todaycount { get; set; }
                public string worktime_checkin { get; set; }
                public string worktime_checkout { get; set; }
                public int status { get; set; }
                public string regtime { get; set; }
            }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public new List<cls_devicedata> data { get; set; }

            public int pageindex { get; set; }
            public int pagesize { get; set; }
            public int pagecount { get; set; }
            public int records { get; set; }

            public devicelist_result()
            {
                this.data = new List<cls_devicedata>();
            }
        }
    }
}
