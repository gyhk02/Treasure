﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QueryUsedTables.aspx.cs" Inherits="Treasure.Main.SmallTool.DataSynchron.QueryUsedTables" %>

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

            <table class="auto-style1">
                <tr>
                    <td style="vertical-align: top;">
                        <table style="width: 100%;">
                            <tr>
                                <td><span style="font-weight: bold;">数据库</span></td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>数据库选项</td>
                                <td>
                                    <asp:DropDownList ID="ddlSourceDb" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSourceDb_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>IP</td>
                                <td>
                                    <asp:TextBox ID="txtSourceIp" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>登录名</td>
                                <td>
                                    <asp:TextBox ID="txtSourceLoginName" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>密码</td>
                                <td>
                                    <asp:TextBox ID="txtSourcePwd" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>数据库名</td>
                                <td>
                                    <asp:TextBox ID="txtSourceDbName" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Button ID="btnConn" runat="server" Text="测试数据库连接" OnClick="btnConn_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblShowConnectionStatus" runat="server" Text="显示数据库链接情况"></asp:Label>
                                </td>
                            </tr>

                        </table>
                    </td>
                    <td style="vertical-align: top;">
                        <table class="auto-style1">
                            <tr>
                                <td>
                                    <asp:Button ID="btnGetProcedure" runat="server" Text="获取存储过程列表" OnClick="btnGetProcedure_Click" />
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:DropDownList ID="ddlProcedure" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlProcedure_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtCode" runat="server" Height="214px" Width="837px" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnQuery" runat="server" OnClick="btnQuery_Click" Text="查看用到哪些表" />
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnClear" runat="server" Text="清空表名" OnClick="btnClear_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtTables" runat="server" Width="604px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

        </div>
    </form>
</body>
</html>
