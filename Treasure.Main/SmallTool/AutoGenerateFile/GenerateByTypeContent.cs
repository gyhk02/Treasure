using System.Collections.Generic;
using System.Data;
using System.Text;
using Treasure.Bll.General;
using Treasure.Model.General;
using Treasure.Model.SmallTool.DataSynchron;
using Treasure.Utility.Utilitys;

namespace Treasure.Main.SmallTool.AutoGenerateFile
{
    public static class GenerateByTypeContent
    {

        #region Model之Parent
        /// <summary>
        /// Model之Parent
        /// <param name="pTableName"></param>
        /// <param name="pProjectName"></param>
        /// <param name="pProjectNamespaceByPrefix"></param>
        /// <param name="pClassName"></param>
        /// <param name="fieldTable"></param>
        /// <returns></returns>
        public static string GetCreateModelFileForParentContent(
               string pTableName, string projectNamespace, string pClassName, DataTable fieldTable)
        {
            string result = "";

            #region 字段Html

            StringBuilder fieldHtml = new StringBuilder();
            foreach (DataRow row in fieldTable.Rows)
            {
                fieldHtml.Append("            public readonly static string "
                    + CamelName.getSmallCamelName(TypeConversion.ToString(row[DataSynchronVO.FieldName])) + " = \""
                    + TypeConversion.ToString(row[DataSynchronVO.FieldName]) + "\"; " + ConstantVO.ENTER_R);
            }

            #endregion

            #region 内容

            result = @"
namespace " + projectNamespace + @"
{
    public partial class " + pClassName + @"Table
    {
        public readonly static string tableName = """ + pTableName + @"""; 

        public static class Fields
        {
" + fieldHtml.ToString() + @"
        }
    }
}";

            #endregion

            return result;
        }
        #endregion

        #region Model之Edit
        /// <summary>
        /// Model之Edit
        /// </summary>
        /// <param name="pProjectName"></param>
        /// <param name="pProjectNamespaceByPrefix"></param>
        /// <param name="pClassName"></param>
        /// <returns></returns>
        public static string GetCreateModelFileForEditContent(string pProjectNamespace, string pClassName)
        {
            string result = "";

            #region 内容

            //string pProjectNamespaceByPrefix

            result = @"
namespace " + pProjectNamespace + @"
{
    public partial class " + pClassName + @"Table
    {
    }
}";

            #endregion

            return result;
        }
        #endregion

