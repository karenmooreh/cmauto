using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinacSDKCore.NET.Models
{
    public class StartAppRequest : BaseRequest
    {
        private const string const_interactive_cmdstring = "startApp";

        public StartAppRequest() : base() {
            Action = const_interactive_cmdstring;
        }

        public string RToken { get; set; }
        public string Id { get; set; }
        public string PackageName { get; set; }
    }
}
