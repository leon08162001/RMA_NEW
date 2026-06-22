<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ucSubstitute.ascx.vb" Inherits="ascx_ucSubstitute" %>


<asp:Panel ID="UI_panel" runat="server" Width="500px" style="display:none;position:absolute">

<fieldset class="form_div" valign="top" style="width:100%">

<table border="0" bgcolor="#E3D8BE" width="100%" id="table2" cellspacing="1" cellpadding="0" class="TableListdownright" align="center" >
    <tr >
        <td align="left" colspan="4"  background="Images/pic_15.gif" class="default">&nbsp;
            <asp:Label ID="UI_lblReportTittle" runat="server" Text="030_Report Information" Font-Bold ="true"></asp:Label>
        </td>
    </tr>

    <tr>
	    <td align="left" class="default">
	    
	    
            <asp:GridView ID="UI_dvSubstitute" runat ="server" Width="100%" CellPadding ="0" CellSpacing ="1" border="0" CssClass="default" AutoGenerateColumns="False"  AllowPaging="true" PagerSettings-Mode="Numeric">
                <Columns>
                  <asp:TemplateField>
                     <HeaderStyle Width="35px" Height="15px" HorizontalAlign="Center"></HeaderStyle>
                     <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                     <ItemTemplate>
                            <asp:Label ID ="UI_SeqID" runat = "server" ></asp:Label>
                     </ItemTemplate>
                  </asp:TemplateField>

                  <asp:BoundField DataField="bmd04" ItemStyle-Width="150px" HeaderText="165_166_取替代料件" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                  <asp:BoundField DataField="RPBOM_DESC" HeaderText="161_Description" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                </Columns> 

                <HeaderStyle CssClass="Text_Head"/>
                <RowStyle CssClass="TR_1" />
                <AlternatingRowStyle CssClass="ListRowEven" />
                <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Center" />
            </asp:GridView>	    
	    
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

