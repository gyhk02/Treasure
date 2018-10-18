using System;
using Treasure.Utility.Helpers;

namespace Treasure.Service.ServiceEmail
{
    public static class BasicEmai
    {
        #region 发送邮件实做
        /// <summary>
        /// 发送邮件实做
        /// </summary>
        public static void SendEmailDo(string pSubject, string pBodyInfo)
        {
            try
            {
                new EmailHelper().SendEmail(pSubject, pBodyInfo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString(), ex);
            }
        }
        #endregion

        #region 其它异常
        /// <summary>
        /// 其它异常
        /// </summary>
        public static void SendEmailByOtherError(string pEmailSubject, string pErrorMsg)
        {
            string pSubject = "【其它异常】";
            string pBodyInfo = "";
            if (string.IsNullOrEmpty(pEmailSubject) == true)
            {
                pBodyInfo = pErrorMsg;
            }
            else
            {
                pBodyInfo = "【" + pEmailSubject + "】邮件发送异常：" + pErrorMsg;
            }
            SendEmailDo(pSubject, pBodyInfo);
        }
        #endregion

    }
}
