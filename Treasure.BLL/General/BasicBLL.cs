using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Treasure.Utility.Helpers;
using Treasure.Utility.Utilitys.Lambda;

namespace Treasure.Bll.General
{
    /// <summary>
    /// 逻辑层基类
    /// </summary>
    public class BasicBll
    {

        #region 执行SQL

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="pCommandType"></param>
        /// <param name="pSQL"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(CommandType pCommandType, string pSQL, SqlParameter[] paras)
        {
            return ExecuteNonQuery(null, pCommandType, pSQL, paras);
        }

        public int ExecuteNonQuery(string pConnection, CommandType pCommandType, string pSQL, SqlParameter[] paras)
        {
            if (string.IsNullOrEmpty(pConnection) == true)
            {
                pConnection = SqlHelper.ConnString;
            }
            return SqlHelper.ExecuteNonQuery(pConnection, pCommandType, pSQL, paras);
        }

        #endregion

        #region 获取

        #region 根据表名获取表的全部数据

        /// <summary>
        /// 根据表名获取表的全部数据
        /// </summary>
        /// <param name="pTableName">表名</param>
        /// <returns>DataTable</returns>
        public DataTable GetTableAllInfo(string pTableName)
        {
            return SqlHelper.ExecuteDataTableByName(pTableName);
        }

        /// <summary>
        /// 根据表名获取表的全部数据
        /// </summary>
        /// <param name="pConnection">数据库链接</param>
        /// <param name="pTableName">表名</param>
        /// <returns>DataTable</returns>
        public DataTable GetTableAllInfo(string pConnection, string pTableName)
        {
            return SqlHelper.ExecuteDataTableByName(pConnection, pTableName);
        }

        #endregion

        #region 根据表名获取表结构
        /// <summary>
        /// 根据表名获取表结构
        /// </summary>
        /// <param name="pTableName"></param>
        public DataTable GetDataTableStructure(string pTableName)
        {
            return SqlHelper.ExecuteDataTable(pTableName);
        }
        #endregion

        #region 根据SQL语句获取数据
        /// <summary>
        /// 根据SQL语句获取数据
        /// </summary>
        /// <param name="pSql"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string pSql, params SqlParameter[] pParas)
        {
            return SqlHelper.ExecuteDataTable(CommandType.Text, pSql, pParas);
        }
        #endregion


        public DataSet GetDataSet(string pSql, params SqlParameter[] pParas)
        {
            return GetDataSet(pSql, CommandType.Text, pParas);
        }

        public DataSet GetDataSet(string pSql, CommandType pType, params SqlParameter[] pParas)
        {
            return GetDataSet("", pSql, pType, pParas);
        }

        public DataSet GetDataSet(string pConnstring, string pSql, CommandType pType = CommandType.Text, params SqlParameter[] pParas)
        {
            if (string.IsNullOrEmpty(pConnstring) == true)
            {
                pConnstring = SqlHelper.ConnString;
            }

            return SqlHelper.ExecuteDataSet(pConnstring, pType, pSql, pParas);
        }

        #region 根据表的id获取一行记录
        /// <summary>
        /// 根据表的id获取一行记录
        /// </summary>
        /// <param name="pTableName"></param>
        /// <param name="pId"></param>
        /// <returns></returns>
        public DataRow GetDataRowById(string pTableName, string pId)
        {
            return SqlHelper.ExecuteDataTable(pTableName, pId);
        }
        #endregion

        #endregion

        #region 删除

        #region 根据ID删除记录
        /// <summary>
        /// 根据ID删除记录
        /// </summary>
        /// <param name="pTableName">表名</param>
        /// <param name="pId">id</param>
        /// <returns></returns>
        public bool DeleteById(string pTableName, string pId)
        {
            return SqlHelper.DeleteById(pTableName, pId);
        }
        #endregion

        #region 根据条件删除记录
        /// <summary>
        /// 根据条件删除记录
        /// </summary>
        /// <param name="pTableName"></param>
        /// <param name="pWhere"></param>
        /// <returns></returns>
        public bool DeleteByWhere(string pTableName, WhereCondition pWhere)
        {
            return SqlHelper.DeleteByWhere(pTableName, pWhere);
        }
        #endregion

        #region 根据表名删除全部数据
        /// <summary>
        /// 根据表名删除全部数据
        /// </summary>
        /// <param name="pConnString">数据库链接</param>
        /// <param name="pTableName">表名</param>
        /// <returns></returns>
        public bool DeleteDataTableByName(string pConnString, string pTableName)
        {
            return SqlHelper.DeleteDataTableByName(pConnString, pTableName);
        }
        #endregion

        #endregion

        #region 插入

        #region 新增一行数据
        /// <summary>
        /// 新增一行数据
        /// </summary>
        /// <param name="row">要插入的数据行</param>
        /// <returns></returns>
        public string AddDataRow(DataRow row)
        {
            return SqlHelper.AddDataRow(row);
        }
        #endregion

        #region 插入DataTable
        /// <summary>
        /// 插入DataTable
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<string> AddDataTable(DataTable dt)
        {
            return SqlHelper.AddDataTable(dt);
        }
        #endregion

        #endregion

        #region 修改

        /// <summary>
        /// 修改一行数据
        /// </summary>
        /// <param name="row">要修改的数据行</param>
        /// <returns></returns>
        public string UpdateDataRow(DataRow row)
        {
            return SqlHelper.UpdateDataRow(row);
        }

        #endregion

    }
}
