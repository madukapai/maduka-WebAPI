using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using MyClassLibrary;

namespace maduka_WebAPI
{
    public class CacheHandle : ActionFilterAttribute
    {
        Cache objCache = new Cache();
        /// <summary>
        /// 指定分鐘數後回收快取，不指定的話預設值為0
        /// </summary>
        public int AbsoluteExpirationMinute { get; set; }
        /// <summary>
        /// 快取最後一次使用後重新指定快取的過期分鐘數，不指定的話預設值為0
        /// </summary>
        public int SlidingExpirationMinute { get; set; }
        /// <summary>
        /// 快取的物件類型
        /// </summary>
        public enumCacheDataType CacheDataType { get; set; }
        /// <summary>
        /// 是否啟用輸入與輸出資料對應的快取機制，當設定為false時，則不論輸入為何，輸出內容都會一模一樣
        /// </summary>
        public bool EnableKeyValueMapping { get; set; }

        /// <summary>
        /// 當WebAPI的控制器剛被啟動的時候，會進入至這個覆寫的事件中
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            DateTime dtStart = DateTime.UtcNow;
            string strInput = "";

            // 因為傳入的參數為多數，所以ActionArguments必須用迴圈將之取出
            foreach (var item in actionContext.ActionArguments)
            {
                // 取出傳入的參數名稱
                string strParamName = item.Key;

                // 取出傳入的內容並作Json資料的處理
                strInput += strParamName + ":" + JsonConvert.SerializeObject(item.Value) + ".";
            }

            // 將資料存入Context中
            actionContext.Request.Properties.Add(new KeyValuePair<string, object>("__CacheInputData__", strInput));

