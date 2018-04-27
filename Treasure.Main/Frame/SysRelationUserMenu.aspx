<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SysRelationUserMenu.aspx.cs" Inherits="Treasure.Main.Frame.SysRelationUserMenu" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
 <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td>用户：</td>
                    <td><dx:ASPxLabel ID="lblUser" runat="server" Text=" "></dx:ASPxLabel></td>
                    <td style="width:50px;">&nbsp;</td>
                    <td><input id="btnConfirm" name="btnConfirm" type="submit" value="确定" /></td>
                    <td><input id="btnToBack" name="btnToBack" type="submit" value="返回" /></td>
                    <td>&nbsp;</td>
                </tr>
                </table>
            <br />
            <div>
                <dx:ASPxTreeList ID="treMenu" runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <dx:TreeListTextColumn Caption="名称" FieldName="NAME" VisibleIndex="1">
                        </dx:TreeListTextColumn>
                        <dx:TreeListTextColumn Caption="类型" FieldName="Type" VisibleIndex="3">
                        </dx:TreeListTextColumn>
                    </Columns>
                    <SettingsSelection AllowSelectAll="True" Enabled="True" />
                </dx:ASPxTreeList>
                <asp:HiddenField ID="hdnUserId" runat="server" />
            </div>
        </div>
    </form>
</body>
</html>
