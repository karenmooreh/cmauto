// See https://aka.ms/new-console-template for more information
using ChinacSDKCore.NET;
using ChinacSDKCore.NET.Models;
using System.Text.Json;

Console.WriteLine("Chinac SDK Test");

obj o1 = new obj() { 
    a = "123",
    b = new List<string>() { "321" }
};
string s1 = JsonSerializer.Serialize(o1);
string s2 = JsonSerializer.Serialize(o1.b);

string __apikeyid = "adb1b4db10744689ab67db9d2a68c189";
string __apisecret = "350bc68aa7824db088295d5b50714a38";

ChinacCloudMobileClient __ccmc = new ChinacCloudMobileClient(__apikeyid, __apisecret);

//__ccmc.Signature("POST", new Dictionary<string, string>() {
//    {"Action", "ListCloudPhone" },
//    {"Version", "1.0" },
//    {"AccessKeyId", "adb1b4db10744689ab67db9d2a68c189" },
//    {"Date", "2025-01-07T16:08:19 +0800" },
//}, __apikeyid, __apisecret);


ListCloudPhoneRequest __lcprrequest = new ListCloudPhoneRequest() { };

ListCloudPhoneResponse __lcprresponse = __ccmc.ListCloudPhone(__lcprrequest);

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine($"[{DateTime.Now.ToString()}]>Command ListCloudPhone result: {__lcprresponse.message}");

List<string> __ids = new List<string>() { __lcprresponse.data.List[0x00].Id };

GetCmdApiSignatureRequest __gcasrrequest = new GetCmdApiSignatureRequest() { 
    Ids = __ids,
    Region = __lcprresponse.data.List[0x00].Region
};

GetCmdApiSignatureResponse __gcasrresponse = __ccmc.GetCmdApiSignature(__gcasrrequest);

string __signature = __gcasrresponse.data.Signature;
string __rtoken = __gcasrresponse.data.RToken;
string __apiuri = __gcasrresponse.data.ApiUrl;

Console.WriteLine($"[{DateTime.Now.ToString()}]>Command GetCmdApiSignature result: {__gcasrresponse.message}");
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine($"Signature:\t{__signature}");
Console.WriteLine($"RToken:\t\t{__rtoken}");
Console.WriteLine($"ApiUrl:\t\t{__apiuri}");

if (false)
{
    GetConnectStatusRequest __gcsrequest = new GetConnectStatusRequest()
    {
        Ids = __ids,
        RToken = __rtoken,
    };

    GetConnectStatusResponse __gcsresponse = __ccmc.GetConnectStatus(__gcsrequest, __apiuri, __signature);
}

if (false)
{
    ScreenShotRequest __ssrequest = new ScreenShotRequest()
    {
        Id = __ids[0x00],
        RToken = __rtoken
    };
    ScreenShotResponse __ssresponse = __ccmc.ScreenShot(__ssrequest, __apiuri, __signature);
}


if (false)
{
    StartAppRequest __sarrequest = new StartAppRequest() { 
        RToken = __rtoken,
        Id = __ids[0x00],
        PackageName = "com.greenpoint.android.mc10086.activity",
    };
    StartAppResponse __sarresponse = __ccmc.StartApp(__sarrequest, __apiuri, __signature);
}

if (false)
{
    StopAppRequest __sarequest = new StopAppRequest() { 
        RToken = __rtoken,
        Id = __ids[0x00],
        PackageName = "com.greenpoint.android.mc10086.activity",
    };
    StopAppResponse __saresponse = __ccmc.StopApp(__sarequest, __apiuri, __signature);
}

string __imageid = null;
if (true)
{
    ListCloudPhoneCustomImageRequest __lcpcirequest = new ListCloudPhoneCustomImageRequest() { 
        Region = ChinacZones.CloudMobile.cn_jsha_cloudphone_3
    };
    ListCloudPhoneCustomImageResponse __lcpciresponse = __ccmc.ListCloudPhoneCustomImage(__lcpcirequest);
    __imageid = __lcpciresponse.data.List[0x00].Id;
}

if(true
    && !string.IsNullOrEmpty(__imageid)) {
    ReinstallCloudPhoneRequest __rcprequest = new ReinstallCloudPhoneRequest() { 
        CloudPhones = new List<ReinstallCloudPhoneRequest.CloudPhone>() { 
            new ReinstallCloudPhoneRequest.CloudPhone() { 
                Id = __lcprresponse.data.List[0x00].Id,
                Region = __lcprresponse.data.List[0x00].Region
            }
        },
        ImageId = __imageid,
        Remark = $"{DateTime.Now.ToString("yyyyMMddHHmmss")}",
        ParamType = "random"
    };
    ReinstallCloudPhoneResponse __rcpresponse = __ccmc.ReinstallCloudPhone(__rcprequest);
}


Console.ReadLine();




class obj
{
    public string a { get; set; }
    public List<string> b { get; set; }
}