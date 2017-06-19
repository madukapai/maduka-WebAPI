using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace maduka_WebAPI.App_Code
{
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using System.IO;
    using Newtonsoft.Json;

    /// <summary>
    /// 進行Log處理的類別物件
    /// </summary>
    public class LogHandle : ActionFilterAttribute
    {
        /// <summary>
        /// 當WebAPI的控制器剛被啟動的時候，會進入至這個覆寫的事件中
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            // 因為傳入的參數為多數，所以ActionArguments必須用迴圈將之取出
            foreach (var item in actionContext.ActionArguments)
            {
                // 取出傳入的參數名稱
                string strParamName = item.Key;

                // 取出傳入的內容並作Json資料的處理
                string strContent = strParamName + ":" + JsonConvert.SerializeObject(item.Value);

                // 寫入Log
                this.WriteLogContent("Input", strContent);
            }
        }

        /// <summary>
        /// 當WebAPI的控制器結束動作，會進入這個覆寫的事件中
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            // 將actionExecutedContext.Response.Content轉換成Json的字串
            string strResponseContent = JsonConvert.SerializeObject(actionExecutedContext.Response.Content);

            // 將Json字串轉換成我們自訂的ResponseContentModel物件
            ResponseContentModel objResponseContent = JsonConvert.DeserializeObject<ResponseContentModel>(strResponseContent);

            // 取出從WebAPI回傳的物件，並轉會成Json字串
            string strContent = JsonConvert.SerializeObject(objResponseContent.Value);

            // 寫入Log
            this.WriteLogContent("Output", strContent);
        }


        /// <summary>
        /// 實際寫入Log的動作
        /// </summary>
        /// <param name="strAction">動作類別字串</param>
        /// <param name="strContent">寫入Log的內容</param>
        private void WriteLogContent(string strAction, string strContent)
        {
            string strLineContent = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " Type:" + strAction + " Content:" + strContent;
            File.AppendAllLines(@"D:\Log\File.txt", new string[] { strLineContent });
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