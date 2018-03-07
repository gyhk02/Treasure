<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuItemList.aspx.cs" Inherits="Treasure.Main.Frame.MenuItemList" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>

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
                    <td>名称：</td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server"></asp:TextBox></td>
                    <td>父节点：</td>
                    <td>
                        <asp:DropDownList ID="ddlParent" runat="server"></asp:DropDownList></td>
                    <td>类型：</td>
                    <td>
                        <asp:DropDownList ID="ddlType" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>图片：</td>
                    <td>
                        <asp:TextBox ID="txtPictureUrl" runat="server"></asp:TextBox></td>
                    <td>文件路径：</td>
                    <td>
                        <asp:TextBox ID="txtFileUrl" runat="server"></asp:TextBox></td>
                    <td>按钮名称</td>
                    <td>
                        <asp:TextBox ID="txtButtonName" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>是否启用</td>
                    <td>
                        <asp:DropDownList ID="ddlEnable" runat="server"></asp:DropDownList></td>
                    <td>是否系统菜单</td>
                    <td>
                        <asp:DropDownList ID="ddlIsSys" runat="server"></asp:DropDownList></td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="btnAdd" runat="server" Text="新增" />
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="btnUpdate" runat="server" Text="修改" />
                    </td>
                </tr>
            </table>

            <br />
            <br />
            <dx:ASPxTreeList ID="treMenuItem" runat="server" AutoGenerateColumns="False"
                KeyFieldName="ID" ParentFieldName="PARENT_ID">
                <Columns>
                    <dx:TreeListTextColumn Caption="名称" FieldName="NAME" VisibleIndex="0">
                    </dx:TreeListTextColumn>
                    <dx:TreeListTextColumn Caption="类型" FieldName="MENU_TYPE_NAME" VisibleIndex="1">
                    </dx:TreeListTextColumn>
                    <dx:TreeListTextColumn Caption="图片" FieldName="PICTURE_URL" VisibleIndex="2">
                    </dx:TreeListTextColumn>
                    <dx:TreeListTextColumn Caption="文件" FieldName="FILE_URL" VisibleIndex="3">
                    </dx:TreeListTextColumn>
                    <dx:TreeListTextColumn Caption="按钮" FieldName="BUTTON_NAME" VisibleIndex="4">
                    </dx:TreeListTextColumn>
                    <dx:TreeListTextColumn Caption="启用" FieldName="ENABLE" VisibleIndex="5">
                    </dx:TreeListTextColumn>
                    <dx:TreeListTextColumn Caption="系统菜单" FieldName="IS_SYS" VisibleIndex="6">
                    </dx:TreeListTextColumn>
                </Columns>
            </dx:ASPxTreeList>
        </div>
    </form>
</body>
</html>
