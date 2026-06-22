<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Customer_Search.aspx.vb" Inherits="Customer_Search" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">

    <table border="0" cellspacing="0" cellpadding="0" width="100%" height="410px">
        <tr height="90px">
            <td width="24" background="Images/pic_12.gif">&nbsp;</td>
            <td valign="top" align="left">
                <table border="0" width="100%" cellspacing="0" cellpadding="0">
                    <!--[Begin]Tittle-->
                    <tr>
                        <td width="3%">&nbsp;</td>
                        <td width="94%" align="left">
                            <asp:Label ID="UI_lblTittle" runat="server" Text="001_Setting - Customer" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
                        </td>
                        <td width="3%">&nbsp;</td>
                    </tr>
                    <!--[End]Tittle-->
                    <!--[Begin]資料查詢條件區-->
                    <tr>
                        <td>&nbsp;</td>
                        <td align="left">
                            <table width="95%" border="0" cellspacing="1" cellpadding="0" class="default">
                                <tr>
                                    <td colspan="6">
                                        <asp:Label ID="UI_lblCustomerSearching" runat="server" Text="002_Customer Searching" Font-Bold="true"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="10%">
                                        <asp:Label ID="UI_lblCustomerID" runat="server" Text="003_Customer ID"></asp:Label>
                                    </td>
                                    <td width="20%">:
			                            <asp:TextBox ID="UI_txtCustomerID" runat="server" Width="80"></asp:TextBox>
                                    </td>
                                    <td width="10%" align="left">
                                        <asp:Label ID="UI_lblCustomerName" runat="server" Text="004_Customer Name"></asp:Label>
                                    </td>
                                    <td width="20%" align="left">:
			                            <asp:TextBox ID="UI_txtCustomerName" runat="server" Width="100"></asp:TextBox>
                                    </td>
                                    <td width="10%"></td>
                                    <td width="25%"></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="UI_lblStatus" runat="server" Text="005_Status"></asp:Label>
                                    </td>
                                    <td>:
			                            <asp:DropDownList ID="UI_cboStatus" runat="server">
                                            <asp:ListItem Value="-1" Text="006_- Select -" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="007_Open"></asp:ListItem>
                                            <asp:ListItem Value="0" Text="008_Close"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td align="left">&nbsp;
			                        <asp:Label ID="UI_lblCountry" runat="server" Text="013_Country"></asp:Label>
                                    </td>
                                    <td>:
			                        <asp:DropDownList ID="UI_cboCountry" runat="server"></asp:DropDownList>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="UI_lblRepairCenter" runat="server" Text="009_Repair Center"></asp:Label>
                                    </td>
                                    <td align="left">:
			                            <asp:DropDownList ID="UI_cboRepairCenter" runat="server"></asp:DropDownList>&nbsp;&nbsp;
	                                    <asp:Button ID="UI_cmdSearch" runat="server" Text="_Search" CssClass="Search" />
                                        <asp:Button ID="Excel_btn" runat="server" Text="產生EXCEL" CssClass="Search"></asp:Button>
                                        <asp:Label ID="Download_Lab" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <!--[End]資料查詢條件區-->
                </table>
            </td>
        </tr>

        <tr height="20px">
            <td width="24" background="Images/pic_14.gif">&nbsp;</td>
            <td background="Images/pic_15.gif">
                <table border="0" width="100%" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="1%">&nbsp;</td>
                        <td width="99%" align="left" class="default">
                            <asp:Label ID="UI_lblCustomerTittle" runat="server" Text="010_Customer Information" Font-Bold="true"></asp:Label>
                            <asp:Button ID="UI_cmdAdd" runat="server" Text="_Add" CssClass="Confirm_l" PostBackUrl="~/Customer.aspx" Width="100" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>

        <tr height="250px" valign="top">
            <td background="Images/pic_20.gif">&nbsp;</td>
            <td valign="top" bgcolor="#E3D8BE" align="center">
                <table border="0" width="100%" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="1%">&nbsp;</td>
                        <td width="99%" align="left" class="default">
                            <!--[Begin]資料列表表單-->
                            <div align="center">
                                <asp:GridView ID="UI_Customer" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric" AllowSorting="true">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="UI_SeqID" runat="server"></asp:Label>
                                                <asp:Label ID="UI_CuNo" runat="server" Text='<%# Eval("CU_NO") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="CU_NO" SortExpression="CU_NO" HeaderText="003_Customer ID" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="CU_NAME" SortExpression="CU_NAME" HeaderText="004_Customer Name" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="CU_TEL" SortExpression="CU_TEL" HeaderText="011_TEL" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="COMP_NAME" SortExpression="COMP_NAME" HeaderText="009_Repair Center" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="005_Status" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>

                                        <asp:TemplateField HeaderText="012_Manager">
                                            <HeaderStyle Width="12%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Button ID="UI_cmdEdit" runat="server" Text="_Edit" CssClass="Problem_Edit" CommandName="cmdEdit" CommandArgument='<%# me.UI_Customer.Rows.Count%>' PostBackUrl="Customer.aspx" />
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
    </table>

    <asp:Label runat="server" ID="UI_lblPreviousPage_CuNo" Visible="false"></asp:Label>
</asp:Content>

