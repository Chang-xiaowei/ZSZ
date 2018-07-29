using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.CommonMVC;

namespace ZSZ.Admin.Web.App_Start
{
    public class FilterConfig
    {
        public static void RegisterFilter(GlobalFilterCollection filter)
        {
            filter.Add(new ZSZAuthorizationFilter());
            filter.Add(new ZSZExceptionFilter());
            filter.Add(new JsonNetActionFilter());
        }
    }
}