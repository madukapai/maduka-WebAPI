using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace maduka_WebAPI
{

    using System.Web.Http.Filters;
    using System.Net;
    using System.Net.Http;

    public class ExceptionHandle : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is NotImplementedException)
            {
                // 尚未實作的例外狀態
                context.Response = new HttpResponseMessage(HttpStatusCode.NotImplemented);
            }
            else
            {
                // 其他的狀態，回傳400的Status Code
                context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
    }
}