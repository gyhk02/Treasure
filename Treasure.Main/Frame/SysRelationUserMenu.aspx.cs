using DevExpress.Web.ASPxTreeList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using Treasure.Bll.Frame;
using Treasure.Bll.General;
using Treasure.Model.Frame;
using Treasure.Utility.Extend;
using Treasure.Utility.Utilitys;

namespace Treasure.Main.Frame
{
    public partial class SysRelationUserMenu : System.Web.UI.Page
    {

        #region 自定义变量

        SysUserBll bll = new SysUserBll();
        DateTime today = DateTime.Now;

        #endregion

        #region 系统事件

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod == "POST")
            {
                if (Request["btnConfirm"] == "确定")
                {
                    InitData();
                    Confirm();
                    return;
                }
                if (Request["btnToBack"] == "返回")
                {
                    ToBack();
                    return;
                }
                if (Request["__CALLBACKID"] == "treMenu")
                {
                    InitData();
                    return;
                }
            }

            if (IsPostBack == false)
            {
                BasicWebBll.CheckLogin();

                //接收参数
                string userId = Request["ID"];
                hdnUserId.Value = userId;

                InitData();

                InitSelectedRow();

                DataRow row = bll.GetDataRowById(SysUserTable.tableName, userId);
                lblUser.Text = TypeConversion.ToString(row[SysUserTable.Fields.name]);
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

            string userId = hdnUserId.Value;

            //删除角色原来的用户
            if (bll.DeleteMenuListByUserId(userId) == false)
            {
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('修改菜单碰到不明错误');</script>");
                return;
            }

            DataTable dt = bll.GetDataTableStructure(SysUserMenuTable.tableName);

            foreach (TreeListNode node in treMenu.GetSelectedNodes())
            {
                DataRow row = dt.NewRow();

                row[SysUserMenuTable.Fields.id] = Guid.NewGuid().ToString().Replace("-", "");
                row[SysUserMenuTable.Fields.sysUserId] = userId;
                row[SysUserMenuTable.Fields.sysMenuItemId] = TypeConversion.ToString(node[SysMenuItemTable.Fields.id]);

                row[SysUserMenuTable.Fields.createUserId] = BasicWebBll.SeUserID;
                row[SysUserMenuTable.Fields.createDatetime] = today;
                row[SysUserMenuTable.Fields.modifyUserId] = BasicWebBll.SeUserID;
                row[SysUserMenuTable.Fields.modifyDatetime] = today;

                bll.AddDataRow(row);
            }

            clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('修改成功');</script>");

            treMenu.ExpandAll();
        }
        #endregion

        #region 返回
        /// <summary>
        /// 返回
        /// </summary>
        private void ToBack()
        {
            Response.Redirect("../ProjectCollection/SystemSetup/SysUser.aspx");
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
            DataTable dt = bll.GetMenuListByUserId(hdnUserId.Value);
            treMenu.DataSource = dt;
            treMenu.DataBind();
            treMenu.ExpandAll();
        }
        #endregion

        #region 装载数据时选中行
        /// <summary>
        /// 装载数据时选中行
        /// </summary>
        private void InitSelectedRow()
        {
            foreach (TreeListNode node in treMenu.GetVisibleNodes())
            {
                if (TypeConversion.ToInt(node["IS_SELECTED"]) == 1)
                {
                    node.Selected = true;
                }
            }
        }



        #endregion

        #endregion
        
    }
}