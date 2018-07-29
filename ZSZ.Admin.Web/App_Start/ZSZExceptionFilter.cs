using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZSZ.Admin.Web.App_Start
{
    public class ZSZExceptionFilter:IExceptionFilter
    {
        private static ILog log = LogManager.GetLogger(typeof(ZSZExceptionFilter));
        public void OnException(ExceptionContext filterContext)
        {
            log.Debug("出现了未处理的异常", filterContext.Exception);
        }
    }
}