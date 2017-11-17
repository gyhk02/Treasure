using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Treasure.BLL.SmallTool.DataSynchron;
using Treasure.Model.General;
using Treasure.Model.SmallTool.DataSynchron;
using Treasure.Utility.Helpers;
using Treasure.Utility.Utilitys;

namespace Treasure.Main.SmallTool.DataSynchron
{
    public class CreateTableSub
    {
        DataSynchronBLL bll = new DataSynchronBLL();

        #region 创建表
        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="pSourceConnection"></param>
        /// <param name="pSourceTable"></param>
        /// <param name="pTargetConnection"></param>
        /// <returns></returns>
        public bool CreateTable(string pSourceConnection, string pTargetConnection, List<string> lstSourceTable, List<string> lstTargetTable)
        {
            bool result = false;

            if (lstSourceTable.Count == 0) { return result; }

            List<string> lst = lstSourceTable.Except(lstTargetTable).ToList();
            if (lst.Count == 0) { return result; }

            string strsql = "";
            foreach (string str in lst)
            {
                strsql = strsql + GetFiledString(pSourceConnection, str);
                strsql = strsql + GetConstraintString(pSourceConnection, str);
                strsql = strsql + GetFiledDescriptionString(pSourceConnection, str);
                strsql = strsql + GetTableDescriptionString(pSourceConnection, str) + ConstantVO.ENTER_STRING;
            }

            //记录sql语句
            string type = "";
            if (bll.CreateTable(pTargetConnection, strsql) == true)
            {
                type = "正常";
            }
            else
            {
                type = "异常";
            }
            string filePath = "Document/DataSynchronSql/CreateTableStructure_" + type + "_" + DateTime.Now.ToString(ConstantVO.DATETIME_Y_M_D_H_M_S_F) + ".txt";
            string description = "在" + pTargetConnection + "上创建表" + string.Join(",", lst.ToArray());
            result = new FileHelper().WriteFile(filePath, description, strsql);

            return result;
        }
        #endregion

        #region 获取表描述的字符串
        /// <summary>
        /// 获取表描述的字符串
        /// </summary>
        /// <param name="pSourceConnection"></param>
        /// <param name="pSourceTable"></param>
        /// <returns></returns>
        private string GetTableDescriptionString(string pSourceConnection, string pSourceTable)
        {
            string result = "";

            string sql = "";

            DataTable dt = bll.GetTableInfoByName(4, pSourceConnection, pSourceTable);
            foreach (DataRow row in dt.Rows)
            {
                sql = sql + @"
IF NOT EXISTS (
	select 1
	from sys.objects o
	join sys.extended_properties ep on o.object_id = ep.major_id and ep.minor_id = 0
	where o.name = '" + pSourceTable + @"'
)
EXEC sys.sp_addextendedproperty @name=N'" + row[DataSynchronVO.DescriptionName].ToString()
                    + "', @value=N'" + row[DataSynchronVO.TableDescription].ToString()
                    + "' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'" + pSourceTable + "'" + @"
GO
";
            }

            result = sql;

            return result;
        }
        #endregion

        #region 获取字段描述的字符串
        /// <summary>
        /// 获取字段描述的字符串
        /// </summary>
        /// <param name="pSourceConnection"></param>
        /// <param name="pSourceTable"></param>
        /// <returns></returns>
        private string GetFiledDescriptionString(string pSourceConnection, string pSourceTable)
        {
            string result = "";

            string sql = "";

            DataTable dt = bll.GetTableInfoByName(3, pSourceConnection, pSourceTable);
            foreach (DataRow row in dt.Rows)
            {
                sql = sql + @"
IF NOT EXISTS(
	select 1
	from sys.objects o
	join sys.columns c on o.object_id = c.object_id
	join sys.extended_properties ep on c.object_id = ep.major_id and c.column_id = ep.minor_id
	where o.name = '" + pSourceTable + @"' and c.name = '" + row[DataSynchronVO.FiledName].ToString() + @"' and ep.name = '" + row[DataSynchronVO.DescriptionName].ToString() + @"'
)
EXEC sys.sp_addextendedproperty @name=N'" + row[DataSynchronVO.DescriptionName].ToString()
                    + "', @value=N'" + row[DataSynchronVO.FiledDescription].ToString()
                    + "' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'"
                    + pSourceTable + "', @level2type=N'COLUMN',@level2name=N'" + row[DataSynchronVO.FiledName].ToString() + "'" + ConstantVO.ENTER_STRING
                    + @"
GO";
            }

            result = sql;

            return result;
        }
        #endregion

