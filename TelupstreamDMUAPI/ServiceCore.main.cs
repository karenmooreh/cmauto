using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelupstreamDMUAPI
{
    public partial class ServiceCore
    {
        public ServiceCore() => __constructor_ServiceCore();

        public static ServiceCore Singleton => __singleton;
        public bool Status => __status;

        public void Start() => __start();



    }
}
