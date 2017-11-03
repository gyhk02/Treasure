using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Treasure.Utility.Helpers
{
    /// <summary>
    /// 文件日志
    /// </summary>
    public static class LogHelper
    {
        /// <summary>
        /// 记录日志且发送邮件
        /// </summary>
        /// <param name="pMessage">日志内容</param>
        /// <param name="pMethod">需要记录日志的方法</param>
        public static void Error(string pMessage, MethodBase pMethod)
        {
            Info(pMessage, pMethod);

            try
            {
                new EmailHelper().SendEmail("类" + pMethod.ReflectedType.FullName + "中方法" + pMethod.Name + "异常", pMessage);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="pMessage">日志内容</param>
        /// <param name="pMethod">需要记录日志的方法</param>
        public static void Info(string pMessage, MethodBase pMethod)
        {
            try
            {
                FileInfo fileinfo = new FileInfo(GetLogFilePath());

                using (FileStream fs = fileinfo.OpenWrite())
                {
                    StreamWriter sw = new StreamWriter(fs);
                    sw.BaseStream.Seek(0, SeekOrigin.End);
                    sw.WriteLine("=====================================");
                    sw.Write("日志时间为:" + DateTime.Now.ToString() + "\r\n");
                    sw.Write("日志内容为(" + pMethod.ReflectedType.FullName + "." + pMethod.Name + "):" + pMessage + "\r\n");
                    sw.WriteLine("=====================================");
                    sw.WriteLine("");
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        
        #region 获取日志文件路径
        /// <summary>
        /// 获取日志文件路径
        /// </summary>
        private static string GetLogFilePath()
        {
            string result = "";

            string path = "";
            DateTime today = DateTime.Now;

            path = AppDomain.CurrentDomain.BaseDirectory + "/Log";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = path + "/" + today.ToString("yyyyMM");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            result = path + "/" + today.ToString("yyyyMMdd") + ".txt";

            return result;
        }
        #endregion

    }
}
