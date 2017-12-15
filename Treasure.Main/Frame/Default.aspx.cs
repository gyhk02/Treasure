using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treasure.BLL.Frame;
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
                lupProject.DataSource = dtRootMenu;
                lupProject.DataBind();
            }

            string a = "";
        }

        #endregion

        #region 按钮

        #region 选择项目
        /// <summary>
        /// 选择项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lupProject_TextChanged(object sender, EventArgs e)
        {
            string projectId = TypeConversion.ToString(lupProject.Value);
            //Response.Write("<script type='text/javascript'>location.href='Left.aspx?ProjectId=" + projectId + "' target='frmLeft';</script>");
            //string url = "<script>window.parent.document.location='Left.aspx?ProjectId=" + projectId + "');</script>";
            //Response.Write(url);
        }
        #endregion

        protected void lupProject_ValueChanged(object sender, EventArgs e)
        {
            string a = "";
        }

        #endregion

        #region 自定义事件


        #endregion

    }
}