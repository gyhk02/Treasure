<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SysReportEdit.aspx.cs" Inherits="Treasure.Main.Frame.SysReportEdit" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../Script/Js/jquery.min.js"></script>
    <script type="text/javascript">
        function ShowAddOrEdit(buttonName) {
            if (buttonName == "btnAdd") {
                $("#btnAdd").show();
                $("#btnEdit").hide();
            }
            else {
                $("#btnAdd").hide();
                $("#btnEdit").show();
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td>原SQL</td>
                    <td>
                        <asp:TextBox ID="txtSourceSQL" runat="server" Height="179px" Width="600px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>中文标题</td>
                    <td>
                        <asp:TextBox ID="txtCnTitle" runat="server" Width="484px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>英文标题</td>
                    <td>
                        <asp:TextBox ID="txtEnTitle" runat="server" Width="484px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:CheckBox ID="chkExcel" runat="server" Text="导出Excel" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="chkPage" runat="server" Text="显示分页" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <input id="btnNext" name="btnNext" type="submit" value="转下一步" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <input id="btnComplete" name="btnComplete" type="submit" value="完成" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <input id="btnBack" name="btnBack" type="submit" value="返回" />
                    </td>
                </tr>
            </table>
            <br />
            <dx:ASPxGridView ID="grdData" runat="server" AutoGenerateColumns="False" KeyFieldName="ID">
                <SettingsPager Mode="ShowAllRecords">
                </SettingsPager>
                <Columns>
                    <dx:GridViewDataTextColumn Caption="列英文名" VisibleIndex="0" FieldName="NAME">
                        <DataItemTemplate>
                            <dx:ASPxTextBox ID="txtNAME" Width="150px" runat="server"></dx:ASPxTextBox>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="列中文名" VisibleIndex="1" FieldName="CN_NAME">
                        <DataItemTemplate>
                            <dx:ASPxTextBox ID="txtCN_NAME" Width="150px" runat="server"></dx:ASPxTextBox>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="类型" VisibleIndex="2" FieldName="COL_DATA_TYPE">
                        <DataItemTemplate>
                            <dx:ASPxTextBox ID="txtCOL_DATA_TYPE" Width="50px" runat="server"></dx:ASPxTextBox>

                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="查询" VisibleIndex="3" FieldName="IS_QUERY">
                        <DataItemTemplate>
                            <dx:ASPxTextBox ID="txtIS_QUERY" Width="50px" runat="server"></dx:ASPxTextBox>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="排序" VisibleIndex="4" FieldName="SORT_RULE">
                        <DataItemTemplate>
                            <dx:ASPxTextBox ID="txtSORT_RULE" Width="50px" runat="server"></dx:ASPxTextBox>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="排序序号" VisibleIndex="5" FieldName="SORT_INDEX">
                        <DataItemTemplate>
                            <dx:ASPxTextBox ID="txtSORT_INDEX" Width="50px" runat="server"></dx:ASPxTextBox>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="小数位数" VisibleIndex="6" FieldName="DECIMAL_DIGITS">
                        <DataItemTemplate>
                            <dx:ASPxTextBox ID="txtDECIMAL_DIGITS" Width="50px" runat="server"></dx:ASPxTextBox>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                </Columns>
            </dx:ASPxGridView>

        </div>
        <asp:HiddenField ID="hdnID" runat="server" />
        <asp:HiddenField ID="hdnReportId" runat="server" />
    </form>
</body>
</html>
