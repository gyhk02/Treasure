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

        SYS_MENU_ITEM_BLL bllSysMenuItem = new SYS_MENU_ITEM_BLL();
        GeneralBll bllGeneral = new GeneralBll();

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
                //父节点
                DataTable dtTreeMenuItem = bllSysMenuItem.GetMenuItemByTree();

                DataRow rowTreeMenuItem = dtTreeMenuItem.NewRow();
                rowTreeMenuItem[SysMenuItemTable.Fields.id] = null;
                rowTreeMenuItem[SysMenuItemTable.Fields.name] = ConstantVO.pleaseSelect;
                dtTreeMenuItem.Rows.InsertAt(rowTreeMenuItem, 0);

                ddlParent.DataSource = dtTreeMenuItem;
                ddlParent.DataTextField = SysMenuItemTable.Fields.name;
                ddlParent.DataValueField = SysMenuItemTable.Fields.id;
                ddlParent.DataBind();

                //类型
                DataTable dtMenuItemType = bllSysMenuItem.GetTableAllInfo(SysMenuItemTypeTable.tableName);
                ddlType.DataSource = dtMenuItemType;
                ddlType.DataTextField = SysMenuItemTypeTable.Fields.name;
                ddlType.DataValueField = SysMenuItemTypeTable.Fields.id;
                ddlType.DataBind();

                //是否启用
                DataTable dtYesOrNot = bllGeneral.GetYesOrNot();
                DropDownListExtend.BindToShowName(ddlEnable, dtYesOrNot, false);

                //是否系统菜单
                DropDownListExtend.BindToShowName(ddlIsSys, dtYesOrNot, false);

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

        /// <summary>
        /// 选择变动时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void treMenuItem_FocusedNodeChanged(object sender, EventArgs e)
        {
            TreeListNode node = treMenuItem.FocusedNode;

            string a = "";

        }

        protected void treMenuItem_CustomDataCallback(object sender, TreeListCustomDataCallbackEventArgs e)
        {
            string a = "";
        }
    }
}