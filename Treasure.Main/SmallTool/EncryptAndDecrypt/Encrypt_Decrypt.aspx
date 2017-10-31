<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Encrypt_Decrypt.aspx.cs" Inherits="Treasure.Main.SmallTool.EncryptAndDecrypt.Encrypt_Decrypt" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            类型
        <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
        </asp:DropDownList>
            <br />
            Key：<asp:TextBox ID="txtKey" runat="server"></asp:TextBox>
            <br />
            <br />
            原字符串：<br />
            <asp:TextBox ID="txtTop" runat="server" Height="100px" TextMode="MultiLine" Width="800px"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="btnEncrypt" runat="server" Text="加密" OnClick="btnEncrypt_Click" />
            &nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnDecrypt" runat="server" Text="解密" OnClick="btnDecrypt_Click" />
            <br />
            <br />
            目标字符串：<br />
            <asp:TextBox ID="txtBottom" runat="server" Height="100px" TextMode="MultiLine" Width="800px"></asp:TextBox>

        </div>
    </form>
</body>
</html>
