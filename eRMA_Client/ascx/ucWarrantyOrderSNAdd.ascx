<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ucWarrantyOrderSNAdd.ascx.vb" Inherits="ascx_ucWarrantyOrderSNAdd" %>

<asp:Panel ID="UI_panel" runat="server" Width="600px" style="display:none;position:absolute;width:70%;border:3px solid #808080;background:#ffffff;">
    <div align="center">
    <fieldset class = "form_div" align="top" style="width: 100%">
    <table id="table2" width="100%" align="center" border="0" cellspacing="1" class="default">
        <tr class="default">
            <td align="left">
                <asp:Label ID="lblTitle" runat ="server" Text="Serial Number Collection"></asp:Label>
                <asp:Button ID="btnReload" runat="server" Text="Reload" CssClass="Confirm" />   
           </td>
        </tr>
       <asp:Panel ID="pnSerial" runat="server">
        <tr>
             <td align="left">
                Serial No
                <asp:TextBox ID="txtStart" runat="server" Width="150px"></asp:TextBox>
               <asp:TextBox ID="txtEnd" runat="server" Width="150px" Visible="false"></asp:TextBox> 
               <asp:Button ID="btnSnSave" runat="server" Text="Add" CssClass="Confirm" />   
            </td>      
        </tr>
       </asp:Panel>  
       <tr>
            <td>
                 <asp:Label ID="lblMsg" runat="server" Font-Bold="true" ForeColor="red"></asp:Label>
                 <asp:Label ID="lblWoNo" runat="server" Visible="false"></asp:Label>
                 <asp:Label ID="lblOrderNo" runat="server" Visible="false"></asp:Label>
                 <asp:Label ID="lblOrderSeq" runat="server" Visible="false"></asp:Label>
            </td>
       </tr> 
        
        <tr class="default" valign="top">
            <td align="center" class="default">
                <div align="center">
                    <asp:GridView ID="dvSerial" runat ="server" Width = "100%" CellPadding ="0" CellSpacing ="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric"  >
                        <Columns>
                          <asp:TemplateField>
                             <HeaderStyle Width="10%" Height ="20px" HorizontalAlign="Center"></HeaderStyle>
                             <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkAll_CheckedChanged" />
                            </HeaderTemplate> 
                             <ItemTemplate>
                                    <asp:CheckBox ID="chk" runat="server" /> 
                             </ItemTemplate>
                          </asp:TemplateField>
                          
                        <asp:TemplateField HeaderText="Serial Number">
                             <HeaderStyle Width="30%" Height ="20px" HorizontalAlign="Center"></HeaderStyle>
                             <ItemStyle HorizontalAlign="left" CssClass="text9pt" Height="25px"></ItemStyle>
                             <ItemTemplate>
                                    <asp:Label runat="server" ID="lblSn" Text='<%# Eval("wats_sn") %>'></asp:Label>
                                    <asp:Label runat="server" ID="lblwats_mark" Text='<%# Eval("wats_mark") %>' Visible="false"></asp:Label>
                             </ItemTemplate>
                          </asp:TemplateField>                                                                                       
                        </Columns> 
                        <HeaderStyle CssClass="Text_Head"/>
                        <RowStyle CssClass="TR_1" />
                        <AlternatingRowStyle CssClass="ListRowEven" />
                        <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Center" />
                    </asp:GridView>
                </div>
            </td>
        </tr>
        <tr>
            <td align="center">&nbsp;&nbsp;
                <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="Confirm" />
                <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="Confirm" />
                <asp:Button ID="btnConfirm" runat="server" Text="Confirm" visible="false" CssClass="Confirm" />
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
CancelControlID="btnClose"
BackgroundCssClass = "modalBackground"
runat="server">
</ajaxToolkit:ModalPopupExtender>
