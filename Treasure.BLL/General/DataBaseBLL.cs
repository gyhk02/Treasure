﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Treasure.Model.General;
using Treasure.Model.SmallTool.DataSynchron;
using Treasure.Utility.Helpers;

namespace Treasure.Bll.General
{
    public class DataBaseBll
    {
        #region 获取数据库链接
        /// <summary>
        /// 获取数据库链接
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetDatabaseLinks()
        {
            DataTable dtDatabase = new DataTable();
            dtDatabase.Columns.Add(GeneralVO.id, Type.GetType("System.Int32"));
            dtDatabase.Columns.Add(DataSynchronVO.Version, Type.GetType("System.String"));
            dtDatabase.Columns.Add(GeneralVO.no, Type.GetType("System.String"));
            dtDatabase.Columns.Add(DataSynchronVO.Ip, Type.GetType("System.String"));
            dtDatabase.Columns.Add(DataSynchronVO.LoginName, Type.GetType("System.String"));
            dtDatabase.Columns.Add(DataSynchronVO.PassWord, Type.GetType("System.String"));
            dtDatabase.Columns.Add(DataSynchronVO.DbName, Type.GetType("System.String"));

            DataRow row12 = dtDatabase.NewRow();
            row12[GeneralVO.id] = 12;
            row12[DataSynchronVO.Version] = ConstantVO.OFFICIAL_VERSION;
            row12[GeneralVO.no] = "ePDM_正式版_18";
            row12[DataSynchronVO.Ip] = "172.16.96.18";
            row12[DataSynchronVO.LoginName] = "erp";
            row12[DataSynchronVO.PassWord] = "0.123456789";
            row12[DataSynchronVO.DbName] = "EVNERP";
            dtDatabase.Rows.Add(row12);

            DataRow row11 = dtDatabase.NewRow();
            row11[GeneralVO.id] = 11;
            row11[DataSynchronVO.Version] = ConstantVO.DEVELOPMENT_VERSION;
            row11[GeneralVO.no] = "Treasure_56";
            row11[DataSynchronVO.Ip] = "172.16.96.56";
            row11[DataSynchronVO.LoginName] = "erp";
            row11[DataSynchronVO.PassWord] = "erp.123";
            row11[DataSynchronVO.DbName] = "EVNERP20180203";
            dtDatabase.Rows.Add(row11);

            DataRow row10 = dtDatabase.NewRow();
            row10[GeneralVO.id] = 10;
            row10[DataSynchronVO.Version] = ConstantVO.OFFICIAL_VERSION;
            row10[GeneralVO.no] = "小采购正式版_GSP_93";
            row10[DataSynchronVO.Ip] = "172.16.96.93";
            row10[DataSynchronVO.LoginName] = "gsp";
            row10[DataSynchronVO.PassWord] = "gsp.123456789";
            row10[DataSynchronVO.DbName] = "GSP";
            dtDatabase.Rows.Add(row10);

            DataRow row9 = dtDatabase.NewRow();
            row9[GeneralVO.id] = 9;
            row9[DataSynchronVO.Version] = ConstantVO.DEVELOPMENT_VERSION;
            row9[GeneralVO.no] = "小采购开发版_GSPDev_172.16.96.55\\sql2014";
            row9[DataSynchronVO.Ip] = "172.16.96.55\\sql2014";
            row9[DataSynchronVO.LoginName] = "csharp";
            row9[DataSynchronVO.PassWord] = "csharp.123";
            row9[DataSynchronVO.DbName] = "GSPDev";
            dtDatabase.Rows.Add(row9);

            DataRow row8 = dtDatabase.NewRow();
            row8[GeneralVO.id] = 8;
            row8[DataSynchronVO.Version] = ConstantVO.TEST_VERSION;
            row8[GeneralVO.no] = "Treasure";
            row8[DataSynchronVO.Ip] = ".";
            row8[DataSynchronVO.LoginName] = "sa";
            row8[DataSynchronVO.PassWord] = "1";
            row8[DataSynchronVO.DbName] = "Treasure";
            dtDatabase.Rows.Add(row8);

            DataRow row7 = dtDatabase.NewRow();
            row7[GeneralVO.id] = 7;
            row7[DataSynchronVO.Version] = ConstantVO.TEST_VERSION;
            row7[GeneralVO.no] = "EVN_Frame_测_55";
            row7[DataSynchronVO.Ip] = "172.16.96.55";
            row7[DataSynchronVO.LoginName] = "programmer";
            row7[DataSynchronVO.PassWord] = "123456";
            row7[DataSynchronVO.DbName] = "Frame";
            dtDatabase.Rows.Add(row7);

            DataRow row6 = dtDatabase.NewRow();
            row6[GeneralVO.id] = 6;
            row6[DataSynchronVO.Version] = ConstantVO.OFFICIAL_VERSION;
            row6[GeneralVO.no] = "EVN_Frame_正_48";
            row6[DataSynchronVO.Ip] = "172.16.96.48";
            row6[DataSynchronVO.LoginName] = "csharp";
            row6[DataSynchronVO.PassWord] = "csharp.123";
            row6[DataSynchronVO.DbName] = "Frame";
            dtDatabase.Rows.Add(row6);

            DataRow row5 = dtDatabase.NewRow();
            row5[GeneralVO.id] = 5;
            row5[DataSynchronVO.Version] = ConstantVO.TEST_VERSION;
            row5[GeneralVO.no] = "ePDM_测_56_0531";
            row5[DataSynchronVO.Ip] = "172.16.96.56";
            row5[DataSynchronVO.LoginName] = "erp";
            row5[DataSynchronVO.PassWord] = "erp.123";
            row5[DataSynchronVO.DbName] = "EVNERP20180531";
            dtDatabase.Rows.Add(row5);

            DataRow row4 = dtDatabase.NewRow();
            row4[GeneralVO.id] = 4;
            row4[DataSynchronVO.Version] = ConstantVO.TEST_VERSION;
            row4[GeneralVO.no] = "ePDM_测试库_TEST";
            row4[DataSynchronVO.Ip] = "172.16.96.56";
            row4[DataSynchronVO.LoginName] = "erp";
            row4[DataSynchronVO.PassWord] = "erp.123";
            row4[DataSynchronVO.DbName] = "NERP_TEST";
            dtDatabase.Rows.Add(row4);

            DataRow row3 = dtDatabase.NewRow();
            row3[GeneralVO.id] = 3;
            row3[DataSynchronVO.Version] = ConstantVO.TEST_VERSION;
            row3[GeneralVO.no] = "LoadGSP01";
            row3[DataSynchronVO.Ip] = ".";
            row3[DataSynchronVO.LoginName] = "sa";
            row3[DataSynchronVO.PassWord] = "1";
            row3[DataSynchronVO.DbName] = "GSP01";
            dtDatabase.Rows.Add(row3);

            DataRow row2 = dtDatabase.NewRow();
            row2[GeneralVO.id] = 2;
            row2[DataSynchronVO.Version] = ConstantVO.TEST_VERSION;
            row2[GeneralVO.no] = "LoadGSP08";
            row2[DataSynchronVO.Ip] = ".";
            row2[DataSynchronVO.LoginName] = "sa";
            row2[DataSynchronVO.PassWord] = "1";
            row2[DataSynchronVO.DbName] = "GSP08";
            dtDatabase.Rows.Add(row2);

            DataRow row1 = dtDatabase.NewRow();
            row1[GeneralVO.id] = 1;
            row1[DataSynchronVO.Version] = ConstantVO.TEST_VERSION;
            row1[GeneralVO.no] = "小采购测试版_GSP_TEST_55\\sql2014";
            row1[DataSynchronVO.Ip] = "172.16.96.55\\sql2014";
            row1[DataSynchronVO.LoginName] = "csharp";
            row1[DataSynchronVO.PassWord] = "csharp.123";
            row1[DataSynchronVO.DbName] = "GSP_TEST";
            dtDatabase.Rows.Add(row1);

            return dtDatabase;
        }
        #endregion

