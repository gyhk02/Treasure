using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treasure.Model.General;
using Treasure.Web.Extend;
using Treasure.Web.Helper;

namespace Treasure.Web.AboutDataBase.DataSynchron
{
    public partial class DataSynchron : System.Web.UI.Page
    {

        #region 系统事件

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //绑定数据库
                DataTable dtDatabase = new DataTable();
                dtDatabase.Columns.Add(GeneralVO.id, Type.GetType("System.Int32"));
                dtDatabase.Columns.Add(GeneralVO.no, Type.GetType("System.String"));
                dtDatabase.Columns.Add(DataSynchronInfo.Ip, Type.GetType("System.String"));
                dtDatabase.Columns.Add(DataSynchronInfo.LoginName, Type.GetType("System.String"));
                dtDatabase.Columns.Add(DataSynchronInfo.Pwd, Type.GetType("System.String"));
                dtDatabase.Columns.Add(DataSynchronInfo.DbName, Type.GetType("System.String"));

                DataRow row1 = dtDatabase.NewRow();
                row1[GeneralVO.id] = 1;
                row1[GeneralVO.no] = "NERP";
                row1[DataSynchronInfo.Ip] = "172.16.96.56";
                row1[DataSynchronInfo.LoginName] = "sa";
                row1[DataSynchronInfo.Pwd] = "sa.123";
                row1[DataSynchronInfo.DbName] = "NERP";
                dtDatabase.Rows.Add(row1);

                DataRow row2 = dtDatabase.NewRow();
                row2[GeneralVO.id] = 2;
                row2[GeneralVO.no] = "NERP_STD";
                row2[DataSynchronInfo.Ip] = "172.16.96.56";
                row2[DataSynchronInfo.LoginName] = "NERP";
                row2[DataSynchronInfo.Pwd] = "nerp@123";
                row2[DataSynchronInfo.DbName] = "NERP_STD";
                dtDatabase.Rows.Add(row2);

                DropDownListExtend.BindToShowNo(ddlSourceDb, dtDatabase, false);
                DropDownListExtend.BindToShowNo(ddlTargetDb, dtDatabase, false);
                Session["DataDb"] = dtDatabase;

                //默认同步类型
                rblSynchronType.Text = "完全同步";
            }
        }

        #endregion

        #region 按钮

        #region 选择源数据库
        /// <summary>
        /// 选择源数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSourceDb_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = TypeConversion.ToInt(ddlSourceDb.SelectedValue);

            DataTable dtDatabase = Session["DataDb"] as DataTable;
            List<DataRow> lstRow = dtDatabase.AsEnumerable().Where(p => p.Field<int>(GeneralVO.id) == id).ToList();
            if (lstRow.Count == 1)
            {
                DataRow row = lstRow[0];
                txtSourceIp.Text = row[DataSynchronInfo.Ip].ToString();
                txtSourceLoginName.Text = row[DataSynchronInfo.LoginName].ToString();
                txtSourcePwd.Text = row[DataSynchronInfo.Pwd].ToString();
                txtSourceDbName.Text = row[DataSynchronInfo.DbName].ToString();
            }
            else
            {
                txtSourceIp.Text = "";
                txtSourceLoginName.Text = "";
                txtSourcePwd.Text = "";
                txtSourceDbName.Text = "";
            }
        }
        #endregion

        #region 选择目标数据库
        /// <summary>
        /// 选择目标数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTargetDb_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = TypeConversion.ToInt(ddlTargetDb.SelectedValue);
            DataTable dtDatabase = Session["DataDb"] as DataTable;
            List<DataRow> lstRow = dtDatabase.AsEnumerable().Where(p => p.Field<int>(GeneralVO.id) == id).ToList();
            if (lstRow.Count == 1)
            {
                DataRow row = lstRow[0];
                txtTargetIp.Text = row[DataSynchronInfo.Ip].ToString();
                txtTargetLoginName.Text = row[DataSynchronInfo.LoginName].ToString();
                txtTargetPwd.Text = row[DataSynchronInfo.Pwd].ToString();
                txtTargetDbName.Text = row[DataSynchronInfo.DbName].ToString();
            }
            else
            {
                txtTargetIp.Text = "";
                txtTargetLoginName.Text = "";
                txtTargetPwd.Text = "";
                txtTargetDbName.Text = "";
            }
        }
        #endregion

        #region 连接数据库
        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConnection_Click(object sender, EventArgs e)
        {

            #region 检查源数据库连接是否正确

            string strIp = txtSourceIp.Text.Trim();
            string strLoginName = txtSourceLoginName.Text.Trim();
            string strPwd = txtSourcePwd.Text.Trim();
            string strDbName = txtSourceDbName.Text.Trim();

            SqlConnection conn = new SqlConnection();
            string strSouceConnection = "Data Source=" + strIp + ";Initial Catalog=" + strDbName + ";User ID=" + strLoginName + ";Password=" + strPwd + ";Persist Security Info=True;";
            conn.ConnectionString = strSouceConnection;
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                conn.Close();
                hdnSourceConnection.Value = strSouceConnection;
                lblConnectionError.Text = "源据库连接成功。";
            }
            catch (Exception ex)
            {
                lblConnectionError.Text = "连接源数据库异常：" + ex.Message;
                conn.Close();
                return;
            }

            #endregion

            #region 检查目标数据库连接是否正确

            strIp = txtTargetIp.Text.Trim();
            strLoginName = txtTargetLoginName.Text.Trim();
            strPwd = txtTargetPwd.Text.Trim();
            strDbName = txtTargetDbName.Text.Trim();

            conn = new SqlConnection();            
            string strTargetConnection = "Data Source=" + strIp + ";Initial Catalog=" + strDbName + ";User ID=" + strLoginName + ";Password=" + strPwd + ";Persist Security Info=True;";
            conn.ConnectionString = strTargetConnection;
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                conn.Close();
                hdnTargetConnection.Value = strTargetConnection;
                lblConnectionError.Text = lblConnectionError.Text + "\n 目标据库连接成功。";
            }
            catch (Exception ex)
            {
                lblConnectionError.Text = "连接目标据库异常：" + ex.Message;
                conn.Close();
                return;
            }

            #endregion

            //显示数据
            string sql = @"
SELECT tbs.name TableName,ds.value Description
FROM sys.extended_properties ds  
LEFT JOIN sysobjects tbs ON ds.major_id=tbs.id  
WHERE  ds.minor_id=0 
";
            DataTable dtData = SQLHelper.ExecuteDataTable(strSouceConnection, CommandType.Text, sql, null);
            grvData.DataSource = dtData;
            grvData.DataBind();


        }
        #endregion

        #endregion
    }
}