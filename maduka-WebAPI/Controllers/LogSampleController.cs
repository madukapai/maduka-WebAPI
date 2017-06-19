using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace maduka_WebAPI.Controllers
{
    using maduka_WebAPI.App_Code;

    public class LogSampleController : ApiController
    {
        [HttpPost]
        [ActionName("WriteLogSample")]
        [LogHandle]
        public WriteLogSampleResultModel Post(WriteLogSampleModel value)
        {
            // 在這裡處理一些事，然後return物件
            WriteLogSampleResultModel objResult = new WriteLogSampleResultModel()
            {
                IsGet = true,
                Message = "我收到你的資料了",
            };

            return objResult;
        }

        /// <summary>
        /// 輸入WebAPI的物件模型
        /// </summary>
        public class WriteLogSampleModel
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public DateTime Birthday { get; set; }
        }

        /// <summary>
        /// 輸出WebAPI的物件模型
        /// </summary>
        public class WriteLogSampleResultModel
        {
            public bool IsGet { get; set; }
            public string Message { get; set; }
        }
    }
}
