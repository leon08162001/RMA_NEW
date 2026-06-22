<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ucPartPic.ascx.vb" Inherits="ascx_ucPartPic" %>


<asp:Panel ID="UI_panel" runat="server" Width="450px" style="display:none;position:absolute">

<fieldset class="form_div" valign="top" style="width:100%">

<table border="0" bgcolor="#E3D8BE" width="100%" id="table2" cellspacing="1" cellpadding="0" class="TableListdownright" align="center" >
    <tr >
        <td align="left" colspan="4"  background="Images/pic_15.gif" class="default">&nbsp;</td>
    </tr>

    <tr>
	    <td align="center" class="default">
            <asp:Image runat="server" ID="UI_imgPic" Width="300px" Height="300px" ImageUrl="~/images/empty.gif" />
        </td>
    </tr>
	
	<tr height ="40" valign ="middle">
	    <td colspan="4" align="center">
	        <asp:Button ID="UI_cmdCancel" runat ="server" Text = "008_Cancel" CssClass ="Problem_Edit" />
	    </td>
	</tr>
</table>

</fieldset>

<asp:button id="UI_butTarget" Width="0px" Height="0px" runat="server" style="display:none"></asp:button>
</asp:Panel>




<ajaxToolkit:ModalPopupExtender ID="ajModalProgress" TargetControlID="UI_butTarget"  
PopupControlID="UI_panel"  
CancelControlID="UI_cmdCancel"
BackgroundCssClass = "modalBackground"
runat="server">
</ajaxToolkit:ModalPopupExtender>

