using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Treasure.Utility.Utilitys
{
    public static class CamelName
    {

        #region 小驼峰法
        /// <summary>
        /// 小驼峰法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string getSmallCamelName(string input)
        {
            string[] arr = input.Split('_');

            StringBuilder name = new StringBuilder();

            for (int i = 0; i < arr.Length; i++)
            {
                if (i == 0)
                {
                    name.Append(arr[i].ToLower());
                }
                else
                {
                    name.Append(System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(arr[i].ToLower()));
                }

            }
            return name.ToString();
        }
        #endregion

        #region 大驼峰法
        /// <summary>
        /// 大驼峰法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string getBigCamelName(string input)
        {
            string[] arr = input.Split('_');

            StringBuilder name = new StringBuilder();

            for (int i = 0; i < arr.Length; i++)
            {
                name.Append(System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(arr[i].ToLower()));
            }

            return name.ToString();
        }
        #endregion

    }
}
