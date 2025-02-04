using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelupstreamDMUAPI.Logger
{
    public partial class Logger
    {
        public enum logtype
        {
            system = 0x00,
            device = 0x01,
            phonenum = 0x02,
            taskphonenum = 0x03,
            other = 0xff
        }
        public class log
        {
            public string id { get; set; }
            public string source { get; set; }
            public logtype type { get; set; }
            public string intro { get; set; }
            public string details { get; set; }
            public bool isdisplay { get; set; }
            public bool isstorage { get; set; }
            public DateTime regtime { get; set; }

            public log(string intro, string details, logtype type, string source = null, bool isdisplay = true, bool isstorage = true)
            {
                this.id = Guid.NewGuid().ToString("N");
                this.source = source;
                this.intro = intro;
                this.details = details;
                this.type = type;
                this.isdisplay = isdisplay;
                this.isstorage = isstorage;
                this.regtime = DateTime.Now;
            }
        }

        static Logger() => __static_constructor_Logger();

        public static void Log(log logdata) => __log(logdata);
        public static void Log(string intro, string details, logtype type, string source = null, bool isdisplay = true, bool isstorage = true)
            => __log(new log(intro, details, type, source, isdisplay, isstorage));
    }
}
