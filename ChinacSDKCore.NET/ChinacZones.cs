using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinacSDKCore.NET
{
    public static class ChinacZones
    {
        /// <summary>
        /// 云主机业务可用区域
        /// </summary>
        public static class CloudHost
        {
            /// <summary>
            /// 华东/苏州一区/BGP
            /// </summary>
            public static string cn_suzhou1 => "cn-suzhou1";
            /// <summary>
            /// 华东江苏一区/电信
            /// </summary>
            public static string cn_huaian => "cn-huaian";
            /// <summary>
            /// 华东/上海一区/cn-shzj
            /// </summary>
            public static string cn_shzj => "cn-shzj";
            /// <summary>
            /// 华北/北京一区/BGP
            /// </summary>
            public static string cn_beijing1 => "cn-beijing1";
            /// <summary>
            /// 华南/福建一区/电信
            /// </summary>
            public static string cn_anxi => "cn-anxi";
            /// <summary>
            /// 华南/深圳一区/BGP、电信
            /// </summary>
            public static string cn_sz_meisheng => "cn-sz-meisheng";
            /// <summary>
            /// 香港/香港一区/国际、香港BGP
            /// </summary>
            public static string cn_hkcmi => "cn-hkcmi";
            /// <summary>
            /// 香港/香港三区/国际、香港BGP
            /// </summary>
            public static string cn_hk3 => "cn-hk3";
            /// <summary>
            /// 海外/硅谷一区/国际
            /// </summary>
            public static string cn_us => "cn-us";
        }

        /// <summary>
        /// 云手机业务可用区域
        /// </summary>
        public static class CloudMobile
        {
            /// <summary>
            /// 华东/江苏二区/电信
            /// </summary>
            public static string cn_jsha_cloudphone_2 => $"cn-jsha-cloudphone-2";
            /// <summary>
            /// 华东/江苏三区/电信
            /// </summary>
            public static string cn_jsha_cloudphone_3 => $"cn-jsha-cloudphone-3";
            /// <summary>
            /// 华东/福建一区/电信
            /// </summary>
            public static string cn_fjqz_cloudphone => $"cn-fjqz-cloudphone";
            /// <summary>
            /// 华南/深圳/电信
            /// </summary>
            public static string cn_szyh_cloudphone => $"cn-szyh-cloudphone";
            /// <summary>
            /// 香港/香港一区/国际
            /// </summary>
            public static string cn_hk_cloudphone => $"cn-hk-cloudphone";
            /// <summary>
            /// 香港/香港二区/国际
            /// </summary>
            public static string cn_hk_cloudphone_2 => $"cn-hk-cloudphone-2";
            /// <summary>
            /// 香港/香港三区/国际
            /// </summary>
            public static string cn_hk_cloudphone_3 => $"cn-hk-cloudphone-3";
            /// <summary>
            /// 美国/美国一区/国际
            /// </summary>
            public static string cn_us_cloudphone => $"cn-us-cloudphone";
        }
    }
}
