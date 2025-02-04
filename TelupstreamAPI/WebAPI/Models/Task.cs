using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TelupstreamAPI.WebAPI.Models
{
    public class Task
    {
        public class result_add
        {
            public bool ret { get; set; }
            public int code { get; set; }
            public string msg { get; set; }
        }

        public class result_query
        {
            public class cls_data
            {
                public string billingno { get; set; }
                [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
                public string tradeno { get; set; }
                public string phonenum { get; set; }
                public string amount { get; set; }
                public int result { get; set; }
                [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]
                public string timestamp { get; set; }
            }
            public bool ret { get; set; }
            public List<cls_data> data { get; set; }
            public int code { get; set; }
            public string msg { get; set; }

            public result_query()
            {
                this.data = new List<cls_data>();
            }
        }

        public class result_intercept
        {
            public bool ret { get; set; }
            public string billingno { get; set; }
            public int status { get; set; }
            public string timestamp { get; set; }
            public int code { get; set; }
            public string msg { get; set; }
        }
    }
}
