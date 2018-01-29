using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace maduka_WebAPI.Controllers
{
    public class CorsController : ApiController
    {
        [HttpGet]
        [EnableCors(origins: "http://mywebclient.azurewebsites.net", headers: "*", methods: "*")]
        public DateTime GetDateTimeBySetting()
        {
            return DateTime.Now;
        }

        [HttpGet]
        [CorsHandle]
        public DateTime GetDateTimeByProlicy()
        {
            return DateTime.Now;
        }

        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [CorsOnActionHandle]
        public DateTime GetDateTimeByActionFilter()
        {
            return DateTime.Now;
        }
    }
}
