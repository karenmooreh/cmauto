using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fsClodService.WebAPI.Models
{
    public class models_notify
    {
        public class result_freshbillings
        {
            public class result_freshbilling
            {
                public string billingno { get; set; }
                public string phonenum { get; set; }
                public double amount { get; set; }
                public string timestamp { get; set; }
            }

            public bool ret { get; set; }
            public int count { get; set; }
            public List<result_freshbilling> data { get; set; }
            public int code { get; set; }
            public string msg { get; set; }

            public result_freshbillings()
            {
                this.data = new List<result_freshbilling>();
            }
        }

        public class result_taskresponse
        {
            public bool ret { get; set; }
            public int code { get; set; }
            public string msg { get; set; }
        }
    }
}
