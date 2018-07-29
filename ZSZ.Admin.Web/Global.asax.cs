using Autofac;
using Autofac.Integration.Mvc;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ZSZ.Admin.Web.App_Start;
using ZSZ.Admin.Web.Jobs;
using ZSZ.CommonMVC;
using ZSZIService;

namespace ZSZ.Admin.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            log4net.Config.XmlConfigurator.Configure();           
            ModelBinders.Binders.Add(typeof(string), new TrimToDBCModelBinder());
            ModelBinders.Binders.Add(typeof(int), new TrimToDBCModelBinder());
            ModelBinders.Binders.Add(typeof(long), new TrimToDBCModelBinder());
            ModelBinders.Binders.Add(typeof(double), new TrimToDBCModelBinder());
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();//把当前程序集中的Controller都注册
            Assembly[] assemblies = new Assembly[] { Assembly.Load("ZSZ.Service") };//获取相关类库的程序集
            builder.RegisterAssemblyTypes(assemblies).Where(type => !type.IsAbstract && typeof(IServiceSupport).IsAssignableFrom(type)).AsImplementedInterfaces().PropertiesAutowired();
            //type1.IsAssignableFrom(type2);type1类型的变量是否可以指向type2类型的对象，判断type2是否实现了type1这个接口/type2是否继承自type1 ,避免其他无关的类注册到AutoFac中
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            //权限验证
            /*
            GlobalFilters.Filters.Add(new ZSZAuthorizationFilter());
            GlobalFilters.Filters.Add(new ZSZExceptionFilter());
            GlobalFilters.Filters.Add(new JsonNetActionFilter());
            */
            FilterConfig.RegisterFilter(GlobalFilters.Filters);
            StartQuartz();
        }
        //启动定时任务
        private void StartQuartz()
        {
            IScheduler sched = new StdSchedulerFactory().GetScheduler();
            //给boss的报表开始
            JobDetailImpl jdBossReport = new JobDetailImpl("jdBossReport", typeof(BossReportJob));
            IMutableTrigger triggerBossReport = CronScheduleBuilder.DailyAtHourAndMinute(22,11).Build();//每天 23:45 执行一次 
            triggerBossReport.Key = new TriggerKey("triggerBossReport");
            sched.ScheduleJob(jdBossReport, triggerBossReport);
            //给boss的报表结束
            sched.Start();
        }
    }
}
