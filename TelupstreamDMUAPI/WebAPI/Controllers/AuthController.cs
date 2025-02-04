using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TelupstreamDMUAPI.WebAPI.Controllers
{
    [ApiController]
    [Route("auth")]
    [EnableCors("DMUMWEBAPISERV-CORSPOLICY-ALLOWDOMAINS")]
    public class AuthController : ControllerBase
    {
        private const string const_dmuauth_claimkey_signuser = "dmuauth_claimkey_signuser";

        [HttpGet]
        [Route("verify")]
        public Models.webapi_result verify()
        {
            Models.webapi_result __result = new Models.webapi_result();

            var __curuser = HttpContext.User.Claims
                        .FirstOrDefault(c => c.Type == const_dmuauth_claimkey_signuser);
            __result.result = null != __curuser &&
                __curuser.Value.Trim().ToLower() == confs.settings.authorize.signuser.Trim().ToLower();
            __result.code = __result.result ? 0x00 : -0x01;
            __result.message = __result.result ? "verified" : "unverified";

            return __result;
        }

        [HttpPost]
        [Route("signin")]
        public Models.models_auth.signin_result signin([FromBody] Models.models_auth.signin_request data)
        {
            Models.models_auth.signin_result __result = new Models.models_auth.signin_result();

            if (!string.IsNullOrEmpty(data.username) && 
                data.username.Trim().ToLower() == confs.settings.authorize.signuser.Trim().ToLower() &&
                Common.SecurityProvider.MD5Crypto(data.password) == confs.settings.authorize.signpassword.Trim().ToLower()) {

                var __claims = new[] {
                            new Claim(const_dmuauth_claimkey_signuser, data.username.Trim().ToLower())
                        };

                var __key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(confs.webapi.BaseSettings.Cors.SecretKey));
                var __credentials = new SigningCredentials(__key, SecurityAlgorithms.HmacSha256);
                var __jwtToken = new JwtSecurityToken(
                    confs.webapi.BaseSettings.Cors.TokenIssuer,
                    confs.webapi.BaseSettings.Cors.TokenAudience, __claims,
                    expires: DateTime.UtcNow.AddDays(confs.webapi.BaseSettings.Cors.ExpiresDays),
                    signingCredentials: __credentials);
                var __token = new JwtSecurityTokenHandler().WriteToken(__jwtToken);

                __result.result = true;
                __result.data = __token;
                __result.code = 0x00;
                __result.message = "success";
            }
            else
            {
                __result.result = false;
                __result.code = -0x01;
                __result.message = "password error or user not exists";
            }

            return __result;
        }
    }
}
