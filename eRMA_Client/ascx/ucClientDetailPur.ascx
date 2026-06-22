<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ucClientDetailPur.ascx.vb" Inherits="ascx_ucClientDetailPur" %>


<asp:Panel ID="UI_panel" runat="server" style="display:none;position:absolute;width:70%;border:3px solid #808080;background:#ffffff;" >
    <table id="table1" class="table"   align="center" border="0" cellspacing="1" >
        <tr valign="top">
            <td>
                <div class="form_div" align="center" >
                <fieldset>
                <table id="table2" class="table"  style="width:100%;" align="center" border="0" cellspacing="1" >
	                <tr class="default">
	                    <td align="left" colspan="4" >
	                        <asp:Label ID="Label1" runat="server" Text="Purchasing Records : " Font-Bold="true"></asp:Label>
	                    </td>
	                </tr>
	                <tr class="default" valign="top">
	                    <td align="left" colspan="4">
                                                <asp:GridView ID="dgvPurchasing" runat="server" Cssclass="table"  PagerSettings-Mode="Numeric" AllowPaging="true" AutoGenerateColumns="False" border="0"         CellSpacing="1" CellPadding="0" AllowSorting="true">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="UI_SEQID" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="SerialNo" HeaderText="SerialNo"
                                                            HeaderStyle-Width="12%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="PurchaseDate" HeaderText="PurchaseDate"
                                                            HeaderStyle-Width="12%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="WarrantyCode" HeaderText="WarrantyCode"
                                                            HeaderStyle-Width="12%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="StartDate" HeaderText="StartDate"
                                                            HeaderStyle-Width="12%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="EndDate" HeaderText="EndDate"
                                                            HeaderStyle-Width="12%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <asp:Label runat="server" ID="UI_NotFound" Text='<%# Session("_isShowNotFound").tostring()%>'></asp:Label>
                                                    </EmptyDataTemplate>
                                                    <EmptyDataRowStyle HorizontalAlign="Center" />
                                                    <HeaderStyle CssClass="Text_Head" />
                                                    <RowStyle CssClass="TR_1" />
                                                    <AlternatingRowStyle CssClass="ListRowEven" />
                                                    <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Center" />
                                                </asp:GridView>
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
