<%@ Page Language="VB" AutoEventWireup="false" CodeFile="BossDefault.aspx.vb" Inherits="BossDefault" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title><%=Session("_Title")%></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <link href="script/rma.css" type="text/css" rel="stylesheet">
    <link href="script/styleDate.css" type="text/css" rel="stylesheet">
    <script src="Script/mouseover.js"></script>
</head>
<body>
    <p>&nbsp;</p>
    <p>&nbsp;</p>
    <form id="oForm" runat="server">
        <table align="center" border="0" width="615" id="table1" height="315" cellspacing="0" cellpadding="0">
            <tr>
                <td background="images/login_bg.gif" valign="top">
                    <!--login input -->
                    <div align="right">
                        <table width="365" border="0" cellspacing="0" cellpadding="2" id="table2" class="login" style="margin-top: 115px; margin-left: 20px; text-heigh: 18px">
                            <tr height="35" valign="bottom">
                                <td width="55" class="Login" align="left">Account ID</td>
                                <td width="20">
                                    <asp:TextBox ID="UI_txtAccountID" runat="server" Width="80" Height="13"></asp:TextBox>
                                </td>
                                <td width="160" align="left">&nbsp;</td>
                            </tr>

                            <tr>
                                <td class="Login" align="left">Password</td>
                                <td align="left">
                                    <asp:TextBox ID="UI_txtPassword" runat="server" Width="80" Height="13" TextMode="Password"></asp:TextBox>
                                </td>
                                <td align="left">
                                    <asp:Button ID="UI_cmdLogin" runat="server" Text="Login" />
                                </td>
                            </tr>

                            <tr>
                                <td colspan="3" align="left" class="default" valign="bottom">
                                    <asp:Label ID="UI_lblMessage" runat="server" ForeColor="red" Visible="false"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <!--end of login input -->
                </td>
            </tr>
        </table>
        <asp:Label ID="UI_lblPreviousPage_RMASMID" runat="server" Visible="false"></asp:Label>
    </form>
</body>
</html>
