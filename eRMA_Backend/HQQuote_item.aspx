<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="HQQuote_item.aspx.vb" Inherits="HQQuote_item" Title="Untitled Page" %>

<%@ Register Src="ascx/ucProgressStatus.ascx" TagName="ucProgressStatus" TagPrefix="uc1" %>
<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>

            <asp:Panel ID="UI_panel" runat="server">

                <table border="0" width="98%" cellspacing="0" cellpadding="0">
                    <tr height="20px">
                        <td width="24" background="Images/pic_14.gif">&nbsp;</td>
                        <td background="Images/pic_15.gif">
                            <table border="0" width="100%" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td width="1%">&nbsp;</td>
                                    <td align="left" class="default">
                                        <asp:Label ID="UI_lblProductTittle" runat="server" Text="010_Product Information" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td align="right" class="default"></td>
                                    <td width="2%">&nbsp;</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>


                <asp:GridView ID="UI_dvRequest" runat="server" Width="98%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric" AllowSorting="true">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                            <HeaderTemplate>
                            </HeaderTemplate>

                            <ItemTemplate>
                                <asp:Label ID="UI_SeqID" runat="server" Text='<%# Eval("SeqID") %>'></asp:Label>
                                <asp:Label ID="UI_Icount" runat="server" Text='<%# me.UI_dvRequest.Rows.Count +1 %>' Visible="false"></asp:Label>
                                <asp:Label ID="UI_RMAID" runat="server" Text='<%# Eval("RMA_ID") %>' Visible="false"></asp:Label>
                                <asp:Label ID="UI_RMADID" runat="server" Text='<%# Eval("RMAD_ID") %>' Visible="false"></asp:Label>
                                <asp:Label ID="UI_RMADSTATUS" runat="server" Text='<%# Eval("RMAD_STATUS") %>' Visible="false"></asp:Label>
                                <asp:Label ID="UI_SERIALNO" runat="server" Text='<%# Eval("RMAD_SERIALNO") %>' Visible="false"></asp:Label>
                                <asp:Label ID="UI_RMAR_COMPNO" runat="server" Text='<%# Eval("RMAR_COMPNO") %>' Visible="false"></asp:Label>
                                <asp:Label ID="UI_RMADCSTMP" runat="server" Text='<%# Eval("RMAD_CSTMP") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="029_RMA No">
                            <HeaderStyle Width="10%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="UI_RMANO" runat="server" Text='<%# Eval("RMAD_RMANO") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="013_Serial Numbe">
                            <HeaderStyle Width="14%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="UI_RMAD_SERIALNO" runat="server" Text='<%# Eval("RMAD_SERIALNO") %>' CommandName="cmdChangeSn" CommandArgument='<%# me.UI_dvRequest.Rows.Count%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="RMAD_MODELNO" HeaderText="035_Model" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                        <asp:BoundField DataField="Warranty" HeaderText="EW Warranty" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center" HtmlEncode="False" DataFormatString="{0:yyyy/MM/dd}"></asp:BoundField>
                        <asp:BoundField DataField="CWEndWarr" HeaderText="CW Warranty" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center" HtmlEncode="False"></asp:BoundField>

                        <asp:TemplateField HeaderText="SW Warranty">
                            <HeaderStyle Width="8%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="cmdSWDetail" runat="server" Text='<%# Eval("SWEndWarr") %>' CommandName="cmdSWDetail" CommandArgument='<%# me.UI_dvRequest.Rows.Count%>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="Quoted" HeaderText="070_Quote" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                        <asp:BoundField DataField="Status" HeaderText="032_Status" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
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



                <p></p>
                <input id="UI_cmdBack" runat="server" type="button" value="006_back" class="Problem_Edit" onclick="javascript:history.back();" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="UI_cmdSubmit" runat="server" Text="_Confirm" CssClass="Confirm_l" OnClientClick="onProgress('Save')" /><!-- UI_cmdApply_Click-->
                <p></p>
                <p></p>

                <table border="0" width="98%" cellspacing="0" cellpadding="0">
                    <tr height="20px">
                        <td width="24" background="Images/pic_14.gif">&nbsp;</td>
                        <td background="Images/pic_15.gif">
                            <table border="0" width="100%" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td width="1%">&nbsp;</td>
                                    <td align="left" class="default">
                                        <asp:Label ID="UI_lblInformationTittle" runat="server" Text="082_Replace Component" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td align="right" class="default">&nbsp;</td>
                                    <td width="2%">&nbsp;</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>

                <asp:DataList ID="UI_dvRepairDetail" runat="server" ExtractTemplateRows="true" BorderColor="#c0c0c0" CellSpacing="0" CellPadding="3" Width="98%" border="1" CssClass="default" bordercolorlight="#c0c0c0">
                    <HeaderTemplate>
                        <asp:Table ID="oTableHeader" runat="server">
                            <asp:TableHeaderRow bgcolor="#fff4d0">
                                <asp:TableHeaderCell Width="2%">&nbsp;</asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="3%">
                                    <asp:Label ID="lblWaive" runat="server" Text="405_Waive"></asp:Label>
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="3%">
                                    <asp:Label ID="lblOption" runat="server" Text="406_Option"></asp:Label>
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="6%">
                                    <asp:Label ID="lblHPart" runat="server" Text="083_Part'sNo"></asp:Label>
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="28%">
                                    <asp:Label ID="lblHDescription" runat="server" Text="099_Description"></asp:Label>
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="10%">
                                    <asp:Label ID="lblHLocation" runat="server" Text="100_SMT Location"></asp:Label>
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="8%">
                                    <asp:Label ID="lblHImproper" runat="server" Text="101_Improper Usage"></asp:Label>
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="5%">
                                    <asp:Label ID="lblHQty" runat="server" Text="103_Qty"></asp:Label>
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="6%">
                                    <asp:Label ID="lblHPrice" runat="server" Text="104_Price"></asp:Label>
                                </asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                        </asp:Table>
                    </HeaderTemplate>

                    <ItemTemplate>
                        <asp:Table ID="oTableRow" runat="server">
                            <asp:TableRow>
                                <asp:TableCell HorizontalAlign="Center">
                                    <asp:Label ID="lblSeq" runat="server" class="default" Text='<%# Container.ItemIndex+1 %>'></asp:Label>

                                    <asp:Label ID="lblNew" runat="server" Text="_New" class="default" Visible="false"></asp:Label>
                                    <asp:Label ID="lblRMARQDID" runat="server" Text='<%# Eval("RMARQD_ID") %>' class="default" Visible="false"></asp:Label>
                                    <asp:Label ID="lblIMPROPERUSAGE" runat="server" Text='<%# Eval("RMARQD_IMPROPERUSAGE") %>' class="default" Visible="false"></asp:Label>
                                    <asp:Label ID="lblDEFECTIVE" runat="server" Text='<%# Eval("RMARQD_DEFECTIVE") %>' class="default" Visible="false"></asp:Label>
                                    <asp:TextBox ID="UI_txtMaterialCost" runat="server" Text='<%# Eval("RMARQD_MATERIALCOST") %>' Style="display: none; width: 1px" class="default"></asp:TextBox>
                                </asp:TableCell>

                                <asp:TableCell HorizontalAlign="Center">
                                    <asp:CheckBox runat="server" ID="chhWaive" Checked='<%# Eval("RMARQD_WAIVE") %>' Visible="false" />
                                    <asp:Label runat="server" ID="UI_lblRMARQD_WAIVE"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="Center">
                                    <asp:CheckBox runat="server" ID="chkOption" Checked='<%# Eval("RMARQD_OPTION") %>' Visible="false" />
                                    <asp:Label runat="server" ID="UI_lblRMARQD_OPTION"></asp:Label>
                                </asp:TableCell>

                                <asp:TableCell>
                                    <asp:Label ID="txtNewPart" runat="server" Text='<%# Eval("RMARQD_NPARTNO") %>' class="default"></asp:Label>
                                    <asp:TextBox ID="txtOldPart" runat="server" Width="100" MaxLength="13" Text='<%# Eval("RMARQD_OPARTNO") %>' class="default" Visible="false"></asp:TextBox>
                                    <asp:TextBox ID="txtOldSerial" runat="server" Width="80px" Text='<%# Eval("RMARQD_OSERIALNO") %>' class="default" Visible="false"></asp:TextBox>
                                </asp:TableCell>

                                <asp:TableCell>
                                    <asp:Label ID="txtDescription" runat="server" Text='<%# Eval("RMARQD_DESC") %>' class="default"></asp:Label>
                                </asp:TableCell>

                                <asp:TableCell HorizontalAlign="Center">
                                    <asp:Label ID="txtLocation" runat="server" Text='<%# Eval("RMARQD_LOCATION") %>' class="default"></asp:Label>
                                </asp:TableCell>

                                <asp:TableCell HorizontalAlign="Center">
                                    <asp:DropDownList ID="cboImproper" runat="server" class="default" Visible="false">
                                        <asp:ListItem Value="1" Text="065_Yes"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="066_No" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label runat="server" ID="UI_RMARQD_IMPROPERUSAGE"></asp:Label>
                                </asp:TableCell>

                                <asp:TableCell HorizontalAlign="Center">
                                    <asp:Label ID="txtQty" runat="server" Text='<%# Eval("RMARQD_QTY") %>' class="default"></asp:Label>
                                </asp:TableCell>

                                <asp:TableCell>
                                    <asp:Label ID="txtPrice" runat="server" Text='<%# Eval("RMARQD_PRICE") %>' class="default"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </ItemTemplate>
                </asp:DataList>


                <table class="default" bordercolor="#c0c0c0" cellspacing="0" cellpadding="3" width="98%" border="0" id="table8">
                    <tr>
                        <td bgcolor="#ffeeb2" align="left" width="30%">
                            <asp:Label ID="uiLbl_Repair_Manpower" runat="server" Text="086_Man power" class="default"></asp:Label>
                            :
            <asp:Label ID="uiTxt_Repair_ManHour" runat="server" Text="0" class="default"></asp:Label>&nbsp;
            <asp:Label ID="uiLbl_CURRENCYCODE" runat="server" Text="057_hour" class="default" Visible="false"></asp:Label>
                        </td>

                        <td bgcolor="#ffeeb2" align="left" width="30%">
                            <asp:Label ID="uiLbl_Repair_Parts" runat="server" Text="087_Parts" class="default"></asp:Label>
                            <asp:Label ID="uiLbl_Repair_Parts_Delimited" runat="server" Text=" :" class="default"></asp:Label>
                            <asp:Label ID="uiLbl_Repair_PartsTotal" runat="server" Text="0" class="default"></asp:Label>
                        </td>

                        <td bgcolor="#ffeeb2" align="right">
                            <asp:Label ID="uiLbl_Repair_TotalText" runat="server" Text="088_Total Amount" class="default"></asp:Label>
                            <asp:Label ID="uiLbl_Repair_TotalText_Delimited" runat="server" Text=" :" class="default"></asp:Label>
                        </td>
                        <td bgcolor="#ffeeb2" align="left" width="20%">
                            <asp:Label ID="uiLbl_Repair_CurrencyCode1" runat="server" Font-Bold="true" class="default"></asp:Label>
                            <asp:Label ID="uiLbl_Repair_Total" runat="server" Text="0" Font-Bold="true" class="default"></asp:Label>
                        </td>
                    </tr>
                </table>

                <p></p>
                <p></p>
            </asp:Panel>

            <asp:TextBox ID="UI_CU_DISCOUNT_OFF" runat="server" Visible="false"></asp:TextBox>
            <asp:TextBox ID="UI_RMAD_ISWARRANTY" runat="server" Visible="false"></asp:TextBox>
            <asp:TextBox ID="UI_RMAD_ISCW" runat="server" Visible="false"></asp:TextBox>

            <uc2:ucMessage ID="ucMessage" runat="server" />

        </ContentTemplate>
    </asp:UpdatePanel>

    <uc1:ucProgressStatus ID="ucProgressStatus" runat="server" />

</asp:Content>

