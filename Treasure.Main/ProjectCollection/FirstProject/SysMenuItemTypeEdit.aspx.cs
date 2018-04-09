using System;
using System.Data;
using System.Web.UI;
using Treasure.Bll.ProjectCollection.FirstProject;
using Treasure.Model.ProjectCollection.FirstProject;
using Treasure.Utility.Utilitys;

namespace Treasure.Main.ProjectCollection.FirstProject
{
    public partial class SysMenuItemTypeEdit : System.Web.UI.Page
    {

        #region 自定义变量

        SysMenuItemTypeBll bll = new SysMenuItemTypeBll();
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
                DataRow row = bll.GetDataRowById(SysMenuItemTypeTable.tableName, id);
                if (row != null)
                {
                    txtNO.Text = TypeConversion.ToString(row[SysMenuItemTypeTable.Fields.no]);
                    txtNAME.Text = TypeConversion.ToString(row[SysMenuItemTypeTable.Fields.name]);
                    txtSORT_INDEX.Text = TypeConversion.ToString(row[SysMenuItemTypeTable.Fields.sortIndex]);
                }
            }
        }
        #endregion

        #region 返回
        /// <summary>
        /// 返回
        /// </summary>
        private void Back()
        {
            Response.Redirect("SysMenuItemType.aspx");
        }
        #endregion

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        private void Add()
        {
            DataTable dt = bll.GetDataTableStructure(SysMenuItemTypeTable.tableName);
            DataRow row = dt.NewRow();

            row[SysMenuItemTypeTable.Fields.id] = Guid.NewGuid().ToString().Replace("-", "");
            row[SysMenuItemTypeTable.Fields.no] = txtNO.Text.Trim();
            row[SysMenuItemTypeTable.Fields.name] = txtNAME.Text.Trim();
            row[SysMenuItemTypeTable.Fields.sortIndex] = txtSORT_INDEX.Text.Trim();

            row[SysMenuItemTypeTable.Fields.createDatetime] = today;
            row[SysMenuItemTypeTable.Fields.modifyDatetime] = today;

            bll.AddDataRow(row);

            Response.Redirect("SysMenuItemType.aspx");
        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改
        /// </summary>
        private void Edit()
        {
            DataRow row = bll.GetDataRowById(SysMenuItemTypeTable.tableName, hdnID.Value);

            row[SysMenuItemTypeTable.Fields.no] = txtNO.Text.Trim();
            row[SysMenuItemTypeTable.Fields.name] = txtNAME.Text.Trim();
            row[SysMenuItemTypeTable.Fields.sortIndex] = txtSORT_INDEX.Text.Trim();

            row[SysMenuItemTypeTable.Fields.modifyDatetime] = today;

            bll.UpdateDataRow(row);

            Response.Redirect("SysMenuItemType.aspx");
        }
        #endregion

        #endregion

    }
}