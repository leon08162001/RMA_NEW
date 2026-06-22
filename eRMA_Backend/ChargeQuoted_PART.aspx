<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="ChargeQuoted_PART.aspx.vb" Inherits="ChargeQuoted_PART" Title="Untitled Page" %>

<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <script type="text/javascript">

        function Validate_AfterDiscount(oSrc, args) {
            var retval = true;

            var Control01 = oSrc.getAttribute("Control01");
            var Control02 = oSrc.getAttribute("Control02");

            var oQUOTE = document.getElementById(Control01);
            var oDISCOUNTAMOUNT = document.getElementById(Control02);

            //   alert(oQUOTE.innerText);
            //   alert(oDISCOUNTAMOUNT.value);

            if (oDISCOUNTAMOUNT != null) {
                if (isNaN(oDISCOUNTAMOUNT.value) != true) {
                    if (parseFloat(oDISCOUNTAMOUNT.value) > parseFloat(oQUOTE.innerText)) {
                        retval = false;
                    }
                }
            }

            args.IsValid = retval;
        }
    </script>
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
                                                <asp:Label ID="uiTag_lblRMANo" runat="server" Text="029_RMA No."></asp:Label></td>
                                            <td width="35%">:&nbsp;<asp:Label ID="UI_RMANo" runat="server"></asp:Label></td>
                                            <td width="12%" align="right">
                                                <asp:Label ID="uiTag_lblRequestDate" runat="server" Text="033_Request Date"></asp:Label></td>
                                            <td width="30%" align="left">:&nbsp;<asp:Label ID="UI_RequestDate" runat="server"></asp:Label></td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <asp:Label ID="uiTag_RepairCenter" runat="server" Text="009_Repair Center"></asp:Label></td>
                                            <td>:&nbsp;<asp:Label ID="UI_RepairCenter" runat="server"></asp:Label></td>
                                            <td align="right">
                                                <asp:Label ID="uiTag_Applicant" runat="server" Text="006_Applicant"></asp:Label></td>
                                            <td align="left">:&nbsp;<asp:Label ID="UI_Applicant" runat="server"></asp:Label></td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <asp:Label ID="uiTag_SerialNumber" runat="server" Text="013_Serial Number"></asp:Label></td>
                                            <td>:&nbsp;<asp:Label ID="uiLbl_SerialNumber" runat="server"></asp:Label></td>

                                            <td align="right">
                                                <asp:Label ID="uiTag_RenewDate" runat="server" Text="443_Renew Date"></asp:Label></td>
                                            <td align="left">:&nbsp;<asp:Label ID="uiLbl_RenewDate" runat="server"></asp:Label></td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <asp:Label ID="uiTag_ApprovalStatus" runat="server" Text="444_Approval Status"></asp:Label></td>
                                            <td>:&nbsp;<asp:Label ID="uiLbl_ApprovalStatus_Text" runat="server"></asp:Label>
                                                <asp:Label ID="uiLbl_ApprovalStatus" runat="server" Visible="false"></asp:Label>
                                            </td>

                                            <td align="right">
                                                <asp:Label ID="uiTag_ApprovalDate" runat="server" Text="445_Approval Date"></asp:Label></td>
                                            <td align="left">:&nbsp;<asp:Label ID="uiLbl_ApprovalDate" runat="server"></asp:Label></td>
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

                                                    <asp:TableHeaderCell Width="6%" HorizontalAlign="Center">
                                                        <asp:Label ID="lblHPart" runat="server" Text="083_Part'sNo"></asp:Label>
                                                    </asp:TableHeaderCell>

                                                    <asp:TableHeaderCell Width="28%" HorizontalAlign="Center">
                                                        <asp:Label ID="lblHDescription" runat="server" Text="099_Description"></asp:Label>
                                                    </asp:TableHeaderCell>

                                                    <asp:TableHeaderCell Width="5%" HorizontalAlign="Center">
                                                        <asp:Label ID="lblHQty" runat="server" Text="103_Qty"></asp:Label>
                                                    </asp:TableHeaderCell>

                                                    <asp:TableHeaderCell Width="6%" HorizontalAlign="Center">
                                                        <asp:Label ID="lblOriginalCharge" runat="server" Text="446_Original Charge"></asp:Label>
                                                    </asp:TableHeaderCell>

                                                    <asp:TableHeaderCell Width="6%" HorizontalAlign="Center">
                                                        <asp:Label ID="lblRenewCharge" runat="server" Text="447_Renew Charge"></asp:Label>
                                                    </asp:TableHeaderCell>
                                                </asp:TableHeaderRow>
                                            </asp:Table>
                                        </HeaderTemplate>

                                        <ItemTemplate>
                                            <asp:Table ID="oTableRow" runat="server">
                                                <asp:TableRow>
                                                    <asp:TableCell HorizontalAlign="Center">
                                                        <asp:Label ID="lblSeq" runat="server" class="default" Text='<%# Container.ItemIndex+1 %>'></asp:Label>

                                                        <asp:Label ID="UI_RMARQD_ID" runat="server" Text='<%# Eval("RMARQD_ID") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMARQD_RMADID" runat="server" Text='<%# Eval("RMARQD_RMADID") %>' Visible="false"></asp:Label>

                                                        <asp:Label ID="UI_RMARQD_QTY" runat="server" Text='<%# Eval("RMARQD_QTY") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMARQD_MATERIALCOST" runat="server" Text='<%# Eval("RMARQD_MATERIALCOST") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMARQD_PRICE" runat="server" Text='<%# Eval("RMARQD_PRICE") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMARQD_CURRENCYCODE" runat="server" Text='<%# Eval("RMARQD_CURRENCYCODE") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMARQD_CURRENCYRATE" runat="server" Text='<%# Eval("RMARQD_CURRENCYRATE") %>' Visible="false"></asp:Label>

                                                        <asp:Label ID="UI_RMACQPT_QTY" runat="server" Text='<%# Eval("RMACQPT_QTY") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMACQPT_MATERIALCOST" runat="server" Text='<%# Eval("RMACQPT_MATERIALCOST") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMACQPT_PRICE" runat="server" Text='<%# Eval("RMACQPT_PRICE") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMACQPT_CURRENCYCODE" runat="server" Text='<%# Eval("RMACQPT_CURRENCYCODE") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMACQPT_CURRENCYRATE" runat="server" Text='<%# Eval("RMACQPT_CURRENCYRATE") %>' Visible="false"></asp:Label>

                                                        <asp:Label ID="UI_RMACQPT_QTY_ORIGINAL" runat="server" Text='<%# Eval("RMACQPT_QTY_ORIGINAL") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMACQPT_MATERIALCOST_ORIGINAL" runat="server" Text='<%# Eval("RMACQPT_MATERIALCOST_ORIGINAL") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMACQPT_PRICE_ORIGINAL" runat="server" Text='<%# Eval("RMACQPT_PRICE_ORIGINAL") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMACQPT_CURRENCYCODE_ORIGINAL" runat="server" Text='<%# Eval("RMACQPT_CURRENCYCODE_ORIGINAL") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMACQPT_CURRENCYRATE_ORIGINAL" runat="server" Text='<%# Eval("RMACQPT_CURRENCYRATE_ORIGINAL") %>' Visible="false"></asp:Label>

                                                    </asp:TableCell>

                                                    <asp:TableCell HorizontalAlign="Center">
                                                        <asp:Label ID="lblRMARQD_NPARTNO" runat="server" Text='<%# Eval("RMARQD_NPARTNO") %>' class="default"></asp:Label>
                                                    </asp:TableCell>

                                                    <asp:TableCell HorizontalAlign="Center">
                                                        <asp:Label ID="txtDescription" runat="server" Text='<%# Eval("RMARQD_DESC") %>' class="default" Style="width: 98%"></asp:Label>
                                                    </asp:TableCell>

                                                    <asp:TableCell HorizontalAlign="Center">
                                                        <asp:Label ID="lblRMARQD_QTY" runat="server" class="default"></asp:Label>
                                                    </asp:TableCell>

                                                    <asp:TableCell HorizontalAlign="Center">
                                                        <asp:Label ID="lblRMARQD_PRICE" runat="server" class="default"></asp:Label>
                                                    </asp:TableCell>

                                                    <asp:TableCell HorizontalAlign="Center">
                                                        <asp:Label ID="UILbl_RMACQPT_RECHARGE_PRICE" runat="server" Visible="false"></asp:Label>
                                                        <asp:TextBox runat="server" ID="UI_RMACQPT_RECHARGE_PRICE" MaxLength="8" Width="80px" Visible="false"></asp:TextBox>

                                                        <asp:CustomValidator ID="cvAfterDiscount" runat="server" ClientValidationFunction="Validate_AfterDiscount"
                                                            ErrorMessage="454_discount金額不可大於原始金額" Display="None" ValidationGroup="ValidationGroup"></asp:CustomValidator>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>
                                        </ItemTemplate>
                                    </asp:DataList>

                                </td>
                            </tr>


                        </table>


                        <table class="default" bordercolor="#c0c0c0" cellspacing="0" cellpadding="3" width="60%" border="0" id="table8">
                            <tr>
                                <td bgcolor="#E3D8BE" align="left" width="30%">
                                    <asp:Label ID="lbl_ServiceCharge" runat="server" Text="404_Service Charge" class="default"></asp:Label>
                                    :
            <asp:Label ID="UI_RMACQSN_LABORCOST" runat="server" Text="0" class="default" Font-Bold="true"></asp:Label>
                                </td>

                                <td bgcolor="#E3D8BE" align="left" width="30%">
                                    <asp:Label ID="uiLbl_MaterialCharge" runat="server" Text="448_Material Charge" class="default"></asp:Label>
                                    <asp:Label ID="uiLbl_Parts_Delimited" runat="server" Text=" :" class="default"></asp:Label>
                                    <asp:Label ID="UI_RMACQSN_MATERIALCOST" runat="server" Text="0" class="default" Font-Bold="true"></asp:Label>
                                </td>

                                <td bgcolor="#E3D8BE" align="right">
                                    <asp:Label ID="uiLbl_TotalCharge" runat="server" Text="449_Total Amount" class="default"></asp:Label>
                                    <asp:Label ID="uiLbl_TotalAmountText_Delimited" runat="server" Text=" :" class="default"></asp:Label>
                                </td>

                                <td bgcolor="#E3D8BE" align="left" width="20%">
                                    <asp:Label ID="UI_RMACQSN_QUOTE" runat="server" Text="0" Font-Bold="true" class="default"></asp:Label>
                                </td>
                            </tr>
                        </table>

                    </td>
                </tr>



                <tr>
                    <td background="Images/pic_20.gif">&nbsp;</td>
                    <td valign="top" bgcolor="#E3D8BE" align="center">
                        <input id="UI_cmdBack" runat="server" type="button" value="006_back" class="Problem_Edit" onclick="javascript:history.back();" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="UI_cmdApply" runat="server" Text="_Apply" CssClass="Confirm_l" ValidationGroup="ValidationGroup" OnClientClick="onProgress('Save')" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="UI_cmdConfirm" runat="server" Visible="false" Text="_Confirm" CssClass="Confirm_l" ValidationGroup="ValidationGroup" OnClientClick="onProgress('Save')" />
                    </td>
                </tr>


            </table>

            <asp:ValidationSummary ID="vsChareg" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="ValidationGroup" />

            <uc2:ucMessage ID="ucMessage" runat="server" />

            <asp:Label ID="Label1" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="Label2" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="Label3" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblRMAD_STATUS" runat="server" Visible="false"></asp:Label>

            <asp:Label ID="UI_CU_DISCOUNT_OFF" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_RMAD_ISWARRANTY" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_RMAD_ISCW" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_RMA_CUNO" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="hid_CU_TIPTOP_ID" runat="server" Visible="false"></asp:Label>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ucMessage" />
            <asp:PostBackTrigger ControlID="UI_cmdApply" />
            <asp:PostBackTrigger ControlID="UI_cmdConfirm" />
        </Triggers>
    </asp:UpdatePanel>


    <asp:Label ID="UI_lblPreviousPage_RMANO" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="UI_lblPreviousPage_RMAID" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="UI_lblPreviousPage_RMADID" runat="server" Visible="false"></asp:Label>

</asp:Content>

