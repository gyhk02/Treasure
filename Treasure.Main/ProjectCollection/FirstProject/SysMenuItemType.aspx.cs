using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using Treasure.BLL.ProjectCollection.FirstProject;
using Treasure.Model.General;
using Treasure.Model.ProjectCollection.FirstProject;
using Treasure.Utility.Utilitys;

namespace Treasure.Main.ProjectCollection.FirstProject
{
    public partial class SysMenuItemType : System.Web.UI.Page
    {
        #region 自定义变量

        SysMenuItemTypeBll bll = new SysMenuItemTypeBll();

        #endregion

        #region 系统事件

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod == "POST")
            {
                if (Request["btnQuery"] == "查询")
                {
                    Query();
                    return;
                }
                if (Request["btnAdd"] == "新增")
                {
                    Add();
                    return;
                }
                if (Request["btnExportExcel"] == "导出Excel")
                {
                    ExportExcel();
                    return;
                }
                if (Request["__CALLBACKID"] == "grdData")
                {
                    InitData();
                    return;
                }
            }

            if (!IsPostBack)
            {
                InitData();
            }
        }
        #endregion

        #region 自定义事件

        #region 导出Excel
        /// <summary>
        /// 导出Excel
        /// </summary>
        private void ExportExcel()
        {
            InitData();

            DataTable dt = grdData.DataSource as DataTable;
            if (dt.Rows.Count == 0)
            {
                ClientScriptManager clientScript = Page.ClientScript;
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('没有数据');</script>");
                return;
            }

            ASPxGridViewExporter1.WriteXlsToResponse();
        }
        #endregion
        
        #region 查询
        /// <summary>
        /// 查询
        /// </summary>
        private void Query()
        {
            InitData();
        }
        #endregion

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        private void Add()
        {
            Response.Redirect("SysMenuItemTypeEdit.aspx");
        }
        #endregion

        #region 初始化列表
        /// <summary>
        /// 初始化列表
        /// </summary>
        private void InitData()
        {
            Dictionary<string, object> dicPara = new Dictionary<string, object>();

            string no = txtNo.Text.Trim();
            dicPara.Add("NO", no);

            string name = txtName.Text.Trim();
            dicPara.Add("NAME", name);
            
            DataTable dt = bll.Query(dicPara);
            grdData.DataSource = dt;
            grdData.DataBind();
        }
        #endregion

        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdData_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ClientScriptManager clientScript = Page.ClientScript;
            

            string id = TypeConversion.ToString(e.Keys[GeneralVO.id]);
            if (bll.DeleteById(SysMenuItemTypeTable.tableName, id) == false)
            {
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('删除失败');</script>");
            }

            clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('删除成功');</script>");

            //要退出编辑模式才能重新绑定数据
            grdData.CancelEdit();
            e.Cancel = true;

            InitData();
        }

        #endregion
        
    }
}