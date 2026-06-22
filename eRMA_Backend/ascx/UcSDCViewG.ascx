<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UcSDCViewG.ascx.vb" Inherits="ascx_UcSDCViewG" %>


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
	                        <asp:Label ID="Label1" runat="server" Text="SDC Records : " Font-Bold="true"></asp:Label>
	                    </td>
	                </tr>
	                <tr class="default" >
	                    <td >
                                                <asp:GridView ID="dgvSDC" runat="server"  PagerSettings-Mode="Numeric" AllowPaging="True" AutoGenerateColumns="False" border="0"
                                                    CellSpacing="1" CellPadding="0" AllowSorting="True" Height="50px" Font-Size="X-Small"  >
                                                    <Columns>
                                                        <asp:BoundField DataField="AKEY" HeaderText="AKEY"
                                                            HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Height="5px" Width="5%" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="AST_TYPENAME" HeaderText="AST TYPENAME"
                                                            HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="12%" Height="5px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="part_nm" HeaderText="Part Name"
                                                            HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="12%" Height="5px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="part_desc" HeaderText="Description"
                                                            HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="12%" Height="5px" />
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


