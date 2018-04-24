<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Treasure.Main.Frame.Login" %>

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
        <div style="background-color: green; height: 60px;"></div>
        <div class="bigDiv" style="height: 30px; width: 600px;">
            用户名：<dx:ASPxTextBox ID="txtUserName" runat="server" Width="170px" Text="Admin"></dx:ASPxTextBox>
            <br />
            <br />
            密码：<dx:ASPxTextBox ID="txtPwd" runat="server" Width="170px" Text="abcd.1234"></dx:ASPxTextBox>
            <br />
            <br />
            <asp:Button ID="btnLogin" runat="server" Text="登录" OnClick="btnLogin_Click" />&nbsp;&nbsp;&nbsp;
        </div>
        <div style="background-color: grey; height: 120px; position: absolute; width: 100%; bottom: 0px;"></div>
    </form>
</body>
</html>
