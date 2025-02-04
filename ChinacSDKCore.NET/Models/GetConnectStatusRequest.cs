using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinacSDKCore.NET.Models
{
    public class GetConnectStatusRequest : BaseRequest
    {
        private const string const_interactive_cmdstring = "getConnectStatus";

        public GetConnectStatusRequest() : base() {
            Action = const_interactive_cmdstring;
            Ids = new List<string>();
        }

        public string RToken { get; set; }
        public List<string> Ids { get; set; }
    }
}
