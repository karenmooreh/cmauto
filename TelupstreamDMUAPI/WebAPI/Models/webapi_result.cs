using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TelupstreamDMUAPI.WebAPI.Models
{
    public class webapi_result
    {
        public bool result { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public virtual object data { get; set; }
        public int code { get; set; }
        public string message { get; set; }
    }
}
