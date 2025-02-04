using ChinacSDKCore.NET.Models;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static ChinacSDKCore.NET.Http;

namespace ChinacSDKCore.NET
{
    public partial class ChinacCloudMobileClient
    {
        private void __constructor_ChinacCloudMobileClient(string APIKeyId, string APIKeySecret)
        {
            __apikeyid = APIKeyId;
            __apikeysecret = APIKeySecret;
        }

        #region restful api impelements

        private string __api_baserequest(Models.BaseRequest RequestData, string GeneratedRequestUri = null, string GeneratedSignature = null)
        {
            string __result = string.Empty;
            RequestData.AccessKeyId = string.IsNullOrEmpty(RequestData.AccessKeyId) ? __apikeyid : RequestData.AccessKeyId;

            bool __httpret = false;
            Http.requestparam __requestargs = new Http.requestparam(
                $"{(string.IsNullOrEmpty(GeneratedRequestUri) ? GlobalPresettings.Chinac_APIBaseUri_Newversion : GeneratedRequestUri)}?",
                HttpMethod.Post, null, null, GlobalPresettings.HttpHeaderPresetValues.PresetValue_ContentType);

            Dictionary<string, string> __params_shared = __getparams_byrequest(RequestData, ArgumentAttribute.Type.Shared)
                .ToDictionary(p => p.Key, p => Convert.ToString(p.Value));
            Dictionary<string, object> __params_uniqued = __getparams_byrequest(RequestData, ArgumentAttribute.Type.Uniqued);

            __params_shared[GlobalPresettings.APIRequestPresetKeys.Signature] = string.IsNullOrEmpty(GeneratedSignature) ?
                __signature(__requestargs.method.Method, __params_shared, __apikeyid, __apikeysecret) : GeneratedSignature;
            __requestargs.uri += __combinepaths(__params_shared, true);
            __requestargs.data = __buildjson(__params_uniqued);
            __result = Http.request(__requestargs, out __httpret);

            return __result;
        }

        private Models.ListCloudPhoneResponse __api_listCloudPhone(Models.ListCloudPhoneRequest Request) {
            Models.ListCloudPhoneResponse __result = null;

            string __response = __api_baserequest(Request);
            if (!string.IsNullOrEmpty(__response))
                try { __result = JsonSerializer.Deserialize<Models.ListCloudPhoneResponse>(__response); } catch { }

            return __result;
        }
    

        private Models.DescribeCloudPhoneResponse __api_describeCloudPhone(Models.DescribeCloudPhoneRequest Request)
            => null;

        private Models.CreateCloudPhoneAdbResponse __api_createCloudPhoneAdb(Models.CreateCloudPhoneAdbRequest Request){

            Models.CreateCloudPhoneAdbResponse __result = null;

            string __response = __api_baserequest(Request);
            if (!string.IsNullOrEmpty(__response))
                try { __result = JsonSerializer.Deserialize<Models.CreateCloudPhoneAdbResponse>(__response); } catch { }

            return __result;
        }

        private Models.CloseCloudPhoneAdbResponse __api_closeCloudPhoneAdb(Models.CloseCloudPhoneAdbRequest Request)
            => null;

        private Models.ListCloudPhoneAdbWhiteIpResponse __api_listCloudPhoneAdbWhiteIp(Models.ListCloudPhoneAdbWhiteIpRequest Request)
            => null;

        private Models.SetCloudPhoneAdbWhiteIpResponse __api_setCloudPhoneAdbWhiteIp(Models.SetCloudPhoneAdbWhiteIpRequest Request)
            => null;

        private Models.ReinstallCloudPhoneResponse __api_reinstallCloudPhone(Models.ReinstallCloudPhoneRequest Request)
        {
            Models.ReinstallCloudPhoneResponse __result = null;
            string __response = __api_baserequest(Request);
            if (!string.IsNullOrEmpty(__response))
                try { __result = JsonSerializer.Deserialize<Models.ReinstallCloudPhoneResponse>(__response); } catch { }

            return __result;
        }

        private Models.GetCmdApiSignatureResponse __api_getCmdApiSignature(Models.GetCmdApiSignatureRequest Request)
        {
            Models.GetCmdApiSignatureResponse __result = null;
            string __response = __api_baserequest(Request);
            if(!string.IsNullOrEmpty(__response))
                try { __result = JsonSerializer.Deserialize<Models.GetCmdApiSignatureResponse>(__response); } catch { }

            return __result;
        }

