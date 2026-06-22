<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="ChargeQuoted_Item.aspx.vb" Inherits="ChargeQuoted_Item" Title="Untitled Page" %>

<%@ Register Src="ascx/ucProgressStatus.ascx" TagName="ucProgressStatus" TagPrefix="uc1" %>
<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">

    <script type="text/javascript">
        function calActualAmount() {
            var oRMACQ_SALEQUOTE = window.document.getElementById('<%=uiTxt_RMACQ_SALEQUOTE.ClientID %>');
        var oRMACQ_DISCOUNT = window.document.getElementById('<%=uiTxt_RMACQ_DISCOUNT.ClientID %>');
        var oActualAmount = window.document.getElementById('<%=uiLbl_ActualAmount.ClientID %>');

            if (oRMACQ_SALEQUOTE != null && oRMACQ_DISCOUNT != null && oActualAmount != null) {
                if (Trim(oRMACQ_SALEQUOTE.value) != "" && Trim(oRMACQ_DISCOUNT.value) != "") {
                    var iRMACQ_SALEQUOTE = parseFloat(Trim(oRMACQ_SALEQUOTE.value));
                    var iRMACQ_DISCOUNT = parseFloat(Trim(oRMACQ_DISCOUNT.value));
                    oActualAmount.innerHTML = iRMACQ_SALEQUOTE - iRMACQ_DISCOUNT;
                }
            }
        }


        function calTotal_AfterDiscount() {
            var oGridView = document.getElementById('<%=UI_dvRequestDetail.ClientID %>');
            var oGridViewID = oGridView.id;
            var iRows = oGridView.rows.length;

            var total_DISCOUNTAMOUNT = 0;

            for (var i = 2; i <= iRows; i++) {
                var oQUOTE = document.getElementById(oGridViewID + '_ctl0' + i + '_UI_TotalAmount_Text');
                var oDISCOUNTAMOUNT = document.getElementById(oGridViewID + '_ctl0' + i + '_UITxt_RMACQSN_DISCOUNTAMOUNT');
                var oFooter_DISCOUNTAMOUNT = document.getElementById(oGridViewID + '_ctl0' + i + '_UI_Footer_DISCOUNTAMOUNT');

                if (oDISCOUNTAMOUNT != null) {
                    if (isNaN(oDISCOUNTAMOUNT.value) != true) {
                        //if (parseFloat(oDISCOUNTAMOUNT.value)>parseFloat(oQUOTE.innerText)){
                        //alert(_discountMessage);
                        //break;
                        //oDISCOUNTAMOUNT.value = oQUOTE.innerText;
                        //}

                        var iDISCOUNTAMOUNT = parseFloat(oDISCOUNTAMOUNT.value);
                        total_DISCOUNTAMOUNT = total_DISCOUNTAMOUNT + iDISCOUNTAMOUNT;
                    }
                }

                if (oFooter_DISCOUNTAMOUNT != null) {
                    oFooter_DISCOUNTAMOUNT.innerHTML = Round(total_DISCOUNTAMOUNT, 2);
                }
            }
        }

        function Validate_AfterDiscount(oSrc, args) {
            var retval = true;

            var Control01 = oSrc.getAttribute("Control01");
            var Control02 = oSrc.getAttribute("Control02");

            var oQUOTE = document.getElementById(Control01);
            var oDISCOUNTAMOUNT = document.getElementById(Control02);
            var re = /,/g;
            var oQUOTE_TEXT = oQUOTE.innerText.replace(re, "");
            if (oDISCOUNTAMOUNT != null) {
                if (isNaN(oDISCOUNTAMOUNT.value) != true) {
                    if (parseFloat(oDISCOUNTAMOUNT.value) > parseFloat(oQUOTE_TEXT)) {
                        retval = false;
                    }
                }
            }

            args.IsValid = retval;
        }


        function Round(value, dights) {
            var nNewValue = Math.round(value * Math.pow(10, dights)) / Math.pow(10, dights);
            return nNewValue;
        }
    </script>



    <uc1:ucProgressStatus ID="ucProgressStatus" runat="server" />

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
                                    <asp:Label ID="uiTag_Tittle" runat="server" Text="429_Wait for Processing" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
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
                                                <asp:Label ID="uiTag_RMANo" runat="server" Text="029_RMA No."></asp:Label></td>
                                            <td width="35%">:&nbsp;<asp:Label ID="uiLbl_RMANo" runat="server"></asp:Label></td>
                                            <td width="12%" align="right">
                                                <asp:Label ID="uiTag_RMAStauts" runat="server" Text="430_RMA Status"></asp:Label></td>
                                            <td width="30%" align="left">:&nbsp;<asp:Label ID="uiLbl_RMAStauts" runat="server"></asp:Label></td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <asp:Label ID="uiTag_AccountName" runat="server" Text="004_Account Name"></asp:Label></td>
                                            <td>:&nbsp;<asp:Label ID="uiLbl_AccountName" runat="server"></asp:Label></td>
                                            <td align="right">
                                                <asp:Label ID="uiTag_RequestDate" runat="server" Text="033_Request Date"></asp:Label></td>
                                            <td align="left">:&nbsp;<asp:Label ID="uiLbl_RequestDate" runat="server"></asp:Label></td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <asp:Label ID="uiTag_lblRepairCenter" runat="server" Text="009_Repair Center"></asp:Label></td>
                                            <td>:&nbsp;<asp:Label ID="uiLbl_RepairCenter" runat="server"></asp:Label></td>
                                        </tr>

                                        <tr style="height: 35px">
                                            <td>
                                                <asp:Label ID="uiTag_TotalAmount" runat="server" Text="088_Total Amount"></asp:Label></td>
                                            <td>:&nbsp;
			                            <asp:Label ID="uiLbl_RMACQ_SALEQUOTE" runat="server"></asp:Label>
                                                <asp:TextBox ID="uiTxt_RMACQ_SALEQUOTE" runat="server" Style="display: none; width: 1px"></asp:TextBox>
                                                <%--style="display: none; width: 1px"--%>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="uiTag_SalesName" runat="server" Text="431_Sales"></asp:Label></td>
                                            <td align="left">:&nbsp;<asp:Label ID="uiLbl_SalesName" runat="server"></asp:Label></td>
                                        </tr>

                                        <tr style="height: 35px">
                                            <td>
                                                <asp:Label ID="uiTag_DiscountOff" runat="server" Text="432_Discount Off"></asp:Label></td>
                                            <td>:&nbsp;
			                            <asp:Label ID="uiLbl_RMACQ_DISCOUNT" runat="server"></asp:Label>
                                                <asp:TextBox ID="uiTxt_RMACQ_DISCOUNT" runat="server" MaxLength="8" Width="80px" Visible="false"></asp:TextBox>
                                                &nbsp;&nbsp;&nbsp;&nbsp;
										<asp:Panel ID="UI_cmdModify_DiscountOff_Panel" runat="server" Visible="false">
                                            <asp:Button ID="UI_cmdModify_DiscountOff" runat="server" Text="435_DiscountOff" CssClass="Confirm_l" CausesValidation="false" Visible="false" />
                                        </asp:Panel>
                                            </td>

                                            <td align="right">
                                                <asp:Label ID="uiTag_Approval" runat="server" Text="433_Approval"></asp:Label></td>
                                            <td align="left">:&nbsp;<asp:Label ID="uiLbl_RMACQ_APPROVAL" runat="server"></asp:Label></td>
                                        </tr>

                                        <tr style="height: 35px">
                                            <td>
                                                <asp:Label ID="uiTag_ActualAmount" runat="server" Text="434_Actual Amount"></asp:Label></td>
                                            <td>:&nbsp;<asp:Label ID="uiLbl_ActualAmount" runat="server"></asp:Label></td>
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
                                    <!--[Begin]資料列表表單-->
                                    <!-- PagerSettings-Mode="Numeric" -->
                                    <asp:GridView ID="UI_dvRequestDetail" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" ShowFooter="true" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="UI_SeqID" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                    <asp:Label ID="UI_RMADID" runat="server" Text='<%# Eval("RMAD_ID") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="UI_RMADWARRANTY" runat="server" Text='<%# Eval("RMAD_WARRANTY") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="UI_RMADCSTMP" runat="server" Text='<%# Eval("RMAD_CSTMP") %>' Visible="false"></asp:Label>

                                                    <asp:Label ID="UI_RMASQ_CURRENCYCODE" runat="server" Text='<%# Eval("RMASQ_CURRENCYCODE") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="UI_RMASQ_CURRENCYRATE" runat="server" Text='<%# Eval("RMASQ_CURRENCYRATE") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="UI_RMACQSN_CURRENCYCODE" runat="server" Text='<%# Eval("RMACQSN_CURRENCYCODE") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="UI_RMACQSN_CURRENCYRATE" runat="server" Text='<%# Eval("RMACQSN_CURRENCYRATE") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="UI_RMAD_STATUS" runat="server" Text='<%# Eval("RMAD_STATUS") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="UI_RMARQ_ACCEPT" runat="server" Text='<%# Eval("RMARQ_ACCEPT") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="013_Serial Number">
                                                <HeaderStyle Width="10%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="UI_RMAD_SERIALNO" runat="server" Text='<%# Eval("RMAD_SERIALNO") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="035_Model">
                                                <HeaderStyle Width="10%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="UI_RMAD_MODELNO" runat="server" Text='<%# Eval("RMAD_MODELNO") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="015_Warranty">
                                                <HeaderStyle Width="8%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="UI_RMAD_ISWARRANTY_Text" runat="server"></asp:Label>
                                                    <asp:Label ID="UI_RMAD_ISWARRANTY" runat="server" Text='<%# Eval("RMAD_ISWARRANTY") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="064_Improper Usage">
                                                <HeaderStyle Width="10%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="UI_RMARQ_IMPROPERUSAGE_Text" runat="server"></asp:Label>
                                                    <asp:Label ID="UI_RMARQ_IMPROPERUSAGE" runat="server" Text='<%# Eval("RMARQ_IMPROPERUSAGE") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="023_Failure Reason">
                                                <HeaderStyle Width="8%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="UI_FailureReason_Text" runat="server"></asp:Label>
                                                    <asp:Label ID="UI_FARC_NAME1" runat="server" Text='<%# Eval("FARC_NAME1") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="UI_FARC_NAME2" runat="server" Text='<%# Eval("FARC_NAME2") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="125_Labor Cost">
                                                <HeaderStyle Width="8%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                <FooterStyle HorizontalAlign="Center" CssClass="text9pt" />
                                                <ItemTemplate>
                                                    <asp:Label ID="UI_ServiceCharge_Text" runat="server"></asp:Label>
                                                    <asp:Label ID="UI_RMASQ_LABORCOST" runat="server" Text='<%# Eval("RMASQ_LABORCOST") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="UI_RMACQSN_LABORCOST" runat="server" Text='<%# Eval("RMACQSN_LABORCOST") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="UI_Footer_ServiceCharge" Text=""></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="126_Material Cost">
                                                <HeaderStyle Width="8%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                <FooterStyle HorizontalAlign="Center" CssClass="text9pt" />
                                                <ItemTemplate>
                                                    <asp:Label ID="UI_MaterialCost_Text" runat="server"></asp:Label>
                                                    <asp:Label ID="UI_RMASQ_MATERIALCOST" runat="server" Text='<%# Eval("RMASQ_MATERIALCOST") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="UI_RMACQSN_MATERIALCOST" runat="server" Text='<%# Eval("RMACQSN_MATERIALCOST") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="UI_Footer_MaterialCost" Text=""></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="127_Total Amount">
                                                <HeaderStyle Width="8%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                <FooterStyle HorizontalAlign="Center" CssClass="text9pt" />
                                                <ItemTemplate>
                                                    <asp:Label ID="UI_TotalAmount_Text" runat="server"></asp:Label>
                                                    <asp:Label ID="UI_RMASQ_QUOTE" runat="server" Text='<%# Eval("RMASQ_QUOTE") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="UI_RMACQSN_QUOTE" runat="server" Text='<%# Eval("RMACQSN_QUOTE") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="UI_Footer_TotalAmount" Text=""></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="438_After Discount">
                                                <HeaderStyle Width="8%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                <FooterStyle HorizontalAlign="Center" CssClass="text9pt" />
                                                <ItemTemplate>
                                                    <asp:Label ID="UI_RMACQSN_DISCOUNTAMOUNT_Text" runat="server" Visible="false"></asp:Label>
                                                    <asp:TextBox ID="UITxt_RMACQSN_DISCOUNTAMOUNT" runat="server" MaxLength="8" Width="80px" Visible="true"></asp:TextBox>
                                                    <asp:Label ID="UI_RMACQSN_DISCOUNTAMOUNT" runat="server" Text='<%# Eval("RMACQSN_DISCOUNTAMOUNT") %>' Visible="false"></asp:Label>

                                                    <asp:RangeValidator ID="rvDISCOUNTAMOUNT" runat="server" ErrorMessage="136_輸入費用加總型態有誤" ControlToValidate="UITxt_RMACQSN_DISCOUNTAMOUNT" Display="None"
                                                        MinimumValue="0" MaximumValue="100000" Type="Double" ValidationGroup="ValidationGroup" SetFocusOnError="true"></asp:RangeValidator>

                                                    <asp:CustomValidator ID="cvAfterDiscount" runat="server" ClientValidationFunction="Validate_AfterDiscount"
                                                        ErrorMessage="454_discount金額不可大於原始金額" Display="None" ValidationGroup="ValidationGroup"></asp:CustomValidator>

                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="UI_Footer_DISCOUNTAMOUNT" Text=""></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="038_Detail">
                                                <HeaderStyle Width="8%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="UI_cmdDetail" ImageUrl="images/icon-detail.gif" runat="server" CommandArgument='<%#Me.UI_dvRequestDetail.Rows.Count%>' CommandName="cmdDetail" PostBackUrl="~/ChargeQuoted_PART.aspx" />
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
                    <td colspan="2" align="center">
                        <br />
                        <input id="UI_cmdBack" runat="server" type="button" value="006_back" class="Problem_Edit" onclick="javascript:history.back();" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="UI_cmdApply_DiscountOff" runat="server" Text="_Apply" CssClass="Confirm_l" OnClientClick="onProgress('Save')" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="UI_cmdConfirm_DiscountOff" runat="server" Text="_Confirm" CssClass="Confirm_l" OnClientClick="onProgress('Save')" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="UI_cmdApply" runat="server" Text="_Apply" CssClass="Confirm_l" ValidationGroup="ValidationGroup" OnClientClick="onProgress('Save')" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="UI_cmdSubmit" runat="server" Text="_Confirm" CssClass="Confirm_l" ValidationGroup="ValidationGroup" OnClientClick="onProgress('Save')" />
                        <asp:Button ID="UI_cmdSubmitFlow" runat="server" Text="_Confirm" CssClass="Confirm_l" ValidationGroup="ValidationGroup" OnClientClick="onProgress('Save')" />
                    </td>
                </tr>

            </table>

            <script type="text/javascript">
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(calTotal_AfterDiscount);
            </script>

            <asp:ValidationSummary ID="vsChareg" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="ValidationGroup" />

            <uc2:ucMessage ID="ucMessage" runat="server" />

            <asp:Label ID="UI_lblPreviousPage_RMANO" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblPreviousPage_RMAID" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblPreviousPage_RMADID" runat="server" Visible="false"></asp:Label>


            <asp:Label ID="hid_RMASQ_CURRENCYCODE" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="hid_RMASQ_CURRENCYRATE" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="hid_RMACQ_QUOTE_ORIGINAL" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="hid_RMACQ_CURRENCYCODE_ORIGINAL" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="hid_RMACQ_CURRENCYRATE_ORIGINAL" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="hid_CU_TIPTOP_ID" runat="server" Visible="false"></asp:Label>

            <!--150702 add by MaggieChen for chargequoted flow (b)-->
            <asp:Label ID="hid_RMASQ_SALEAD" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="hid_COMP_NO" runat="server" Visible="false"></asp:Label>
            <!--150702 add by MaggieChen for chargequoted flow (e)-->

            <asp:Label runat="server" ID="UI_Total_ServiceCharge" Text="0" Visible="false"></asp:Label>
            <asp:Label runat="server" ID="UI_Total_MaterialCost" Text="0" Visible="false"></asp:Label>
            <asp:Label runat="server" ID="UI_Total_TotalAmount" Text="0" Visible="false"></asp:Label>
            <asp:Label runat="server" ID="UI_Total_DISCOUNTAMOUNT" Text="0" Visible="false"></asp:Label>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>