        #region 通过数据库链接获取全部表的名称

        /// <summary>
        /// 通过数据库链接获取全部表的名称
        /// </summary>
        /// <param name="pConnection">数据库链接</param>
        /// <returns>DataTable</returns>
        public DataTable GetTableNameList(string pConnection)
        {
            return GetTableNameList(pConnection, null, null);
        }

        /// <summary>
        /// 通过数据库链接获取全部表的名称
        /// </summary>
        /// <param name="pConnection">数据库链接</param>
        /// <param name="pTableList">表名列表</param>
        /// <param name="pTableName">
        /// 表名
        /// 逗号隔开：精确查询
        /// 没用逗号：模糊查询</param>
        /// <returns></returns>
        private DataTable GetTableNameList(string pConnection, List<string> pTableList, string pTableName)
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
select distinct o.object_id " + GeneralVO.id + ", o.name " + DataSynchronVO.TableName + @"
from sys.objects o
where o.type = 'U' " + condition + @"
order by o.name
";

            result = SqlHelper.ExecuteDataTable(pConnection, CommandType.Text, sql, null);

            return result;
        }

        #endregion

        #region 通过数据库链接获取全部表的名称及描述

        /// <summary>
        /// 通过数据库链接获取全部表的名称及描述
        /// </summary>
        /// <param name="pConnection">数据库链接</param>
        /// <returns>DataTable</returns>
        public DataTable GetTableList(string pConnection = "")
        {
            if (string.IsNullOrEmpty(pConnection) == true)
            {
                pConnection = SqlHelper.ConnString;
            }
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
select distinct o.object_id " + GeneralVO.id + ", o.name " + DataSynchronVO.TableName + @", ep.value " + DataSynchronVO.TableDescription + @"
from sys.objects o
left join sys.extended_properties ep on o.object_id = ep.major_id and ep.minor_id = 0 AND ep.name = 'MS_Description'
where o.name <> 'sysdiagrams' and o.type = 'U' " + condition;

            result = SqlHelper.ExecuteDataTable(pConnection, CommandType.Text, sql, null);

            return result;
        }

        #endregion

