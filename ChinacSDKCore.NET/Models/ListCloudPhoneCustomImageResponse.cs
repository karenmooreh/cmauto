using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinacSDKCore.NET.Models
{
    public class ListCloudPhoneCustomImageResponse : BaseResponse
    {
        public ListCloudPhoneCustomImageResponse() : base() { }

        public class Data
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
                public string Uuid { get; set; }
                public string Name { get; set; }
                public string RootImageId { get; set; }
                public string RootImageName { get; set; }
                public List<int> ProductModelIds { get; set; }
                public int Storage { get; set; }
                public int IsRoot { get; set; }
                public string Description { get; set; }
                public string Status { get; set; }
                public long CreateTime { get; set; }
                public long UpdateTime { get; set; }
            }
            public PageObject Page { get; set; }
            public List<ListElement> List { get; set; }
        }

        public new Data data { get; set; }
    }
}
