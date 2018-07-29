using Autofac;
using CaptchaGen;
using CodeCarvings.Piczard;
using CodeCarvings.Piczard.Filters.Watermarks;
using Common.Helper;
using IService;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShrinkandWatermark
{
    class Program
    {
        static void Main(string[] args)
        {
            //缩列图
            /*ImageProcessingJob jobThumb = new ImageProcessingJob();
            jobThumb.Filters.Add(new FixedResizeConstraint(200,200));//缩列图尺寸200*200
            TextWatermark txtWm = new TextWatermark();
            txtWm.Text = "如鹏网www.rupeng.com";
            txtWm.ContentAlignment = System.Drawing.ContentAlignment.TopCenter;
            jobThumb.Filters.Add(txtWm);
            jobThumb.SaveProcessedImageToFileSystem(@"D:\temp\1.png", @"D:\temp\3.png");*/

            #region 定时任务
            /*IScheduler sched = new StdSchedulerFactory().GetScheduler();
            JobDetailImpl jobBossReport = new JobDetailImpl("TestJob", typeof(TestJob));
            IMutableTrigger triggerBossReport = CronScheduleBuilder.AtHourAndMinuteOnGivenDaysOfWeek(11, 15, DayOfWeek.Friday, DayOfWeek.Sunday).Build();//CronScheduleBuilder.DailyAtHourAndMinute(11,21).Build();
            triggerBossReport.Key = new TriggerKey("triggerTest");
            sched.ScheduleJob(jobBossReport, triggerBossReport);
            sched.Start();*/
            #endregion
            #region AutoFac
            /* ContainerBuilder builder = new ContainerBuilder();
             builder.RegisterType<Service>().AsImplementedInterfaces();//.As<IService>();
             IService service = new Service();
             service.Test();
             IService2 service2 = new Service();
             service2.Test2();
             */
            ContainerBuilder builder = new ContainerBuilder();
            Assembly asm = Assembly.Load("Service");
            builder.RegisterAssemblyTypes(asm).AsImplementedInterfaces();

            ITest test = new Test();
            test.Test1();
            Console.ReadKey();

            #endregion



        }
    }
}
