using System.Data;
using System.Data.SqlClient;
using Treasure.Utility.Helpers;

namespace Treasure.BLL.General
{
    /// <summary>
    /// 逻辑层基类
    /// </summary>
    public class BasicBll
    {

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
