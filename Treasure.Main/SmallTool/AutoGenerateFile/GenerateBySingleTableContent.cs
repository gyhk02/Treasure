using System.Collections.Generic;
using System.Data;
using System.Text;
using Treasure.Bll.General;
using Treasure.Model.General;
using Treasure.Model.SmallTool.DataSynchron;
using Treasure.Utility.Utilitys;

namespace Treasure.Main.SmallTool.AutoGenerateFile
{
    public static class GenerateBySingleTableContent
    {
        #region Model之Parent
        /// <summary>
        /// Model之Parent
        /// </summary>
        /// <param name="pTableName"></param>
        /// <param name="pProjectName"></param>
        /// <param name="pProjectNamespaceByPrefix"></param>
        /// <param name="pClassName"></param>
        /// <param name="fieldTable"></param>
        /// <returns></returns>
        public static string GetCreateModelFileForParentContent(
               string pTableName, string pProjectName, string pProjectNamespaceByPrefix, string pClassName, DataTable fieldTable)
        {
            string result = "";

            #region 字段Html

            StringBuilder fieldHtml = new StringBuilder();
            foreach (DataRow row in fieldTable.Rows)
            {
                fieldHtml.Append("            public static string "
                    + CamelName.getSmallCamelName(TypeConversion.ToString(row[DataSynchronVO.FieldName])) + " = \""
                    + TypeConversion.ToString(row[DataSynchronVO.FieldName]) + "\"; ");
            }

            #endregion

            #region 内容

            result = @"
namespace " + pProjectNamespaceByPrefix + @"." + pProjectName + @"
{
    public partial class " + pClassName + @"Table
    {
        public static string tableName = """ + pTableName + @"""; 

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
        public static string GetCreateModelFileForEditContent(string pProjectName, string pProjectNamespaceByPrefix, string pClassName)
        {
            string result = "";

            #region 内容

            result = @"
namespace " + pProjectNamespaceByPrefix + @"." + pProjectName + @"
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
            string pTableName, string pProjectName, List<object> lstQueryField, string pProjectNamespaceByPrefix, string pClassName
            , string pSolutionName)
        {
            string result = "";

            #region 方法中的HTML

            //条件HTML
            StringBuilder whereHtml = new StringBuilder();

            //参数HTML
            StringBuilder paraHtml = new StringBuilder();

            foreach (object[] arrQueryField in lstQueryField)
            {
                string fieldName = TypeConversion.ToString(arrQueryField[0]);
                string fieldType = TypeConversion.ToString(arrQueryField[1]);

                switch (fieldType)
                {
                    case "nvarchar":
                        whereHtml.Append(" AND " + fieldName + " LIKE '%' + " + fieldName + " + '%'");
                        paraHtml.Append("            lstPara.Add("
                            + "new SqlParameter(\"" + fieldName + "\", SqlDbType.NVarChar) { Value = dicPara[\"" + fieldName + "\"] });"
                            + ConstantVO.ENTER_R);
                        break;
                }
            }

            #endregion

            #region 内容

            result = @"
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using " + pSolutionName + @".Bll.General;

namespace " + pProjectNamespaceByPrefix + @"." + pProjectName + @"
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

            string sql = ""SELECT * FROM " + pTableName + @" WHERE 1 = 1 " + whereHtml.ToString() + @""";

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
            string pTableName, string pProjectName, List<object> lstQueryField, string pProjectNamespaceByPrefix, string pClassName)
        {
            string result = "";

            #region 查询的HTML

            StringBuilder queryHtml = new StringBuilder();
            foreach (object[] arrQueryField in lstQueryField)
            {
                string fieldName = TypeConversion.ToString(arrQueryField[0]);
                string fieldType = TypeConversion.ToString(arrQueryField[1]);

                switch (fieldType)
                {
                    case "nvarchar":
                        queryHtml.Append("        protected global::DevExpress.Web.ASPxTextBox txt" + CamelName.getBigCamelName(fieldName)
                            + ";"
                            + ConstantVO.ENTER_R);
                        break;
                }
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

namespace " + pProjectNamespaceByPrefix + @"." + pProjectName + @" {
    
    
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
            string pTableName, string pProjectName, List<object> lstQueryField, string pProjectNamespaceByPrefix, string pClassName)
        {
            string result = "";

            #region InitData方法中的HTML

            StringBuilder InitDataQueryHtml = new StringBuilder();

            foreach (object[] arrQueryField in lstQueryField)
            {
                string fieldName = TypeConversion.ToString(arrQueryField[0]);
                string fieldType = TypeConversion.ToString(arrQueryField[1]);

                switch (fieldType)
                {
                    case "nvarchar":
                        InitDataQueryHtml.Append("            string " + CamelName.getSmallCamelName(fieldName)
                            + " = txtN" + CamelName.getBigCamelName(fieldName) + ".Text.Trim();"
                            + ConstantVO.ENTER_R);
                        InitDataQueryHtml.Append("            dicPara.Add(\"" + fieldName + "\", " + CamelName.getSmallCamelName(fieldName) + ");"
                            + ConstantVO.ENTER_R);
                        break;
                }
            }

            #endregion

            #region 内容

            result = @"
using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using Treasure.BLL.ProjectCollection." + pProjectName + @";
using Treasure.Model.General;
using Treasure.Model.ProjectCollection." + pProjectName + @";
using Treasure.Utility.Utilitys;

namespace " + pProjectNamespaceByPrefix + @"." + pProjectName + @"
{
    public partial class " + pClassName + @" : System.Web.UI.Page
    {
        #region 自定义变量

        " + pClassName + @"Bll bll = new " + pClassName + @"Bll();

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
                if (Request[""btnExportExcel""] == ""导出Excel"")
                {
                    ExportExcel();
                    return;
                }
                if (Request[""__CALLBACKID""] == ""grdData"")
                {
                    InitData();
                    return;
                }
            }

            if (!IsPostBack)
            {
                InitData();
            }
        }
        #endregion

        #region 自定义事件

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
        #endregion

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
            Response.Redirect(""" + pClassName + @"Edit.aspx"");
        }
        #endregion

        #region 初始化列表
        /// <summary>
        /// 初始化列表
        /// </summary>
        private void InitData()
        {
            Dictionary<string, object> dicPara = new Dictionary<string, object>();

" + InitDataQueryHtml.ToString() + @"

            DataTable dt = bll.Query(dicPara);
            grdData.DataSource = dt;
            grdData.DataBind();
        }
        #endregion

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
            string pTableName, string pProjectName, List<object> lstQueryField, string pProjectNamespaceByPrefix, string pClassName
            , DataTable pFieldTable)
        {
            string result = "";

            #region 查询HTML

            StringBuilder queryHtml = new StringBuilder();

            foreach (object[] arrQueryField in lstQueryField)
            {
                string fieldName = TypeConversion.ToString(arrQueryField[0]);
                string fieldType = TypeConversion.ToString(arrQueryField[1]);
                string fieldDescription = TypeConversion.ToString(arrQueryField[2]);

                queryHtml.Append("                    <td>" + fieldDescription + "</td>" + ConstantVO.ENTER_R);
                queryHtml.Append("                    <td>" + ConstantVO.ENTER_R);

                switch (fieldType)
                {
                    case "nvarchar":
                        queryHtml.Append("                        <dx:ASPxTextBox ID=\"txt" + CamelName.getBigCamelName(fieldName) + "\" runat=\"server\" Width=\"170px\">" + ConstantVO.ENTER_R);
                        queryHtml.Append("                        </dx:ASPxTextBox>" + ConstantVO.ENTER_R);
                        break;
                }

                queryHtml.Append("                    </td>" + ConstantVO.ENTER_R);
            }

            queryHtml.Append("                    <td>" + ConstantVO.ENTER_R);
            queryHtml.Append("                        < input id = \"btnQuery\" name = \"btnQuery\" type = \"submit\" value = \"查询\" /></ td > " + ConstantVO.ENTER_R);
            queryHtml.Append("                    < td > &nbsp; &nbsp; &nbsp;</ td > " + ConstantVO.ENTER_R);

            #endregion

            #region Grid列的HTML

            StringBuilder gridColumnsHtml = new StringBuilder();

            GeneralBll bllGeneral = new GeneralBll();
            List<string> lstExcludedField = bllGeneral.GetExcludedFields();

            int idx = 0;
            foreach (DataRow row in pFieldTable.Rows)
            {
                string fieldName = TypeConversion.ToString(row[DataSynchronVO.FieldName]);

                //默认要排序的字段
                if (lstExcludedField.Contains(fieldName) == true)
                {
                    continue;
                }

                switch (TypeConversion.ToString(row[DataSynchronVO.FieldType]))
                {
                    case "nvarchar":
                        gridColumnsHtml.Append("<dx:GridViewDataTextColumn Caption=\"" + TypeConversion.ToString(row[DataSynchronVO.FieldDescription])
                            + "\" FieldName=\"" + fieldName
                            + "\" Name=\"col" + fieldName
                            + "\" VisibleIndex=\"" + idx.ToString() + "\">");
                        gridColumnsHtml.Append("</dx:GridViewDataTextColumn>");
                        break;
                }
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
                    <td><input id=""btnExportExcel"" name=""btnExportExcel"" type=""submit"" value=""导出Excel"" /></td>
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
            string pTableName, string pProjectName, List<object> lstQueryField, string pProjectNamespaceByPrefix, string pClassName
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

                switch (TypeConversion.ToString(row[DataSynchronVO.FieldType]))
                {
                    case "nvarchar":
                        fieldHtml.Append("        protected global::DevExpress.Web.ASPxTextBox txt" + CamelName.getBigCamelName(fieldName)
                            + ";"
                            + ConstantVO.ENTER_R);
                        break;
                }
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

namespace " + pProjectNamespaceByPrefix + @"." + pProjectName + @" {    
    

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
            , DataTable pFieldTable)
        {
            string result = "";

            #region 方法中的HTML

            //InitData方法中的HTML
            StringBuilder InitDataHtml = new StringBuilder();

            //Add方法中的HTML
            StringBuilder AddHtml = new StringBuilder();

            //Edit方法中的HTML
            StringBuilder EditHtml = new StringBuilder();

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

                switch (TypeConversion.ToString(row[DataSynchronVO.FieldType]))
                {
                    case "nvarchar":
                        InitDataHtml.Append("                    txt" + fieldName + ".Text = TypeConversion.ToString(row["
                            + pClassName + "Table.Fields." + CamelName.getSmallCamelName(fieldName) + "]);"
                            + ConstantVO.ENTER_R);
                        AddHtml.Append("            row["
                            + pClassName + "Table.Fields." + CamelName.getSmallCamelName(fieldName) + "] = txt" + fieldName + ".Text.Trim();"
                            + ConstantVO.ENTER_R);
                        EditHtml.Append("            row["
                            + pClassName + "Table.Fields." + CamelName.getSmallCamelName(fieldName) + "] = txt" + fieldName + ".Text.Trim();"
                            + ConstantVO.ENTER_R);
                        break;
                }
            }

            #endregion

            #region 内容

            result = @"
using System;
using System.Data;
using System.Web.UI;
using Treasure.BLL.ProjectCollection." + pProjectName + @";
using Treasure.Model.ProjectCollection." + pProjectName + @";
using Treasure.Utility.Utilitys;

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

                InitData();
            }
        }

        #endregion

        #region 按钮
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

            row[SysMenuItemTypeTable.Fields.createDatetime] = today;
            row[SysMenuItemTypeTable.Fields.modifyDatetime] = today;

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

            row[SysMenuItemTypeTable.Fields.modifyDatetime] = today;

            bll.UpdateDataRow(row);

            Response.Redirect(""" + pClassName + @".aspx"");
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
            string pTableName, string pProjectName, List<object> lstQueryField, string pProjectNamespaceByPrefix, string pClassName
            , DataTable pFieldTable)
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

                switch (TypeConversion.ToString(row[DataSynchronVO.FieldType]))
                {
                    case "nvarchar":
                        editHtml.Append("                <tr>" + ConstantVO.ENTER_R);
                        editHtml.Append("                    <td>" + TypeConversion.ToString(row[DataSynchronVO.FieldDescription]) + "：</td>"
                            + ConstantVO.ENTER_R);
                        editHtml.Append("                    <td>" + ConstantVO.ENTER_R);
                        editHtml.Append("                        <dx:ASPxTextBox ID=\"txt" + fieldName + "\" runat=\"server\" Width=\"170px\">"
                            + ConstantVO.ENTER_R);
                        editHtml.Append("                        </dx:ASPxTextBox>" + ConstantVO.ENTER_R);
                        editHtml.Append("                    </td>" + ConstantVO.ENTER_R);
                        editHtml.Append("                </tr>" + ConstantVO.ENTER_R);
                        break;
                }
            }

            #endregion

            #region 内容

            result = @"
<%@ Page Language=""C#"" AutoEventWireup=""true"" CodeBehind=""" + pClassName + @"Edit.aspx.cs"" Inherits=""" + pProjectNamespaceByPrefix + @"." + pProjectName + @"." + pClassName + @"Edit"" %>

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