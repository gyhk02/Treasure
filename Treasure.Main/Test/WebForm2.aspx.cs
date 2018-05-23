using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treasure.Bll.General;
using Treasure.Model.Frame;
using Treasure.Utility.Utilitys;

namespace Treasure.Main.Test
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        #region 自定义变量

        BasicBll bll = new BasicBll();

        #endregion

        #region 系统事件

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod == "POST")
            {
                if (Request["btnSave"] == "保存")
                {
                    Save();
                    return;
                }
                if (Request["__CALLBACKID"] == "grvData")
                {
                    InitData();
                    return;
                }
            }

            if (IsPostBack == false)
            {
                InitData();
            }
        }

        #endregion

        #region 按钮

        #endregion

        #region 自定义事件

        #region 初始化数据
        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData()
        {
            DataTable dt = bll.GetTableAllInfo(TemplateTestTable.tableName);
            grvData.DataSource = dt;
            grvData.DataBind();

            GridViewDataColumn columnID = (GridViewDataColumn)grvData.Columns["ID"];
            GridViewDataColumn columnNO = (GridViewDataColumn)grvData.Columns["NO"];
            GridViewDataColumn columnNAME = (GridViewDataColumn)grvData.Columns["NAME"];
            GridViewDataColumn columnTITLE = (GridViewDataColumn)grvData.Columns["TITLE"];

            for (int idx = 0; idx < dt.Rows.Count; idx++)
            {
                DataRow row = dt.Rows[idx];

                ASPxTextBox txtId = grvData.FindRowCellTemplateControl(idx, columnID, "txtId") as ASPxTextBox;
                txtId.Text = TypeConversion.ToString(row[TemplateTestTable.Fields.id]);

                ASPxTextBox txtNo = grvData.FindRowCellTemplateControl(idx, columnNO, "txtNo") as ASPxTextBox;
                txtNo.Text = TypeConversion.ToString(row[TemplateTestTable.Fields.no]);

                ASPxTextBox txtName = grvData.FindRowCellTemplateControl(idx, columnNAME, "txtName") as ASPxTextBox;
                txtName.Text = TypeConversion.ToString(row[TemplateTestTable.Fields.name]);

                ASPxTextBox txtTitle = grvData.FindRowCellTemplateControl(idx, columnTITLE, "txtTitle") as ASPxTextBox;
                txtTitle.Text = TypeConversion.ToString(row[TemplateTestTable.Fields.title]);
            }
        }
        #endregion

        #endregion

        private void Save() {
            int rowCount = grvData.VisibleRowCount;
            GridViewDataColumn columnID = (GridViewDataColumn)grvData.Columns["ID"];
            GridViewDataColumn columnNO = (GridViewDataColumn)grvData.Columns["NO"];
            GridViewDataColumn columnNAME = (GridViewDataColumn)grvData.Columns["NAME"];
            GridViewDataColumn columnTITLE = (GridViewDataColumn)grvData.Columns["TITLE"];

            DataRow row = null;
            for (int idx = 0; idx < rowCount; idx++)
            {
                ASPxTextBox txtId = grvData.FindRowCellTemplateControl(idx, columnID, "txtId") as ASPxTextBox;
                ASPxTextBox txtNo = grvData.FindRowCellTemplateControl(idx, columnNO, "txtNo") as ASPxTextBox;
                ASPxTextBox txtName = grvData.FindRowCellTemplateControl(idx, columnNAME, "txtName") as ASPxTextBox;
                ASPxTextBox txtTitle = grvData.FindRowCellTemplateControl(idx, columnTITLE, "txtTitle") as ASPxTextBox;

                row = bll.GetDataRowById(TemplateTestTable.tableName, txtId.Text);
                row[TemplateTestTable.Fields.no] = txtNo.Text;
                row[TemplateTestTable.Fields.name] = txtName.Text;
                row[TemplateTestTable.Fields.title] = txtTitle.Text;

                bll.UpdateDataRow(row);
            }
        }


    }
}