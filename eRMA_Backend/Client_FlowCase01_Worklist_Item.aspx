<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Client_FlowCase01_Worklist_Item.aspx.vb" Inherits="Client_FlowCase01_Worklist_Item" Title="Untitled Page" ValidateRequest="false" %>

<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>
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
                                            <td width="15%">
                                                <asp:Label ID="UI_lblRMANo" runat="server" Text="029_RMA No."></asp:Label></td>
                                            <td width="35%">:&nbsp;<asp:Label ID="UI_RMANo" runat="server"></asp:Label></td>
                                            <td width="12%" align="right">
                                                <asp:Label ID="UI_lblRequestDate" runat="server" Text="033_Request Date"></asp:Label></td>
                                            <td width="30%" align="left">:&nbsp;<asp:Label ID="UI_RequestDate" runat="server"></asp:Label></td>
                                            <td rowspan="2">
                                                <asp:Button ID="UI_btnDetail" CssClass="Button_3" Font-Size="Large" Text="Repair Detail" runat="server" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <asp:Label ID="UI_lblRepairCenter" runat="server" Text="009_Repair Center"></asp:Label></td>
                                            <td>:&nbsp;<asp:Label ID="UI_RepairCenter" runat="server"></asp:Label></td>
                                            <td align="right">
                                                <asp:Label ID="UI_lblApplicant" runat="server" Text="006_Applicant"></asp:Label></td>
                                            <td align="left">:&nbsp;<asp:Label ID="UI_Applicant" runat="server"></asp:Label></td>
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

                <tr>
                    <td background="Images/pic_20.gif">&nbsp;</td>
                    <td valign="top" bgcolor="#E3D8BE" align="center">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="1%">&nbsp;</td>
                                <td width="99%" align="left" class="default">

                                    <asp:DataList ID="UI_dvRepairDetail" runat="server" ExtractTemplateRows="true" BorderColor="#c0c0c0" CellSpacing="0" CellPadding="3" Width="98%" border="1" CssClass="default" bordercolorlight="#c0c0c0">
                                        <HeaderTemplate>
                                            <asp:Table ID="oTableHeader" runat="server">
                                                <asp:TableHeaderRow bgcolor="#fff4d0">
                                                    <asp:TableHeaderCell Width="2%">&nbsp;</asp:TableHeaderCell>
                                                    <asp:TableHeaderCell Width="3%" Visible="false">
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
                                                        <asp:Label ID="lblRMARQD_RMADID" runat="server" Text='<%# Eval("RMARQD_RMADID") %>' class="default" Visible="false"></asp:Label>

                                                        <asp:Label ID="lblIMPROPERUSAGE" runat="server" Text='<%# Eval("RMARQD_IMPROPERUSAGE") %>' class="default" Visible="false"></asp:Label>
                                                        <asp:Label ID="lblDEFECTIVE" runat="server" Text='<%# Eval("RMARQD_DEFECTIVE") %>' class="default" Visible="false"></asp:Label>
                                                        <asp:TextBox ID="UI_txtMaterialCost" runat="server" Text='<%# Eval("RMARQD_MATERIALCOST") %>' Style="display: none; width: 1px" class="default"></asp:TextBox>
                                                        <asp:Label ID="lblRMARQD_CURRENCYCODE" runat="server" Text='<%# Eval("RMARQD_CURRENCYCODE") %>' class="default" Visible="false"></asp:Label>
                                                        <asp:Label ID="lblRMARQD_CURRENCYRATE" runat="server" Text='<%# Eval("RMARQD_CURRENCYRATE") %>' class="default" Visible="false"></asp:Label>

                                                        <asp:Label ID="lblRMARQD_ASSIGECURRENCYCODE" runat="server" Text='<%# Eval("RMARQD_ASSIGECURRENCYCODE") %>' class="default" Visible="false"></asp:Label>
                                                        <asp:Label ID="lblRMARQD_ASSIGECURRENCYRATE" runat="server" Text='<%# Eval("RMARQD_ASSIGECURRENCYRATE") %>' class="default" Visible="false"></asp:Label>

                                                        <asp:Label ID="UI_RMARQD_WAIVE" runat="server" Text='<%# Eval("RMARQD_WAIVE") %>' class="default" Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMARQD_ACC" runat="server" Text='<%# Eval("RMARQD_ACC") %>' class="default" Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMARQD_OPTION" runat="server" Text='<%# Eval("RMARQD_OPTION") %>' class="default" Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMARQD_OPTIONCLIENT" runat="server" Text='<%# Eval("RMARQD_OPTIONCLIENT") %>' class="default" Visible="false"></asp:Label>

                                                        <asp:Label ID="lblRMARQD_AD" runat="server" Text='<%# Eval("RMARQD_AD") %>' class="default" Visible="false"></asp:Label>
                                                        <asp:Label ID="lblRMARQD_ADNAME" runat="server" Text='<%# Eval("RMARQD_ADNAME") %>' class="default" Visible="false"></asp:Label>
                                                        <asp:Label ID="lblRMARQD_CSTMP" runat="server" Text='<%# Eval("RMARQD_CSTMP") %>' class="default" Visible="false"></asp:Label>

                                                    </asp:TableCell>

                                                    <asp:TableCell HorizontalAlign="Center" Visible="false">
                                                        <asp:CheckBox runat="server" ID="chhWaive" Checked='<%# Eval("RMARQD_WAIVE") %>' />
                                                    </asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Center">
                                                        <asp:CheckBox runat="server" ID="UI_chkOptionClent" />
                                                    </asp:TableCell>

                                                    <asp:TableCell HorizontalAlign="Center">
                                                        <asp:Label ID="lblRMARQD_NPARTNO" runat="server" Width="100" MaxLength="13" Text='<%# Eval("RMARQD_NPARTNO") %>' class="default"></asp:Label>
                                                        <asp:Label ID="txtOldPart" runat="server" Width="100" MaxLength="13" Text='<%# Eval("RMARQD_OPARTNO") %>' class="default" Visible="false"></asp:Label>
                                                        <asp:Label ID="txtOldSerial" runat="server" Width="80px" Text='<%# Eval("RMARQD_OSERIALNO") %>' class="default" Visible="false"></asp:Label>
                                                    </asp:TableCell>

                                                    <asp:TableCell HorizontalAlign="Center">
                                                        <asp:Label ID="txtDescription" runat="server" Text='<%# Eval("RMARQD_DESC") %>' class="default" Style="width: 98%"></asp:Label>
                                                    </asp:TableCell>

                                                    <asp:TableCell HorizontalAlign="Center">
                                                        <asp:Label ID="txtLocation" runat="server" Text='<%# Eval("RMARQD_LOCATION") %>' class="default"></asp:Label>
                                                    </asp:TableCell>

                                                    <asp:TableCell HorizontalAlign="Center">
                                                        <asp:Label ID="UI_Improper" runat="server" class="default"></asp:Label>
                                                    </asp:TableCell>

                                                    <asp:TableCell HorizontalAlign="Center">
                                                        <asp:Label ID="lblRMARQD_QTY" runat="server" Text='<%# Eval("RMARQD_QTY") %>' class="default"></asp:Label>
                                                    </asp:TableCell>

                                                    <asp:TableCell>
                                                        <asp:Label ID="lblRMARQD_PRICE" runat="server" Text='<%# Eval("RMARQD_PRICE") %>' class="default"></asp:Label>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>
                                        </ItemTemplate>
                                    </asp:DataList>

                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td background="Images/pic_20.gif">&nbsp;</td>
                    <td valign="top" bgcolor="#E3D8BE" align="center">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="20%">&nbsp;</td>
                                <td width="60%" align="left" class="default">

                                    <table class="default" bordercolor="#c0c0c0" cellspacing="0" cellpadding="3" width="98%" border="0" id="table8">
                                        <tr>
                                            <td bgcolor="#E3D8BE" align="left" width="30%">
                                                <asp:Label ID="lbl_Manpower" runat="server" Text="086_Man power" class="default"></asp:Label>
                                                :
            <asp:Label ID="UI_RMARQ_LABORHOUR" runat="server" Text="0" class="default" Font-Bold="true"></asp:Label>
                                            </td>

                                            <td bgcolor="#E3D8BE" align="left" width="30%">
                                                <asp:Label ID="uiLbl_Parts" runat="server" Text="087_Parts" class="default"></asp:Label>
                                                <asp:Label ID="uiLbl_Parts_Delimited" runat="server" Text=" :" class="default"></asp:Label>
                                                <asp:Label ID="UI_RMARQ_MATERIALCOST" runat="server" Text="0" class="default" Font-Bold="true"></asp:Label>
                                            </td>

                                            <td bgcolor="#E3D8BE" align="right">
                                                <asp:Label ID="uiLbl_TotalAmountText" runat="server" Text="088_Total Amount" class="default"></asp:Label>
                                                <asp:Label ID="uiLbl_TotalAmountText_Delimited" runat="server" Text=" :" class="default"></asp:Label>
                                            </td>

                                            <td bgcolor="#E3D8BE" align="left" width="20%">
                                                <asp:Label ID="UI_RMARQ_QUOTE" runat="server" Text="0" Font-Bold="true" class="default"></asp:Label>

                                                <asp:Label ID="UI_RMARQ_ID" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="UI_RMARQ_LABORPRICE" runat="server" class="default" Visible="false"></asp:Label>
                                                <asp:Label ID="UI_RMARQ_CURRENCYRATE" runat="server" class="default" Visible="false"></asp:Label>
                                                <asp:Label ID="UI_RMARQ_ASSIGECURRENCYRATE" runat="server" class="default" Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                                <td width="20%">&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td background="Images/pic_20.gif">&nbsp;</td>
                    <td valign="top" bgcolor="#E3D8BE" align="center">
                        <input id="UI_cmdBack" runat="server" type="button" value="006_back" class="Problem_Edit" onclick="javascript:history.back();" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="UI_cmdConfirm" runat="server" Text="_Confirm" CssClass="Confirm_l" OnClientClick="onProgress('Save')" />
                    </td>
                </tr>
            </table>

            <uc2:ucMessage ID="ucMessage" runat="server" />
            <uc1:ucClientDetail ID="ucClientDetail" runat="server" />
            <asp:Label ID="UI_lblPreviousPage_RMADID" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblPreviousPage_RMAID" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblPreviousPage_RMANO" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblRMAD_STATUS" runat="server" Visible="false"></asp:Label>

            <asp:Label ID="UI_CU_DISCOUNT_OFF" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_RMAD_ISWARRANTY" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_RMARQ_IMPROPERUSAGE" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_RMAD_ISCW" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_RMA_CUNO" runat="server" Visible="false"></asp:Label>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ucMessage" />
            <asp:PostBackTrigger ControlID="UI_cmdConfirm" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>




