using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Treasure.Utility.Helpers
{
    public class EmailHelper
    {
        public MailMessage mMailMessage;   //主要处理发送邮件的内容（如：收发人地址、标题、主体、图片等等）
        private SmtpClient mSmtpClient; //主要处理用smtp方式发送此邮件的配置信息（如：邮件服务器、发送端口号、验证方式等等）
        private int mSenderPort = 25;   //发送邮件所用的端口号（htmp协议默认为25）
        private string mSenderServerHost = "evervan.com.cn";    //发件箱的邮件服务器地址（IP形式或字符串形式均可）
        private string mSenderPassword = "12Evq34";    //发件箱的密码
        private string mSenderUsername = "erp";   //发件箱的用户名（即@符号前面的字符串，例如：hello@163.com，用户名为：hello）
        private bool mEnableSsl = false;    //是否对邮件内容进行socket层加密传输
        private bool mEnablePwdAuthentication = false;  //是否对发件人邮箱进行密码验证
        private string mFromEmail = "erp@evervan.com.cn";
        private string mToEmail = "rongzhi.liu@evervan.com.cn";

        public void SendEmail(string pSubject, string pEmailBody)
        {
            try
            {
                Init(mSenderServerHost, mToEmail, mFromEmail, "", "", pSubject, pEmailBody, mSenderUsername, mSenderPassword, mSenderPort, mEnableSsl, mEnablePwdAuthentication);
                Send();
                mMailMessage.Dispose();
            }
            catch (Exception ex)
            {
                LogHelper.Info(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }
        }

        #region 初始化数据
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="pSenderServerHost"></param>
        /// <param name="toMail"></param>
        /// <param name="fromMail"></param>
        /// <param name="ccMail"></param>
        /// <param name="bccMail"></param>
        /// <param name="pSubject"></param>
        /// <param name="pEmailBody"></param>
        /// <param name="pSenderUsername"></param>
        /// <param name="pSenderPassword"></param>
        /// <param name="pSenderPort"></param>
        /// <param name="pEnableSsl"></param>
        /// <param name="pEnablePwdAuthentication"></param>
        private void Init(string pSenderServerHost, string toMail, string fromMail, string ccMail, string bccMail, string pSubject, string pEmailBody
            , string pSenderUsername, string pSenderPassword, int pSenderPort, bool pEnableSsl, bool pEnablePwdAuthentication)
        {
            mMailMessage = new MailMessage();

            string[] arr = toMail.Split(';');

            foreach (string address in arr)
                mMailMessage.To.Add(address);

            if (ccMail.Length > 0)
            {
                arr = ccMail.Split(';');
                foreach (string address in arr)
                    mMailMessage.CC.Add(address);
            }
            if (bccMail.Length > 0)
            {
                arr = bccMail.Split(';');
                foreach (string address in arr)
                    mMailMessage.Bcc.Add(address);
            }

            try
            {
                mMailMessage.From = new MailAddress(fromMail);
                mMailMessage.Subject = pSubject;
                mMailMessage.Body = pEmailBody;
                mMailMessage.IsBodyHtml = true;
                mMailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                mMailMessage.Priority = MailPriority.Normal;
                this.mSenderServerHost = pSenderServerHost;
                this.mSenderUsername = pSenderUsername;
                this.mSenderPassword = pSenderPassword;
                this.mSenderPort = pSenderPort;
                this.mEnableSsl = pEnableSsl;
                this.mEnablePwdAuthentication = pEnablePwdAuthentication;
            }
            catch (Exception ex)
            {
                LogHelper.Info(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }
        }
        #endregion

        #region 添加附件
        ///<summary>
        /// 添加附件
        ///</summary>
        ///<param name="attachmentsPath">附件的路径集合，以分号分隔</param>
        private void AddAttachments(string attachmentsPath)
        {
            try
            {
                string[] path = attachmentsPath.Split(';'); //分隔符可以自定义
                Attachment data;
                //ContentDisposition disposition;
                for (int i = 0; i < path.Length; i++)
                {
                    data = new Attachment(path[i], MediaTypeNames.Application.Octet);
                    /*
                    disposition = data.ContentDisposition;
                   
                    disposition.CreationDate = File.GetCreationTime(path[i]);
                    disposition.ModificationDate = File.GetLastWriteTime(path[i]);
                    disposition.ReadDate = File.GetLastAccessTime(path[i]);
                    */
                    mMailMessage.Attachments.Add(data);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }
        }
        #endregion

        #region 实现邮件的发送
        ///<summary>
        /// 实现邮件的发送
        ///</summary>
        private void Send()
        {
            try
            {
                if (mMailMessage != null)
                {
                    mSmtpClient = new SmtpClient();
                    mSmtpClient.Host = this.mSenderServerHost;
                    mSmtpClient.Port = this.mSenderPort;
                    mSmtpClient.UseDefaultCredentials = false;
                    mSmtpClient.EnableSsl = this.mEnableSsl;
                    if (this.mEnablePwdAuthentication)
                    {
                        System.Net.NetworkCredential nc = new System.Net.NetworkCredential(this.mSenderUsername, this.mSenderPassword);
                        mSmtpClient.Credentials = nc.GetCredential(mSmtpClient.Host, mSmtpClient.Port, "NTLM");
                    }
                    else
                    {
                        mSmtpClient.Credentials = new System.Net.NetworkCredential(this.mSenderUsername, this.mSenderPassword);
                    }
                    mSmtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    mSmtpClient.Send(mMailMessage);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }
        }
        #endregion

    }
}
