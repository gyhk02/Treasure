<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataSynchron.aspx.cs" Inherits="Treasure.Web.AboutDataBase.DataSynchron.DataSynchron" %>

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
    
        <table style="width:100%;">
            <tr>
                <td style="vertical-align:top;">
                    <table style="width:100%;">
                        <tr>
                            <td><span style="font-weight:bold;">原数据库</span></td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>数据库选项</td>
                            <td>
                                <asp:DropDownList ID="ddlSourceDb" runat="server" AutoPostBack="True" Height="16px" OnSelectedIndexChanged="ddlSourceDb_SelectedIndexChanged">
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
                            <td><span style="font-weight:bold;">目标数据库</span></td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>数据库选项</td>
                            <td>
                                <asp:DropDownList ID="ddlTargetDb" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTargetDb_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>IP</td>
                            <td>
                                <asp:TextBox ID="txtTargetIp" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>登录名</td>
                            <td>
                                <asp:TextBox ID="txtTargetLoginName" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>密码</td>
                            <td>
                                <asp:TextBox ID="txtTargetPwd" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>数据库名</td>
                            <td>
                                <asp:TextBox ID="txtTargetDbName" runat="server"></asp:TextBox>
                            </td>
                        </tr>
 <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
 
                    </table>
                </td>
                <td style="vertical-align:top;">
                    <asp:Button ID="btnConnection" runat="server" Text="连接数据库" OnClick="btnConnection_Click" />
                    <br /><br />
                    <asp:Label ID="lblConnectionError" runat="server"></asp:Label>
                </td>
                <td style="vertical-align:top;">
                    <table style="width:100%;">
                        <tr>
                            <td>表名：<asp:TextBox ID="txtTableName" runat="server"></asp:TextBox><asp:Button ID="btnSearch" runat="server" Text="查询" />
                                <br /> <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <dx:ASPxGridView ID="grvData" runat="server">
                                </dx:ASPxGridView>
                            </td>
                        </tr>
                    </table>
                </td>
                 <td style="vertical-align:top;">
<asp:RadioButtonList ID="rblSynchronType" runat="server">
    <asp:ListItem>完全同步</asp:ListItem>
    <asp:ListItem>增量同步</asp:ListItem>
                     </asp:RadioButtonList>
                     <br />
                     <br />
                     <asp:Button ID="btnSynchron" runat="server" Text="开始同步" />
                 </td>
            </tr>
        </table>
    
    </div>
        <asp:HiddenField ID="hdnSourceConnection" runat="server" />
        <asp:HiddenField ID="hdnTargetConnection" runat="server" />
    </form>
</body>
</html>
