<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataSynchron.aspx.cs" Inherits="Treasure.Main.SmallTool.DataSynchron.DataSynchron" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../../Script/Js/jquery.min.js"></script>
    <script type="text/javascript">
        function PromptMsg() {
            if ($("#lblTargetVersion").text() == "正式版本") {
                if (confirm('小心操作：要将数据同步到正式版本？') == true) {
                    $("#hdnIsSubmit").val(1);
                } else {
                    $("#hdnIsSubmit").val(0);
                }
            } else {
                if (confirm('开始同步？') == true) {
                    $("#hdnIsSubmit").val(1);
                } else {
                    $("#hdnIsSubmit").val(0);
                }
            }
        }
    </script>
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
                        <br />
                        <br />
                        <br />
                        <br />
                    </td>
                    <td style="vertical-align: top;">
                        <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0">
                            <TabPages>
                                <dx:TabPage Text="表">
                                    <ContentCollection>
                                        <dx:ContentControl runat="server">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td>表名：
                                                        <asp:TextBox ID="txtTableName" runat="server" Width="278px"></asp:TextBox>
                                                        <asp:Button ID="btnTableSearch" runat="server" Text="查询" OnClick="btnTableSearch_Click" />
                                                        <br />
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <dx:ASPxGridView ID="grvTableList" runat="server" AutoGenerateColumns="False" KeyFieldName="ID">
                                                            <SettingsPager Mode="ShowAllRecords">
                                                            </SettingsPager>
                                                            <Columns>
                                                                <dx:GridViewDataTextColumn Caption="表名" FieldName="TableName" ReadOnly="True" VisibleIndex="1">
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="描述" FieldName="TableDescription" VisibleIndex="2">
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" SelectAllCheckboxMode="Page">
                                                                </dx:GridViewCommandColumn>
                                                            </Columns>
                                                        </dx:ASPxGridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </dx:ContentControl>
                                    </ContentCollection>
                                </dx:TabPage>
                                <dx:TabPage Text="存储过程">
                                    <ContentCollection>
                                        <dx:ContentControl runat="server">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td>存储过程：
                                                        <asp:TextBox ID="txtProcedureName" runat="server" Width="278px"></asp:TextBox>
                                                        <asp:Button ID="btnProcedureSearch" runat="server" Text="查询" OnClick="btnProcedureSearch_Click" />
                                                        <br />
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <dx:ASPxGridView ID="grvProcedureList" runat="server" AutoGenerateColumns="False" KeyFieldName="ID">
                                                            <SettingsPager Mode="ShowAllRecords">
                                                            </SettingsPager>
                                                            <Columns>
                                                                <dx:GridViewDataTextColumn Caption="存储过程名称" FieldName="NAME" ReadOnly="True" VisibleIndex="1">
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="描述" FieldName="ProcedureDescription" VisibleIndex="2">
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" SelectAllCheckboxMode="Page">
                                                                </dx:GridViewCommandColumn>
                                                            </Columns>
                                                        </dx:ASPxGridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </dx:ContentControl>
                                    </ContentCollection>
                                </dx:TabPage>
                                <dx:TabPage Text="函数">
                                    <ContentCollection>
                                        <dx:ContentControl runat="server">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td>函数：
                                                        <asp:TextBox ID="txtFunction" runat="server" Width="278px"></asp:TextBox>
                                                        <asp:Button ID="btnFunction" runat="server" Text="查询" OnClick="btnFunction_Click" />
                                                        <br />
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <dx:ASPxGridView ID="grvFunctionList" runat="server" AutoGenerateColumns="False" KeyFieldName="ID">
                                                            <SettingsPager Mode="ShowAllRecords">
                                                            </SettingsPager>
                                                            <Columns>
                                                                <dx:GridViewDataTextColumn Caption="函数名称" FieldName="NAME" ReadOnly="True" VisibleIndex="1">
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="描述" FieldName="FunctionDescription" VisibleIndex="2">
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" SelectAllCheckboxMode="Page">
                                                                </dx:GridViewCommandColumn>
                                                            </Columns>
                                                        </dx:ASPxGridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </dx:ContentControl>
                                    </ContentCollection>
                                </dx:TabPage>
                            </TabPages>
                        </dx:ASPxPageControl>
                    </td>
                    <td style="vertical-align: top;">
                        <asp:RadioButtonList ID="rblSynchronType" runat="server">
                            <asp:ListItem>表结构同步</asp:ListItem>
                            <asp:ListItem>数据完全同步</asp:ListItem>
                            <asp:ListItem>数据增量同步(按ID)</asp:ListItem>
                            <asp:ListItem>存储过程同步</asp:ListItem>
                            <asp:ListItem>函数同步</asp:ListItem>
                        </asp:RadioButtonList>
                        <br />
                        <br />
                        <asp:Button ID="btnSynchron" runat="server" Text="开始同步" OnClick="btnSynchron_Click" OnClientClick="return PromptMsg(); " />
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
        <asp:HiddenField ID="hdnIsSubmit" Value="0" runat="server" />
    </form>
</body>
</html>
