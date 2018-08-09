<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Entity.aspx.cs" Inherits="Treasure.Main.Entity" %>

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
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td></td>
                    <td><a href="SmallTool/EncryptAndDecrypt/Encrypt_Decrypt.aspx" target="_blank">加密解密</a></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td></td>
                    <td><a href="SmallTool/DataSynchron/DataSynchron.aspx" target="_blank">数据同步</a></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td></td>
                    <td><a href="Treasure/SecondToDatetime.aspx" target="_blank">时间转换</a></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td><a href="SmallTool/DataSynchron/QueryUsedTables.aspx" target="_blank">查询表名</a></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td><a href="SmallTool/ToTableClass/ToTableClass.aspx" target="_blank">生成Model</a></td>
                    <td>&nbsp;</td>
                </tr> <tr>
                    <td>&nbsp;</td>
                    <td><a href="SmallTool/AutoGenerateFile/GenerateByType.aspx" target="_blank">生成文件</a></td>
                    <td>&nbsp;</td>
                </tr> <tr>
                    <td>&nbsp;</td>
                    <td><a href="SmallTool/AutoGenerateReport/GenerateReport.aspx" target="_blank">生成报表</a></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>EVQ</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td></td>
                    <td><a href="SmallTool/evq/MianSynchron.aspx" target="_blank">Mian项目菜单同步(正式->测试)</a></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>框架：</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td><a href="Frame/Default.aspx" target="_blank">框架首页</a></td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
