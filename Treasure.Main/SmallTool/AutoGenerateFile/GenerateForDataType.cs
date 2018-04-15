using System.Collections.Generic;

namespace Treasure.Main.SmallTool.AutoGenerateFile
{
    public static class GenerateForDataType
    {

        /// <summary>
        /// 根据不同的数据类型获取不同地方的值
        /// </summary>
        /// <param name="pDataType">数据类型</param>
        /// <param name="pType">方法名称</param>
        /// <param name="pDic">
        /// [CreateListFileForAspx_Grid=Caption，FieldName，Idx]；
        /// [CreateListFileForAspx_Query=FieldName]；
        /// [GetCreateListFileForCsContent=FieldName]；
        /// [GetCreateListFileForDesignerContent=FieldName]
        /// </param>
        /// <returns></returns>
        public static string GetString(string pDataType, string pType, Dictionary<string, string> pDic)
        {
            string result = "";

            switch (pDataType)
            {
                case "nvarchar":
                    switch (pType)
                    {
                        case "GetCreateListFileForDesignerContent":
                            result = "protected global::DevExpress.Web.ASPxTextBox txt"+ pDic["FieldName"] + @";";
                            break;
                        case "GetCreateListFileForCsContent":
                            result = @"dicPara.Add("""+ pDic["FieldName"] + @""", txt" + pDic["FieldName"] + @".Text.Trim());";
                            break;
                        case "CreateListFileForAspx_Grid":
                            result = @"<dx:GridViewDataTextColumn Caption=""" + pDic["Caption"] 
                                + @""" FieldName=""" + pDic["FieldName"] 
                                + @""" Name=""col" + pDic["FieldName"]
                                + @""" VisibleIndex="""+ pDic["Idx"] + @"""></dx:GridViewDataTextColumn>";
                            break;
                        case "CreateListFileForAspx_Query":
                            result = @"<dx:ASPxTextBox ID=""txt" + pDic["FieldName"] + @""" runat=""server"" Width=""170px""></dx:ASPxTextBox>";
                            break;
                    }
                    break;
                case "varchar":
                    switch (pType)
                    {
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
                            result = @"<dx:ASPxTextBox ID=""txt" + pDic["FieldName"] + @""" runat=""server"" Width=""170px""></dx:ASPxTextBox>";
                            break;
                    }
                    break;
                case "int":
                    switch (pType)
                    {
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
                            result = @"<dx:ASPxTextBox ID=""txt" + pDic["FieldName"] + @""" runat=""server"" Width=""170px""></dx:ASPxTextBox>";
                            break;
                    }
                    break;
            }

            return result;
        }

    }
}