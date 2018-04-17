using System.Collections.Generic;
using System.Text;
using Treasure.Utility.Utilitys;

namespace Treasure.Main.SmallTool.AutoGenerateFile
{
    public static class GenerateForDataType
    {

        #region 根据不同的数据类型获取不同地方的值
        /// <summary>
        /// 根据不同的数据类型获取不同地方的值
        /// </summary>
        /// <param name="pDataType">数据类型</param>
        /// <param name="pType">方法名称</param>
        /// <param name="pDic">
        /// [CreateListFileForAspx_Grid=Caption，FieldName，Idx]；
        /// [CreateListFileForAspx_Query=FieldDescription，FieldName]；
        /// [GetCreateListFileForCsContent=FieldName]；
        /// [GetCreateListFileForDesignerContent=FieldName]；
        /// [GetCreateEditFileForDesignerContent=FieldName]；
        /// [GetCreateEditFileForCsContent_Add=ClassName，FieldName]；
        /// [GetCreateEditFileForCsContent_Edit=ClassName，FieldName]；
        /// [GetCreateEditFileForCsContent_Init=ClassName，FieldName]；
        /// [CreateEditFileForAspx=FieldDescription，FieldName]；
        /// [GetCreateBllFileContent_Where=FieldName]；
        /// [GetCreateBllFileContent_Para=FieldName]；
        /// </param>
        /// <returns></returns>
        public static string GetString(string pDataType, string pType, Dictionary<string, string> pDic)
        {
            string result = "";

            switch (pDataType)
            {
                case "nvarchar":
                    result = GetNvarcharStrig(pType, pDic);
                    break;
                case "varchar":
                    result = GetNvarcharStrig(pType, pDic);
                    break;
                case "int":
                    result = GetNvarcharStrig(pType, pDic);
                    break;
                case "datetime":
                    result = GetDatetimeStrig(pType, pDic);
                    break;
            }

            return result;
        }
        #endregion

        #region 获取Nvarchar类型所返回的字符串
        public static string GetNvarcharStrig(string pType, Dictionary<string, string> pDic)
        {
            string result = "";

            switch (pType)
            {
                case "GetCreateBllFileContent_Where":
                    result = " AND " + pDic["FieldName"] + " LIKE '%' + @" + pDic["FieldName"] + " + '%'";
                    break;
                case "GetCreateBllFileContent_Para":
                    result = @"lstPara.Add(new SqlParameter(""@" + pDic["FieldName"] + @""", SqlDbType.NVarChar) { Value = dicPara[""" + pDic["FieldName"] + @"""] });";
                    break;
                case "CreateEditFileForAspx":
                    result = @"
                <tr>
                    <td>" + pDic["FieldDescription"] + @"：</td>
                    <td>
                        <dx:ASPxTextBox ID=""txt" + pDic["FieldName"] + @""" runat=""server"" Width=""170px"">
                        </dx:ASPxTextBox> 
                     </td> 
                 </tr> ";
                    break;
                case "GetCreateEditFileForCsContent_Add":
                    result = "row[" + pDic["ClassName"] + "Table.Fields." + CamelName.getSmallCamelName(pDic["FieldName"]) + "] = txt" + pDic["FieldName"] + ".Text.Trim();";
                    break;
                case "GetCreateEditFileForCsContent_Edit":
                    result = "row[" + pDic["ClassName"] + "Table.Fields." + CamelName.getSmallCamelName(pDic["FieldName"]) + "] = txt" + pDic["FieldName"] + ".Text.Trim();";
                    break;
                case "GetCreateEditFileForCsContent_Init":
                    result = "txt" + pDic["FieldName"] + ".Text = TypeConversion.ToString(row[" + pDic["ClassName"] + "Table.Fields." + CamelName.getSmallCamelName(pDic["FieldName"]) + "]);";
                    break;
                case "GetCreateEditFileForDesignerContent":
                    result = "protected global::DevExpress.Web.ASPxTextBox txt" + pDic["FieldName"] + ";";
                    break;
                case "GetCreateListFileForDesignerContent":
                    result = "protected global::DevExpress.Web.ASPxTextBox txt" + pDic["FieldName"] + @";";
                    break;
                case "GetCreateListFileForCsContent":
                    result = @"dicPara.Add(""" + pDic["FieldName"] + @""", txt" + pDic["FieldName"] + @".Text.Trim());";
                    break;
                case "CreateListFileForAspx_Grid":
                    result = @"<dx:GridViewDataTextColumn Caption=""" + pDic["Caption"]
                        + @""" FieldName=""" + pDic["FieldName"]
                        + @""" Name=""col" + pDic["FieldName"]
                        + @""" VisibleIndex=""" + pDic["Idx"] + @"""></dx:GridViewDataTextColumn>";
                    break;
                case "CreateListFileForAspx_Query":
                    result = @"
                    <td>" + pDic["FieldDescription"] + @"</td>
                    <td>
                        <dx:ASPxTextBox ID=""txt" + pDic["FieldName"] + @""" runat=""server"" Width=""170px""></dx:ASPxTextBox>
                    </td>";
                    break;
            }

            return result;
        }
        #endregion

