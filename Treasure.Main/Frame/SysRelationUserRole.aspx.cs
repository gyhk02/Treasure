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
using Treasure.Model.General;
using Treasure.Utility.Utilitys;

namespace Treasure.Main.Frame
{
    public partial class SysRelationUserRole : System.Web.UI.Page
    {
        #region 自定义变量

        SysRoleBll bll = new SysRoleBll();
        DateTime today = DateTime.Now;

        #endregion

        #region 系统事件

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod == "POST")
            {
                if (Request["btnConfirm"] == "确定")
                {
                    Confirm();
                    return;
                }
                if (Request["btnToBack"] == "返回")
                {
                    ToBack();
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
                BasicWebBll.CheckLogin();

                //接收参数
                string roleId = Request["ID"];
                hdnSysRoleId.Value = roleId;

                InitData();

                InitSelectedRow();

                DataRow row = bll.GetDataRowById(SysRoleTable.tableName, roleId);
                lblRole.Text = TypeConversion.ToString(row[SysRoleTable.Fields.name]);
            }
        }

        #endregion

        #region 按钮

        #region 确定
        /// <summary>
        /// 确定
        /// </summary>
        private void Confirm()
        {
            ClientScriptManager clientScript = Page.ClientScript;

            string roleId = hdnSysRoleId.Value;

            //删除角色原来的用户
            if (bll.DeleteUserListByRoleId(roleId) == false)
            {
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('修改角色碰到不明错误');</script>");
                return;
            }

            DataTable dt = bll.GetDataTableStructure(SysUserRoleListTable.tableName);

            List<object> lstResult = grdData.GetSelectedFieldValues(new string[] { GeneralVO.id });
            foreach (object arr in lstResult)
            {
                DataRow row = dt.NewRow();

                row[SysUserRoleListTable.Fields.id] = Guid.NewGuid().ToString().Replace("-", "");
                row[SysUserRoleListTable.Fields.sysUserId] = TypeConversion.ToString(arr);
                row[SysUserRoleListTable.Fields.sysRoleId] = roleId;

                row[SysUserTable.Fields.createUserId] = BasicWebBll.SeUserID;
                row[SysUserTable.Fields.createDatetime] = today;
                row[SysUserTable.Fields.modifyUserId] = BasicWebBll.SeUserID;
                row[SysUserTable.Fields.modifyDatetime] = today;

                bll.AddDataRow(row);
            }

            clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('修改成功');</script>");
        }
        #endregion

        #region 返回
        /// <summary>
        /// 返回
        /// </summary>
        private void ToBack()
        {
            Response.Redirect("../ProjectCollection/SystemSetup/SysRole.aspx");
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
            DataTable dt = bll.GetUserListByRoleId(hdnSysRoleId.Value);
            grdData.DataSource = dt;
            grdData.DataBind();
        }
        #endregion

        #region 装载数据时选中行
        /// <summary>
        /// 装载数据时选中行
        /// </summary>
        private void InitSelectedRow()
        {
            for (int idx = 0; idx < grdData.VisibleRowCount; idx++)
            {
                DataRowView row = grdData.GetRow(idx) as DataRowView;
                if (TypeConversion.ToInt(row["IS_SEFT"]) == 1)
                {
                    grdData.Selection.SelectRow(idx);
                }
            }
        }
        #endregion

        #endregion

    }
}