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
            <a href="MenuAdd.aspx">新增菜单</a>
            <br /><br />
            <dx:ASPxTreeList ID="treMenuItem" runat="server" AutoGenerateColumns="False"
                 KeyFieldName="ID" ParentFieldName="PARENT_ID" OnNodeDeleting="treMenuItem_NodeDeleting">
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
                    <dx:TreeListHyperLinkColumn Caption="修改" FieldName="ID" VisibleIndex="7">
                        <PropertiesHyperLink NavigateUrlFormatString="MenuAdd.aspx?ID={0}" Text="修改">
                        </PropertiesHyperLink>
                    </dx:TreeListHyperLinkColumn>
                    <dx:TreeListCommandColumn Caption="删除" VisibleIndex="8">
                        <DeleteButton Text="删除" Visible="True">
                        </DeleteButton>
                    </dx:TreeListCommandColumn>
                </Columns>
                <SettingsBehavior AllowFocusedNode="True" />
            </dx:ASPxTreeList>
            <br />
        </div>
    </form>
</body>
</html>
