using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fsLocalService.models
{
    public class task
    {
        public string tasktradeno { get; set; }
        public string billingno { get; set; }
        public string phonenum { get; set; }
        public double amount { get; set; }
        public string isptradeno { get; set; }
        public int status { get; set; }
        public string timestamp { get; set; }
        public string finishedtimestamp { get; set; }
    }
}
