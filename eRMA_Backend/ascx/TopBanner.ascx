<%@ Control Language="VB" AutoEventWireup="false" CodeFile="TopBanner.ascx.vb" Inherits="ascx_TopBanner" %>

<td colspan="2">
    <table border="0" width="100%" id="table3" cellspacing="0" cellpadding="0">
        <tr>
            <td>
                <asp:HyperLink runat="server" id="UI_linLogin" NavigateUrl="~/Default.aspx">
                <asp:Image ID="UI_imgLogin" runat ="server" ImageUrl="~/Images/pic_01.gif" ImageAlign="AbsMiddle"/>
                </asp:HyperLink>
            </td>

		    <td align="right" valign="top" class="default">&nbsp;
		    	  
		        <asp:LinkButton runat="server" ID="UI_hrefLogout"><u>Logout</u><img border="0" src="Images/logout.gif" width="16" height="16" align="middle"></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
		        <br />
		        </><asp:LinkButton runat="server" ID="UI_linkUserGuide" Text="User Guide&nbsp;&nbsp;&nbsp;&nbsp;" Font-Size="Small"></asp:LinkButton><br />
		    </td>

        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
    </table>
</td>

