using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Treasure.Model.General;
using Treasure.Utility.Utilitys;

namespace Treasure.Main.SmallTool.AutoGenerateFile
{
    public static class GenerateBySingleTableContent
    {
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
using Treasure.BLL.ProjectCollection.FirstProject;
using Treasure.Model.General;
using Treasure.Model.ProjectCollection.FirstProject;
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

        /// <summary>
        /// 列表之aspx的文件内容
        /// </summary>
        /// <returns></returns>
        public static string CreateListFileForAspx(
           string pTableName, string pProjectName, List<object> lstQueryField, string pProjectNamespaceByPrefix, string pClassName)
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
                    <dx:GridViewDataTextColumn Caption=""NO"" FieldName=""NO"" Name=""colNO"" VisibleIndex=""0"">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption=""NAME"" FieldName=""NAME"" Name=""colNAME"" VisibleIndex=""1"">
                    </dx:GridViewDataTextColumn>
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

    }
}