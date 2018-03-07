<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Left.aspx.cs" Inherits="Treasure.Main.Frame.Left" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" href="../Script/Css/Basic.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <dx:ASPxTreeList ID="treMenu" runat="server" AutoGenerateColumns="False" KeyFieldName="ID"
                ParentFieldName="PARENT_ID">
                <Columns>
                    <dx:TreeListTextColumn Caption="菜单" FieldName="NAME" VisibleIndex="0">
                        <DataCellTemplate>
                            <%# Eval("TypeName").ToString() == "页面" ? "<a href='" + Eval("FILE_URL") + "?PageId=" + Eval(Treasure.Model.General.GeneralVO.id) + "' target='frmMain'>" + Eval(Treasure.Model.General.GeneralVO.name) + "</a>" : Eval(Treasure.Model.General.GeneralVO.name) %>
                            <%--<%# Eval(Treasure.Model.General.GeneralVO.Name) %>--%>
                        </DataCellTemplate>
                    </dx:TreeListTextColumn>
                </Columns>
            </dx:ASPxTreeList>
        </div>
        <asp:HiddenField ID="hdnProjectId" runat="server" />
    </form>
</body>
</html>
