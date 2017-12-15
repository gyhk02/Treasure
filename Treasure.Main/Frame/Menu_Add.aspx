<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu_Add.aspx.cs" Inherits="Treasure.Main.Frame.Menu_Add" %>

<%@ Register assembly="DevExpress.Web.ASPxTreeList.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTreeList" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <dx:ASPxTreeList ID="treMenu" runat="server" AutoGenerateColumns="False" KeyFieldName="ID" ParentFieldName="PARENT_ID">
            <Columns>
                <dx:TreeListTextColumn Caption="菜单" FieldName="NAME" VisibleIndex="0">
                </dx:TreeListTextColumn>
            </Columns>
        </dx:ASPxTreeList>
    
    </div>
    </form>
</body>
</html>
