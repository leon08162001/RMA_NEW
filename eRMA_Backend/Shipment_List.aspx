<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Shipment_List.aspx.vb" Inherits="Shipment_List" Title="Untitled Page" %>


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
                            <asp:Label ID="UI_lblTittle" runat="server" Text="146_Wait for Processing-Shipment Notice" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
                        </td>
                        <td width="3%">&nbsp;</td>
                    </tr>
                    <!--[End]Tittle-->
                    <!--[Begin]資料查詢條件區-->
                    <tr>
                        <td>&nbsp;</td>
                        <td align="left">

                            <asp:Panel runat="server" ID="UI_panSearch" DefaultButton="UI_cmdSearch">
                                <table width="95%" border="0" cellspacing="1" cellpadding="0" class="default">
                                    <tr>
                                        <td colspan="4">
                                            <asp:Label ID="UI_lblShipmentInformation" runat="server" Text="143_Search for Shipment Notice Items."></asp:Label>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td width="15%">
                                            <asp:Label ID="UI_lblRMANo" runat="server" Text="029_Notice Number"></asp:Label>
                                        </td>
                                        <td width="35%" colspan="3">:
			                            <asp:TextBox ID="UI_txtRMANo" runat="server" Width="150"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td width="15%">
                                            <asp:Label ID="UI_lblNotice" runat="server" Text="144_Notice Number"></asp:Label>
                                        </td>
                                        <td width="35%">:
			                            <asp:TextBox ID="UI_txtNotice" runat="server" Width="150"></asp:TextBox>
                                        </td>
                                        <td width="12%" align="right">
                                            <asp:Label ID="UI_lblCustomer" runat="server" Text="030_Customer"></asp:Label>
                                        </td>
                                        <td width="30%" align="left">:
			                            <asp:TextBox ID="UI_txtCustomer" runat="server" Width="120"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="UI_lblTracking" runat="server" Text="138_Tracking Number"></asp:Label>
                                        </td>
                                        <td>:
			                            <asp:TextBox ID="UI_txtTracking" runat="server" Width="150"></asp:TextBox>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="UI_lblSerialNumber" runat="server" Text="013_Serial Number"></asp:Label>
                                        </td>
                                        <td align="left">:
			                            <asp:TextBox ID="UI_txtSerialNumber" runat="server" Width="120"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="UI_lblNoticeDate" runat="server" Text="139_Notice Date"></asp:Label>
                                        </td>
                                        <td align="left" colspan="3">:
			                            <asp:DropDownList ID="UI_cboBYear" runat="server"></asp:DropDownList>
                                            <asp:DropDownList ID="UI_cboBMonth" runat="server"></asp:DropDownList>
                                            <asp:DropDownList ID="UI_cboBDay" runat="server"></asp:DropDownList>&nbsp;~&nbsp;
                                        <asp:DropDownList ID="UI_cboEYear" runat="server"></asp:DropDownList>
                                            <asp:DropDownList ID="UI_cboEMonth" runat="server"></asp:DropDownList>
                                            <asp:DropDownList ID="UI_cboEDay" runat="server"></asp:DropDownList>
                                            <asp:Button ID="UI_cmdSearch" runat="server" Text="004_Search" CssClass="Search" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>

                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="3">&nbsp;</td>
                    </tr>
                    <!--[End]資料查詢條件區-->
                </table>
            </td>
        </tr>
        <tr height="28px">
            <td width="24" background="Images/pic_14.gif">&nbsp;</td>
            <td background="Images/pic_15.gif">
                <table border="0" width="100%" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="1%">&nbsp;</td>
                        <td width="20%" align="left" class="default">
                            <asp:Label ID="UI_lblShippingTittle" runat="server" Text="142_Shipping Information" Font-Bold="true"></asp:Label>
                        </td>
                        <td width="79%" align="left" class="default">
                            <asp:Button ID="UI_cmdShippingAdd" runat="server" CssClass="Confirm_l" Text="043_Add Ship Notice" PostBackUrl="Shipment_Notice.aspx" />
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
                                <asp:GridView ID="UI_dvShipping" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric" AllowSorting="true">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="UI_SeqID" runat="server"></asp:Label>
                                                <asp:Label ID="UI_RMASMID" runat="server" Text='<%# Eval("RMASM_ID") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="RMASMD_RMANO" SortExpression="RMASMD_RMANO" HeaderText="029_RMA No." HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="RMASM_PACKINGNO" SortExpression="RMASM_PACKINGNO" HeaderText="145_Notice No." HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="CU_NAME" SortExpression="CU_NAME" HeaderText="030_Customer" HeaderStyle-Width="27%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="NoticeDate" SortExpression="NoticeDate" HeaderText="139_Notice Date" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HtmlEncode="False" DataFormatString="{0:yyyy/MM/dd}"></asp:BoundField>
                                        <asp:BoundField DataField="TrackingNo" SortExpression="TrackingNo" HeaderText="140_Tracking No" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center" Visible="false"></asp:BoundField>
                                        <asp:BoundField DataField="ShippedDate" SortExpression="ShippedDate" HeaderText="141_Shipped Date" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HtmlEncode="False" DataFormatString="{0:yyyy/MM/dd}" Visible="false"></asp:BoundField>

                                        <asp:TemplateField HeaderText="038_Detail">
                                            <HeaderStyle Width="10%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="UI_imgEdit" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/icon-detail.gif" CommandArgument='<%#Me.UI_dvShipping.Rows.Count%>' CommandName="cmdEdit" PostBackUrl="Shipment_Notice.aspx" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

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
                            </div>
                            <!--[End]資料列表表單-->
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <asp:Label ID="UI_lblPreviousPage_RMASMID" runat="server" Visible="false"></asp:Label>

</asp:Content>
