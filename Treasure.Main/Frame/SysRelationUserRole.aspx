<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SysRelationUserRole.aspx.cs" Inherits="Treasure.Main.Frame.SysRelationUserRole" %>

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
                    <td>角色：</td>
                    <td><dx:ASPxLabel ID="lblRole" runat="server" Text=" "></dx:ASPxLabel></td>
                    <td style="width:50px;">&nbsp;</td>
                    <td><input id="btnConfirm" name="btnConfirm" type="submit" value="确定" /></td>
                    <td><input id="btnToBack" name="btnToBack" type="submit" value="返回" /></td>
                    <td>&nbsp;</td>
                </tr>
                </table>
            <br />
            <div>
                <dx:ASPxGridView ID="grdData" runat="server" AutoGenerateColumns="False" KeyFieldName="ID">
                    <SettingsPager Mode="ShowAllRecords">
                    </SettingsPager>
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="用户编号" FieldName="NO" VisibleIndex="2">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="用户名称" FieldName="NAME" VisibleIndex="3">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="登录名" FieldName="LOGIN_NAME" VisibleIndex="4">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewCommandColumn Caption="选择" ShowSelectCheckbox="True" VisibleIndex="1">
                        </dx:GridViewCommandColumn>
                    </Columns>
                </dx:ASPxGridView>
                <asp:HiddenField ID="hdnSysRoleId" runat="server" />
            </div>
        </div>
    </form>
</body>
</html>
