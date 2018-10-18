using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Treasure.Utility.Helpers;

namespace Treasure.Service.ServiceEmail
{
    public class AutoSendEmail
    {
        public void SendEmail()
        {
            string subjectStr = "邮件自动发送服务";

            try
            {

                IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();

                /**
                IJobDetail jobSrbFifthDecimal = JobBuilder.Create<SrbFifthDecimal>().WithIdentity("jobSrbFifthDecimal").Build();
                ITrigger triggerSrbFifthDecimal = TriggerBuilder.Create().WithIdentity("triggerSrbFifthDecimal").ForJob(jobSrbFifthDecimal).StartNow()
                .WithCronSchedule("5 * * * * ?").Build();
                scheduler.ScheduleJob(jobSrbFifthDecimal, triggerSrbFifthDecimal);
    */

                IJobDetail jobDoNoDecimalMaterial = JobBuilder.Create<DoNoDecimalMaterial>().WithIdentity("jobDoNoDecimalMaterial").Build();
                ITrigger triggerDoNoDecimalMaterial = TriggerBuilder.Create().WithIdentity("triggerDoNoDecimalMaterial").ForJob(jobDoNoDecimalMaterial).StartNow()
                .WithCronSchedule("5 * * * * ?").Build();
                scheduler.ScheduleJob(jobDoNoDecimalMaterial, triggerDoNoDecimalMaterial);

                scheduler.Start();
            }
            catch (SchedulerException ex)
            {
                string errorMsg = "错误的方法：Service1.SendEmail <br />" + ex.Message;
                BasicEmai.SendEmailByOtherError(subjectStr, errorMsg);
                LogHelper.Error(subjectStr + "异常：" + errorMsg, System.Reflection.MethodBase.GetCurrentMethod());
            }
        }
    }
}
