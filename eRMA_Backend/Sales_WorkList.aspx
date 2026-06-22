<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Sales_WorkList.aspx.vb" Inherits="Sales_WorkList" Title="Untitled Page" %>

<%@ Register Src="ascx/ucProgressStatus.ascx" TagName="ucProgressStatus" TagPrefix="uc1" %>
<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <uc1:ucProgressStatus ID="ucProgressStatus" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table border="0" cellspacing="0" cellpadding="0" width="100%" height="410px">
                <tr height="90px">
                    <td width="24" background="Images/pic_12.gif">&nbsp;</td>
                    <td valign="top" align="left">
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

                                    <asp:Panel runat="server" ID="UI_panSearch" DefaultButton="UI_cmdSearch">
                                        <table width="95%" border="0" cellspacing="1" cellpadding="0" class="default">
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Label ID="UI_lblRequestInformation" runat="server" Text="028_Search for Request Items Information"></asp:Label>
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
                                                    <asp:Label ID="UI_lblCustomer" runat="server" Text="030_Customer"></asp:Label>
                                                </td>
                                                <td width="30%" align="left">:
			                            <asp:TextBox ID="UI_txtCustomer" runat="server" Width="120"></asp:TextBox>
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
                                <td width="49%" align="left" class="default">
                                    <asp:Label ID="UI_lblRequestedTittle" runat="server" Text="034_Requested Information" Font-Bold="true"></asp:Label>
                                </td>
                                <td width="50%" align="right" class="default">&nbsp;</td>
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
                                        <asp:GridView ID="UI_dvSales" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric" AllowSorting="true">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="UI_SEQID" runat="server"></asp:Label>
                                                        <asp:Label ID="UI_RMAID" runat="server" Text='<%# Eval("RMA_ID") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMANO" runat="server" Text='<%# Eval("RMA_NO") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="029_RMA No" SortExpression="RMA_NO">
                                                    <HeaderStyle Width="15%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="UI_RMADNO" runat="server" Text='<%# Eval("RMA_NO") %>' CommandName="cmdDetail" CommandArgument='<%# me.UI_dvSales.Rows.Count%>' PostBackUrl="RMARepair_UpLoad.aspx"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="RequestDate" SortExpression="RequestDate" HeaderText="033_Request Date" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="CUNAME" SortExpression="CUNAME" HeaderText="036_Client" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="Applicant" SortExpression="Applicant" HeaderText="006_Applicant" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center" HtmlEncode="False"></asp:BoundField>

                                                <asp:BoundField DataField="RequestQTY" SortExpression="RequestQTY" HeaderText="213_Request Q'TY" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="ReceivedQTY" SortExpression="ReceivedQTY" HeaderText="216_Received Q'TY" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>

                                                <asp:TemplateField HeaderText="070_Quote" SortExpression="RepairQuoted">
                                                    <HeaderStyle Width="10%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="UI_RepairCode" runat="server" Text='<%# Eval("RepairCode") %>'></asp:Label>
                                                        <asp:Label ID="UI_RepairQuoted" runat="server" Text='<%# Eval("RepairQuoted") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="037_Amount" SortExpression="SaleQuoted">
                                                    <HeaderStyle Width="10%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="UI_SaleCode" runat="server" Text='<%# Eval("SaleCode") %>'></asp:Label>
                                                        <asp:Label ID="UI_SaleQuoted" runat="server" Width="50px" Text='<%# Eval("SaleQuoted") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="038_Detail" Visible="false">
                                                    <HeaderStyle Width="9%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Button ID="UI_cmdEdit" runat="server" Text="_Quoting" CssClass="Pick" Width="50" CommandName="cmdEdit" CommandArgument='<%# me.UI_dvSales.Rows.Count%>' PostBackUrl="Sales_WorkList_Item.aspx" />
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

            <asp:Label ID="UI_lblPreviousPage_RMAID" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblPreviousPage_RMANO" runat="server" Visible="false"></asp:Label>

            <uc2:ucMessage ID="ucMessage" runat="server" />

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="UI_cmdSearch" />
            <asp:PostBackTrigger ControlID="ucMessage" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:ValidationSummary ID="vsSaleSave" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="SaleSaveGroup" />

</asp:Content>

