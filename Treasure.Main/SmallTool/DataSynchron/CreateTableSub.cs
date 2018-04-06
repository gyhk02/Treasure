using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Treasure.BLL.General;
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
        DataBaseBll bllDataBase = new DataBaseBll();

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

            StringBuilder strsql = new StringBuilder();

            foreach (string str in lst)
            {
                strsql.Append(GetFiledString(pSourceConnection, str));
                strsql.Append(GetConstraintStringNoF(pSourceConnection, str));
                strsql.Append(GetFiledDescriptionString(pSourceConnection, str));
                strsql.Append(GetTableDescriptionString(pSourceConnection, str) + ConstantVO.ENTER_STRING);
            }
            foreach (string str in lst)
            {
                strsql.Append(GetConstraintStringByF(pSourceConnection, str));
            }

            //记录sql语句
            string type = "";
            if (bll.CreateTable(pTargetConnection, strsql.ToString()) == true)
            {
                type = "正常";
            }
            else
            {
                type = "异常";
            }
            string filePath = "Document/DataSynchronSql/CreateTableStructure_" + type + "_" + DateTime.Now.ToString(ConstantVO.DATETIME_Y_M_D_H_M_S_F) + ".txt";
            string description = "在" + pTargetConnection + "上创建表" + string.Join(",", lst.ToArray());
            result = new FileHelper().WriteFile(filePath, description, strsql.ToString());

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

            DataTable dt = bllDataBase.GetTableInfoByName(4, pSourceTable, pSourceConnection);
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
                    + "' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'" + pSourceTable + "'";
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

            DataTable dt = bllDataBase.GetTableInfoByName(3, pSourceTable, pSourceConnection);
            foreach (DataRow row in dt.Rows)
            {
                sql = sql + @"
IF NOT EXISTS(
	select 1
	from sys.objects o
	join sys.columns c on o.object_id = c.object_id
	join sys.extended_properties ep on c.object_id = ep.major_id and c.column_id = ep.minor_id
	where o.name = '" + pSourceTable + @"' and c.name = '" + row[DataSynchronVO.FieldName].ToString() + @"' and ep.name = '" + row[DataSynchronVO.DescriptionName].ToString() + @"'
)
EXEC sys.sp_addextendedproperty @name=N'" + row[DataSynchronVO.DescriptionName].ToString()
                    + "', @value=N'" + row[DataSynchronVO.FieldDescription].ToString()
                    + "' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'"
                    + pSourceTable + "', @level2type=N'COLUMN',@level2name=N'" + row[DataSynchronVO.FieldName].ToString() + "'" + ConstantVO.ENTER_STRING;
            }

            result = sql;

            return result;
        }
        #endregion

        #region 获取除外键约束以外的字符串
        /// <summary>
        /// 获取除外键约束以外的字符串
        /// </summary>
        /// <param name="pSourceConnection"></param>
        /// <param name="pSourceTable"></param>
        /// <returns></returns>
        private string GetConstraintStringNoF(string pSourceConnection, string pSourceTable)
        {
            string result = "";

            StringBuilder sql = new StringBuilder();

            DataTable dt = bllDataBase.GetTableInfoByName(2, pSourceTable, pSourceConnection);
            foreach (DataRow row in dt.Rows)
            {
                string constraintType = row[DataSynchronVO.ConstraintType].ToString().Trim().ToUpper();

                if (constraintType.Equals("F") == true) { continue; }

                string constraintName = row[DataSynchronVO.ConstraintName].ToString();
                string fieldName = row[DataSynchronVO.FieldName].ToString();
                string foreignTableName = row[DataSynchronVO.ForeignTableName].ToString();
                string foreignFieldName = row[DataSynchronVO.ForeignFieldName].ToString();

                sql.Append("")
                   .Append("IF NOT EXISTS(")
                   .Append("	SELECT 1 FROM sys.objects WHERE name = '" + constraintName + "'")
                   .Append(")")
                   .Append("ALTER TABLE [dbo].[" + pSourceTable + "] ADD CONSTRAINT");

                switch (constraintType)
                {
                    case "D":
                        sql.Append(" " + constraintName)
                            .Append(" DEFAULT " + row[DataSynchronVO.DefaultValue].ToString())
                            .Append(" FOR [" + fieldName + "]");
                        break;
                    case "PK":
                        sql.Append(" " + constraintName + " PRIMARY KEY " + TypeConversion.ToString(row[DataSynchronVO.IndexDescripton]))
                            .Append(" (" + fieldName + ")");
                        break;
                    case "UQ":
                        sql.Append(" " + constraintName + " UNIQUE " + TypeConversion.ToString(row[DataSynchronVO.IndexDescripton]))
                            .Append("(" + fieldName + ")");
                        break;
                }

                sql.Append(ConstantVO.ENTER_STRING);
            }

            result = sql.ToString();

            return result;
        }
        #endregion

        #region 获取外键约束字符串
        /// <summary>
        /// 获取外键约束字符串
        /// </summary>
        /// <param name="pSourceConnection"></param>
        /// <param name="pSourceTable"></param>
        /// <returns></returns>
        private string GetConstraintStringByF(string pSourceConnection, string pSourceTable)
        {
            string result = "";

            StringBuilder sql = new StringBuilder();

            DataTable dt = bllDataBase.GetTableInfoByName(2, pSourceTable, pSourceConnection);
            foreach (DataRow row in dt.Rows)
            {
                string constraintType = row[DataSynchronVO.ConstraintType].ToString().Trim().ToUpper();

                if (constraintType.Equals("F") == false) { continue; }

                string constraintName = row[DataSynchronVO.ConstraintName].ToString();
                string fieldName = row[DataSynchronVO.FieldName].ToString();
                string foreignTableName = row[DataSynchronVO.ForeignTableName].ToString();
                string foreignFieldName = row[DataSynchronVO.ForeignFieldName].ToString();

                sql.Append("")
                    .Append("IF NOT EXISTS(")
                    .Append("	SELECT 1 FROM sys.objects WHERE name = '" + constraintName + "'")
                    .Append(")")
                    .Append("BEGIN")
                    .Append("    IF EXISTS(")
                    .Append("	    select * from sys.objects o")
                    .Append("	    join sys.columns c on o.object_id = c.object_id")
                    .Append("	    where o.type = 'U' and o.name = '" + foreignTableName + "' and c.name = '" + foreignFieldName + "'")
                    .Append("    )")
                    .Append("    ALTER TABLE [dbo].[" + pSourceTable + "] ADD CONSTRAINT " + constraintName)
                    .Append(" FOREIGN KEY (" + fieldName + ") REFERENCES [dbo].[")
                    .Append(foreignTableName + "] ([")
                    .Append(foreignFieldName + "])")
                    .Append("END")
                    .Append(ConstantVO.ENTER_STRING);
            }

            result = sql.ToString();

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
            string filedLen = "";

            StringBuilder sql = new StringBuilder();
            sql.Append("IF NOT EXISTS(")
                .Append("	select 1 from sys.objects where type = 'U' and name = '" + pSourceTable + "'")
                .Append(")")
                .Append("CREATE TABLE [dbo].[" + pSourceTable + "](" + ConstantVO.ENTER_STRING);

            DataTable dt = bllDataBase.GetTableInfoByName(1, pSourceTable, pSourceConnection);
            foreach (DataRow row in dt.Rows)
            {
                string filedtype = row[DataSynchronVO.FieldType].ToString().Trim().ToLower();

                //字段名+字段类型
                sql.Append(" [" + row[DataSynchronVO.FieldName].ToString() + "] [" + filedtype + "]");

                switch (filedtype)
                {
                    case "int":
                        if (TypeConversion.ToBool(row[DataSynchronVO.IsIdentity]) == true)
                        {
                            sql.Append(" IDENTITY(1,1)");
                        }
                        break;
                    case "numeric":
                        sql.Append("(" + row[DataSynchronVO.DecimalPrecision].ToString() + ", " + row[DataSynchronVO.DecimalDigits].ToString() + ")");
                        break;
                    case "nvarchar":
                    case "varchar":
                    case "char":
                    case "varbinary":
                        if (TypeConversion.ToInt(row[DataSynchronVO.IsMax]) == 1)
                        {
                            filedLen = "MAX";
                        }
                        else
                        {
                            filedLen = row[DataSynchronVO.FiledLen].ToString();
                        }
                        sql.Append("(" + filedLen + ")");
                        break;
                }

                //可为空
                if (TypeConversion.ToBool(row[DataSynchronVO.IsNullable]) == true)
                {
                    sql.Append(" NULL");
                }
                else
                {
                    sql.Append(" NOT NULL");
                }

                sql.Append(" ," + ConstantVO.ENTER_STRING);
            }

            sql.Append(")" + ConstantVO.ENTER_STRING);

            result = sql.ToString();

            return result;
        }
        #endregion

    }
}