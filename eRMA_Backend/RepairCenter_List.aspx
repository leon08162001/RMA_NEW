<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="RepairCenter_List.aspx.vb" Inherits="RepairCenter_List" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">

    <table border="0" cellspacing="0" cellpadding="0" width="100%" height="100%">
        <tr height="10%">
            <td width="24" background="Images/pic_12.gif">&nbsp;</td>
            <td valign="top" align="left">
                <table border="0" width="100%" cellspacing="0" cellpadding="0">
                    <!--[Begin]Tittle-->
                    <tr>
                        <td width="3%">&nbsp;</td>
                        <td width="94%" align="left">
                            <asp:Label ID="UI_lblTittle" runat="server" Text="001_Setting - Repair Center" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
                        </td>
                        <td width="3%">&nbsp;</td>
                    </tr>
                    <!--[End]Tittle-->
                </table>
            </td>
        </tr>
        <%--		<tr>
		    <td background="Images/pic_12.gif">&nbsp;</td>
		    <td>&nbsp;</td>
		</tr>
        --%>
        <tr>
            <td width="24" background="Images/pic_14.gif">&nbsp;</td>
            <td background="Images/pic_15.gif">
                <table border="0" width="100%" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="1%">&nbsp;</td>
                        <td width="98%" align="left" class="default">
                            <asp:Label ID="UI_lblRepairCenterTittle" runat="server" Text="002_Repair Center Information " Font-Bold="true"></asp:Label>
                            <asp:Button ID="UI_cmdAdd" runat="server" Text="_Add New Center" CssClass="Confirm_l" PostBackUrl="RepairCenter.aspx" />
                        </td>
                        <td width="1%">&nbsp;</td>
                    </tr>
                </table>
            </td>
        </tr>

        <tr>
            <td background="Images/pic_20.gif">&nbsp;</td>
            <td valign="top" bgcolor="#E3D8BE" align="center">
                <table border="0" width="100%" cellspacing="0" cellpadding="0" height="100%">
                    <tr>
                        <td width="1%">&nbsp;</td>
                        <td width="99%" align="left" class="default" valign="top">
                            <!--[Begin]資料列表表單-->
                            <div align="center">
                                <asp:GridView ID="UI_RepairCenter" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric" AllowSorting="true">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="UI_SeqID" runat="server"></asp:Label>
                                                <asp:Label ID="UI_CompNo" runat="server" Text='<%# Eval("COMP_NO") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="COMP_NO" SortExpression="COMP_NO" HeaderText="003_Center Code" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="COMP_NAME" SortExpression="COMP_NAME" HeaderText="004_Center Name" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="COMP_TEL" SortExpression="COMP_TEL" HeaderText="005_TEL" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="COMP_CURRENCYCODE" SortExpression="COMP_CURRENCYCODE" HeaderText="006_Currency" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="COMP_STOCKMANAGER" SortExpression="COMP_STOCKMANAGER" HeaderText="044_COMP_STOCKMANAGER" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="007_Status" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>


                                        <asp:TemplateField HeaderText="008_Repair Center / Part's Maintain">
                                            <HeaderStyle Width="17%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Button ID="UI_cmdEdit" runat="server" Text="_Edit" CssClass="Problem_Edit" CommandName="cmdEdit" CommandArgument='<%# me.UI_RepairCenter.Rows.Count%>' PostBackUrl="~/RepairCenter.aspx" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="Text_Head" />
                                    <RowStyle CssClass="TR_1" />
                                    <AlternatingRowStyle CssClass="ListRowEven" />
                                    <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Center" />
                                </asp:GridView>
                            </div>
                            <!--[End]資料列表表單-->
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td background="Images/pic_20.gif">&nbsp;</td>
            <td valign="top" bgcolor="#E3D8BE" align="center">
                <!--[Begin]頁數-->
                <table border="0" width="100%" cellspacing="0" cellpadding="0">
                    <tr valign="middle">
                        <td width="3%">&nbsp;</td>
                        <td width="94%">&nbsp</td>
                        <td width="3%">&nbsp;
		                    <asp:Label runat="server" ID="UI_lblPreviousPage_CompNo" Visible="false"></asp:Label>
                        </td>
                    </tr>
                </table>
                <!--[End]頁數-->
            </td>
        </tr>
    </table>

</asp:Content>

