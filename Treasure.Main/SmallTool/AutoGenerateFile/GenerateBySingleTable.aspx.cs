using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treasure.BLL.Frame;
using Treasure.BLL.General;
using Treasure.Model.Frame;
using Treasure.Model.General;
using Treasure.Model.SmallTool.DataSynchron;
using Treasure.Utility.Utilitys;

namespace Treasure.Main.SmallTool.AutoGenerateFile
{
    public partial class GenerateBySingleTable : System.Web.UI.Page
    {

        #region 自定义变量

        DataBaseBll bllDataBase = new DataBaseBll();
        SysMenuItemBll bllSysMenuItem = new SysMenuItemBll();

        #endregion

        #region 系统事件

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack == false)
            {
                //项目存储目录
                string projectFolder = "ProjectCollection";

                //目录前缀
                hdnProjectPathByPrefix.Value = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, projectFolder);

                //命名空间前缀
                hdnProjectNamespaceByPrefix.Value = typeof(GenerateBySingleTable).Assembly.GetName().Name + "." + projectFolder;

                InitTableList();
                InitProject();
            }
            else
            {
                //按钮
                if (Request.HttpMethod == "POST")
                {
                    if (Request["btnQuery"] == "查询")
                    {
                        Query();
                        return;
                    }
                    if (Request["btnGenerate"] == "开始生成")
                    {
                        Generate();
                        return;
                    }
                    if (Request["__CALLBACKID"] == "gluTableList$DDD$gv")
                    {
                        InitTableList();
                        return;
                    }
                    if (Request["__CALLBACKID"] == "gluProject$DDD$gv")
                    {
                        InitProject();
                        return;
                    }
                    if (Request["__CALLBACKID"] == "grdData")
                    {
                        InitData();
                        return;
                    }
                }
            }
        }

        #endregion

        #region 按钮
        #endregion

        #region 自定义事件

        #region 查询
        /// <summary>
        /// 查询
        /// </summary>
        private void Query()
        {
            hdnTableName.Value = gluTableList.Text.Trim();

            InitData();
        }
        #endregion

        #region 生成

        #region 生成文件入口
        /// <summary>
        /// 生成文件入口
        /// </summary>
        private void Generate()
        {
            ClientScriptManager clientScript = Page.ClientScript;

            List<string> lstFieldName = new List<string>();
            lstFieldName.Add(DataSynchronVO.FieldName);
            lstFieldName.Add(DataSynchronVO.FieldType);
            lstFieldName.Add(DataSynchronVO.FieldDescription);

            List<object> lstQueryField = grdData.GetSelectedFieldValues(lstFieldName.ToArray());

            string tableName = hdnTableName.Value;

            #region 项目英文名称

            string sysMenuItemId = TypeConversion.ToString(gluProject.Value);
            if (string.IsNullOrEmpty(sysMenuItemId) == true)
            {
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('请先选择项目');</script>");
                return;
            }

            DataRow row = bllSysMenuItem.GetDataRowById(SysMenuItemTable.tableName, sysMenuItemId);

            string projectName = TypeConversion.ToString(row[SysMenuItemTable.Fields.englishName]);
            if (string.IsNullOrEmpty(projectName) == true)
            {
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('请先录入项目的英文名称');</script>");
                return;
            }

            string projectPath = Path.Combine(hdnProjectPathByPrefix.Value, projectName);
            if (Directory.Exists(projectPath) == false)
            {
                Directory.CreateDirectory(projectPath);
            }

            #endregion

            string listFileMsg = CreateListFile(tableName, projectName, lstQueryField);
            if (string.IsNullOrEmpty(listFileMsg) == false)
            {
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('" + listFileMsg + "');</script>");
                return;
            }

            clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('生成成功');</script>");
        }
        #endregion

        #region 创建列表文件

        #region 创建列表文件主体
        /// <summary>
        /// 创建列表文件主体
        /// </summary>
        /// <returns></returns>
        private string CreateListFile(string pTableName, string pProjectName, List<object> lstQueryField)
        {
            string errorMsg = "";

            string designerMsg = CreateListFileForDesigner(pTableName, pProjectName, lstQueryField);
            if (string.IsNullOrEmpty(designerMsg) == false)
            {
                return designerMsg;
            }

            string csMsg = CreateListFileForCs(pTableName, pProjectName, lstQueryField);
            if (string.IsNullOrEmpty(csMsg) == false)
            {
                return csMsg;
            }

            string aspxMsg = CreateListFileForAspx(pTableName, pProjectName);
            if (string.IsNullOrEmpty(aspxMsg) == false)
            {
                return aspxMsg;
            }

            return errorMsg;
        }
        #endregion

        #region 列表之设计
        /// <summary>
        /// 列表之设计
        /// </summary>
        /// <returns></returns>
        private string CreateListFileForDesigner(string pTableName, string pProjectName, List<object> lstQueryField)
        {
            string errorMsg = "";

            string className = CamelName.getBigCamelName(pTableName);

            string content = GenerateBySingleTableContent.GetCreateListFileForDesignerContent(
               pTableName, pProjectName, lstQueryField, hdnProjectNamespaceByPrefix.Value, className);

            string fileName = Path.Combine(
                Path.Combine(hdnProjectPathByPrefix.Value, pProjectName)
                , className + ".aspx.designer.cs");
            File.AppendAllText(fileName, content, Encoding.UTF8);

            return errorMsg;
        }
        #endregion

        #region 列表之cs
        /// <summary>
        /// 列表之cs
        /// </summary>
        /// <returns></returns>
        private string CreateListFileForCs(string pTableName, string pProjectName, List<object> lstQueryField)
        {
            string errorMsg = "";

            string className = CamelName.getBigCamelName(pTableName);

            string content = GenerateBySingleTableContent.GetCreateListFileForCsContent(
               pTableName, pProjectName, lstQueryField, hdnProjectNamespaceByPrefix.Value, className);

            string fileName = Path.Combine(
                Path.Combine(hdnProjectPathByPrefix.Value, pProjectName)
                , CamelName.getBigCamelName(pTableName) + ".aspx.cs");
            File.AppendAllText(fileName, content.ToString(), Encoding.UTF8);

            return errorMsg;
        }
        #endregion

        #region 列表之aspx
        /// <summary>
        /// 列表之aspx
        /// </summary>
        /// <returns></returns>
        private string CreateListFileForAspx(string pTableName, string pProjectName)
        {
            string errorMsg = "";

            string content = "";

            string fileName = Path.Combine(hdnProjectPathByPrefix.Value + @"\\" + pProjectName, CamelName.getBigCamelName(pTableName) + ".aspx");
            File.AppendAllText(fileName, content, Encoding.UTF8);

            return errorMsg;
        }
        #endregion

        #endregion

        #region 创建编辑文件

        /// <summary>
        /// 创建编辑文件
        /// </summary>
        /// <returns></returns>
        private string CreateEditFile()
        {
            string errorMsg = "";
            return errorMsg;
        }

        /// <summary>
        /// 编辑之设计
        /// </summary>
        /// <returns></returns>
        private string CreateEditFileForDesigner()
        {
            string errorMsg = "";
            return errorMsg;
        }

        /// <summary>
        /// 编辑之cs
        /// </summary>
        /// <returns></returns>
        private string CreateEditFileForCs()
        {
            string errorMsg = "";
            return errorMsg;
        }

        /// <summary>
        /// 编辑之aspx
        /// </summary>
        /// <returns></returns>
        private string CreateEditFileForAspx()
        {
            string errorMsg = "";
            return errorMsg;
        }

        #endregion

        #endregion

        #region 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        private void InitData()
        {
            string tableName = hdnTableName.Value;

            DataTable dt = null;
            if (string.IsNullOrEmpty(tableName) == false)
            {
                string colIsForeign = "IsForeign";
                dt = bllDataBase.GetTableInfoByName(1, tableName);
                dt.Columns.Add(colIsForeign);
                foreach (DataRow row in dt.Rows)
                {
                    if (TypeConversion.ToBool(row["isForeign"]) == true)
                    {
                        row[colIsForeign] = "√";
                    }
                }
            }
            grdData.DataSource = dt;
            grdData.DataBind();
        }
        #endregion

        #region 初始化表的列表
        /// <summary>
        /// 初始化表的列表
        /// </summary>
        private void InitTableList()
        {
            //全部表
            DataTable dt = bllDataBase.GetTableList();
            gluTableList.DataSource = dt;
            gluTableList.DataBind();
        }
        #endregion

        #region 初始化项目列表
        /// <summary>
        /// 初始化项目列表
        /// </summary>
        private void InitProject()
        {
            //全部表
            DataTable dt = bllSysMenuItem.GetProjectByNoSys();
            gluProject.DataSource = dt;
            gluProject.DataBind();
        }
        #endregion

        #endregion

    }
}