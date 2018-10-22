using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace maduka_WebAPI.Controllers
{
    public class HeaderController : ApiController
    {
        [Swagger.Header(headersConfig="header1,string|header2,integer|header3,boolean,true/false")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Swagger.Header(headersConfig = "no1,string|no2,integer,1/2/3/4/5/6/7")]
        public string Get(int id)
        {
            var strValues = new string[] { "1", "2" };
            return strValues[2];
            // throw new System.NotImplementedException();
            // return "value";
        }
    }
}
