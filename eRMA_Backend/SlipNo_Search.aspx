<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="SlipNo_Search.aspx.vb" Inherits="SlipNo_Search" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <table border="0" cellspacing="0" cellpadding="0" width="100%" height="100%">
        <tr>
            <td width="24" background="Images/pic_12.gif">&nbsp;</td>
            <td valign="top" align="left" height="100">

                <table border="0" width="100%" cellspacing="0" cellpadding="0">
                    <!--[Begin]Tittle-->
                    <tr>
                        <td width="3%">&nbsp;</td>
                        <td width="94%" align="left">
                            <asp:Label ID="UI_lblTittle" runat="server" Text="Searching - Decrypt SlipNo" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
                        </td>
                        <td width="3%">&nbsp;</td>
                    </tr>

                    <tr>
                        <td>&nbsp;</td>
                        <td align="left" colspan="2">
                            <table width="95%" border="0" cellspacing="1" cellpadding="0" class="default">
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="SlipNoSearching" runat="server" Text="Decrypt Slip No Searching" Font-Bold="true"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblSlipNo" runat="server" Text="Decrypt No(17 Number)"></asp:Label>
                                    </td>
                                    <td colspan="3">:
				                        <asp:TextBox ID="txtSlipNo" runat="server" Width="120"></asp:TextBox>&nbsp;
				                        <asp:Button ID="UI_cmdSearch" runat="server" Text="Search" CssClass="Search" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblNoTitle" runat="server" Text="No"></asp:Label>
                                    </td>
                                    <td colspan="3">:
				                        <asp:Label ID="lblNo" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCustTitle" runat="server" Text="Customer Code"></asp:Label>
                                    </td>
                                    <td colspan="3">:
				                        <asp:Label ID="lblCust" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCountTitle" runat="server" Text="No Recored Count"></asp:Label>
                                    </td>
                                    <td colspan="3">:
				                        <asp:Label ID="lblCount" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="lblErrorMsg" runat="server" Font-Bold="true"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <!--[End]資料查詢條件區-->
                </table>
            </td>
        </tr>

        <tr height="10">
            <td background="Images/pic_12.gif">&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
    </table>
</asp:Content>
