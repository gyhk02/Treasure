using System.Collections.Generic;
using System.Text;
using Treasure.Utility.Utilitys;

namespace Treasure.Main.SmallTool.AutoGenerateFile
{
    public static class GenerateByTypeForDataType
    {

        #region 根据不同的数据类型获取不同地方的值
        /// <summary>
        /// 根据不同的数据类型获取不同地方的值
        /// </summary>
        /// <param name="pDataType">数据类型</param>
        /// <param name="pType">方法名称</param>
        /// <param name="pDic">
        /// [CreateListFileForAspx_Grid=Caption，FieldName，Idx，ForeignTableName]；
        /// [CreateListFileForAspx_Query=FieldDescription，FieldName，ForeignTableName]；
        /// [GetCreateListFileForCsContent_InitData=FieldName，ForeignTableName]；
        /// [GetCreateListFileForCsContent_Query=FieldName]；
        /// [GetCreateListFileForCsContent_PageLoadBoolMothed=FieldName，ForeignTableName]；
        /// [GetCreateListFileForCsContent_PageLoadIsPostBack=FieldName，ForeignTableName]；
        /// [GetCreateListFileForCsContent_BoolMothed=FieldName，ForeignTableName]；
        /// [GetCreateListFileForDesignerContent=FieldName，ForeignTableName]；
        /// [GetCreateEditFileForDesignerContent=FieldName，ForeignTableName]；
        /// [GetCreateEditFileForCsContent_Add=ClassName，FieldName，ForeignTableName]；
        /// [GetCreateEditFileForCsContent_Edit=ClassName，FieldName，ForeignTableName]；
        /// [GetCreateEditFileForCsContent_Init=ClassName，FieldName，ForeignTableName]；
        /// [GetCreateEditFileForCsContent_PageLoadMothed=ClassName，FieldName，ForeignTableName]；
        /// [GetCreateEditFileForCsContent_PageLoadIsPostBack=ClassName，FieldName，ForeignTableName]；
        /// [GetCreateEditFileForCsContent_Mothed=ClassName，FieldName，FieldDescription，ForeignTableName]；
        /// [CreateEditFileForAspx=FieldDescription，FieldName，ForeignTableName]；
        /// [GetCreateBllFileContent_Where=FieldName]；
        /// [GetCreateBllFileContent_SqlStr=FieldName，ForeignTableName, Idx]；
        /// [GetCreateBllFileContent_JoinStr=FieldName，ForeignTableName, Idx，ForeignFieldName]；
        /// [GetCreateBllFileContent_Para=FieldName，ForeignTableName]；
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
                    result = GetVarcharStrig(pType, pDic);
                    break;
                case "int":
                    result = GetIntStrig(pType, pDic);
                    break;
                case "datetime":
                    result = GetDatetimeStrig(pType, pDic);
                    break;
                case "bit":
                    result = GetBitStrig(pType, pDic);
                    break;
            }

