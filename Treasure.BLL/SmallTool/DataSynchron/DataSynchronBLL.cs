﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using Treasure.Bll.General;
using Treasure.Model.General;
using Treasure.Model.SmallTool.DataSynchron;
using Treasure.Utility.Helpers;
using Treasure.Utility.Utilitys;

namespace Treasure.Bll.SmallTool.DataSynchron
{
    /// <summary>
    /// 数据同步类
    /// </summary>
    public class DataSynchronBll : BasicBll
    {
        #region 函数同步
        /// <summary>
        /// 函数同步
        /// </summary>
        /// <param name="pSourceConnection">源数据库链接</param>
        /// <param name="pTargetConnection">目标数据库链接</param>
        /// <param name="pType">类型 1:存储过程 2:函数</param>
        /// <param name="pName">函数名称</param>
        /// <returns></returns>
        public bool SynchronFunction(string pSourceConnection, string pTargetConnection, int pType, string pName)
        {
            bool result = false;

            string strsql = GetProcedureOrFunctionText(pSourceConnection, pType, pName);

            if (JudgeExistProcedureOrFunction(pTargetConnection, pType, pName) == true)
            {
                strsql = strsql.Replace("CREATE FUNCTION", "ALTER FUNCTION");
            }

            try
            {
                SqlHelper.ExecuteNonQuery(pTargetConnection, CommandType.Text, strsql, null);
                result = true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }

            //记录sql语句
            string type = "";
            if (result == true)
            {
                type = "正常";
            }
            else
            {
                type = "异常";
            }
            string filePath = "Document/DataSynchronSql/SynchronFunction_" + type + "_" + DateTime.Now.ToString(ConstantVO.DATETIMEYMDHMSF) + "_" + pName + ".txt";
            string description = "函数" + pName + "：从" + pSourceConnection + "到" + pTargetConnection;
            new FileHelper().WriteFile(filePath, description, strsql);

            return result;
        }
        #endregion

        #region 存储过程同步
        /// <summary>
        /// 存储过程同步
        /// </summary>
        /// <param name="pSourceConnection">源数据库链接</param>
        /// <param name="pTargetConnection">目标数据库链接</param>
        /// <param name="pType">类型 1:存储过程 2:函数</param>
        /// <param name="pName">函数名称</param>
        /// <returns></returns>
        public bool SynchronProcedure(string pSourceConnection, string pTargetConnection, int pType, string pName)
        {
            bool result = false;

            string strsql = GetProcedureOrFunctionText(pSourceConnection, pType, pName);

            if (JudgeExistProcedureOrFunction(pTargetConnection, pType, pName) == true)
            {
                strsql = strsql.Replace("CREATE PROCEDURE", "ALTER PROCEDURE");
            }

            try
            {
                SqlHelper.ExecuteNonQuery(pTargetConnection, CommandType.Text, strsql, null);
                result = true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }

            //记录sql语句
            string type = "";
            if (result == true)
            {
                type = "正常";
            }
            else
            {
                type = "异常";
            }
            string filePath = "Document/DataSynchronSql/SynchronProcedure_" + type + "_" + DateTime.Now.ToString(ConstantVO.DATETIMEYMDHMSF) + "_" + pName + ".txt";
            string description = "存储过程" + pName + "：从" + pSourceConnection + "到" + pTargetConnection;
            new FileHelper().WriteFile(filePath, description, strsql);

            return result;
        }
        #endregion

