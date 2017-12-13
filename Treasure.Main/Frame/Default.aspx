<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Treasure.Main.Frame.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" href="../Script/Css/Basic.css" />
    <script src="../Script/Js/jquery.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table id="theTable" style="width: 100%; border-collapse: collapse; border-spacing: 0; border: 0px;">
                <tr>
                    <td colspan="2" style="height: 50px; background-color:gray;">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 300px; background-color:GrayText">
                        <iframe id="frmLeft" name="frmLeft" src="Left.aspx" frameborder="0" style="border: 0px; width:100%;"></iframe>
                    </td>
                    <td style="background-color:aqua;">
                        <iframe id="frmMain" name="frmMain" src="Main.aspx" frameborder="0" style="border: 0px; width:100%;"></iframe>
                    </td>
                </tr>
            </table>
        </div>
    </form>
    <script type="text/javascript">
        $(window).bind('resize', function () {
            var windowHeight = $(window).height();
            var windowWidth = $(window).width();

            $("#theTable").height(windowHeight);
            $("#frmLeft").height(windowHeight - 50 - 10);
            $("#frmMain").height(windowHeight - 50 - 10);

            //$("#frmCenter").width($(window).width() - 200 - 5);
        });

        $(function () {
            var windowHeight = $(window).height();
            var windowWidth = $(window).width();

            $("#theTable").height(windowHeight);
            $("#frmLeft").height(windowHeight - 50 - 10);
            $("#frmMain").height(windowHeight - 50 - 10);

            //$("#frmCenter").width($(window).width() - 200 - 5);
        });
    </script>
</body>
</html>
