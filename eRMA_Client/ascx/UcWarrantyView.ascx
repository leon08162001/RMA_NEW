<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UcWarrantyView.ascx.vb" Inherits="ascx_UcWarrantyView" %>


<style type="text/css">
    .auto-style1 {
        width: 100%;
    }
    .auto-style2 {
        width: 100%;
    }
    .auto-style3 {
        position: absolute;
        left: 10px;
        top: 15px;
        width: 100%;
    }
</style>

<asp:Panel ID="UI_panel" runat="server">

                <div class="auto-style2" >
    
                <table id="table2" border="0"  class="auto-style1" >  
	                <tr class="default">
	                    <td >
	                        <asp:Label ID="Label1" runat="server" Text="Warranty : " Font-Bold="true"></asp:Label>
	                    </td>
	                </tr>
	                <tr class="default" >
	                    <td >
                                                <asp:GridView ID="dgvWarrantyType" runat="server"  PagerSettings-Mode="Numeric" AllowPaging="True" AutoGenerateColumns="False" border="0"
                                                    CellSpacing="1" CellPadding="0" AllowSorting="True" Height="10px" HeaderStyle-Font-Size="8px" Font-Size="XX-Small" Font-Names="Arial"  >
                                                    <Columns>
                                                        <asp:BoundField DataField="War_Type" HeaderText="Warranty Type"
                                                            HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="5%" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="PROGRAM_TYPE_NAME" HeaderText="PROGRAM TYPE"
                                                            HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="10%" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ITEM_TYPE_NAME" HeaderText="ITEM TYPE"
                                                            HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="10%" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="PRICE_VER_NAME" HeaderText="PRICE VER"
                                                            HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="10%"  />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="WAR_SPEC_DESC" HeaderText="SPEC DESC"
                                                            HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="10%"  />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <asp:Label runat="server" ID="UI_NotFound" Text='<%# Session("_isShowNotFound").ToString()%>'></asp:Label>
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

              </asp:Panel> 