        #region 插入数据_增量同步
        /// <summary>
        /// 插入数据_增量同步
        /// </summary>
        /// <param name="pSourceConnection">源数据库链接</param>
        /// <param name="pTargetConnection">目标数据库链接</param>
        /// <param name="pTableName">表名</param>
        /// <returns>bool</returns>
        public bool InsertIncrementData(string pSourceConnection, string pTargetConnection, string pTableName, List<DataRow> pLstRow)
        {
            bool result = false;

            StringBuilder strTmp = new StringBuilder();

            DataColumnCollection ColumnCollection = pLstRow[0].Table.Columns;

            if (pLstRow.Count > 0)
            {
                StringBuilder strsql = new StringBuilder();
                strsql.Append(" IF OBJECTPROPERTY(OBJECT_ID('").Append(pTableName).Append("'),'TableHasIdentity') = 1 ").Append(ConstantVO.ENTER_STRING)
                    .Append(" SET IDENTITY_INSERT [").Append(pTableName).Append("] ON ").Append(ConstantVO.ENTER_STRING);

                //拼接字段名称
                strTmp.Length = 0;

                foreach (DataColumn col in ColumnCollection)
                {
                    strTmp.Append(",[").Append(col.ToString()).Append("]");
                }
                strsql.Append(" INSERT INTO [").Append(pTableName).Append("](").Append(strTmp.Remove(0, 1)).Append(")").Append(ConstantVO.ENTER_STRING);

                //拼接数据
                int cels = ColumnCollection.Count;
                for (int idx = 0; idx < pLstRow.Count; idx++)
                {
                    DataRow row = pLstRow[idx];

                    strTmp.Length = 0;
                    for (int idy = 0; idy < cels; idy++)
                    {
                        if (row[idy] == DBNull.Value)
                        {
                            strTmp.Append(",NULL");
                        }
                        else
                        {
                            strTmp.Append(",'").Append(row[idy].ToString()).Append("'");
                        }
                    }
                    strsql.Append(" SELECT ").Append(strTmp.Remove(0, 1));
                    if (idx != pLstRow.Count - 1)
                    {
                        strsql.Append(" UNION ALL ").Append(ConstantVO.ENTER_STRING);
                    }
                }

                strsql.Append(ConstantVO.ENTER_STRING).Append(" IF OBJECTPROPERTY(OBJECT_ID('").Append(pTableName).Append("'),'TableHasIdentity') = 1 ").Append(ConstantVO.ENTER_STRING)
                      .Append(" SET IDENTITY_INSERT [").Append(pTableName).Append("] OFF ");

                try
                {
                    SqlHelper.ExecuteNonQuery(pTargetConnection, CommandType.Text, strsql.ToString(), null);
                    result = true;
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
                }

                //记录sql语句
                string type = "";
                if (result == true)
                {
                    type = "正常";
                }
                else
                {
                    type = "异常";
                }
                string filePath = "Document/DataSynchronSql/DataIncrementSynchron_" + type + "_" + DateTime.Now.ToString(ConstantVO.DATETIMEYMDHMSF) + "_" + pTableName + ".txt";
                string description = "表" + pTableName + "：从" + pSourceConnection + "到" + pTargetConnection;
                new FileHelper().WriteFile(filePath, description, strsql.ToString());

                return result;
            }

            result = true;

            return result;
        }
        #endregion

