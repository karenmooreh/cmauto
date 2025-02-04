using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelupstreamDMUAPI.WebAPI.Controllers
{
    [ApiController]
    [Route("test")]
    [EnableCors("DMUMWEBAPISERV-CORSPOLICY-ALLOWDOMAINS")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [Route("hello")]
        public Models.webapi_result hello()
            => new Models.webapi_result() { result = true, code = 0x00, message = "success" };
    }
}
