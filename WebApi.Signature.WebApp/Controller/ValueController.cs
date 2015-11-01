using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Signature.WebApp.Controller
{
    //註冊 Api 簽章屬性內容
    [ApiSignature]
    [RoutePrefix("api")]
    public class ValueController : ApiController
    {
        [HttpGet]
        [Route("v1/GetValue")]
        public IEnumerable<string> Get()
        {
            return new String[]{ "value01","value02" };
        }

        [HttpPost]
        [Route("v1/AddValue")]
        public string Create(UserModel item)
        {
            return JsonConvert.SerializeObject(item);
        }
    }
}