        #region 获取Datetime类型所返回的字符串
        public static string GetDatetimeStrig(string pType, Dictionary<string, string> pDic)
        {
            string result = "";

            switch (pType)
            {
                case "GetCreateBllFileContent_Where":
                    result = @" AND " + pDic["FieldName"] + " BETWEEN @" + pDic["FieldName"] + "_FROM AND @" + pDic["FieldName"] + "_TO";
                    break;
                case "GetCreateBllFileContent_Para":
                    result = @"
            lstPara.Add(new SqlParameter(""@" + pDic["FieldName"] + @"_FROM"", SqlDbType.NVarChar) { Value = dicPara[""" + pDic["FieldName"] + @"_FROM""] });
            lstPara.Add(new SqlParameter(""@" + pDic["FieldName"] + @"_TO"", SqlDbType.NVarChar) { Value = dicPara[""" + pDic["FieldName"] + @"_TO""] });";
                    break;
                case "CreateEditFileForAspx":
                    result = @"
                <tr>
                    <td>" + pDic["FieldDescription"] + @"：</td>
                    <td>
                        <dx:ASPxTextBox ID=""txt" + pDic["FieldName"] + @""" runat=""server"" Width=""170px"">
                        </dx:ASPxTextBox> 
                     </td> 
                 </tr> ";
                    break;
                case "GetCreateEditFileForCsContent_Add":
                    result = "row[" + pDic["ClassName"] + "Table.Fields." + CamelName.getSmallCamelName(pDic["FieldName"]) + "] = txt" + pDic["FieldName"] + ".Text.Trim();";
                    break;
                case "GetCreateEditFileForCsContent_Edit":
                    result = "row[" + pDic["ClassName"] + "Table.Fields." + CamelName.getSmallCamelName(pDic["FieldName"]) + "] = txt" + pDic["FieldName"] + ".Text.Trim();";
                    break;
                case "GetCreateEditFileForCsContent_Init":
                    result = "txt" + pDic["FieldName"] + ".Text = TypeConversion.ToString(row[" + pDic["ClassName"] + "Table.Fields." + CamelName.getSmallCamelName(pDic["FieldName"]) + "]);";
                    break;
                case "GetCreateEditFileForDesignerContent":
                    result = "protected global::DevExpress.Web.ASPxTextBox txt" + pDic["FieldName"] + ";";
                    break;
                case "GetCreateListFileForDesignerContent":
                    result = "protected global::DevExpress.Web.ASPxTextBox txt" + pDic["FieldName"] + @";";
                    break;
                case "GetCreateListFileForCsContent":
                    result = @"
            string " + pDic["FieldName"] + @"_FROM = """";
            if (string.IsNullOrEmpty(dat" + pDic["FieldName"] + @"_FROM.Text) == true)
            {
                " + pDic["FieldName"] + @"_FROM = DateTime.Now.AddYears(-5).ToString(ConstantVO.DATE_Y_M_D);
            }
            else
            {
                " + pDic["FieldName"] + @"_FROM = TypeConversion.TimeToString(dat" + pDic["FieldName"] + @"_FROM.Text, ConstantVO.DATE_Y_M_D);
            }
            dicPara.Add(""" + pDic["FieldName"] + @"_FROM"", " + pDic["FieldName"] + @"_FROM);

            string " + pDic["FieldName"] + @"_TO = """";
            if (string.IsNullOrEmpty(dat" + pDic["FieldName"] + @"_TO.Text) == true)
            {
                " + pDic["FieldName"] + @"_TO = DateTime.Now.AddYears(5).ToString(ConstantVO.DATE_Y_M_D);
            }
            else
            {
                " + pDic["FieldName"] + @"_TO = ((DateTime)TypeConversion.ToDateTime(dat" + pDic["FieldName"] + @"_TO.Text)).AddDays(1).ToString(ConstantVO.DATE_Y_M_D);
            }
            dicPara.Add(""" + pDic["FieldName"] + @"_TO"", " + pDic["FieldName"] + @"_TO);";
                    break;
                case "CreateListFileForAspx_Grid":
                    result = @"
                    <dx:GridViewDataTextColumn Caption=""" + pDic["Caption"] + @""" Name = ""col" + pDic["FieldName"] + @""" FieldName=""" + pDic["FieldName"]
                        + @""" VisibleIndex=""" + pDic["Idx"] + @""">
                        <PropertiesTextEdit DisplayFormatString = ""yyyy-MM-dd""></PropertiesTextEdit>
                     </dx:GridViewDataTextColumn>";
                    break;
                case "CreateListFileForAspx_Query":
                    result = @"
                    <td>" + pDic["FieldDescription"] + @"</td>
                    <td>
                        <dx:ASPxDateEdit ID=""dat" + pDic["FieldName"] + @"_FROM"" runat=""server""></dx:ASPxDateEdit>
                    </td>
                    <td> -</td>
                    <td>
                        <dx:ASPxDateEdit ID = ""dat" + pDic["FieldName"] + @"_TO"" runat = ""server""></dx:ASPxDateEdit>
                     </td>";
                    break;
            }

            return result;
        }
        #endregion

    }
}