        private Models.GetConnectStatusResponse __api_getConnectStatus(Models.GetConnectStatusRequest Request, string GeneratedApiUri, string GeneratedSignature) { 
            Models.GetConnectStatusResponse __result = null;
            string __response = __api_baserequest(Request, $"{GeneratedApiUri}/cloudPhone/cmd/getConnectStatus", $"{GeneratedSignature}");
            if (!string.IsNullOrEmpty(__response))
                try { __result = JsonSerializer.Deserialize<Models.GetConnectStatusResponse>(__response); } catch { }
            return __result;
        }

        private Models.ScreenShotResponse __api_screenShot(Models.ScreenShotRequest Request, string GeneratedApiUri, string GeneratedSignature)
        {
            Models.ScreenShotResponse __result = null;
            string __response = __api_baserequest(Request, $"{GeneratedApiUri}/cloudPhone/cmd/screenShot", $"{GeneratedSignature}");
            if (!string.IsNullOrEmpty(__response))
                try { __result = JsonSerializer.Deserialize<Models.ScreenShotResponse>(__response); } catch { }
            return __result;
        }

        private Models.SendShellCmdResponse __api_sendShellCmd(Models.SendShellCmdRequest Request)
            => null;

        private Models.StartAppResponse __api_startApp(Models.StartAppRequest Request, string GeneratedApiUri, string GeneratedSignature)
        {
            Models.StartAppResponse __result = null;
            string __response = __api_baserequest(Request, $"{GeneratedApiUri}/cloudPhone/cmd/startApp", $"{GeneratedSignature}");
            if (!string.IsNullOrEmpty(__response))
                try { __result = JsonSerializer.Deserialize<Models.StartAppResponse>(__response); } catch { }
            return __result;
        }

        private Models.StopAppResponse __api_stopApp(Models.StopAppRequest Request, string GeneratedApiUri, string GeneratedSignature)
        {
            Models.StopAppResponse __result = null;
            string __response = __api_baserequest(Request, $"{GeneratedApiUri}/cloudPhone/cmd/stopApp", $"{GeneratedSignature}");
            if (!string.IsNullOrEmpty(__response))
                try { __result = JsonSerializer.Deserialize<Models.StopAppResponse>(__response); } catch { }
            return __result;
        }

        private Models.ListCloudPhoneCustomImageResponse __api_listCloudPhoneCustomImage(Models.ListCloudPhoneCustomImageRequest Request)
        {
            Models.ListCloudPhoneCustomImageResponse __result = null;

            string __response = __api_baserequest(Request);
            if (!string.IsNullOrEmpty(__response))
                try { __result = JsonSerializer.Deserialize<Models.ListCloudPhoneCustomImageResponse>(__response); } catch { }

            return __result;
        }

        private Models.ClickHomeResponse __api_clickHome(Models.ClickHomeRequest Request, string GeneratedApiUri, string GeneratedSignature)
        {
            Models.ClickHomeResponse __result = null;
            string __response = __api_baserequest(Request, $"{GeneratedApiUri}/cloudPhone/cmd/clickHome", $"{GeneratedSignature}");
            if (!string.IsNullOrEmpty(__response))
                try { __result = JsonSerializer.Deserialize<Models.ClickHomeResponse>(__response); } catch { }
            return __result;
        }

        #endregion

        private static string __signature(string Method, Dictionary<string, string> RequestParams, string APIKeyId, string APIKeySecret)
        {
            string __result = string.Empty;

            var __sortedparams = RequestParams.OrderBy(p => p.Key).ToDictionary(p => p.Key, p => p.Value);

            foreach (var __key in __sortedparams.Keys)
                if (!string.IsNullOrEmpty(__sortedparams[__key]) && __key.Trim().ToLower() != "signature")
                    __sortedparams[__key] = __percent_encode(Uri.EscapeDataString(__sortedparams[__key]));

            StringBuilder __pathsbuilder = new StringBuilder();
            foreach (var __keyval in __sortedparams)
                if (!string.IsNullOrEmpty(__sortedparams[__keyval.Key]) && __keyval.Key.Trim().ToLower() != "signature")
                    __pathsbuilder.Append($"&{__keyval.Key}={__keyval.Value}");
            if (__pathsbuilder.Length > 0x00) __pathsbuilder.Remove(0x00, 0x01);
            string __presignstr = $"{Method}\n{__md5(__pathsbuilder.ToString())}\n{GlobalPresettings.HttpHeaderPresetValues.PresetValue_ContentType}\n{__sortedparams[GlobalPresettings.APIRequestPresetKeys.Date]}\n";

            __result = __hmacsha256v3(__presignstr, APIKeySecret);

            return __result;
        }

