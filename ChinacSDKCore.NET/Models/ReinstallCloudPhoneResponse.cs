using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinacSDKCore.NET.Models
{
    public class ReinstallCloudPhoneResponse : BaseResponse
    {
        public ReinstallCloudPhoneResponse() : base() { }

        public class Data
        {
            public string TaskId { get; set; }
        }

        public new Data data { get; set; }
    }
}
