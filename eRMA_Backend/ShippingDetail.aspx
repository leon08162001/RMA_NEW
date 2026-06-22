<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="ShippingDetail.aspx.vb" Inherits="ShippingDetail" Title="Untitled Page" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <table border="0" cellspacing="0" cellpadding="0" width="100%" height="100%">
        <tr height="10%">
            <td width="24" background="Images/pic_12.gif">&nbsp;</td>
            <td valign="top" align="left">
                <table border="0" width="100%" cellspacing="0" cellpadding="0">
                    <!--[Begin]Tittle-->
                    <tr>
                        <td width="3%">&nbsp;</td>
                        <td width="94%" align="left">&nbsp;&nbsp;
	                        <asp:Label ID="UI_lblTittle" runat="server" Text="154_Shipping" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
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
                                    <td colspan="4">&nbsp;&nbsp;
			                            <asp:Label ID="UI_lblShipmentInformation" runat="server" Text="155_Search for Shipping Information"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="20%">&nbsp;&nbsp;
			                            <asp:Label ID="UI_lblShipping" runat="server" Text="156_Shipping Number"></asp:Label>
                                    </td>
                                    <td width="40%">:
			                            <asp:Label ID="UI_lblShippingText" runat="server"></asp:Label>
                                    </td>
                                    <td width="15%" align="right">
                                        <asp:Label ID="UI_lblDate" runat="server" Text="097_Date"></asp:Label>
                                    </td>
                                    <td width="25%" align="left">:
			                            <asp:Label ID="UI_lblDateText" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>* 
			                            <asp:Label ID="UI_lblPacking" runat="server" Text="157_Packing List of"></asp:Label>
                                    </td>
                                    <td colspan="3">:
			                            <asp:Label ID="UI_lblPackingtTxt" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <%--<tr>
			                        <td>&nbsp;&nbsp;
			                            <asp:Label ID="UI_lblShipped" runat="server" Text="158_Shipped by"></asp:Label>
			                        </td>
			                        <td colspan="3">:
			                            <asp:Label ID="UI_lblShippedText" runat="server"></asp:Label>
			                        </td>
			                    </tr>--%>
                                <tr>
                                    <td>* 
			                            <asp:Label ID="UI_lblFrom" runat="server" Text="159_From"></asp:Label>
                                    </td>
                                    <td colspan="3">:
			                            <asp:Label ID="UI_lblFromText" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>* 
			                            <asp:Label ID="UI_lblTo" runat="server" Text="160_To"></asp:Label>
                                    </td>
                                    <td colspan="3">:
			                            <asp:Label ID="UI_lblCustomer" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;&nbsp;
			                            <asp:Label ID="UI_lblAddress" runat="server" Text="008_Address"></asp:Label>
                                    </td>
                                    <td colspan="3">:
			                            <asp:Label ID="UI_lblAddressText" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;&nbsp;
			                            <asp:Label ID="UI_lblExpress" runat="server" Text="161_Express Co."></asp:Label>
                                    </td>
                                    <td colspan="3">:
			                            <asp:Label ID="UI_lblExpressText" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>* 
			                            <asp:Label ID="UI_lblTracking" runat="server" Text="140_Tracking No."></asp:Label>
                                    </td>
                                    <td colspan="3">:
			                            <asp:Label ID="UI_lblTrackingText" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;&nbsp;
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
                        <td width="49%" align="left" class="default">
                            <asp:Label ID="UI_lblShippingTittle" runat="server" Text="162_Add Shipping Pack" Font-Bold="true"></asp:Label>
                        </td>
                        <td width="50%" align="right">&nbsp;</td>
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
                                    <asp:Label ID="UI_lblAddShippingTittle" runat="server" Text="162_Add Shipping Pack" Font-Bold="true" Font-Size="Larger"></asp:Label>

                                    <!--[Begin]ShippingList列表表單-->
                                    <asp:GridView ID="UI_dvShippingList" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric" Visible="false">
                                        <Columns>

                                            <asp:BoundField DataField="RMASHD_CTNNO" HeaderText="163_Ctn. & No" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            <asp:BoundField DataField="RMASHD_DESCRIPTION" HeaderText="099_Description" HeaderStyle-Width="25%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>

                                            <asp:TemplateField HeaderText="168_Shipment Notice Number">
                                                <HeaderStyle Width="15%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="UI_URLShipment" runat="server" Text='<%# Eval("RMASHD_SHIPMENTNO") %>'></asp:HyperLink>
                                                    <asp:Label ID="UI_RMASHDID" runat="server" Text='<%# Eval("RMASHD_ID") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="UI_Mark" runat="server" Text='<%# Eval("RMASHD_oldMark") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="RMASHD_QUANTITY" HeaderText="169_Quantity" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            <asp:BoundField DataField="RMASHD_NETWEIGHT" HeaderText="165_Net Weight" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            <asp:BoundField DataField="RMASHD_GROSSWEIGH" HeaderText="166_Gross Weigh" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            <asp:BoundField DataField="RMASHD_MEASUREMENT" HeaderText="167_Measurement" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>

                                        </Columns>
                                        <HeaderStyle CssClass="Text_Head" />
                                        <RowStyle CssClass="TR_1" />
                                        <AlternatingRowStyle CssClass="ListRowEven" />
                                        <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Center" />
                                    </asp:GridView>


                                    <asp:GridView ID="UI_dvSerial" runat="server" Width="100%" CellPadding="0" CellSpacing="1"
                                        border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true"
                                        PagerSettings-Mode="Numeric">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderStyle Width="5%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="UI_RMASMD_RMANO" runat="server" Text='<%# Eval("RMASMD_RMANO") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="UI_RMASMD_SERIALNO" runat="server" Text='<%# Eval("RMASMD_SERIALNO") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="UI_RMASMD_MODELNO" runat="server" Text='<%# Eval("RMASMD_MODELNO") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="UI_RMA_INVNO" runat="server" Text='<%# Eval("RMA_INVNO") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="UI_RMA_ARNO" runat="server" Text='<%# Eval("RMA_ARNO") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="RMASMD_RMANO" HeaderText="046_RMA Number" HeaderStyle-Width="30%"
                                                ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            <asp:BoundField DataField="RMASMD_SERIALNO" HeaderText="013_Serial Number" HeaderStyle-Width="32%"
                                                ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                            <asp:BoundField DataField="RMASMD_MODELNO" HeaderText="021_Model Name" HeaderStyle-Width="33%"
                                                ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        </Columns>
                                        <HeaderStyle CssClass="Text_Head" />
                                        <RowStyle CssClass="TR_1" />
                                        <AlternatingRowStyle CssClass="ListRowEven" />
                                        <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Center" />
                                    </asp:GridView>


                                    <input id="UI_cmdBack" runat="server" type="button" value="006_back" class="Problem_Edit" onclick="javascript:history.back();" />
                                    <asp:Button ID="UI_cmdAD" runat="server" CssClass="Confirm" Text="AD" />
                                    <asp:Button ID="UI_cmdINVOICE" runat="server" CssClass="Confirm" Text="INVOICE" />
                                    <asp:Button ID="UI_cmdMail" runat="server" CssClass="Confirm" Text="AR/Invoice Email" Visible="false" />
                                    <!--[End]ShippingList列表表單-->
                                </fieldset>
                            </div>
                            <!--[End]資料列表表單-->
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <asp:Label ID="UI_lblPreviousPage_RMASHID" runat="server" Visible="false"></asp:Label>
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />

    <asp:Label ID="UI_lblRMA_ARNO" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="UI_lblRMASH_COMPNO" runat="server" Visible="false"></asp:Label>

</asp:Content>

