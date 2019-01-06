using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace maduka_WebAPI.Models
{
    public class ValidModels
    {
        /// <summary>
        /// 輸入資料的模型
        /// </summary>
        public class ValidInfoQuery
        {
            [Required]
            public string Name { get; set; }
            public string Tel { get; set; }
            public string Address { get; set; }
            [Required]
            [Range(1, 130)]
            public int Age { get; set; }
            [Required]
            public DateTime Birthday { get; set; }
        }

        /// <summary>
        /// 回傳驗證結果的模型
        /// </summary>
        public class ValidInfoResult
        {
            public bool IsValid { get; set; }
            public List<ValidItem> List { get; set; }
            public class ValidItem
            {
                public IEnumerable<string> Field { get; set; }
                public string Message { get; set; }
            }
        }
    }
}