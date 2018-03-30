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
    public partial class Left : System.Web.UI.Page
    {
        #region 自定义变量

        SysMenuItemBll bllSysMenuItem = new SysMenuItemBll();

        #endregion

        #region 系统事件

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hdnProjectId.Value = Request["ProjectId"];
            }

            //加载
            DataTable dtMenu = bllSysMenuItem.GetFunctionsAndPagesMenu(hdnProjectId.Value);
            treMenu.DataSource = dtMenu;
            treMenu.DataBind();
            treMenu.ExpandAll();

          //SysMenuItemInfo.Fields.ID
              

        }
        #endregion

        #region 按钮
        #endregion

        #region 自定义事件
        #endregion
    }
}