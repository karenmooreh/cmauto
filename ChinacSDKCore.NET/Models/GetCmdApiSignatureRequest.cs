using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinacSDKCore.NET.Models
{
    public class GetCmdApiSignatureRequest : BaseRequest
    {
        private const string const_interactive_cmdstring = "GetCmdApiSignature";

        public GetCmdApiSignatureRequest() : base()
        {
            Action = const_interactive_cmdstring;
        }

        public List<string> Ids { get; set; }
        public string Region { get; set; }
    }
}