        #region 插入数据_增量同步_带文件
        /// <summary>
        /// 插入数据_增量同步_带文件_增量同步
        /// </summary>
        /// <param name="pSourceConnection">源数据库链接</param>
        /// <param name="pTargetConnection">目标数据库链接</param>
        /// <param name="pTableName">表名</param>
        /// <returns>bool</returns>
        public void InsertDataIncrementByByte(string pSourceConnection, string pTargetConnection, string pTableName, List<DataRow> pLstRow)
        {
            DataColumnCollection ColumnCollection = pLstRow[0].Table.Columns;

            if (pLstRow.Count > 0)
            {
                //插入数据
                StringBuilder strParam = new StringBuilder();
                StringBuilder strValue = new StringBuilder();
                foreach (DataColumn col in ColumnCollection)
                {
                    strParam.Append(",[").Append(col.ToString()).Append("]");
                    strValue.Append(",@").Append(col.ToString()).Append("");
                }
                strParam.Remove(0, 1);
                strValue.Remove(0, 1);

                string sql = @"
IF OBJECTPROPERTY(OBJECT_ID('" + pTableName + @"'),'TableHasIdentity') = 1
SET IDENTITY_INSERT [" + pTableName + @"] ON

INSERT INTO " + pTableName + "(" + strParam + ") VALUES(" + strValue + @")

IF OBJECTPROPERTY(OBJECT_ID('" + pTableName + @"'),'TableHasIdentity') = 1
SET IDENTITY_INSERT [" + pTableName + @"] OFF ";

                int cels = ColumnCollection.Count;
                SqlParameter[] paras;
                SqlDbType type = new SqlDbType();
                for (int idx = 0; idx < pLstRow.Count; idx++)
                {
                    paras = new SqlParameter[cels];
                    DataRow row = pLstRow[idx];

                    for (int idy = 0; idy < cels; idy++)
                    {
                        switch (ColumnCollection[idy].DataType.Name)
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

                        paras[idy] = new SqlParameter("@" + ColumnCollection[idy], type) { Value = row[idy] };
                    }

                    try
                    {
                        SqlHelper.ExecuteNonQuery(pTargetConnection, CommandType.Text, sql, paras);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());

                        string errorMsg = "插入数据到" + pTableName + "表异常，对应的第一个字段值为" + row[0].ToString();
                        Page page = (Page)System.Web.HttpContext.Current.Handler;
                        page.ClientScript.RegisterStartupScript(page.GetType(), "失败", "<script>alert('" + errorMsg + "');</script>");
                    }
                }
            }
        }
        #endregion

        #region 插入数据
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="pSourceConnection">源数据库链接</param>
        /// <param name="pTargetConnection">目标数据库链接</param>
        /// <param name="pTableName">表名</param>
        /// <returns>bool</returns>
        public bool InsertData(string pSourceConnection, string pTargetConnection, string pTableName)
        {
            bool result = false;
            StringBuilder strTmp = new StringBuilder();

            DataTable dt = base.GetTableAllInfo(pSourceConnection, pTableName);
            if (dt.Rows.Count > 0)
            {
                StringBuilder strsql = new StringBuilder();
                strsql.Append(" IF OBJECTPROPERTY(OBJECT_ID('").Append(pTableName).Append("'),'TableHasIdentity') = 1 ").Append(ConstantVO.ENTER_STRING)
                    .Append(" SET IDENTITY_INSERT [").Append(pTableName).Append("] ON ").Append(ConstantVO.ENTER_STRING);

                strTmp.Length = 0;
                foreach (DataColumn col in dt.Columns)
                {
                    strTmp.Append(",[").Append(col.ToString()).Append("]");
                }
                strsql.Append(" INSERT INTO [").Append(pTableName).Append("](").Append(strTmp.Remove(0, 1)).Append(")").Append(ConstantVO.ENTER_STRING);

                int cels = dt.Columns.Count;
                for (int idx = 0; idx < dt.Rows.Count; idx++)
                {
                    DataRow row = dt.Rows[idx];

                    strTmp.Length = 0;
                    for (int idy = 0; idy < cels; idy++)
                    {
                        if (row[idy] == DBNull.Value)
                        {
                            strTmp.Append(",NULL");
                        }
                        else
                        {
                            strTmp.Append(",'").Append(row[idy].ToString()).Append("'");
                        }
                    }
                    strsql.Append(" SELECT ").Append(strTmp.Remove(0, 1));
                    if (idx != dt.Rows.Count - 1)
                    {
                        strsql.Append(" UNION ALL ").Append(ConstantVO.ENTER_STRING);
                    }
                }

                strsql.Append(ConstantVO.ENTER_STRING).Append(" IF OBJECTPROPERTY(OBJECT_ID('").Append(pTableName).Append("'),'TableHasIdentity') = 1 ").Append(ConstantVO.ENTER_STRING)
                       .Append(" SET IDENTITY_INSERT [").Append(pTableName).Append("] OFF ");

                try
                {
                    SqlHelper.ExecuteNonQuery(pTargetConnection, CommandType.Text, strsql.ToString(), null);
                    result = true;
                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message + @"

也许再次同步表的数据即可。";
                    LogHelper.Error(errorMsg, System.Reflection.MethodBase.GetCurrentMethod());
                }

                //记录sql语句
                string type = "";
                if (result == true)
                {
                    type = "正常";
                }
                else
                {
                    type = "异常";
                }
                string filePath = "Document/DataSynchronSql/DataSynchron_" + type + "_" + DateTime.Now.ToString(ConstantVO.DATETIMEYMDHMSF) + "_" + pTableName + ".txt";
                string description = "表" + pTableName + "：从" + pSourceConnection + "到" + pTargetConnection;
                new FileHelper().WriteFile(filePath, description, strsql.ToString());

                return result;
            }

            result = true;

            return result;
        }
        #endregion

