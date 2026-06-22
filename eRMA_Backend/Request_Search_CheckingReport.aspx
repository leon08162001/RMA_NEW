<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Request_Search_CheckingReport.aspx.vb" Inherits="Request_Search_CheckingReport" Title="Untitled Page" %>

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
                                            <asp:Label ID="UI_lblRequestInformation" runat="server" Text="028_Search for Request Items Information"></asp:Label>
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
                                            <asp:Label ID="UI_lblCustomer" runat="server" Text="030_Customer"></asp:Label>
                                        </td>
                                        <td width="30%" align="left">:
			                            <asp:TextBox ID="UI_txtCustomer" runat="server" Width="120"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="UI_lblModelNo" runat="server" Text="012_Model No"></asp:Label>
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
                                        <td>
                                            <asp:Label ID="UI_lblRepairCenter" runat="server" Text="009_Repair Center"></asp:Label>
                                        </td>
                                        <td>:
			                            <asp:DropDownList ID="UI_cboRepairCenter" runat="server"></asp:DropDownList>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="UI_lblStatus" runat="server" Text="032_Status"></asp:Label>
                                        </td>
                                        <td align="left">:
			                            <asp:DropDownList ID="UI_cboStatus" runat="server">
                                        </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="UI_lblRequestDate" runat="server" Text="033_Request Date"></asp:Label>*
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
                            <div align="center">
                                <asp:GridView ID="UI_dvRequest" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric" AllowSorting="true">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="UI_SeqID" runat="server"></asp:Label>
                                                <asp:Label ID="UI_Icount" runat="server" Text='<%# me.UI_dvRequest.Rows.Count +1 %>' Visible="false"></asp:Label>
                                                <asp:Label ID="UI_RMAID" runat="server" Text='<%# Eval("RMA_ID") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="UI_RMADID" runat="server" Text='<%# Eval("RMAD_ID") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="UI_RMANO" runat="server" Text='<%# Eval("RMAD_RMANO") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="UI_RMADSTATUS" runat="server" Text='<%# Eval("RMAD_STATUS") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="UI_RECEVSTATUS" runat="server" Text='<%# Eval("RMAD_RECEVSTATUS") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="UI_RMADPARTSN" runat="server" Text='<%# Eval("RMAD_PARTSN") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="029_RMA No." SortExpression="RMAD_RMANO">
                                            <HeaderStyle Width="12%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="UI_RMADNO" runat="server" Text='<%# Eval("RMAD_RMANO") %>' CommandName="cmdDetail" CommandArgument='<%# me.UI_dvRequest.Rows.Count%>' PostBackUrl="RMARepair_UpLoad.aspx"></asp:LinkButton>
                                                <asp:Label ID="UI_RMADNOText" runat="server" Text='<%# Eval("RMAD_RMANO") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="013_Serial Numbe">
                                            <HeaderStyle Width="14%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="UI_RMAD_SERIALNO" runat="server" Text='<%# Eval("RMAD_SERIALNO") %>' CommandName="cmdChangeSn" CommandArgument='<%# me.UI_dvRequest.Rows.Count%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:BoundField DataField="RMAD_MODELNO" SortExpression="RMAD_MODELNO" HeaderText="035_Model" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="Warranty" SortExpression="Warranty" HeaderText="015_Warranty" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center" HtmlEncode="False" DataFormatString="{0:yyyy/MM/dd}"></asp:BoundField>
                                        <asp:BoundField DataField="RequestDate" SortExpression="RequestDate" HeaderText="033_Request Date" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center" HtmlEncode="False" DataFormatString="{0:yyyy/MM/dd}"></asp:BoundField>
                                        <asp:BoundField DataField="CU_NAME" SortExpression="CU_NAME" HeaderText="036_Client" HeaderStyle-Width="14%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>

                                        <%--<asp:BoundField DataField="RMAD_RMANO" HeaderText="029_RMA No." HeaderStyle-Width="12%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>--%>


                                        <asp:BoundField DataField="Quoted" SortExpression="Quoted" HeaderText="070_Quote" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="Amount" SortExpression="Amount" HeaderText="037_Amount" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="032_Status" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>

                                        <asp:TemplateField HeaderText="038_Detail">
                                            <HeaderStyle Width="5%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="UI_imgEdit" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/icon-detail.gif" CommandArgument='<%#Me.UI_dvRequest.Rows.Count%>' CommandName="cmdEdit" PostBackUrl="Request_Detail.aspx" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <%--  <asp:TemplateField Visible="false">
                                         <HeaderStyle Width="5%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                         <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                         <ItemTemplate>
                                                <asp:ImageButton ID="UI_imgDel" runat ="server" ImageAlign ="AbsMiddle" ImageUrl ="~/images/xx.gif" CommandArgument='<%# me.UI_dvRequest.Rows.Count%>' CommandName ="cmdDel" />
                                         </ItemTemplate>
                                      </asp:TemplateField>--%>
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
    <asp:Label ID="UI_lblPreviousPage_RMADID" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="UI_lblPreviousPage_RMANO" runat="server" Visible="false"></asp:Label>

    <asp:Label ID="UI_flowCase" runat="server" Visible="false"></asp:Label>

</asp:Content>

