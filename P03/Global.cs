using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P03
{
    internal class Global
    {
        public const string CONST_PAGEURL = "https://mgkfun.migu.cn/h5page/jiangsuorder/?channelId=SZ1100150&goodCode=5190&pageId=PJS00020";

        public const string CONST_XPATHKEY_WOYAODINGYUE = "woyaodingyue";
        public const string CONST_XPATHKEY_SHOUJIHAOMA = "shoujihaoma";
        public const string CONST_XPATHKEY_YUEDUTONGYI = "yuedutongyi";
        public const string CONST_XPATHKEY_HUOQUYANZHENGMA = "huoquyanzhengma";
        public const string CONST_XPATHKEY_TIANXIEYANZHENGMA = "tianxieyanzhengma";
        public const string CONST_XPATHKEY_QUERENDINGGOU = "querendinggou";
        public const string CONST_XPATHKEY_SUCCESS = "success";
        public static readonly Dictionary<string, string> const_xpaths = new Dictionary<string, string>() {
            { CONST_XPATHKEY_WOYAODINGYUE,
                "/html/body/div[@id='app']/div[@class='contain']/div[@class='box1']/div[@class='button']/img" },
            { CONST_XPATHKEY_SHOUJIHAOMA, 
                "/html/body/div[@id='app']/div[@class='contain']/div[@class='pop']/div[@class='order_box']/div[@class='pop-6']/input" },
            { CONST_XPATHKEY_YUEDUTONGYI, 
                "/html/body/div[@id='app']/div[@class='contain']/div[@class='pop']/div[@class='order_box']/div[@class='pop-rules']/label[@class='el-checkbox']/span[@class='el-checkbox__input']" },
            { CONST_XPATHKEY_HUOQUYANZHENGMA, 
                "/html/body/div[@id='app']/div[@class='contain']/div[@class='pop']/div[@class='order_box']/div[@class='pop-6-code']/div[@class='code']" },
            { CONST_XPATHKEY_TIANXIEYANZHENGMA, 
                "/html/body/div[@id='app']/div[@class='contain']/div[@class='pop']/div[@class='order_box']/div[@class='pop-6-code']/div[@class='pop-code']/input" },
            { CONST_XPATHKEY_QUERENDINGGOU, 
                "/html/body/div[@id='app']/div[@class='contain']/div[@class='pop']/div[@class='order_box']/div[@class='sure']" },
            { CONST_XPATHKEY_SUCCESS,
                "/html/body/div[@id='app']/div[@class='contain']/div[@class='pop']/div[@class='order_success']" },
        };
    }
}