        #region 插入数据_带文件
        /// <summary>
        /// 插入数据_带文件
        /// </summary>
        /// <param name="pSourceConnection">源数据库链接</param>
        /// <param name="pTargetConnection">目标数据库链接</param>
        /// <param name="pTableName">表名</param>
        /// <returns>bool</returns>
        public bool InsertDataByByte(string pSourceConnection, string pTargetConnection, string pTableName)
        {
            DataTable dt = GetTableAllInfo(pSourceConnection, pTableName);
            if (dt.Rows.Count > 0)
            {
                //删除数据

                //插入数据
                StringBuilder strParam = new StringBuilder();
                StringBuilder strValue = new StringBuilder();
                foreach (DataColumn col in dt.Columns)
                {
                    strParam.Append(",[").Append(col.ToString()).Append("]");
                    strValue.Append(",@").Append(col.ToString()).Append("");
                }
                strParam.Remove(strParam.Length - 1, 1);
                strValue.Remove(strValue.Length - 1, 1);

                string sql = @"
IF OBJECTPROPERTY(OBJECT_ID('" + pTableName + @"'),'TableHasIdentity') = 1
SET IDENTITY_INSERT [" + pTableName + @"] ON

INSERT INTO " + pTableName + "(" + strParam + ") VALUES(" + strValue + @")

IF OBJECTPROPERTY(OBJECT_ID('" + pTableName + @"'),'TableHasIdentity') = 1
SET IDENTITY_INSERT [" + pTableName + @"] OFF ";

                int cels = dt.Columns.Count;
                SqlParameter[] paras;
                for (int idx = 0; idx < dt.Rows.Count; idx++)
                {
                    paras = new SqlParameter[cels];
                    DataRow row = dt.Rows[idx];

                    for (int idy = 0; idy < cels; idy++)
                    {
                        SqlDbType type = TypeConversion.ToSqlDbType(dt.Columns[idy].DataType);

                        paras[idy] = new SqlParameter("@" + dt.Columns[idy], type) { Value = row[idy] };
                    }

                    try
                    {
                        SqlHelper.ExecuteNonQuery(pTargetConnection, CommandType.Text, sql, paras);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());

                        string errorMsg = "插入数据到" + pTableName + "表异常，对应的第一个字段值为" + row[0].ToString();
                        Page page = (Page)System.Web.HttpContext.Current.Handler;
                        page.ClientScript.RegisterStartupScript(page.GetType(), "失败", "<script>alert('" + errorMsg + "');</script>");
                    }
                }
            }

