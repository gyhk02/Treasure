using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI;
using Treasure.Bll.Frame;
using Treasure.Bll.General;
using Treasure.BLL.Frame;
using Treasure.Model.Frame;
using Treasure.Utility.Helpers;
using Treasure.Utility.Utilitys;

namespace Treasure.Main.SmallTool.AutoGenerateReport
{
    public partial class GenerateReportEdit : System.Web.UI.Page
    {
        #region 自定义变量

        SysReportBll bll = new SysReportBll();
        SysMenuItemBll bllSysMenuItem = new SysMenuItemBll();
        DateTime today = DateTime.Now;

        #endregion

        #region 系统事件

        protected void Page_Load(object sender, EventArgs e)
        {
            //按钮
            if (Request.HttpMethod == "POST")
            {
                if (Request["btnNext"] == "转下一步")
                {
                    Next();
                    return;
                }
                if (Request["btnComplete"] == "完成")
                {
                    Complete();
                    return;
                }
                if (Request["btnBack"] == "返回")
                {
                    Back();
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

            if (IsPostBack == false)
            {
                //接收参数
                hdnID.Value = Request["ID"];

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

                //显示隐藏新增或修改按钮
                /**
                ClientScriptManager clientScript = Page.ClientScript;
                if (hdnID.Value != null && string.IsNullOrEmpty(hdnID.Value) == false)
                {
                    clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>ShowAddOrEdit('btnEdit');</script>");
                }
                else
                {
                    clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>ShowAddOrEdit('btnAdd');</script>");
                }
                */

                InitData();
            }
        }

        #endregion

        #region 按钮

        #region 返回
        /// <summary>
        /// 返回
        /// </summary>
        private void Back()
        {
            Response.Redirect("GenerateReport.aspx");
        }
        #endregion

        #region 转下一步
        /// <summary>
        /// 转下一步
        /// </summary>
        private void Next()
        {
            string sourceSql = txtSourceSQL.Text.Trim();
            string cnTitle = txtCnTitle.Text.Trim();
            string enTitle = txtEnTitle.Text.Trim();

            ClientScriptManager clientScript = Page.ClientScript;
            if (string.IsNullOrEmpty(sourceSql) == true)
            {
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('原SQL不能为空');</script>");
                return;
            }
            if (string.IsNullOrEmpty(cnTitle) == true)
            {
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('中文标题不能为空');</script>");
                return;
            }
            if (string.IsNullOrEmpty(enTitle) == true)
            {
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('英文标题不能为空');</script>");
                return;
            }

            hdnTableName.Value = enTitle;

            //写主表
            DataTable dtReport = bll.GetDataTableStructure(SysReportTable.tableName);
            DataRow rowReport = dtReport.NewRow();

            string id = Guid.NewGuid().ToString().Replace("-", "");
            rowReport[SysReportTable.Fields.id] = id;
            rowReport[SysReportTable.Fields.name] = txtCnTitle.Text.Trim();
            rowReport[SysReportTable.Fields.enName] = txtEnTitle.Text.Trim();
            rowReport[SysReportTable.Fields.sourceSql] = txtSourceSQL.Text.Trim();
            rowReport[SysReportTable.Fields.targetSql] = "";
            rowReport[SysReportTable.Fields.hasExportExcel] = false;
            rowReport[SysReportTable.Fields.hasPage] = false;
            rowReport[SysReportTable.Fields.createUserId] = BasicWebBll.SeUserID;
            rowReport[SysReportTable.Fields.createDatetime] = today;
            rowReport[SysReportTable.Fields.modifyUserId] = BasicWebBll.SeUserID;
            rowReport[SysReportTable.Fields.modifyDatetime] = today;

            bll.AddDataRow(rowReport);

            hdnReportId.Value = id;

            //根据SQL获取全部列
            DataTable dtReportCol = bll.GetDataTableStructure(SysReportColTable.tableName);
            DataTable dtSql = bll.GetDataTable(sourceSql, null);
            foreach (DataColumn col in dtSql.Columns)
            {
                DataRow rowReportCol = dtReportCol.NewRow();
                rowReportCol[SysReportColTable.Fields.name] = col.ColumnName;
                dtReportCol.Rows.Add(rowReportCol);
            }

            grdData.DataSource = dtReportCol;
            grdData.DataBind();

            //将值赋给文本框
            for (int idx = 0; idx < dtReportCol.Rows.Count; idx++)
            {
                DataRow row = dtReportCol.Rows[idx];

                ASPxTextBox txtNAME = grdData.FindRowCellTemplateControl(idx, (GridViewDataColumn)grdData.Columns["NAME"], "txtNAME") as ASPxTextBox;
                txtNAME.Text = TypeConversion.ToString(row[SysReportColTable.Fields.name]);

                ASPxTextBox txtCN_NAME = grdData.FindRowCellTemplateControl(idx, (GridViewDataColumn)grdData.Columns["CN_NAME"], "txtCN_NAME") as ASPxTextBox;
                txtCN_NAME.Text = TypeConversion.ToString(row[SysReportColTable.Fields.cnName]);

                ASPxTextBox txtCOL_DATA_TYPE = grdData.FindRowCellTemplateControl(idx, (GridViewDataColumn)grdData.Columns["COL_DATA_TYPE"], "txtCOL_DATA_TYPE") as ASPxTextBox;
                txtCOL_DATA_TYPE.Text = TypeConversion.ToString(row[SysReportColTable.Fields.colDataType]);

                ASPxTextBox txtIS_QUERY = grdData.FindRowCellTemplateControl(idx, (GridViewDataColumn)grdData.Columns["IS_QUERY"], "txtIS_QUERY") as ASPxTextBox;
                txtIS_QUERY.Text = TypeConversion.ToString(row[SysReportColTable.Fields.isQuery]);

                ASPxTextBox txtSORT_RULE = grdData.FindRowCellTemplateControl(idx, (GridViewDataColumn)grdData.Columns["SORT_RULE"], "txtSORT_RULE") as ASPxTextBox;
                txtSORT_RULE.Text = TypeConversion.ToString(row[SysReportColTable.Fields.sortRule]);

                ASPxTextBox txtSORT_INDEX = grdData.FindRowCellTemplateControl(idx, (GridViewDataColumn)grdData.Columns["SORT_INDEX"], "txtSORT_INDEX") as ASPxTextBox;
                txtSORT_INDEX.Text = TypeConversion.ToString(row[SysReportColTable.Fields.sortIndex]);

                ASPxTextBox txtDECIMAL_DIGITS = grdData.FindRowCellTemplateControl(idx, (GridViewDataColumn)grdData.Columns["DECIMAL_DIGITS"], "txtDECIMAL_DIGITS") as ASPxTextBox;
                txtDECIMAL_DIGITS.Text = TypeConversion.ToString(row[SysReportColTable.Fields.decimalDigits]);
            }
        }
        #endregion

        #region 完成
        /// <summary>
        /// 完成
        /// </summary>
        private void Complete()
        {
            int rowCount = grdData.VisibleRowCount;

            try
            {
                #region 将数据插入表

                DataTable dt = bll.GetDataTableStructure(SysReportColTable.tableName);
                for (int idx = 0; idx < rowCount; idx++)
                {
                    ASPxTextBox txtNAME = grdData.FindRowCellTemplateControl(idx, (GridViewDataColumn)grdData.Columns["NAME"], "txtNAME") as ASPxTextBox;
                    ASPxTextBox txtCN_NAME = grdData.FindRowCellTemplateControl(idx, (GridViewDataColumn)grdData.Columns["CN_NAME"], "txtCN_NAME") as ASPxTextBox;
                    ASPxTextBox txtCOL_DATA_TYPE = grdData.FindRowCellTemplateControl(idx, (GridViewDataColumn)grdData.Columns["COL_DATA_TYPE"], "txtCOL_DATA_TYPE") as ASPxTextBox;
                    ASPxTextBox txtIS_QUERY = grdData.FindRowCellTemplateControl(idx, (GridViewDataColumn)grdData.Columns["IS_QUERY"], "txtIS_QUERY") as ASPxTextBox;
                    ASPxTextBox txtSORT_RULE = grdData.FindRowCellTemplateControl(idx, (GridViewDataColumn)grdData.Columns["SORT_RULE"], "txtSORT_RULE") as ASPxTextBox;
                    ASPxTextBox txtSORT_INDEX = grdData.FindRowCellTemplateControl(idx, (GridViewDataColumn)grdData.Columns["SORT_INDEX"], "txtSORT_INDEX") as ASPxTextBox;
                    ASPxTextBox txtDECIMAL_DIGITS = grdData.FindRowCellTemplateControl(idx, (GridViewDataColumn)grdData.Columns["DECIMAL_DIGITS"], "txtDECIMAL_DIGITS") as ASPxTextBox;

                    DataRow rowCol = dt.NewRow();
                    rowCol[SysReportColTable.Fields.id] = Guid.NewGuid().ToString().Replace("-", "");
                    rowCol[SysReportColTable.Fields.name] = txtNAME.Text;
                    rowCol[SysReportColTable.Fields.cnName] = txtCN_NAME.Text ?? "";
                    rowCol[SysReportColTable.Fields.colDataType] = txtCOL_DATA_TYPE.Text ?? "NVARCHAR";
                    rowCol[SysReportColTable.Fields.isQuery] = txtIS_QUERY.Text ?? "0";
                    rowCol[SysReportColTable.Fields.sortRule] = txtSORT_RULE.Text ?? "ASC";
                    rowCol[SysReportColTable.Fields.sortIndex] = string.IsNullOrEmpty(txtSORT_INDEX.Text) == true ? 0 : TypeConversion.ToInt(txtSORT_INDEX.Text);
                    rowCol[SysReportColTable.Fields.decimalDigits] = string.IsNullOrEmpty(txtDECIMAL_DIGITS.Text) == true ? 0 : TypeConversion.ToInt(txtDECIMAL_DIGITS.Text);
                    rowCol[SysReportColTable.Fields.sysReportId] = hdnReportId.Value;
                    rowCol[SysReportColTable.Fields.createDatetime] = today;
                    rowCol[SysReportColTable.Fields.modifyDatetime] = today;

                    dt.Rows.Add(rowCol);
                }
                bll.AddDataTable(dt);

                #endregion
                /**
                #region 开始写文件

                DataRow rowReport = bll.GetDataRowById(SysReportTable.tableName, hdnReportId.Value);
                DataTable dtReportCol = bll.GetDataTable();

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

                #endregion
                */
                Response.Redirect("GenerateReport.aspx", false);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }
        }
        #endregion

        #endregion

        #region 自定义事件

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

        #region 创建文件

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

            string projectNamespace = hdnSolutionName.Value + ".Bll." + hdnProjectRootFolder.Value + "." + pProjectName;

            DataTable dtAll = Session["FieldTable"] as DataTable;
            /**
            string content = GenerateBySingleTableContent.GetCreateBllFileContent(
                pTableName, lstQueryField, projectNamespace, className, hdnSolutionName.Value, dtAll);

            File.Delete(fileName);
            File.AppendAllText(fileName, content.ToString(), Encoding.UTF8);
            /*
            return errorMsg;
        }
        #endregion

        #region 创建Model文件主体
        /// <summary>
        /// 创建Model文件主体
        /// </summary>
        /// <returns></returns>
        private string CreateModelFile(string pTableName, string pProjectName, DataTable pFieldTable)
        {
            string errorMsg = "";

            string editMsg = CreateModelFileToDo(pTableName, pProjectName, pFieldTable);
            if (string.IsNullOrEmpty(editMsg) == false)
            {
                return editMsg;
            }

            return errorMsg;
        }
        private string CreateModelFileToDo(string pTableName, string pProjectName, DataTable pFieldTable)
        {
            string errorMsg = "";

            string className = CamelName.getBigCamelName(pTableName);

            string thePath = hdnSolutionPath.Value + @"\\" + hdnSolutionName.Value + ".Model\\"
                + hdnProjectRootFolder.Value + @"\\" + pProjectName ;
            if (Directory.Exists(thePath) == false)
            {
                Directory.CreateDirectory(thePath);
            }
            string fileName = thePath + @"\\" + className + "Vo.cs";

            string projectNamespace = hdnSolutionName.Value + ".Model." + hdnProjectRootFolder.Value + "." + pProjectName;

            string content = GenerateReportContent.GetCreateModelFileContent(projectNamespace, className, pFieldTable);

            File.Delete(fileName);
            File.AppendAllText(fileName, content.ToString(), Encoding.UTF8);

            return errorMsg;
        }
        #endregion

        #endregion

        #region 初始化数据
        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData()
        {
            string id = hdnID.Value;

            if (string.IsNullOrEmpty(id) == false)
            {
                /**
                DataRow row = bll.GetDataRowById(SysReportTable.tableName, id);
                if (row != null)
                {
                    txtNO.Text = TypeConversion.ToString(row[SysReportTable.Fields.no]);                    txtNAME.Text = TypeConversion.ToString(row[SysReportTable.Fields.name]);                    txtSOURCE_SQL.Text = TypeConversion.ToString(row[SysReportTable.Fields.sourceSql]);                    txtTARGET_SQL.Text = TypeConversion.ToString(row[SysReportTable.Fields.targetSql]);
                }
                */
            }
        }
        #endregion

        #endregion
    }
}