            return result;
        }
        #endregion

        #region 获取Bit类型所返回的字符串
        public static string GetBitStrig(string pType, Dictionary<string, string> pDic)
        {
            string result = "";

            switch (pType)
            {
                case "GetCreateBllFileContent_SqlStr":
                    if (string.IsNullOrEmpty(pDic["ForeignTableName"]) == true)
                    {
                        result = " IIF(" + pDic["FieldName"] + " = 1,'√','') " + pDic["FieldName"] + "_STR,";
                    }
                    else
                    {
                        result = " IIF(A." + pDic["FieldName"] + " = 1,'√','') " + pDic["FieldName"] + "_STR,";
                    }
                    break;
                case "GetCreateBllFileContent_Where":
                    result = @"
            if (dicPara[""" + pDic["FieldName"] + @"""] != null && string.IsNullOrEmpty(TypeConversion.ToString(dicPara[""" + pDic["FieldName"] + @"""])) == false)
            {
                sql = sql + "" AND " + pDic["FieldName"] + @" = @" + pDic["FieldName"] + @""";
            }";
                    break;
                case "GetCreateBllFileContent_Para":
                    result = @"lstPara.Add(new SqlParameter(""@" + pDic["FieldName"] + @""", SqlDbType.Bit) { Value = dicPara[""" + pDic["FieldName"] + @"""] });";
                    break;
                case "CreateEditFileForAspx":
                    result = @"
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <dx:ASPxCheckBox ID=""chk" + pDic["FieldName"] + @""" runat=""server"" Text=""" + pDic["FieldDescription"] + @"""></dx:ASPxCheckBox>
                    </td>
                </tr>";
                    break;
                case "GetCreateEditFileForCsContent_Add":
                case "GetCreateEditFileForCsContent_Edit":
                    result = @"
            if (chk" + pDic["FieldName"] + @".Value == null)
            {
                row[" + pDic["ClassName"] + @"Table.Fields." + CamelName.getSmallCamelName(pDic["FieldName"]) + @"] = DBNull.Value;
            }
            else
            {
                row[" + pDic["ClassName"] + @"Table.Fields." + CamelName.getSmallCamelName(pDic["FieldName"]) + @"] = chk" + pDic["FieldName"] + @".Value;
            }";
                    break;
                case "GetCreateEditFileForCsContent_Init":
                    result = @"chk" + pDic["FieldName"] + ".Value = row[" + pDic["ClassName"] + "Table.Fields." + CamelName.getSmallCamelName(pDic["FieldName"]) + "];";
                    break;
                case "GetCreateEditFileForDesignerContent":
                    result = "protected global::DevExpress.Web.ASPxCheckBox chk" + pDic["FieldName"] + ";";
                    break;
                case "GetCreateListFileForDesignerContent":
                    result = "protected global::DevExpress.Web.ASPxGridLookup lup" + pDic["FieldName"] + @";";
                    break;
                case "GetCreateListFileForCsContent_Query":
                    result = @"";
                    break;
                case "GetCreateListFileForCsContent_PageLoadBoolMothed":
                    result = @"
                if (Request[""__CALLBACKID""] == ""lup" + pDic["FieldName"] + @"$DDD$gv"")
                {
                    Init" + CamelName.getBigCamelName(pDic["FieldName"]) + @"();
                    return;
                }";
                    break;
                case "GetCreateListFileForCsContent_PageLoadIsPostBack":
                    result = @"Init" + CamelName.getBigCamelName(pDic["FieldName"]) + @"();";
                    break;
                case "GetCreateListFileForCsContent_BoolMothed":
                    result = @"
        #region 初始化" + pDic["FieldDescription"] + @"
        /// <summary>
        /// 初始化" + pDic["FieldDescription"] + @"
        /// </summary>
        private void Init" + CamelName.getBigCamelName(pDic["FieldName"]) + @"()
        {
            DataTable dt = bllGeneral.GetYesOrNot();
            ASPxGridLookupExtend.BindToShowName(lup" + pDic["FieldName"] + @", dt, true);
        }
        #endregion";
                    break;
                case "GetCreateListFileForCsContent_InitData":
                    result = @"
            if (lup" + pDic["FieldName"] + @".Value == null)
            {
                dicPara.Add(""" + pDic["FieldName"] + @""", DBNull.Value);
            }
            else
            {
                dicPara.Add(""" + pDic["FieldName"] + @""", lup" + pDic["FieldName"] + @".Value);
            }";
                    break;
                case "CreateListFileForAspx_Grid":
                    result = @"<dx:GridViewDataTextColumn Caption=""" + pDic["Caption"] + @""" FieldName=""" + pDic["FieldName"] + @"_STR"" Name=""col" + pDic["FieldName"] + @""" VisibleIndex=""" + pDic["Idx"] + @"""></dx:GridViewDataTextColumn>";
                    break;
                case "CreateListFileForAspx_Query":
                    result = @"
                    <td>" + pDic["FieldDescription"] + @"</td>
                    <td>
                        <dx:ASPxGridLookup ID=""lup" + pDic["FieldName"] + @""" runat=""server"" Width=""80px"" AutoGenerateColumns=""False"" KeyFieldName=""ID"">
                            <GridViewProperties>
                                <SettingsBehavior AllowFocusedRow=""True"" AllowSelectSingleRowOnly=""True""></SettingsBehavior>
                            </GridViewProperties>
                            <Columns>
                                <dx:GridViewDataTextColumn Caption=""" + pDic["FieldDescription"] + @""" FieldName=""NAME"" VisibleIndex=""0"">
                                </dx:GridViewDataTextColumn>
                            </Columns>
                        </dx:ASPxGridLookup>
                    </td>";
                    break;
            }

            return result;
        }
        #endregion

        #region 获取Int类型所返回的字符串
        /// <summary>
        /// 获取Int类型所返回的字符串。
        /// 与GetNvarcharStrig仅GetCreateEditFileForCsContent_Edit的内容不同
        /// </summary>
        /// <param name="pType"></param>
        /// <param name="pDic"></param>
        /// <returns></returns>
        public static string GetIntStrig(string pType, Dictionary<string, string> pDic)
        {
            string result = "";

            switch (pType)
            {
                case "GetCreateBllFileContent_Where":
                    result = @"
            if (string.IsNullOrEmpty(TypeConversion.ToString(dicPara[""" + pDic["FieldName"] + @"""])) == false)
            {
                sql = sql + "" AND " + pDic["FieldName"] + @" LIKE '%' + @" + pDic["FieldName"] + @" + '%'"";
            }";
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
                </tr>";
                    break;
                case "GetCreateEditFileForCsContent_Add":
                    result = "row[" + pDic["ClassName"] + "Table.Fields." + CamelName.getSmallCamelName(pDic["FieldName"]) + "] = txt" + pDic["FieldName"] + ".Text.Trim();";
                    break;
                case "GetCreateEditFileForCsContent_Edit":
                    result = "row[" + pDic["ClassName"] + "Table.Fields." + CamelName.getSmallCamelName(pDic["FieldName"]) + "] = TypeConversion.ToInt(txt" + pDic["FieldName"] + ".Text.Trim());";
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
                case "GetCreateListFileForCsContent_InitData":
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

        #region 获取Nvarchar类型所返回的字符串
        public static string GetNvarcharStrig(string pType, Dictionary<string, string> pDic)
        {
            string result = "";

            switch (pType)
            {
                case "GetCreateBllFileContent_SqlStr":
                    if (string.IsNullOrEmpty(pDic["ForeignTableName"]) == false)
                    {
                        result = " A" + pDic["Idx"] + ".NAME " + pDic["ForeignTableName"] + "_NAME,";
                    }
                    break;
                case "GetCreateBllFileContent_JoinStr":
                    if (string.IsNullOrEmpty(pDic["ForeignTableName"]) == false)
                    {
                        result = " JOIN " + pDic["ForeignTableName"] + " A" + pDic["Idx"] + " ON A." + pDic["FieldName"] + " = A" + pDic["Idx"] + "." + pDic["ForeignFieldName"] + "";
                    }
                    break;
                case "GetCreateBllFileContent_Where":
                    if (string.IsNullOrEmpty(pDic["ForeignTableName"]) == true)
                    {
                        result = @"
            if (string.IsNullOrEmpty(TypeConversion.ToString(dicPara[""" + pDic["FieldName"] + @"""])) == false)
            {
                sql = sql + "" AND " + pDic["FieldName"] + @" LIKE '%' + @" + pDic["FieldName"] + @" + '%'"";
            }";
                    }
                    else
                    {
                        result = @"
            if (dicPara[""" + pDic["FieldName"] + @"""] != null && string.IsNullOrEmpty(TypeConversion.ToString(dicPara[""" + pDic["FieldName"] + @"""])) == false)
            {
                sql = sql + "" AND " + pDic["FieldName"] + @" = @" + pDic["FieldName"] + @""";
            }";
                    }

                    break;
                case "GetCreateBllFileContent_Para":
                    if (string.IsNullOrEmpty(pDic["ForeignTableName"]) == true)
                    {
                        result = @"lstPara.Add(new SqlParameter(""@" + pDic["FieldName"] + @""", SqlDbType.NVarChar) { Value = dicPara[""" + pDic["FieldName"] + @"""] });";
                    }
                    else
                    {
                        result = @"lstPara.Add(new SqlParameter(""@" + pDic["FieldName"] + @""", SqlDbType.NVarChar) { Value = dicPara[""" + pDic["FieldName"] + @"""] });";
                    }
                    break;
                case "CreateEditFileForAspx":
                    if (string.IsNullOrEmpty(pDic["ForeignTableName"]) == true)
                    {
                        result = @"
                <tr>
                    <td>" + pDic["FieldDescription"] + @"：</td>
                    <td>
                        <dx:ASPxTextBox ID=""txt" + pDic["FieldName"] + @""" runat=""server"" Width=""170px"">
                        </dx:ASPxTextBox>
                    </td>
                </tr>";
                    }
                    else
                    {
                        result = @"
                    <td>" + pDic["FieldDescription"] + @"：</td>
                    <td>
                        <dx:ASPxGridLookup ID=""lup" + pDic["ForeignTableName"] + @""" runat=""server"" AutoGenerateColumns=""False"" KeyFieldName=""ID"">
                            <GridViewProperties>
                                <SettingsBehavior AllowFocusedRow=""True"" AllowSelectSingleRowOnly=""True""></SettingsBehavior>
                            </GridViewProperties>
                            <Columns>
                                <dx:GridViewDataTextColumn Caption=""名称"" FieldName=""NAME"" VisibleIndex=""0"">
                                </dx:GridViewDataTextColumn>
                            </Columns>
                        </dx:ASPxGridLookup>
                    </td>
                </tr>";
                    }
                    break;
                case "GetCreateEditFileForCsContent_Add":
                case "GetCreateEditFileForCsContent_Edit":
                    if (string.IsNullOrEmpty(pDic["ForeignTableName"]) == true)
                    {
                        result = "row[" + pDic["ClassName"] + "Table.Fields." + CamelName.getSmallCamelName(pDic["FieldName"]) + "] = txt" + pDic["FieldName"] + ".Text.Trim();";
                    }
                    else
                    {
                        result = "row[" + pDic["ClassName"] + "Table.Fields." + CamelName.getSmallCamelName(pDic["FieldName"]) + "] = lup" + pDic["ForeignTableName"] + ".Value;";
                    }
                    break;
                case "GetCreateEditFileForCsContent_Init":
                    if (string.IsNullOrEmpty(pDic["ForeignTableName"]) == true)
                    {
                        result = "txt" + pDic["FieldName"] + ".Text = TypeConversion.ToString(row[" + pDic["ClassName"] + "Table.Fields." + CamelName.getSmallCamelName(pDic["FieldName"]) + "]);";
                    }
                    else
                    {
                        result = "lup" + pDic["ForeignTableName"] + ".Value = row[" + pDic["ClassName"] + "Table.Fields." + CamelName.getSmallCamelName(pDic["FieldName"]) + "];";
                    }
                    break;
                case "GetCreateEditFileForCsContent_PageLoadMothed":
                    if (string.IsNullOrEmpty(pDic["ForeignTableName"]) == false)
                    {
                        result = @"
                if (Request[""__CALLBACKID""] == ""lup" + pDic["ForeignTableName"] + @"$DDD$gv"")
                {
                    Init" + pDic["ForeignTableName"] + @"();
                    return;
                }";
                    }
                    break;
                case "GetCreateEditFileForCsContent_PageLoadIsPostBack":
                    if (string.IsNullOrEmpty(pDic["ForeignTableName"]) == false)
                    {
                        result = "                Init" + pDic["ForeignTableName"] + "();";
                    }
                    break;
                case "GetCreateEditFileForCsContent_Mothed":
                    if (string.IsNullOrEmpty(pDic["ForeignTableName"]) == false)
                    {
                        result = @"
        #region 初始化" + pDic["FieldDescription"] + @"
        /// <summary>
        /// 初始化" + pDic["FieldDescription"] + @"
        /// </summary>
        private void Init" + pDic["ForeignTableName"] + @"()
        {
            DataTable dt = bll.GetTableAllInfo(" + CamelName.getBigCamelName(pDic["ForeignTableName"]) + @"Table.tableName);
            ASPxGridLookupExtend.BindToShowName(lup" + pDic["ForeignTableName"] + @", dt, false);
        }
        #endregion";
                    }
                    break;
                case "GetCreateEditFileForDesignerContent":
                    if (string.IsNullOrEmpty(pDic["ForeignTableName"]) == true)
                    {
                        result = "protected global::DevExpress.Web.ASPxTextBox txt" + pDic["FieldName"] + ";";
                    }
                    else
                    {
                        result = "protected global::DevExpress.Web.ASPxGridLookup lup" + pDic["ForeignTableName"] + ";";
                    }
                    break;
                case "GetCreateListFileForDesignerContent":
                    if (string.IsNullOrEmpty(pDic["ForeignTableName"]) == true)
                    {
                        result = "protected global::DevExpress.Web.ASPxTextBox txt" + pDic["FieldName"] + ";";
                    }
                    else
                    {
                        result = "protected global::DevExpress.Web.ASPxGridLookup lup" + pDic["ForeignTableName"] + ";";
                    }
                    break;
                case "GetCreateListFileForCsContent_InitData":
                    if (string.IsNullOrEmpty(pDic["ForeignTableName"]) == true)
                    {
                        result = @"dicPara.Add(""" + pDic["FieldName"] + @""", txt" + pDic["FieldName"] + @".Text.Trim());";
                    }
                    else
                    {
                        result = @"
            if (lup" + pDic["ForeignTableName"] + @".Value == null)
            {
                dicPara.Add(""" + pDic["FieldName"] + @""", DBNull.Value);
            }
            else
            {
                dicPara.Add(""" + pDic["FieldName"] + @""", lup" + pDic["ForeignTableName"] + @".Value);
            }";
                    }
                    break;
                case "GetCreateListFileForCsContent_PageLoadBoolMothed":
                    if (string.IsNullOrEmpty(pDic["ForeignTableName"]) == false)
                    {
                        result = @"
                if (Request[""__CALLBACKID""] == ""lup" + pDic["ForeignTableName"] + @"$DDD$gv"")
                {
                    Init" + CamelName.getBigCamelName(pDic["ForeignTableName"]) + @"();
                    return;
                }";
                    }
                    break;
                case "GetCreateListFileForCsContent_BoolMothed":
                    if (string.IsNullOrEmpty(pDic["ForeignTableName"]) == false)
                    {
                        result = @"
        #region 初始化" + pDic["FieldDescription"] + @"
        /// <summary>
        /// 初始化" + pDic["FieldDescription"] + @"
        /// </summary>
        private void Init" + CamelName.getBigCamelName(pDic["ForeignTableName"]) + @"()
        {
            DataTable dt = bll.GetTableAllInfo(" + CamelName.getBigCamelName(pDic["ForeignTableName"]) + @"Table.tableName);;
            ASPxGridLookupExtend.BindToShowName(lup" + pDic["ForeignTableName"] + @", dt, true);
        }
        #endregion";
                    }
                    break;
                case "GetCreateListFileForCsContent_PageLoadIsPostBack":
                    if (string.IsNullOrEmpty(pDic["ForeignTableName"]) == false)
                    {
                        result = @"Init" + CamelName.getBigCamelName(pDic["ForeignTableName"]) + @"();";
                    }
                    break;
                case "CreateListFileForAspx_Grid":
                    if (string.IsNullOrEmpty(pDic["ForeignTableName"]) == true)
                    {
                        result = @"<dx:GridViewDataTextColumn Caption=""" + pDic["Caption"]
                            + @""" FieldName=""" + pDic["FieldName"]
                            + @""" Name=""col" + pDic["FieldName"]
                            + @""" VisibleIndex=""" + pDic["Idx"] + @"""></dx:GridViewDataTextColumn>";
                    }
                    else
                    {
                        result = @"<dx:GridViewDataTextColumn Caption=""" + pDic["Caption"]
                            + @""" FieldName=""" + pDic["ForeignTableName"]
                            + @"_NAME"" Name=""col" + pDic["ForeignTableName"] + @"_NAME"" VisibleIndex=""" + pDic["Idx"] + @"""></dx:GridViewDataTextColumn>";
                    }
                    break;
                case "CreateListFileForAspx_Query":
                    if (string.IsNullOrEmpty(pDic["ForeignTableName"]) == true)
                    {
                        result = @"
                    <td>" + pDic["FieldDescription"] + @"</td>
                    <td>
                        <dx:ASPxTextBox ID=""txt" + pDic["FieldName"] + @""" runat=""server"" Width=""170px""></dx:ASPxTextBox>
                    </td>";
                    }
                    else
                    {
                        result = @"
                    <td>" + pDic["FieldDescription"] + @"</td>
                    <td>
                        <dx:ASPxGridLookup ID=""lup" + pDic["ForeignTableName"] + @""" Width=""80px"" runat=""server"" AutoGenerateColumns=""False"" KeyFieldName=""ID"">
                            <GridViewProperties>
                                <SettingsBehavior AllowFocusedRow=""True"" AllowSelectSingleRowOnly=""True""></SettingsBehavior>
                            </GridViewProperties>
                            <Columns>
                                <dx:GridViewDataTextColumn Caption=""名称"" FieldName=""NAME"" VisibleIndex=""0"">
                                </dx:GridViewDataTextColumn>
                            </Columns>
                        </dx:ASPxGridLookup>
                    </td>";
                    }
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
                    result = @"sql = sql + "" AND " + pDic["FieldName"] + @" BETWEEN @" + pDic["FieldName"] + @"_FROM AND @" + pDic["FieldName"] + @"_TO"";";
                    result = @"
            if (
                (string.IsNullOrEmpty(TypeConversion.ToString(dicPara[""" + pDic["FieldName"] + @"_FROM""])) == true
                    && string.IsNullOrEmpty(TypeConversion.ToString(dicPara[""" + pDic["FieldName"] + @"_TO""])) == true
                )
                == false
            )
            {
                if (string.IsNullOrEmpty(TypeConversion.ToString(dicPara[""" + pDic["FieldName"] + @"_FROM""])) == true)
                {
                    dicPara[""" + pDic["FieldName"] + @"_FROM""] = DateTime.Now.AddYears(-5).ToString(ConstantVO.DATE_Y_M_D);
                }
                else
                {
                    dicPara[""" + pDic["FieldName"] + @"_FROM""] = TypeConversion.TimeToString(dicPara[""" + pDic["FieldName"] + @"_FROM""], ConstantVO.DATE_Y_M_D);
                }
                if (string.IsNullOrEmpty(TypeConversion.ToString(dicPara[""" + pDic["FieldName"] + @"_TO""])) == true)
                {
                    dicPara[""" + pDic["FieldName"] + @"_TO""] = DateTime.Now.AddYears(-5).ToString(ConstantVO.DATE_Y_M_D);
                }
                else
                {
                    dicPara[""" + pDic["FieldName"] + @"_TO""] = TypeConversion.TimeToString(dicPara[""" + pDic["FieldName"] + @"_TO""], ConstantVO.DATE_Y_M_D);
                }
                sql = sql + "" AND " + pDic["FieldName"] + @" BETWEEN @" + pDic["FieldName"] + @"_FROM AND @" + pDic["FieldName"] + @"_TO"";
            }";
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
                        <dx:ASPxDateEdit ID=""dat" + pDic["FieldName"] + @""" runat=""server""></dx:ASPxDateEdit>
                     </td> 
                 </tr>";
                    break;
                case "GetCreateEditFileForCsContent_Add":
                case "GetCreateEditFileForCsContent_Edit":
                    result = @"
            string " + CamelName.getSmallCamelName(pDic["FieldName"]) + @" = dat" + pDic["FieldName"] + @".Text.Trim();
            if (string.IsNullOrEmpty(" + CamelName.getSmallCamelName(pDic["FieldName"]) + @") == true)
            {
                row[" + pDic["ClassName"] + @"Table.Fields." + CamelName.getSmallCamelName(pDic["FieldName"]) + @"] = DBNull.Value;
            }
            else
            {
                row[" + pDic["ClassName"] + @"Table.Fields." + CamelName.getSmallCamelName(pDic["FieldName"]) + @"] = " + CamelName.getSmallCamelName(pDic["FieldName"]) + @";
            }";
                    break;
                case "GetCreateEditFileForCsContent_Init":
                    result = "dat" + pDic["FieldName"] + ".Value = row[" + pDic["ClassName"] + "Table.Fields." + CamelName.getSmallCamelName(pDic["FieldName"]) + "];";
                    break;
                case "GetCreateEditFileForDesignerContent":
                    result = "protected global::DevExpress.Web.ASPxDateEdit dat" + pDic["FieldName"] + ";";
                    break;
                case "GetCreateListFileForDesignerContent":
                    result = @"
        protected global::DevExpress.Web.ASPxDateEdit dat" + pDic["FieldName"] + @"_FROM;
        protected global::DevExpress.Web.ASPxDateEdit dat" + pDic["FieldName"] + @"_TO;";
                    break;
                case "GetCreateListFileForCsContent_Query":
                    result = @"
            if (TypeConversion.ToDateTime(dat" + pDic["FieldName"] + @"_FROM.Text) > TypeConversion.ToDateTime(dat" + pDic["FieldName"] + @"_TO))
            {
                ClientScriptManager clientScript = Page.ClientScript;
                clientScript.RegisterStartupScript(this.GetType(), """", ""<script type=text/javascript>alert('起始时间不能大于结时间');</script>"");
                return;
            }";
                    break;
                case "GetCreateListFileForCsContent_InitData":
                    result = @"
            dicPara.Add(""" + pDic["FieldName"] + @"_FROM"", dat" + pDic["FieldName"] + @"_FROM.Text);
            dicPara.Add(""" + pDic["FieldName"] + @"_TO"", dat" + pDic["FieldName"] + @"_TO.Text);";
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
        
        #region 获取Varchar类型所返回的字符串
        public static string GetVarcharStrig(string pType, Dictionary<string, string> pDic)
        {
            string result = "";

            switch (pType)
            {
                case "GetCreateBllFileContent_Where":
                    result = @"
            if (string.IsNullOrEmpty(TypeConversion.ToString(dicPara[""" + pDic["FieldName"] + @"""])) == false)
            {
                sql = sql + "" AND " + pDic["FieldName"] + @" LIKE '%' + @" + pDic["FieldName"] + @" + '%'"";
            }";
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
                </tr>";
                    break;
                case "GetCreateEditFileForCsContent_Add":
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
                    result = "protected global::DevExpress.Web.ASPxTextBox txt" + pDic["FieldName"] + ";";
                    break;
                case "GetCreateListFileForCsContent_InitData":
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
    }
}