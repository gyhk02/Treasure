using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treasure.BLL.SmallTool.DataSynchron;
using Treasure.Model.General;
using Treasure.Model.SmallTool.DataSynchron;
using Treasure.Utility.Extend;
using Treasure.Utility.Helpers;
using Treasure.Utility.Utilitys;

namespace Treasure.Main.SmallTool.DataSynchron
{
    public partial class DataSynchron : System.Web.UI.Page
    {

        #region 自定义变量

        DataSynchronBLL bll = new DataSynchronBLL();

        #endregion

        #region 系统事件

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //绑定数据库
                DataTable dtDatabase = bll.GetDatabaseLinks();

                DropDownListExtend.BindToShowNo(ddlSourceDb, dtDatabase, false);
                DropDownListExtend.BindToShowNo(ddlTargetDb, dtDatabase, false);

                Session["dtDataDb"] = dtDatabase;

                ddlSourceDb.SelectedIndex = 1;
                ddlTargetDb.SelectedIndex = 0;
                ddlSourceDb_SelectedIndexChanged(sender, e);
                ddlTargetDb_SelectedIndexChanged(sender, e);

                //默认同步类型
                rblSynchronType.Text = "完全同步";

                grvTableList.KeyFieldName = DataSynchronVO.TableName;
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

        #region 选择目标数据库
        /// <summary>
        /// 选择目标数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTargetDb_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = TypeConversion.ToInt(ddlTargetDb.SelectedValue);
            DataTable dtDatabase = Session["dtDataDb"] as DataTable;
            List<DataRow> lstRow = dtDatabase.AsEnumerable().Where(p => p.Field<int>(GeneralVO.id) == id).ToList();
            if (lstRow.Count == 1)
            {
                DataRow row = lstRow[0];
                txtTargetIp.Text = row[DataSynchronVO.Ip].ToString();
                txtTargetLoginName.Text = row[DataSynchronVO.LoginName].ToString();
                txtTargetPwd.Text = row[DataSynchronVO.Pwd].ToString();
                txtTargetDbName.Text = row[DataSynchronVO.DbName].ToString();
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

            DataTable dt = bll.GetTableList(strSouceConnection);
            grvTableList.DataSource = dt;
            grvTableList.DataBind();
        }
        #endregion

        #region 同步
        /// <summary>
        /// 同步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSynchron_Click(object sender, EventArgs e)
        {
            ClientScriptManager clientScript = Page.ClientScript;

            //获取同步类型
            string synchronType = rblSynchronType.SelectedValue;
            if (string.IsNullOrEmpty(synchronType) == true)
            {
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('请先选择同步类型');</script>");
                return;
            }

            //获取要同步的表
            List<string> lstTableList = grvTableList.GetSelectedFieldValues(new string[] { DataSynchronVO.TableName }).ConvertAll<string>(c => string.Format("{0}", c));
            if (lstTableList.Count == 0)
            {
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('请选择要同步的表');</script>");
                return;
            }

            try
            {
                switch (synchronType)
                {
                    case "表结构同步":
                        SynchronTableStructure(lstTableList);
                        break;
                    case "数据完全同步":
                        SynchronCompleteData(lstTableList);
                        break;
                    case "数据增量同步":
                        break;
                }

                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('同步成功');</script>");
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }
        }
        #endregion

        #endregion

        #region 自定义事件

        #region 数据完全同步
        /// <summary>
        /// 数据完全同步
        /// </summary>
        private void SynchronCompleteData(List<string> lstSourceTable)
        {
            ClientScriptManager clientScript = Page.ClientScript;

            string pSourceConnection = hdnSourceConnection.Value;
            string pTargetConnection = hdnTargetConnection.Value;

            foreach (string str in lstSourceTable)
            {
                if (bll.InsertData(pSourceConnection, pTargetConnection, str) == false)
                {
                    clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('插入表" + str + "数据出现异常');</script>");
                }
            }
        }
        #endregion

