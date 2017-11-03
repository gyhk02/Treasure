using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Treasure.Model.General;
using Treasure.Model.SmallTool.DataSynchron;
using Treasure.Utility.Helpers;

namespace Treasure.BLL.General
{
    public class BasicBLL
    {
        #region 根据表名获取表的全部数据

        /// <summary>
        /// 根据表名获取表的全部数据
        /// </summary>
        /// <param name="pTableName"></param>
        /// <returns></returns>
        public DataTable GetTableAllInfo(string pTableName)
        {
            return SQLHelper.ExecuteDataTableByName(pTableName);
        }

        public DataTable GetTableAllInfo(string pConnection, string pTableName)
        {
            return SQLHelper.ExecuteDataTableByName(pConnection, pTableName);
        }

        #endregion

        #region 获取数据库链接
        /// <summary>
        /// 获取数据库链接
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetDatabaseLinks()
        {
            DataTable dtDatabase = new DataTable();
            dtDatabase.Columns.Add(GeneralVO.id, Type.GetType("System.Int32"));
            dtDatabase.Columns.Add(GeneralVO.no, Type.GetType("System.String"));
            dtDatabase.Columns.Add(DataSynchronVO.Ip, Type.GetType("System.String"));
            dtDatabase.Columns.Add(DataSynchronVO.LoginName, Type.GetType("System.String"));
            dtDatabase.Columns.Add(DataSynchronVO.Pwd, Type.GetType("System.String"));
            dtDatabase.Columns.Add(DataSynchronVO.DbName, Type.GetType("System.String"));

            DataRow row3 = dtDatabase.NewRow();
            row3[GeneralVO.id] = 3;
            row3[GeneralVO.no] = "LoadGSP02";
            row3[DataSynchronVO.Ip] = ".";
            row3[DataSynchronVO.LoginName] = "sa";
            row3[DataSynchronVO.Pwd] = "1";
            row3[DataSynchronVO.DbName] = "GSP02";
            dtDatabase.Rows.Add(row3);

            DataRow row2 = dtDatabase.NewRow();
            row2[GeneralVO.id] = 2;
            row2[GeneralVO.no] = "LoadGSP01";
            row2[DataSynchronVO.Ip] = ".";
            row2[DataSynchronVO.LoginName] = "sa";
            row2[DataSynchronVO.Pwd] = "1";
            row2[DataSynchronVO.DbName] = "GSP01";
            dtDatabase.Rows.Add(row2);

            DataRow row1 = dtDatabase.NewRow();
            row1[GeneralVO.id] = 1;
            row1[GeneralVO.no] = "GSP_Test_95";
            row1[DataSynchronVO.Ip] = "172.16.96.95";
            row1[DataSynchronVO.LoginName] = "csharp";
            row1[DataSynchronVO.Pwd] = "csharp.123";
            row1[DataSynchronVO.DbName] = "GSP_Test";
            dtDatabase.Rows.Add(row1);

            return dtDatabase;
        }
        #endregion

    }
}
