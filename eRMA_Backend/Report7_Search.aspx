<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Report7_Search.aspx.vb" Inherits="Report7_Search" Title="Untitled Page" %>

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
                            <asp:Label ID="UI_lblTittle" runat="server" Text="004_RMA Detail Report" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
                        </td>
                        <td width="3%">&nbsp;</td>
                    </tr>
                    <!--[End]Tittle-->
                    <!--[Begin]資料查詢條件區-->
                    <tr>
                        <td>&nbsp;</td>
                        <td align="left">

                            <asp:Panel runat="server" ID="UI_panSearch" DefaultButton="UI_cmdSearch">
                                <table width="100%" border="0" cellspacing="1" cellpadding="0" class="default">
                                    <tr runat="server" id="UI_trCompanyName">
                                        <td>
                                            <asp:Label ID="UI_lblCompanyName" runat="server" Text="100_Company Name"></asp:Label>
                                        </td>
                                        <td colspan="3">:
			                            <asp:TextBox ID="UI_txtCompanyName" runat="server" Width="120"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td width="15%">
                                            <asp:Label ID="UI_lblRepairCenter" runat="server" Text="102_Repair Center"></asp:Label>
                                        </td>
                                        <td width="35%">:
			                            <asp:DropDownList ID="UI_cboRepairCenter" runat="server"></asp:DropDownList>
                                        </td>

                                        <td width="12%" align="right">
                                            <asp:Label ID="UI_lblWarranty" runat="server" Text="103_Warranty"></asp:Label>
                                        </td>
                                        <td width="30%" align="left">:
			                            <asp:DropDownList ID="UI_cboWarranty" runat="server"></asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <asp:Label ID="UI_lblDurationDate" runat="server" Text="105_Duration"></asp:Label>
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
                <table border="0" width="70%" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="1%">&nbsp;</td>
                        <td width="99%" align="left" class="default">
                            <!--[Begin]資料列表表單-->
                            <div align="center">
                                <asp:GridView ID="UI_dvReport" runat="server" ShowFooter="true" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderStyle Width="4%" Height="15px" HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="UI_SeqID" runat="server" Text='<%# Eval("SeqID") %>'></asp:Label>
                                                <asp:Label ID="UI_isShow" runat="server" Text='<%# Eval("isShow") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="RMA_CUNO" HeaderText="143_Category ID." HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>

                                        <asp:TemplateField HeaderText="144_Category">
                                            <HeaderStyle Width="10%" HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="UI_CU_NAME" runat="server" Text='<%# Eval("CU_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="145_Year-Month">
                                            <HeaderStyle Width="10%" HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="UI_RequestYM" runat="server" Text='<%# Eval("RequestYM") %>'></asp:Label>
                                            </ItemTemplate>

                                            <FooterStyle HorizontalAlign="Center" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="UI_Footer" Text="" Font-Bold="true"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="146_Request Quantity">
                                            <HeaderStyle Width="10%" HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="UI_RequestQTY" runat="server" Text='<%# Eval("RequestQTY") %>'></asp:Label>
                                            </ItemTemplate>

                                            <FooterStyle HorizontalAlign="Center" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="UI_Footer_RequestQTY" Text="" Font-Bold="true"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="147_Closed  Quantity">
                                            <HeaderStyle Width="10%" HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="UI_ClosedQTY" runat="server" Text='<%# Eval("ClosedQTY") %>'></asp:Label>
                                            </ItemTemplate>

                                            <FooterStyle HorizontalAlign="Center" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="UI_Footer_ClosedQTY" Text="" Font-Bold="true"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="148_Canceled Quantity">
                                            <HeaderStyle Width="10%" HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="UI_CanceledQTY" runat="server" Text='<%# Eval("CanceledQTY") %>'></asp:Label>
                                            </ItemTemplate>

                                            <FooterStyle HorizontalAlign="Center" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="UI_Footer_CanceledQTY" Text="" Font-Bold="true"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                    </Columns>

                                    <EmptyDataTemplate>
                                        <asp:Label runat="server" ID="UI_NotFound" Text='<%# Session("_isShowNotFound").tostring()%>'></asp:Label>
                                    </EmptyDataTemplate>
                                    <EmptyDataRowStyle HorizontalAlign="Center" />

                                    <HeaderStyle CssClass="Text_Head" />
                                    <RowStyle CssClass="TR_1" Height="25px" />
                                    <AlternatingRowStyle CssClass="ListRowEven" Height="25px" />
                                    <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Center" />
                                    <FooterStyle BackColor="DarkGray" Height="25px" />
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