            return true;
        }
        #endregion

        #region 创建表
        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="pConnection">数据库链接</param>
        /// <param name="pSql">sql语句</param>
        /// <returns></returns>
        public bool CreateTable(string pConnection, string pSql)
        {
            bool result = false;

            try
            {
                SqlHelper.ExecuteNonQuery(pConnection, CommandType.Text, pSql, null);
                result = true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }
            return result;
        }
        #endregion

        #region 获取表名、表描述类型、表描述

        /// <summary>
        /// 获取表名、表描述类型、表描述
        /// </summary>
        /// <param name="pConnection">数据库链接</param>
        /// <param name="pTableList">表名列表</param>
        /// <returns></returns>
        public DataTable Get_TableName_DescriptionType_Description(string pConnection, List<string> pTableList)
        {
            return Get_TableName_DescriptionType_Description(pConnection, pTableList, null);
        }

        /// <summary>
        /// 获取表名、表描述类型、表描述
        /// </summary>
        /// <param name="pConnection">数据库链接</param>
        /// <param name="pTableList">表名列表</param>
        /// <param name="pTableName">
        /// 表名
        /// 逗号隔开：精确查询
        /// 没用逗号：模糊查询
        /// </param>
        /// <returns></returns>
        private DataTable Get_TableName_DescriptionType_Description(string pConnection, List<string> pTableList, string pTableName)
        {
            DataTable result = null;

            string condition = "";

            if (string.IsNullOrEmpty(pTableName) == false)
            {
                if (pTableName.Contains(",") == true)
                {
                    condition = " and o.name in('" + pTableName.Replace(",", "','") + "')";
                }
                else
                {
                    condition = " and o.name like '%" + pTableName + "%'";
                }
            }

            if (pTableList != null && pTableList.Count > 0)
            {
                string str = string.Join("','", pTableList.ToArray());
                condition = "'" + str + "'";
                condition = " and o.name in(" + condition + ")";
            }

            string sql = @"
select o.name " + DataSynchronVO.TableName + @", ep.name " + DataSynchronVO.DescriptionName + @", ep.value " + DataSynchronVO.TableDescription + @"
from sys.objects o
left join sys.extended_properties ep on o.object_id = ep.major_id and ep.minor_id = 0
where o.type = 'U' " + condition;

            result = SqlHelper.ExecuteDataTable(pConnection, CommandType.Text, sql, null);

            return result;
        }

        #endregion

        #region 获取存储过程或函数的名称和描述
        /// <summary>
        /// 获取存储过程或函数的名称和描述
        /// </summary>
        /// <param name="pConnection">数据库链接</param>
        /// <param name="pType">类型 1:存储过程 2:函数</param>
        /// <param name="pList">名称列表</param>
        /// <param name="pName">名称</param>
        /// <returns></returns>
        public DataTable GetProcedureOrFunctionList(string pConnection, int pType)
        {
            return GetProcedureOrFunctionList(pConnection, pType, null, null);
        }

        /// <summary>
        /// 获取存储过程或函数的名称和描述
        /// </summary>
        /// <param name="pConnection">数据库链接</param>
        /// <param name="pType">类型 1:存储过程 2:函数</param>
        /// <param name="pList">名称列表</param>
        /// <param name="pName">名称</param>
        /// <returns></returns>
        public DataTable GetProcedureOrFunctionList(string pConnection, int pType, string pName)
        {
            return GetProcedureOrFunctionList(pConnection, pType, null, pName);
        }

        /// <summary>
        /// 获取存储过程或函数的名称和描述
        /// </summary>
        /// <param name="pConnection">数据库链接</param>
        /// <param name="pType">类型 1:存储过程 2:函数</param>
        /// <param name="pList">名称列表</param>
        /// <param name="pName">名称</param>
        /// <returns></returns>
        public DataTable GetProcedureOrFunctionList(string pConnection, int pType, List<string> pList)
        {
            return GetProcedureOrFunctionList(pConnection, pType, pList, null);
        }

        /// <summary>
        /// 获取存储过程或函数的名称和描述
        /// </summary>
        /// <param name="pConnection">数据库链接</param>
        /// <param name="pType">类型 1:存储过程 2:函数</param>
        /// <param name="pList">名称列表</param>
        /// <param name="pName">名称</param>
        /// <returns></returns>
        private DataTable GetProcedureOrFunctionList(string pConnection, int pType, List<string> pList, string pName)
        {
            DataTable result = new DataTable();

            try
            {
                string strType = EnumerationHelper.GetEnumDes<EnumerationHelper.DBStructureType>(pType);

                string condition = "";

                if (string.IsNullOrEmpty(pName) == false)
                {
                    if (pName.Contains(",") == true)
                    {
                        condition = " and o.name in('" + pName.Replace(",", "','") + "')";
                    }
                    else
                    {
                        condition = " and o.name like '%" + pName + "%'";
                    }
                }

                if (pList != null && pList.Count > 0)
                {
                    string str = string.Join("','", pList.ToArray());
                    condition = "'" + str + "'";
                    condition = " and name in(" + condition + ")";
                }

                string strDescription = "";
                switch (pType)
                {
                    case 1:
                        strDescription = DataSynchronVO.ProcedureDescription;
                        break;
                    case 2:
                        strDescription = DataSynchronVO.FunctionDescription;
                        break;
                }

                string sql = @"
select o.object_id " + GeneralVO.id + ", o.name " + GeneralVO.name + ", ep.value " + strDescription + @"
from sys.objects o
left join sys.extended_properties ep on o.object_id = ep.major_id and ep.minor_id = 0 and ep.name = 'MS_Description'
where o.type = @ObjectType" + condition + @"
order by o.name
";

                List<SqlParameter> paras = new List<SqlParameter>();
                paras.Add(new SqlParameter("@ObjectType", strType));

                result = SqlHelper.ExecuteDataTable(pConnection, CommandType.Text, sql, paras.ToArray());
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }

            return result;
        }

        #endregion

        #region 获取排序后的表，有考虑表外键
        /// <summary>
        /// 获取排序后的表，有考虑表外键
        /// 用到的地方：1.数据完全同步 2.数据增量同步
        /// </summary>
        /// <param name="pSourceConnection">数据库链接</param>
        /// <param name="lstCalculation">列名集合</param>
        /// <returns></returns>
        public List<string> GetTableListBySort(string pSourceConnection, List<string> lstCalculation)
        {
            List<string> lst = null;

            try
            {
                #region sql

                string sql = @"
CREATE TABLE #Sort(idx int, TableName NVARCHAR(50))

--iSign 1:最初的表	2:关联后的表	3:已经插入排序表
SELECT position, RTRIM(LTRIM(value)) mainTable, CONVERT(NVARCHAR(200), '') foreignTable , 1 iSign
INTO #Tables FROM uf_split(@Tables, ',')

INSERT INTO #Tables
SELECT T.mainTable, OBJECT_NAME(fk.referenced_object_id), 2
FROM #Tables T
LEFT JOIN sys.foreign_keys fk ON fk.parent_object_id = OBJECT_ID(T.mainTable)
LEFT JOIN sys.foreign_key_columns fkc on fk.object_id = fkc.constraint_object_id

DECLARE @Position INT, @MainTable NVARCHAR(50), @ForeignTable NVARCHAR(50), @CurrentIdx INT
WHILE EXISTS(SELECT * FROM #Tables WHERE iSign = 2)
BEGIN
	SELECT TOP 1 @Position = position, @MainTable = mainTable, @ForeignTable = foreignTable FROM #Tables WHERE iSign = 2

	--如果两张表都存在，将忽略
	IF (SELECT COUNT(1) FROM #Sort WHERE TableName IN(@MainTable, @ForeignTable)) <> 2
	BEGIN
		
		IF EXISTS(SELECT * FROM #Sort WHERE TableName = @MainTable)		--主表存在，外键表不存在：外键表插上面
		BEGIN
			SELECT @CurrentIdx = idx FROM #Sort WHERE TableName = @MainTable
			UPDATE #Sort SET idx = idx + 1 WHERE idx >= @CurrentIdx

			INSERT INTO #Sort
			SELECT @CurrentIdx, @ForeignTable
		END
		ELSE IF EXISTS(SELECT * FROM #Sort WHERE TableName = @ForeignTable)	--主表不存在，外键表存在：主表插下面
		BEGIN
			SELECT @CurrentIdx = idx FROM #Sort WHERE TableName = @ForeignTable
			UPDATE #Sort SET idx = idx + 1 WHERE idx > @CurrentIdx

			INSERT INTO #Sort
			SELECT @CurrentIdx + 1, @MainTable
		END
		ELSE
		BEGIN																--主表不存在，外键表不存在：先插外键表，后插主表
			SELECT @CurrentIdx = 0
			SELECT TOP 1 @CurrentIdx = idx FROM #Sort ORDER BY idx DESC

			INSERT INTO #Sort
			SELECT @CurrentIdx + 1, @ForeignTable
			UNION
			SELECT @CurrentIdx + 2, @MainTable
		END

	END

	UPDATE #Tables SET iSign = 3 WHERE position = @Position
END

SELECT S.TableName
FROM #Tables T
JOIN #Sort S ON T.mainTable = S.TableName
WHERE T.iSign = 1
ORDER BY S.idx

DROP TABLE #Tables
DROP TABLE #Sort
            ";

                #endregion

                List<SqlParameter> paras = new List<SqlParameter>();
                paras.Add(new SqlParameter("@Tables", string.Join(",", lstCalculation.ToArray())));

                DataTable dt = SqlHelper.ExecuteDataTable(pSourceConnection, CommandType.Text, sql, paras.ToArray());

                lst = (from a in dt.AsEnumerable() select a.Field<string>("TableName")).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }
            
            return lst;
        }
        #endregion

        #region 获取存储过程或函数内容
        /// <summary>
        /// 获取存储过程或函数内容
        /// </summary>
        /// <param name="pConnString">数据库链接</param>
        /// <param name="pType">类型 1:存储过程 2:函数</param>
        /// <param name="pName">名称</param>
        /// <returns></returns>
        public string GetProcedureOrFunctionText(string pConnString, int pType, string pName)
        {
            string result = "";

            string strType = EnumerationHelper.GetEnumDes<EnumerationHelper.DBStructureType>(pType);

            string sql = @"
declare @text nvarchar(max) = ''

select @text = @text + text
from sys.syscomments s
join sys.objects o on s.id = o.object_id
where o.type = @ObjectType and o.name = @ProcedureName
order by colid

select @text";

            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("@ProcedureName", pName));
            paras.Add(new SqlParameter("@ObjectType", strType));

            DataTable dt = SqlHelper.ExecuteDataTable(pConnString, CommandType.Text, sql, paras.ToArray());
            if (dt != null && dt.Rows.Count > 0)
            {
                result = dt.Rows[0][0].ToString();
            }

            return result;
        }
        #endregion

        #region 判断存在过程或函数是否存在
        /// <summary>
        /// 判断存在过程或函数是否存在
        /// </summary>
        /// <param name="pConnString">数据库链接</param>
        /// <param name="pType">类型 1:存储过程 2:函数</param>
        /// <param name="pName">名称</param>
        /// <returns></returns>
        private bool JudgeExistProcedureOrFunction(string pConnString, int pType, string pName)
        {
            bool result = false;

            string strType = EnumerationHelper.GetEnumDes<EnumerationHelper.DBStructureType>(pType);

            string sql = @"select count(1) from sys.objects where name = @ProcedureName and type = @ObjectType";

            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("@ProcedureName", pName));
            paras.Add(new SqlParameter("@ObjectType", strType));

            DataTable dt = SqlHelper.ExecuteDataTable(pConnString, CommandType.Text, sql, paras.ToArray());
            if (dt != null && dt.Rows.Count > 0)
            {
                result = true;
            }

            return result;
        }
        #endregion

    }
}
