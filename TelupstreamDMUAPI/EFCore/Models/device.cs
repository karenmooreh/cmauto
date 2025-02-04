using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelupstreamDMUAPI.EFCore.Models
{
    public class device
    {
        public string id { get; set; }
        public string sn { get; set; }
        public string? tag { get; set; }
        public int todaycount { get; set; }
        public DateTime? worktime_checkin { get; set; }
        public DateTime? worktime_checkout { get; set; }
        public int status { get; set; }
        public DateTime regtime { get; set; }
    }
}
