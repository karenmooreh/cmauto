using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinacSDKCore.NET.Models
{
    public class ListCloudPhoneResponse : BaseResponse
    {
        public ListCloudPhoneResponse() : base() { }

        public class Data : BaseData
        {
            public class PageObject
            {
                public int PageSize { get; set; }
                public int CurrentPageNo { get; set; }
                public int TotalPage { get; set; }
                public int TotalCount { get; set; }
            }

            public class ListElement
            {
                public string Id { get; set; }
                public string Name { get; set; }
                public string Region { get; set; }
                public string PayType { get; set; }
                public string TaskStatus { get; set; }
                public int IsEnable { get; set; }
                public string CloudPhoneNetworkId { get; set; }
                public string NetworkType { get; set; }
                public string Eip { get; set; }
                public string IntranetIp { get; set; }
                public string Status { get; set; }
                public string ProductStatus { get; set; }
                public int ProductModelId { get; set; }
                public string ProductModelName { get; set; }
                public string DisplaySize { get; set; }
                public string ProductType { get; set; }
                public long LastStartTime { get; set; }
                public long ShutdownTime { get; set; }
                public long DueTime { get; set; }
                public long CloseTime { get; set; }
                public long CreateTime { get; set; }
                public long UpdateTime { get; set; }
                public string AdbHostPort { get; set; }
                public string AdbStatus { get; set; }
            }

            public PageObject Page { get; set; }
            public List<ListElement> List { get; set; }

            public Data()
            {
                this.List = new List<ListElement>();
            }
        }

        public new Data data { get; set; }
    }
}
