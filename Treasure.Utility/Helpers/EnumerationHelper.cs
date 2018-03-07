using System;
using System.Reflection;
using System.ComponentModel;

namespace Treasure.Utility.Helpers
{
    public static class EnumerationHelper
    {
        #region 枚举

        public enum DBStructureType
        {
            [Description("P")]
            Procedure = 1,
            [Description("TF")]
            Function = 2,
        }

        #endregion

        #region 获取枚举的描述

        /// <summary>
        /// 获取枚举的描述
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public static string GetEnumDes(this Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }
            return en.ToString();
        }

        /// <summary>
        /// 获取枚举的描述
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string GetEnumDes<T>(int i)
        {
            var typeInfo = typeof(T);
            FieldInfo[] enumFields = typeInfo.GetFields();
            foreach (var entity in enumFields)
            {
                if (!entity.IsSpecialName)
                {
                    if (i.ToString() == entity.GetRawConstantValue().ToString())
                    {
                        object[] attrs = entity.GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (attrs != null && attrs.Length > 0)
                            return ((DescriptionAttribute)attrs[0]).Description;
                    }

                }
            }
            return "";
        }
        #endregion

    }
}
