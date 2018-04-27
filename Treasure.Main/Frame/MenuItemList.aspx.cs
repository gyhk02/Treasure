using DevExpress.Web.ASPxTreeList;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Treasure.Bll.Frame;
using Treasure.Bll.General;
using Treasure.Model.Frame;
using Treasure.Model.General;
using Treasure.Utility.Extend;
using Treasure.Utility.Utilitys;

namespace Treasure.Main.Frame
{
    public partial class MenuItemList : System.Web.UI.Page
    {

        #region 自定义变量

        SysMenuItemBll bll = new SysMenuItemBll();
        
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

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void treMenuItem_NodeDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ClientScriptManager clientScript = Page.ClientScript;

            string id = TypeConversion.ToString(e.Keys[GeneralVO.id]);

            DataRow row = bll.GetDataRowById(SysMenuItemTable.tableName, id);

            if (bll.DeleteMenu(TypeConversion.ToString(row[SysMenuItemTable.Fields.no])
                , TypeConversion.ToString(row[SysMenuItemTable.Fields.sysMenuItemTypeId])) == false)
            {
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('删除失败');</script>");
                return;
            }

            clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('删除成功');</script>");

            //要退出编辑模式才能重新绑定数据
            treMenuItem.CancelEdit();
            e.Cancel = true;

            InitData();
        }
        #endregion

        #endregion

        #region 自定义事件

        #region 初始化列表
        /// <summary>
        /// 初始化列表
        /// </summary>
        private void InitData()
        {
            DataTable dtMenuItem = bll.GetMenuItemList();
            treMenuItem.DataSource = dtMenuItem;
            treMenuItem.DataBind();
            treMenuItem.ExpandAll();
        }
        #endregion

        #endregion



    }
}