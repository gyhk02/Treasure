<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataSynchron.aspx.cs" Inherits="Treasure.Main.SmallTool.DataSynchron.DataSynchron" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <table style="width: 100%;">
                <tr>
                    <td style="vertical-align: top;">
                        <table style="width: 100%;">
                            <tr>
                                <td><span style="font-weight: bold;">原数据库</span></td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>类型</td>
                                <td>
                                    <asp:Label ID="lblSourceVersion" runat="server" Text=""></asp:Label>
                                </td>
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
                                <td><span style="font-weight: bold;">目标数据库</span></td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>类型</td>
                                <td>
                                    <asp:Label ID="lblTargetVersion" runat="server" Text=""></asp:Label></td>
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
                    <td style="vertical-align: top;">
                        <asp:Button ID="btnConnection" runat="server" Text="连接数据库" OnClick="btnConnection_Click" />
                        <br />
                        <br />
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </td>
                    <td style="vertical-align: top;">
                        <table style="width: 100%;">
                            <tr>
                                <td>表名：<asp:TextBox ID="txtTableName" runat="server" Width="278px"></asp:TextBox><asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" />
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <dx:ASPxGridView ID="grvTableList" runat="server" AutoGenerateColumns="False">
                                        <SettingsPager Mode="ShowAllRecords">
                                        </SettingsPager>
                                        <Columns>
                                            <dx:GridViewDataTextColumn Caption="表名" FieldName="TableName" ReadOnly="True" VisibleIndex="1">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="描述" FieldName="TableDescription" VisibleIndex="2">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataCheckColumn Caption="选择" FieldName="Selected" Name="colSelected" Visible="False" VisibleIndex="3">
                                            </dx:GridViewDataCheckColumn>
                                            <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0">
                                            </dx:GridViewCommandColumn>
                                        </Columns>
                                    </dx:ASPxGridView>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="vertical-align: top;">
                        <asp:RadioButtonList ID="rblSynchronType" runat="server">
                            <asp:ListItem>表结构同步</asp:ListItem>
                            <asp:ListItem>数据完全同步</asp:ListItem>
                            <asp:ListItem>数据增量同步(按ID)</asp:ListItem>
                        </asp:RadioButtonList>
                        <br />
                        <br />
                        <asp:Button ID="btnSynchron" runat="server" Text="开始同步" OnClick="btnSynchron_Click" OnClientClick="return confirm('开始同步？');" />
                        <br />
                        <br />
                        <asp:Button ID="btnCompare" runat="server" Text="表结构对比" OnClick="btnCompare_Click" />
                        <br />
                        <br />
                        <dx:ASPxGridView ID="grvTableStructure" runat="server">
                            <SettingsPager Mode="ShowAllRecords">
                            </SettingsPager>
                        </dx:ASPxGridView>
                    </td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="hdnSourceConnection" Value="" runat="server" />
        <asp:HiddenField ID="hdnTargetConnection" Value="" runat="server" />
    </form>
</body>
</html>
