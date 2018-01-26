using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Treasure.Model.General;
using Treasure.Model.SmallTool.DataSynchron;
using Treasure.Utility.Helpers;

namespace Treasure.BLL.General
{
    /// <summary>
    /// 逻辑层基类
    /// </summary>
    public class BasicBLL
    {

        #region 获取

        #region 根据SQL语句获取数据
        /// <summary>
        /// 根据SQL语句获取数据
        /// </summary>
        /// <param name="pSql"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string pSql, params SqlParameter[] pParas)
        {
            return SQLHelper.ExecuteDataTable(CommandType.Text, pSql, pParas);
        }
        #endregion

        #endregion

        #region 删除

        #region 根据表名删除全部数据
        /// <summary>
        /// 根据表名删除全部数据
        /// </summary>
        /// <param name="pConnString">数据库链接</param>
        /// <param name="pTableName">表名</param>
        /// <returns></returns>
        public bool DeleteDataTableByName(string pConnString, string pTableName)
        {
            return SQLHelper.DeleteDataTableByName(pConnString, pTableName);
        }
        #endregion

        #endregion

        #region 根据表名获取表的全部数据

        /// <summary>
        /// 根据表名获取表的全部数据
        /// </summary>
        /// <param name="pTableName">表名</param>
        /// <returns>DataTable</returns>
        public DataTable GetTableAllInfo(string pTableName)
        {
            return SQLHelper.ExecuteDataTableByName(pTableName);
        }

        /// <summary>
        /// 根据表名获取表的全部数据
        /// </summary>
        /// <param name="pConnection">数据库链接</param>
        /// <param name="pTableName">表名</param>
        /// <returns>DataTable</returns>
        public DataTable GetTableAllInfo(string pConnection, string pTableName)
        {
            return SQLHelper.ExecuteDataTableByName(pConnection, pTableName);
        }

        #endregion

    }
}
