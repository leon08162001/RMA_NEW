<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Report12_Search.aspx.vb" Inherits="Report12_Search" Title="Untitled Page" %>

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
                            <asp:Label ID="UI_lblTittle" runat="server" Text=" " CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
                        </td>
                        <td width="3%">&nbsp;</td>
                    </tr>

                    <!--[End]Tittle-->
                    <!--[Begin]資料查詢條件區-->
                    <tr>
                        <td>&nbsp;</td>
                        <td align="left">

                            <asp:Panel runat="server" ID="UI_panSearch" DefaultButton="UI_cmdSearch">
                                <table width="40%" border="0" cellspacing="1" cellpadding="0" class="default">
                                    <tr>
                                        <td width="15%">
                                            <asp:Label ID="UI_lblProduct_SerialNo" runat="server" Text="153_Product Serial No"></asp:Label>
                                        </td>
                                        <td align="left" width="25%">:
			                            <asp:TextBox ID="UI_txtParts_SerialNo" runat="server" Width="120"></asp:TextBox>
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

        <tr height="28">
            <td width="24" background="Images/pic_14.gif">&nbsp;</td>
            <td background="Images/pic_15.gif">
                <table border="0" width="100%" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="1%">&nbsp;</td>
                        <td width="99%" align="left" class="default">
                            <asp:Label ID="UI_lblReportTittle" runat="server" Text="030_Report Information" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>

        <tr height="80%">
            <td background="Images/pic_20.gif">&nbsp;</td>
            <td valign="top" bgcolor="#E3D8BE" align="left">
                <table runat="server" id="tbReport" border="0" width="100%" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="1%">&nbsp;</td>
                        <td width="99%" align="left" class="default">
                            <!--[Begin]資料列表表單-->
                            <div align="center">
                                <asp:GridView ID="UI_dvReport" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:BoundField DataField="PARTS_SN" HeaderText="H_015_PARTS_SN">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PARTS_NO" HeaderText="H_016_PARTS_NO">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SHIPPED_TIME" HeaderText="H_017_SHIPPED_TIME">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CUSTOMER_ID" HeaderText="H_018_CUSTOMER_ID" HtmlEncode="False">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CUSTOMER_NAME" HeaderText="H_019_CUSTOMER_NAME" HtmlEncode="False">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ORDER_ID" HeaderText="H_020_ORDER_ID" />
                                        <asp:BoundField DataField="SHIPPING_NOTICE_ID" HeaderText="H_021_SHIPPING_NOTICE_ID" />
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
</asp:Content>

