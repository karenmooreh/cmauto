using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinacSDKCore.NET.Models
{
    public class ReinstallCloudPhoneRequest : BaseRequest
    {
        private const string const_interactive_cmdstring = "ReinstallCloudPhone";

        public class CloudPhone
        {
            public string Region { get; set; }
            public string Id { get; set; }
        }

        public ReinstallCloudPhoneRequest() : base()
        {
            Action = const_interactive_cmdstring;
        }

        public List<CloudPhone> CloudPhones { get; set; }
        public string ImageId { get; set; }
        public string Remark { get; set; }
        public string ParamType { get; set; }
    }
}
