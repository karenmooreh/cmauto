using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinacSDKCore.NET.Models
{
    public class GetCmdApiSignatureResponse : BaseResponse
    {
        public class Data
        {
            public string Signature { get; set; }
            public string RToken { get; set; }
            public string ApiUrl { get; set; }
        }

        public new Data data { get; set; }
    }
}
