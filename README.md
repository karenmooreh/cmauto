# cmauto
中国移动掌上营业厅APP充值（小额代充）自动化测试项目

起因：
24年末本人就职南京一家公司从事自动化开发工作。该公司老板人品有问题，本质上就是想着白嫖技术解决方案的心态找人解决他公司核心业务的技术问题。本人兢兢业业上班加班得了甲流一把鼻涕一把眼泪地也给他坚持加班赶工，结果这屌人在等技术方案全部测试通过后各种找麻烦找理由踹人。好吧，既然这样那么这些技术方案留着也没用，就当垃圾扔出来了，如果有感兴趣的自取研究，请勿用于商业目的。

目前中国移动已整合各地方移动单位业务到统一的APP业务中，并且中国移动官方也取消了各合作方的小额代充业务，电信灰产行业以往通过HTTP接口方式提交登录开通业务接口的方式已经不管用了（吐槽：这屌老板就是因为以前的土屌技术员还在玩HTTP接口提交这一套结果玩不下去了才想着找人白嫖技术方案）。

仓库代码是本人使用Visual Studio 2022/C#进行可行性测试和初步通过Appium+ADB方式，使用云手机方案进行自动化登录+小额充值（最低1元可充），并且借助平台能力实现无限充值账号登录（跳过掌厅APP的本地号码登录限制，据本人通过公司同事了解，该公司屯了几十万个手机卡号通过猫池接码不知道违法不违法，几个小技术每天干活都提心吊胆的，本人原本是不知道这行里面的门道的，直到最后被踢了才恍然大悟，相关部门请勿追责），测试项目中（以fs开头的C#项目）实现：全程自动化接码登录+自动化按单金额充值+自动化支付（测试中是使用支付宝，实际上微信、银行卡等也都可以）全套流程，仅供学习研究勿做他用。

中国移动官方相关技术部门也可引以为鉴，单纯靠封堵HTTP接口、参数加密以及简单的检测APP应用环境的话无法彻底避免这种业务漏洞。

另：本人在自动化方面也有相关研究沉积，如需技术支持可联系本人。
