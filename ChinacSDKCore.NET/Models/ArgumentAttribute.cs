using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinacSDKCore.NET.Models
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Property)]
    public class ArgumentAttribute : Attribute
    {
        public enum Type
        {
            Shared = 0x01,
            Uniqued = 0x02
        }
        public ArgumentAttribute(Type type)
        {
            __type = type;
        }
        public Type type => __type;
        private Type __type;
    }
}
