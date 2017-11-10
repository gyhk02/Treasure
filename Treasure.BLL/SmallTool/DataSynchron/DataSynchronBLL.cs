using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Treasure.BLL.General;
using Treasure.Model.General;
using Treasure.Model.SmallTool.DataSynchron;
using Treasure.Utility.Helpers;

namespace Treasure.BLL.SmallTool.DataSynchron
{
    public class DataSynchronBLL : BasicBLL
    {
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
            
            DataTable dt = GetTableAllInfo(pSourceConnection, pTableName);
            if (dt.Rows.Count > 0)
            {
                string strsql = "DELETE FROM [" + pTableName +"]" + ConstantVO.ENTER_STRING;
                strsql = strsql + " IF OBJECTPROPERTY(OBJECT_ID('" + pTableName + "'),'TableHasIdentity') = 1 SET IDENTITY_INSERT [" + pTableName + "] ON " + ConstantVO.ENTER_STRING;

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
                            strTmp = strTmp + ",'" + row[idy].ToString() + "'";
                        }
                    }
                    strsql = strsql + " SELECT " + strTmp.Substring(1);
                    if (idx != dt.Rows.Count - 1)
                    {
                        strsql = strsql + " UNION ALL " + ConstantVO.ENTER_STRING;
                    }
                }

                strsql = strsql + ConstantVO.ENTER_STRING + " IF OBJECTPROPERTY(OBJECT_ID('" + pTableName + "'),'TableHasIdentity') = 1 SET IDENTITY_INSERT [" + pTableName + "] OFF ";

                try
                {
                    SQLHelper.ExecuteNonQuery(pTargetConnection, CommandType.Text, strsql, null);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
                    return result;
                }
            }

            result = true;

            return result;
        }
        #endregion

        #region 创建表
        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="pConnection">数据库链接</param>
        /// <param name="pSql">sql语句</param>
        /// <returns></returns>
        public int CreateTable(string pConnection, string pSql)
        {
            int result = -1;

            try
            {
                result = SQLHelper.ExecuteNonQuery(pConnection, CommandType.Text, pSql, null);
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
select o.name TableName, c.name FiledName, t.name FiledType, c.max_length / 2 FiledLen
	, c.precision DecimalPrecision, c.scale DecimalDigits, ep.value FiledDescription
	, c.is_nullable IsNullable, c.is_identity IsIdentity, sc.text DefaultValue
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
order by c.name
";
                    break;
                case 4: //表说明列表
                    strsql = @"
select o.name TableName, ep.name DescriptionName, ep.value TableDescription
from sys.objects o
join sys.extended_properties ep on o.object_id = ep.major_id
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
select o.name " + DataSynchronVO.TableName + @", ep.name " + DataSynchronVO.DescriptionName + @", ep.value " + DataSynchronVO.TableDescription + @"
from sys.objects o
left join sys.extended_properties ep on o.object_id = ep.major_id and ep.minor_id = 0
where o.type = 'U' " + condition;

            result = SQLHelper.ExecuteDataTable(pConnection, CommandType.Text, sql, null);

            return result;
        }

        #endregion


    }
}
