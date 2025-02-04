using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelupstreamAPI.EFCore.Models
{
    public class task
    {
        public string id { get; set; }
        public string tradeno { get; set; }
        public string appid { get; set; }
        public string billingno { get; set; }
        public string phonenum { get; set; }
        public double amount { get; set; }
        public string notifyuri { get; set; }
        public string param { get; set; }
        public int status { get; set; }
        public DateTime? proctime { get; set; }
        public DateTime regtime { get; set; }
    }
}
