<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="Treasure.Main.Test.WebForm1" %>

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
    <dx:ASPxGridView ID="ItemsGrid" runat="server" Width="100%" KeyFieldName="ItemID">

<Columns>

<dx:GridViewDataColumn Caption="Qty" Width="20%" FieldName="Qty">

<DataItemTemplate>

<dx:ASPxTextBox ID="Qutity" runat="server" Width="80%" Text="1">

</dx:ASPxTextBox>

</DataItemTemplate>

</dx:GridViewDataColumn>

<dx:GridViewDataTextColumn Caption="Item" FieldName="Item" Width="50%">

</dx:GridViewDataTextColumn>

<dx:GridViewDataImageColumn Caption="Image" FieldName="PhotoLocation">

<PropertiesImage>

<ExportImageSettings Height="50" Width="50" />

</PropertiesImage>

</dx:GridViewDataImageColumn>

</Columns>

</dx:ASPxGridView>
    </div>
    </form>
</body>
</html>
