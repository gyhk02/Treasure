using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace Treasure.Main.SmallTool.ToTableClass
{
    public partial class ToTableClass : System.Web.UI.Page
    {

        #region 自定义变量
        #endregion

        #region 系统事件

        protected void Page_Load(object sender, EventArgs e)
        {


        }

        #endregion

        #region 按钮

        #region 查询
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //string pSourceConnection = hdnSourceConnection.Value;
            string pSourceConnection = "";
            string pTableName = txtTableName.Text.Trim();

            if (string.IsNullOrEmpty(pSourceConnection) == true)
            {
                MessageBox.Show("源数据库链接为空，请先点击【链接数据库】");
                return;
            }

            //DataTable dt = bll.GetTableList(pSourceConnection, pTableName);
            DataTable dt = null;
            grvTableList.DataSource = dt;
            grvTableList.DataBind();
        }
        #endregion

        #endregion

        #region 自定义事件
        #endregion


    }
}