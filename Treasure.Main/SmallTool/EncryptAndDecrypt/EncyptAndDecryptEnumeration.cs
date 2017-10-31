using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI.WebControls;

namespace Treasure.Main.SmallTool.EncryptAndDecrypt
{

    #region 把枚举写到这里

    public enum Encrypt_Decrypt_Type
    {
        [Description("子项目")]
        ChildItem = 1,

        [Description("Main项目")]
        MainItem = 2,

        [Description("AX ERP")]
        AxErp = 3,

        [Description("CF")]
        Cf = 4,

        [Description("RFID")]
        RFID = 5,

        [Description("JIT Weight")]
        JitWeight = 6
    }

    #endregion

    public static class EncyptAndDecryptEnumeration
    {
        /// <summary>
        /// 取得枚举的描述信息
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public static string GetEnumDes(this Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }
            return en.ToString();
        }

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
                        object[] attrs = entity.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                        if (attrs != null && attrs.Length > 0)
                            return ((DescriptionAttribute)attrs[0]).Description;
                    }

                }
            }
            return "";
        }

        /// <summary>
        /// 绑定试用枚举的ComboBox
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="pDropDownList"></param>
        public static void BindToEnum<T>(this DropDownList pDropDownList)
        {
            pDropDownList.DataSource = null;
            var typeInfo = typeof(T);
            FieldInfo[] enumFields = typeInfo.GetFields();
            DataTable table = new DataTable();
            table.Columns.Add("Name", Type.GetType("System.String"));
            table.Columns.Add("Value", Type.GetType("System.Int32"));

            DataRow row0 = table.NewRow();
            row0[0] = "请选择";
            row0[1] = "-1";
            table.Rows.Add(row0);

            foreach (var entity in enumFields)
            {
                if (!entity.IsSpecialName)
                {
                    DataRow row = table.NewRow();
                    object[] attrs = entity.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                    var str = ((DescriptionAttribute)attrs[0]).Description;
                    var value = entity.GetRawConstantValue().ToString();
                    row[0] = str;
                    row[1] = value;
                    table.Rows.Add(row);
                }
            }
            pDropDownList.DataSource = table;
            pDropDownList.DataTextField = "Name";
            pDropDownList.DataValueField = "Value";
            pDropDownList.DataBind();
        }
    }
}