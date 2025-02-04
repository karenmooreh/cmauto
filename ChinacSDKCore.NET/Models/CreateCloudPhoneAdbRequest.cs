using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinacSDKCore.NET.Models
{
    public class CreateCloudPhoneAdbRequest : BaseRequest
    {
        private const string const_interactive_cmdstring = "CreateCloudPhoneAdb";
        public CreateCloudPhoneAdbRequest() : base()
        {
            Action = const_interactive_cmdstring;
            CloudPhoneIds = new List<string>();
            Ips = new List<string>();
        }
        public string Region { get; set; }
        public List<string> CloudPhoneIds { get; set; }
        public List<string> Ips { get; set; }
    }
}
