<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GenerateBySingleTable.aspx.cs" Inherits="Treasure.Main.SmallTool.AutoGenerateFile.GenerateBySingleTable" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div style="float: left;">
                <table>
                    <tr>
                        <td>
                            <dx:ASPxGridLookup ID="gluTableList" runat="server" KeyFieldName="ID" AutoGenerateColumns="False" NullText="选择表">
                                <GridViewProperties>
                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>
                                </GridViewProperties>
                                <Columns>
                                    <dx:GridViewDataTextColumn Caption="表名" FieldName="TableName" VisibleIndex="0">
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                            </dx:ASPxGridLookup>
                        </td>
                        <td>&nbsp;                 
                        </td>
                        <td>
                            <input id="btnQuery" name="btnQuery" type="submit" value="查询" /></td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>

                        <td>&nbsp;</td>

                    </tr>
                </table>
                <br />
                <dx:ASPxGridView ID="grdData" runat="server" AutoGenerateColumns="False" KeyFieldName="FieldName">
                    <SettingsPager Mode="ShowAllRecords" Visible="False">
                    </SettingsPager>
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="字段名" VisibleIndex="1" FieldName="FieldName">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="字段描述" VisibleIndex="3" FieldName="FieldDescription">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="外键" VisibleIndex="4" FieldName="IsForeign">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewCommandColumn Caption="是查询条件?" ShowSelectCheckbox="True" VisibleIndex="0">
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn Caption="字段类型" FieldName="FieldType" VisibleIndex="2">
                        </dx:GridViewDataTextColumn>
                    </Columns>
                </dx:ASPxGridView>
            </div>
            <div style="float: left; width: 50px;">&nbsp;</div>
            <div style="float: left;">
                <table style="width: 100%;">
                    <tr>
                        <td>
                            <dx:ASPxGridLookup ID="gluProject" runat="server" NullText="选择项目" KeyFieldName="ID" AutoGenerateColumns="False">
                                <GridViewProperties>
                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>
                                </GridViewProperties>
                                <Columns>
                                    <dx:GridViewDataTextColumn Caption="项目名称" FieldName="NAME" VisibleIndex="0">
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                            </dx:ASPxGridLookup>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxCheckBox ID="chkReportExcel" runat="server" Text="导出Excel ?">
                            </dx:ASPxCheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <input id="btnGenerate" name="btnGenerate" type="submit" value="开始生成" /></td>
                    </tr>
                </table>
            </div>
            <div style="float:left;">
                <asp:HiddenField ID="hdnTableName" runat="server" />
                <asp:HiddenField ID="hdnProjectPathByPrefix" runat="server" />
                <asp:HiddenField ID="hdnProjectNamespaceByPrefix" runat="server" />
                <asp:HiddenField ID="hdnProjectFolder" runat="server" />
                <asp:HiddenField ID="hdnSolutionPath" runat="server" />
                <asp:HiddenField ID="hdnSolutionName" runat="server" />
            </div>
        </div>

    </form>
</body>
</html>
