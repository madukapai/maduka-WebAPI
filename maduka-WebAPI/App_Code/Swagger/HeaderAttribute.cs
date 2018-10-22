using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace maduka_WebAPI.Swagger
{
    public class HeaderAttribute : Attribute
    {
        public string headersConfig { get; set; }
    }
}