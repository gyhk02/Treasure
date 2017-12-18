using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treasure.BLL.Frame;
using Treasure.Utility.Extend;
using Treasure.Utility.Utilitys;

namespace Treasure.Main.Frame
{
    public partial class Default : System.Web.UI.Page
    {
        #region 自定义变量

        SYS_MENU_ITEM_BLL bllSysMenuItem = new SYS_MENU_ITEM_BLL();

        #endregion

        #region 系统事件

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //加载项目列表
                DataTable dtRootMenu = bllSysMenuItem.GetRootMenu();
                DropDownListExtend.BindToShowName(ddlMenu, dtRootMenu, true);
            }

            string a = "";
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