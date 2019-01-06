using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace maduka_WebAPI.Controllers
{
    public class ValidController : ApiController
    {
        /// <summary>
        /// 執行資料寫入的Post動作
        /// </summary>
        /// <param name="query"></param>
        public Models.ValidModels.ValidInfoResult Post(Models.ValidModels.ValidInfoQuery query)
        {
            Models.ValidModels.ValidInfoResult result = new Models.ValidModels.ValidInfoResult() { IsValid = true, };
            result.List = new List<Models.ValidModels.ValidInfoResult.ValidItem>();

            #region // 傳統欄位驗證的寫法
            //if (string.IsNullOrEmpty(query.Name))
            //{
            //    result.IsValid = false;
            //    result.List.Add(new Models.ValidModels.ValidInfoResult.ValidItem()
            //    {
            //        Field = new List<string> { "Name" },
            //        Message = "Name欄位必填"
            //    });
            //}

            //if (query.Age > 130 || query.Age < 1)
            //{
            //    result.IsValid = false;
            //    result.List.Add(new Models.ValidModels.ValidInfoResult.ValidItem()
            //    {
            //        Field = new List<string> { "Age" },
            //        Message = "Age欄位必須在1與130之間"
            //    });
            //}
            #endregion


            #region // 使用ValidationContext的驗證物件
            // 定義ValidationContext的驗證物件
            var context = new ValidationContext(query);

            // 定義進行Validation回傳的訊息
            var validationResults = new List<ValidationResult>();

            // 進行驗證動作
            bool isValid = Validator.TryValidateObject(query, context, validationResults, true);

            // 將驗證結果進行處理，並回傳到指定的回傳物件中
            result.IsValid = isValid;
            result.List = validationResults.Select(c => new Models.ValidModels.ValidInfoResult.ValidItem()
                                            {
                                                Field = c.MemberNames,
                                                Message = c.ErrorMessage
                                            })
                                            .ToList();
            #endregion

            return result;
        }
    }
}
