<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Report2_Search.aspx.vb" Inherits="Report2_Search" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <table border="0" cellspacing="0" cellpadding="0" width="100%" height="100%">
        <tr height="10%">
            <td width="24" background="Images/pic_12.gif">&nbsp;</td>
            <td valign="top" align="left">
                <table border="0" width="90%" cellspacing="0" cellpadding="0">
                    <!--[Begin]Tittle-->
                    <tr>
                        <td width="3%">&nbsp;</td>
                        <td width="94%" align="left">
                            <asp:Label ID="UI_lblTittle" runat="server" Text="002_Repair Frequency Query" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
                        </td>
                        <td width="3%">&nbsp;</td>
                    </tr>
                    <!--[End]Tittle-->
                    <!--[Begin]資料查詢條件區-->
                    <tr>
                        <td>&nbsp;</td>
                        <td align="left">
                            <asp:Panel runat="server" ID="UI_panSearch" DefaultButton="UI_cmdSearch">
                                <table width="800px" border="0" cellspacing="1" cellpadding="0" class="default">
                                    <tr>

                                        <td width="15%">
                                            <asp:Label ID="UI_lblSerialNo" runat="server" Text="150_Serial No"></asp:Label>
                                        </td>
                                        <td width="85%%" colspan="3">:
			                            <asp:TextBox ID="UI_txtSeriallNo" runat="server" Width="120"></asp:TextBox>
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
                <table runat="server" id="tbReport" border="0" width="80%" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="1%">&nbsp;</td>
                        <td width="99%" align="left" class="default">
                            <!--[Begin]資料列表表單-->
                            <div align="center">
                                <asp:GridView ID="UI_dvReport" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderStyle Width="35px" Height="15px" HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="UI_SeqID" runat="server"></asp:Label>
                                                <asp:Label ID="UI_SerialNo" runat="server" Text='<%# Eval("SerialNo") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="UI_ModelNo" runat="server" Text='<%# Eval("ModelNo") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="SerialNo" HeaderText="159_Serial No" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30%"></asp:BoundField>
                                        <asp:BoundField DataField="ModelNo" HeaderText="101_Model No" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30%"></asp:BoundField>
                                        <asp:BoundField DataField="iCount" HeaderText="179_Quantity" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>

                                        <asp:TemplateField HeaderText="180_Detail">
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle Width="25%" HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="UI_HrefDetail" Text="181_Link to Request List" CommandArgument='<%# me.UI_dvReport.Rows.Count%>' CommandName="cmdDetail" PostBackUrl="RequestRMA_List.aspx"></asp:LinkButton>
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
                                    <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="center" />
                                </asp:GridView>
                            </div>
                            <!--[End]資料列表表單-->
                        </td>
                    </tr>

                    <%--	                
                    <tr>
	                    <td colspan="2" align="left">
	                        <p style=" margin-top:10px; padding-left:50px">
	                         <asp:Button ID="UI_cmdExport" runat="server" Text="076_Export" CssClass="Confirm" />
	                         </p>
	                    </td>
	                </tr>
                    --%>
                </table>
            </td>
        </tr>

        <tr>
            <td colspan="2" align="left" background="Images/pic_23.gif">&nbsp;</td>
        </tr>
    </table>

    <asp:Label runat="server" ID="UI_lblPreviousPage_SerialNo" Visible="false"></asp:Label>
    <asp:Label runat="server" ID="UI_lblPreviousPage_ModelNo" Visible="false"></asp:Label>
    <asp:Label runat="server" ID="UI_lblFdate" Visible="false"></asp:Label>
    <asp:Label runat="server" ID="UI_lblEdate" Visible="false"></asp:Label>

</asp:Content>
