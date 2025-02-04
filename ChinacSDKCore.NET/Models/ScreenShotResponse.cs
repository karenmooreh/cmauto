using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinacSDKCore.NET.Models
{
    public class ScreenShotResponse : BaseData
    {
        public class Data
        {
            public string Img { get;set; }
        }

        public Data ResponseData { get; set; }
        public new string ResponseDate { get; set; }
    }
}
