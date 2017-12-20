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

            DataRow row10 = dtDatabase.NewRow();
            row10[GeneralVO.Id] = 10;
            row10[DataSynchronVO.Version] = ConstantVO.OFFICIAL_VERSION;
            row10[GeneralVO.No] = "小采购正式版_GSP_93";
            row10[DataSynchronVO.Ip] = "172.16.96.93";
            row10[DataSynchronVO.LoginName] = "gsp";
            row10[DataSynchronVO.Pwd] = "gsp.123456789";
            row10[DataSynchronVO.DbName] = "GSP";
            dtDatabase.Rows.Add(row10);

            DataRow row9 = dtDatabase.NewRow();
            row9[GeneralVO.Id] = 9;
            row9[DataSynchronVO.Version] = ConstantVO.DEVELOPMENT_VERSION;
            row9[GeneralVO.No] = "小采购开发版_GSPDev_95";
            row9[DataSynchronVO.Ip] = "172.16.96.95";
            row9[DataSynchronVO.LoginName] = "csharp";
            row9[DataSynchronVO.Pwd] = "csharp.123";
            row9[DataSynchronVO.DbName] = "GSPDev";
            dtDatabase.Rows.Add(row9);

            DataRow row8 = dtDatabase.NewRow();
            row8[GeneralVO.Id] = 8;
            row8[DataSynchronVO.Version] = ConstantVO.TEST_VERSION;
            row8[GeneralVO.No] = "Treasure";
            row8[DataSynchronVO.Ip] = ".";
            row8[DataSynchronVO.LoginName] = "sa";
            row8[DataSynchronVO.Pwd] = "1";
            row8[DataSynchronVO.DbName] = "Treasure";
            dtDatabase.Rows.Add(row8);

            DataRow row7 = dtDatabase.NewRow();
            row7[GeneralVO.Id] = 7;
            row7[DataSynchronVO.Version] = ConstantVO.TEST_VERSION;
            row7[GeneralVO.No] = "EVN_Frame_测_55";
            row7[DataSynchronVO.Ip] = "172.16.96.55";
            row7[DataSynchronVO.LoginName] = "programmer";
            row7[DataSynchronVO.Pwd] = "123456";
            row7[DataSynchronVO.DbName] = "Frame";
            dtDatabase.Rows.Add(row7);

            DataRow row6 = dtDatabase.NewRow();
            row6[GeneralVO.Id] = 6;
            row6[DataSynchronVO.Version] = ConstantVO.OFFICIAL_VERSION;
            row6[GeneralVO.No] = "EVN_Frame_正_48";
            row6[DataSynchronVO.Ip] = "172.16.96.48";
            row6[DataSynchronVO.LoginName] = "csharp";
            row6[DataSynchronVO.Pwd] = "csharp.123";
            row6[DataSynchronVO.DbName] = "Frame";
            dtDatabase.Rows.Add(row6);

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
            row1[GeneralVO.No] = "小采购测试版_GSP_Test_95";
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