        #region 表结构同步
        /// <summary>
        /// 表结构同步，方法没有写完
        /// </summary>
        /// <param name="lstSourceTable"></param>
        private void SynchronTableStructure(List<string> lstSourceTable)
        {
            ClientScriptManager clientScript = Page.ClientScript;

            //获取目标表集合
            string targetConnection = hdnTargetConnection.Value;
            DataTable dtTargetTableList = bll.GetTableList(targetConnection, lstSourceTable);
            List<string> lstTargetTable = (from d in dtTargetTableList.AsEnumerable() select d.Field<string>(DataSynchronVO.TableName)).ToList();

            string pSourceConnection = hdnSourceConnection.Value;
            string pTargetConnection = hdnTargetConnection.Value;

            //目标没有的表，创建新表
            foreach (string str in lstSourceTable)
            {
                if (lstTargetTable.Contains(str) == false)
                {
                    if (new CreateTableSub().CreateTable(pSourceConnection, str, pTargetConnection) == true)
                    {
                        if (bll.InsertData(pSourceConnection, pTargetConnection, str) == false)
                        {
                            clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('插入表" + str + "数据出现异常');</script>");
                        }
                    }
                }
            }

            //获取源结构
            //获取目标结构

            //目标不存在表
            //目标存在表：1.多字段；2.少字段；3.主键是否一至
        }
        #endregion

