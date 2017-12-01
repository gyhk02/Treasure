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

        #region 获取数据库链接
        /// <summary>
        /// 获取数据库链接
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetDatabaseLinks()
        {
            DataTable dtDatabase = new DataTable();
            dtDatabase.Columns.Add(GeneralVO.Id, Type.GetType("System.Int32"));
            dtDatabase.Columns.Add(DataSynchronVO.Version, Type.GetType("System.String"));
            dtDatabase.Columns.Add(GeneralVO.No, Type.GetType("System.String"));
            dtDatabase.Columns.Add(DataSynchronVO.Ip, Type.GetType("System.String"));
            dtDatabase.Columns.Add(DataSynchronVO.LoginName, Type.GetType("System.String"));
            dtDatabase.Columns.Add(DataSynchronVO.Pwd, Type.GetType("System.String"));
            dtDatabase.Columns.Add(DataSynchronVO.DbName, Type.GetType("System.String"));

            DataRow row5 = dtDatabase.NewRow();
            row5[GeneralVO.Id] = 5;
            row5[DataSynchronVO.Version] = ConstantVO.OFFICIAL_VERSION;
            row5[GeneralVO.No] = "ePDM_正_56";
            row5[DataSynchronVO.Ip] = "172.16.96.56";
            row5[DataSynchronVO.LoginName] = "sa";
            row5[DataSynchronVO.Pwd] = "sa.123";
            row5[DataSynchronVO.DbName] = "EVNERP";
            dtDatabase.Rows.Add(row5);

            DataRow row4 = dtDatabase.NewRow();
            row4[GeneralVO.Id] = 4;
            row4[DataSynchronVO.Version] = ConstantVO.OFFICIAL_VERSION;
            row4[GeneralVO.No] = "ePDM_临时正式_56";
            row4[DataSynchronVO.Ip] = "172.16.96.56";
            row4[DataSynchronVO.LoginName] = "sa";
            row4[DataSynchronVO.Pwd] = "sa.123";
            row4[DataSynchronVO.DbName] = "NERP_STD";
            dtDatabase.Rows.Add(row4);

            DataRow row3 = dtDatabase.NewRow();
            row3[GeneralVO.Id] = 3;
            row3[DataSynchronVO.Version] = ConstantVO.TEST_VERSION;
            row3[GeneralVO.No] = "LoadGSP01";
            row3[DataSynchronVO.Ip] = ".";
            row3[DataSynchronVO.LoginName] = "sa";
            row3[DataSynchronVO.Pwd] = "1";
            row3[DataSynchronVO.DbName] = "GSP01";
            dtDatabase.Rows.Add(row3);

            DataRow row2 = dtDatabase.NewRow();
            row2[GeneralVO.Id] = 2;
            row2[DataSynchronVO.Version] = ConstantVO.TEST_VERSION;
            row2[GeneralVO.No] = "LoadGSP08";
            row2[DataSynchronVO.Ip] = ".";
            row2[DataSynchronVO.LoginName] = "sa";
            row2[DataSynchronVO.Pwd] = "1";
            row2[DataSynchronVO.DbName] = "GSP08";
            dtDatabase.Rows.Add(row2);

            DataRow row1 = dtDatabase.NewRow();
            row1[GeneralVO.Id] = 1;
            row1[DataSynchronVO.Version] = ConstantVO.TEST_VERSION;
            row1[GeneralVO.No] = "GSP_Test_95";
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
