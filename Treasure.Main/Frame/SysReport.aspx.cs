using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treasure.Bll.General;
using Treasure.BLL.Frame;
using Treasure.Model.Frame;
using Treasure.Model.General;
using Treasure.Utility.Utilitys;

namespace Treasure.Main.Frame
{
    public partial class SysReport : System.Web.UI.Page
    {
        #region 自定义变量

        SysReportBll bll = new SysReportBll();
        GeneralBll bllGeneral = new GeneralBll();

        #endregion

        #region 系统事件

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod == "POST")
            {
                if (Request["btnQuery"] == "查询")
                {
                    Query();
                    return;
                }
                if (Request["btnAdd"] == "新增")
                {
                    Add();
                    return;
                }

                if (Request["__CALLBACKID"] == "grdData")
                {
                    InitData();
                    return;
                }
            }

            if (IsPostBack == false)
            {
                InitData();
            }
        }
        #endregion

        #region 按钮



        #region 查询
        /// <summary>
        /// 查询
        /// </summary>
        private void Query()
        {


            InitData();
        }
        #endregion

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        private void Add()
        {
            Response.Redirect("SysReportEdit.aspx");
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdData_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ClientScriptManager clientScript = Page.ClientScript;

            string id = TypeConversion.ToString(e.Keys[GeneralVO.id]);
            if (bll.DeleteById(SysReportTable.tableName, id) == false)
            {
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('删除失败');</script>");
            }

            clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('删除成功');</script>");

            //要退出编辑模式才能重新绑定数据
            grdData.CancelEdit();
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
            Dictionary<string, object> dicPara = new Dictionary<string, object>();

            dicPara.Add("NAME", txtNAME.Text.Trim());
            DataTable dt = bll.Query(dicPara);
            grdData.DataSource = dt;
            grdData.DataBind();
        }
        #endregion

        #endregion
    }
}