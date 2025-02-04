using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinacSDKCore.NET.Models
{
    public class BaseResponse
    {
        public BaseResponse() { }

        public int code { get; set; }
        public object? data { get; set; }
        public string message { get; set; }
    }
}