        #region 表结构比较
        /// <summary>
        /// 表结构比较
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCompare_Click(object sender, EventArgs e)
        {
            string pSourceConnection = hdnSourceConnection.Value;
            string pTargetConnection = hdnTargetConnection.Value;
            List<string> lstTableList = grvTableList.GetSelectedFieldValues(new string[] { DataSynchronVO.TableName }).ConvertAll<string>(c => string.Format("{0}", c));

            DataTable dtSourceTableStructure = bll.GetTableInfoByName(1, pSourceConnection, lstTableList);
            DataTable dtTargetTableStructure = bll.GetTableInfoByName(1, pTargetConnection, lstTableList);

            #region DataTable初始化

            DataTable dtResult = new DataTable();

            dtResult.Columns.Add(DataSynchronVO.TableName);
            dtResult.Columns.Add(DataSynchronVO.FiledName);

            dtResult.Columns.Add(DataSynchronVO.ISIGN, typeof(string));

            dtResult.Columns.Add(DataSynchronVO.FiledType);
            dtResult.Columns.Add(DataSynchronVO.FiledLen);
            dtResult.Columns.Add(DataSynchronVO.FiledDescription);
            dtResult.Columns.Add(DataSynchronVO.DecimalPrecision);
            dtResult.Columns.Add(DataSynchronVO.DecimalDigits);
            dtResult.Columns.Add(DataSynchronVO.IsNullable);
            dtResult.Columns.Add(DataSynchronVO.IsIdentity);
            dtResult.Columns.Add(DataSynchronVO.DefaultValue, typeof(string));

            dtResult.Columns.Add(DataSynchronVO.TargetFiledType);
            dtResult.Columns.Add(DataSynchronVO.TargetFiledLen);
            dtResult.Columns.Add(DataSynchronVO.TargetFiledDescription);
            dtResult.Columns.Add(DataSynchronVO.TargetDecimalPrecision);
            dtResult.Columns.Add(DataSynchronVO.TargetDecimalDigits);
            dtResult.Columns.Add(DataSynchronVO.TargetIsNullable);
            dtResult.Columns.Add(DataSynchronVO.TargetIsIdentity);
            dtResult.Columns.Add(DataSynchronVO.TargetDefaultValue, typeof(string));

            #endregion

            #region 数据处理

            var query =
               (from s in dtSourceTableStructure.AsEnumerable()
                from t in dtTargetTableStructure.AsEnumerable()
                where s.Field<string>(DataSynchronVO.TableName) == t.Field<string>(DataSynchronVO.TableName) && s.Field<string>(DataSynchronVO.FiledName) == t.Field<string>(DataSynchronVO.FiledName)
                select new { s, t });

            var lstTmp = query.ToList();

            for (int idx = 0; idx < lstTmp.Count; idx++)
            {
                DataRow rowSource = lstTmp[idx].s;
                DataRow rowTarget = lstTmp[idx].t;

                DataRow row = dtResult.NewRow();

                row[DataSynchronVO.TableName] = rowSource[DataSynchronVO.TableName];
                row[DataSynchronVO.FiledName] = rowSource[DataSynchronVO.FiledName];
                row[DataSynchronVO.FiledType] = rowSource[DataSynchronVO.FiledType];
                row[DataSynchronVO.FiledLen] = rowSource[DataSynchronVO.FiledLen];
                row[DataSynchronVO.FiledDescription] = rowSource[DataSynchronVO.FiledDescription];
                row[DataSynchronVO.DecimalPrecision] = rowSource[DataSynchronVO.DecimalPrecision];
                row[DataSynchronVO.DecimalDigits] = rowSource[DataSynchronVO.DecimalDigits];
                row[DataSynchronVO.IsNullable] = rowSource[DataSynchronVO.IsNullable];
                row[DataSynchronVO.IsIdentity] = rowSource[DataSynchronVO.IsIdentity];
                row[DataSynchronVO.DefaultValue] = rowSource[DataSynchronVO.DefaultValue];

                row[DataSynchronVO.TargetFiledType] = rowTarget[DataSynchronVO.FiledType];
                row[DataSynchronVO.TargetFiledLen] = rowTarget[DataSynchronVO.FiledLen];
                row[DataSynchronVO.TargetFiledDescription] = rowTarget[DataSynchronVO.FiledDescription];
                row[DataSynchronVO.TargetDecimalPrecision] = rowTarget[DataSynchronVO.DecimalPrecision];
                row[DataSynchronVO.TargetDecimalDigits] = rowTarget[DataSynchronVO.DecimalDigits];
                row[DataSynchronVO.TargetIsNullable] = rowTarget[DataSynchronVO.IsNullable];
                row[DataSynchronVO.TargetIsIdentity] = rowTarget[DataSynchronVO.IsIdentity];
                row[DataSynchronVO.TargetDefaultValue] = rowTarget[DataSynchronVO.DefaultValue];

                if (
                    (
                    row[DataSynchronVO.FiledType].ToString() == row[DataSynchronVO.TargetFiledType].ToString()
                    && row[DataSynchronVO.FiledLen].ToString() == row[DataSynchronVO.TargetFiledLen].ToString()
                    && row[DataSynchronVO.FiledDescription].ToString() == row[DataSynchronVO.TargetFiledDescription].ToString()
                    && row[DataSynchronVO.DecimalPrecision].ToString() == row[DataSynchronVO.TargetDecimalPrecision].ToString()
                    && row[DataSynchronVO.DecimalDigits].ToString() == row[DataSynchronVO.TargetDecimalDigits].ToString()
                    && row[DataSynchronVO.IsNullable].ToString() == row[DataSynchronVO.TargetIsNullable].ToString()
                    && row[DataSynchronVO.IsIdentity].ToString() == row[DataSynchronVO.TargetIsIdentity].ToString()
                    && row[DataSynchronVO.DefaultValue].ToString() == row[DataSynchronVO.TargetDefaultValue].ToString()
                    ) == false
                    )
                {
                    row[DataSynchronVO.ISIGN] = "x";
                }

                dtResult.Rows.Add(row);
            }

            #endregion

            grvTableStructure.DataSource = dtResult;
            grvTableStructure.DataBind();
        }
        #endregion

        #region 查询
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string pSourceConnection = hdnSourceConnection.Value;
            string pTableName = txtTableName.Text.Trim();

            if (string.IsNullOrEmpty(pSourceConnection) == true)
            {
                ClientScriptManager clientScript = Page.ClientScript;
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('源数据库链接为空，请先点击【链接数据库】');</script>");
                return;
            }

            DataTable dt = bll.GetTableList(pSourceConnection, pTableName);
            grvTableList.DataSource = dt;
            grvTableList.DataBind();
        }
        #endregion

        #endregion

    }
}