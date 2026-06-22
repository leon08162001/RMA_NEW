<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ucWarrantyOrderCust.ascx.vb" Inherits="ascx_ucWarrantyOrderCust" %>

<asp:Panel ID="UI_panel" runat="server" Width="500px" style="display:none;position:absolute">
    <div align="center">

    <fieldset class = "form_div" align="top" style="width: 100%">
    <table border="0" width="95%" id="table1" cellspacing="1" cellpadding="0" class="default" align="center" style="margin-top:30px;">
		    <tr>
		      <td align="left">
		           <asp:Label ID="lblCustomer" runat="server" Text="Customer No"></asp:Label>:
                  <asp:TextBox ID="txtCustomer" runat="server" Width="150px"></asp:TextBox> 
                   <asp:Button ID="UI_cmdSearch" runat="server" Text="Search" CssClass="Search"></asp:Button>
             </td>
		   </tr>
    </table>  
   
    <table id="table2" width="100%" align="center" border="0" cellspacing="1" >
        <tr class="default">
            <td align="left">
                <asp:Label ID="UI_lblAddressTittle" runat ="server" Text="Please pick one customer from below lists and get window closed."></asp:Label>
            </td>
        </tr>
        
        
        <tr class="default" valign="top">
            <td align="center">
                <div align="center">
                    <asp:GridView ID="UI_dvAddress" runat ="server" Width = "100%" CellPadding ="0" CellSpacing ="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric"  >
                        <Columns>
                          <asp:TemplateField>
                             <HeaderStyle Width="10%" Height ="20px" HorizontalAlign="Center"></HeaderStyle>
                             <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                             <ItemTemplate>
                                    <asp:RadioButton runat="server" ID="raoAddress" GroupName="G1" />
                             </ItemTemplate>
                          </asp:TemplateField>
                          
                        <asp:TemplateField HeaderText="Customer ID">
                             <HeaderStyle Width="30%" Height ="20px" HorizontalAlign="left"></HeaderStyle>
                             <ItemStyle HorizontalAlign="left" CssClass="text9pt" Height="25px"></ItemStyle>
                             <ItemTemplate>
                                    <asp:Label runat="server" ID="UI_CUNo" Text='<%# Eval("CU_NO") %>'></asp:Label>
                             </ItemTemplate>
                          </asp:TemplateField>        
 
                          
                        <asp:TemplateField HeaderText="Customer Name">
                             <HeaderStyle Width="60%" Height ="20px" HorizontalAlign="left"></HeaderStyle>
                             <ItemStyle HorizontalAlign="left" CssClass="text9pt" Height="25px"></ItemStyle>
                             <ItemTemplate>
                                    <asp:Label runat="server" ID="UI_CUName" Text='<%# Eval("CU_NAME") %>'></asp:Label>
                                    <asp:Label runat="server" ID="lblCurrency" Visible="false" Text='<%# Eval("occ42") %>'></asp:Label>
                                    <asp:Label runat="server" ID="lblOrderType" Visible="false" Text='<%# Eval("occ43") %>'></asp:Label>
                                    <asp:Label runat="server" ID="lblSalesID" Visible="false" Text='<%# Eval("occ04") %>'></asp:Label>
                                    <asp:Label runat="server" ID="lblSalesName" Visible="false" Text='<%# Eval("gen02") %>'></asp:Label>
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
                <asp:Button ID="UI_cmdClose" runat="server" Text="_Close" CssClass="Problem_Edit" />
                <asp:Button ID="UI_cmdSubmit" runat="server" Text="_Submit" CssClass="Confirm_l" />
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
CancelControlID="UI_cmdClose"
BackgroundCssClass = "modalBackground"
runat="server">
</ajaxToolkit:ModalPopupExtender>
