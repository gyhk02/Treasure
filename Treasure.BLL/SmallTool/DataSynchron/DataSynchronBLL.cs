using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Treasure.BLL.General;
using Treasure.Model.General;
using Treasure.Model.SmallTool.DataSynchron;
using Treasure.Utility.Helpers;

namespace Treasure.BLL.SmallTool.DataSynchron
{
    public class DataSynchronBLL : BasicBLL
    {
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

            string strTmp = "";

            DataColumnCollection ColumnCollection = pLstRow[0].Table.Columns;

            if (pLstRow.Count > 0)
            {
                string strsql = " IF OBJECTPROPERTY(OBJECT_ID('" + pTableName + "'),'TableHasIdentity') = 1 " + ConstantVO.ENTER_STRING
                    + " SET IDENTITY_INSERT [" + pTableName + "] ON " + ConstantVO.ENTER_STRING;

                //拼接字段名称
                strTmp = "";
                foreach (DataColumn col in ColumnCollection)
                {
                    strTmp = strTmp + ",[" + col.ToString() + "]";
                }
                strsql = strsql + " INSERT INTO [" + pTableName + "](" + strTmp.Substring(1) + ")" + ConstantVO.ENTER_STRING;

                //拼接数据
                int cels = ColumnCollection.Count;
                for (int idx = 0; idx < pLstRow.Count; idx++)
                {
                    DataRow row = pLstRow[idx];

                    strTmp = "";
                    for (int idy = 0; idy < cels; idy++)
                    {
                        if (row[idy] == DBNull.Value)
                        {
                            strTmp = strTmp + ",NULL";
                        }
                        else
                        {
                            strTmp = strTmp + ",'" + row[idy].ToString() + "'";
                        }
                    }
                    strsql = strsql + " SELECT " + strTmp.Substring(1);
                    if (idx != pLstRow.Count - 1)
                    {
                        strsql = strsql + " UNION ALL " + ConstantVO.ENTER_STRING;
                    }
                }

                strsql = strsql + ConstantVO.ENTER_STRING + " IF OBJECTPROPERTY(OBJECT_ID('" + pTableName + "'),'TableHasIdentity') = 1 " + ConstantVO.ENTER_STRING
                    + " SET IDENTITY_INSERT [" + pTableName + "] OFF ";

                try
                {
                    SQLHelper.ExecuteNonQuery(pTargetConnection, CommandType.Text, strsql, null);
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
                new FileHelper().WriteFile(filePath, description, strsql);

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
                string strParam = "";
                string strValue = "";
                foreach (DataColumn col in ColumnCollection)
                {
                    strParam = strParam + ",[" + col.ToString() + "]";
                    strValue = strValue + ",@" + col.ToString() + "";
                }
                strParam = strParam.Substring(1);
                strValue = strValue.Substring(1);

                string sql = @"
IF OBJECTPROPERTY(OBJECT_ID('" + pTableName + @"'),'TableHasIdentity') = 1
SET IDENTITY_INSERT [" + pTableName + @"] ON

INSERT INTO " + pTableName + "(" + strParam + ") VALUES(" + strValue + @")

IF OBJECTPROPERTY(OBJECT_ID('" + pTableName + @"'),'TableHasIdentity') = 1
SET IDENTITY_INSERT [" + pTableName + @"] OFF ";

                int cels = ColumnCollection.Count;
                SqlParameter[] paras = new SqlParameter[0];
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
                        SQLHelper.ExecuteNonQuery(pTargetConnection, CommandType.Text, sql, paras);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
                        MessageBox.Show("插入数据到" + pTableName + "表异常，对应的第一个字段值为" + row[0].ToString());
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
            string strTmp = "";

            DataTable dt = base.GetTableAllInfo(pSourceConnection, pTableName);
            if (dt.Rows.Count > 0)
            {
                //string strsql = "DELETE FROM [" + pTableName + "]" + ConstantVO.ENTER_STRING;

                string strsql = " IF OBJECTPROPERTY(OBJECT_ID('" + pTableName + "'),'TableHasIdentity') = 1 " + ConstantVO.ENTER_STRING
                    + " SET IDENTITY_INSERT [" + pTableName + "] ON " + ConstantVO.ENTER_STRING;

                strTmp = "";
                foreach (DataColumn col in dt.Columns)
                {
                    strTmp = strTmp + ",[" + col.ToString() + "]";
                }
                strsql = strsql + " INSERT INTO [" + pTableName + "](" + strTmp.Substring(1) + ")" + ConstantVO.ENTER_STRING;

                int cels = dt.Columns.Count;
                for (int idx = 0; idx < dt.Rows.Count; idx++)
                {
                    DataRow row = dt.Rows[idx];

                    strTmp = "";
                    for (int idy = 0; idy < cels; idy++)
                    {
                        if (row[idy] == DBNull.Value)
                        {
                            strTmp = strTmp + ",NULL";
                        }
                        else
                        {
                            //if (dt.Columns[idy].DataType.Name == "Byte[]")
                            //{
                            //    Byte[] arrByte = (Byte[])row[idy];
                            //    string strByte = System.Text.Encoding.Default.GetString(arrByte);
                            //    strTmp = strTmp + ",'" + strByte + "'";
                            //    //string a = System.Text.Encoding.Default.GetString(b);
                            //}
                            //else
                            //{
                            strTmp = strTmp + ",'" + row[idy].ToString() + "'";
                            //}
                        }
                    }
                    strsql = strsql + " SELECT " + strTmp.Substring(1);
                    if (idx != dt.Rows.Count - 1)
                    {
                        strsql = strsql + " UNION ALL " + ConstantVO.ENTER_STRING;
                    }
                }

                strsql = strsql + ConstantVO.ENTER_STRING + " IF OBJECTPROPERTY(OBJECT_ID('" + pTableName + "'),'TableHasIdentity') = 1 " + ConstantVO.ENTER_STRING
                    + " SET IDENTITY_INSERT [" + pTableName + "] OFF ";

                try
                {
                    SQLHelper.ExecuteNonQuery(pTargetConnection, CommandType.Text, strsql, null);
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
                string filePath = "Document/DataSynchronSql/DataSynchron_" + type + "_" + DateTime.Now.ToString(ConstantVO.DATETIMEYMDHMSF) + "_" + pTableName + ".txt";
                string description = "表" + pTableName + "：从" + pSourceConnection + "到" + pTargetConnection;
                new FileHelper().WriteFile(filePath, description, strsql);

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
                //if (base.DeleteDataTableByName(pTargetConnection, pTableName) == false)
                //{
                //    MessageBox.Show("删除表" + pTableName + "数据失败");
                //    return false;
                //}

                //插入数据
                string strParam = "";
                string strValue = "";
                foreach (DataColumn col in dt.Columns)
                {
                    strParam = strParam + ",[" + col.ToString() + "]";
                    strValue = strValue + ",@" + col.ToString() + "";
                }
                strParam = strParam.Substring(1);
                strValue = strValue.Substring(1);

                string sql = @"
IF OBJECTPROPERTY(OBJECT_ID('" + pTableName + @"'),'TableHasIdentity') = 1
SET IDENTITY_INSERT [" + pTableName + @"] ON

INSERT INTO " + pTableName + "(" + strParam + ") VALUES(" + strValue + @")

IF OBJECTPROPERTY(OBJECT_ID('" + pTableName + @"'),'TableHasIdentity') = 1
SET IDENTITY_INSERT [" + pTableName + @"] OFF ";

                int cels = dt.Columns.Count;
                SqlParameter[] paras = new SqlParameter[0];
                SqlDbType type = new SqlDbType();
                for (int idx = 0; idx < dt.Rows.Count; idx++)
                {
                    paras = new SqlParameter[cels];
                    DataRow row = dt.Rows[idx];

                    for (int idy = 0; idy < cels; idy++)
                    {
                        switch (dt.Columns[idy].DataType.Name)
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

                        paras[idy] = new SqlParameter("@" + dt.Columns[idy], type) { Value = row[idy] };
                    }

                    try
                    {
                        SQLHelper.ExecuteNonQuery(pTargetConnection, CommandType.Text, sql, paras);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
                        MessageBox.Show("插入数据到" + pTableName + "表异常，对应的第一个字段值为" + row[0].ToString());
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
                SQLHelper.ExecuteNonQuery(pConnection, CommandType.Text, pSql, null);
                result = true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }
            return result;
        }
        #endregion

        #region 根据表名获取表结构的一些相关信息

        /// <summary>
        /// 根据表名获取表结构的一些相关信息
        /// </summary>
        /// <param name="pType">1.字段列表 2.约束列表 3.字段说明列表 4.表说明列表</param>
        /// <param name="pConnection">数据库链接</param>
        /// <param name="pLstTableName">表名集合</param>
        /// <returns>DataTable</returns>
        public DataTable GetTableInfoByName(int pType, string pConnection, List<string> pLstTableName)
        {
            DataTable result = new DataTable();

            List<DataTable> lstTable = new List<DataTable>();
            foreach (string str in pLstTableName)
            {
                DataTable dt = GetTableInfoByName(pType, pConnection, str);
                lstTable.Add(dt);
            }

            result = new DataTableHelper().Merge(lstTable);

            return result;
        }

        /// <summary>
        /// 根据表名获取表结构的一些相关信息
        /// </summary>
        /// <param name="pType">1.字段列表 2.约束列表 3.字段说明列表 4.表说明列表</param>
        /// <param name="pConnection">数据库链接</param>
        /// <param name="pTableName">表名</param>
        /// <returns>DataTable</returns>
        public DataTable GetTableInfoByName(int pType, string pConnection, string pTableName)
        {
            DataTable result = new DataTable();

            string strsql = "";

            switch (pType)
            {
                case 1: //字段列表
                    strsql = @"
select o.name TableName, c.name FiledName, t.name FiledType
	, IIF(t.name = 'nchar' OR t.name = 'nvarchar',  c.max_length / 2, c.max_length) FiledLen
	, c.precision DecimalPrecision, c.scale DecimalDigits, ep.value FiledDescription
	, c.is_nullable IsNullable, c.is_identity IsIdentity, IIF(c.max_length = -1, 1, 0) IsMax, sc.text DefaultValue
from sys.objects as o
join sys.columns as c on o.object_id = c.object_id
join sys.types as t on c.system_type_id = t.system_type_id and c.user_type_id = t.user_type_id
left join sys.extended_properties as ep on c.object_id = ep.major_id and c.column_id = ep.minor_id and ep.name = 'MS_Description'
left join syscomments sc on c.default_object_id = sc.id
where o.name = @TableName
order by c.column_id
";
                    break;
                case 2: //约束列表
                    strsql = @"
select o.name TableName, oa.name ConstraintName, oa.type ConstraintType
	, ISNULL(c.name, ISNULL(kc.COLUMN_NAME, COL_NAME(fkc.parent_object_id, fkc.parent_column_id))) FiledName
	, OBJECT_NAME(fk.referenced_object_id) ForeignTableName
	, COL_NAME(fkc.referenced_object_id, fkc.referenced_column_id) ForeignFiledName
	, sc.text DefaultValue, i.type_desc IndexDescripton
from sys.objects o
join sys.objects oa on o.object_id = oa.parent_object_id
left join sys.foreign_keys fk on oa.object_id = fk.object_id
left join sys.foreign_key_columns fkc on fk.object_id = fkc.constraint_object_id
left join information_schema.constraint_column_usage kc on o.name = kc.TABLE_NAME and oa.name = kc.CONSTRAINT_NAME and oa.type in('PK','UQ')
left join sys.columns c on oa.object_id = c.default_object_id
left join sys.syscomments sc on c.default_object_id = sc.id
left join sys.indexes i on o.object_id = i.object_id and oa.name = i.name
where o.name = @TableName
order by oa.type
";
                    break;
                case 3: //字段说明列表
                    strsql = @"
select o.name TableName, c.name FiledName, ep.name DescriptionName, ep.value FiledDescription
from sys.objects o
join sys.columns c on o.object_id = c.object_id
join sys.extended_properties ep on c.object_id = ep.major_id and c.column_id = ep.minor_id
where o.name = @TableName
order by c.column_id
";
                    break;
                case 4: //表说明列表
                    strsql = @"
select o.name TableName, ep.name DescriptionName, ep.value TableDescription
from sys.objects o
join sys.extended_properties ep on o.object_id = ep.major_id and ep.minor_id = 0
where o.name = @TableName
";
                    break;
            }

            SqlParameter[] paras = new SqlParameter[1];
            paras[0] = new SqlParameter("@TableName", pTableName);

            result = SQLHelper.ExecuteDataTable(pConnection, CommandType.Text, strsql, paras);

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
            DataTable result = new DataTable();

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

            if (pTableList != null)
            {
                if (pTableList.Count > 0)
                {
                    string str = string.Join("','", pTableList.ToArray());
                    condition = "'" + str + "'";
                    condition = " and o.name in(" + condition + ")";
                }
            }

            string sql = @"
select o.name " + DataSynchronVO.TableName + @", ep.name " + DataSynchronVO.DescriptionName + @", ep.value " + DataSynchronVO.TableDescription + @"
from sys.objects o
left join sys.extended_properties ep on o.object_id = ep.major_id and ep.minor_id = 0
where o.type = 'U' " + condition;

            result = SQLHelper.ExecuteDataTable(pConnection, CommandType.Text, sql, null);

            return result;
        }

        #endregion

        #region 获取表名称及表的描述

        /// <summary>
        /// 获取表名称及表的描述
        /// </summary>
        /// <param name="pConnection">数据库链接</param>
        /// <returns>DataTable</returns>
        public DataTable GetTableList(string pConnection)
        {
            return GetTableList(pConnection, null, null);
        }

        /// <summary>
        /// 获取表名称及表的描述
        /// </summary>
        /// <param name="pConnection">数据库链接</param>
        /// <param name="pTableName">
        /// 表名
        /// 逗号隔开：精确查询
        /// 没用逗号：模糊查询
        /// </param>
        /// <returns>DataTable</returns>
        public DataTable GetTableList(string pConnection, string pTableName)
        {
            return GetTableList(pConnection, null, pTableName);
        }

        /// <summary>
        /// 获取表名称及表的描述
        /// </summary>
        /// <param name="pConnection">数据库链接</param>
        /// <returns>DataTable</returns>
        public DataTable GetTableList(string pConnection, List<string> pTableList)
        {
            return GetTableList(pConnection, pTableList, null);
        }

        /// <summary>
        /// 获取表名称及表的描述
        /// </summary>
        /// <param name="pConnection">数据库链接</param>
        /// <param name="pTableList">表名列表</param>
        /// <param name="pTableName">
        /// 表名
        /// 逗号隔开：精确查询
        /// 没用逗号：模糊查询
        /// </param>
        /// <returns>DataTable</returns>
        private DataTable GetTableList(string pConnection, List<string> pTableList, string pTableName)
        {
            DataTable result = new DataTable();

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

            if (pTableList != null)
            {
                if (pTableList.Count > 0)
                {
                    string str = string.Join("','", pTableList.ToArray());
                    condition = "'" + str + "'";
                    condition = " and o.name in(" + condition + ")";
                }
            }

            string sql = @"
select distinct o.name " + DataSynchronVO.TableName + @", ep.value " + DataSynchronVO.TableDescription + @"
from sys.objects o
left join sys.extended_properties ep on o.object_id = ep.major_id and ep.minor_id = 0
where o.type = 'U' " + condition;

            result = SQLHelper.ExecuteDataTable(pConnection, CommandType.Text, sql, null);

            return result;
        }

        #endregion

        #region 获取排序后的表，有考虑表外键
        /// <summary>
        /// 获取排序后的表，有考虑表外键
        /// </summary>
        /// <param name="pSourceConnection">数据库链接</param>
        /// <param name="lstCalculation">列名集合</param>
        /// <returns></returns>
        public List<string> GetTableListBySort(string pSourceConnection, List<string> lstCalculation)
        {
            List<string> lst = new List<string>();

            string sql = @"
CREATE TABLE #Sort(idx int, TableName NVARCHAR(50))

--DECLARE @Tables NVARCHAR(1000) = 'ReturnNoteTable, ReturnNoteLine, ReturnNoteDetail'

--iSign 1:最初的表	2:关联后的表	3:已经插入排序表
SELECT position, RTRIM(LTRIM(value)) mainTable, CONVERT(NVARCHAR(200), '') foreignTable , 1 iSign
INTO #Tables FROM fn_Split(@Tables, ',')

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

            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("@Tables", string.Join(",", lstCalculation.ToArray())));

            DataTable dt = SQLHelper.ExecuteDataTable(pSourceConnection, CommandType.Text, sql, paras.ToArray());

            lst = (from a in dt.AsEnumerable() select a.Field<string>("TableName")).ToList();

            return lst;
        }
        #endregion

    }
}
