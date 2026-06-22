<%@ Page Language="VB" AutoEventWireup="false" CodeFile="BossShipment_Notice.aspx.vb" Inherits="BossShipment_Notice" %>

<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc3" %>
<%@ Register Src="ascx/ucRepairDetail.ascx" TagName="ucRepairDetail" TagPrefix="uc5" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=Session("_Title")%></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <link href="script/rma.css" type="text/css" rel="stylesheet">
    <link href="script/styleDate.css" type="text/css" rel="stylesheet">
    <script src="Script/mouseover.js"></script>
    <script type="text/javascript" src="script/jsUpdateProgress.js"></script>
</head>
<body>
    <form id="oForm" runat="server">
        <asp:ScriptManager ID="ScriptManager" runat="server" EnablePageMethods="true" EnableScriptGlobalization="true"></asp:ScriptManager>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
            <ContentTemplate>

                <table runat="server" id="UI_tbShipmentNotice" border="0" cellspacing="0" cellpadding="0" width="100%" height="100%" bgcolor="#EFE9D9">
                    <tr height="10%">
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
			                                <asp:Label ID="UI_lblCustomerName" runat="server" Visible="true"></asp:Label>
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
			                                <asp:Label ID="UI_lblShippingOrdersText" runat="server"></asp:Label>&nbsp;&nbsp;
			                                <asp:Label ID="UI_lblShippingNumber" runat="server" Text="150_Number"></asp:Label>:
			                                <asp:Label ID="UI_lblShippingNumberText" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="UI_lblMemo" runat="server" Text="151_Memo"></asp:Label>
                                                </td>
                                                <td align="left" colspan="3">:
			                                <asp:Label ID="UI_lblMemoText" runat="server"></asp:Label>
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
                    <tr height="28">
                        <td width="24" background="Images/pic_14.gif">&nbsp;</td>
                        <td background="Images/pic_15.gif">
                            <table border="0" width="100%" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td width="1%">&nbsp;</td>
                                    <td width="99%" align="left" class="default">
                                        <asp:Label ID="UI_lblShippingTittle" runat="server" Text="152_Add Shipment Notice item." Font-Bold="true"></asp:Label>
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
                                                                    <asp:Label ID="lblHSerial" runat="server" Text="013_Serial Number"></asp:Label>
                                                                </asp:TableHeaderCell>
                                                                <asp:TableHeaderCell Width="19%">
                                                                    <asp:Label ID="lblHModel" runat="server" Text="020_Model Number"></asp:Label>
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
                                                                    <asp:Label ID="lblSerialNo" runat="server" Text='<%# Eval("RMASMD_SERIALNO") %>'></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell RowSpan="2">
                                                                    <asp:Label ID="lblModelNo" runat="server" Text='<%# Eval("RMASMD_MODELNO") %>'></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell Width="6%">
                                                                    <asp:Label ID="lblLabor" runat="server" Text="094_Repaired"></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell>
                                                                    <asp:Label ID="lblRepairLabor" runat="server" Text='<%# Eval("RMARSD_oldLABORCOST") %>'></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell>
                                                                    <asp:Label ID="lblRepairMaterial" runat="server" Text='<%# Eval("RMARSD_oldMATERIALCOST") %>'></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell>
                                                                    <asp:Label ID="lblRepairQuoted" runat="server" Text='<%# Eval("RMARSD_oldQUOTE") %>'></asp:Label>
                                                                </asp:TableCell>

                                                                <asp:TableCell RowSpan="2" HorizontalAlign="Center">
                                                                    <asp:ImageButton ID="imgDetail" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/icon-detail.gif" CommandName="cmdDetail" />
                                                                </asp:TableCell>

                                                            </asp:TableRow>
                                                            <asp:TableRow CssClass="default">
                                                                <asp:TableCell>
                                                                    <asp:Label ID="lblQuoted" runat="server" Text="179_Quoted"></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell>
                                                                    <asp:Label ID="lblSaleLabor" runat="server" Text='<%# Eval("RMARSD_LABORCOST") %>'></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell>
                                                                    <asp:Label ID="lblSaleMaterial" runat="server" Text='<%# Eval("RMARSD_MATERIALCOST") %>'></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell>
                                                                    <asp:Label ID="lblSaleQuoted" runat="server" Text='<%# Eval("RMARSD_QUOTE") %>'></asp:Label>
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
                                        <asp:Button ID="UI_cmdSubmit" runat="server" CssClass="Confirm_l" Text="001_Submit" />
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

                <uc3:ucMessage ID="ucMessage" runat="server" />
                <uc5:ucRepairDetail ID="ucRepairDetail" runat="server" />

            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="UI_cmdSubmit" EventName="Click" />
                <asp:PostBackTrigger ControlID="ucMessage" />
            </Triggers>
        </asp:UpdatePanel>

    </form>
</body>
</html>
