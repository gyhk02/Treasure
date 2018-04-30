<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SysReport.aspx.cs" Inherits="Treasure.Main.Frame.SysReport" %>

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
            <table>
                <tr>

                    <td>报表名称</td>
                    <td>
                        <dx:ASPxTextBox ID="txtNAME" runat="server" Width="170px"></dx:ASPxTextBox>
                    </td>
                    <td>
                        <input id="btnQuery" name="btnQuery" type="submit" value="查询" /></td>
                    <td>&nbsp; &nbsp; &nbsp;</td>
                    <td>
                        <input id="btnAdd" name="btnAdd" type="submit" value="新增" /></td>

                    <td>&nbsp;</td>
                </tr>
            </table>
            <br />
            <dx:ASPxGridView ID="grdData" runat="server" AutoGenerateColumns="False" Style="margin-right: 0px"
                KeyFieldName="ID" OnRowDeleting="grdData_RowDeleting">
                <SettingsPager PageSize="20">
                </SettingsPager>
                <SettingsBehavior ConfirmDelete="True" />
                <Columns>
                    <dx:GridViewDataTextColumn Caption="编号" FieldName="NO" Name="colNO" VisibleIndex="0"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="报表名称" FieldName="NAME" Name="colNAME" VisibleIndex="0"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="原SQL" FieldName="SOURCE_SQL" Name="colSOURCE_SQL" VisibleIndex="0"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="目标SQL" FieldName="TARGET_SQL" Name="colTARGET_SQL" VisibleIndex="0"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataHyperLinkColumn Caption="修改" VisibleIndex="2" FieldName="ID">
                        <PropertiesHyperLinkEdit NavigateUrlFormatString="SysReportEdit.aspx?ID={0}" Text="修改">
                        </PropertiesHyperLinkEdit>
                    </dx:GridViewDataHyperLinkColumn>
                    <dx:GridViewCommandColumn Caption="删除"
                        ShowDeleteButton="True" VisibleIndex="3">
                    </dx:GridViewCommandColumn>
                </Columns>
            </dx:ASPxGridView>
            <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server" GridViewID="grdData">
            </dx:ASPxGridViewExporter>
            <asp:HiddenField ID="hidCondition" runat="server" />
        </div>
    </form>
</body>
</html>
