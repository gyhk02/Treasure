using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treasure.BLL.Frame;
using Treasure.Model.Frame;
using Treasure.Utility.Utilitys;

namespace Treasure.Main.Frame
{
    public partial class SysReportEdit : System.Web.UI.Page
    {

        #region 自定义变量

        SysReportBll bll = new SysReportBll();
        DateTime today = DateTime.Now;

        #endregion

        #region 系统事件

        protected void Page_Load(object sender, EventArgs e)
        {
            //按钮
            if (Request.HttpMethod == "POST")
            {
                if (Request["btnAdd"] == "新增")
                {
                    Add();
                }
                if (Request["btnEdit"] == "修改")
                {
                    Edit();
                }
                if (Request["btnBack"] == "返回")
                {
                    Back();
                }

            }

            if (IsPostBack == false)
            {
                //接收参数
                hdnID.Value = Request["ID"];

                //显示隐藏新增或修改按钮
                ClientScriptManager clientScript = Page.ClientScript;
                if (hdnID.Value != null && string.IsNullOrEmpty(hdnID.Value) == false)
                {
                    clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>ShowAddOrEdit('btnEdit');</script>");
                }
                else
                {
                    clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>ShowAddOrEdit('btnAdd');</script>");
                }






                InitData();
            }
        }

        #endregion

        #region 按钮



        #region 返回
        /// <summary>
        /// 返回
        /// </summary>
        private void Back()
        {
            Response.Redirect("SysReport.aspx");
        }
        #endregion

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        private void Add()
        {
            DataTable dt = bll.GetDataTableStructure(SysReportTable.tableName);
            DataRow row = dt.NewRow();

            row[SysReportTable.Fields.id] = Guid.NewGuid().ToString().Replace("-", "");
            row[SysReportTable.Fields.no] = txtNO.Text.Trim();
            row[SysReportTable.Fields.name] = txtNAME.Text.Trim();
            row[SysReportTable.Fields.sourceSql] = txtSOURCE_SQL.Text.Trim();
            row[SysReportTable.Fields.targetSql] = txtTARGET_SQL.Text.Trim();

            row[SysReportTable.Fields.createDatetime] = today;
            row[SysReportTable.Fields.modifyDatetime] = today;

            bll.AddDataRow(row);

            Response.Redirect("SysReport.aspx");
        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改
        /// </summary>
        private void Edit()
        {
            DataRow row = bll.GetDataRowById(SysReportTable.tableName, hdnID.Value);

            row[SysReportTable.Fields.no] = txtNO.Text.Trim();
            row[SysReportTable.Fields.name] = txtNAME.Text.Trim();
            row[SysReportTable.Fields.sourceSql] = txtSOURCE_SQL.Text.Trim();
            row[SysReportTable.Fields.targetSql] = txtTARGET_SQL.Text.Trim();
            row[SysReportTable.Fields.modifyDatetime] = today;

            bll.UpdateDataRow(row);

            Response.Redirect("SysReport.aspx");
        }
        #endregion

        #endregion

        #region 自定义事件

        #region 初始化数据
        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData()
        {
            string id = hdnID.Value;

            if (string.IsNullOrEmpty(id) == false)
            {
                DataRow row = bll.GetDataRowById(SysReportTable.tableName, id);
                if (row != null)
                {
                    txtNO.Text = TypeConversion.ToString(row[SysReportTable.Fields.no]);                    txtNAME.Text = TypeConversion.ToString(row[SysReportTable.Fields.name]);                    txtSOURCE_SQL.Text = TypeConversion.ToString(row[SysReportTable.Fields.sourceSql]);                    txtTARGET_SQL.Text = TypeConversion.ToString(row[SysReportTable.Fields.targetSql]);
                }
            }
        }
        #endregion

        #endregion
    }
}