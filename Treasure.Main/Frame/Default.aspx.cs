using System;
using System.Data;
using System.Web;
using Treasure.Bll.Frame;
using Treasure.Bll.General;
using Treasure.Model.Frame;
using Treasure.Utility.Extend;
using Treasure.Utility.Utilitys;

namespace Treasure.Main.Frame
{
    public partial class Default : System.Web.UI.Page
    {
        #region 自定义变量

        SysMenuItemBll bllSysMenuItem = new SysMenuItemBll();

        #endregion

        #region 系统事件

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod == "POST" && Request["btnSignOut"] == "退出登录")
            {
                BasicWebBll.SeUserID = null;
                BasicWebBll.CheckLogin();

                return;
            }
            if (!IsPostBack)
            {
                BasicWebBll.CheckLogin();
                
                //加载项目列表
                DataTable dtRootMenu = bllSysMenuItem.GetMenuListByUser("", BasicWebBll.SeUserID);
                dtRootMenu.DefaultView.RowFilter = "PARENT_ID = ''";
                DropDownListExtend.BindToShowName(ddlMenu, dtRootMenu, true);
            }
        }

        #endregion

        #region 按钮

        protected void ddlMenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            string proejctId = ddlMenu.SelectedValue;
            frmLeft.Src = "Left.aspx?ProjectId=" + proejctId;
        }

        #endregion

        #region 自定义事件


        #endregion

    }
}