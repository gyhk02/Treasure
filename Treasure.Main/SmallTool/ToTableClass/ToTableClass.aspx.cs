using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Treasure.BLL.General;
using Treasure.Model.Frame;
using Treasure.Model.General;
using Treasure.Model.SmallTool.DataSynchron;
using Treasure.Utility.Extend;
using Treasure.Utility.Utilitys;

namespace Treasure.Main.SmallTool.ToTableClass
{
    public partial class ToTableClass : System.Web.UI.Page
    {

        #region 自定义变量

        BasicBLL bll = new BasicBLL();
        DataBaseBLL bllDataBase = new DataBaseBLL();

        #endregion

        #region 系统事件

        protected void Page_Load(object sender, EventArgs e)
        {
            //绑定数据库链接
            if (!IsPostBack)
            {
                //绑定数据库
                DataTable dtDatabase = bllDataBase.GetDatabaseLinks();
                DropDownListExtend.BindToShowNo(ddlDataBase, dtDatabase, false);

                Session["dtDataDb"] = dtDatabase;
                ddlDataBase.SelectedValue = "1";
                ddlDataBase_SelectedIndexChanged(sender, e);
            }
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
            //如果数据库没有链接，将提示要先连接数据库

            //根据条件查询数据，并绑定列表

            //string pSourceConnection = hdnSourceConnection.Value;
            string pSourceConnection = "";
            string pTableName = txtTableName.Text.Trim();

            if (string.IsNullOrEmpty(pSourceConnection) == true)
            {
                MessageBox.Show("源数据库链接为空，请先点击【链接数据库】");
                return;
            }

            string a = SysMenuItemInfo.Fields.no;

            //DataTable dt = bll.GetTableList(pSourceConnection, pTableName);
            DataTable dt = null;
            grvTableList.DataSource = dt;
            grvTableList.DataBind();
        }
        #endregion

        #region 链接数据库
        /// <summary>
        /// 链接数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConnection_Click(object sender, EventArgs e)
        {
            string connStr = hdnConnection.Value;
            if (string.IsNullOrEmpty(connStr) == true)
            {
                MessageBox.Show("数据库不能为空");
                return;
            }
            if (bllDataBase.JudgeConneStr(connStr) == true)
            {
                lblShowConnectionResult.Text = "连接成功(" + DateTime.Now.ToString(ConstantVO.DATETIME_Y_M_D_H_M_S) + ")";
            }
            else
            {
                lblShowConnectionResult.Text = "连接异常(" + DateTime.Now.ToString(ConstantVO.DATETIME_Y_M_D_H_M_S) + ")";
                return;
            }

            //ListItem item = ddlDataBase.SelectedItem;

            //如果正常就绑定列表，并提示正常(要显示时间)，将数据库链接字符串赋值到隐藏控件

            DataTable dtTable = bllDataBase.GetTableList(connStr);
            grvTableList.DataSource = dtTable;
            grvTableList.DataBind();
            

            //如果异常将提示异常(要显示时间), 隐藏控件的值置空

            
        }
        #endregion

        #region 生成Model类
        /// <summary>
        /// 生成Model类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            //提示：命名空间不能为空
            //提示：生成文件路径不能为空
            //提示：请先指定要生成表的集合
        }
        #endregion

        #region 选择数据库
        /// <summary>
        /// 选择数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlDataBase_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = TypeConversion.ToInt(ddlDataBase.SelectedValue);

            DataTable dtDatabase = Session["dtDataDb"] as DataTable;
            List<DataRow> lstRow = dtDatabase.AsEnumerable().Where(p => p.Field<int>(GeneralVO.Id) == id).ToList();

            if (lstRow.Count == 1)
            {
                DataRow row = lstRow[0];

                string strSouceConnection = "Data Source=" + row[DataSynchronVO.Ip].ToString()
                    + ";Initial Catalog=" + row[DataSynchronVO.DbName].ToString()
                    + ";User ID=" + row[DataSynchronVO.LoginName].ToString()
                    + ";Password=" + row[DataSynchronVO.Pwd].ToString() + ";Persist Security Info=True;";

                hdnConnection.Value = strSouceConnection;
            }
            else
            {
                hdnConnection.Value = "";
            }
        }
        #endregion

        #endregion

        #region 自定义事件
        #endregion

    }
}