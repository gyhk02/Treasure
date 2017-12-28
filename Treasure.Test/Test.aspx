<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="Treasure.Test.Test" %>

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

            <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="1">
                <TabPages>
                    <dx:TabPage>
                        <ContentCollection>
                            <dx:ContentControl runat="server">
                                <dx:ASPxGridView ID="ASPxGridView1" runat="server">
                                </dx:ASPxGridView>
                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:TabPage>
                    <dx:TabPage>
                        <ContentCollection>
                            <dx:ContentControl runat="server">
                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:TabPage>
                </TabPages>
            </dx:ASPxPageControl>





        </div>
    </form>
</body>
</html>
