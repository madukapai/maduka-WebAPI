using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace maduka_WebAPI.Swagger
{
    using Swashbuckle.Swagger;
    using System.Web.Http.Description;

    public class HeaderFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (!apiDescription.ActionDescriptor.GetCustomAttributes<HeaderAttribute>().Any())
            {
                return;
            }

            if (operation.parameters == null)
            {
                operation.parameters = new List<Parameter>();
            }

            HeaderAttribute attr = apiDescription.ActionDescriptor.GetCustomAttributes<HeaderAttribute>().FirstOrDefault();
            // 取出Headers，並進行切割
            List<string> strHeaders = attr.headersConfig.Split("|".ToCharArray()).ToList();
            for (int i = 0; i < strHeaders.Count; i++)
            {
                // 切割參數
                List<object> objEnums = new List<object>();
                List<string> strHeaderParam = strHeaders[i].Split(",".ToCharArray()).ToList();
                if (strHeaderParam.Count > 2)
                    objEnums = strHeaderParam[2].Split("/".ToCharArray()).ToList<object>();

                Parameter objParam = new Parameter()
                {
                    name = strHeaderParam[0],
                    @in = "header",
                    type = strHeaderParam[1],
                    required = true,
                };

                if (objEnums.Count > 0)
                    objParam.@enum = objEnums;

                operation.parameters.Add(objParam);
            }
        }
    }
}