        #region 判断数据连接是否正常
        /// <summary>
        /// 判断数据连接是否正常
        /// </summary>
        /// <returns>true正常</returns>
        public bool JudgeConneStr(string connStr)
        {
            bool result = false;

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = connStr;

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                conn.Close();

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
        public DataTable GetTableInfoByName(int pType, List<string> pLstTableName, string pConnection)
        {
            List<DataTable> lstTable = new List<DataTable>();
            foreach (string str in pLstTableName)
            {
                DataTable dt = GetTableInfoByName(pType, str, pConnection);
                lstTable.Add(dt);
            }

            return new DataTableHelper().Merge(lstTable);
        }

        /// <summary>
        /// 根据表名获取表结构的一些相关信息
        /// </summary>
        /// <param name="pType">1.字段列表 2.约束列表 3.字段说明列表 4.表说明列表</param>
        /// <param name="pConnection">数据库链接</param>
        /// <param name="pTableName">表名</param>
        /// <returns>DataTable</returns>
        public DataTable GetTableInfoByName(int pType, string pTableName, string pConnection = "")
        {
            DataTable result = null;

            if (string.IsNullOrEmpty(pConnection) == true)
            {
                pConnection = SqlHelper.ConnString;
            }

            string strsql = "";

            switch (pType)
            {
                case 1: //字段列表
                    strsql = @"
select o.name " + DataSynchronVO.TableName + ", c.name " + DataSynchronVO.FieldName + ", t.name " + DataSynchronVO.FieldType + @"
	, IIF(t.name = 'nchar' OR t.name = 'nvarchar', c.max_length / 2, c.max_length) FiledLen
	, c.precision DecimalPrecision, c.scale DecimalDigits, ep.value " + DataSynchronVO.FieldDescription + @"
    , c.is_nullable IsNullable, c.is_identity IsIdentity, IIF(c.max_length = -1, 1, 0) IsMax, sc.text DefaultValue
	, convert(bit, iif(fk." + DataSynchronVO.ForeignTableName + @" is null, 0, 1)) isForeign
    ,  fk." + DataSynchronVO.ForeignTableName + @", fk." + DataSynchronVO.ForeignFieldName + @"
from sys.objects as o
join sys.columns as c on o.object_id = c.object_id
join sys.types as t on c.system_type_id = t.system_type_id and c.user_type_id = t.user_type_id
left join sys.extended_properties as ep on c.object_id = ep.major_id and c.column_id = ep.minor_id and ep.name = 'MS_Description'
left join syscomments sc on c.default_object_id = sc.id
left join(
	select OBJECT_NAME(fk.parent_object_id) " + DataSynchronVO.TableName + @"
        , COL_NAME(fkc.parent_object_id, fkc.parent_column_id) " + DataSynchronVO.FieldName + @"
	    , OBJECT_NAME(fk.referenced_object_id) " + DataSynchronVO.ForeignTableName + @"
        , COL_NAME(fkc.referenced_object_id, fkc.referenced_column_id) " + DataSynchronVO.ForeignFieldName + @"
	FROM sys.foreign_keys fk
	JOIN sys.foreign_key_columns fkc on fk.object_id = fkc.constraint_object_id
) fk on fk." + DataSynchronVO.TableName + @" = o.name and fk." + DataSynchronVO.FieldName + @" = c.name
where o.name = @TableName
order by c.column_id";
                    break;
                case 2: //约束列表
                    strsql = @"
select o.name " + DataSynchronVO.TableName + @", oa.name ConstraintName, oa.type ConstraintType
	, ISNULL(c.name, ISNULL(kc.COLUMN_NAME, COL_NAME(fkc.parent_object_id, fkc.parent_column_id))) " + DataSynchronVO.FieldName + @"
	, OBJECT_NAME(fk.referenced_object_id) " + DataSynchronVO.ForeignTableName + @"
	, COL_NAME(fkc.referenced_object_id, fkc.referenced_column_id) " + DataSynchronVO.ForeignFieldName + @"
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
select o.name " + DataSynchronVO.TableName + @", c.name " + DataSynchronVO.FieldName + @", ep.name " + DataSynchronVO.DescriptionName
+ ", ep.value " + DataSynchronVO.FieldDescription + @"
from sys.objects o
join sys.columns c on o.object_id = c.object_id
join sys.extended_properties ep on c.object_id = ep.major_id and c.column_id = ep.minor_id
where o.name = @TableName
order by c.column_id
";
                    break;
                case 4: //表说明列表
                    strsql = @"
select o.name " + DataSynchronVO.TableName + ", ep.name " + DataSynchronVO.DescriptionName + ", ep.value " + DataSynchronVO.TableDescription + @" 
from sys.objects o
join sys.extended_properties ep on o.object_id = ep.major_id and ep.minor_id = 0
where o.name = @TableName
";
                    break;
            }

            SqlParameter[] paras = new SqlParameter[1];
            paras[0] = new SqlParameter("@TableName", pTableName);

            result = SqlHelper.ExecuteDataTable(pConnection, CommandType.Text, strsql, paras);

            return result;
        }
        #endregion

    }
}
