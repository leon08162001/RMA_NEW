<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Warranty_SellingafterOrder.aspx.vb" Inherits="Warranty_SellingafterOrder" Title="Untitled Page" %>

<%@ Register Src="ascx/ucProgressStatus.ascx" TagName="ucProgressStatus" TagPrefix="uc1" %>
<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>
<%@ Register Src="ascx/ucProductGroup.ascx" TagName="ucProductGroup" TagPrefix="uc6" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <uc1:ucProgressStatus ID="ucProgressStatus" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                <tbody>
                    <tr height="50">
                        <td width="24" background="Images/pic_12.gif">&nbsp;
                        </td>
                        <td valign="top" align="left">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tbody>
                                    <tr>
                                        <td width="3%">&nbsp;
                                        </td>
                                        <td align="left" width="94%">
                                            <asp:Label ID="UI_lblTittle" runat="server" Text="Warranty Sales Order"
                                                ForeColor="#326B9B" CssClass="text_tittle"></asp:Label>
                                        </td>
                                        <td width="3%">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td align="left">
                                            <table class="default" cellspacing="1" cellpadding="0" width="95%" border="0">
                                                <tbody>
                                                    <tr>
                                                        <td width="15%">
                                                            <asp:Label ID="lblOperationCenter" runat="server" Text="022_Operation Center"></asp:Label>
                                                        </td>
                                                        <td>:
                                                            <asp:DropDownList ID="UI_CboOperationCenter" runat="server" Width="120">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblSearchPeriod" runat="server" Text="Search Period"></asp:Label>
                                                        </td>
                                                        <td>:
																<table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="txtStart" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:ImageButton ID="ImgBtnStart" runat="server" ImageUrl="Images/icon_cal.gif" /></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtEnd" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:ImageButton ID="ImgBtnEnd" runat="server" ImageUrl="Images/icon_cal.gif" /></td>
                                                                    </tr>

                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <asp:Calendar ID="Calend_Start" runat="server" Visible="false"></asp:Calendar>
                                                                        </td>

                                                                        <td colspan="2">
                                                                            <asp:Calendar ID="Calend_End" runat="server" Visible="false"></asp:Calendar>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblWarrantyNo" runat="server" Text="Warranty No"></asp:Label>
                                                        </td>
                                                        <td>:
                                                            <asp:TextBox ID="txtWarrantyNo" runat="server" Width="120"></asp:TextBox>
                                                            &nbsp;&nbsp; 
                                                            <asp:Label ID="lblErpNo" runat="server" Text=" Invoice No"></asp:Label>
                                                            <asp:TextBox ID="txtErpNo" runat="server" Width="120"></asp:TextBox>
                                                            &nbsp;&nbsp; 
                                                            <asp:Label ID="lblCustomer" runat="server" Text="Company Name"></asp:Label>
                                                            <asp:TextBox ID="txtCustomer" runat="server" Width="120"></asp:TextBox>
                                                            <asp:Button ID="cmdSearch" runat="server" Text="_Search" CssClass="Search"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr height="30">
                        <td width="24" background="Images/pic_14.gif">&nbsp;
                        </td>
                        <td background="Images/pic_15.gif">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tbody>
                                    <tr>
                                        <td width="1%">&nbsp;
                                        </td>
                                        <td class="default" align="left" width="49%">
                                            <asp:Label ID="lblOrderItemList" runat="server" Text="Order Item List" Font-Bold="true"></asp:Label>
                                            <asp:Button ID="cmdAdd" runat="server" Text="_Add" CssClass="Confirm_l" PostBackUrl="Warranty_SellingafterOrder_add.aspx"
                                                Width="100" />
                                            <asp:Button ID="Services" runat="server" Text="_Services" CssClass="Confirm_l" Width="100" />
                                        </td>
                                        <td class="default" align="right" width="50%">&nbsp;
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr valign="top" height="250">
                        <td background="Images/pic_20.gif">&nbsp;
                        </td>
                        <td valign="top" align="center" bgcolor="#e3d8be">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tbody>
                                    <tr>
                                        <td width="1%">&nbsp;
                                        </td>
                                        <td class="default" align="left" width="99%">
                                            <div align="left">
                                                <asp:GridView ID="UI_dvSales" runat="server" CssClass="default" Width="100%" PagerSettings-Mode="Numeric" AllowPaging="true" AutoGenerateColumns="False" border="0"
                                                    CellSpacing="1" CellPadding="0" AllowSorting="true">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="UI_SEQID" runat="server"></asp:Label>
                                                                <asp:Label ID="lblSwID" runat="server" Text='<%# Eval("waty_no") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblConfirm" runat="server" Text='<%# Eval("ISConfirm") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblFlow" runat="server" Text='<%# Eval("ISFLOW") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblCompno" runat="server" Text='<%# Eval("WATY_COMPNO") %>' Visible="false"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Warranty No" SortExpression="waty_no">
                                                            <HeaderStyle Width="15%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="UI_wo_no" runat="server" Text='<%# Eval("waty_no") %>'
                                                                    CommandName="cmdDetail" CommandArgument='<%# me.UI_dvSales.Rows.Count%>' PostBackUrl="Warranty_SellingafterOrder_add.aspx"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Company Name" SortExpression="CU_NAME">
                                                            <HeaderStyle Width="15%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCustCode" runat="server" Text='<%# Eval("waty_cust") %>'></asp:Label>
                                                                / 
                                                                <asp:Label ID="lblCustName" runat="server" Text='<%# Eval("CU_NAME") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="waty_date" SortExpression="waty_date" HeaderText="Purchase Date"
                                                            HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:yyyy/MM/dd}"></asp:BoundField>
                                                        <asp:BoundField DataField="FlowName" SortExpression="FlowName" HeaderText="Status"
                                                            HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="waty_erpno" SortExpression="waty_erpno" HeaderText="Invoice No."
                                                            HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center" HtmlEncode="False"></asp:BoundField>
                                                        <asp:TemplateField HeaderText="">
                                                            <HeaderStyle Width="10%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                            <ItemTemplate>

                                                                <asp:Button ID="UI_cmdEdit" runat="server" Text="Edit" CssClass="Pick" Width="50"
                                                                    CommandName="cmdEdit" CommandArgument='<%# me.UI_dvSales.Rows.Count%>' PostBackUrl="Warranty_SellingafterOrder_add.aspx" />
                                                                <asp:Button ID="UI_cmdDele" runat="server" Text="Delete" CssClass="Pick" Width="50"
                                                                    CommandName="cmdDel" CommandArgument='<%# me.UI_dvSales.Rows.Count%>' PostBackUrl="Warranty_SellingafterOrder.aspx" />
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
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
            <uc2:ucMessage ID="ucMessage" runat="server"></uc2:ucMessage>
            <uc6:ucProductGroup ID="ucProductGroup" runat="server"></uc6:ucProductGroup>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="cmdSearch" />
            <asp:PostBackTrigger ControlID="ucMessage" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="vsSaleSave" runat="server" ShowMessageBox="true" ShowSummary="false"
        ValidationGroup="SaleSaveGroup" />
    <asp:Label runat="server" ID="lblPreviousPage_SwID" Visible="false"></asp:Label>
    <asp:Label runat="server" ID="lblPreviousPage_Compno" Visible="false"></asp:Label>
</asp:Content>
