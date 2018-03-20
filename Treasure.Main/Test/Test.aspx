<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="Treasure.Main.Test.Test" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register assembly="DevExpress.Web.ASPxTreeList.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTreeList" tagprefix="dx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>部门维护</title>
 
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    
        <dx:ASPxTreeList ID="ASPxTreeList1" runat="server" AutoGenerateColumns="False" 
        Width="100%" KeyFieldName="DeptID" ParentFieldName="fatherID" 
        AutoGenerateServiceColumns="True" 
        onnodeupdating="ASPxTreeList1_NodeUpdating" 
        onnodedeleting="ASPxTreeList1_NodeDeleting" 
        onnodeinserting="ASPxTreeList1_NodeInserting" 
        onnodevalidating="ASPxTreeList1_NodeValidating" 
            oninitnewnode="ASPxTreeList1_InitNewNode" 
            onhtmlrowprepared="ASPxTreeList1_HtmlRowPrepared" 
            oncelleditorinitialize="ASPxTreeList1_CellEditorInitialize" 
            onfocusednodechanged="ASPxTreeList1_FocusedNodeChanged">
        <SettingsText ConfirmDelete="确认删除吗?" />
        <SettingsSelection AllowSelectAll="True" Recursive="True" />
        <SettingsPager Mode="ShowPager">
        </SettingsPager>
        <Settings GridLines="Both" />
        <SettingsBehavior ExpandCollapseAction="NodeDblClick" AllowFocusedNode="True" 
            AutoExpandAllNodes="True" />
        <SettingsCookies StoreSelection="True" />
        <SettingsEditing Mode="EditFormAndDisplayNode" />
        <SettingsPopupEditForm Width="500" />
        <Columns>
            <dx:TreeListTextColumn Caption="部门名称" FieldName="DeptName" VisibleIndex="0">
            </dx:TreeListTextColumn>
            <dx:TreeListTextColumn Caption="部门编号" FieldName="DeptNum" VisibleIndex="1">
            </dx:TreeListTextColumn>
            <dx:TreeListTextColumn Caption="父编号" FieldName="fatherID" VisibleIndex="3" 
                Visible="False">
            </dx:TreeListTextColumn>
           
            <dx:TreeListCommandColumn Caption="操作" VisibleIndex="4">
                <EditButton Text="修改" Visible="True">
                </EditButton>
                <NewButton Text="新增" Visible="True">
                </NewButton>
                <DeleteButton Text="删除" Visible="True">
                </DeleteButton>
                <UpdateButton Text="更新">
                </UpdateButton>
                <CancelButton Text="取消">
                </CancelButton>
            </dx:TreeListCommandColumn>
        </Columns>
            <ClientSideEvents NodeClick="function(s, e) {
	var a=&quot;&quot;;
}" />
        </dx:ASPxTreeList>
    
    </div>
    </form>
</body>
</html>