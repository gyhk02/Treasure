using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treasure.BLL.Frame;
using Treasure.BLL.General;
using Treasure.Model.Frame;
using Treasure.Model.General;
using Treasure.Utility.Extend;
using Treasure.Utility.Utilitys;

namespace Treasure.Main.Frame
{
    public partial class MenuAdd : System.Web.UI.Page
    {
        #region 自定义变量

        SysMenuItemBll bll = new SysMenuItemBll();
        GeneralBll bllGeneral = new GeneralBll();
        DateTime today = DateTime.Now;

        #endregion

        #region 系统事件

        protected void Page_Load(object sender, EventArgs e)
        {
            //按钮
            if (Request.HttpMethod == "POST")
            {
                if (Request["btnAdd"] == "新增")
                {
                    Add();
                }
                if (Request["btnEdit"] == "修改")
                {
                    Edit();
                }
                if (Request["btnBack"] == "返回")
                {
                    Back();
                }
            }

            if (IsPostBack == false)
            {
                //父节点
                DataTable dtTreeMenuItem = bll.GetMenuItemByTree();

                DataRow rowTreeMenuItem = dtTreeMenuItem.NewRow();
                rowTreeMenuItem[SysMenuItemTable.Fields.id] = null;
                rowTreeMenuItem[SysMenuItemTable.Fields.name] = ConstantVO.pleaseSelect;
                dtTreeMenuItem.Rows.InsertAt(rowTreeMenuItem, 0);

                ddlParentId.DataSource = dtTreeMenuItem;
                ddlParentId.DataTextField = SysMenuItemTable.Fields.name;
                ddlParentId.DataValueField = SysMenuItemTable.Fields.id;
                ddlParentId.DataBind();

                //是否启用
                DataTable dtYesOrNot = bllGeneral.GetYesOrNot();
                DropDownListExtend.BindToShowName(ddlEnable, dtYesOrNot, false);

                //是否系统菜单
                DropDownListExtend.BindToShowName(ddlIsSys, dtYesOrNot, false);

                //接收参数
                hdnId.Value = Request["ID"];

                //显示隐藏新增或修改按钮
                ClientScriptManager clientScript = Page.ClientScript;
                if (hdnId.Value != null && string.IsNullOrEmpty(hdnId.Value) == false)
                {
                    clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>ShowAddOrEdit('btnEdit');</script>");
                }
                else
                {
                    clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>ShowAddOrEdit('btnAdd');</script>");
                }

                InitData();
            }
        }

        #endregion

        #region 按钮
        #endregion

        #region 自定义事件

        #region 初始化数据
        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData()
        {
            string id = hdnId.Value;

            if (string.IsNullOrEmpty(id) == false)
            {
                DataRow row = bll.GetDataRowById(SysMenuItemTable.tableName, id);
                if (row != null)
                {
                    txtName.Text = row[SysMenuItemTable.Fields.name].ToString();
                    txtEnglishName.Text = TypeConversion.ToString(row[SysMenuItemTable.Fields.englishName]);
                    ddlParentId.SelectedValue = row[SysMenuItemTable.Fields.parentId].ToString();
                    txtPictureUrl.Text = row[SysMenuItemTable.Fields.pictureUrl].ToString();
                    txtFileUrl.Text = row[SysMenuItemTable.Fields.fileUrl].ToString();
                    txtButtonName.Text = row[SysMenuItemTable.Fields.buttonName].ToString();
                    ddlEnable.SelectedValue = row[SysMenuItemTable.Fields.enable].ToString();
                    ddlIsSys.SelectedValue = row[SysMenuItemTable.Fields.isSys].ToString();
                }
            }
        }
        #endregion

