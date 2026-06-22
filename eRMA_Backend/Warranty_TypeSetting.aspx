<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Warranty_TypeSetting.aspx.vb" Inherits="Warranty_TypeSetting" Title="Untitled Page" %>

<%@ Register Src="ascx/ucProgressStatus.ascx" TagName="ucProgressStatus" TagPrefix="uc1" %>
<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>
<%@ Register Src="~/ascx/ucProductGroup.ascx" TagName="ucProductGroup" TagPrefix="uc6" %>

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
                                            <asp:Label ID="UI_lblTittle" runat="server" Text="Warranty Setting- Searching"
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
                                                        <td width="8%">
                                                            <asp:Label ID="UI_lblOperationCenter" runat="server" Text="022_Operation Center"></asp:Label>
                                                        </td>
                                                        <td width="10%">:
                                                            <asp:DropDownList ID="UI_CboOperationCenter" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="right" width="8%">
                                                            <asp:Label ID="UI_lblProductGroup" runat="server" Text="023_Product Group"></asp:Label>
                                                        </td>
                                                        <td align="left" width="20%">:
                                                            <asp:TextBox ID="UI_txtProductGroup" runat="server" Width="120"></asp:TextBox>
                                                            <asp:Button ID="UI_cmdProductGroupPick" runat="server" Text="_Pick" CssClass="Pick"></asp:Button>
                                                        </td>
                                                        <td align="right" width="8%">
                                                            <asp:Label ID="UI_lblWarrantyType" runat="server" Text="024_Warranty Type"></asp:Label>
                                                        </td>
                                                        <td align="left" width="30%">:
                                                            <asp:DropDownList ID="UI_CboWarrantyType" runat="server" AutoPostBack="True">
                                                            </asp:DropDownList>

                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="UI_lblProgramType" runat="server" Text="010_Project Type"></asp:Label>
                                                        </td>
                                                        <td align="left">:
                                                            <asp:DropDownList ID="UI_cboProgramType" runat="server" Visible="true"></asp:DropDownList>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="UI_lblItemtype" runat="server" Text="009_Item Type"></asp:Label>
                                                        </td>
                                                        <td align="left">:
			                                    <asp:DropDownList ID="UI_cboItemType" runat="server" Visible="true"></asp:DropDownList>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="UI_lblPriceVer" runat="server" Text="009_Price Version"></asp:Label>
                                                        </td>
                                                        <td align="left">:
                                                <asp:DropDownList ID="UI_cboPriceVer" runat="server" Visible="true"></asp:DropDownList>

                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="UI_lblCust" runat="server" Text="023_Cust NO"></asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="UI_txtCust" runat="server" Width="120"></asp:TextBox>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="UI_lblWarrantyName" runat="server" Text="023_Warranty Name"></asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="UI_txtWarrantyName" runat="server" Width="120"></asp:TextBox>
                                                        </td>

                                                        <td align="right">
                                                            <asp:Label ID="UI_lblStatus" runat="server" Text="009_Status"></asp:Label>
                                                        </td>
                                                        <td align="left">:
                                                          <asp:DropDownList ID="UI_cboStatus" runat="server" Visible="true"></asp:DropDownList>
                                                            <asp:Button ID="UI_cmdSearch" runat="server" Text="_Search" CssClass="Search"></asp:Button>
                                                            <asp:Button ID="Excel_btn" runat="server" Text="產生EXCEL" CssClass="Search"></asp:Button>
                                                            <asp:Label ID="Download_Lab" runat="server" Text=""></asp:Label>


                                                        </td>
                                                    </tr>


                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="UI_lblCust_" runat="server" Text="Show Warranty Customer Can Purchase"></asp:Label>
                                                        </td>
                                                        <td align="left">

                                                            <asp:CheckBox ID="UI_CheckBox_Cust" runat="server" Checked="false"></asp:CheckBox>
                                                        </td>
                                                        <td align="right"></td>
                                                        <td align="left"></td>

                                                        <td align="right"></td>
                                                        <td align="left"></td>
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
                                            <asp:Label ID="UI_lblPriceList" runat="server" Text="025_Price List" Font-Bold="true"></asp:Label>
                                            <asp:Button ID="UI_cmdAdd" runat="server" Text="_Add" CssClass="Confirm_l" PostBackUrl="~/Warranty_TypeSetting_add.aspx"
                                                Width="100" />
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
                                                                <asp:Label ID="lblSwID" runat="server" Text='<%# Eval("war_id") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>' Visible="false"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="026_Operation Center" SortExpression="OperationCenter">
                                                            <HeaderStyle Width="10%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="UI_RMADNO" runat="server" Text='<%# Eval("COMP_NAME") %>'
                                                                    CommandName="cmdDetail" CommandArgument='<%# me.UI_dvSales.Rows.Count%>' PostBackUrl="Warranty_TypeSetting_add.aspx"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="ProductGroup" SortExpression="ProductGroup" HeaderText="027_Product Group"
                                                            HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="WarrantyType" SortExpression="WarrantyType" HeaderText="030_Warranty Type"
                                                            HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="Program_Type" SortExpression="Version" HeaderText="084_Program_Type"
                                                            HeaderStyle-Width="6%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="Item_Type" SortExpression="Version" HeaderText="082_Item_Type"
                                                            HeaderStyle-Width="6%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="Price_Ver" SortExpression="Version" HeaderText="083_Price_Ver"
                                                            HeaderStyle-Width="6%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="cu_no" SortExpression="Version" HeaderText="083_cu_no"
                                                            HeaderStyle-Width="6%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="cu_name" SortExpression="Version" HeaderText="083_cu_name"
                                                            HeaderStyle-Width="6%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="Version" SortExpression="Version" HeaderText="028_Version"
                                                            HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="WarrantyName" SortExpression="WarrantyName" HeaderText="029_Warranty Name"
                                                            HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left" HtmlEncode="False"></asp:BoundField>
                                                        <asp:BoundField DataField="StdYears" SortExpression="StdYears" HeaderText="031_ExtraMonths"
                                                            HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="LongestYears" SortExpression="LongestYears" HeaderText="032_Years" HeaderStyle-Width="10%"
                                                            ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:TemplateField HeaderText="026_Discount" SortExpression="Discount">
                                                            <HeaderStyle Width="5%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDiscount" runat="server" Text='<%# Eval("Discount") %>' Visible="true"></asp:Label>%
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="034_Status"
                                                            HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:TemplateField HeaderText="">
                                                            <HeaderStyle Width="20%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:Button ID="UI_cmdEdit" runat="server" Text="_Edit" CssClass="Pick" Width="50"
                                                                    CommandName="cmdEdit" CommandArgument='<%# me.UI_dvSales.Rows.Count%>' PostBackUrl="Warranty_TypeSetting_add.aspx" />
                                                                <asp:Button ID="cmdCopy" runat="server" Text="Copy" CssClass="Pick" Width="50" OnClientClick="if(!confirm('請確認是否要複製?'))return false;"
                                                                    CommandName="cmdCopy" CommandArgument='<%# me.UI_dvSales.Rows.Count%>' PostBackUrl="Warranty_TypeSetting_add.aspx" />
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
            <asp:PostBackTrigger ControlID="UI_cmdSearch" />
            <asp:PostBackTrigger ControlID="ucMessage" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="vsSaleSave" runat="server" ShowMessageBox="true" ShowSummary="false"
        ValidationGroup="SaleSaveGroup" />
    <asp:Label runat="server" ID="lblPreviousPage_SwID" Visible="false"></asp:Label>
    <asp:Label runat="server" ID="lblPreviousPage_Type" Visible="false"></asp:Label>
</asp:Content>
