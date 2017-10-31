<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Treasure.Main.Default" %>

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
                    <td>小工具：</td>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                    <td><a href="SmallTool/EncryptAndDecrypt/Encrypt_Decrypt.aspx" target="_blank">加密解密</a></td>
                </tr>
                <tr>
                    <td></td>
                    <td><a href="SmallTool/DataSynchron/DataSynchron.aspx" target="_blank">数据同步</a></td>
                </tr>
                <tr>
                    <td></td>
                    <td><a href="Treasure/SecondToDatetime.aspx" target="_blank">时间转换</a></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
