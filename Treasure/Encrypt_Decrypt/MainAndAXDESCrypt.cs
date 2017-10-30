using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Treasure.Web.Treasure.Encrypt_Decrypt
{
    public class MainAndAXDESCrypt
    {

        private static string KEY = "?_MainProject$168";

        public static string Encrypt(string str)
        {
            return Encrypt(str, KEY);
        }

        public static string Decrypt(string str)
        {
            return Decrypt(str, KEY);
        }

        /// <summary>  
        /// DES加密方法  
        /// </summary>  
        /// <param name="strPlain">明文</param>  
        /// <param name="strDESKey">密钥</param>  
        /// <param name="strDESIV">向量</param>  
        /// <returns>密文</returns>  
        public static string Encrypt(string str, string key)
        {
            try
            {
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider(); provider.Key = Encoding.ASCII.GetBytes(key.Substring(0, 8)); provider.IV = Encoding.ASCII.GetBytes(key.Substring(0, 8)); byte[] bytes = Encoding.GetEncoding("GB2312").GetBytes(str); MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);
                stream2.Write(bytes, 0, bytes.Length); stream2.FlushFinalBlock();
                StringBuilder builder = new StringBuilder(); foreach (byte num in stream.ToArray())
                {
                    builder.AppendFormat("{0:X2}", num);
                }
                stream.Close();
                return builder.ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>  
        /// 进行DES解密。  
        /// </summary>  
        /// <param name="pToDecrypt">要解密的串</param>  
        /// <param name="sKey">密钥，且必须为8位。</param>  
        /// <returns>已解密的字符串。</returns>  
        public static string Decrypt(string str, string sKey)
        {
            try
            {
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider(); provider.Key = Encoding.ASCII.GetBytes(sKey.Substring(0, 8)); provider.IV = Encoding.ASCII.GetBytes(sKey.Substring(0, 8)); byte[] buffer = new byte[str.Length / 2]; for (int i = 0; i < (str.Length / 2); i++)
                {
                    int num2 = Convert.ToInt32(str.Substring(i * 2, 2), 0x10); buffer[i] = (byte)num2;
                }
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write);
                stream2.Write(buffer, 0, buffer.Length); stream2.FlushFinalBlock(); stream.Close();
                return Encoding.GetEncoding("GB2312").GetString(stream.ToArray());
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}