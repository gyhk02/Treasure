using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Treasure.Model.General;

namespace Treasure.Utility.Utilitys
{
    public static class TypeConversion
    {

        #region DataTable 序列化成字符串
        /// <summary>
        /// 序列化DataTable为String
        /// </summary>
        /// <param name="tb">包含数据的DataTable</param>
        /// <returns>序列化的DataTable</returns>
        public static string SerializeDataTableToString(this DataTable tb)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(sb);
            XmlSerializer serializer = new XmlSerializer(typeof(DataTable));
            serializer.Serialize(writer, tb);
            writer.Close();
            return sb.ToString();
        }
        #endregion

        #region 字符串序列化成DataTable
        /// <summary>
        /// 反序列化String为DataTable
        /// </summary>
        /// <param name="strXml">序列化的DataTable</param>
        /// <returns>DataTable</returns>
        public static DataTable DeserializeStringToDataTable(this string strXml)
        {
            StringReader strReader = new StringReader(strXml);
            XmlReader xmlReader = XmlReader.Create(strReader);
            XmlSerializer serializer = new XmlSerializer(typeof(DataTable));
            DataTable dt = serializer.Deserialize(xmlReader) as DataTable;
            return dt;
        }
        #endregion

        #region DataTable转换成实体类

        /// <summary>  
        /// 填充对象列表：用DataTable填充实体类
        /// </summary>  
        public static List<T> FillModel<T>(this DataTable dt) where T : new()
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            List<T> modelList = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                //T model = (T)Activator.CreateInstance(typeof(T));  
                T model = dr.FillModel<T>();

                modelList.Add(model);
            }
            return modelList;
        }

        /// <summary>  
        /// 填充对象：用DataRow填充实体类
        /// </summary>  
        public static T FillModel<T>(this DataRow dr) where T : new()
        {
            if (dr == null)
            {
                return default(T);
            }

            //T model = (T)Activator.CreateInstance(typeof(T));  
            T model = new T();

            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                PropertyInfo propertyInfo = model.GetType().GetProperty(dr.Table.Columns[i].ColumnName);
                if (propertyInfo != null && dr[i] != DBNull.Value)
                {
                    string v = dr[i].ToString();
                    if (propertyInfo.PropertyType.FullName.ToUpper().Contains("GUID"))
                    {
                        if (!string.IsNullOrEmpty(v))
                        {
                            propertyInfo.SetValue(model, Guid.Parse(v), null);
                        }
                        else
                        {
                            propertyInfo.SetValue(model, null, null);
                        }
                    }
                    else if (propertyInfo.PropertyType.FullName.ToUpper().Contains("BYTE"))
                    {
                        if (!string.IsNullOrEmpty(v))
                        {
                            propertyInfo.SetValue(model, byte.Parse(v), null);
                        }
                        else
                        {
                            propertyInfo.SetValue(model, null, null);
                        }
                    }
                    else
                    {
                        propertyInfo.SetValue(model, dr[i], null);
                    }
                }
            }
            return model;
        }

        #endregion

        #region 实体类转换成DataTable

        /// <summary>
        /// 实体类转换成DataTable
        /// </summary>
        /// <param name="modelList">实体类列表</param>
        /// <returns></returns>
        public static DataTable FillDataTable<T>(this List<T> modelList) where T : new()
        {
            if (modelList == null || modelList.Count == 0)
            {
                return null;
            }
            DataTable dt = CreateData<T>(modelList[0]);

            foreach (T model in modelList)
            {
                DataRow dataRow = dt.NewRow();
                foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
                {
                    dataRow[propertyInfo.Name] = propertyInfo.GetValue(model, null);
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        /// <summary>
        /// 根据实体类得到表结构
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        private static DataTable CreateData<T>(T model) where T : new()
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                dataTable.Columns.Add(new DataColumn(propertyInfo.Name, propertyInfo.PropertyType));
            }
            return dataTable;
        }

        #endregion


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

        public static DateTime? ToDateTime(this object value)
        {
            DateTime d;

            if (value == null)
                return null;

            if (string.IsNullOrEmpty(value.ToString()) == true)
                return null;

            DateTime.TryParse(value.ToString(), out d);
            return d;
        }

        #region 对象转换成bool
        /// <summary>
        /// 对象转换成bool
        /// </summary>
        /// <param name="DataColumnType"></param>
        /// <returns></returns>
        public static bool? ToBool(Object obj)
        {
            bool result = false;

            if (obj == null)
            {
                return null;
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
