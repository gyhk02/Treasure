
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SysUser.aspx.cs" Inherits="Treasure.Main.ProjectCollection.SystemSetup.SysUser" %>

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
            <table >
                <tr>
                        
                    <td>工号</td>
                    <td>
                        <dx:ASPxTextBox ID="txtNO" runat="server" Width="170px"></dx:ASPxTextBox>
                    </td>                        
                    <td>姓名</td>
                    <td>
                        <dx:ASPxTextBox ID="txtNAME" runat="server" Width="170px"></dx:ASPxTextBox>
                    </td>                        
                    <td>登陆名</td>
                    <td>
                        <dx:ASPxTextBox ID="txtLOGIN_NAME" runat="server" Width="170px"></dx:ASPxTextBox>
                    </td>                    <td>                        <input id = "btnQuery" name = "btnQuery" type = "submit" value = "查询" /></td>                     <td> &nbsp; &nbsp; &nbsp;</td> 
                    <td><input id="btnAdd" name="btnAdd" type="submit" value="新增" /></td>

                    <td>&nbsp;</td>
                </tr>
            </table>
            <br />
            <dx:ASPxGridView ID="grdData" runat="server" AutoGenerateColumns="False" style="margin-right: 0px"
                 KeyFieldName="ID" OnRowDeleting="grdData_RowDeleting">
                <SettingsPager PageSize="20">
                </SettingsPager>
                <SettingsBehavior ConfirmDelete="True" />
                <Columns>
                    <dx:GridViewDataTextColumn Caption="工号" FieldName="NO" Name="colNO" VisibleIndex="0"></dx:GridViewDataTextColumn>                    <dx:GridViewDataTextColumn Caption="姓名" FieldName="NAME" Name="colNAME" VisibleIndex="0"></dx:GridViewDataTextColumn>                    <dx:GridViewDataTextColumn Caption="登陆名" FieldName="LOGIN_NAME" Name="colLOGIN_NAME" VisibleIndex="0"></dx:GridViewDataTextColumn>                    <dx:GridViewDataTextColumn Caption="密码" FieldName="PASSWORD" Name="colPASSWORD" VisibleIndex="0"></dx:GridViewDataTextColumn>                    <dx:GridViewDataTextColumn Caption="性别" FieldName="SEX" Name="colSEX" VisibleIndex="0"></dx:GridViewDataTextColumn>                    <dx:GridViewDataTextColumn Caption="邮件地址" FieldName="EMAIL" Name="colEMAIL" VisibleIndex="0"></dx:GridViewDataTextColumn>                    
                    <dx:GridViewDataTextColumn Caption="账户过期日期" Name = "colEXPIRED_DATE" FieldName="EXPIRED_DATE" VisibleIndex="0">
                        <PropertiesTextEdit DisplayFormatString = "yyyy-MM-dd"></PropertiesTextEdit>
                     </dx:GridViewDataTextColumn>
                    <dx:GridViewDataHyperLinkColumn Caption="修改" VisibleIndex="2" FieldName="ID">
                        <PropertiesHyperLinkEdit NavigateUrlFormatString="SysUserEdit.aspx?ID={0}" Text="修改">
                        </PropertiesHyperLinkEdit>
                    </dx:GridViewDataHyperLinkColumn>
                    <dx:GridViewCommandColumn Caption="删除"
                        ShowDeleteButton="True" VisibleIndex="3">
                    </dx:GridViewCommandColumn>
                </Columns>
            </dx:ASPxGridView>
            <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server" GridViewID="grdData">
            </dx:ASPxGridViewExporter>
            <asp:HiddenField ID="hidCondition" runat="server" />
        </div>
    </form>
</body>
</html>
