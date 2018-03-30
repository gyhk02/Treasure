using System;
using System.Data;
using Treasure.Model.General;

namespace Treasure.Utility.Utilitys
{
    public static class TypeConversion
    {

        #region CShare的字段类型转换成SQL字段类型
        /// <summary>
        /// CShare的字段类型转换成SQL字段类型
        /// </summary>
        /// <param name="fieldType">CShare的字段类型</param>
        /// <returns>SQL字段类型</returns>
        public static SqlDbType ToSqlDbType(Type fieldType)
        {
            SqlDbType type = new SqlDbType();

            switch (fieldType.Name)
            {
                case ConstantVO.SQLDBTYPE_STRING:
                    type = SqlDbType.NVarChar;
                    break;
                case ConstantVO.SQLDBTYPE_VARBINARY:
                    type = SqlDbType.VarBinary;
                    break;
                case ConstantVO.SQLDBTYPE_INT32:
                    type = SqlDbType.Int;
                    break;
                case ConstantVO.SQLDBTYPE_INT64:
                    type = SqlDbType.BigInt;
                    break;
                case ConstantVO.SQLDBTYPE_BIT:
                    type = SqlDbType.Bit;
                    break;
                case ConstantVO.SQLDBTYPE_DATETIME:
                    type = SqlDbType.DateTime;
                    break;
            }

            return type;
        }
        #endregion

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
        /// <param name="DataColumnType"></param>
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
