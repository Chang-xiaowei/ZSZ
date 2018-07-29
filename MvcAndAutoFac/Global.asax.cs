using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcAndAutoFac
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();//把当前程序集中的Controller都注册
            Assembly[] assemblies = new Assembly[] {Assembly.Load("Service") };//获取相关类库的程序集
            builder.RegisterAssemblyTypes(assemblies).Where(type=>!type.IsAbstract).AsImplementedInterfaces().PropertiesAutowired();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));//注册系统级别得DependencyResolver,这样当MVC框架创建Controller等对象的时候都是管Autofac要对象
            GlobalFilters.Filters.Add(new JsonNetActionFilter());
        }
    }
}
