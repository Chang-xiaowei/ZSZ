using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.Common;
using ZSZ.CommonMVC;
using ZSZ.IService;
using ZSZIService;

namespace ZSZ.Admin.Web.App_Start
{
    public class ZSZAuthorizationFilter:IAuthorizationFilter
    {
        /// <summary>
        /// 识别标签
        /// </summary>
        //  public IAdminUserService userService { get; set; }
        //注意ZSZAuthorizationFilter没有被AutoFac注入
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            //获得当前要执行的Action上标注的CheckPermissionAttribute实例对象
            CheckPermissonAttribute[]permAtts =(CheckPermissonAttribute[]) filterContext.ActionDescriptor.GetCustomAttributes(typeof(CheckPermissonAttribute),false);
            //得到当前登录用户的Id
            if (permAtts.Length<=0)//即没有标注任何的CheckPermissionAttribute,则就不需要检查是否登录
            {
                //无欲无求，如登录不要求登录
                return;
            }
            long? userId = (long?)filterContext.HttpContext.Session["LoginUserId"];
            if (userId==null)//连登陆都没有，就不能访问
            {
                //根据不同的请求，给予不同的返回格式，确保ajax请求，浏览器端也能收到Json格式
                //如果是Ajax请求就返回Json
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {                    
                    AjaxResult ajaxResult = new AjaxResult();
                    ajaxResult.Status = "redirect";
                    ajaxResult.Data = "/Main/Login";//重定向的地址
                    ajaxResult.ErrorMsg = "没有登录";
                    filterContext.Result = new JsonNetResult{Data=ajaxResult };
                }
                else
                {
                    filterContext.Result = new RedirectResult("~/Main/Login");//没有登录则转到登录界面
                }
                //filterContext.HttpContext.Response.Write("没有登录");//不建议使用这种
                //filterContext.HttpContext.Response.Redirect("");
                //filterContext.Result = new ContentResult() { Content="没有登录"};
               // filterContext.Result = new RedirectResult("~/Main/Login");//没有登录转到登录界面
                return;
            }
            //由于ZSZAuthorizationFilter不是被autofac创建的，因此不会自动进行属性的注入
            //需要手动获取Service对象
            IAdminUserService userService = DependencyResolver.
                Current.GetService<IAdminUserService>();

            //检查是否有权限
            foreach (var permAtt in permAtts)
            {
                //(long)userId (userId可空类型)等价于 userId.Value
                if (!userService.HasPermission(userId.Value,permAtt.Permission))
                {
                    //只要碰到任何一个没有的权限，就禁止访问
                    //在IAuthorizationFilter里面只要修改filterContext.Result，
                    //那么真正的Action方法就不会执行了
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        AjaxResult ajaxResult = new AjaxResult();
                        ajaxResult.Status = "error";
                        ajaxResult.ErrorMsg = "没有权限"+permAtt.Permission;                        
                        filterContext.Result = new JsonNetResult { Data = ajaxResult };
                        
                    }
                    else
                    {
                        filterContext.Result = new ContentResult()
                        { Content = "没有" + permAtt.Permission + "这个权限" };
                    }                   
                    return;
                }
            }           
        }
    }
}