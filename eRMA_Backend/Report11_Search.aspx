<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Report11_Search.aspx.vb" Inherits="Report11_Search" Title="Untitled Page" %>

<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>

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
                            <asp:Label ID="UI_lblTittle" runat="server" Text="H_001_Warranty Query" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
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
                                            <asp:Label ID="UI_lblProduct_SerialNo" runat="server" Text="H_002_Product Serial No"></asp:Label>
                                        </td>
                                        <td align="left" width="25%">:
			                            <asp:TextBox ID="UI_txtProduct_SerialNo" runat="server" Width="120"></asp:TextBox>
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
                <table runat="server" id="tbReport" border="0" width="1200px" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="1%">&nbsp;</td>
                        <td width="99%" align="left" class="default">
                            <!--[Begin]資料列表表單-->
                            <div align="center">
                                <asp:GridView ID="UI_dvReport" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:BoundField DataField="Order No." HeaderText="H_003_Order No.">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Item" HeaderText="H_004_Item" />
                                        <asp:BoundField DataField="Product No." HeaderText="H_005_Product No." />
                                        <asp:BoundField DataField="Desc" HeaderText="H_006_Desc">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Ord. dt" HeaderText="H_007_Ord. Date" />
                                        <asp:BoundField DataField="E.D.D" HeaderText="H_008_E.D.D" />
                                        <asp:BoundField DataField="Plan Ship dt" HeaderText="H_009_Plan Ship Date" />
                                        <asp:BoundField DataField="Ship dt" HeaderText="H_010_Ship Date" />
                                        <asp:BoundField DataField="Del. Qty." HeaderText="H_011_Del. Qty." />
                                        <asp:BoundField DataField="Undel. Qty." HeaderText="H_012_Undel. Qty." />
                                        <asp:BoundField DataField="Customer Ord. No." HeaderText="H_013_Customer Ord. No.">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
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
    <uc1:ucMessage ID="UcMessage" runat="server" />

</asp:Content>

