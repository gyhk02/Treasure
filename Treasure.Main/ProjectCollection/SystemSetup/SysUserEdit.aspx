
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SysUserEdit.aspx.cs" Inherits="Treasure.Main.ProjectCollection.SystemSetup.SysUserEdit" %>

<%@ Register assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../../Script/Js/jquery.min.js"></script>
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
                    <td>工号：</td>
                    <td>
                        <dx:ASPxTextBox ID="txtNO" runat="server" Width="170px">
                        </dx:ASPxTextBox>
                    </td>
                </tr>
                <tr>
                    <td>姓名：</td>
                    <td>
                        <dx:ASPxTextBox ID="txtNAME" runat="server" Width="170px">
                        </dx:ASPxTextBox>
                    </td>
                </tr>
                <tr>
                    <td>登陆名：</td>
                    <td>
                        <dx:ASPxTextBox ID="txtLOGIN_NAME" runat="server" Width="170px">
                        </dx:ASPxTextBox>
                    </td>
                </tr>
                <tr>
                    <td>密码：</td>
                    <td>
                        <dx:ASPxTextBox ID="txtPASSWORD" runat="server" Width="170px">
                        </dx:ASPxTextBox>
                    </td>
                </tr>
                <tr>
                    <td>性别：</td>
                    <td>
                        <dx:ASPxTextBox ID="txtSEX" runat="server" Width="170px">
                        </dx:ASPxTextBox>
                    </td>
                </tr>
                <tr>
                    <td>邮件地址：</td>
                    <td>
                        <dx:ASPxTextBox ID="txtEMAIL" runat="server" Width="170px">
                        </dx:ASPxTextBox>
                    </td>
                </tr>
                <tr>
                    <td>账户过期日期：</td>
                    <td>
                        <dx:ASPxDateEdit ID="datEXPIRED_DATE" runat="server"></dx:ASPxDateEdit>
                     </td> 
                 </tr>
            </table>
            <table >
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <input id="btnAdd" name="btnAdd" type="submit" value="新增" />
                        <input id="btnEdit" name="btnEdit" type="submit" value="修改" /></td>
                    <td>
                        <input id="btnBack" name="btnBack" type="submit" value="返回" /></td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="hdnID" runat="server" />
    </form>
</body>
</html>
