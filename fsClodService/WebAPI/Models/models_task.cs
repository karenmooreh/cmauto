using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fsClodService.WebAPI.Models
{
    public class models_task
    {
        public class result_add
        {
            public bool ret { get; set; }
            public int code { get; set; }
            public string msg { get; set; }
        }

        public class result_query
        {
            public class result_query_data
            {
                public string billingno { get; set; }
                public string tradeno { get; set; }
                public string phonenum { get; set; }
                public double amount { get; set; }
                public int status { get; set; }
                public int code { get; set; }
                public string timestamp { get; set; }
            }

            public bool ret { get; set; }
            public List<result_query_data> data { get; set; }
            public int code { get; set; }
            public string msg { get; set; }

            public result_query()
            {
                this.data = new List<result_query_data>();
            }
        }
    }
}
