using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZSZ.Admin.Web.App_Start
{
    //可以应用在方法上，而且可以添加多个
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CheckPermissonAttribute:Attribute
    {        
        public string Permission { get; set; }
        public CheckPermissonAttribute(string permission)
        {
            this.Permission = permission;
        }
    }
}