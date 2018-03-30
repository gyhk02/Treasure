using DevExpress.Web.ASPxTreeList;
using System;
using System.Data;
using Treasure.BLL.Frame;
using Treasure.BLL.General;
using Treasure.Model.Frame;
using Treasure.Model.General;
using Treasure.Utility.Extend;

namespace Treasure.Main.Frame
{
    public partial class MenuItemList : System.Web.UI.Page
    {

        #region 自定义变量

        SysMenuItemBll bllSysMenuItem = new SysMenuItemBll();
       

        #endregion

        #region 系统事件

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod == "POST" && Request["__CALLBACKID"] == "treMenuItem")
            {
                InitData();
                return;
            }

            if (!IsPostBack)
            {
                InitData();
            }
        }

        #endregion

        #region 按钮
        #endregion

        #region 自定义事件

        #region 初始化列表
        /// <summary>
        /// 初始化列表
        /// </summary>
        private void InitData()
        {
            DataTable dtMenuItem = bllSysMenuItem.GetMenuItemList();
            treMenuItem.DataSource = dtMenuItem;
            treMenuItem.DataBind();
            treMenuItem.ExpandAll();
        }
        #endregion

        #endregion

    }
}