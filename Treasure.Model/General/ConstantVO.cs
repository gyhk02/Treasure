using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Treasure.Model.General
{
    public static class ConstantVO
    {
        public static string ENTER_STRING = "\r\n";
        public static string ENTER_BR = "<br />";

        /// <summary>
        /// 平时显示的时间
        /// </summary>
        public static string DATETIME_Y_M_D_H_M_S = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// 时间：中间没有间隔
        /// </summary>
        public static string DATETIME_YMDHMS = "yyyyMMddHHmmss";

        /// <summary>
        /// 下划线隔开
        /// </summary>
        public static string DATETIME_Y_M_D_H_M_S_F = "yyyy_MM_dd_HH_mm_ss_fff";

        public static string OFFICIAL_VERSION = "正式版本";
        public static string TEST_VERSION = "测式版本";
        public static string DEVELOPMENT_VERSION = "开发版本";
    }
}
