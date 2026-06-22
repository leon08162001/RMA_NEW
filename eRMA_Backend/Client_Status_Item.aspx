<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Client_Status_Item.aspx.vb" Inherits="Client_Status_Item" Title="Untitled Page" %>

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
                                    <table width="95%" border="0" cellspacing="1" cellpadding="0" class="default">
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="UI_lblRequestInformation" runat="server" Text="028_Search for Request Items Information" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td width="15%">
                                                <asp:Label ID="UI_lblRMANo" runat="server" Text="029_RMA No."></asp:Label></td>
                                            <td width="35%">:&nbsp;<asp:Label ID="UI_RMANo" runat="server"></asp:Label></td>
                                            <td width="12%" align="right">
                                                <asp:Label ID="UI_lblRequestDate" runat="server" Text="033_Request Date"></asp:Label></td>
                                            <td width="30%" align="left">:&nbsp;<asp:Label ID="UI_RequestDate" runat="server"></asp:Label></td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <asp:Label ID="UI_lblRepairCenter" runat="server" Text="009_Repair Center"></asp:Label></td>
                                            <td>:&nbsp;<asp:Label ID="UI_RepairCenter" runat="server"></asp:Label></td>
                                            <td align="right">
                                                <asp:Label ID="UI_lblApplicant" runat="server" Text="006_Applicant"></asp:Label></td>
                                            <td align="left">:&nbsp;<asp:Label ID="UI_Applicant" runat="server"></asp:Label></td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <asp:Label ID="UI_lblRemark" runat="server" Text="134_Remark"></asp:Label></td>
                                            <td colspan="3">:&nbsp;<asp:Label ID="UI_Remark" runat="server"></asp:Label></td>
                                        </tr>
                                    </table>
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
                                                    <asp:Label ID="UI_RMADWARRANTY" runat="server" Text='<%# Eval("RMAD_WARRANTY") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="UI_RMADCSTMP" runat="server" Text='<%# Eval("RMAD_CSTMP") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="UI_RMANO" runat="server" Text='<%# Eval("RMAD_RMANO") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="UI_RMADID" runat="server" Text='<%# Eval("RMAD_ID") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="RMAD_SERIALNO" HeaderText="013_Serial Number" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            <asp:BoundField DataField="RMAD_MODELNO" HeaderText="035_Model" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            <asp:BoundField DataField="WARRANTY" HeaderText="015_Warranty" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd}"></asp:BoundField>
                                            <asp:BoundField DataField="FailureReason" HeaderText="023_Failure Reason" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd}"></asp:BoundField>

                                            <asp:BoundField DataField="LaborCost" HeaderText="125_Labor Cost" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center" Visible="false"></asp:BoundField>
                                            <asp:BoundField DataField="MaterialCost" HeaderText="126_Material Cost" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center" Visible="false"></asp:BoundField>

                                            <asp:BoundField DataField="TotalAmount" HeaderText="127_Total Amount" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            <asp:BoundField DataField="Status" HeaderText="032_Status" HeaderStyle-Width="9%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            <asp:BoundField DataField="ShippedDate" HeaderText="141_Shipped Date" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            <asp:BoundField DataField="TrackingNo" HeaderText="140_Tracking No" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>

                                            <asp:TemplateField HeaderText="038_Detail">
                                                <HeaderStyle Width="8%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="UI_cmdDetail" ImageUrl="images/icon-detail.gif" runat="server" CommandName="cmdDetail" CommandArgument='<%# me.UI_dvRequest.Rows.Count%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
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
                <tr>
                    <td background="Images/pic_20.gif">&nbsp;</td>
                    <td valign="top" bgcolor="#E3D8BE" align="center">
                        <input id="UI_cmdBack" runat="server" type="button" value="006_back" class="Problem_Edit" onclick="javascript:history.back();" />
                    </td>
                </tr>
            </table>
            <uc1:ucClientDetail ID="ucClientDetail" runat="server" />
            <asp:Label ID="UI_lblPreviousPage_RMAID" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblPreviousPage_RMANO" runat="server" Visible="false"></asp:Label>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
