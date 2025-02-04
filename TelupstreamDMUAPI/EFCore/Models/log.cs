using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelupstreamDMUAPI.EFCore.Models
{
    public class log
    {
        public string id { get; set; }
        public string source { get; set; }
        public int type { get; set; }
        public string intro { get; set; }
        public string details { get; set; }
        public DateTime regtime { get; set; }
    }
}
