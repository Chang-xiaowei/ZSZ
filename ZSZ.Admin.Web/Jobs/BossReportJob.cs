using Autofac;
using Autofac.Integration.Mvc;
using log4net;
using log4net.Repository.Hierarchy;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using ZSZIService;

namespace ZSZ.Admin.Web.Jobs
{
    public class BossReportJob : IJob
    {
        private static ILog logger = LogManager.GetLogger(typeof(BossReportJob));
        
        public void Execute(IJobExecutionContext context)
        {
            logger.Debug("准备收集新增房源数量");
            try
            {
                //统计每个城市当天新增房源的的数量
                var container = AutofacDependencyResolver.Current.ApplicationContainer;
                using (container.BeginLifetimeScope())
                {
                   string bossEmails;
                   string smtpServer, smtpUserName, smtpPassword,smtpEmail;
                   StringBuilder sbMsg = new StringBuilder();
                   var cityService = container.Resolve<ICityService>();
                   var houseService = container.Resolve<IHouseService>();                 
                   var settingService = container.Resolve<ISettingService>();
                   bossEmails = settingService.GetValue("老板邮箱");//获得老板邮箱可能多个
                   smtpServer = settingService.GetValue("SmtpServer");
                   smtpUserName = settingService.GetValue("SmtpUserName");
                   smtpPassword = settingService.GetValue("SmtpPassword");
                   smtpEmail = settingService.GetValue("SmtpEmail");
                    var cities = cityService.GetAll();//获得所有的城市
                    foreach (var city in cities)
                    {
                        long count = houseService.GetTodayNewHouseCount(city.Id);//获得今天所增加的房源数量
                        sbMsg.Append(city.Name).Append("新增房源的数量是：").Append(count).AppendLine();
                    }
                    logger.Debug("收集新增房源数量完成"+sbMsg);
                    //邮件发送，  要使用using System.Net.Mail;
                    using (MailMessage mailMessage = new MailMessage())
                    using (SmtpClient smtpClient = new SmtpClient(smtpServer))
                    {
                        //由于多个老板都想收到这个邮件，因此在配置中可以以分号分割
                        foreach (var bossEmail in bossEmails.Split(';'))
                        {
                            mailMessage.To.Add(bossEmail);
                        }                       
                        mailMessage.Body = sbMsg.ToString();
                        mailMessage.From = new MailAddress(smtpEmail);
                        mailMessage.Subject = "今日新增房源数量报表";
                        smtpClient.Credentials = new System.Net.NetworkCredential(smtpUserName, smtpPassword);//如果启用了“客户端授权码”，要用授权码代替密码 
                        smtpClient.Send(mailMessage);
                    }
                    logger.Debug("给老板发送新增房源数量报表完成");
                }

            }
            catch (Exception ex)
            {

                logger.Error("给老板发报表出错",ex);
            }
        }
    }
}