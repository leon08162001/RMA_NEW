<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ucShippingSerial_pick.ascx.vb" Inherits="ascx_ucShippingSerial_pick" %>

<asp:Panel ID="UI_panel" runat="server" Width="90%" style="display:none;position:absolute">
    <div align="center">
    <fieldset class = "form_div" align="top" style="width: 100%">
    <table id="table2" width="100%" align="center" border="0" cellspacing="1" >
        <tr class="default">
            <td align="left" colspan="4">
                <asp:Label ID="UI_lblShippingSerial" runat="server" Text ="204_Please click check box for packs selection from shipment notice  and get closed from below lists."></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <!--[Begin]AddShipping列表表單-->
                <table width="98%" CellPadding ="0" CellSpacing ="1" border="0" class="default" >
                    <tr align="center" class="Text_Head">
                        <td width="14%">
                            <asp:Label id="UI_lblCtnNo" runat ="server" Text="163_Ctn. & No" Font-Bold="true"></asp:Label>
                        </td>
                        <td width="41%">
                            <asp:Label id="UI_lblDescription" runat ="server" Text="099_Description" Font-Bold="true"></asp:Label>
                        </td>
                        <td width="16%">
                            <asp:Label id="UI_lblNetWeight" runat ="server" Text="165_Net Weight" Font-Bold="true"></asp:Label>
                        </td>
                        <td width="16%">
                            <asp:Label id="UI_lblGrossWeigh" runat ="server" Text="166_Gross Weigh" Font-Bold="true"></asp:Label>
                        </td>
                        <td width="16%">
                            <asp:Label id="UI_lblMeasurement" runat ="server" Text="167_Measurement" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr class="TR_1">
                        <td>
                            <asp:TextBox ID="UI_txtCtnNo" runat="server" Width ="80px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="UI_txtDescription" runat="server" TextMode="MultiLine" Rows="3" Columns="30"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="UI_txtNetWeight" runat="server" Width ="80px" Text="0"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="UI_txtGrossWeigh" runat="server" Width ="80px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="UI_txtMeasurement" runat="server" Width ="100px"></asp:TextBox>
                        </td>
                    </tr>
                </table>                          
                <!--[End]AddShipping列表表單-->
            </td>
        </tr>
        <tr class="default">
            <td align="left">&nbsp;&nbsp;
                <asp:Label ID="UI_lblCustomer" runat="server" Text="030_Customer"></asp:Label>:
                <asp:Label ID="UI_txtCustomer" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="default" valign="top">
            <td align="center">
                <asp:GridView ID="UI_dvShipping" runat ="server" Width = "100%" CellPadding ="0" CellSpacing ="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric">
                    <Columns>
                      <asp:TemplateField>
                         <HeaderStyle Width="5%" Height ="20px" HorizontalAlign="Center"></HeaderStyle>
                         <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                         <HeaderTemplate>
                                <asp:CheckBox ID="UI_CheckGroup" runat="server" AutoPostBack="true" OnCheckedChanged="UI_CheckGroup_CheckedChanged" />
                         </HeaderTemplate>
                         <ItemTemplate>
                                <asp:CheckBox ID="UI_Check" runat="server" AutoPostBack="true" OnCheckedChanged="UI_check_CheckedChanged" />
                                <asp:Label ID ="UI_PACKINGNO" runat ="server" Text='<%# Eval("RMASM_PACKINGNO") %>' Visible="false"></asp:Label> 
                                <asp:Label ID ="UI_RMANO" runat ="server" Text='<%# Eval("RMASMD_RMANO") %>' Visible="false"></asp:Label> 
                                <asp:Label ID ="UI_QUANTITY" runat ="server" Text='<%# Eval("Qty") %>' Visible="false"></asp:Label> 
                                <asp:Label ID ="UI_Mark" runat ="server" Text='<%# Eval("oldMark") %>' Visible="false"></asp:Label> 
                         </ItemTemplate>
                      </asp:TemplateField>
                      
                      <asp:BoundField DataField="RMASM_PACKINGNO" HeaderText="145_Notice No." HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                      <asp:BoundField DataField="RMASMD_RMANO" HeaderText="029_RMA No." HeaderStyle-Width="12%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                      <asp:BoundField DataField="RMASM_SHIPNO" HeaderText="187_Along with Goods" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                      <asp:BoundField DataField="Qty" HeaderText="188_Numbers" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                      <asp:BoundField DataField="RMASMD_SERIALNO" HeaderText="013_Serial Number" HeaderStyle-Width="62%" ItemStyle-HorizontalAlign="Left" HtmlEncode="false" ></asp:BoundField>
                     
                    </Columns> 
                    <HeaderStyle CssClass="Text_Head"/>
                    <RowStyle CssClass="TR_1" />
                    <AlternatingRowStyle CssClass="ListRowEven" />
                    <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Center" />
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="center">&nbsp;&nbsp;
                <asp:Button ID="UI_cmdCancel" runat ="server" Text = "010_Cancel" CssClass ="Problem_Edit" />
                <asp:Button ID="UI_cmdSubmit" runat ="server" Text = "001_Submit" CssClass ="Confirm_l" ValidationGroup="ShipmentGroup" Enabled="false"/>
            </td>
        </tr>
    </table>
    </fieldset>	
    </div>

    <asp:button id="UI_butTarget" Width="0px" Height="0px" runat="server" style="display:none"></asp:button>
    <asp:button id="UI_butOK" Width="0px" Height="0px" runat="server" style="display:none"></asp:button>
</asp:Panel>

<asp:RequiredFieldValidator ID="rfv_txtNetWeight" runat="server" ErrorMessage = "_必須填寫NetWeight" Display="None" ControlToValidate="UI_txtNetWeight" ValidationGroup ="ShipmentGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
<asp:RangeValidator ID="rv_txtNetWeight" runat="server" ControlToValidate="UI_txtNetWeight" ErrorMessage="_NetWeight輸入格式有誤" Display="None" MinimumValue="0" MaximumValue="10000" Type="Double" SetFocusOnError="true" ValidationGroup="ShipmentGroup" ></asp:RangeValidator>

<asp:ValidationSummary ID="vsShipment" runat ="server" ShowMessageBox ="true" ShowSummary ="false" ValidationGroup="ShipmentGroup" />  


<ajaxToolkit:ModalPopupExtender ID="ajModalProgress" TargetControlID="UI_butTarget"  
PopupControlID="UI_panel"  
OkControlID="UI_butOK" 
CancelControlID="UI_cmdCancel"
BackgroundCssClass = "modalBackground"
runat="server">
</ajaxToolkit:ModalPopupExtender>