using ChinacSDKCore.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinacSDKCore.NET
{
    public partial class ChinacCloudMobileClient
    {
        public ChinacCloudMobileClient(string APIKeyId, string APIKeySecret)
            => __constructor_ChinacCloudMobileClient(APIKeyId, APIKeySecret);

        public string Signature(string Method, Dictionary<string, string> RequestParams, string APIKeyId, string APIKeySecret)
            => __signature(Method, RequestParams, APIKeyId, APIKeySecret);

        public Models.ListCloudPhoneResponse ListCloudPhone(Models.ListCloudPhoneRequest Request)
            => __api_listCloudPhone(Request);

        public Models.DescribeCloudPhoneResponse DescribeCloudPhone(Models.DescribeCloudPhoneRequest Request)
            => __api_describeCloudPhone(Request);

        public Models.CreateCloudPhoneAdbResponse CreateCloudPhoneAdb(Models.CreateCloudPhoneAdbRequest Request)
            => __api_createCloudPhoneAdb(Request);

        public Models.CloseCloudPhoneAdbResponse CloseCloudPhoneAdb(Models.CloseCloudPhoneAdbRequest Request)
            => __api_closeCloudPhoneAdb(Request);

        public Models.ListCloudPhoneAdbWhiteIpResponse ListCloudPhoneAdbWhiteIp(Models.ListCloudPhoneAdbWhiteIpRequest Request)
            => __api_listCloudPhoneAdbWhiteIp(Request);

        public Models.SetCloudPhoneAdbWhiteIpResponse SetCloudPhoneAdbWhiteIp(Models.SetCloudPhoneAdbWhiteIpRequest Request)
            => __api_setCloudPhoneAdbWhiteIp(Request);

        public Models.ReinstallCloudPhoneResponse ReinstallCloudPhone(Models.ReinstallCloudPhoneRequest Request)
            => __api_reinstallCloudPhone(Request);

        public Models.GetCmdApiSignatureResponse GetCmdApiSignature(Models.GetCmdApiSignatureRequest Request)
            => __api_getCmdApiSignature(Request);

        public Models.GetConnectStatusResponse GetConnectStatus(Models.GetConnectStatusRequest Request, 
            string GeneratedApiUri, string GeneratedSignature)
            => __api_getConnectStatus(Request, GeneratedApiUri, GeneratedSignature);

        public Models.ScreenShotResponse ScreenShot(Models.ScreenShotRequest Request, string GeneratedApiUri, string GeneratedSignature)
            => __api_screenShot(Request, GeneratedApiUri, GeneratedSignature);

        public Models.SendShellCmdResponse SendShellCmd(Models.SendShellCmdRequest Request)
            => __api_sendShellCmd(Request);

        public Models.StartAppResponse StartApp(Models.StartAppRequest Request, string GeneratedApiUri, string GeneratedSignature)
            => __api_startApp(Request, GeneratedApiUri, GeneratedSignature);

        public Models.StopAppResponse StopApp(Models.StopAppRequest Request, string GeneratedApiUri, string GeneratedSignature)
            => __api_stopApp(Request, GeneratedApiUri, GeneratedSignature);

        public Models.ListCloudPhoneCustomImageResponse ListCloudPhoneCustomImage(Models.ListCloudPhoneCustomImageRequest Request)
            => __api_listCloudPhoneCustomImage(Request);

        public Models.ClickHomeResponse ClickHome(Models.ClickHomeRequest Request, string GeneratedApiUri, string GeneratedSignature)
            => __api_clickHome(Request, GeneratedApiUri, GeneratedSignature);
    }
}
