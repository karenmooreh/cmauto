using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelupstreamDMUAPI.WebAPI.Models
{
    public static class models_auth
    {
        public class signin_request
        {
            public string username { get; set; }
            public string password { get; set; }
        }

        public class signin_result : webapi_result
        {
            public new string data { get; set; }
        }
    }
}
