<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Client_Status_List.aspx.vb" Inherits="Client_Status_List" Title="Untitled Page" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

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
			                            <asp:TextBox ID="UI_txtRMANo" runat="server" Width="150"></asp:TextBox>
                                                </td>
                                                <td width="12%" align="right">
                                                    <asp:Label ID="UI_lblStatus" runat="server" Text="032_Status"></asp:Label>
                                                </td>
                                                <td width="30%" align="left">:
			                            <asp:DropDownList ID="UI_cboStatus" runat="server"></asp:DropDownList>
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

                                    <asp:GridView ID="UI_dvRequest" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric" AllowSorting="true">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="UI_SeqID" runat="server"></asp:Label>
                                                    <asp:Label ID="UI_RMANO" runat="server" Text='<%# Eval("RMA_NO") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="UI_RMAID" runat="server" Text='<%# Eval("RMA_ID") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="UI_RMAD_STATUS" runat="server" Text='<%# Eval("RMAD_STATUS") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="UI_PrintQuotedFRBH" runat="server" Text='<%# Eval("PrintQuotedFRBH") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="RMA_NO" SortExpression="RMA_NO" HeaderText="029_RMA No" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            <asp:BoundField DataField="RequestDate" SortExpression="RequestDate" HeaderText="033_Request Date" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            <asp:BoundField DataField="Applicant" SortExpression="Applicant" HeaderText="006_Applicant" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center" HtmlEncode="False"></asp:BoundField>

                                            <asp:TemplateField HeaderText="127_Total Amount" SortExpression="QUOTE">
                                                <HeaderStyle Width="10%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="UI_CurrencyCode" runat="server" Text='<%# Eval("CurrencyCode") %>'></asp:Label>&nbsp;
                                                <asp:Label ID="UI_Quote" runat="server" Text='<%# Eval("QUOTE") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="RequestQty" SortExpression="RequestQty" HeaderText="213_Request Qty" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            <asp:BoundField DataField="ProcessingQty" SortExpression="ProcessingQty" HeaderText="215_Processing Qty" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            <asp:BoundField DataField="ShippedQty" SortExpression="ShippedQty" HeaderText="214_Shipped Qty" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            <asp:BoundField DataField="Remark" SortExpression="Remark" HeaderText="134_Remark" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>

                                            <asp:TemplateField HeaderText="038_Detail">
                                                <HeaderStyle Width="8%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="UI_cmdDetail" ImageUrl="images/icon-detail.gif" runat="server" CommandName="cmdDetail" CommandArgument='<%#Me.UI_dvRequest.Rows.Count%>' PostBackUrl="~/Client_Status_Item.aspx" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="217_Print">
                                                <HeaderStyle Width="15%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                <ItemTemplate>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="UI_cmdPrintForm" runat="server" Text="419_Print" CssClass="Button01" Style="width: 100px" CommandName="cmdPrintForm" CommandArgument='<%#Me.UI_dvRequest.Rows.Count%>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="UI_cmdPrintQuotedFRBH" runat="server" Text="420_Print" CssClass="Button01" Style="width: 100px" CommandName="cmdPrintQuotedFRBH" CommandArgument='<%#Me.UI_dvRequest.Rows.Count%>' />
                                                            </td>
                                                        </tr>
                                                    </table>
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

            <asp:Label ID="UI_lblPreviousPage_RMAID" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblPreviousPage_RMANO" runat="server" Visible="false"></asp:Label>

        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="UI_cmdSearch" />
            <asp:PostBackTrigger ControlID="UI_dvRequest" />
        </Triggers>
    </asp:UpdatePanel>

    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />

</asp:Content>
