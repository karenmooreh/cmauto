using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelupstreamDMUAPI.EFCore.Models
{
    public class task
    {
        public string id { get; set; }
        public string task_id { get; set; }
        public string billingno { get; set; }
        public DateTime? accepttime { get; set; }
        public string topup_phonenum { get; set; }
        public double topup_amount { get; set; }
        public string? deviceid { get; set; }
        public string? devicesn { get; set; }
        public string? task_phonenum { get; set; }
        public string? isp_tradeno { get; set; }
        public int task_status { get; set; }
        public DateTime? task_runtime { get; set; }
        public DateTime regtime { get; set; }
    }
}
