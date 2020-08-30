using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace maduka_WebAPI.Controllers
{
    using maduka_WebAPI.App_Code;
    using Newtonsoft.Json.Linq;

    public class CacheSampleController : ApiController
    {
        /// <summary>
        /// 取得字串用的控制器，加入快取的AOP機制
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        [CacheHandle(AbsoluteExpirationMinute = 1, SlidingExpirationMinute = 1, CacheDataType = CacheHandle.enumCacheDataType.String, EnableKeyValueMapping = true)]
        [HttpGet]
        [ActionName("GetStringCache")]
        public string GetStringCache([FromUri] string strName)
        {
            string strResponse = "";

            switch (strName)
            {
                case "John": strResponse = "John Wick"; break;
                case "Mary": strResponse = "Mary Jane"; break;
                case "Amy": strResponse = "Amy Adams"; break;
            }

            System.Threading.Thread.Sleep(5000);

            return strResponse;
        }

        /// <summary>
        /// 取得模型用的控制器，加入快取的AOP機制
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [CacheHandle(AbsoluteExpirationMinute = 1, SlidingExpirationMinute = 1, CacheDataType = CacheHandle.enumCacheDataType.JObject, EnableKeyValueMapping = true)]
        [HttpPost]
        [ActionName("GetModelCache")]
        public ResultModel GetModelCache(QueryModel query)
        {
            ResultModel objResponse = new ResultModel();

            switch (query.Name)
            {
                case "John": objResponse = new ResultModel { Age = 52, Phone = "1234567890" }; break;
                case "Mary": objResponse = new ResultModel { Age = 13, Phone = "1122334455" }; break;
                case "Amy": objResponse = new ResultModel { Age = 47, Phone = "0099887766" }; break;
            }

            System.Threading.Thread.Sleep(5000);

            return objResponse;
        }

        public class QueryModel
        {
            public string Name { get; set; }
        }

        public class ResultModel
        {
            public string Phone { get; set; }
            public int Age { get; set; }
        }
    }
}