            // 判斷是否有存在快取資料, 如果存在快取的話就直接回傳
            string strActionName = actionContext.ActionDescriptor.ControllerDescriptor.ControllerName + "-" + actionContext.ActionDescriptor.ActionName;
            List<CacheItem> objCacheItem = objCache.GetCache<List<CacheItem>>(strActionName);
            if (objCacheItem != null)
            {
                CacheItem objItem = null;

                // 如果啟用鍵值對應，就要過濾輸入值找出對應內容
                if (this.EnableKeyValueMapping)
                    objItem = objCacheItem.Where(x => x.CacheName == strInput).FirstOrDefault();
                else
                    objItem = objCacheItem.FirstOrDefault();

                if (objItem != null)
                {
                    object objReturn = null;
                    switch (this.CacheDataType)
                    {
                        case enumCacheDataType.Bool: objReturn = bool.Parse(objItem.CacheValue.Replace(@"""", "")); break;
                        case enumCacheDataType.Decimal: objReturn = decimal.Parse(objItem.CacheValue.Replace(@"""", "")); break;
                        case enumCacheDataType.Int: objReturn = int.Parse(objItem.CacheValue.Replace(@"""", "")); break;
                        case enumCacheDataType.JObject: objReturn = JObject.Parse(objItem.CacheValue); break;
                        case enumCacheDataType.String: objReturn = objItem.CacheValue.Replace(@"""", ""); break;
                    }
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, objReturn);
                }
            }
        }

        /// <summary>
        /// 當WebAPI的控制器結束動作，會進入這個覆寫的事件中
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            DateTime dtEnd = DateTime.UtcNow;
            string strOutput = "";

            if (actionExecutedContext.Response != null)
            {
                // 將actionExecutedContext.Response.Content轉換成Json的字串
                if (actionExecutedContext.Response.Content != null && !actionExecutedContext.Response.Content.GetType().ToString().Contains("System.IO.Stream"))
                {
                    string strResponseContent = JsonConvert.SerializeObject(actionExecutedContext.Response.Content);

                    // 將Json字串轉換成我們自訂的ResponseContentModel物件
                    ResponseContentModel objResponseContent = JsonConvert.DeserializeObject<ResponseContentModel>(strResponseContent);

                    // 取出從WebAPI回傳的物件，並轉會成Json字串
                    strOutput = JsonConvert.SerializeObject(objResponseContent.Value);
                }
            }

            // 取得Input資料
            object objInput;
            actionExecutedContext.Request.Properties.TryGetValue("__CacheInputData__", out objInput);
            string strInput = (string)objInput;

            // 寫入快取資料
            string strActionName = actionExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName + "-" + actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
            List<CacheItem> objCacheItem = objCache.GetCache<List<CacheItem>>(strActionName);
            if (objCacheItem == null)
                objCacheItem = new List<CacheItem>();

            var objItem = objCacheItem.Where(x => x.CacheName == strInput).FirstOrDefault();
            if (objItem == null)
            {
                objCacheItem.Add(new CacheItem()
                {
                    CacheName = strInput,
                    CacheValue = strOutput,
                });
            }

            objCache.SetCache(strActionName, objCacheItem, this.AbsoluteExpirationMinute, this.SlidingExpirationMinute);
        }

        /// <summary>
        /// 快取的物件
        /// </summary>
        private class CacheItem
        {
            public string CacheName { get; set; }
            public string CacheValue { get; set; }
        }

        /// <summary>
        /// 快取的物件類型
        /// </summary>
        public enum enumCacheDataType
        {
            Int,
            String,
            Bool,
            Decimal,
            JObject,
        }

        #region // 轉換actionExecutedContext.Response.Content的物件模型
        public class ResponseContentModel
        {
            public string ObjectType { get; set; }
            public Formatter Formatter { get; set; }
            public object Value { get; set; }
            public Header[] Headers { get; set; }
        }

        public class Formatter
        {
            public bool UseDataContractJsonSerializer { get; set; }
            public bool Indent { get; set; }
            public int MaxDepth { get; set; }
            public Serializersettings SerializerSettings { get; set; }
            public Supportedmediatype[] SupportedMediaTypes { get; set; }
            public Supportedencoding[] SupportedEncodings { get; set; }
            public Mediatypemapping[] MediaTypeMappings { get; set; }
            public Requiredmemberselector RequiredMemberSelector { get; set; }
        }

        public class Serializersettings
        {
            public int ReferenceLoopHandling { get; set; }
            public int MissingMemberHandling { get; set; }
            public int ObjectCreationHandling { get; set; }
            public int NullValueHandling { get; set; }
            public int DefaultValueHandling { get; set; }
            public object[] Converters { get; set; }
            public int PreserveReferencesHandling { get; set; }
            public int TypeNameHandling { get; set; }
            public int MetadataPropertyHandling { get; set; }
            public int TypeNameAssemblyFormat { get; set; }
            public int ConstructorHandling { get; set; }
            public Contractresolver ContractResolver { get; set; }
            public object ReferenceResolver { get; set; }
            public object TraceWriter { get; set; }
            public object Binder { get; set; }
            public object Error { get; set; }
            public ResponseContext Context { get; set; }
            public string DateFormatString { get; set; }
            public object MaxDepth { get; set; }
            public int Formatting { get; set; }
            public int DateFormatHandling { get; set; }
            public int DateTimeZoneHandling { get; set; }
            public int DateParseHandling { get; set; }
            public int FloatFormatHandling { get; set; }
            public int FloatParseHandling { get; set; }
            public int StringEscapeHandling { get; set; }
            public string Culture { get; set; }
            public bool CheckAdditionalContent { get; set; }
        }

        public class Contractresolver
        {
            public bool DynamicCodeGeneration { get; set; }
            public int DefaultMembersSearchFlags { get; set; }
            public bool SerializeCompilerGeneratedMembers { get; set; }
            public bool IgnoreSerializableInterface { get; set; }
            public bool IgnoreSerializableAttribute { get; set; }
        }

        public class ResponseContext
        {
            public object Context { get; set; }
            public int State { get; set; }
        }

        public class Requiredmemberselector
        {
        }

        public class Supportedmediatype
        {
            public object CharSet { get; set; }
            public object[] Parameters { get; set; }
            public string MediaType { get; set; }
        }

        public class Supportedencoding
        {
            public string BodyName { get; set; }
            public string EncodingName { get; set; }
            public string HeaderName { get; set; }
            public string WebName { get; set; }
            public int WindowsCodePage { get; set; }
            public bool IsBrowserDisplay { get; set; }
            public bool IsBrowserSave { get; set; }
            public bool IsMailNewsDisplay { get; set; }
            public bool IsMailNewsSave { get; set; }
            public bool IsSingleByte { get; set; }
            public Encoderfallback EncoderFallback { get; set; }
            public Decoderfallback DecoderFallback { get; set; }
            public bool IsReadOnly { get; set; }
            public int CodePage { get; set; }
        }

        public class Encoderfallback
        {
            public int MaxCharCount { get; set; }
        }

        public class Decoderfallback
        {
            public int MaxCharCount { get; set; }
        }

        public class Mediatypemapping
        {
            public string HeaderName { get; set; }
            public string HeaderValue { get; set; }
            public int HeaderValueComparison { get; set; }
            public bool IsValueSubstring { get; set; }
            public Mediatype MediaType { get; set; }
        }

        public class Mediatype
        {
            public object CharSet { get; set; }
            public object[] Parameters { get; set; }
            public string MediaType { get; set; }
        }

        public class Header
        {
            public string Key { get; set; }
            public string[] Value { get; set; }
        }
        #endregion

    }
}