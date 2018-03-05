using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Treasure.Model.General
{
    /// <summary>
    /// 常量
    /// </summary>
    public static class ConstantVO
    {
        public static string pleaseSelect = "请选择";

        /// <summary>
        /// 回车、换行
        /// </summary>
        public static string ENTER_STRING = "\r\n";
        public static string ENTER_RN_JS = "\\r\\n";
        public static char ENTER_R = '\r';
        public static string ENTER_BR = "<br />";

        /// <summary>
        /// 平时显示的时间
        /// </summary>
        public const string DATETIME_Y_M_D_H_M_S = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// 时间：中间没有间隔
        /// </summary>
        public static string DATETIME_YMDHMS = "yyyyMMddHHmmss";

        /// <summary>
        /// 下划线隔开
        /// </summary>
        public static string DATETIME_Y_M_D_H_M_S_F = "yyyy_MM_dd_HH_mm_ss_fff";
        public static string DATETIMEYMDHMSF = "yyyyMMddHHmmssfff";

        public static string OFFICIAL_VERSION = "正式版本";
        public static string TEST_VERSION = "测式版本";
        public static string DEVELOPMENT_VERSION = "开发版本";

        public const string SQLDBTYPE_STRING = "String";
        public const string SQLDBTYPE_DATETIME = "DateTime";
        public const string SQLDBTYPE_INT32 = "Int32";
        public const string SQLDBTYPE_INT64 = "Int64";
        public const string SQLDBTYPE_VARBINARY = "Byte[]";
        public const string SQLDBTYPE_BIT = "Boolean";
    }
}
