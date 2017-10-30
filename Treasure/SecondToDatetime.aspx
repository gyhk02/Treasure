<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SecondToDatetime.aspx.cs" Inherits="Treasure.Web.Treasure.SecondToDatetime" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        New Erp时间：<asp:TextBox ID="txtSecond" runat="server"></asp:TextBox>
        <asp:Button ID="btnCalculation" runat="server" OnClick="btnCalculation_Click" Text="计算" />
        <br />
        <br />
        时间：<asp:TextBox ID="txtDatetime" runat="server"></asp:TextBox>
    
    </div>
    </form>
</body>
</html>