        private static string __hmacsha256(string data, string key)
        {
            string __result = string.Empty;
            using (HMACSHA256 __operator = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
                __result = Convert.ToBase64String(
                    __operator.ComputeHash(Encoding.UTF8.GetBytes(data)));
            return __result;
        }

        private static string __hmacsha256plus(string data, string key)
        {
            string __result = string.Empty;
            using (HMACSHA256 __operator = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                __operator.Initialize();
                byte[] __tempbuff = __operator.ComputeHash(Encoding.UTF8.GetBytes(data));
                sbyte[] __stdbuff = new sbyte[__tempbuff.Length];
                for (var i = 0x00; i < __tempbuff.Length; i++)
                    __stdbuff[i] = __tempbuff[i] < 0x80 ? 
                        (sbyte)__tempbuff[i] : (sbyte)(__tempbuff[i] - 0x100);
                __result = Convert.ToBase64String((byte[])(Array)__stdbuff);
            }
            return __result;
        }

        private static string __buildjson(Dictionary<string, object> Params)
        {
            StringBuilder __jsonbuilder = new StringBuilder();
            foreach (var __param in Params)
            {
                string __paramval = string.Empty;
                if (__param.Value is string) __paramval = $"\"{Convert.ToString(__param.Value)}\"";
                else __paramval = null != __param.Value ? JsonSerializer.Serialize(__param.Value) : null;
                if (!string.IsNullOrEmpty(__param.Key) && !string.IsNullOrEmpty(__paramval) && __paramval != "\"\"")
                    __jsonbuilder.Append($",\"{__param.Key}\": {__paramval}");
            }
            if (__jsonbuilder.Length > 0x00) __jsonbuilder.Remove(0x00, 0x01);
            __jsonbuilder.Append("}");
            __jsonbuilder.Insert(0x00,"{");
            return __jsonbuilder.ToString();
        }

        private static string __hmacsha256v3(string data, string key)
        {
            string __result = string.Empty;

            var __hmac = new HMac(new Sha256Digest());
            __hmac.Init(new KeyParameter(Encoding.UTF8.GetBytes(key)));
            byte[] __retbytes = new byte[__hmac.GetMacSize()];
            byte[] __databuff = Encoding.UTF8.GetBytes(data);
            __hmac.BlockUpdate(__databuff, 0x00, __databuff.Length);
            __hmac.DoFinal(__retbytes, 0x00);
            __result = Convert.ToBase64String(__retbytes);

            return __result;
        }

        private static string __combinepaths(Dictionary<string,string> RequestParams, bool ValueEncode = false, bool Sorted = true)
        {
            StringBuilder __pathsbuilder = new StringBuilder();

            var __params = !Sorted ? RequestParams :
                RequestParams.OrderBy(p => p.Key).ToDictionary(p => p.Key, p => p.Value);

            foreach (var __param in __params)
                if (!string.IsNullOrEmpty(__param.Key) && !string.IsNullOrEmpty(__param.Value))
                    __pathsbuilder.Append($"&{__param.Key}={(ValueEncode ? Uri.EscapeDataString(__param.Value) : __param.Value)}");
            if (__pathsbuilder.Length > 0x00) __pathsbuilder.Remove(0x00, 0x01);

            return __pathsbuilder.ToString();
        }

        private static string __percent_encode(string text)
            => Regex.Replace(
                Regex.Replace(
                    Regex.Replace(text, "\\+", "%20", RegexOptions.None),
                        "\\*", "%2A", RegexOptions.None),
                "\\%7E", "~", RegexOptions.None);

        private static string __md5(string proclaimed)
            => Regex.Replace(
                BitConverter.ToString(
                    MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(proclaimed))),
                "\\-", string.Empty, RegexOptions.None).ToLower();

        private static Dictionary<string,object> __getparams_byrequest(object Request, 
            ArgumentAttribute.Type ParamType = ArgumentAttribute.Type.Shared) {
            Dictionary<string, object> __params = new Dictionary<string, object>();

            Type __realtype = Request.GetType();
            var __properties = __realtype.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            foreach(var __property in __properties)
                if (!__params.ContainsKey(__property.Name))
                {
                    object? __propertyvalue = __property.GetValue(Request, null);
                    var __ptype = __property.GetCustomAttribute<ArgumentAttribute>();
                    if (__ptype != null)
                    {
                        if(__ptype.type == ArgumentAttribute.Type.Shared && ParamType == ArgumentAttribute.Type.Shared)
                            __params.Add(__property.Name,
                                null != __propertyvalue ? __propertyvalue : string.Empty);
                        else if(__ptype.type == ArgumentAttribute.Type.Uniqued && ParamType == ArgumentAttribute.Type.Uniqued)
                            __params.Add(__property.Name,
                                null != __propertyvalue ? __propertyvalue : string.Empty);
                    }
                    else if (ParamType == ArgumentAttribute.Type.Uniqued)
                        __params.Add(__property.Name, 
                            null != __propertyvalue ? __propertyvalue : string.Empty);
                }

            return __params;
        } 

    }
}
