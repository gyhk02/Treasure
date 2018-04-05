using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Treasure.Main.SmallTool.EncryptAndDecrypt
{
    public static class EncryptHelper
    {
        /// <summary>
        /// 密钥
        /// </summary>
        static string encryptKey = "aRong";

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string EncryptString(string input, string sKey = "")
        {
            if (string.IsNullOrEmpty(sKey) == true)
            {
                sKey = encryptKey;
            }

            byte[] data = Encoding.UTF8.GetBytes(input);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                Byte[] dd = ASCIIEncoding.ASCII.GetBytes("dddddddd");
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                ICryptoTransform desencrypt = des.CreateEncryptor();
                byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
                return BitConverter.ToString(result);
            }
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string DecryptString(string input, string sKey = "")
        {
            if (string.IsNullOrEmpty(sKey) == true)
            {
                sKey = encryptKey;
            }

            string[] sInput = input.Split("-".ToCharArray());
            byte[] data = new byte[sInput.Length];
            for (int i = 0; i < sInput.Length; i++)
            {
                data[i] = byte.Parse(sInput[i], NumberStyles.HexNumber);
            }
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                ICryptoTransform desencrypt = des.CreateDecryptor();
                byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
                return Encoding.UTF8.GetString(result);
            }
        }
    }
}