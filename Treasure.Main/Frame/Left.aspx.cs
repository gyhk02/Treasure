﻿using System;
using System.Data;
using Treasure.Bll.Frame;
using Treasure.Bll.General;

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
                BasicWebBll.CheckLogin();

                hdnProjectId.Value = Request["ProjectId"];
            }

            //加载
            DataTable dtMenu = bllSysMenuItem.GetMenuListByUser(hdnProjectId.Value, BasicWebBll.SeUserID);
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