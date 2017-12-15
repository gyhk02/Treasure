using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treasure.BLL.Frame;

namespace Treasure.Main.Frame
{
    public partial class Menu_Add : System.Web.UI.Page
    {
        #region 自定义变量

        SYS_MENU_ITEM_BLL bllSysMenuItem = new SYS_MENU_ITEM_BLL();

        #endregion

        #region 系统事件

        protected void Page_Load(object sender, EventArgs e)
        {
            //加载
            DataTable dtMenu = bllSysMenuItem.GetFunctionsAndPagesMenu("55c64e3590ea45fdbe671e056c497ff9");
            treMenu.DataSource = dtMenu;
            treMenu.DataBind();
            treMenu.ExpandAll();
        }
        #endregion

        #region 按钮
        #endregion

        #region 自定义事件
        #endregion


    }
}