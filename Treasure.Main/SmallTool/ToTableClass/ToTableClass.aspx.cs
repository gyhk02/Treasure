﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treasure.BLL.General;
using Treasure.Model.Frame;
using Treasure.Model.General;
using Treasure.Model.SmallTool.DataSynchron;
using Treasure.Utility.Extend;
using Treasure.Utility.Helpers;
using Treasure.Utility.Utilitys;

namespace Treasure.Main.SmallTool.ToTableClass
{
    public partial class ToTableClass : System.Web.UI.Page
    {

        #region 自定义变量
        
        DataBaseBLL bllDataBase = new DataBaseBLL();
        CamelNameBLL bllCamelName = new CamelNameBLL();

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
            string pConnStr = hdnConnection.Value;
            string pTableName = txtTableName.Text.Trim();

            if (string.IsNullOrEmpty(pConnStr) == true)
            {
                ClientScriptManager clientScript = Page.ClientScript;
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('数据库没有链接');</script>");
                return;
            }

            DataTable dt = bllDataBase.GetTableList(pConnStr, pTableName);
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
                ClientScriptManager clientScript = Page.ClientScript;
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('数据库不能为空');</script>");
                return;
            }
            if (bllDataBase.JudgeConneStr(connStr) == true)
            {
                DataTable dtTable = bllDataBase.GetTableList(connStr);
                grvTableList.DataSource = dtTable;
                grvTableList.DataBind();

                lblShowConnectionResult.Text = "连接成功(" + DateTime.Now.ToString(ConstantVO.DATETIME_Y_M_D_H_M_S) + ")";
            }
            else
            {
                grvTableList.DataSource = null;
                grvTableList.DataBind();

                lblShowConnectionResult.Text = "连接异常(" + DateTime.Now.ToString(ConstantVO.DATETIME_Y_M_D_H_M_S) + ")";
                return;
            }
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
            ClientScriptManager clientScript = Page.ClientScript;

            string connStr = hdnConnection.Value;
            string strNamespace = txtNamespace.Text;
            string strSavePath = txtSavePath.Text;
            List<string> lstTableList = grvTableList.GetSelectedFieldValues(new string[] { DataSynchronVO.TableName }).ConvertAll<string>(c => string.Format("{0}", c));

            #region 判断

            if (string.IsNullOrEmpty(connStr) == true)
            {
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('数据库还没有链接');</script>");
                btnConnection.Focus();
                return;
            }
            if (string.IsNullOrEmpty(strNamespace) == true)
            {
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('命名空间不能为空');</script>");
                txtNamespace.Focus();
                return;
            }
            if (string.IsNullOrEmpty(strSavePath) == true)
            {
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('请先选择要保存Model文件的目录');</script>");
                btnGetPath.Focus();
                return;
            }
            if (Directory.Exists(strSavePath) == false)
            {
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('保存Model文件的目录不存在');</script>");
                return;
            }
            if (lstTableList.Count == 0)
            {
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('请先选择要生成Model的表');</script>");
                return;
            }

            #endregion

            string parentPath = strSavePath + "\\AutoGenerated";
            if (Directory.Exists(parentPath) == false)
            {
                Directory.CreateDirectory(parentPath);
            }

            string createFileName = "";
            string strTmp = "";
            string content = "";
            foreach (string str in lstTableList)
            {
                createFileName = bllCamelName.getBigCamelName(str) + "Table";
                strTmp = strSavePath + "\\" + createFileName;
                content = GetFileContent(strNamespace, createFileName);

                if (string.IsNullOrEmpty(content) == true) { continue; }

                if (File.Exists(strTmp) == false)
                {
                    File.AppendAllText(strTmp, content, Encoding.UTF8);
                }

                FileStream fs = new FileStream("a.txt", FileMode.Open, FileAccess.Write); 
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                sw.Write("");
            }
        }
        #endregion

        #region 获取文件内容
        /// <summary>
        /// 获取文件内容
        /// </summary>
        /// <returns></returns>
        private string GetFileContent(string pNamespace, string pFileName)
        {
            string result = "";

            result = @"
namespace " + pNamespace + @"
{
    public partial class " + pFileName + @"
    {
    }
}
            ";

            return result;
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

        #region 选择保存路径
        /// <summary>
        /// 选择保存路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGetPath_Click(object sender, EventArgs e)
        {
            FileHelper file = new FileHelper();
            txtSavePath.Text = file.SelectPath();
        }
        #endregion

        #endregion

        #region 自定义事件
        #endregion

    }
}