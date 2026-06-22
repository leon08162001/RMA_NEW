<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ForgetPassword.aspx.vb" Inherits="ForgetPassword" %>

<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=Session("_Title")%></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <link href="script/rma.css" type="text/css" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager" runat="server" EnablePageMethods="true" EnableScriptGlobalization="true"></asp:ScriptManager>

        <p>&nbsp;</p>
        <p>&nbsp;</p>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
            <ContentTemplate>

                <table align="center" border="0" width="615" id="table1" height="315" cellspacing="0" cellpadding="0">
                    <tr>
                        <td background="images/bg_ForgotPassword.gif" valign="top">
                            <!--login input -->
                            <div align="center">
                                <table width="390" border="0" cellspacing="0" cellpadding="2" id="table2" class="login" style="margin-top: 95px; margin-left: 20px; margin-right: 20px; text-heigh: 18px">
                                    <tr>
                                        <td colspan="3" class="default">
                                            <asp:Label runat="server" ID="UI_lblForgetTitle" Text="054_ForgetTitle"></asp:Label></td>
                                    </tr>

                                    <tr>
                                        <td width="80" class="default" align="right"><font face="Arial Unicode MS">
                                            <asp:Label runat="server" ID="UI_lblAccountID" Text="055_AccountID"></asp:Label></font></td>
                                        <td width="150" align="left">
                                            <asp:TextBox ID="UI_txtAccountID" runat="server" Width="80" CssClass="Login" MaxLength="20"></asp:TextBox>
                                        </td>
                                        <td align="left">&nbsp; </td>
                                    </tr>

                                    <tr>
                                        <td class="default" align="right"><font face="Arial Unicode MS">
                                            <asp:Label runat="server" ID="UI_lblEmail" Text="056_Email"></asp:Label></font></td>
                                        <td align="left">
                                            <asp:TextBox ID="UI_txtMail" runat="server" Width="180" CssClass="Login" MaxLength="200"></asp:TextBox>
                                        </td>

                                        <td align="left" width="106" class="default">
                                            <asp:Button ID="UI_cmdsubmit" runat="server" Text="001_submit" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="3">&nbsp;</td>
                                    </tr>

                                    <tr>
                                        <td height="44" colspan="3" align="center" class="default" valign="bottom">&nbsp;</td>
                                    </tr>
                                </table>
                            </div>
                            <!--end of login input -->

                        </td>
                    </tr>
                </table>

                <uc3:ucMessage ID="ucMessage" runat="server" />

            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="UI_cmdsubmit" EventName="Click" />
                <asp:PostBackTrigger ControlID="ucMessage" />
            </Triggers>
        </asp:UpdatePanel>

    </form>
</body>
</html>
