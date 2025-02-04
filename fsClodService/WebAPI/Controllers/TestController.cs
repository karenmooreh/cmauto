using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fsClodService.WebAPI.Controllers
{
    [ApiController]
    [Route("test")]
    [EnableCors("FSVDMUMWEBAPISERV-CORSPOLICY-ALLOWDOMAINS")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [Route("hello")]
        public string hello()
        {
            return "success";
        }
    }
}
