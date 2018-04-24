
using System;
using System.Data;
using System.Web.UI;
using Treasure.Utility.Extend;
using Treasure.Bll.ProjectCollection.SystemSetup;
using Treasure.Model.ProjectCollection.SystemSetup;
using Treasure.Utility.Utilitys;

namespace Treasure.Main.ProjectCollection.SystemSetup
{
    public partial class SysRoleEdit : System.Web.UI.Page
    {

        #region 自定义变量

        SysRoleBll bll = new SysRoleBll();
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
            Response.Redirect("SysRole.aspx");
        }
        #endregion

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        private void Add()
        {
            DataTable dt = bll.GetDataTableStructure(SysRoleTable.tableName);
            DataRow row = dt.NewRow();

            row[SysRoleTable.Fields.id] = Guid.NewGuid().ToString().Replace("-", "");
                    row[SysRoleTable.Fields.no] = txtNO.Text.Trim();                    row[SysRoleTable.Fields.name] = txtNAME.Text.Trim();

            row[SysRoleTable.Fields.createDatetime] = today;
            row[SysRoleTable.Fields.modifyDatetime] = today;

            bll.AddDataRow(row);

            Response.Redirect("SysRole.aspx");
        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改
        /// </summary>
        private void Edit()
        {
            DataRow row = bll.GetDataRowById(SysRoleTable.tableName, hdnID.Value);

                    row[SysRoleTable.Fields.no] = txtNO.Text.Trim();                    row[SysRoleTable.Fields.name] = txtNAME.Text.Trim();
            row[SysRoleTable.Fields.modifyDatetime] = today;

            bll.UpdateDataRow(row);

            Response.Redirect("SysRole.aspx");
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
                DataRow row = bll.GetDataRowById(SysRoleTable.tableName, id);
                if (row != null)
                {
                    txtNO.Text = TypeConversion.ToString(row[SysRoleTable.Fields.no]);                    txtNAME.Text = TypeConversion.ToString(row[SysRoleTable.Fields.name]);
                }
            }
        }
        #endregion

        #endregion
    }
}