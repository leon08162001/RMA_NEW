<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>
        <%=Session("_Title")%></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <link href="script/rma.css" type="text/css" rel="stylesheet">
    <link href="script/styleDate.css" type="text/css" rel="stylesheet">
    <script src="Script/mouseover.js"></script>
    <style type="text/css">
        .auto-style2 {
            width: 118px;
        }
        .auto-style3 {
            width: 625px;
        }
        .auto-style5 {
            COLOR: black;
            FONT-SIZE: 12px;
            font-family: "Arial";
            TEXT-DECORATION: none;
            LETTER-SPACING: 0px;
            LINE-HEIGHT: 22px;
            width: 204px;
            height: 6px;
        }
        .auto-style7 {
            margin-top: 25px;
            height: 271px;
        }
        .auto-style8 {
            COLOR: black;
            FONT-SIZE: 11px;
            font-family: "Arial";
            TEXT-DECORATION: none;
            LETTER-SPACING: 0px;
            LINE-HEIGHT: 18px;
            height: 18px;
            width: 474px;
        }
        .auto-style9 {
            width: 474px;
            height: 6px;
        }
        .auto-style10 {
            COLOR: black;
            FONT-SIZE: 12px;
            font-family: "Arial";
            TEXT-DECORATION: none;
            LETTER-SPACING: 0px;
            LINE-HEIGHT: 22px;
            width: 474px;
            height: 41px;
        }
        .auto-style12 {
            width: 204px;
            height: 10px;
        }
        .auto-style15 {
            height: 43px;
        }
        .auto-style16 {
            COLOR: black;
            FONT-SIZE: 11px;
            font-family: "Arial";
            TEXT-DECORATION: none;
            LETTER-SPACING: 0px;
            LINE-HEIGHT: 18px;
            height: 43px;
            width: 474px;
        }
        .auto-style17 {
            width: 204px;
            height: 43px;
        }
        .auto-style18 {
            COLOR: black;
            FONT-SIZE: 12px;
            font-family: "Arial";
            TEXT-DECORATION: none;
            LETTER-SPACING: 0px;
            LINE-HEIGHT: 22px;
            height: 41px;
        }
        .auto-style19 {
            width: 102px;
            height: 6px;
        }
        .auto-style20 {
            width: 17px;
            height: 17px;
            margin-top: 4px;
        }
        .auto-style22 {
            width: 102px;
            height: 10px;
        }
        .auto-style23 {
            COLOR: black;
            FONT-SIZE: 11px;
            font-family: "Arial";
            TEXT-DECORATION: none;
            LETTER-SPACING: 0px;
            LINE-HEIGHT: 18px;
            height: 10px;
            width: 474px;
        }
    </style>
</head>
<body>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <form id="oForm" runat="server">
    <table align="center" border="0" id="table1" height="315" cellspacing="0"
        cellpadding="0" class="auto-style3">
        <tr>
            <td background="images/login_bg.gif" valign="top">
                <!--login input -->
                <table width="615" border="0" cellspacing="0" cellpadding="2" id="table2" class="auto-style7"
                    style="text-heigh: 18px">
                    <%--           		    <tr>
					    <td>&nbsp;</td>
					    <td>&nbsp;</td>
					    <td>&nbsp;</td>
					</tr>
                    --%>
                    <%--           			<tr height="43" valign="bottom">
           			    <td width="55" class="Login">Compay ID</td>
           			    <td width="45">
           			        <asp:TextBox ID="UI_txtCompanyID" runat="server" Width ="100" Height ="13"></asp:TextBox>
           			    </td>
						<td align="left">&nbsp;</td>
					</tr>
                    --%>
                    <!--JW/shaili -->
                    <tr height="80" valign="top">
                        <td class="auto-style8" align="right">
                        </td>
                        <td align="left" colspan="3">
                            <font size="2"><span style="background-color: #ffffff; color: #ff0000;"><br />
                            <asp:Label ID="UI_lblMessage" runat="server" ForeColor="red" Visible="false" Text="050_051_Message"></asp:Label>
                            <br />
                            <br />
                            </span></font></td>
                        <%--<td width="200" align="left"><asp:LinkButton runat="server" ID="UI_linkRigister" Text="Apply for New Account" PostBackUrl="~/forgetpassword.aspx"></asp:LinkButton></td>--%>
                    </tr>
                    <tr valign="bottom">
                        <td class="auto-style16" align="right">
                            Account ID
                        </td>
                        <td class="auto-style17">
                            <asp:TextBox ID="UI_txtAccountID" runat="server" Width="80" Height="13"></asp:TextBox>
                        </td>
                        <td align="left" colspan="2" class="auto-style15">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style23" align="right">
                            Password
                        </td>
                        <td align="left" class="auto-style12">
                            <asp:TextBox ID="UI_txtPassword" runat="server" Width="80" Height="13" TextMode="Password"></asp:TextBox>
                        &nbsp;
                            <asp:Button ID="UI_cmdLogin" runat="server" Text="Login" />
                        </td>
                        <td align="left" class="auto-style22">
                            </td>
                        <td rowspan="2" class="auto-style2">
                            &nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style9">
                            &nbsp;
                        </td>
                        <td align="left" class="auto-style5">
                            <asp:LinkButton runat="server" ID="UI_linkForget" Text="053_Forget Password" PostBackUrl="~/forgetpassword.aspx"></asp:LinkButton>
                        </td>
                        <td class="auto-style19">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="auto-style10" >
                            <img src="\images\login.gif" alt="" class="auto-style20" />
                            <asp:LinkButton runat="server" ID="UI_linkRigister" Text="Register for Reseller/End User Account"
                                Font-Size="Small" OnClientClick='return confirm("Your CipherLab device must have been registered with comprehensive warranty before you can apply for an Reseller/End User Account for direct RMA service.")'></asp:LinkButton>
                            <br />
                            <font size="2"><span style="color: #ff0000;">(Only for Comprehensive Warranty User)</span></font></td>

                         <td colspan="3" class="auto-style18" >
                             <br />
                             <img src="\images\km.ico" alt="" class="auto-style20" />
                            <asp:LinkButton runat="server" ID="UI_linkEndUser" Text="User Guide for Reseller/End User"
                                Font-Size="Small"></asp:LinkButton><br />
                             <img src="\images\km.ico" alt="" class="auto-style20" />
                             <asp:LinkButton runat="server" ID="UI_linkDist" Text="User Guide for Distributor"
                                Font-Size="Small"></asp:LinkButton>

                             <br />
                             <img src="\images\POLICY.ico" alt="" class="auto-style20" />
                            <asp:HyperLink ID="UI_linkPrivacy_policy1" runat="server" Target="_blank"
                                NavigateUrl="~/Privacy_policy.aspx" Font-Size="Small">Privacy Policy</asp:HyperLink>

                        </td>
                    </tr>
                </table>
                <!--end of login input -->
            </td>
        </tr>
    </table>

         <div style="text-align: center;"><span style="color: #ff0000;"><strong> *By signing up CipherLab E-RMA system, you acknowledge that you consent to CipherLab&#39;s Privacy Policy. You can withdraw your consent at any time.</strong></span>
         </div>
    </form>
</body>
</html>
