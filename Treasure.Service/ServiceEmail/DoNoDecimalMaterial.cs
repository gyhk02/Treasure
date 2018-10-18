using Quartz;
using System;
using System.Configuration;
using Treasure.BLL.Service;
using Treasure.Utility.Helpers;

namespace Treasure.Service.ServiceEmail
{
    public class DoNoDecimalMaterial : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            ToExecute();
        }


        public void ToExecute()
        {
            string subjectStr = "[可用利库不该有小数的物料查询]有异常数据" + DateTime.Now.ToString("yyyy-MM-dd");
            DateTime dToday = DateTime.Now;

            try
            {
                string pSubject = subjectStr + "(发送日期：" + dToday.ToString("yyyy.MM.dd") + ")";

                if (new DoNoDecimalMaterialBll().JudgeHasDecimalMaterial(ConfigurationManager.AppSettings["ServericeEmailConnectionString"]) == false)
                {
                    return;
                }

                string pBodyInfo = "可用利库不该有小数的物料查询 有异常数据";
                BasicEmai.SendEmailDo(pSubject, pBodyInfo);
            }
            catch (Exception ex)
            {
                string errorMsg = "错误的方法：DoNoDecimalMaterial.Execute <br />" + ex.Message;
                BasicEmai.SendEmailByOtherError(subjectStr, errorMsg);
                LogHelper.Error(subjectStr + "异常：" + errorMsg, System.Reflection.MethodBase.GetCurrentMethod());
            }
        }

    }
}