        #region 返回
        /// <summary>
        /// 返回
        /// </summary>
        private void Back()
        {
            Response.Redirect("MenuItemList.aspx");
        }
        #endregion

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        private void Add()
        {
            DataTable dt = bll.GetDataTableStructure(SysMenuItemTable.tableName);
            DataRow row = dt.NewRow();

            string parentId = ddlParentId.SelectedValue;
            string sysMenuItemTypeId = "1";
            if (string.IsNullOrEmpty(parentId) == false)
            {
                DataRow rowParent = bll.GetDataRowById(SysMenuItemTable.tableName, parentId);
                int parentSysMenuItemTypeId = TypeConversion.ToInt(rowParent[SysMenuItemTable.Fields.sysMenuItemTypeId]);
                if (parentSysMenuItemTypeId == 4)
                {
                    ClientScriptManager clientScript = Page.ClientScript;
                    clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('父节点已经是按钮，不能在下面添加子节点');</script>");
                    return;
                }
                sysMenuItemTypeId = (parentSysMenuItemTypeId + 1).ToString();
            }

            row[SysMenuItemTable.Fields.no] = bll.GetMenuItemNo(parentId);
            row[SysMenuItemTable.Fields.parentId] = parentId;
            row[SysMenuItemTable.Fields.sysMenuItemTypeId] = sysMenuItemTypeId;

            row[SysMenuItemTable.Fields.id] = Guid.NewGuid().ToString().Replace("-", "");
            row[SysMenuItemTable.Fields.name] = txtName.Text.Trim();
            row[SysMenuItemTable.Fields.englishName] = txtEnglishName.Text.Trim();
            row[SysMenuItemTable.Fields.pictureUrl] = txtPictureUrl.Text.Trim();
            row[SysMenuItemTable.Fields.fileUrl] = txtFileUrl.Text.Trim();
            row[SysMenuItemTable.Fields.buttonName] = txtButtonName.Text.Trim();
            row[SysMenuItemTable.Fields.enable] = ddlEnable.SelectedValue;
            row[SysMenuItemTable.Fields.isSys] = ddlIsSys.SelectedValue;

            row[SysMenuItemTable.Fields.createDatetime] = today;
            row[SysMenuItemTable.Fields.modifyDatetime] = today;

            bll.AddDataRow(row);

            Response.Redirect("MenuItemList.aspx");
        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改
        /// </summary>
        private void Edit()
        {
            DataRow row = bll.GetDataRowById(SysMenuItemTable.tableName, hdnId.Value);

            string parentId = ddlParentId.SelectedValue;
            string sysMenuItemTypeId = "1";
            if (string.IsNullOrEmpty(parentId) == false)
            {
                DataRow rowParent = bll.GetDataRowById(SysMenuItemTable.tableName, parentId);
                int parentSysMenuItemTypeId = TypeConversion.ToInt(rowParent[SysMenuItemTable.Fields.sysMenuItemTypeId]);
                if (parentSysMenuItemTypeId == 4)
                {
                    ClientScriptManager clientScript = Page.ClientScript;
                    clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('父节点已经是按钮，不能在下面添加子节点');</script>");
                    return;
                }
                sysMenuItemTypeId = (parentSysMenuItemTypeId + 1).ToString();
            }

            row[SysMenuItemTable.Fields.no] = bll.GetMenuItemNo(parentId);
            row[SysMenuItemTable.Fields.parentId] = parentId;
            row[SysMenuItemTable.Fields.sysMenuItemTypeId] = sysMenuItemTypeId;

            row[SysMenuItemTable.Fields.name] = txtName.Text.Trim();
            row[SysMenuItemTable.Fields.englishName] = txtEnglishName.Text.Trim();
            row[SysMenuItemTable.Fields.pictureUrl] = txtPictureUrl.Text.Trim();
            row[SysMenuItemTable.Fields.fileUrl] = txtFileUrl.Text.Trim();
            row[SysMenuItemTable.Fields.buttonName] = txtButtonName.Text.Trim();
            row[SysMenuItemTable.Fields.enable] = ddlEnable.SelectedValue;
            row[SysMenuItemTable.Fields.isSys] = ddlIsSys.SelectedValue;

            row[SysMenuItemTable.Fields.modifyDatetime] = today;

            bll.UpdateDataRow(row);

            Response.Redirect("MenuItemList.aspx");
        }
        #endregion

        #endregion

    }
}