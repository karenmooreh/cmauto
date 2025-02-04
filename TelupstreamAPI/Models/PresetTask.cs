using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelupstreamAPI.Models
{
    internal class PresetTask
    {
        public string appid { get; set; }
        public string appkey { get; set; }
        public string billingno { get; set; }
        public string phonenum { get; set; }
        public string amount { get; set; }
        public string param { get; set; }
        public string notifyuri { get; set; }
    }
}