        #region BLL文件
        /// <summary>
        /// BLL文件
        /// </summary>
        /// <returns></returns>
        public static string GetCreateBllFileContent(
            string pTableName, List<object> lstQueryField, string pProjectNamespace, string pClassName
            , string pSolutionName, DataTable pDtAll, List<string> pExcludedFiedList)
        {
            string result = "";

            #region 方法中的HTML

            //条件HTML
            StringBuilder sqlHtml = new StringBuilder();

            //参数HTML
            StringBuilder paraHtml = new StringBuilder();

            //sql后面的join
            StringBuilder joinHtml = new StringBuilder();

            sqlHtml.Append("            string sql = \"SELECT");
            int idx = 1;
            foreach (DataRow row in pDtAll.Rows)
            {
                if (pExcludedFiedList.Contains(TypeConversion.ToString(row[DataSynchronVO.FieldName])) == true)
                {
                    continue;
                }

                string fieldName = TypeConversion.ToString(row[DataSynchronVO.FieldName]);
                string fieldType = TypeConversion.ToString(row[DataSynchronVO.FieldType]);
                Dictionary<string, string> pDic = new Dictionary<string, string>();
                pDic.Add("FieldName", fieldName);
                pDic.Add("ForeignTableName", TypeConversion.ToString(row[DataSynchronVO.ForeignTableName]));
                pDic.Add("ForeignFieldName", TypeConversion.ToString(row[DataSynchronVO.ForeignFieldName]));
                pDic.Add("Idx", idx.ToString());

                sqlHtml.Append(GenerateByTypeForDataType.GetString(fieldType, "GetCreateBllFileContent_SqlStr", pDic));
                joinHtml.Append(GenerateByTypeForDataType.GetString(fieldType, "GetCreateBllFileContent_JoinStr", pDic));
                idx++;
            }
            sqlHtml.Append("  A.* FROM " + pTableName + " A" + joinHtml.ToString() + "  WHERE 1 = 1\";");

            foreach (object[] arrQueryField in lstQueryField)
            {
                string fieldName = TypeConversion.ToString(arrQueryField[0]);
                string fieldType = TypeConversion.ToString(arrQueryField[1]);
                string foreignTableName = TypeConversion.ToString(arrQueryField[3]);

                Dictionary<string, string> pDic = new Dictionary<string, string>();
                pDic.Add("FieldName", fieldName);
                pDic.Add("ForeignTableName", foreignTableName);

                sqlHtml.Append(GenerateByTypeForDataType.GetString(fieldType, "GetCreateBllFileContent_Where", pDic));

                paraHtml.Append("            "
                    + GenerateByTypeForDataType.GetString(fieldType, "GetCreateBllFileContent_Para", pDic)
                    + ConstantVO.ENTER_R);
            }

            #endregion

            #region 内容

            result = @"
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using " + pSolutionName + @".Bll.General;
using " + pSolutionName + @".Utility.Utilitys;
using " + pSolutionName + @".Model.General;
using " + pSolutionName + @".Utility.Utilitys;

namespace " + pProjectNamespace + @"
{
    public class " + pClassName + @"Bll : BasicBll
    {

        #region 查询
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public DataTable Query(Dictionary<string, object> dicPara)
        {
            DataTable dt = null;

" + sqlHtml.ToString() + @"

            List<SqlParameter> lstPara = new List<SqlParameter>();
" + paraHtml.ToString() + @"

            dt = base.GetDataTable(sql, lstPara.ToArray());

            return dt;
        }
        #endregion

    }
}
";

            #endregion

            return result;
        }
        #endregion

        #region 列表之设计的文件内容
        /// <summary>
        /// 列表之设计的文件内容
        /// </summary>
        /// <returns></returns>
        public static string GetCreateListFileForDesignerContent(
            string pTableName, List<object> lstQueryField, string pProjectNamespace, string pClassName)
        {
            string result = "";

            #region 查询的HTML

            StringBuilder queryHtml = new StringBuilder();
            foreach (object[] arrQueryField in lstQueryField)
            {
                string fieldName = TypeConversion.ToString(arrQueryField[0]);
                string fieldType = TypeConversion.ToString(arrQueryField[1]);
                string foreignTableName = TypeConversion.ToString(arrQueryField[3]);

                Dictionary<string, string> pDic = new Dictionary<string, string>();
                pDic.Add("FieldName", fieldName);
                pDic.Add("ForeignTableName", foreignTableName);
                queryHtml.Append("        "
                    + GenerateByTypeForDataType.GetString(fieldType, "GetCreateListFileForDesignerContent", pDic)
                    + ConstantVO.ENTER_R);
            }
            #endregion

            #region 内容

            result = @"
//------------------------------------------------------------------------------
// <自动生成>
//     此代码由工具生成。
//
//     对此文件的更改可能导致不正确的行为，如果
//     重新生成代码，则所做更改将丢失。
// </自动生成>
//------------------------------------------------------------------------------

namespace " + pProjectNamespace + @" {
    
    
    public partial class " + pClassName + @" {
        
        /// <summary>
        /// form1 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlForm form1;
        
" + queryHtml.ToString() + @"
        
        /// <summary>
        /// grdData 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::DevExpress.Web.ASPxGridView grdData;
        
        /// <summary>
        /// ASPxGridViewExporter1 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::DevExpress.Web.ASPxGridViewExporter ASPxGridViewExporter1;
        
        /// <summary>
        /// hidCondition 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.WebControls.HiddenField hidCondition;
    }
}
";
            #endregion

            return result;
        }
        #endregion

