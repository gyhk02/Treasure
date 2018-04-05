<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SysMenuItemType.aspx.cs" Inherits="Treasure.Main.ProjectCollection.FirstProject.SysMenuItemType" %>

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
            <table >
                <tr>
                    <td>类型编号</td>
                    <td>
                        <dx:ASPxTextBox ID="txtNo" runat="server" Width="170px">
                        </dx:ASPxTextBox>
                    </td>
                    <td>类型名称</td>
                    <td>
                        <dx:ASPxTextBox ID="txtName" runat="server" Width="170px">
                        </dx:ASPxTextBox>
                    </td>
                    <td>
                        <input id="btnQuery" name="btnQuery" type="submit" value="查询" /></td>
                    <td>&nbsp;&nbsp;&nbsp;</td>
                    <td><input id="btnAdd" name="btnAdd" type="submit" value="新增" /></td>
                    <td><input id="btnExportExcel" name="btnExportExcel" type="submit" value="导出Excel" /></td>
                    <td>&nbsp;</td>
                </tr>
            </table>
            <br />
            <dx:ASPxGridView ID="grdData" runat="server" AutoGenerateColumns="False" style="margin-right: 0px"
                 KeyFieldName="ID" OnRowDeleting="grdData_RowDeleting">
                <SettingsPager PageSize="100">
                </SettingsPager>
                <SettingsBehavior ConfirmDelete="True" />
                <Columns>
                    <dx:GridViewDataTextColumn Caption="NO" FieldName="NO" Name="colNO" VisibleIndex="0">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="NAME" FieldName="NAME" Name="colNAME" VisibleIndex="1">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataHyperLinkColumn Caption="修改" VisibleIndex="2" FieldName="ID">
                        <PropertiesHyperLinkEdit NavigateUrlFormatString="SysMenuItemTypeEdit.aspx?ID={0}" Text="修改">
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
