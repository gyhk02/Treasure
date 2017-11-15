using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Treasure.Utility.Utilitys
{
    public static class TypeConversion
    {
        public static string TimeToString(this object value, string formatStr)
        {
            if (value == null)
                return "";

            if (string.IsNullOrEmpty(value.ToString()) == true)
                return "";

            DateTime d;
            DateTime.TryParse(value.ToString(), out d);
            return d.ToString(formatStr);
        }


        #region 对象转换成bool
        /// <summary>
        /// 对象转换成bool
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool ToBool(Object obj)
        {
            bool result = false;

            if (obj == null)
            {
                return result;
            }

            Boolean.TryParse(obj.ToString(), out result);

            return result;
        }
        #endregion

        #region 转小数，再转成字符串，带格式，比如#.##
        /// <summary>
        /// 转小数，再转成字符串，带格式，比如#.##
        /// </summary>
        /// <param name="value">要转换的对象</param>
        /// <param name="formatStr">转换后要求的格式</param>
        /// <returns>string</returns>
        public static string ToStringByFormat(this object value, string formatStr)
        {
            if (value == null)
                return "";

            double d;
            double.TryParse(value.ToString(), out d);
            return d.ToString(formatStr);
        }
        #endregion

        #region 将字符转成Decimal
        /// <summary>
        /// 将字符转成Decimal
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Decimal ToDecimal(Object obj)
        {
            Decimal result = 0;

            if (obj == null)
            {
                return 0;
            }

            if (Decimal.TryParse(obj.ToString(), out result) == true)
            {
                return result;
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region 将对象转成Double
        /// <summary>
        /// 将对象转成Double
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Double ToDouble(Object obj)
        {
            Double result = 0;

            if (obj == null)
            {
                return 0;
            }

            if (Double.TryParse(obj.ToString(), out result) == true)
            {
                return result;
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region 将字符转成Double
        /// <summary>
        /// 将字符转成Double
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Double ToDouble(string s)
        {
            Double result = 0;

            if (Double.TryParse(s, out result) == true)
            {
                return result;
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region 对象转换成int
        /// <summary>
        /// 对象转换成int
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ToInt(Object obj)
        {
            int result = 0;

            if (obj == null)
            {
                return 0;
            }

            if (Int32.TryParse(obj.ToString(), out result) == true)
            {
                return result;
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region 对象转成String
        /// <summary>
        /// 对象转成String
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToString(Object obj)
        {
            string result = "";

            if (obj == null)
            {
                return result;
            }
            else
            {
                return obj.ToString();
            }
        }
        #endregion

    }
}
