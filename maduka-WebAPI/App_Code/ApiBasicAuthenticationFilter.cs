using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maduka_WebAPI
{
    using System.Web.Http.Controllers;

    public class ApiBasicAuthenticationFilter : BasicAuthenticationFilter
    {

        public ApiBasicAuthenticationFilter()
        { }

        public ApiBasicAuthenticationFilter(bool active) : base(active)
        { }


        protected override bool OnAuthorizeUser(string username, string password, HttpActionContext actionContext)
        {
            // 在這裡加上帳號密碼的驗證，可以從資料庫取出資料進行比對
            string strUserName = username;
            string strPassword = password;
            bool blIsAuthorize = false;

            /* 作一個假的驗證，測試用
            if (strUserName == "maduka" && strPassword == "ABCDE")
                blIsAuthorize = true;
            */

            return blIsAuthorize;
        }
    }
}