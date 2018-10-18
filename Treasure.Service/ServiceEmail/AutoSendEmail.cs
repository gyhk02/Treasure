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

                IJobDetail job = JobBuilder.Create<SrbFifthDecimal>().WithIdentity("job").Build();
                ITrigger trigger = TriggerBuilder.Create().WithIdentity("trigger").ForJob(job).StartNow()
                .WithCronSchedule("5 * * * * ?").Build();
                scheduler.ScheduleJob(job, trigger);

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
