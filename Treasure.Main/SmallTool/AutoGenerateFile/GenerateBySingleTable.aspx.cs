using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI;
using Treasure.Bll.Frame;
using Treasure.Bll.General;
using Treasure.Model.Frame;
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
                hdnProjectRootFolder.Value = projectFolder;

                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

                //当前运行的项目名称
                string tmpStr = baseDirectory;
                string[] tmpArr = tmpStr.Split(new string[] { "\\" }, StringSplitOptions.None);
                string runProjectName = tmpArr[tmpArr.Length - 1 - 1];
                hdnRunProjectName.Value = runProjectName;

                //解决方案路径
                tmpStr = tmpStr.Replace("\\" + runProjectName + "\\", "");
                hdnSolutionPath.Value = tmpStr;

                //解决方案名称
                tmpArr = tmpStr.Split(new string[] { "\\" }, StringSplitOptions.None);
                hdnSolutionName.Value = tmpArr[tmpArr.Length - 1];

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

            string projectPath = hdnSolutionPath.Value + @"\\" + hdnRunProjectName.Value + @"\\" + hdnProjectRootFolder.Value + @"\\" + projectName;
            if (Directory.Exists(projectPath) == false)
            {
                Directory.CreateDirectory(projectPath);
            }

            #endregion

            DataTable fieldTable = Session["FieldTable"] as DataTable;
            if (fieldTable == null || fieldTable.Rows.Count == 0)
            {
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('此表没有字段');</script>");
                return;
            }

            //列表文件
            string listFileMsg = CreateListFile(tableName, projectName, lstQueryField, fieldTable);
            if (string.IsNullOrEmpty(listFileMsg) == false)
            {
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('" + listFileMsg + "');</script>");
                return;
            }

            //编辑文件
            string editFileMsg = CreateEditFile(tableName, projectName, lstQueryField, fieldTable);
            if (string.IsNullOrEmpty(editFileMsg) == false)
            {
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('" + editFileMsg + "');</script>");
                return;
            }

            //BLL文件
            string bllFileMsg = CreateBllFile(tableName, projectName, lstQueryField);
            if (string.IsNullOrEmpty(bllFileMsg) == false)
            {
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('" + bllFileMsg + "');</script>");
                return;
            }

            //Model文件
            string modelFileMsg = CreateModelFile(tableName, projectName, fieldTable);
            if (string.IsNullOrEmpty(modelFileMsg) == false)
            {
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('" + modelFileMsg + "');</script>");
                return;
            }

            clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('生成成功');</script>");
        }
        #endregion

        #region 创建Model文件

        #region 创建Model文件主体
        /// <summary>
        /// 创建Model文件主体
        /// </summary>
        /// <returns></returns>
        private string CreateModelFile(string pTableName, string pProjectName, DataTable pFieldTable)
        {
            string errorMsg = "";

            string editMsg = CreateModelFileForEdit(pTableName, pProjectName);
            if (string.IsNullOrEmpty(editMsg) == false)
            {
                return editMsg;
            }

            string parentMsg = CreateModelFileForParent(pTableName, pProjectName, pFieldTable);
            if (string.IsNullOrEmpty(parentMsg) == false)
            {
                return parentMsg;
            }

            return errorMsg;
        }
        #endregion

        #region Model之可编辑
        /// <summary>
        /// Model之可编辑
        /// </summary>
        /// <returns></returns>
        private string CreateModelFileForEdit(string pTableName, string pProjectName)
        {
            string errorMsg = "";

            string className = CamelName.getBigCamelName(pTableName);

            string thePath = hdnSolutionPath.Value + @"\\" + hdnSolutionName.Value + ".Model\\" + hdnProjectRootFolder.Value + @"\\" + pProjectName;
            if (Directory.Exists(thePath) == false)
            {
                Directory.CreateDirectory(thePath);
            }
            string fileName = thePath + @"\\" + className + "Table.cs";

            //文件不存在才创建
            if (File.Exists(fileName) == false)
            {
                string projectNamespace = hdnSolutionName.Value + ".Model." + hdnProjectRootFolder.Value + "." + pProjectName;

                string content = GenerateBySingleTableContent.GetCreateModelFileForEditContent(projectNamespace, className);

                File.AppendAllText(fileName, content, Encoding.UTF8);
            }
            return errorMsg;
        }
        #endregion

        #region Model之可Parent
        /// <summary>
        /// Model之可Parent
        /// </summary>
        /// <returns></returns>
        private string CreateModelFileForParent(string pTableName, string pProjectName, DataTable pFieldTable)
        {
            string errorMsg = "";

            string className = CamelName.getBigCamelName(pTableName);

            string thePath = hdnSolutionPath.Value + @"\\" + hdnSolutionName.Value + ".Model\\"
                + hdnProjectRootFolder.Value + @"\\" + pProjectName + "\\AutoGenerated";
            if (Directory.Exists(thePath) == false)
            {
                Directory.CreateDirectory(thePath);
            }
            string fileName = thePath + @"\\" + className + "ParentTable.cs";

            string projectNamespace = hdnSolutionName.Value + ".Model." + hdnProjectRootFolder.Value + "." + pProjectName;

            string content = GenerateBySingleTableContent.GetCreateModelFileForParentContent(
                pTableName, projectNamespace, className, pFieldTable);

            File.Delete(fileName);
            File.AppendAllText(fileName, content.ToString(), Encoding.UTF8);

            return errorMsg;
        }
        #endregion

        #endregion

        #region 创建BLL文件
        /// <summary>
        /// 创建BLL文件
        /// </summary>
        /// <param name="pTableName"></param>
        /// <param name="pProjectName"></param>
        /// <param name="lstQueryField"></param>
        /// <returns></returns>
        private string CreateBllFile(string pTableName, string pProjectName, List<object> lstQueryField)
        {
            string errorMsg = "";

            string className = CamelName.getBigCamelName(pTableName);

            string filePath = hdnSolutionPath.Value + @"\\" + hdnSolutionName.Value + ".Bll\\" + hdnProjectRootFolder.Value + @"\\" + pProjectName;
            if (Directory.Exists(filePath) == false)
            {
                Directory.CreateDirectory(filePath);
            }
            string fileName = filePath + @"\\" + className + "Bll.cs";

            //文件不存在才创建
            if (File.Exists(fileName) == false)
            {
                string projectNamespace = hdnSolutionName.Value + ".Bll." + hdnProjectRootFolder.Value + "." + pProjectName;

                string content = GenerateBySingleTableContent.GetCreateBllFileContent(
                    pTableName, lstQueryField, projectNamespace, className, hdnSolutionName.Value);

                File.AppendAllText(fileName, content.ToString(), Encoding.UTF8);
            }
            return errorMsg;
        }
        #endregion

        #region 创建列表文件

        #region 创建列表文件主体
        /// <summary>
        /// 创建列表文件主体
        /// </summary>
        /// <returns></returns>
        private string CreateListFile(string pTableName, string pProjectName, List<object> lstQueryField, DataTable pFieldTable)
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

            string aspxMsg = CreateListFileForAspx(pTableName, pProjectName, lstQueryField, pFieldTable);
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

            string thePath = hdnSolutionPath.Value + @"\\" + hdnRunProjectName.Value + @"\\" + hdnProjectRootFolder.Value + @"\\" + pProjectName;
            if (Directory.Exists(thePath) == false)
            {
                Directory.CreateDirectory(thePath);
            }

            string fileName = thePath + @"\\" + className + ".aspx.designer.cs";

            //文件不存在才创建
            if (File.Exists(fileName) == false)
            {
                string projectNamespace = hdnRunProjectName.Value + "." + hdnProjectRootFolder.Value + "." + pProjectName;

                string content = GenerateBySingleTableContent.GetCreateListFileForDesignerContent(
                   pTableName, lstQueryField, projectNamespace, className);

                File.AppendAllText(fileName, content, Encoding.UTF8);
            }
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

            string thePath = hdnSolutionPath.Value + @"\\" + hdnRunProjectName.Value + @"\\" + hdnProjectRootFolder.Value + @"\\" + pProjectName;
            if (Directory.Exists(thePath) == false)
            {
                Directory.CreateDirectory(thePath);
            }

            string fileName = thePath + @"\\" + className + ".aspx.cs";

            //文件不存在才创建
            if (File.Exists(fileName) == false)
            {
                string solutionName = hdnSolutionName.Value;

                string projectNamespace = hdnRunProjectName.Value + "." + hdnProjectRootFolder.Value + "." + pProjectName;

                bool IsReport = chkReportExcel.Checked;

                string content = GenerateBySingleTableContent.GetCreateListFileForCsContent(
                    pTableName, pProjectName, lstQueryField, projectNamespace, className, solutionName, IsReport);

                File.AppendAllText(fileName, content.ToString(), Encoding.UTF8);
            }
            return errorMsg;
        }
        #endregion

        #region 列表之aspx
        /// <summary>
        /// 列表之aspx
        /// </summary>
        /// <returns></returns>
        private string CreateListFileForAspx(string pTableName, string pProjectName, List<object> lstQueryField, DataTable pFieldTable)
        {
            string errorMsg = "";

            string className = CamelName.getBigCamelName(pTableName);

            string thePath = hdnSolutionPath.Value + @"\\" + hdnRunProjectName.Value + @"\\" + hdnProjectRootFolder.Value + @"\\" + pProjectName;
            if (Directory.Exists(thePath) == false)
            {
                Directory.CreateDirectory(thePath);
            }

            string fileName = thePath + @"\\" + className + ".aspx";

            //文件不存在才创建
            if (File.Exists(fileName) == false)
            {
                string projectNamespaceByPrefix = hdnRunProjectName.Value + "." + hdnProjectRootFolder.Value + "." + pProjectName;

                bool IsReport = chkReportExcel.Checked;

                string content = GenerateBySingleTableContent.CreateListFileForAspx(
                    pTableName, lstQueryField, projectNamespaceByPrefix, className, pFieldTable, IsReport);

                File.AppendAllText(fileName, content, Encoding.UTF8);
            }
            return errorMsg;
        }
        #endregion

        #endregion

        #region 创建编辑文件

        #region 创建编辑文件主体
        /// <summary>
        /// 创建编辑文件主体
        /// </summary>
        /// <returns></returns>
        private string CreateEditFile(string pTableName, string pProjectName, List<object> lstQueryField, DataTable pFieldTable)
        {
            string errorMsg = "";

            string designerMsg = CreateEditFileForDesigner(pTableName, pProjectName, lstQueryField, pFieldTable);
            if (string.IsNullOrEmpty(designerMsg) == false)
            {
                return designerMsg;
            }

            string csMsg = CreateEditFileForCs(pTableName, pProjectName, lstQueryField, pFieldTable);
            if (string.IsNullOrEmpty(csMsg) == false)
            {
                return csMsg;
            }

            string aspxMsg = CreateEditFileForAspx(pTableName, pProjectName, lstQueryField, pFieldTable);
            if (string.IsNullOrEmpty(aspxMsg) == false)
            {
                return aspxMsg;
            }

            return errorMsg;
        }
        #endregion

        #region 编辑之设计
        /// <summary>
        /// 编辑之设计
        /// </summary>
        /// <returns></returns>
        private string CreateEditFileForDesigner(string pTableName, string pProjectName, List<object> lstQueryField, DataTable pFieldTable)
        {
            string errorMsg = "";

            string className = CamelName.getBigCamelName(pTableName);

            string thePath = hdnSolutionPath.Value + @"\\" + hdnRunProjectName.Value + @"\\" + hdnProjectRootFolder.Value + @"\\" + pProjectName;
            if (Directory.Exists(thePath) == false)
            {
                Directory.CreateDirectory(thePath);
            }

            string fileName = thePath + @"\\" + className + "Edit.aspx.designer.cs";

            //文件不存在才创建
            if (File.Exists(fileName) == false)
            {
                string projectNamespace = hdnRunProjectName.Value + "." + hdnProjectRootFolder.Value + "." + pProjectName;

                string content = GenerateBySingleTableContent.GetCreateEditFileForDesignerContent(
                   pTableName, lstQueryField, projectNamespace, className, pFieldTable);

                File.AppendAllText(fileName, content, Encoding.UTF8);
            }

            return errorMsg;
        }
        #endregion

        #region 编辑之cs
        /// <summary>
        /// 编辑之cs
        /// </summary>
        /// <returns></returns>
        private string CreateEditFileForCs(string pTableName, string pProjectName, List<object> lstQueryField, DataTable pFieldTable)
        {
            string errorMsg = "";

            string className = CamelName.getBigCamelName(pTableName);

            string thePath = hdnSolutionPath.Value + @"\\" + hdnRunProjectName.Value + @"\\" + hdnProjectRootFolder.Value + @"\\" + pProjectName;
            if (Directory.Exists(thePath) == false)
            {
                Directory.CreateDirectory(thePath);
            }

            string fileName = thePath + @"\\" + className + "Edit.aspx.cs";

            //文件不存在才创建
            if (File.Exists(fileName) == false)
            {

                string projectNamespaceByPrefix = hdnRunProjectName.Value + "." + hdnProjectRootFolder.Value;

                string solutionName = hdnSolutionName.Value;

                string content = GenerateBySingleTableContent.GetCreateEditFileForCsContent(
                   pTableName, pProjectName, lstQueryField, projectNamespaceByPrefix, className, pFieldTable, solutionName);

                File.AppendAllText(fileName, content.ToString(), Encoding.UTF8);
            }
            return errorMsg;
        }
        #endregion

        #region 编辑之aspx
        /// <summary>
        /// 编辑之aspx
        /// </summary>
        /// <returns></returns>
        private string CreateEditFileForAspx(string pTableName, string pProjectName, List<object> lstQueryField, DataTable pFieldTable)
        {
            string errorMsg = "";

            string className = CamelName.getBigCamelName(pTableName);

            string thePath = hdnSolutionPath.Value + @"\\" + hdnRunProjectName.Value + @"\\" + hdnProjectRootFolder.Value + @"\\" + pProjectName;
            if (Directory.Exists(thePath) == false)
            {
                Directory.CreateDirectory(thePath);
            }

            string fileName = thePath + @"\\" + className + "Edit.aspx";

            //文件不存在才创建
            if (File.Exists(fileName) == false)
            {
                string projectNamespace = hdnRunProjectName.Value + "." + hdnProjectRootFolder.Value + "." + pProjectName;

                string content = GenerateBySingleTableContent.CreateEditFileForAspx(
                    pTableName, lstQueryField, projectNamespace, className, pFieldTable);

                File.AppendAllText(fileName, content, Encoding.UTF8);
            }
            return errorMsg;
        }
        #endregion

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

            Session["FieldTable"] = dt;

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