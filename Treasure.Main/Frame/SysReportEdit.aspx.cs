using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treasure.Bll.General;
using Treasure.BLL.Frame;
using Treasure.Model.Frame;
using Treasure.Utility.Helpers;
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
                if (Request["btnNext"] == "转下一步")
                {
                    Next();
                    return;
                }
                if (Request["btnComplete"] == "完成")
                {
                    Complete();
                    return;
                }
                if (Request["btnBack"] == "返回")
                {
                    Back();
                    return;
                }
                if (Request["__CALLBACKID"] == "grdData")
                {
                    InitData();
                    return;
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

        #region 转下一步
        /// <summary>
        /// 转下一步
        /// </summary>
        private void Next()
        {
            string sourceSql = txtSourceSQL.Text.Trim();

            //写主表
            DataTable dtReport = bll.GetDataTableStructure(SysReportTable.tableName);
            DataRow rowReport = dtReport.NewRow();

            string id = Guid.NewGuid().ToString().Replace("-", "");
            rowReport[SysReportTable.Fields.id] = id;
            rowReport[SysReportTable.Fields.name] = txtCnTitle.Text.Trim();
            rowReport[SysReportTable.Fields.enName] = txtEnTitle.Text.Trim();
            rowReport[SysReportTable.Fields.sourceSql] = txtSourceSQL.Text.Trim();
            rowReport[SysReportTable.Fields.targetSql] = "";
            rowReport[SysReportTable.Fields.hasExportExcel] = false;
            rowReport[SysReportTable.Fields.hasPage] = false;
            rowReport[SysReportTable.Fields.createUserId] = BasicWebBll.SeUserID;
            rowReport[SysReportTable.Fields.createDatetime] = today;
            rowReport[SysReportTable.Fields.modifyUserId] = BasicWebBll.SeUserID;
            rowReport[SysReportTable.Fields.modifyDatetime] = today;

            bll.AddDataRow(rowReport);

            hdnReportId.Value = id;

            //根据SQL获取全部列
            DataTable dtReportCol = bll.GetDataTableStructure(SysReportColTable.tableName);
            DataTable dtSql = bll.GetDataTable(sourceSql, null);
            foreach (DataColumn col in dtSql.Columns)
            {
                DataRow rowReportCol = dtReportCol.NewRow();
                rowReportCol[SysReportColTable.Fields.name] = col.ColumnName;
                dtReportCol.Rows.Add(rowReportCol);
            }

            grdData.DataSource = dtReportCol;
            grdData.DataBind();

            //将值赋给文本框
            for (int idx = 0; idx < dtReportCol.Rows.Count; idx++)
            {
                DataRow row = dtReportCol.Rows[idx];

                ASPxTextBox txtNAME = grdData.FindRowCellTemplateControl(idx, (GridViewDataColumn)grdData.Columns["NAME"], "txtNAME") as ASPxTextBox;
                txtNAME.Text = TypeConversion.ToString(row[SysReportColTable.Fields.name]);

                ASPxTextBox txtCN_NAME = grdData.FindRowCellTemplateControl(idx, (GridViewDataColumn)grdData.Columns["CN_NAME"], "txtCN_NAME") as ASPxTextBox;
                txtCN_NAME.Text = TypeConversion.ToString(row[SysReportColTable.Fields.cnName]);

                ASPxTextBox txtCOL_DATA_TYPE = grdData.FindRowCellTemplateControl(idx, (GridViewDataColumn)grdData.Columns["COL_DATA_TYPE"], "txtCOL_DATA_TYPE") as ASPxTextBox;
                txtCOL_DATA_TYPE.Text = TypeConversion.ToString(row[SysReportColTable.Fields.colDataType]);

                ASPxTextBox txtIS_QUERY = grdData.FindRowCellTemplateControl(idx, (GridViewDataColumn)grdData.Columns["IS_QUERY"], "txtIS_QUERY") as ASPxTextBox;
                txtIS_QUERY.Text = TypeConversion.ToString(row[SysReportColTable.Fields.isQuery]);

                ASPxTextBox txtSORT_RULE = grdData.FindRowCellTemplateControl(idx, (GridViewDataColumn)grdData.Columns["SORT_RULE"], "txtSORT_RULE") as ASPxTextBox;
                txtSORT_RULE.Text = TypeConversion.ToString(row[SysReportColTable.Fields.sortRule]);

                ASPxTextBox txtSORT_INDEX = grdData.FindRowCellTemplateControl(idx, (GridViewDataColumn)grdData.Columns["SORT_INDEX"], "txtSORT_INDEX") as ASPxTextBox;
                txtSORT_INDEX.Text = TypeConversion.ToString(row[SysReportColTable.Fields.sortIndex]);

                ASPxTextBox txtDECIMAL_DIGITS = grdData.FindRowCellTemplateControl(idx, (GridViewDataColumn)grdData.Columns["DECIMAL_DIGITS"], "txtDECIMAL_DIGITS") as ASPxTextBox;
                txtDECIMAL_DIGITS.Text = TypeConversion.ToString(row[SysReportColTable.Fields.decimalDigits]);
            }
        }
        #endregion

        #region 完成
        /// <summary>
        /// 完成
        /// </summary>
        private void Complete()
        {
            int rowCount = grdData.VisibleRowCount;

            try
            {

                DataTable dt = bll.GetDataTableStructure(SysReportColTable.tableName);
                for (int idx = 0; idx < rowCount; idx++)
                {
                    ASPxTextBox txtNAME = grdData.FindRowCellTemplateControl(idx, (GridViewDataColumn)grdData.Columns["NAME"], "txtNAME") as ASPxTextBox;
                    ASPxTextBox txtCN_NAME = grdData.FindRowCellTemplateControl(idx, (GridViewDataColumn)grdData.Columns["CN_NAME"], "txtCN_NAME") as ASPxTextBox;
                    ASPxTextBox txtCOL_DATA_TYPE = grdData.FindRowCellTemplateControl(idx, (GridViewDataColumn)grdData.Columns["COL_DATA_TYPE"], "txtCOL_DATA_TYPE") as ASPxTextBox;
                    ASPxTextBox txtIS_QUERY = grdData.FindRowCellTemplateControl(idx, (GridViewDataColumn)grdData.Columns["IS_QUERY"], "txtIS_QUERY") as ASPxTextBox;
                    ASPxTextBox txtSORT_RULE = grdData.FindRowCellTemplateControl(idx, (GridViewDataColumn)grdData.Columns["SORT_RULE"], "txtSORT_RULE") as ASPxTextBox;
                    ASPxTextBox txtSORT_INDEX = grdData.FindRowCellTemplateControl(idx, (GridViewDataColumn)grdData.Columns["SORT_INDEX"], "txtSORT_INDEX") as ASPxTextBox;
                    ASPxTextBox txtDECIMAL_DIGITS = grdData.FindRowCellTemplateControl(idx, (GridViewDataColumn)grdData.Columns["DECIMAL_DIGITS"], "txtDECIMAL_DIGITS") as ASPxTextBox;

                    DataRow rowCol = dt.NewRow();
                    rowCol[SysReportColTable.Fields.id] = Guid.NewGuid().ToString().Replace("-", "");
                    rowCol[SysReportColTable.Fields.name] = txtNAME.Text;
                    rowCol[SysReportColTable.Fields.cnName] = txtCN_NAME.Text ?? "";
                    rowCol[SysReportColTable.Fields.colDataType] = txtCOL_DATA_TYPE.Text ?? "NVARCHAR";
                    rowCol[SysReportColTable.Fields.isQuery] = txtIS_QUERY.Text ?? "0";
                    rowCol[SysReportColTable.Fields.sortRule] = txtSORT_RULE.Text ?? "ASC";
                    rowCol[SysReportColTable.Fields.sortIndex] = string.IsNullOrEmpty(txtSORT_INDEX.Text) == true ? 0 : TypeConversion.ToInt(txtSORT_INDEX.Text);
                    rowCol[SysReportColTable.Fields.decimalDigits] = string.IsNullOrEmpty(txtDECIMAL_DIGITS.Text) == true ? 0 : TypeConversion.ToInt(txtDECIMAL_DIGITS.Text);
                    rowCol[SysReportColTable.Fields.sysReportId] = hdnReportId.Value;
                    rowCol[SysReportColTable.Fields.createDatetime] = today;
                    rowCol[SysReportColTable.Fields.modifyDatetime] = today;

                    dt.Rows.Add(rowCol);
                }
                bll.AddDataTable(dt);

                Response.Redirect("SysReport.aspx");
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }
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
                /**
                DataRow row = bll.GetDataRowById(SysReportTable.tableName, id);
                if (row != null)
                {
                    txtNO.Text = TypeConversion.ToString(row[SysReportTable.Fields.no]);                    txtNAME.Text = TypeConversion.ToString(row[SysReportTable.Fields.name]);                    txtSOURCE_SQL.Text = TypeConversion.ToString(row[SysReportTable.Fields.sourceSql]);                    txtTARGET_SQL.Text = TypeConversion.ToString(row[SysReportTable.Fields.targetSql]);
                }
                */
            }
        }
        #endregion

        #endregion
    }
}