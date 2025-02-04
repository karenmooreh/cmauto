using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinacSDKCore.NET.Models
{
    public class ListCloudPhoneRequest : BaseRequest
    {
        private const string const_interactive_cmdstring = "ListCloudPhone";
        public ListCloudPhoneRequest() : base() {
            Action = const_interactive_cmdstring;
        }

        public int? PageNo { get; set; }
        public int? PageSize { get; set; }
        public int? IsEnable { get; set; }
        public string? CreateStartTime { get; set; }
        public string? CreateEndTime { get; set; }
        public string? ExpireStartTime { get; set; }
        public string? ExpireEndTime { get; set; }
        public string? Status { get; set; }
        public string? NeedInstallStatus { get; set; }
        public string? Ip { get; set; }
        public string? KeyWord { get; set; }
        public string? ImageId { get; set; }
        public string? CloudPhoneNetworkId { get; set; }
        public int? ProductModelId { get; set; }
        public string? NetworkType { get; set; }
        public string? ProductStatus { get; set; }
        public string? PayType { get; set; }
    }
}
