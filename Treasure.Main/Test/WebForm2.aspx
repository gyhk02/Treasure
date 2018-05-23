<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="Treasure.Main.Test.WebForm2" %>

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
        <input id="btnSave" name="btnSave" type="submit" value="保存" />
        <dx:ASPxGridView ID="grvData" runat="server" KeyFieldName="ID" AutoGenerateColumns="False">
            <Columns>
                <dx:GridViewDataTextColumn  Caption="NO" FieldName="NO" VisibleIndex="1" Name="colNO">
                    <DataItemTemplate>
                        <dx:ASPxTextBox ID="txtNo" runat="server" Width="170px"></dx:ASPxTextBox>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="NAME" FieldName="NAME" VisibleIndex="2" Name="colNAME">
                     <DataItemTemplate>
                        <dx:ASPxTextBox ID="txtName" runat="server" Width="170px"></dx:ASPxTextBox>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="TITLE" FieldName="TITLE" VisibleIndex="3" Name="colTITLE">
                     <DataItemTemplate>
                        <dx:ASPxTextBox ID="txtTitle" runat="server" Width="170px"></dx:ASPxTextBox>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" Name="colID" VisibleIndex="0" Width="0px" ReadOnly="True">
                    <DataItemTemplate>
                        <dx:ASPxTextBox ID="txtId" runat="server"></dx:ASPxTextBox>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
            </Columns>            
        </dx:ASPxGridView>
    
    </div>
    </form>
</body>
</html>
