<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Client_Search.aspx.vb" Inherits="Client_Client_Search" Title="Untitled Page" %>

<%@ Register Src="ascx/ucClientDetail.ascx" TagName="ucClientDetail" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table border="0" cellspacing="0" cellpadding="0" width="100%" height="100%">
                <tr height="10%">
                    <td width="24" background="Images/pic_12.gif">&nbsp;</td>
                    <td valign="top" align="left">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <!--[Begin]Tittle-->
                            <tr>
                                <td width="3%">&nbsp;</td>
                                <td width="94%" align="left">
                                    <asp:Label ID="UI_lblTittle" runat="server" Text="027_Status Query" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
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
                                                    <asp:Label ID="UI_lblRequestInformation" runat="server" Text="028_Search for Request Items Information" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="15%">
                                                    <asp:Label ID="UI_lblRMANo" runat="server" Text="029_RMA No."></asp:Label>
                                                </td>
                                                <td width="35%">:
			                            <asp:TextBox ID="UI_txtRMANo" runat="server" Width="120"></asp:TextBox>
                                                </td>
                                                <td width="12%" align="right">
                                                    <asp:Label ID="UI_lblStatus" runat="server" Text="032_Status"></asp:Label>
                                                </td>
                                                <td width="30%" align="left">:
			                            <asp:DropDownList ID="UI_cboStatus" runat="server"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="UI_lblModelNo" runat="server" Text="012_Model No."></asp:Label>
                                                </td>
                                                <td>:
			                            <asp:TextBox ID="UI_txtModelNo" runat="server" Width="120"></asp:TextBox>
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="UI_lblSerialNumber" runat="server" Text="013_Serial Number"></asp:Label>
                                                </td>
                                                <td align="left">:
			                            <asp:TextBox ID="UI_txtSerialNumber" runat="server" Width="120"></asp:TextBox>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td>*
			                            <asp:Label ID="UI_lblRequestDate" runat="server" Text="033_Request Date"></asp:Label>
                                                </td>
                                                <td align="left" colspan="3">:
			                            <asp:DropDownList ID="UI_cboBYear" runat="server"></asp:DropDownList>
                                                    <asp:DropDownList ID="UI_cboBMonth" runat="server"></asp:DropDownList>
                                                    <asp:DropDownList ID="UI_cboBDay" runat="server"></asp:DropDownList>&nbsp;~&nbsp;
                                        <asp:DropDownList ID="UI_cboEYear" runat="server"></asp:DropDownList>
                                                    <asp:DropDownList ID="UI_cboEMonth" runat="server"></asp:DropDownList>
                                                    <asp:DropDownList ID="UI_cboEDay" runat="server"></asp:DropDownList>
                                                    <asp:Button ID="UI_cmdSearch" runat="server" Text="_Search" CssClass="Search" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>

                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <!--[End]資料查詢條件區-->
                        </table>
                    </td>
                </tr>

                <tr height="20px">
                    <td background="Images/pic_12.gif">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>

                <tr height="10px">
                    <td width="24" background="Images/pic_14.gif">&nbsp;</td>
                    <td background="Images/pic_15.gif">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="1%">&nbsp;</td>
                                <td width="99%" align="left" class="default">
                                    <asp:Label ID="UI_lblRequestedTittle" runat="server" Text="034_Requested Information" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>


                <tr height="80%">
                    <td background="Images/pic_20.gif">&nbsp;</td>
                    <td valign="top" bgcolor="#E3D8BE" align="center">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="1%">&nbsp;</td>
                                <td width="99%" align="left" class="default">
                                    <!--[Begin]資料列表表單-->
                                    <asp:GridView ID="UI_dvRequest" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="UI_SeqID" runat="server" Text='<%# Eval("SeqID") %>'></asp:Label>
                                                    <asp:Label ID="UI_RMAID" runat="server" Text='<%# Eval("RMA_ID") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="UI_RMANO" runat="server" Text='<%# Eval("RMA_NO") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="UI_RMADID" runat="server" Text='<%# Eval("RMAD_ID") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="UI_RMASTATUS" runat="server" Text='<%# Eval("RMA_STATUS") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="UI_RMADSTATUS" runat="server" Text='<%# Eval("RMAD_STATUS") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="UI_RMADWARRANTY" runat="server" Text='<%# Eval("RMAD_WARRANTY") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="UI_RMADCSTMP" runat="server" Text='<%# Eval("RMAD_CSTMP") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="RMAD_SERIALNO" HeaderText="_Serial Number" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            <asp:BoundField DataField="RMAD_MODELNO" HeaderText="_Model" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            <asp:BoundField DataField="WARRANTY" HeaderText="_Warranty" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd}"></asp:BoundField>
                                            <asp:BoundField DataField="RMA_CSTMP" HeaderText="_Request Date" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd}"></asp:BoundField>
                                            <asp:BoundField DataField="RMA_NO" HeaderText="_RMA No" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            <asp:BoundField DataField="Amount" HeaderText="_Amount" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            <asp:BoundField DataField="Status" HeaderText="_Status" HeaderStyle-Width="9%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>

                                            <asp:TemplateField HeaderText="_Detail">
                                                <HeaderStyle Width="8%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="UI_cmdDetail" ImageUrl="images/icon-detail.gif" runat="server" CommandName="cmdDetail" CommandArgument='<%# me.UI_dvRequest.Rows.Count%>' Visible="false" />
                                                    <asp:Button ID="UI_cmdEdit" runat="server" Text="_Edit" CssClass="Pick" Width="50" CommandName="cmdEdit" CommandArgument='<%# me.UI_dvRequest.Rows.Count%>' PostBackUrl="~/Request_New.aspx" Visible="false" />
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
                                    <!--[End]資料列表表單-->
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <uc1:ucClientDetail ID="ucClientDetail" runat="server" />

            <asp:Label ID="UI_lblPreviousPage_RMAID" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblPreviousPage_RMANO" runat="server" Visible="false"></asp:Label>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
