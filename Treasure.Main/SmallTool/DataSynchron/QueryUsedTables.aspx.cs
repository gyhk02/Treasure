using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treasure.BLL.General;
using Treasure.BLL.SmallTool.DataSynchron;
using Treasure.Model.General;
using Treasure.Model.SmallTool.DataSynchron;
using Treasure.Utility.Extend;
using Treasure.Utility.Utilitys;

namespace Treasure.Main.SmallTool.DataSynchron
{
    public partial class QueryUsedTables : System.Web.UI.Page
    {

        #region 自定义变量

        DataSynchronBLL bll = new DataSynchronBLL();
        DataBaseBLL bllDataBase = new DataBaseBLL();

        #endregion

        #region 系统事件

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //绑定数据库
                DataTable dtDatabase = bllDataBase.GetDatabaseLinks();
                DropDownListExtend.BindToShowNo(ddlSourceDb, dtDatabase, false);

                Session["dtDataDb"] = dtDatabase;

                ddlSourceDb.SelectedIndex = 0;
                ddlSourceDb_SelectedIndexChanged(sender, e);
            }
        }

        #endregion

        #region 按钮

        #region 查看用到哪些表
        /// <summary>
        /// 查看用到哪些表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            //获取数据库链接字符串
            string strIp = txtSourceIp.Text.Trim();
            string strLoginName = txtSourceLoginName.Text.Trim();
            string strPwd = txtSourcePwd.Text.Trim();
            string strDbName = txtSourceDbName.Text.Trim();
            string strSouceConnection = "Data Source=" + strIp + ";Initial Catalog=" + strDbName + ";User ID=" + strLoginName + ";Password=" + strPwd + ";Persist Security Info=True;";

            //获取全部表
            List<string> lstTable = new List<string>();
            string strCode = txtCode.Text.Trim();

            if (string.IsNullOrEmpty(strCode) == false)
            {
                DataTable dtTableList = bllDataBase.GetTableList(strSouceConnection);
                foreach (DataRow row in dtTableList.Rows)
                {
                    string tableName = row[DataSynchronVO.TableName].ToString();
                    if (strCode.Contains(tableName) == true)
                    {
                        lstTable.Add(tableName);
                    }
                }
            }

            txtTables.Text = string.Join(",", lstTable.ToArray());
        }
        #endregion

        #region 选择源数据库
        /// <summary>
        /// 选择源数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSourceDb_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = TypeConversion.ToInt(ddlSourceDb.SelectedValue);

            DataTable dtDatabase = Session["dtDataDb"] as DataTable;
            List<DataRow> lstRow = dtDatabase.AsEnumerable().Where(p => p.Field<int>(GeneralVO.Id) == id).ToList();
            if (lstRow.Count == 1)
            {
                DataRow row = lstRow[0];
                txtSourceIp.Text = row[DataSynchronVO.Ip].ToString();
                txtSourceLoginName.Text = row[DataSynchronVO.LoginName].ToString();
                txtSourcePwd.Text = row[DataSynchronVO.Pwd].ToString();
                txtSourceDbName.Text = row[DataSynchronVO.DbName].ToString();
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

        #endregion




    }
}