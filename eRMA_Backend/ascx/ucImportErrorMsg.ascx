<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ucImportErrorMsg.ascx.vb" Inherits="ascx_ucImportErrorMsg" %>



<asp:Panel ID="UI_panel" runat="server" Width="700px" style="display:none;position:absolute">
    <table id="table1" width="100%" align="center" border="0" cellspacing="1" class="form_div">
        <tr valign="top">
            <td>
                <div class="form_div" align="center" style="width: 98%" >
                <fieldset>
                <table id="table2" width="100%" height="100%" align="center" border="0" cellspacing="1" >
	                <tr class="default">
	                    <td align="center" colspan="4">
	                        <asp:Label ID="UI_lblUploadData" runat="server" Text ="006_Imported Information" Font-Bold ="true" ForeColor ="red"></asp:Label>
	                    </td>
	                </tr>
	                <tr class="default">
	                    <td width="25%" align="left">
	                        <asp:Label ID="UI_lblRmaNo" runat="server" Text="009_Temp No" Font-Bold="true"></asp:Label>.
	                    </td>
	                    <td width="30%" align="left">:
	                        <asp:Label ID="UI_txtRmaNo" runat="server"></asp:Label>
	                    </td>
	                    <td width="15%" align="right">
	                        <asp:Label ID="UI_lblRmaDate" runat="server" Text="010_Import Date" Font-Bold="true"></asp:Label>
	                    </td>
	                    <td width="30%" align="left">:
	                        <asp:Label ID="UI_txtRmaDate" runat="server"></asp:Label>
	                    </td>
	                </tr>
	                <tr class="default">
	                    <td width="25%" align="left">
	                        <asp:Label ID="UI_lblCustomer" runat="server" Text="011_Customer" Font-Bold="true"></asp:Label>.
	                    </td>
	                    <td width="30%" align="left">:
	                        <asp:Label ID="UI_txtCustomer" runat="server"></asp:Label>
	                    </td>
	                    <td width="15%" align="right">
	                        <asp:Label ID="UI_lblStatus" runat="server" Text="012_Status" Font-Bold="true"></asp:Label>
	                    </td>
	                    <td width="30%" align="left">:
	                        <asp:Label ID="UI_txtStatus" runat="server"></asp:Label>
	                    </td>
	                </tr>
		            <tr class="default">
	                    <td align="left">
	                        <asp:Label ID="UI_lblErrMsg" runat="server" Text="013_Error Msg" Font-Bold="true"></asp:Label>
	                    </td>
	                    <td align="left" colspan="3">:
	                        <asp:TextBox ID="UI_txtErrMsg" runat="server" TextMode="multiLine" Height="150px"></asp:TextBox>
	                    </td>
	                </tr>	                
	            </table>
                </fieldset>	
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div align="center"><br />
	                <asp:Button ID="UI_cmdClose" runat ="server" Text = "039_Close Window" CssClass ="Confirm_l" />
	            </div>
            </td>
        </tr>
    </table>

    <asp:button id="UI_butTarget" Width="0px" Height="0px" runat="server" style="display:none"></asp:button>
    <asp:button id="UI_butOK" Width="0px" Height="0px" runat="server" style="display:none"></asp:button>

</asp:Panel>


<ajaxToolkit:ModalPopupExtender ID="ajModalProgress" TargetControlID="UI_butTarget"  
PopupControlID="UI_panel"  
OkControlID="UI_butOK" 
CancelControlID="UI_cmdClose"
BackgroundCssClass = "modalBackground"
runat="server">
</ajaxToolkit:ModalPopupExtender>
