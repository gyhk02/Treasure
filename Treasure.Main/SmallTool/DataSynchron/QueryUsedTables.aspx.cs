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
        DataBaseBll bllDataBase = new DataBaseBll();

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

                ddlSourceDb.SelectedIndex = 6;
                ddlSourceDb_SelectedIndexChanged(sender, e);
            }
        }

        #endregion

        #region 按钮

        #region 清空表名
        /// <summary>
        /// 清空表名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtTables.Text = "";
        }
        #endregion

        #region 查看用到哪些表
        /// <summary>
        /// 查看用到哪些表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            string connString = GetConnString();
            if (bllDataBase.JudgeConneStr(connString) == false)
            {
                ClientScriptManager clientScript = Page.ClientScript;
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('源数据库连接异常');</script>");
                return;
            }

            //获取全部表
            List<string> lstTable = new List<string>();
            string strCode = txtCode.Text.Trim();

            if (string.IsNullOrEmpty(strCode) == false)
            {
                DataTable dtTableList = bllDataBase.GetTableNameList(connString);
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
            List<DataRow> lstRow = dtDatabase.AsEnumerable().Where(p => p.Field<int>(GeneralVO.id) == id).ToList();
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

        #region 测试数据库链接
        /// <summary>
        /// 测试数据库链接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConn_Click(object sender, EventArgs e)
        {
            string connString = GetConnString();

            if (bllDataBase.JudgeConneStr(connString) == true)
            {
                lblShowConnectionStatus.Text = "源数据库连接成功。" + ConstantVO.ENTER_BR;
            }
            else
            {
                lblShowConnectionStatus.Text = "源数据库连接异常。" + ConstantVO.ENTER_BR;
            }
        }
        #endregion

        #region 获取存储过程列表
        /// <summary>
        /// 获取存储过程列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGetProcedure_Click(object sender, EventArgs e)
        {
            string connString = GetConnString();
            if (bllDataBase.JudgeConneStr(connString) == false)
            {
                ClientScriptManager clientScript = Page.ClientScript;
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('源数据库连接异常');</script>");
                return;
            }

            DataTable dtProcedure = bll.GetProcedureOrFunctionList(connString, 1);

            DropDownListExtend.BindToShowName(ddlProcedure, dtProcedure, true);
        }
        #endregion

        #region 选择存储过程
        /// <summary>
        /// 选择存储过程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlProcedure_SelectedIndexChanged(object sender, EventArgs e)
        {
            string connString = GetConnString();
            if (bllDataBase.JudgeConneStr(connString) == false)
            {
                ClientScriptManager clientScript = Page.ClientScript;
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('源数据库连接异常');</script>");
                return;
            }

            DropDownList obj = sender as DropDownList;
            string procedureName = obj.SelectedItem.Text;

            txtCode.Text = bll.GetProcedureOrFunctionText(connString, 1, procedureName);
            txtTables.Text = "";
        }
        #endregion

        #endregion

        #region 自定义事件

        #region 获取数据库连接字符串
        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <returns></returns>
        private string GetConnString()
        {
            string result = "";

            string strIp = txtSourceIp.Text.Trim();
            string strLoginName = txtSourceLoginName.Text.Trim();
            string strPwd = txtSourcePwd.Text.Trim();
            string strDbName = txtSourceDbName.Text.Trim();
            result = "Data Source=" + strIp + ";Initial Catalog=" + strDbName + ";User ID=" + strLoginName + ";Password=" + strPwd + ";Persist Security Info=True;";

            return result;
        }
        #endregion

   

        #endregion

    }
}