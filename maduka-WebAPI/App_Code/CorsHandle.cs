using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace maduka_WebAPI
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Cors;
    using System.Web.Http.Cors;

    public class CorsHandle : Attribute, ICorsPolicyProvider
    {
        private CorsPolicy objProlicy;

        public CorsHandle()
        {
            // 建立一個跨網域存取的原則物件
            objProlicy = new CorsPolicy
            {
                AllowAnyMethod = true,
                AllowAnyHeader = true
            };

            // 在這裡透過資料庫或是設定的方式，可動態加入允許存取的來源網域清單
            objProlicy.Origins.Add("http://myclient.azurewebsites.net");
            objProlicy.Origins.Add("http://www.facebook.com");
        }

        public Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(objProlicy);
        }
    }
}