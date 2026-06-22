<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="RMARepair_Edit.aspx.vb" Inherits="RMARepair_Edit" Title="Untitled Page" ValidateRequest="false" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ PreviousPageType VirtualPath="Repair_WorkList.aspx" %>
<%@ Register Src="ascx/ucProgressStatus.ascx" TagName="ucProgressStatus" TagPrefix="uc1" %>
<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>
<%@ Register Src="ascx/ucRepairRarts.ascx" TagName="ucRepairRarts" TagPrefix="uc5" %>
<%@ Register Src="ascx/ucClientDetailPur.ascx" TagName="ucClientDetailPur" TagPrefix="uc7" %>
<%@ Register Src="ascx/ucSpecialSetting.ascx" TagName="ucSpecialSetting" TagPrefix="uc8" %>
<%@ Register Src="ascx/UcSDCViewG.ascx" TagName="UcSDCViewG" TagPrefix="uc10" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register Src="ascx/ucClientDetail.ascx" TagName="ucClientDetail" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <script type="text/javascript" language="javascript">
        function FrmDelete() {
            if (confirm(delMsg)) {
                onProgress('Delete')
                return true;
            }
            return false;
        }

        function FrmSave() {
            var isChecked = false;
            var UI_dvRequest = document.getElementById('<%=UI_dvRequest.ClientID %>');
            var UI_dvRequestID = UI_dvRequest.id;
            var iRows = UI_dvRequest.rows.length;

            for (var i = 1; i <= iRows; i++) {
                var UI_Check = document.getElementById(UI_dvRequestID + '_ctl0' + i + '_UI_Check');
                if (UI_Check != null) {
                    if (UI_Check.checked == true) {
                        isChecked = true;
                        break;
                    }
                }
            }

            if (isChecked == true) {
                if (confirm(doubleConfirmMsg)) {
                    onProgress('Save')
                    return true;
                }
                return false;
            }

        }


        function SerialSearch() {
            if (event.keyCode == 13) {
                var vs = document.getElementById('ctl00_ContentPlaceHolder_btnQuickSearch');
                vs.focus();
                vs.click();
            }
        }

        function cal_subTotalAMT() {
            var blnFlag_Cal = false;
            var iPrice = 0;
            var iTotalParts = 0;

            var oGridView = document.getElementById('<%=UI_dvRepairDetail.ClientID %>');
            var oGridViewID = oGridView.id;
            var iRows = oGridView.rows.length;

            for (var i = 1; i < iRows; i++) {
                var isSOURCE = document.getElementById(oGridViewID + '_ctl0' + i + '_UI_RMARED_ISSOURCE');
                var oQty = document.getElementById(oGridViewID + '_ctl0' + i + '_txtQty');
                var oMaterialCost = document.getElementById(oGridViewID + '_ctl0' + i + '_UI_txtMaterialCost');

                var txtRMARED_PRICE = document.getElementById(oGridViewID + '_ctl0' + i + '_UI_txtRMARED_PRICE');
                var lblRMARED_PRICE = document.getElementById(oGridViewID + '_ctl0' + i + '_UI_lblRMARED_PRICE');

                if (oQty != null && oMaterialCost != null) {
                    if (isNaN(oQty.value) != true && isNaN(oMaterialCost.value) != true) {
                        if (isSOURCE.value == "1") {
                            iPrice = parseFloat(oQty.value) * parseFloat(oMaterialCost.value);
                            iPrice = Round(iPrice, 2);
                        } else {
                            iPrice = Round(parseFloat(txtRMARED_PRICE.value), 2);
                        }

                        txtRMARED_PRICE.value = iPrice;
                        if (lblRMARED_PRICE != null) {
                            lblRMARED_PRICE.innerHTML = iPrice;
                        }
                        iTotalParts = iTotalParts + iPrice;
                        blnFlag_Cal = true;
                    }
                }
            }


            var UI_lblPartsTotal = window.document.getElementById('<%=UI_lblPartsTotal.ClientID %>');
            var UI_txtPartsTotal = window.document.getElementById('<%=UI_txtPartsTotal.ClientID %>');
            if (blnFlag_Cal == true) {
                UI_lblPartsTotal.innerHTML = Round(iTotalParts, 2);
                UI_txtPartsTotal.value = Round(iTotalParts, 2);
            }

            cal_TotalAMT();
        }


        function cal_TotalAMT() {
            var blnFlag_Cal = false;
            var iTotalManAmt = 0;
            var iManHour = window.document.getElementById('<%=UI_txtManHour.ClientID %>').value;
            var iLABORPRICE = window.document.getElementById('<%=UI_txtLABORPrice.ClientID %>').value;
            var UI_lblLaborCost = window.document.getElementById('<%=UI_lblLaborCost.ClientID %>');

            UI_lblLaborCost.innerText = "";
            if (iManHour != "" && iLABORPRICE != "") {
                if (isNaN(iManHour) != true && isNaN(iLABORPRICE) != true) {
                    iTotalManAmt = parseFloat(iManHour) * parseFloat(iLABORPRICE);
                    iTotalManAmt = Round(iTotalManAmt, 2);
                    UI_lblLaborCost.innerHTML = iTotalManAmt;
                    blnFlag_Cal = true;
                }
            }

            var iTotalParts = 0;
            var UI_txtPartsTotal = window.document.getElementById('<%=UI_txtPartsTotal.ClientID %>').value;
            if (UI_txtPartsTotal != "") {
                if (isNaN(UI_txtPartsTotal) != true) {
                    iTotalParts = parseFloat(UI_txtPartsTotal);
                    iTotalParts = Round(iTotalParts, 2);
                    blnFlag_Cal = true;
                }
            }

            var UI_lblTotal = window.document.getElementById('<%=UI_lblTotal.ClientID %>');
            if (blnFlag_Cal == true) {
                UI_lblTotal.innerHTML = Round(iTotalManAmt + iTotalParts, 2);
            }

        }



        function Round(value, dights) {
            var nNewValue = Math.round(value * Math.pow(10, dights)) / Math.pow(10, dights);
            return nNewValue;
        }

    </script>
    <uc1:ucProgressStatus ID="ucProgressStatus" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table border="0" width="100%" id="table4" cellspacing="0" cellpadding="0" height="100%">
                <tr>
                    <td width="24" background="Images/pic_12.gif">&nbsp;</td>
                    <td valign="top" align="left">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0" height="100%">
                            <!--[Begin]Tittle-->
                            <tr>
                                <td width="3%">&nbsp;</td>
                                <td width="94%" align="left">
                                    <asp:Label ID="UI_lblTittle" runat="server" Text="111_Repair Detail" CssClass="text_tittle"
                                        ForeColor="#326B9B"></asp:Label>
                                </td>
                                <td width="3%">&nbsp;</td>
                            </tr>
                            <!--[End]Tittle-->
                            <!--[Begin]新增資料-->
                            <tr>
                                <td>&nbsp;</td>
                                <td align="left">
                                    <table width="100%" align="center" border="0" cellspacing="1" class="default">
                                        <tr>
                                            <td>&nbsp;<asp:Label ID="UI_lblRMANo" runat="server" Text="029_RMA No."></asp:Label>
                                            </td>
                                            <td colspan="5">:
                                                <asp:Label ID="UI_RMANO" runat="server" class="default"></asp:Label>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>&nbsp;
                                                <asp:Label ID="UI_lblModelNo" runat="server" Text="012_ModelNo" class="default"></asp:Label>
                                            </td>
                                            <td colspan="5">:
                                                <asp:Label ID="UI_lblModelNoText" runat="server" class="default"></asp:Label>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td width="18%">&nbsp;
                                                <asp:Label ID="UI_lblSerial" runat="server" Text="013_Serial Number" class="default"></asp:Label>
                                            </td>
                                            <td width="35%">:
                                                <asp:Label ID="UI_lblSerialText" runat="server" Visible="false" class="default"></asp:Label>
                                                <asp:Label ID="UI_lblShowSerial" runat="server" class="default"></asp:Label>
                                            </td>
                                            <td width="15%" align="right" colspan="3">
                                                <asp:Label ID="UI_lblCustomer" runat="server" Text="030_Customer" class="default"></asp:Label>
                                            </td>
                                            <td width="35%" align="left">:
                                                <asp:Label ID="UI_lblCustomerText" runat="server" class="default"></asp:Label>
                                            </td>
                                        </tr>
                                        <%--					            <tr>
				                    <td>&nbsp;
				                        <asp:Label ID="UI_lblDuty" runat="server" Text="051_Duty"></asp:Label>.
				                    </td>
				                    <td colspan="3">:
				                        <asp:DropDownList ID="UI_cboDuty" runat="server" ></asp:DropDownList>
                                    </td>
				                </tr>
                                        --%>
                                        <tr runat="server" id="uiTR_EstimatedAmount">
                                            <td>&nbsp;
                                                <asp:Label ID="UI_lblLaborHour" runat="server" Text="112_Man Hour" class="default"></asp:Label>
                                            </td>
                                            <td>:
                                                <asp:Label ID="UI_lblLaborHourvalue" runat="server" class="default"></asp:Label>&nbsp;
                                                <asp:Label ID="UI_lblLaborHourText" runat="server" Text="057_hour" Visible="false"
                                                    class="default"></asp:Label>
                                            </td>
                                            <td align="right" colspan="3">
                                                <asp:Label ID="UI_lblQuote" runat="server" Text="113_Estimated Amount" class="default"></asp:Label>
                                            </td>
                                            <td align="left">:
                                                <asp:Label ID="UI_lblQuoteCode" runat="server" class="default"></asp:Label>
                                                <asp:Label ID="UI_lblQuoteText" runat="server" class="default"></asp:Label>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>&nbsp;
                                                <asp:Label ID="UI_lblFailure" runat="server" Text="023_Failure Reason" class="default"></asp:Label>
                                            </td>
                                            <td colspan="3" style="width: 232px">:
                                                <asp:Label ID="UI_lblFailureText" runat="server" class="default"></asp:Label>
                                                <asp:Label ID="UI_lblFARCNO" runat="server" Visible="false" class="default"></asp:Label>
                                                <asp:Label ID="UI_lblFARNO" runat="server" Visible="false" class="default"></asp:Label>
                                            </td>
                                </td>
                                <td valign="top" align="left" colspan="2" rowspan="6">
                                    <font size="1">
                                        <uc10:UcSDCViewG ID="UcSDCViewG" runat="server" />
                                    </font>
                                </td>

                            </tr>
                            <tr class="default" style="display: none">
                                <td align="left">&nbsp;
                                                <asp:Label ID="UI_lblProductDesc" runat="server" Text="196_Product Desc" class="default"></asp:Label>.
                                </td>
                                <td align="left" colspan="3">:
                                                <asp:Label ID="UI_lblProductDescText" runat="server" class="default"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                                <asp:Label ID="UI_lblProblemDesc" runat="server" Text="025_Problem Description" class="default"></asp:Label>
                                </td>
                                <td colspan="3">:
                                                <asp:Label ID="UI_lblProblemDescText" runat="server" class="default"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                                <asp:Label ID="UI_lblRepairDesc" runat="server" Text="053_Repair Description" class="default"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="UI_txtRepairDesc" runat="server" TextMode="MultiLine" Rows="3" Columns="70"
                                        class="default"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                                <asp:Label ID="UI_lblRepairMemo" runat="server" Text="054_Repair Memo" class="default"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="UI_txtRepairMemo" runat="server" TextMode="MultiLine" Rows="3" Columns="70"
                                        class="default"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">&nbsp;</td>
                                <td colspan="3">&nbsp;</td>
                            </tr>
                        </table>
                    </td>

                </tr>
                <!--[End]新增資料-->
            </table>
            </td>
                </tr>
                <tr>
                    <td width="24" height="27" background="Images/pic_14.gif">&nbsp;</td>
                    <td height="27" background="Images/pic_15.gif">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="99%" class="default">
                                    <table border="0" width="100%" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td width="100%" align="left">
                                                <asp:Label ID="UI_lblInformationTittle" runat="server" Text="082_Replace Component"
                                                    Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            <tr>
                <td width="24" background="Images/pic_20.gif">&nbsp;</td>
                <td valign="top" bgcolor="#E3D8BE" align="center" colspan="2">
                    <!-- List -->
                    <div class="form_div" align="center">
                        <fieldset>
                            <table class="default" style="border-collapse: collapse" bordercolor="#111111" cellspacing="0"
                                cellpadding="5" width="100%" border="0">
                                <tr>
                                    <td>
                                        <table id="table6" class="default" cellspacing="0" cellpadding="3" width="100%" border="0">
                                            <tr>
                                                <td bgcolor="#ffeeb2" align="left">
                                                    <asp:Panel runat="server" ID="UIpan_AddPart" DefaultButton="UI_cmdSearch">
                                                        <asp:Label ID="UI_lblModel" runat="server" Text="012_Model No" Visible="false" class="default"></asp:Label>
                                                        <%--.:--%>
                                                        <asp:TextBox ID="UI_txtModel" runat="server" Width="100" Visible="false" class="default"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                                                        <%--<font color="#ff0000">*</font>--%>
                                                        <asp:Label ID="UI_lblPartsNo" runat="server" Text="083_Part's No" class="default"></asp:Label>.:
                                                        <asp:TextBox ID="UI_txtPartsNo" runat="server" Width="100" MaxLength="13" class="default"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                                                        <asp:Label ID="UI_lblLocation" runat="server" Text="084_Location" Visible="false" class="default"></asp:Label>
                                                        <%--:--%>
                                                        <asp:TextBox ID="UI_txtLocation" runat="server" Width="80" Visible="false" class="default"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                                                        
                                                        <asp:Button ID="UI_cmdSearch" runat="server" CssClass="Confirm_l" Text="Search" CausesValidation="false" />&nbsp;&nbsp;&nbsp;
                                                        <asp:Button ID="UI_cmdParts_Search" runat="server" Text="078_Search" CssClass="Confirm" CausesValidation="false" OnClick="UI_cmdParts_Search_Click" Visible="false" />
                                                    </asp:Panel>
                                                </td>

                                                <td bgcolor="#ffeeb2">
                                                    <asp:Label ID="UI_lblCurrency" runat="server" Text="085_Currency" class="default"></asp:Label>:&nbsp;
                                                        <asp:Label ID="UI_lblCurrencyCode" runat="server" class="default"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>


                                        <!-- [Begin] Search Export -->
                                        <asp:DataList ID="UI_dvRepairDetail" runat="server" ExtractTemplateRows="true" BorderColor="#c0c0c0"
                                            CellSpacing="0" CellPadding="3" Width="100%" border="1" bordercolorlight="#c0c0c0">
                                            <HeaderTemplate>
                                                <asp:Table ID="oTableHeader" runat="server">
                                                    <asp:TableHeaderRow bgcolor="#fff4d0">
                                                        <asp:TableHeaderCell Width="2%">Item</asp:TableHeaderCell>

                                                        <asp:TableHeaderCell Width="3%">
                                                            <asp:Label ID="lblWaive" runat="server" Text="405_Waive"></asp:Label>
                                                        </asp:TableHeaderCell>
                                                        <asp:TableHeaderCell Width="3%">
                                                            <asp:Label ID="lblOption" runat="server" Text="406_Option"></asp:Label>
                                                        </asp:TableHeaderCell>


                                                        <asp:TableHeaderCell Width="10%">
                                                            <asp:Label ID="lblHPart" runat="server" Text="083_Part'sNo"></asp:Label>
                                                        </asp:TableHeaderCell>
                                                        <asp:TableHeaderCell Width="10%">
                                                            <asp:Label ID="lblHSerial" runat="server" Text="098_Serial No"></asp:Label>
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
                                                        <asp:TableHeaderCell Width="19%">
                                                            <asp:Label ID="lblHReason" runat="server" Text="102_Defective Reason"></asp:Label>
                                                        </asp:TableHeaderCell>
                                                        <asp:TableHeaderCell Width="5%">
                                                            <asp:Label ID="lblHQty" runat="server" Text="103_Qty"></asp:Label>
                                                        </asp:TableHeaderCell>
                                                        <asp:TableHeaderCell Width="6%">
                                                            <asp:Label ID="lblHPrice" runat="server" Text="104_Price"></asp:Label>
                                                        </asp:TableHeaderCell>
                                                        <asp:TableHeaderCell Width="3%">
                                                            <asp:Label ID="lblHDel" runat="server" Text="017_Delete"></asp:Label>
                                                        </asp:TableHeaderCell>
                                                    </asp:TableHeaderRow>
                                                </asp:Table>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Table ID="oTableRow" runat="server">
                                                    <asp:TableRow>
                                                        <asp:TableCell RowSpan="2" HorizontalAlign="Center">
                                                            <asp:Label ID="lbl_Seq" runat="server" Text='<%# Me.UI_dvRepairDetail.Items.Count+1 %>' class="default"></asp:Label>
                                                            <asp:Label ID="lblRMAREDID" runat="server" Text='<%# Eval("RMARED_ID") %>' Visible="false" class="default"></asp:Label>
                                                            <asp:Label ID="lblDEFECTIVE" runat="server" Text='<%# Eval("RMARED_DEFECTIVE") %>' Visible="false" class="default"></asp:Label>
                                                            <!-- Style="display: none; width: 1px" -->
                                                        </asp:TableCell>


                                                        <asp:TableCell HorizontalAlign="Center" RowSpan="2">
                                                            <asp:CheckBox runat="server" ID="chhWaive" Checked='<%# Eval("RMARED_WAIVE") %>' />
                                                            <asp:Label ID="UI_lblWaive" runat="server"></asp:Label>
                                                            <asp:Label ID="UI_RMARED_WAIVE" runat="server" Text='<%# Eval("RMARED_WAIVE") %>' class="default" Visible="false"></asp:Label>

                                                        </asp:TableCell>
                                                        <asp:TableCell HorizontalAlign="Center" RowSpan="2">
                                                            <asp:CheckBox runat="server" ID="chkOption" />
                                                            <asp:Label ID="UI_lblOption" runat="server"></asp:Label>
                                                            <asp:Label ID="UI_RMARED_OPTION" runat="server" Text='<%# Eval("RMARED_OPTION") %>' class="default" Visible="false"></asp:Label>
                                                        </asp:TableCell>


                                                        <asp:TableCell>
                                                            <asp:TextBox ID="txtNewPart" runat="server" Width="100" MaxLength="13" Text='<%# Eval("RMARED_NPARTNO") %>' class="default"></asp:TextBox>
                                                            <asp:Label ID="UI_RMARED_NPARTNO" runat="server" Text='<%# Eval("RMARED_NPARTNO") %>' Visible="false"></asp:Label>
                                                            <asp:RequiredFieldValidator ID="rfvNewPart" runat="server" ErrorMessage="107_請輸入New Part No" Display="None" TabIndex="0" ValidationGroup="RepairGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        </asp:TableCell>

                                                        <asp:TableCell HorizontalAlign="left">
                                                            <asp:Label ID="lblNew" runat="server" Text="_New" class="default"></asp:Label>
                                                            <asp:TextBox ID="txtNewSerial" runat="server" Text='<%# Eval("RMARED_NSERIALNO") %>' class="default"></asp:TextBox>
                                                            <asp:Label ID="UI_RMARED_NSERIALNO" runat="server" Text='<%# Eval("RMARED_NSERIALNO") %>' Visible="false"></asp:Label>
                                                            <%--                                                <asp:RequiredFieldValidator ID="rfvNewSerial" runat="server" ErrorMessage="108_請輸入New Serial No" Display="None" TabIndex="0" ValidationGroup="RepairGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                                                        </asp:TableCell>

                                                        <asp:TableCell RowSpan="2">
                                                            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Columns="20" Rows="3" Text='<%# Eval("RMARED_DESC") %>' class="default"></asp:TextBox>
                                                            <asp:Label ID="UI_RMARED_DESC" runat="server" Text='<%# Eval("RMARED_DESC") %>' Visible="false"></asp:Label>
                                                        </asp:TableCell>

                                                        <asp:TableCell RowSpan="2" HorizontalAlign="Center">
                                                            <asp:TextBox ID="txtLocation" runat="server" Width="80px" Text='<%# Eval("RMARED_LOCATION") %>' class="default"></asp:TextBox>
                                                            <asp:Label ID="UI_RMARED_LOCATION" runat="server" Text='<%# Eval("RMARED_LOCATION") %>' Visible="false"></asp:Label>
                                                        </asp:TableCell>

                                                        <asp:TableCell RowSpan="2" HorizontalAlign="Center">
                                                            <asp:DropDownList ID="cboImproper" runat="server" class="default">
                                                                <asp:ListItem Value="1" Text="065_Yes"></asp:ListItem>
                                                                <asp:ListItem Value="0" Text="066_No" Selected="True"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:Label ID="lblIMPROPERUSAGE" runat="server" Text='<%# Eval("RMARED_IMPROPERUSAGE") %>' class="default" Visible="false"></asp:Label>
                                                            <asp:Label ID="UI_RMARED_IMPROPERUSAGE" runat="server" Visible="false"></asp:Label>
                                                        </asp:TableCell>

                                                        <asp:TableCell RowSpan="2">
                                                            <asp:DropDownList ID="UI_cboDefective" runat="server" class="default"></asp:DropDownList>
                                                            <asp:Label ID="UI_Defective" runat="server" Visible="false"></asp:Label>
                                                        </asp:TableCell>

                                                        <asp:TableCell RowSpan="2" HorizontalAlign="Center">
                                                            <asp:Label ID="UI_RMARED_QTY" runat="server" Text='<%# Eval("RMARED_QTY") %>' Visible="false"></asp:Label>
                                                            <asp:TextBox ID="txtQty" runat="server" Width="20px" Text='<%# Eval("RMARED_QTY") %>' class="default"></asp:TextBox>

                                                            <asp:TextBox ID="UI_txtMaterialCost" runat="server" Text='<%# Eval("RMARED_MATERIALCOST") %>' class="default"></asp:TextBox>
                                                            <asp:TextBox ID="UI_txtRMARED_PRICE" runat="server" Text='<%# Eval("RMARED_PRICE") %>' class="default"></asp:TextBox>
                                                            <asp:TextBox ID="UI_RMARED_ISSOURCE" runat="server" Text='<%# Eval("RMARED_ISSOURCE") %>' class="default"></asp:TextBox>
                                                            <!-- Style="display: none; width: 1px" -->

                                                            <asp:RangeValidator ID="rvQty" runat="server" ErrorMessage="109_輸入Qty值型態有誤" Display="None" MinimumValue="0" MaximumValue="10000" Type="Integer" ValidationGroup="RepairGroup"></asp:RangeValidator>
                                                        </asp:TableCell>

                                                        <asp:TableCell RowSpan="2" HorizontalAlign="Center">
                                                            <asp:Label ID="UI_lblRMARED_PRICE" runat="server" class="default" Text='<%# Eval("RMARED_PRICE") %>'></asp:Label>
                                                        </asp:TableCell>

                                                        <asp:TableCell RowSpan="2" HorizontalAlign="Center">
                                                            <asp:ImageButton ID="imgDel" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/xx.gif" CommandName="cmdDel" OnClientClick="return FrmDelete()" />
                                                        </asp:TableCell>
                                                    </asp:TableRow>


                                                    <asp:TableRow>
                                                        <asp:TableCell Visible="false">
                                                        </asp:TableCell>

                                                        <asp:TableCell>
                                                            <asp:TextBox ID="txtOldPart" runat="server" Width="100" MaxLength="13" Text='<%# Eval("RMARED_OPARTNO") %>' class="default"></asp:TextBox>
                                                            <asp:Label ID="UI_RMARED_OPARTNO" runat="server" Text='<%# Eval("RMARED_OPARTNO") %>' class="default"></asp:Label>
                                                        </asp:TableCell>

                                                        <asp:TableCell HorizontalAlign="left">
                                                            <asp:Label ID="lblOld" runat="server" Text="_Old" class="default"></asp:Label>
                                                            <asp:TextBox ID="txtOldSerial" runat="server" Text='<%# Eval("RMARED_OSERIALNO") %>' class="default"></asp:TextBox>
                                                            <asp:Label ID="UI_RMARED_OSERIALNO" runat="server" Text='<%# Eval("RMARED_OSERIALNO") %>' class="default" Visible="false"></asp:Label>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </ItemTemplate>
                                        </asp:DataList>


                                        <!-- [End] Search Export -->
                                        <table class="default" bordercolor="#c0c0c0" cellspacing="0" cellpadding="3" width="100%"
                                            border="0" id="table8">
                                            <tr>
                                                <td bgcolor="#ffeeb2" align="left" width="30%">
                                                    <!-- Service Charge 金額 -->
                                                    <asp:Label ID="UI_lblManpower" runat="server" Text="086_Man power" class="default"></asp:Label>
                                                    <asp:Label ID="UI_lblManpower_Delimited" runat="server" Text=" :" class="default"></asp:Label>
                                                    <asp:TextBox ID="UI_txtManHour" runat="server" Text="0" Width="30px" class="default"></asp:TextBox>

                                                    <asp:Label ID="UI_lblHour" runat="server" Text="057_hour" class="default"></asp:Label>
                                                    <asp:Label ID="UI_lblHour_Delimited" runat="server" Text="&nbsp;=&nbsp;" class="default"></asp:Label>

                                                    <!-- 人工每小時單價, 已用不到了, 預設是 1 -->
                                                    <asp:TextBox ID="UI_txtLABORPrice" runat="server" class="default"></asp:TextBox>

                                                    <!-- 人工維修費用(Service Charge 金額 * 人工每小時單價) -->
                                                    <asp:Label ID="UI_lblLaborCost" runat="server" Text="0" class="default"></asp:Label>
                                                </td>

                                                <td bgcolor="#ffeeb2" align="left" width="30%">
                                                    <asp:Label ID="UI_lblParts" runat="server" Text="087_Parts" class="default"></asp:Label>
                                                    <asp:Label ID="uiLbl_Parts_Delimited" runat="server" Text=" :" class="default"></asp:Label>

                                                    <!-- 報價零件加總金額 -->
                                                    <asp:Label ID="UI_lblPartsTotal" runat="server" Text="0" class="default"></asp:Label>
                                                    <asp:TextBox ID="UI_txtPartsTotal" runat="server" class="default"></asp:TextBox>
                                                    <!-- Style="display: none; width: 1px" -->
                                                </td>

                                                <td bgcolor="#ffeeb2" align="right">
                                                    <asp:Label ID="UI_lblTotalText" runat="server" Text="088_Total Amount" class="default"></asp:Label>
                                                    <asp:Label ID="UI_lblTotalText_Delimited" runat="server" Text=" :" class="default"></asp:Label>
                                                </td>

                                                <td bgcolor="#ffeeb2" align="left" width="20%">
                                                    <asp:Label ID="UI_lblCurrencyCode1" runat="server" Font-Bold="true" class="default"></asp:Label>
                                                    <!-- 總金額 (Service Charge + 報價零件加總金額) -->
                                                    <asp:Label ID="UI_lblTotal" runat="server" Text="0" Font-Bold="true" class="default"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>


                                        <p align="center" style="margin-top: 10px; margin-bottom: 0">
                                            <asp:Button ID="UI_cmdCancel" runat="server" Text="_Cancel" CssClass="Problem_Edit" />
                                            <asp:Button ID="UI_cmdSubmit" runat="server" Text="_Submit" CssClass="Confirm" ValidationGroup="RepairGroup" OnClientClick="return FrmSave()" />
                                            <asp:Button ID="UI_cmdSendMail" runat="server" Text="_SendMail" CssClass="Confirm_l" />
											<asp:Button ID="UI_cmdSendMail1" runat="server" Text="_SendMailQuoting" CssClass="Confirm_l" />
                                        </p>
                                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                                            <tr height="20px">
                                                <td width="24" background="Images/pic_14.gif">&nbsp;</td>
                                                <td background="Images/pic_15.gif">
                                                    <table border="0" width="100%" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td width="1%">&nbsp;</td>
                                                            <td align="left" class="default">
                                                                <asp:Label ID="UI_lblProductTittle" runat="server" Text="010_Product Information"
                                                                    Font-Bold="true"></asp:Label>
                                                            </td>
                                                            <td align="right" class="default">
                                                                <asp:Label ID="UI_lblQuickSearch" runat="server" Text="069_Quick search"></asp:Label>:
                                                                        <asp:Label ID="UI_lblQcSn" runat="server" Text="013_Serial Number search"></asp:Label>:
                                                                        <asp:TextBox ID="UI_txtSN" runat="server" Width="140px" onkeydown="if(event.keyCode==13){SerialSearch();} "></asp:TextBox>
                                                            </td>
                                                            <td width="6%" align="left">
                                                                <asp:Button ID="btnQuickSearch" runat="server" Text="_Search" CssClass="Confirm_l" OnClick="btnQuickSearch_Click" />
                                                            </td>
                                                            <td width="2%">&nbsp;</td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:GridView ID="UI_dvRequest" runat="server" Width="100%" CellPadding="0" CellSpacing="1"
                                            border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true"
                                            PagerSettings-Mode="Numeric" AllowSorting="true">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <asp:Label runat="server" ID="UI_lblSeq" Text="Item"></asp:Label>
                                                        <asp:CheckBox ID="UI_CheckGroup" runat="server" AutoPostBack="true" OnCheckedChanged="UI_checkGroup_CheckedChanged" Visible="false" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="UI_Icount" runat="server" Text='<%# me.UI_dvRequest.Rows.Count +1 %>'></asp:Label>
                                                        <asp:CheckBox ID="UI_Check" runat="server" AutoPostBack="true" Visible="false" OnCheckedChanged="UI_Check_CheckedChanged" />

                                                        <asp:Label ID="UI_SeqID" runat="server"></asp:Label>
                                                        <asp:Label ID="UI_RMAID" runat="server" Text='<%# Eval("RMA_ID") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMADID" runat="server" Text='<%# Eval("RMAD_ID") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMANO" runat="server" Text='<%# Eval("RMAD_RMANO") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMADSTATUS" runat="server" Text='<%# Eval("RMAD_STATUS") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="RMAR_REPAIRAD" runat="server" Text='<%# Eval("RMAR_REPAIRAD") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_SERIALNO" runat="server" Text='<%# Eval("RMAD_SERIALNO") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_Status" runat="server" Text='<%# Eval("Status") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMAR_COMPNO" runat="server" Text='<%# Eval("RMAR_COMPNO") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMADCSTMP" runat="server" Text='<%# Eval("RMAD_CSTMP") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMADPARTSN" runat="server" Text='<%# Eval("RMAD_PARTSN") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="013_Serial Numbe">
                                                    <HeaderStyle Width="14%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="UI_RMAD_SERIALNO" runat="server" Text='<%# Eval("RMAD_SERIALNO") %>'
                                                            CommandName="cmdChangeSn" CommandArgument='<%# me.UI_dvRequest.Rows.Count%>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="RMAD_MODELNO" HeaderText="035_Model" HeaderStyle-Width="7%"
                                                    ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="Warranty" HeaderText="EW Warranty" HeaderStyle-Width="7%"
                                                    ItemStyle-HorizontalAlign="Center" HtmlEncode="False" DataFormatString="{0:yyyy/MM/dd}"></asp:BoundField>
                                                <asp:BoundField DataField="CWEndWarr" HeaderText="CW Warranty" HeaderStyle-Width="7%"
                                                    ItemStyle-HorizontalAlign="Center" HtmlEncode="False"></asp:BoundField>
                                                <asp:TemplateField HeaderText="SW Warranty">
                                                    <HeaderStyle Width="8%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="cmdSWDetail" runat="server" Text='<%# Eval("SWEndWarr") %>' CommandName="cmdSWDetail"
                                                            CommandArgument='<%# me.UI_dvRequest.Rows.Count%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Detail">
                                                    <HeaderStyle Width="5%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>

                                                        <asp:ImageButton ID="UI_cmdDetail" runat="server" ImageUrl="images/icon-detail.gif" CommandName="cmdDetail" Visible="false" CommandArgument='<%#Me.UI_dvRequest.Rows.Count%>' />
                                                        <asp:ImageButton ID="UI_cmdDetail_img" runat="server" ImageUrl="images/icon-detail.gif" CommandName="cmdDetail_img" CommandArgument='<%# me.UI_dvRequest.Rows.Count%>' />

                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Quoted" HeaderText="070_Quote" HeaderStyle-Width="10%"
                                                    ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="Status" HeaderText="032_Status" HeaderStyle-Width="10%"
                                                    ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="Assign" SortExpression="Assign" HeaderText="071_Re-Assign"
                                                    HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
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

                                        <center>
                                            <asp:Button ID="UI_cmdApply" runat="server" Text="_Apply" CssClass="Confirm" ValidationGroup="RepairGroup" OnClientClick="return FrmSave()" OnClick="UI_cmdApply_Click" />
                                        </center>

                                        <!-- 版本 START -->
                                        <table class="default" bordercolor="#c0c0c0" cellspacing="0" cellpadding="3" width="100%" border="0" id="table5">
                                            <tr>
                                                <td>
                                                    <table id="table3" class="default" bordercolor="#c0c0c0" cellspacing="0" cellpadding="3"
                                                        width="100%" border="1" bordercolorlight="#FFFFFF">
                                                        <tr>
                                                            <td colspan="4" align="left">
                                                                <asp:CheckBox ID="UI_CheckVer" runat="server" AutoPostBack="true" Visible="true" OnCheckedChanged="UI_CheckVer_CheckedChanged" /><asp:Label runat="server" ID="lblVerChange"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:Panel ID="pnlVersion" runat="server">
                                                        <table id="table2" class="default" bordercolor="#c0c0c0" cellspacing="0" cellpadding="3"
                                                            width="100%" border="1" bordercolorlight="#FFFFFF">
                                                            <tr>
                                                                <td style="width: 10%;" class="default">
                                                                    <asp:Label runat="server" ID="lblColumn"></asp:Label>
                                                                </td>
                                                                <td style="width: 15%;" class="default">
                                                                    <asp:Label runat="server" ID="lblVerName"></asp:Label>
                                                                </td>
                                                                <td style="width: 37%;" class="default">
                                                                    <asp:Label runat="server" ID="lblVerBefore"></asp:Label>
                                                                </td>
                                                                <td class="default">
                                                                    <asp:Label runat="server" ID="lblVerAfter"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="default" align="left">
                                                                    <asp:Label runat="server" ID="lblC01"></asp:Label></td>
                                                                <td class="default" align="left">
                                                                    <asp:Label ID="lblA01" runat="Server"></asp:Label></td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtA01" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    &nbsp;<asp:Label ID="lblOA01" runat="Server"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtQA01" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    <asp:Button ID="btnQA01" runat="Server" Text="查 詢" CssClass="button" OnClick="btnQA01_Click" />
                                                                    &nbsp;<asp:Label ID="lblNA01" runat="Server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="default" align="left">
                                                                    <asp:Label runat="server" ID="lblC02"></asp:Label></td>
                                                                <td class="default" align="left">
                                                                    <asp:Label ID="lblA02" runat="Server"></asp:Label></td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtA02" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    &nbsp;<asp:Label ID="lblOA02" runat="Server"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtQA02" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    <asp:Button ID="btnQA02" runat="Server" Text="查 詢" CssClass="button" OnClick="btnQA02_Click" />
                                                                    &nbsp;<asp:Label ID="lblNA02" runat="Server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="default" align="left">
                                                                    <asp:Label runat="server" ID="lblC03"></asp:Label></td>
                                                                <td class="default" align="left">
                                                                    <asp:Label ID="lblA03" runat="Server"></asp:Label></td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtA03" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    &nbsp;<asp:Label ID="lblOA03" runat="Server"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtQA03" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    <asp:Button ID="btnQA03" runat="Server" Text="查 詢" CssClass="button" OnClick="btnQA03_Click" />
                                                                    &nbsp;<asp:Label ID="lblNA03" runat="Server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="default" align="left">
                                                                    <asp:Label runat="server" ID="lblC04"></asp:Label></td>
                                                                <td class="default" align="left">
                                                                    <asp:Label ID="lblA04" runat="Server"></asp:Label></td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtA04" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    &nbsp;<asp:Label ID="lblOA04" runat="Server"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtQA04" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    <asp:Button ID="btnQA04" runat="Server" Text="查 詢" CssClass="button" OnClick="btnQA04_Click" />
                                                                    &nbsp;<asp:Label ID="lblNA04" runat="Server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="default" align="left">
                                                                    <asp:Label runat="server" ID="lblC05"></asp:Label></td>
                                                                <td class="default" align="left">
                                                                    <asp:Label ID="lblA05" runat="Server"></asp:Label></td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtA05" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    &nbsp;<asp:Label ID="lblOA05" runat="Server"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtQA05" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    <asp:Button ID="btnQA05" runat="Server" Text="查 詢" CssClass="button" OnClick="btnQA05_Click" />
                                                                    &nbsp;<asp:Label ID="lblNA05" runat="Server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="default" align="left">
                                                                    <asp:Label runat="server" ID="lblC06"></asp:Label></td>
                                                                <td class="default" align="left">
                                                                    <asp:Label ID="lblA06" runat="Server"></asp:Label></td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtA06" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    &nbsp;<asp:Label ID="lblOA06" runat="Server"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtQA06" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    <asp:Button ID="btnQA06" runat="Server" Text="查 詢" CssClass="button" OnClick="btnQA06_Click" />
                                                                    &nbsp;<asp:Label ID="lblNA06" runat="Server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="default" align="left">
                                                                    <asp:Label runat="server" ID="lblC07"></asp:Label></td>
                                                                <td class="default" align="left">
                                                                    <asp:Label ID="lblA07" runat="Server"></asp:Label></td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtA07" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    &nbsp;<asp:Label ID="lblOA07" runat="Server"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtQA07" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    <asp:Button ID="btnQA07" runat="Server" Text="查 詢" CssClass="button" OnClick="btnQA07_Click" />
                                                                    &nbsp;<asp:Label ID="lblNA07" runat="Server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="default" align="left">
                                                                    <asp:Label runat="server" ID="lblC08"></asp:Label></td>
                                                                <td class="default" align="left">
                                                                    <asp:Label ID="lblA08" runat="Server"></asp:Label></td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtA08" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    &nbsp;<asp:Label ID="lblOA08" runat="Server"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtQA08" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    <asp:Button ID="btnQA08" runat="Server" Text="查 詢" CssClass="button" OnClick="btnQA08_Click" />
                                                                    &nbsp;<asp:Label ID="lblNA08" runat="Server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="default" align="left">
                                                                    <asp:Label runat="server" ID="lblC09"></asp:Label></td>
                                                                <td class="default" align="left">
                                                                    <asp:Label ID="lblA09" runat="Server"></asp:Label></td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtA09" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    &nbsp;<asp:Label ID="lblOA09" runat="Server"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtQA09" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    <asp:Button ID="btnQA09" runat="Server" Text="查 詢" CssClass="button" OnClick="btnQA09_Click" />
                                                                    &nbsp;<asp:Label ID="lblNA09" runat="Server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="default" align="left">
                                                                    <asp:Label runat="server" ID="lblC10"></asp:Label></td>
                                                                <td class="default" align="left">
                                                                    <asp:Label ID="lblA10" runat="Server"></asp:Label></td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtA10" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    &nbsp;<asp:Label ID="lblOA10" runat="Server"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtQA10" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    <asp:Button ID="btnQA10" runat="Server" Text="查 詢" CssClass="button" OnClick="btnQA10_Click" />
                                                                    &nbsp;<asp:Label ID="lblNA10" runat="Server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="default" align="left">
                                                                    <asp:Label runat="server" ID="lblC11"></asp:Label></td>
                                                                <td class="default" align="left">
                                                                    <asp:Label ID="lblA11" runat="Server"></asp:Label></td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtA11" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    &nbsp;<asp:Label ID="lblOA11" runat="Server"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtQA11" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    <asp:Button ID="btnQA11" runat="Server" Text="查 詢" CssClass="button" OnClick="btnQA11_Click" />
                                                                    &nbsp;<asp:Label ID="lblNA11" runat="Server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="default" align="left">
                                                                    <asp:Label runat="server" ID="lblC12"></asp:Label></td>
                                                                <td class="default" align="left">
                                                                    <asp:Label ID="lblA12" runat="Server"></asp:Label></td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtA12" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    &nbsp;<asp:Label ID="lblOA12" runat="Server"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtQA12" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    <asp:Button ID="btnQA12" runat="Server" Text="查 詢" CssClass="button" OnClick="btnQA12_Click" />
                                                                    &nbsp;<asp:Label ID="lblNA12" runat="Server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="default02" align="left">
                                                                    <asp:Label runat="server" ID="lblC13"></asp:Label></td>
                                                                <td class="default02" align="left">
                                                                    <asp:Label ID="lblB01" runat="Server"></asp:Label></td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtB01" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    &nbsp;<asp:Label ID="lblOB01" runat="Server"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtQB01" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    <asp:Button ID="btnQB01" runat="Server" Text="查 詢" CssClass="button" OnClick="btnQB01_Click" />
                                                                    &nbsp;<asp:Label ID="lblNB01" runat="Server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="default02" align="left">
                                                                    <asp:Label runat="server" ID="lblC14"></asp:Label></td>
                                                                <td class="default02" align="left">
                                                                    <asp:Label ID="lblB02" runat="Server"></asp:Label></td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtB02" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    &nbsp;<asp:Label ID="lblOB02" runat="Server"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtQB02" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    <asp:Button ID="btnQB02" runat="Server" Text="查 詢" CssClass="button" OnClick="btnQB02_Click" />
                                                                    &nbsp;<asp:Label ID="lblNB02" runat="Server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="default02" align="left">
                                                                    <asp:Label runat="server" ID="lblC15"></asp:Label></td>
                                                                <td class="default02" align="left">
                                                                    <asp:Label ID="lblB03" runat="Server"></asp:Label></td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtB03" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    &nbsp;<asp:Label ID="lblOB03" runat="Server"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtQB03" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    <asp:Button ID="btnQB03" runat="Server" Text="查 詢" CssClass="button" OnClick="btnQB03_Click" />
                                                                    &nbsp;<asp:Label ID="lblNB03" runat="Server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="default02" align="left">
                                                                    <asp:Label runat="server" ID="lblC16"></asp:Label></td>
                                                                <td class="default02" align="left">
                                                                    <asp:Label ID="lblB04" runat="Server"></asp:Label></td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtB04" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    &nbsp;<asp:Label ID="lblOB04" runat="Server"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtQB04" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    <asp:Button ID="btnQB04" runat="Server" Text="查 詢" CssClass="button" OnClick="btnQB04_Click" />
                                                                    &nbsp;<asp:Label ID="lblNB04" runat="Server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="default02" align="left">
                                                                    <asp:Label runat="server" ID="lblC17"></asp:Label></td>
                                                                <td class="default02" align="left">
                                                                    <asp:Label ID="lblB05" runat="Server"></asp:Label></td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtB05" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    &nbsp;<asp:Label ID="lblOB05" runat="Server"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtQB05" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    <asp:Button ID="btnQB05" runat="Server" Text="查 詢" CssClass="button" OnClick="btnQB05_Click" />
                                                                    &nbsp;<asp:Label ID="lblNB05" runat="Server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="default02" align="left">
                                                                    <asp:Label runat="server" ID="lblC18"></asp:Label></td>
                                                                <td class="default02" align="left">
                                                                    <asp:Label ID="lblB06" runat="Server"></asp:Label></td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtB06" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    &nbsp;<asp:Label ID="lblOB06" runat="Server"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtQB06" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                                                    <asp:Button ID="btnQB06" runat="Server" Text="查 詢" CssClass="button" OnClick="btnQB06_Click" />
                                                                    &nbsp;<asp:Label ID="lblNB06" runat="Server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4" align="center">
                                                                    <asp:Button ID="UI_cmdVerApply" runat="server" Text="_Apply" CssClass="Confirm" OnClick="UI_cmdVerApply_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                        <!-- 版本 END -->
                                        <div align="left">
                                            <asp:Label ID="UI_lblStatusUpdate" runat="server" Text="089_Status Update" Font-Bold="true"
                                                class="default"></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                                <!-- 簽核 START -->
                                <tr>
                                    <td>
                                        <table id="table1" class="default" bordercolor="#c0c0c0" cellspacing="0" cellpadding="3"
                                            width="100%" border="1" bordercolorlight="#FFFFFF">
                                            <tr bgcolor="#fff4d0">
                                                <td align="center" width="10%">&nbsp;</td>
                                                <td align="center" width="15%">
                                                    <asp:Label ID="UI_lblReceived" runat="server" Text="090_Received" class="default"></asp:Label></td>
                                                <td align="center" width="15%">
                                                    <asp:Label ID="UI_lblRepair" runat="server" Text="091_Repair Quoted" class="default"></asp:Label></td>
                                                <td align="center" width="15%">
                                                    <asp:Label ID="UI_lblSales" runat="server" Text="092_Sales Confirmed" class="default"></asp:Label></td>
                                                <td align="center" width="15%">
                                                    <asp:Label ID="UI_lblClient" runat="server" Text="093_Client Confirmed" class="default"></asp:Label></td>
                                                <td align="center" width="15%">
                                                    <asp:Label ID="UI_lblRepaired" runat="server" Text="094_Repaired" class="default"></asp:Label></td>
                                                <td align="center" width="15%">
                                                    <asp:Label ID="UI_lblClose" runat="server" Text="095_Close" Font-Bold="true" ForeColor="red"
                                                        class="default"></asp:Label></td>
                                                <td align="center" width="15%">
                                                    <asp:Label ID="UI_lblCancel" runat="server" Text="041_Cancel" class="default"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td align="center" bgcolor="#fff4d0">
                                                    <asp:Label ID="UI_lblApprover" runat="server" Text="096_Approver" class="default"></asp:Label></td>
                                                <td align="center">
                                                    <asp:Label ID="UI_lblReceivedUser" runat="server" class="default"></asp:Label>&nbsp;</td>
                                                <td align="center">
                                                    <asp:Label ID="UI_lblRepairQuotedUser" runat="server" class="default"></asp:Label>&nbsp;</td>
                                                <td align="center">
                                                    <asp:Label ID="UI_lblSalesUser" runat="server" class="default"></asp:Label>&nbsp;</td>
                                                <td align="center">
                                                    <asp:Label ID="UI_lblClientUser" runat="server" class="default"></asp:Label>&nbsp;</td>
                                                <td align="center">
                                                    <asp:Label ID="UI_lblRepairedUser" runat="server" class="default"></asp:Label>&nbsp;</td>
                                                <td align="center">
                                                    <asp:Label ID="UI_lblCloseUser" runat="server" class="default"></asp:Label>&nbsp;</td>
                                                <td align="center">
                                                    <asp:Label ID="UI_lblCancelUser" runat="server" class="default"></asp:Label>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="center" bgcolor="#fff4d0">
                                                    <asp:Label ID="UI_lblDate" runat="server" Text="097_Date" class="default"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="UI_lblReceivedDate" runat="server" class="default"></asp:Label>&nbsp;
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="UI_lblRepairQuotedDate" runat="server" class="default"></asp:Label>&nbsp;
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="UI_lblSalesDate" runat="server" class="default"></asp:Label>&nbsp;
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="UI_lblClientDate" runat="server" class="default"></asp:Label>&nbsp;
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="UI_lblRepairedDate" runat="server" class="default"></asp:Label>&nbsp;
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="UI_lblCloseDate" runat="server" class="default"></asp:Label>&nbsp;
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="UI_lblCancelDate" runat="server" class="default"></asp:Label>&nbsp;</td>
                                            </tr>
                                        </table>
                                        <p>
                                    </td>
                                </tr>
                                <!-- 簽核 END -->

                            </table>
                        </fieldset>
                    </div>
                </td>
            </tr>
            </table>
            <asp:Label ID="UI_lblPreviousPage_RMADID" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblPreviousPage_RMANO" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblCurrencyRate" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblCompNO" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblEssCompNO" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblEssCurrencyCode" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblEssCurrencyRate" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblRMAID" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblRMACUNO" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblRMAACCOUNTID" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_flowCase" runat="server" Visible="false"></asp:Label>
            <uc1:ucClientDetail ID="ucClientDetail" runat="server" />
            <uc2:ucMessage ID="ucMessage" runat="server" />

            <uc5:ucRepairRarts ID="ucRepairRarts" runat="server" />
            <uc7:ucClientDetailPur ID="ucClientDetailPur" runat="server" />
            <uc8:ucSpecialSetting ID="ucSpecialSetting" runat="server" />

            <asp:LinkButton ID="lnkPopSelectPart" runat="server"></asp:LinkButton>
            <asp:ModalPopupExtender ID="mdlPupSelectPart" runat="server" TargetControlID="lnkPopSelectPart"
                PopupControlID="pnPopSelectPart" BackgroundCssClass="modalBackground" CancelControlID="btnPopPartClose"
                DropShadow="true" Drag="true" />
            <asp:Panel ID="pnPopSelectPart" runat="server" Style="display: none">
                <asp:Panel ID="Panel2" runat="server" CssClass="modalPopup" Width="600px">
                    <div class="form_div_Body" style="width: 100%; text-align: center; height: 500px; overflow-y: scroll; vertical-align: middle;">
                        <center>
                            <table class="default" border="1" style="background-color: White;">
                                <asp:DataList ID="lstPartNoSelect" runat="server" Width="100%" HorizontalAlign="center" CellPadding="0" CellSpacing="0" BorderWidth="0" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                    <HeaderTemplate>
                                        <tr>
                                            <td style="width: 25%;" class="list_head">Choose</td>
                                            <td style="width: 25%;" class="list_head">PartNo</td>
                                            <td class="list_head">Spec</td>
                                        </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="chkPopPartNoSelect" runat="server" GroupName="G1" AutoPostBack="True" OnCheckedChanged="CheckedchkPopPartNoSelectChanged"></asp:RadioButton>
                                                <input id="lblPartNo" type="hidden" value='<%# DataBinder.Eval(Container.DataItem, "VER_ITEM") %>' runat="server" name="lblPartNo" />
                                                <input id="lblPartName" type="hidden" value='<%# DataBinder.Eval(Container.DataItem, "ima021") %>' runat="server" name="lblPartName" />
                                            </td>
                                            <td><%# DataBinder.Eval(Container.DataItem, "VER_ITEM")%></td>
                                            <td><%# DataBinder.Eval(Container.DataItem, "ima021")%></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:DataList>
                            </table>
                        </center>
                    </div>
                    <br />
                    <p style="text-align: center;">
                        <asp:Button ID="btnPopPartClose" CssClass="button" runat="server" Width="80px" Text="Close" OnClick="btnPopSelectPartClose_Click" />
                    </p>
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="UI_cmdSearch" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="UI_cmdSubmit" EventName="Click" />
            <asp:PostBackTrigger ControlID="ucMessage" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="vsRepair" runat="server" ShowMessageBox="true" ShowSummary="false"
        ValidationGroup="RepairGroup" />

    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />

</asp:Content>