        #region 列表之cs的文件内容
        /// <summary>
        /// 列表之cs的文件内容
        /// </summary>
        /// <returns></returns>
        public static string GetCreateListFileForCsContent(
            string pTableName, string pProjectName, List<object> lstQueryField, string pProjectNamespace
            , string pClassName, string pSolutionName, bool pIsReport)
        {
            string result = "";

            #region PageLoad的HTML

            string PageLoadHtml = "";
            if (pIsReport == true)
            {
                PageLoadHtml = @"
                if (Request[""btnExportExcel""] == ""导出Excel"")
                {
                    ExportExcel();
                    return;
                }";
            }

            #endregion

            #region ExportExcel的Html

            string ExportExcelHtml = "";

            if (pIsReport == true)
            {
                ExportExcelHtml = @"
        #region 导出Excel
        /// <summary>
        /// 导出Excel
        /// </summary>
        private void ExportExcel()
        {
            InitData();

            DataTable dt = grdData.DataSource as DataTable;
            if (dt.Rows.Count == 0)
            {
                ClientScriptManager clientScript = Page.ClientScript;
                clientScript.RegisterStartupScript(this.GetType(), """", ""<script type=text/javascript>alert('没有数据');</script>"");
                return;
            }

            ASPxGridViewExporter1.WriteXlsToResponse();
        }
        #endregion";
            }

            #endregion

            #region 查询相关的HTML

            StringBuilder PageLoadMothedHtml = new StringBuilder();
            StringBuilder PageLoadIsPostBackHtml = new StringBuilder();
            StringBuilder MothedHtml = new StringBuilder();

            StringBuilder InitDataHtml = new StringBuilder();
            StringBuilder QueryHtml = new StringBuilder();

            foreach (object[] arrQueryField in lstQueryField)
            {
                string fieldName = TypeConversion.ToString(arrQueryField[0]);
                string fieldType = TypeConversion.ToString(arrQueryField[1]);
                string fieldDescription = TypeConversion.ToString(arrQueryField[2]);
                string foreignTableName = TypeConversion.ToString(arrQueryField[3]);

                Dictionary<string, string> pDic = new Dictionary<string, string>();
                pDic.Add("FieldName", fieldName);
                pDic.Add("FieldDescription", fieldDescription);
                pDic.Add("ForeignTableName", foreignTableName);

                InitDataHtml.Append("            "
                    + GenerateByTypeForDataType.GetString(fieldType, "GetCreateListFileForCsContent_InitData", pDic)
                    + ConstantVO.ENTER_R);

                QueryHtml.Append(GenerateByTypeForDataType.GetString(fieldType, "GetCreateListFileForCsContent_Query", pDic));

                PageLoadMothedHtml.Append(GenerateByTypeForDataType.GetString(fieldType, "GetCreateListFileForCsContent_PageLoadBoolMothed", pDic));
                PageLoadIsPostBackHtml.Append("                "
                    + GenerateByTypeForDataType.GetString(fieldType, "GetCreateListFileForCsContent_PageLoadIsPostBack", pDic)
                    + ConstantVO.ENTER_R);
                MothedHtml.Append(GenerateByTypeForDataType.GetString(fieldType, "GetCreateListFileForCsContent_BoolMothed", pDic));
            }

            #endregion

            #region 内容

            result = @"
using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using " + pSolutionName + @".Bll.General;
using " + pSolutionName + @".Bll.ProjectCollection." + pProjectName + @";
using " + pSolutionName + @".Model.General;
using " + pSolutionName + @".Model.ProjectCollection." + pProjectName + @";
using " + pSolutionName + @".Utility.Extend;
using " + pSolutionName + @".Utility.Utilitys;

namespace " + pProjectNamespace + @"
{
    public partial class " + pClassName + @" : System.Web.UI.Page
    {
        #region 自定义变量

        " + pClassName + @"Bll bll = new " + pClassName + @"Bll();
        GeneralBll bllGeneral = new GeneralBll();

        #endregion

        #region 系统事件

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod == ""POST"")
            {
                if (Request[""btnQuery""] == ""查询"")
                {
                    Query();
                    return;
                }
                if (Request[""btnAdd""] == ""新增"")
                {
                    Add();
                    return;
                }
" + PageLoadHtml + @"
" + PageLoadMothedHtml.ToString() + @"
                if (Request[""__CALLBACKID""] == ""grdData"")
                {
                    InitData();
                    return;
                }
            }

            if (IsPostBack==false)
            {
" + PageLoadIsPostBackHtml.ToString() + @"
                InitData();
            }
        }
        #endregion

        #region 按钮

" + ExportExcelHtml + @"

        #region 查询
        /// <summary>
        /// 查询
        /// </summary>
        private void Query()
        {
" + QueryHtml.ToString() + @"

            InitData();
        }
        #endregion

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        private void Add()
        {
            Response.Redirect(""" + pClassName + @"Edit.aspx"");
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name=""sender""></param>
        /// <param name=""e""></param>
        protected void grdData_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ClientScriptManager clientScript = Page.ClientScript;

            string id = TypeConversion.ToString(e.Keys[GeneralVO.id]);
            if (bll.DeleteById(" + pClassName + @"Table.tableName, id) == false)
            {
                clientScript.RegisterStartupScript(this.GetType(), """", ""<script type=text/javascript>alert('删除失败');</script>"");
            }

            clientScript.RegisterStartupScript(this.GetType(), """", ""<script type=text/javascript>alert('删除成功');</script>"");

            //要退出编辑模式才能重新绑定数据
            grdData.CancelEdit();
            e.Cancel = true;

            InitData();
        }
        #endregion

        #endregion

        #region 自定义事件
" + MothedHtml.ToString() + @"
        #region 初始化列表
        /// <summary>
        /// 初始化列表
        /// </summary>
        private void InitData()
        {
            Dictionary<string, object> dicPara = new Dictionary<string, object>();

" + InitDataHtml.ToString() + @"
            DataTable dt = bll.Query(dicPara);
            grdData.DataSource = dt;
            grdData.DataBind();
        }
        #endregion

        #endregion
    }
}";

            #endregion

            return result;
        }
        #endregion

        #region 列表之aspx的文件内容
        /// <summary>
        /// 列表之aspx的文件内容
        /// </summary>
        /// <returns></returns>
        public static string CreateListFileForAspx(
            string pTableName, List<object> lstQueryField, string pProjectNamespaceByPrefix, string pClassName
            , DataTable pFieldTable, bool pIsReport, List<string> pExcludedFiedList)
        {
            string result = "";

            string strReport = "";
            if (pIsReport == true)
            {
                strReport = "                    <td><input id=\"btnExportExcel\" name=\"btnExportExcel\" type=\"submit\" value=\"导出Excel\" /></td>";
            }

            #region 查询HTML

            StringBuilder queryHtml = new StringBuilder();

            foreach (object[] arrQueryField in lstQueryField)
            {
                string fieldName = TypeConversion.ToString(arrQueryField[0]);
                string fieldType = TypeConversion.ToString(arrQueryField[1]);
                string fieldDescription = TypeConversion.ToString(arrQueryField[2]);
                string foreignTableName = TypeConversion.ToString(arrQueryField[3]);

                Dictionary<string, string> pDic = new Dictionary<string, string>();
                pDic.Add("FieldName", fieldName);
                pDic.Add("FieldDescription", fieldDescription);
                pDic.Add("ForeignTableName", foreignTableName);
                queryHtml.Append("                        "
                    + GenerateByTypeForDataType.GetString(fieldType, "CreateListFileForAspx_Query", pDic)
                    + ConstantVO.ENTER_R);
            }

            if (string.IsNullOrEmpty(queryHtml.ToString()) == false)
            {
                queryHtml.Append("                    <td>" + ConstantVO.ENTER_R);
                queryHtml.Append("                        <input id = \"btnQuery\" name = \"btnQuery\" type = \"submit\" value = \"查询\" /></td> " + ConstantVO.ENTER_R);
                queryHtml.Append("                    <td> &nbsp; &nbsp; &nbsp;</td> " + ConstantVO.ENTER_R);
            }

            #endregion

            #region Grid列的HTML

            StringBuilder gridColumnsHtml = new StringBuilder();

            GeneralBll bllGeneral = new GeneralBll();

            int idx = 0;
            foreach (DataRow row in pFieldTable.Rows)
            {
                string fieldName = TypeConversion.ToString(row[DataSynchronVO.FieldName]);

                //默认要排序的字段
                if (pExcludedFiedList.Contains(fieldName) == true)
                {
                    continue;
                }

                Dictionary<string, string> pDic = new Dictionary<string, string>();
                pDic.Add("Caption", TypeConversion.ToString(row[DataSynchronVO.FieldDescription]));
                pDic.Add("FieldName", fieldName);
                pDic.Add("Idx", idx.ToString());
                pDic.Add("ForeignTableName", TypeConversion.ToString(row[DataSynchronVO.ForeignTableName]));
                gridColumnsHtml.Append("                    "
                    + GenerateByTypeForDataType.GetString(TypeConversion.ToString(row[DataSynchronVO.FieldType]), "CreateListFileForAspx_Grid", pDic)
                    + ConstantVO.ENTER_R
                    );
            }

            #endregion

            #region 内容

            result = @"
<%@ Page Language=""C#"" AutoEventWireup=""true"" CodeBehind=""" + pClassName + @".aspx.cs"" Inherits=""" + pProjectNamespaceByPrefix + "." + pClassName + @""" %>

<%@ Register Assembly=""DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"" Namespace=""DevExpress.Web"" TagPrefix=""dx"" %>

<!DOCTYPE html>

<html xmlns=""http://www.w3.org/1999/xhtml"">
<head runat=""server"">
    <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
    <title></title>
</head>
<body>
    <form id=""form1"" runat=""server"">
        <div>
            <table >
                <tr>
" + queryHtml.ToString() + @"
                    <td><input id=""btnAdd"" name=""btnAdd"" type=""submit"" value=""新增"" /></td>
" + strReport + @"
                    <td>&nbsp;</td>
                </tr>
            </table>
            <br />
            <dx:ASPxGridView ID=""grdData"" runat=""server"" AutoGenerateColumns=""False"" style=""margin-right: 0px""
                 KeyFieldName=""ID"" OnRowDeleting=""grdData_RowDeleting"">
                <SettingsPager PageSize=""100"">
                </SettingsPager>
                <SettingsBehavior ConfirmDelete=""True"" />
                <Columns>
" + gridColumnsHtml.ToString() + @"
                    <dx:GridViewDataHyperLinkColumn Caption=""修改"" VisibleIndex=""2"" FieldName=""ID"">
                        <PropertiesHyperLinkEdit NavigateUrlFormatString=""" + pClassName + @"Edit.aspx?ID={0}"" Text=""修改"">
                        </PropertiesHyperLinkEdit>
                    </dx:GridViewDataHyperLinkColumn>
                    <dx:GridViewCommandColumn Caption=""删除""
                        ShowDeleteButton=""True"" VisibleIndex=""3"">
                    </dx:GridViewCommandColumn>
                </Columns>
            </dx:ASPxGridView>
            <dx:ASPxGridViewExporter ID=""ASPxGridViewExporter1"" runat=""server"" GridViewID=""grdData"">
            </dx:ASPxGridViewExporter>
            <asp:HiddenField ID=""hidCondition"" runat=""server"" />
        </div>
    </form>
</body>
</html>
";

            #endregion

            return result;
        }
        #endregion

        #region 编辑之设计的文件内容
        /// <summary>
        /// 编辑之设计的文件内容
        /// </summary>
        /// <returns></returns>
        public static string GetCreateEditFileForDesignerContent(
            string pTableName, List<object> lstQueryField, string pProjectNamespace, string pClassName
            , DataTable pFieldTable)
        {
            string result = "";

            #region 字段对应的HTML

            StringBuilder fieldHtml = new StringBuilder();

            GeneralBll bllGeneral = new GeneralBll();
            List<string> lstExcludedField = bllGeneral.GetExcludedFields();

            foreach (DataRow row in pFieldTable.Rows)
            {
                string fieldName = TypeConversion.ToString(row[DataSynchronVO.FieldName]);

                //默认要排序的字段
                if (lstExcludedField.Contains(fieldName) == true)
                {
                    continue;
                }

                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("FieldName", fieldName);
                dic.Add("ForeignTableName", TypeConversion.ToString(row[DataSynchronVO.ForeignTableName]));
                fieldHtml.Append("        "
                    + GenerateByTypeForDataType.GetString(TypeConversion.ToString(row[DataSynchronVO.FieldType]), "GetCreateEditFileForDesignerContent", dic)
                    + ConstantVO.ENTER_R);

            }

            #endregion

            #region 内容

            result = @"
//------------------------------------------------------------------------------
// <自动生成>
//     此代码由工具生成。
//
//     对此文件的更改可能导致不正确的行为，如果
//     重新生成代码，则所做更改将丢失。
// </自动生成>
//------------------------------------------------------------------------------

namespace " + pProjectNamespace + @" {    
    

    public partial class " + pClassName + @"Edit {
        
        /// <summary>
        /// form1 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlForm form1;
        
" + fieldHtml.ToString() + @"
        
        /// <summary>
        /// hdnID 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.WebControls.HiddenField hdnID;
    }
}
";
            #endregion

            return result;
        }
        #endregion

        #region 编辑之cs的文件内容
        /// <summary>
        /// 编辑之cs的文件内容
        /// </summary>
        /// <returns></returns>
        public static string GetCreateEditFileForCsContent(
            string pTableName, string pProjectName, List<object> lstQueryField, string pProjectNamespaceByPrefix, string pClassName
            , DataTable pFieldTable, string pSolutionName)
        {
            string result = "";

            #region 方法中的HTML

            //InitData方法中的HTML
            StringBuilder InitDataHtml = new StringBuilder();

            //Add方法中的HTML
            StringBuilder AddHtml = new StringBuilder();

            //Edit方法中的HTML
            StringBuilder EditHtml = new StringBuilder();

            StringBuilder PageLoadMothedHtml = new StringBuilder();
            StringBuilder PageLoadIsPostBackHtml = new StringBuilder();
            StringBuilder MothedHtml = new StringBuilder();

            GeneralBll bllGeneral = new GeneralBll();
            List<string> lstExcludedField = bllGeneral.GetExcludedFields();

            foreach (DataRow row in pFieldTable.Rows)
            {
                string fieldName = TypeConversion.ToString(row[DataSynchronVO.FieldName]);

                //默认要排序的字段
                if (lstExcludedField.Contains(fieldName) == true)
                {
                    continue;
                }

                string fieldType = TypeConversion.ToString(row[DataSynchronVO.FieldType]);

                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("ClassName", pClassName);
                dic.Add("FieldName", fieldName);
                dic.Add("FieldDescription", TypeConversion.ToString(row[DataSynchronVO.FieldDescription]));
                dic.Add("ForeignTableName", TypeConversion.ToString(row[DataSynchronVO.ForeignTableName]));
                InitDataHtml.Append("                    "
                    + GenerateByTypeForDataType.GetString(fieldType, "GetCreateEditFileForCsContent_Init", dic)
                    + ConstantVO.ENTER_R);
                AddHtml.Append("                    "
                    + GenerateByTypeForDataType.GetString(fieldType, "GetCreateEditFileForCsContent_Add", dic)
                    + ConstantVO.ENTER_R);
                EditHtml.Append("                    "
                    + GenerateByTypeForDataType.GetString(fieldType, "GetCreateEditFileForCsContent_Edit", dic)
                    + ConstantVO.ENTER_R);
                PageLoadMothedHtml.Append(GenerateByTypeForDataType.GetString(fieldType, "GetCreateEditFileForCsContent_PageLoadMothed", dic));
                PageLoadIsPostBackHtml.Append("                "
                    + GenerateByTypeForDataType.GetString(fieldType, "GetCreateEditFileForCsContent_PageLoadIsPostBack", dic)
                    + ConstantVO.ENTER_R);
                MothedHtml.Append(GenerateByTypeForDataType.GetString(fieldType, "GetCreateEditFileForCsContent_Mothed", dic));
            }

            #endregion

            #region 内容

            result = @"
using System;
using System.Data;
using System.Web.UI;
using " + pSolutionName + @".Utility.Extend;
using " + pSolutionName + @".Bll.ProjectCollection." + pProjectName + @";
using " + pSolutionName + @".Model.ProjectCollection." + pProjectName + @";
using " + pSolutionName + @".Utility.Utilitys;

namespace " + pProjectNamespaceByPrefix + @"." + pProjectName + @"
{
    public partial class " + pClassName + @"Edit : System.Web.UI.Page
    {

        #region 自定义变量

        " + pClassName + @"Bll bll = new " + pClassName + @"Bll();
        DateTime today = DateTime.Now;

        #endregion

        #region 系统事件

        protected void Page_Load(object sender, EventArgs e)
        {
            //按钮
            if (Request.HttpMethod == ""POST"")
            {
                if (Request[""btnAdd""] == ""新增"")
                {
                    Add();
                }
                if (Request[""btnEdit""] == ""修改"")
                {
                    Edit();
                }
                if (Request[""btnBack""] == ""返回"")
                {
                    Back();
                }
" + PageLoadMothedHtml.ToString() + @"
            }

            if (IsPostBack == false)
            {
                //接收参数
                hdnID.Value = Request[""ID""];

                //显示隐藏新增或修改按钮
                ClientScriptManager clientScript = Page.ClientScript;
                if (hdnID.Value != null && string.IsNullOrEmpty(hdnID.Value) == false)
                {
                    clientScript.RegisterStartupScript(this.GetType(), """", ""<script type=text/javascript>ShowAddOrEdit('btnEdit');</script>"");
                }
                else
                {
                    clientScript.RegisterStartupScript(this.GetType(), """", ""<script type=text/javascript>ShowAddOrEdit('btnAdd');</script>"");
                }

" + PageLoadIsPostBackHtml.ToString() + @"
                InitData();
            }
        }

        #endregion

        #region 按钮

" + MothedHtml.ToString() + @"

        #region 返回
        /// <summary>
        /// 返回
        /// </summary>
        private void Back()
        {
            Response.Redirect(""" + pClassName + @".aspx"");
        }
        #endregion

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        private void Add()
        {
            DataTable dt = bll.GetDataTableStructure(" + pClassName + @"Table.tableName);
            DataRow row = dt.NewRow();

            row[" + pClassName + @"Table.Fields.id] = Guid.NewGuid().ToString().Replace(""-"", """");
" + AddHtml.ToString() + @"

            row[" + pClassName + @"Table.Fields.createDatetime] = today;
            row[" + pClassName + @"Table.Fields.modifyDatetime] = today;

            bll.AddDataRow(row);

            Response.Redirect(""" + pClassName + @".aspx"");
        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改
        /// </summary>
        private void Edit()
        {
            DataRow row = bll.GetDataRowById(" + pClassName + @"Table.tableName, hdnID.Value);

" + EditHtml.ToString() + @"
            row[" + pClassName + @"Table.Fields.modifyDatetime] = today;

            bll.UpdateDataRow(row);

            Response.Redirect(""" + pClassName + @".aspx"");
        }
        #endregion

        #endregion

        #region 自定义事件

        #region 初始化数据
        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData()
        {
            string id = hdnID.Value;

            if (string.IsNullOrEmpty(id) == false)
            {
                DataRow row = bll.GetDataRowById(" + pClassName + @"Table.tableName, id);
                if (row != null)
                {
" + InitDataHtml.ToString() + @"
                }
            }
        }
        #endregion

        #endregion
    }
}";

            #endregion

            return result;
        }
        #endregion

        #region 编辑之aspx的文件内容
        /// <summary>
        /// 编辑之aspx的文件内容
        /// </summary>
        /// <returns></returns>
        public static string CreateEditFileForAspx(
            string pTableName, List<object> lstQueryField, string pProjectNamespace, string pClassName, DataTable pFieldTable)
        {
            string result = "";

            #region 要编辑栏位的HTML

            StringBuilder editHtml = new StringBuilder();

            GeneralBll bllGeneral = new GeneralBll();
            List<string> lstExcludedField = bllGeneral.GetExcludedFields();

            foreach (DataRow row in pFieldTable.Rows)
            {
                string fieldName = TypeConversion.ToString(row[DataSynchronVO.FieldName]);

                //默认要排序的字段
                if (lstExcludedField.Contains(fieldName) == true)
                {
                    continue;
                }

                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("FieldDescription", TypeConversion.ToString(row[DataSynchronVO.FieldDescription]));
                dic.Add("FieldName", fieldName);
                dic.Add("ForeignTableName", TypeConversion.ToString(row[DataSynchronVO.ForeignTableName]));
                editHtml.Append(GenerateByTypeForDataType.GetString(TypeConversion.ToString(row[DataSynchronVO.FieldType]), "CreateEditFileForAspx", dic)
                    + ConstantVO.ENTER_R);
            }

            #endregion

            #region 内容

            result = @"
<%@ Page Language=""C#"" AutoEventWireup=""true"" CodeBehind=""" + pClassName + @"Edit.aspx.cs"" Inherits=""" + pProjectNamespace + @"." + pClassName + @"Edit"" %>

<%@ Register assembly=""DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"" namespace=""DevExpress.Web"" tagprefix=""dx"" %>

<!DOCTYPE html>

<html xmlns=""http://www.w3.org/1999/xhtml"">
<head runat=""server"">
    <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
    <title></title>
    <script src=""../../Script/Js/jquery.min.js""></script>
    <script type=""text/javascript"">
        function ShowAddOrEdit(buttonName) {
            if (buttonName == ""btnAdd"") {
                $(""#btnAdd"").show();
                $(""#btnEdit"").hide();
            }
            else {
                $(""#btnAdd"").hide();
                $(""#btnEdit"").show();
            }
        }
    </script>
</head>
<body>
    <form id=""form1"" runat=""server"">
        <div>
            <table>
" + editHtml.ToString() + @"
            </table>
            <table >
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <input id=""btnAdd"" name=""btnAdd"" type=""submit"" value=""新增"" />
                        <input id=""btnEdit"" name=""btnEdit"" type=""submit"" value=""修改"" /></td>
                    <td>
                        <input id=""btnBack"" name=""btnBack"" type=""submit"" value=""返回"" /></td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID=""hdnID"" runat=""server"" />
    </form>
</body>
</html>
";

            #endregion

            return result;
        }
        #endregion

    }
}