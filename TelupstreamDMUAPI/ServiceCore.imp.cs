using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenNetworkPlatform.cores.services.webapi;

namespace TelupstreamDMUAPI
{
    public partial class ServiceCore
    {
        private void __constructor_ServiceCore() {
            __singleton = this;
        }

        private void __start()
        {
            if (!__status) {
                __status = true;
                Logger.Logger.Log("系统启动", "系统主进程正在启动，请稍候...",
                    Logger.Logger.logtype.system, ServiceCore.CONST_LOGTARGET_SYSTEM0);

                __serv_webapi = new IntegratedWebAPIService<ServiceCore>();
                __serv_webapi.Start();
                Logger.Logger.Log("WEBAPI服务启动", "WEBAPI服务初始化完成",
                    Logger.Logger.logtype.system, ServiceCore.CONST_LOGTARGET_SYSTEM0);

                __serv_adbserv = new AdbService.AdbManagedService();
                __serv_adbserv.Start();
                Logger.Logger.Log("托管ADB服务启动", "托管ADB服务初始化完成",
                    Logger.Logger.logtype.system, ServiceCore.CONST_LOGTARGET_SYSTEM0);

                Logger.Logger.Log("系统主进程启动", "系统主进程启动初始化完成", 
                    Logger.Logger.logtype.system, ServiceCore.CONST_LOGTARGET_SYSTEM0);
            }
        }
    }
}
