using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Treasure.BLL.General
{
    public class CamelNameBLL
    {

        #region 小驼峰法
        /// <summary>
        /// 小驼峰法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string getSmallCamelName(string input)
        {
            string[] arr = input.Split('_');
            string name = "";
            for (int i = 0; i < arr.Length; i++)
            {
                if (i == 0)
                {
                    name += arr[i].ToLower();
                }
                else
                {
                    name += System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(arr[i].ToLower());
                }

            }
            return name;
        }
        #endregion

        #region 大驼峰法
        /// <summary>
        /// 大驼峰法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string getBigCamelName(string input)
        {
            string[] arr = input.Split('_');
            string name = "";
            for (int i = 0; i < arr.Length; i++)
            {
                name += System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(arr[i].ToLower());
            }
            return name;
        }
        #endregion

    }
}
