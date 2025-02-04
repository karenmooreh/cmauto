using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinacSDKCore.NET.Models
{
    public class ListCloudPhoneCustomImageRequest : BaseRequest
    {
        private const string const_interactive_cmdstring = "ListCloudPhoneCustomImage";

        public ListCloudPhoneCustomImageRequest() : base() {
            Action = const_interactive_cmdstring;
        }

        public string Region { get; set; }
        public string Key { get; set; }
        public string RootImageId { get; set; }
        public string Status { get; set; }
        public long? CreateStartTime { get; set; }
        public long? CreateEndTime { get; set; }
        public int? PageNo { get; set; }
        public int? PageSize { get; set; }
        public List<string>? ProductModelIds { get; set; }
    }
}
