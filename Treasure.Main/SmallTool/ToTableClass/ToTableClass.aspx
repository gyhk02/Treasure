<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ToTableClass.aspx.cs" Inherits="Treasure.Main.SmallTool.ToTableClass.ToTableClass" %>

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
            <div>命名空间：<asp:TextBox ID="txtNamespace" runat="server" Width="459px">Treasure.Model.Frame</asp:TextBox></div>
            <div>保存路径：<asp:TextBox ID="txtSavePath" runat="server" Width="500px">E:\Win8Expand\c\IISFold\Treasure\Treasure.Model\Frame</asp:TextBox>
                </div>
            <table style="width: 100%;">
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>数据库：<asp:DropDownList ID="ddlDataBase" runat="server" OnSelectedIndexChanged="ddlDataBase_SelectedIndexChanged"></asp:DropDownList>
                        <asp:Button ID="btnConnection" runat="server" Text="连接" OnClick="btnConnection_Click" />
                        <asp:Label ID="lblShowConnectionResult" runat="server" Text=" "></asp:Label>
                        <br />
                        <br />
                        表名：<asp:TextBox ID="txtTableName" runat="server" Width="278px"></asp:TextBox>
                        <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnConfirm" runat="server" Text="生成Model类" OnClick="btnConfirm_Click" />
                        <br />
                        <br />
                        <dx:ASPxGridView ID="grvTableList" runat="server" AutoGenerateColumns="False" KeyFieldName="ID">
                            <SettingsPager Mode="ShowAllRecords">
                            </SettingsPager>
                            <Columns>
                                <dx:GridViewDataTextColumn Caption="表名" FieldName="TableName" ReadOnly="True" VisibleIndex="1">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="描述" FieldName="TableDescription" VisibleIndex="2">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataCheckColumn Caption="选择" FieldName="Selected" Name="colSelected" Visible="false" VisibleIndex="3">
                                </dx:GridViewDataCheckColumn>
                                <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" SelectAllCheckboxMode="Page">
                                </dx:GridViewCommandColumn>
                            </Columns>
                        </dx:ASPxGridView>
                    </td>
                </tr> 
                <tr>
                    <td>&nbsp;</td>
                </tr>
                 <tr>
                    <td>
                        
                        </td>
                </tr>
            </table>
            <div>
                <asp:HiddenField ID="hdnConnection" Value="" runat="server" />
            </div>
        </div>
    </form>
</body>
</html>
