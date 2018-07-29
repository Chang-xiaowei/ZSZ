using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Web;
using System.Web.Mvc;

namespace ZSZ.CommonMVC
{
    public class JsonNetResult: System.Web.Mvc.JsonResult
    {
        public JsonNetResult()
        {
            Settings = new JsonSerializerSettings{
                ReferenceLoopHandling=ReferenceLoopHandling.Ignore,//忽略循环引用
                DateFormatString="yyyy-MM-dd HH:mm:ss",//日期格式化
                ContractResolver=new  Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()//Json中属性开头字母小写的驼峰命名
            };
        }
        public JsonSerializerSettings Settings { get; private set; }
        public override void ExecuteResult(ControllerContext context)
        {
            if (context==null)
                throw new ArgumentNullException("context");
            if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet && string.Equals(context.HttpContext.Request.HttpMethod, "Get", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Json GET is not allowed");
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrEmpty(this.ContentType) ? "appliaction/json" : this.ContentType;
            if (this.ContentEncoding != null)
                response.ContentEncoding = this.ContentEncoding;
            if (this.Data == null)
                return;
            var scriptSerialzer = JsonSerializer.Create(this.Settings);
            scriptSerialzer.Serialize(response.Output, this.Data);
        }
    }
}