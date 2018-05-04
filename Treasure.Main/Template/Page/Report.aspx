<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="Treasure.Main.Template.Page.Report" %>

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
                    <td>名称</td>
                    <td>
                        <asp:TextBox ID="txtNAME" runat="server"></asp:TextBox>
                    </td>
                    <td>序号</td>
                    <td>
                        <asp:TextBox ID="txtID_INDEX" runat="server"></asp:TextBox>
                    </td>
                    <td>系统参数?</td>
                    <td>
                        <asp:DropDownList ID="ddlIS_SYS" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>创建日期</td>
                    <td>
                        <dx:ASPxDateEdit ID="datCREATE_DATETIME_FROM" runat="server">
                        </dx:ASPxDateEdit>
                    </td>
                    <td>-</td>
                    <td>
                        <dx:ASPxDateEdit ID="datCREATE_DATETIME_TO" runat="server">
                        </dx:ASPxDateEdit>
                    </td>
                    <td>
                        <input id="btnQuery" name="btnQuery" type="submit" value="查询" /></td>
                    <td>
                        <input id="btnExcel" name="btnExcel" type="submit" value="导出Excel" /></td>
                    <td>&nbsp;</td>
                </tr>
            </table>
            <br />
            <dx:ASPxGridView ID="grdData" runat="server" AutoGenerateColumns="False" KeyFieldName="T_ID">
                <Columns>
                    <dx:GridViewDataTextColumn Caption="名称" FieldName="NAME" VisibleIndex="0">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="序号" FieldName="ID_INDEX" VisibleIndex="1">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="是系统？" FieldName="IS_SYS_STR" VisibleIndex="2">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="创建日期" FieldName="CREATE_DATETIME" VisibleIndex="3">
                    </dx:GridViewDataTextColumn>
                </Columns>
            </dx:ASPxGridView>
            <dx:ASPxGridViewExporter ID="expData" runat="server" GridViewID="grdData">
            </dx:ASPxGridViewExporter>
        </div>
    </form>
</body>
</html>
