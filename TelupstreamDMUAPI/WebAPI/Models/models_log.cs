using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TelupstreamDMUAPI.WebAPI.Models
{
    public static class models_log
    {
        public class loglist_result : webapi_result {
            public class cls_logdata
            {
                public string id { get; set; }
                public string source { get; set; }
                public int type { get; set; }
                public string intro { get; set; }
                public string details { get; set; }
                public string regtime { get; set; }
            }
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public new List<cls_logdata> data { get; set; }

            public int pageindex { get; set; }
            public int pagesize { get; set; }
            public int pagecount { get; set; }
            public int records { get; set; }
            public string timestamp { get; set; }

            public loglist_result()
            {
                this.data = new List<cls_logdata>();
            }
        }
    }
}
