
using System;
using System.Diagnostics;
using System.IO;
using Treasure.Model.General;
namespace Treasure.Utility.Helpers
{
    /// <summary>
    /// 文件帮助类
    /// </summary>
    public class FileHelper
    {

        #region 写文件

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="pFilePath">文件路径，包含文件名</param>
        /// <param name="pContent">文件内容</param>
        public bool WriteFile(string pFilePath, string pContent)
        {
            return WriteFile(pFilePath, "", pContent);
        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="pFilePath">文件路径，包含文件名</param>
        /// <param name="pDescription">文件说明</param>
        /// <param name="pContent">文件内容</param>
        public bool WriteFile(string pFilePath, string pDescription, string pContent)
        {
            bool result = false;

            try
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory + pFilePath;

                FileInfo fileinfo = new FileInfo(filePath);

                using (FileStream fs = fileinfo.OpenWrite())
                {
                    StreamWriter sw = new StreamWriter(fs);
                    sw.BaseStream.Seek(0, SeekOrigin.End);

                    sw.Write(pDescription + ConstantVO.ENTER_STRING);
                    sw.WriteLine(pContent);

                    sw.Flush();
                    sw.Close();
                }

                result = true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }

            return result;
        }

        #endregion

        #region 没验证

        public int FileDelete(string SaveFolder, string SaveFileName, string Server, string Username, string Password)
        {
            Process process = new Process();
            try
            {
                process.StartInfo.FileName = "net.exe";
                process.StartInfo.Arguments = @"use \\" + Server + @"\" + SaveFolder + " \"" + Password + "\" /user:\"" + Username + "\" ";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.Start();
                System.Diagnostics.Debug.WriteLine(process.StartInfo.FileName + " " + process.StartInfo.Arguments);
                process.WaitForExit();
                string strTarget = @"\\" + Server + @"\" + SaveFolder + @"\" + SaveFileName;
                if (!File.Exists(strTarget))
                {
                    return 0;
                }
                System.Diagnostics.Debug.WriteLine("Start Delete File.....");
                File.Delete(strTarget);
                System.Diagnostics.Debug.WriteLine("End Delete File.....");
                process.StartInfo.Arguments = @"use \\" + Server + @"\" + SaveFolder + " /delete";
                process.Start();
                process.Close();
            }
            catch (IOException ex)
            {
                System.Diagnostics.Debug.WriteLine("in FileCopy IOException:" + ex.Message);
                return -1;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("in FileCopy Exception:" + ex.Message);
                return -1;
            }
            finally
            {
                process.Dispose();
            }
            return 0;
        }

        public int FileCopy(string strSource, string SaveFolder, string SaveFileName, string Server, string Username, string Password)
        {
            Process process = new Process();
            try
            {
                process.StartInfo.FileName = "net.exe";
                process.StartInfo.Arguments = @"use \\" + Server + @"\" + SaveFolder + " \"" + Password + "\" /user:\"" + Username + "\" ";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.Start();
                System.Diagnostics.Debug.WriteLine(process.StartInfo.FileName + " " + process.StartInfo.Arguments);
                process.WaitForExit();
                if (!Directory.Exists(@"\\" + Server + @"\" + SaveFolder))
                {
                    Directory.CreateDirectory(@"\\" + Server + @"\" + SaveFolder);
                }
                System.Diagnostics.Debug.WriteLine("Start Copy File.....");
                string strTarget = @"\\" + Server + @"\" + SaveFolder + @"\" + SaveFileName;
                File.Copy(strSource, strTarget, true);
                System.Diagnostics.Debug.WriteLine("End Copy File.....");
                process.StartInfo.Arguments = @"use \\" + Server + @"\" + SaveFolder + " /delete";
                process.Start();
                process.Close();
            }
            catch (IOException ex)
            {
                System.Diagnostics.Debug.WriteLine("in FileCopy IOException:" + ex.Message);
                return -1;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("in FileCopy Exception:" + ex.Message);
                return -1;
            }
            finally
            {
                process.Dispose();
            }
            return 0;
        }

        #endregion

    }
}
