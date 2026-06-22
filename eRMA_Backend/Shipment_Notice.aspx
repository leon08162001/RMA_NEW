<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Shipment_Notice.aspx.vb" Inherits="Shipment_Notice" Title="Untitled Page" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register Src="ascx/ucProgressStatus.ascx" TagName="ucProgressStatus" TagPrefix="uc1" %>
<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>
<%@ Register Src="ascx/ucCustomer_pick.ascx" TagName="ucCustomer_pick" TagPrefix="uc3" %>
<%@ Register Src="ascx/ucRMASerial_Pick.ascx" TagName="ucRMASerial_Pick" TagPrefix="uc4" %>
<%@ Register Src="ascx/ucRepairDetail.ascx" TagName="ucRepairDetail" TagPrefix="uc5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <script type="text/javascript" language="javascript">
        function FrmDelete() {
            if (confirm(delMsg)) {
                onProgress('Delete')
                return true;
            }
            return false;
        }

        function calTotalAMT() {
            var blnFlag_Cal = false;

            var oGridView = document.getElementById('<%=UI_dvShipping.ClientID %>');
            var oGridViewID = oGridView.id;
            var iRows = oGridView.rows.length;

            var iTotal_LaborAmt = 0;
            var iTotal_MaterialAmt = 0;
            var iTotal_Quote = 0;

            for (var i = 0; i < iRows; i++) {
                var j = i + 1;

                if (j < 100) {
                    j = "0000000" + j
                    if (iRows.toString().length == 1) {
                        j = Right(j, iRows.toString().length + 1);
                    } else {
                        //                j = Right(j,iRows.toString().length);
                        j = Right(j, 2);
                    }
                }


                var txtSaleLabor = document.getElementById(oGridViewID + '_ctl' + j + '_txtSaleLabor');
                var txtSaleMaterial = document.getElementById(oGridViewID + '_ctl' + j + '_txtSaleMaterial');
                var txtSaleQuoted = document.getElementById(oGridViewID + '_ctl' + j + '_txtSaleQuoted');

                if (txtSaleLabor != null && txtSaleMaterial != null && txtSaleQuoted != null) {

                    var iLaborAmt = 0;
                    var iMaterialAmt = 0;
                    var iQuote = 0;

                    if (txtSaleLabor.value != "") {
                        if (isNaN(txtSaleLabor.value) != true) {
                            iLaborAmt = parseFloat(txtSaleLabor.value);
                            iTotal_LaborAmt = iTotal_LaborAmt + iLaborAmt;
                            blnFlag_Cal = true;
                        }
                    }

                    if (txtSaleMaterial.value != "") {
                        if (isNaN(txtSaleMaterial.value) != true) {
                            iMaterialAmt = parseFloat(txtSaleMaterial.value);
                            iTotal_MaterialAmt = iTotal_MaterialAmt + iMaterialAmt;
                            blnFlag_Cal = true;
                        }
                    }

                    iQuote = iLaborAmt + iMaterialAmt;
                    iTotal_Quote = iTotal_Quote + iQuote;
                    iQuote = Round(iQuote, 2);
                    txtSaleQuoted.value = iQuote;
                }

                var lblFLaborTotal = document.getElementById(oGridViewID + '_ctl' + j + '_lblFLaborTotal');
                var lblFMaterialTotal = document.getElementById(oGridViewID + '_ctl' + j + '_lblFMaterialTotal');
                var lblFQuotedTotal = document.getElementById(oGridViewID + '_ctl' + j + '_lblFQuotedTotal');
                if (lblFLaborTotal != null && lblFMaterialTotal != null && lblFQuotedTotal != null && blnFlag_Cal == true) {
                    iTotal_LaborAmt = Round(iTotal_LaborAmt, 2);
                    iTotal_MaterialAmt = Round(iTotal_MaterialAmt, 2);
                    iTotal_Quote = Round(iTotal_Quote, 2);

                    lblFLaborTotal.innerText = iTotal_LaborAmt;
                    lblFMaterialTotal.innerText = iTotal_MaterialAmt;
                    lblFQuotedTotal.innerText = iTotal_Quote;
                }

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
            <table border="0" cellspacing="0" cellpadding="0" width="100%" height="410px">
                <tr height="90px">
                    <td width="24" background="Images/pic_12.gif">&nbsp;</td>
                    <td valign="top" align="left">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <!--[Begin]Tittle-->
                            <tr>
                                <td width="3%">&nbsp;</td>
                                <td width="94%" align="left">
                                    <asp:Label ID="UI_lblTittle" runat="server" Text="147_Shipment Notice" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
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
                                                <asp:Label ID="UI_lblShipmentInformation" runat="server" Text="148_Please select customer before pick any part."></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="15%">
                                                <asp:Label ID="UI_lblNotice" runat="server" Text="144_Notice Number"></asp:Label>
                                            </td>
                                            <td width="55%">:
			                            <asp:Label ID="UI_lblNoticeText" runat="server"></asp:Label>
                                            </td>
                                            <td width="15%" align="right">
                                                <asp:Label ID="UI_lblDate" runat="server" Text="097_Date"></asp:Label>
                                            </td>
                                            <td width="15%" align="left">:
			                            <asp:Label ID="UI_lblDateText" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="UI_lblCustomer" runat="server" Text="030_Customer"></asp:Label>
                                            </td>
                                            <td colspan="3">:
			                            <asp:TextBox ID="UI_txtCustomer" runat="server" Width="300" Enabled="false"></asp:TextBox>&nbsp;
			                            <asp:Button ID="UI_cmdCustomerSearch" runat="server" CssClass="Pick" Text="004_Search" />
                                                <asp:Label ID="UI_lblCustomerID" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="UI_lblCurrencyCode" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="UI_lblCurrencyRate" runat="server" Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="UI_lblShippingOrders" runat="server" Text="149_Ship with Orders"></asp:Label>
                                            </td>
                                            <td align="left" colspan="3">:
			                            <asp:RadioButtonList ID="UI_opgShippingOrders" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem Text="066_No" Value="0" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="065_Yes" Value="1"></asp:ListItem>
                                        </asp:RadioButtonList>,
			                            <asp:Label ID="UI_lblShippingNumber" runat="server" Text="150_Number"></asp:Label>:
			                            <asp:TextBox ID="UI_txtShippingNumber" runat="server" Width="100"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="UI_lblMemo" runat="server" Text="151_Memo"></asp:Label>
                                            </td>
                                            <td align="left" colspan="3">:
			                            <asp:TextBox ID="UI_txtMemo" runat="server" TextMode="MultiLine" Rows="3" Columns="60"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
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
                <tr height="28px">
                    <td width="24" background="Images/pic_14.gif">&nbsp;</td>
                    <td background="Images/pic_15.gif">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="1%">&nbsp;</td>
                                <td width="99%" align="left" class="default">
                                    <asp:Label ID="UI_lblShippingTittle" runat="server" Text="152_Add Shipment Notice item." Font-Bold="true"></asp:Label>
                                    <asp:Button ID="UI_cmdShippingSearch" runat="server" CssClass="Pick" Text="004_Search" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr height="250px" valign="top">
                    <td background="Images/pic_20.gif">&nbsp;</td>
                    <td valign="top" bgcolor="#E3D8BE" align="center">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="1%">&nbsp;</td>
                                <td width="99%" align="left" class="default">
                                    <!--[Begin]資料列表表單-->
                                    <div class="form_div" align="center">
                                        <fieldset>
                                            <asp:DataList ID="UI_dvShipping" runat="server" FooterStyle-CssClass="default" ShowFooter="false" ExtractTemplateRows="true" CellSpacing="0" CellPadding="3" Width="100%" border="1" bordercolorlight="#c0c0c0">
                                                <HeaderTemplate>
                                                    <asp:Table ID="oTableHeader" runat="server">
                                                        <asp:TableHeaderRow CssClass="default">
                                                            <asp:TableHeaderCell Width="3%">
                                                                <asp:Label ID="lblHNo" runat="server" Text="066_No"></asp:Label>
                                                            </asp:TableHeaderCell>
                                                            <asp:TableHeaderCell Width="12%">
                                                                <asp:Label ID="lblHRMA" runat="server" Text="046_RMA Number"></asp:Label>
                                                            </asp:TableHeaderCell>

                                                            <asp:TableHeaderCell Width="14%">
                                                                <asp:Label ID="lblHModel" runat="server" Text="020_Model Number"></asp:Label>
                                                            </asp:TableHeaderCell>

                                                            <asp:TableHeaderCell Width="14%">
                                                                <asp:Label ID="lblHSerial" runat="server" Text="013_Serial Number"></asp:Label>
                                                            </asp:TableHeaderCell>

                                                            <asp:TableHeaderCell Width="19%" ColumnSpan="2">
                                                                <asp:Label ID="lblHLabor" runat="server" Text="178_Labor"></asp:Label>
                                                            </asp:TableHeaderCell>
                                                            <asp:TableHeaderCell Width="15%">
                                                                <asp:Label ID="lblHMaterial" runat="server" Text="059_Material"></asp:Label>
                                                            </asp:TableHeaderCell>
                                                            <asp:TableHeaderCell Width="18%">
                                                                <asp:Label ID="lblHAmount" runat="server" Text="180_subTotal"></asp:Label>
                                                            </asp:TableHeaderCell>
                                                            <asp:TableHeaderCell Width="5%">
                                                                <asp:Label ID="lblHDelete" runat="server" Text="017_Delete"></asp:Label>
                                                            </asp:TableHeaderCell>
                                                            <asp:TableHeaderCell Width="5%">
                                                                <asp:Label ID="lblHDetail" runat="server" Text="038_Detail"></asp:Label>
                                                            </asp:TableHeaderCell>
                                                        </asp:TableHeaderRow>
                                                    </asp:Table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Table ID="oTableRow2" runat="server">
                                                        <asp:TableRow CssClass="default">
                                                            <asp:TableCell RowSpan="2">
                                                                <asp:Label ID="lblNO" runat="server" Text='<%# me.UI_dvShipping.Items.Count + 1%>'></asp:Label>
                                                                <asp:Label ID="lblRMASMDRMASMID" runat="server" Text='<%# Eval("RMASMD_RMASMID") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblRMASMDID" runat="server" Text='<%# Eval("RMASMD_ID") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblRMADID" runat="server" Text='<%# Eval("RMASMD_RMADID") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblMark" runat="server" Text='<%# Eval("RMASMD_oldMark") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblCurrncyCode" runat="server" Text='<%# Eval("RMARSD_CURRENCYCODE") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblCurrncyRate" runat="server" Text='<%# Eval("RMARSD_CURRENCYRATE") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblLOWESTDISCOUNT" runat="server" Text='<%# Eval("RMASMD_LOWESTDISCOUNT") %>' Visible="false"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell RowSpan="2">
                                                                <asp:Label ID="lblRMANO" runat="server" Text='<%# Eval("RMASMD_RMANO") %>'></asp:Label>
                                                            </asp:TableCell>

                                                            <asp:TableCell RowSpan="2">
                                                                <asp:Label ID="lblModelNo" runat="server" Text='<%# Eval("RMASMD_MODELNO") %>'></asp:Label>
                                                            </asp:TableCell>

                                                            <asp:TableCell RowSpan="2">
                                                                <asp:Label ID="lblSerialNo" runat="server" Text='<%# Eval("RMASMD_SERIALNO") %>'></asp:Label>
                                                            </asp:TableCell>

                                                            <asp:TableCell>
                                                                <asp:Label ID="lblLabor" runat="server" Text="094_Repaired"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell>
                                                                <asp:TextBox ID="txtRepairLabor" runat="server" Width="80px" Text='<%# Eval("RMARSD_oldLABORCOST") %>' Enabled="false"></asp:TextBox>
                                                            </asp:TableCell>
                                                            <asp:TableCell>
                                                                <asp:TextBox ID="txtRepairMaterial" runat="server" Width="80px" Text='<%# Eval("RMARSD_oldMATERIALCOST") %>' Enabled="false"></asp:TextBox>
                                                            </asp:TableCell>
                                                            <asp:TableCell>
                                                                <asp:TextBox ID="txtRepairQuoted" runat="server" Width="80px" Text='<%# Eval("RMARSD_oldQUOTE") %>' Enabled="false"></asp:TextBox>
                                                            </asp:TableCell>

                                                            <asp:TableCell RowSpan="2">
                                                                <asp:ImageButton ID="imgDel" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/xx.gif" CommandName="cmdDel" OnClientClick="return FrmDelete()" />
                                                            </asp:TableCell>

                                                            <asp:TableCell RowSpan="2">
                                                                <asp:ImageButton ID="imgDetail" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/icon-detail.gif" CommandName="cmdDetail" />
                                                            </asp:TableCell>
                                                        </asp:TableRow>

                                                        <asp:TableRow CssClass="default">
                                                            <asp:TableCell>
                                                                <asp:Label ID="lblQuoted" runat="server" Text="179_Quoted"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell>
                                                                <asp:TextBox ID="txtSaleLabor" runat="server" Width="80px" Text='<%# Eval("RMARSD_LABORCOST") %>'></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="rfv_SaleLaborCost" runat="server" ErrorMessage="074_請輸入工時" Display="None" ValidationGroup="vsShippingGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                <asp:RangeValidator ID="rv_SaleLaborCost" runat="server" ErrorMessage="076_輸入工時型態有誤" Display="None" MinimumValue="0" MaximumValue="10000" Type="Double" ValidationGroup="vsShippingGroup"></asp:RangeValidator>
                                                            </asp:TableCell>
                                                            <asp:TableCell>
                                                                <asp:TextBox ID="txtSaleMaterial" runat="server" Width="80px" Text='<%# Eval("RMARSD_MATERIALCOST") %>'></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="rfv_SaleMaterialCost" runat="server" ErrorMessage="075_請輸入零件費用" Display="None" ValidationGroup="vsShippingGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                <asp:RangeValidator ID="rv_SaleMaterialCost" runat="server" ErrorMessage="077_輸入零件費用型態有誤" Display="None" MinimumValue="0" MaximumValue="10000" Type="Double" ValidationGroup="vsShippingGroup"></asp:RangeValidator>
                                                            </asp:TableCell>
                                                            <asp:TableCell>
                                                                <asp:TextBox ID="txtSaleQuoted" runat="server" Width="80px" Text='<%# Eval("RMARSD_QUOTE") %>' Enabled="false"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="rfv_SaleAmount" runat="server" ErrorMessage="135_請輸入費用加總" Display="None" ValidationGroup="vsShippingGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                <asp:RangeValidator ID="rv_SaleAmount" runat="server" ErrorMessage="136_輸入費用加總型態有誤" Display="None" MinimumValue="0" MaximumValue="10000" Type="Double" ValidationGroup="vsShippingGroup"></asp:RangeValidator>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                    </asp:Table>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Table ID="oTableFooter" runat="server">
                                                        <asp:TableHeaderRow>
                                                            <asp:TableHeaderCell>&nbsp;</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell>&nbsp;</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell>&nbsp;</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell>&nbsp;</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell ColumnSpan="2" HorizontalAlign="Left">
                                                                <asp:Label ID="lblFLabor" runat="server" Text="178_Labor"></asp:Label>:
                                                    <asp:Label ID="lblFLaborTotal" runat="server"></asp:Label>
                                                            </asp:TableHeaderCell>
                                                            <asp:TableHeaderCell HorizontalAlign="Left">
                                                                <asp:Label ID="lblFMaterial" runat="server" Text="059_Material"></asp:Label>:
                                                    <asp:Label ID="lblFMaterialTotal" runat="server"></asp:Label>
                                                            </asp:TableHeaderCell>
                                                            <asp:TableHeaderCell HorizontalAlign="Left">
                                                                <asp:Label ID="lblFAmount" runat="server" Text="088_Total Amount"></asp:Label>:<br>
                                                                &nbsp;&nbsp;
                                                    <asp:Label ID="lblFCurrnecyCode" runat="server"></asp:Label>&nbsp;
                                                    <asp:Label ID="lblFQuotedTotal" runat="server"></asp:Label>
                                                            </asp:TableHeaderCell>
                                                            <asp:TableHeaderCell></asp:TableHeaderCell>
                                                        </asp:TableHeaderRow>
                                                    </asp:Table>
                                                </FooterTemplate>
                                            </asp:DataList>
                                        </fieldset>
                                    </div>
                                    <!--[End]資料列表表單-->
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <br />
                                    <asp:Button ID="UI_cmdSave" runat="server" CssClass="Confirm_l" Text="002_Save" Enabled="false" ValidationGroup="vsShippingGroup" OnClientClick="onProgress('Save')" />
                                    <input id="UI_cmdBack" runat="server" type="button" value="006_back" class="Problem_Edit" onclick="javascript:history.back();" />
                                    <asp:Button ID="UI_cmdSubmit" runat="server" CssClass="Confirm_l" Text="001_Submit" Enabled="false" ValidationGroup="vsShippingGroup" OnClientClick="onProgress('Save')" />
                                    <asp:Button ID="UI_cmdPrint" runat="server" CssClass="Confirm_l" Text="044_Print" Enabled="false" ValidationGroup="vsShippingGroup" OnClientClick="onProgress('Process')" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <asp:Label ID="UI_lblPreviousPage_RMASMID" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblLaborTotal" runat="server" Text="0" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblMaterialTotal" runat="server" Text="0" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblQuotedTotal" runat="server" Text="0" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblIsSumbit" runat="server" Text="0" Visible="false"></asp:Label>

            <uc2:ucMessage ID="ucMessage" runat="server" />
            <uc3:ucCustomer_pick ID="ucCustomer_pick" runat="server" />
            <uc4:ucRMASerial_Pick ID="ucRMASerial_Pick" runat="server" />
            <uc5:ucRepairDetail ID="ucRepairDetail" runat="server" />
            <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="UI_cmdCustomerSearch" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="UI_cmdShippingSearch" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="UI_cmdSave" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="UI_cmdSubmit" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="UI_cmdPrint" EventName="Click" />
            <asp:PostBackTrigger ControlID="ucMessage" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:RequiredFieldValidator ID="rfvtxtCustomer" runat="server" ErrorMessage="181_請選取客戶" Display="None" ControlToValidate="UI_txtCustomer" ValidationGroup="vsShippingGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>

    <asp:ValidationSummary ID="vsShipping" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="vsShippingGroup" />


</asp:Content>

