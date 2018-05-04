using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treasure.Bll.General;
using Treasure.BLL.Template.Page;
using Treasure.Utility.Extend;

namespace Treasure.Main.Template.Page
{
    public partial class Report : System.Web.UI.Page
    {

        #region 自定义变量

        ReportBll bll = new ReportBll();
        GeneralBll bllGeneral = new GeneralBll();

        #endregion

        #region 系统事件

        protected void Page_Load(object sender, EventArgs e)
        {
            //按钮
            if (Request.HttpMethod == "POST")
            {
                if (Request["btnQuery"] == "查询")
                {
                    Query();
                    return;
                }
                if (Request["btnExcel"] == "导出Excel")
                {
                    Excel();
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
                BasicWebBll.CheckLogin();

                //是否启用
                DataTable dtYesOrNot = bllGeneral.GetYesOrNot();
                DropDownListExtend.BindToShowName(ddlIS_SYS, dtYesOrNot, false);

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

        /// <summary>
        /// 导出Excel
        /// </summary>
        private void Excel()
        {
            expData.WriteXlsToResponse("a.xls");
        }

        #endregion

        #region 自定义事件

        #region 初始化列表
        /// <summary>
        /// 初始化列表
        /// </summary>
        private void InitData()
        {
            Dictionary<string, object> dicPara = new Dictionary<string, object>();

            dicPara.Add("NAME", txtNAME.Text.Trim());            dicPara.Add("ID_INDEX", txtID_INDEX.Text.Trim());            dicPara.Add("IS_SYS", ddlIS_SYS.SelectedValue);            dicPara.Add("CREATE_DATETIME_FROM", datCREATE_DATETIME_FROM.Value);            dicPara.Add("CREATE_DATETIME_TO", datCREATE_DATETIME_TO.Value);
            DataTable dt = bll.Query(grdData.PageIndex, dicPara);
            grdData.DataSource = dt;
            grdData.DataBind();
        }
        #endregion

        #endregion


    }
}