        #region 获取约束的字符串
        /// <summary>
        /// 获取约束的字符串
        /// </summary>
        /// <param name="pSourceConnection"></param>
        /// <param name="pSourceTable"></param>
        /// <returns></returns>
        private string GetConstraintString(string pSourceConnection, string pSourceTable)
        {
            string result = "";

            string sql = "";

            DataTable dt = bll.GetTableInfoByName(2, pSourceConnection, pSourceTable);
            foreach (DataRow row in dt.Rows)
            {
                string constraintType = row[DataSynchronVO.ConstraintType].ToString().Trim().ToUpper();

                sql = sql + "ALTER TABLE [dbo].[" + pSourceTable + "] ADD CONSTRAINT";

                switch (constraintType)
                {
                    case "D":
                        sql = sql + " " + row[DataSynchronVO.ConstraintName].ToString()
                            + " DEFAULT " + row[DataSynchronVO.DefaultValue].ToString()
                            + " FOR [" + row[DataSynchronVO.FiledName].ToString() + "]";
                        break;
                    case "F":
                        sql = sql + " " + row[DataSynchronVO.ConstraintName].ToString()
                            + " FOREIGN KEY (" + row[DataSynchronVO.FiledName].ToString() + ") REFERENCES [dbo].["
                            + row[DataSynchronVO.ForeignTableName].ToString() + "] (["
                            + row[DataSynchronVO.ForeignFiledName].ToString() + "])";
                        break;
                    case "PK":
                        sql = sql + " " + row[DataSynchronVO.ConstraintName].ToString() + " PRIMARY KEY " + TypeConversion.ToString(row[DataSynchronVO.IndexDescripton])
                            + " (" + row[DataSynchronVO.FiledName].ToString() + ")";
                        break;
                    case "UQ":
                        sql = sql + " " + row[DataSynchronVO.ConstraintName].ToString() + " UNIQUE " + TypeConversion.ToString(row[DataSynchronVO.IndexDescripton])
                            + "(" + row[DataSynchronVO.FiledName].ToString() + ")";
                        break;
                }

                sql = sql + @"
GO" + ConstantVO.ENTER_STRING;
            }

            result = sql;

            return result;
        }
        #endregion

        #region 获取创建表的字符串
        /// <summary>
        /// 获取创建表的字符串
        /// </summary>
        /// <param name="pSourceConnection">数据库链接</param>
        /// <param name="pSourceTable">表名</param>
        /// <returns>string</returns>
        private string GetFiledString(string pSourceConnection, string pSourceTable)
        {
            string result = "";

            string sql = "CREATE TABLE [dbo].[" + pSourceTable + "](" + ConstantVO.ENTER_STRING;

            DataTable dt = bll.GetTableInfoByName(1, pSourceConnection, pSourceTable);
            foreach (DataRow row in dt.Rows)
            {
                string filedtype = row[DataSynchronVO.FiledType].ToString().Trim().ToLower();

                //字段名+字段类型
                sql = sql + " [" + row[DataSynchronVO.FiledName].ToString() + "] [" + filedtype + "]";

                switch (filedtype)
                {
                    case "int":
                        if (TypeConversion.ToBool(row[DataSynchronVO.IsIdentity]) == true)
                        {
                            sql = sql + " IDENTITY(1,1)";
                        }
                        break;
                    case "numeric":
                        sql = sql + "(" + row[DataSynchronVO.DecimalPrecision].ToString() + ", " + row[DataSynchronVO.DecimalDigits].ToString() + ")";
                        break;
                    case "nvarchar":
                    case "varchar":
                    case "char":
                        sql = sql + "(" + row[DataSynchronVO.FiledLen].ToString() + ")";
                        break;
                }

                //可为空
                if (TypeConversion.ToBool(row[DataSynchronVO.IsNullable]) == true)
                {
                    sql = sql + " NULL";
                }
                else
                {
                    sql = sql + " NOT NULL";
                }

                sql = sql + " ," + ConstantVO.ENTER_STRING;
            }

            sql = sql + ")" + ConstantVO.ENTER_STRING;

            result = sql;

            return result;
        }
        #endregion

    }
}