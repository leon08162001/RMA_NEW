<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Client_Worklist.aspx.vb" Inherits="Client_Worklist" Title="Untitled Page" %>

<%@ Register Src="ascx/ucProgressStatus.ascx" TagName="ucProgressStatus" TagPrefix="uc1" %>
<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>
<%@ Register Src="ascx/ucClientDetail.ascx" TagName="ucClientDetail" TagPrefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <script type="text/javascript">
        function FrmCancel() {
            if (confirm(CancelItemMsg)) {
                onProgress('Cancel')
                return true;
            }
            return false;
        }    </script>

    <uc1:ucProgressStatus ID="ucProgressStatus" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>

            <table border="0" cellspacing="0" cellpadding="0" width="100%" height="410px">
                <tr height="90px">
                    <td width="24" background="Images/pic_12.gif">&nbsp;</td>
                    <td valign="top" align="left">
                        <asp:Panel runat="server" ID="UI_panSearch" DefaultButton="UI_cmdSearch">
                            <table border="0" width="100%" cellspacing="0" cellpadding="0">
                                <!--[Begin]Tittle-->
                                <tr>
                                    <td width="3%">&nbsp;</td>
                                    <td width="94%" align="left">
                                        <asp:Label ID="UI_lblTittle" runat="server" Text="067_Wait for Processing" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
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
                                                <td colspan="4">
                                                    <asp:Label ID="UI_lblRequestInformation" runat="server" Text="028_Search for Request Items Information"></asp:Label>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td width="10%">
                                                    <asp:Label ID="UI_lblRMANo" runat="server" Text="029_RMA No."></asp:Label>
                                                </td>
                                                <td width="40%">:
			                            <asp:TextBox ID="UI_txtRMANo" runat="server" Width="150"></asp:TextBox>
                                                </td>


                                                <td width="12%" align="right">&nbsp;
			                            <asp:Label ID="UI_lblStatus" runat="server" Text="032_Status" Visible="false"></asp:Label>
                                                </td>
                                                <td width="30%" align="left">&nbsp;<!--:-->
                                                    <asp:DropDownList ID="UI_cboStatus" runat="server" Visible="false">
                                                        <asp:ListItem Text="-All-" Value="-1"></asp:ListItem>
                                                        <asp:ListItem Text="Request" Value="10"></asp:ListItem>
                                                        <asp:ListItem Text="Processing" Value="20"></asp:ListItem>
                                                        <asp:ListItem Text="Shipped" Value="90"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>

                                            </tr>

                                            <tr>
                                                <td>
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
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="3">&nbsp;</td>
                                </tr>
                                <!--[End]資料查詢條件區-->
                            </table>
                        </asp:Panel>
                    </td>
                </tr>

                <tr height="28px">
                    <td width="24" background="Images/pic_14.gif">&nbsp;</td>
                    <td background="Images/pic_15.gif">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="1%">&nbsp;</td>
                                <td width="20%" align="left" class="default">
                                    <asp:Label ID="UI_lblRequestedTittle" runat="server" Text="034_Requested Information" Font-Bold="true"></asp:Label>
                                </td>
                                <td width="79%" align="left" class="default">
                                    <asp:Label ID="UI_lblQuickTittle" runat="server" Text="171_Please click check boxes and click 'confirm' button to start repairing."></asp:Label>
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


                                        <asp:GridView ID="UI_dvCustomer" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowSorting="true">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="UI_CheckGroup" runat="server" AutoPostBack="true" OnCheckedChanged="UI_CheckGroup_CheckedChanged" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="UI_Check" runat="server" AutoPostBack="true" OnCheckedChanged="UI_check_CheckedChanged" />
                                                        <asp:Label ID="UI_RMANO" runat="server" Text='<%# Eval("RMA_NO") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMAID" runat="server" Text='<%# Eval("RMA_ID") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMASTATUS" runat="server" Text='<%# Eval("RMAD_STATUS") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMADID" runat="server" Text='<%# Eval("RMAD_ID") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMASQID" runat="server" Text='<%# Eval("RMASQ_ID") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="RMA_NO" HeaderText="029_RMA No" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center" SortExpression="RMA_NO"></asp:BoundField>
                                                <asp:BoundField DataField="RequestDate" HeaderText="033_Request Date" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center" SortExpression="RequestDate"></asp:BoundField>
                                                <asp:BoundField DataField="Applicant" HeaderText="006_Applicant" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center" HtmlEncode="False" SortExpression="Applicant"></asp:BoundField>

                                                <asp:TemplateField HeaderText="127_Total Amount" SortExpression="QUOTE">
                                                    <HeaderStyle Width="17%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="UI_CurrencyCode" runat="server" Text='<%# Eval("CurrencyCode") %>'></asp:Label>&nbsp;
                                                <asp:Label ID="UI_Quote" runat="server" Text='<%# Eval("QUOTE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="RequestQty" HeaderText="213_Request Qty" HeaderStyle-Width="12%" ItemStyle-HorizontalAlign="Center" SortExpression="RequestQty"></asp:BoundField>
                                                <asp:BoundField DataField="ShippedQty" HeaderText="214_Shipped Qty" HeaderStyle-Width="12%" ItemStyle-HorizontalAlign="Center" Visible="false" SortExpression="ShippedQty"></asp:BoundField>
                                                <asp:BoundField DataField="Remark" HeaderText="134_Remark" HeaderStyle-Width="25%" ItemStyle-HorizontalAlign="Center" SortExpression="Remark"></asp:BoundField>

                                                <asp:TemplateField HeaderText="032_Status" SortExpression="RMAD_STATUS">
                                                    <HeaderStyle Width="7%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="UI_Status" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="038_Detail">
                                                    <HeaderStyle Width="7%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="UI_imgDetail" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/icon-detail.gif" CommandArgument='<%#Me.UI_dvCustomer.Rows.Count%>' CommandName="cmdDetail" PostBackUrl="~/Client_Worklist_Item.aspx" />
                                                        <asp:Button ID="UI_cmdEdit" runat="server" Text="_Edit" CssClass="Pick" Width="50" CommandName="cmdEdit" CommandArgument='<%# me.UI_dvCustomer.Rows.Count%>' PostBackUrl="~/Request_New.aspx" Visible="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>

                                            <EmptyDataTemplate>
                                                <asp:Label runat="server" ID="UI_NotFound" Text='<%# Session("_isShowNotFound").tostring()%>'></asp:Label>
                                            </EmptyDataTemplate>

                                            <FooterStyle CssClass="TR_1" HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="Text_Head" />
                                            <RowStyle CssClass="TR_1" />
                                            <AlternatingRowStyle CssClass="ListRowEven" />
                                            <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Center" />
                                        </asp:GridView>

                                    </div>
                                    <!--[End]資料列表表單-->
                                </td>
                            </tr>

                            <tr>
                                <td colspan="2" align="left">
                                    <table class="default" cellspacing="0" cellpadding="0" rules="all" border="0" style="width: 100%;">
                                        <tr>
                                            <td class="TR_1">&nbsp;&nbsp;&nbsp;&nbsp;
                    	            <font face="Wingdings">þ </font>
                                                &nbsp;
                    	            <font color="#6699FF">
                                        <asp:Label runat="server" ID="lblFooterDesc"></asp:Label></font>
                                            </td>
                                            <td width="300" class="TR_1">
                                                <asp:Label runat="server" CssClass="default" ID="lblQuoteTotal"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="2" align="center">
                                    <br />
                                    <asp:Button ID="UI_cmdConfirm" runat="server" Text="_Confirm" CssClass="Confirm_l" OnClientClick="onProgress('Save')" Enabled="false" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="UI_cmdCancel" runat="server" Text="_Cancel" CssClass="Confirm_l" CausesValidation="false" OnClientClick="return FrmCancel()" Enabled="false" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <asp:Label ID="UI_lblPreviousPage_RMAID" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblPreviousPage_RMANO" runat="server" Visible="false"></asp:Label>

            <uc2:ucMessage ID="ucMessage" runat="server" />
            <uc1:ucClientDetail ID="ucClientDetail" runat="server" />

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="UI_cmdConfirm" EventName="Click" />
            <asp:PostBackTrigger ControlID="ucMessage" />
            <asp:PostBackTrigger ControlID="UI_cmdSearch" />
            <asp:PostBackTrigger ControlID="UI_dvCustomer" />
        </Triggers>
    </asp:UpdatePanel>


</asp:Content>




