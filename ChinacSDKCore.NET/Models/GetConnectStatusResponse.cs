using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinacSDKCore.NET.Models
{
    public class GetConnectStatusResponse : BaseData
    {
        public class Data
        {
            public class StatusInfo
            {
                public string Id { get; set; }
                public string ConnectStatus { get; set; }
            }

            public List<StatusInfo> StatusInfos { get; set; }

            public Data()
            {
                this.StatusInfos = new List<StatusInfo>();
            }
        }

        public Data ResponseData { get; set; }
        public string ResponseTaskId { get; set; }
        public new string ResponseDate { get; set; }
    }
}
