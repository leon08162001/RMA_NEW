<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ucRMASerial_Pick.ascx.vb" Inherits="ascx_ucRMASerial_Pick" %>


<asp:Panel ID="UI_panel" runat="server" style="display:none;position:absolute;width:70%;border:3px solid #808080;">
    <div align="center">
    <fieldset class = "form_div" align="top" style="width: 100%">
    <table id="table2" width="100%" align="center" border="0" cellspacing="1" >
        <tr class="default">
            <td align="left" colspan="4">
                <asp:Label ID="UI_lblShippingSerial" runat="server" Text ="177_Please click check box for item selection and get closed from below lists."></asp:Label>
            </td>
        </tr>
        <tr class="default" valign="top">
            <td align="center">
                <asp:GridView ID="UI_dvSerial" runat ="server" Width = "100%" CellPadding ="0" CellSpacing ="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric">
                    <Columns>
                      <asp:TemplateField>
                         <HeaderStyle Width="5%" Height ="20px" HorizontalAlign="Center"></HeaderStyle>
                         <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                         <HeaderTemplate>
                                <asp:CheckBox ID="UI_CheckGroup" runat="server" AutoPostBack="true" OnCheckedChanged="UI_CheckGroup_CheckedChanged" />
                         </HeaderTemplate>
                         <ItemTemplate>
                                <asp:CheckBox ID="UI_Check" runat="server" AutoPostBack="true" OnCheckedChanged="UI_check_CheckedChanged" />
                                <asp:Label ID ="UI_RMADID" runat ="server" Text='<%# Eval("RMASMD_RMADID") %>' Visible="false"></asp:Label> 
                                <asp:Label ID ="UI_Mark" runat ="server" Text='<%# Eval("RMASMD_oldMark") %>' Visible="false"></asp:Label> 
                         </ItemTemplate>
                      </asp:TemplateField>
                      
                      <asp:BoundField DataField="RMASMD_RMANO" HeaderText="046_RMA Number" HeaderStyle-Width="30%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                      <asp:BoundField DataField="RMASMD_SERIALNO" HeaderText="013_Serial Number" HeaderStyle-Width="32%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                      <asp:BoundField DataField="RMASMD_MODELNO" HeaderText="021_Model Name" HeaderStyle-Width="33%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                     
                    </Columns> 
                    <HeaderStyle CssClass="Text_Head"/>
                    <RowStyle CssClass="TR_1" />
                    <AlternatingRowStyle CssClass="ListRowEven" />
                    <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Center" />
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="UI_cmdCancel" runat ="server" Text = "010_Cancel" CssClass ="Problem_Edit" />
                <asp:Button ID="UI_cmdSubmit" runat ="server" Text = "001_Submit" CssClass ="Confirm_l" Enabled="false" />
                <asp:Label ID="UI_lblCuID" runat="server" Visible="false"></asp:Label>
            </td>
        </tr>
    </table>
    </fieldset>	
    </div>

    <asp:button id="UI_butTarget" Width="0px" Height="0px" runat="server" style="display:none"></asp:button>
    <asp:button id="UI_butOK" Width="0px" Height="0px" runat="server" style="display:none"></asp:button>
</asp:Panel>

<ajaxToolkit:ModalPopupExtender ID="ajModalProgress" TargetControlID="UI_butTarget"  
PopupControlID="UI_panel"  
OkControlID="UI_butOK" 
CancelControlID="UI_cmdCancel"
BackgroundCssClass = "modalBackground"
runat="server">
</ajaxToolkit:ModalPopupExtender>



