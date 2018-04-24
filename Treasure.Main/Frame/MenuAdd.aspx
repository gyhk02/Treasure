<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuAdd.aspx.cs" Inherits="Treasure.Main.Frame.MenuAdd" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
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
                    <td>父节点：</td>
                    <td><asp:DropDownList ID="ddlParentId" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>名称：</td>
                    <td><asp:TextBox ID="txtName" runat="server" Width="555px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>英文名称：</td>
                    <td><dx:ASPxTextBox ID="txtEnglishName" runat="server" Width="555px">
                        </dx:ASPxTextBox></td>
                </tr> <tr>
                    <td>图片：</td>
                    <td><asp:TextBox ID="txtPictureUrl" runat="server" Width="555px"></asp:TextBox></td>
                </tr> <tr>
                    <td>文件路径：</td>
                    <td><asp:TextBox ID="txtFileUrl" runat="server" Width="555px"></asp:TextBox></td>
                </tr> <tr>
                    <td>按钮名称：</td>
                    <td><asp:TextBox ID="txtButtonName" runat="server" Width="555px"></asp:TextBox></td>
                </tr> <tr>
                    <td>是否启用：</td>
                    <td><asp:DropDownList ID="ddlEnable" runat="server"></asp:DropDownList></td>
                </tr> <tr>
                    <td>是否系统菜单</td>
                    <td><asp:DropDownList ID="ddlIsSys" runat="server"></asp:DropDownList></td>
                </tr> <tr>
                    <td><input id="btnAdd" name="btnAdd" type="submit" value="新增" /><input id="btnEdit" name="btnEdit" type="submit" value="修改" /></td>
                    <td><input id="btnBack" name="btnBack" type="submit" value="返回" /></td>
                </tr> <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr> 
            </table>
        </div>
        <asp:HiddenField ID="hdnMenuId" runat="server" />
    </form>
</body>
</html>
