<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Request_Detail.aspx.vb" Inherits="Request_Detail" Title="Untitled Page" %>

<%@ Register Src="ascx/UcSDCViewG.ascx" TagName="UcSDCViewG" TagPrefix="uc10" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <table border="0" width="100%" id="table4" cellspacing="0" cellpadding="0" height="100%">
        <tr>
            <td width="24" background="Images/pic_12.gif">&nbsp;</td>
            <td align="left">
                <table border="0" width="100%" cellspacing="0" cellpadding="0" height="100%">
                    <!--[Begin]Tittle-->
                    <tr>
                        <td width="3%">&nbsp;</td>
                        <td width="94%" align="left">
                            <asp:Label ID="UI_lblTittle" runat="server" Text="111_Repair Detail" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
                        </td>
                        <td width="3%">&nbsp;</td>
                    </tr>
                    <!--[End]Tittle-->
                    <!--[Begin]新增資料-->
                    <tr>
                        <td>&nbsp;</td>
                        <td align="left" valign="top" >
                            <table width="100%"align="center" border="0" cellspacing="1" class="default">
                                <tr>
                                    <td colspan="5">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
			                            <asp:Label ID="UI_lblRMANo" runat="server" Text="029_RMA No."></asp:Label>
                                    </td>
                                    <td>:
			                            <asp:Label ID="UI_lblRMANoText" runat="server"></asp:Label>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>

                                <tr>
                                    <td width="20%">&nbsp;
				                        <asp:Label ID="UI_lblSerial" runat="server" Text="013_Serial Number"></asp:Label>
                                    </td>
                                    <td width="35%">:
				                        <asp:Label ID="UI_lblSerialText" runat="server"></asp:Label>
                                    </td>
                                    <td width="15%" align="right">
                                        <asp:Label ID="UI_lblCustomer" runat="server" Text="030_Customer"></asp:Label>
                                    </td>
                                    <td width="35%" align="left">:
				                        <asp:Label ID="UI_lblCustomerText" runat="server"></asp:Label>
                                    </td>
                                </tr>

                                <tr runat="server" id="uiTR_LaborHourCost">
                                    <td>&nbsp;
				                        <asp:Label ID="UI_lblLaborHourCost" runat="server" Text="125_Labor Cost"></asp:Label>
                                    </td>
                                    <td>:
				                        <asp:Label ID="UI_lblLaborHourvalue" runat="server"></asp:Label>&nbsp;
				                        <asp:Label ID="UI_lblLaborHourText" runat="server" Text="057_hour" Visible="false"></asp:Label>
                                        <asp:Label ID="UI_lblSymbol" runat="server" Text="=" Visible="false"></asp:Label>
                                        <asp:Label ID="UI_lblHourCode" runat="server"></asp:Label>
                                        <asp:Label ID="UI_lblHourQuotedText" runat="server"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="UI_lblQuote" runat="server" Text="113_Estimated Amount"></asp:Label>
                                    </td>
                                    <td align="left">:
				                        <asp:Label ID="UI_lblQuoteText" runat="server"></asp:Label>
                                        <asp:Label ID="UI_lblQuoteCode" runat="server"></asp:Label>
                                    </td>
                                </tr>

                                <tr runat="server" id="Tr1">
                                    <td>&nbsp;
				                        RMA Quoted 
                                    </td>
                                    <td>:
				                        <asp:Label ID="UI_lblRMAQuoteText" runat="server"></asp:Label>&nbsp;
				                        <asp:Label ID="UI_lblRMAQuoteCode" runat="server"></asp:Label>
                                    </td>
                                    <td align="right">Sales Quoted
                                    </td>
                                    <td align="left">:
				                        <asp:Label ID="UI_lblSalesQuoteText" runat="server"></asp:Label>
                                        <asp:Label ID="UI_lblSalesQuoteCode" runat="server"></asp:Label>
                                    </td>
                                </tr>

                                <tr runat="server" id="Tr2">

                                    <td>
                                        <asp:Label ID="RMAD_RMANO" runat="server" Text="Standard Battery"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="txtRMAD_RMANO_QTY" runat="server"></asp:Label>
                                        <asp:Label ID="LabRMAD_RMANO_QTY" runat="server" Text=""></asp:Label>
                                    </td>

                                    <td align="right">
                                        <asp:Label ID="Insurance_Label" runat="server" Text="Apply Total "></asp:Label>
                                        <asp:Label ID="Apply_Label" runat="server" Text="Loss Insurance:"></asp:Label>
                                    </td>
                                    <td align="left">

                                        <asp:Label ID="UI_Apply_Total_Loss_Insurance" runat="server" Text="No"></asp:Label>
                                    </td>

                                </tr>

                                <tr>
                                    <td>&nbsp;
				                        <asp:Label ID="UI_lblFailure" runat="server" Text="023_Failure Reason"></asp:Label>
                                    </td>
                                    <td>:
				                        <asp:Label ID="UI_lblFailureText" runat="server"></asp:Label>
                                        <asp:Label ID="UI_lblFARCNO" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="UI_lblFARNO" runat="server" Visible="false"></asp:Label>
                                    </td>
                                    <%-- 需求新增:BI保固 By buck Add 20250902 begin --%>
                                    <td id="UI_lblApplyBI_TD" runat ="server" align="right">&nbsp;
                                        <asp:Label ID="UI_lblApplyBatteryInsurance" runat="server" Text="222_Apply Battery Insurance"></asp:Label>
                                    </td>
                                    <td id="UI_lblApplyBIText_TD" runat ="server" align="left">
                                        <table width="100%">
                                            <tr>
                                                <td align="left">:
                                                    <asp:Label ID="UI_lblApplyBatteryInsuranceText" runat="server"></asp:Label>
                                                </td>
                                                <td align="left">
                                                     <asp:Label ID="uiTxt_ApplyBatteryInsurance" runat="server" Text="223_Apply Battery Insurance"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                     <%-- 需求新增:BI保固 By buck Add 20250902 end --%>
                                </tr>

                                <tr>
                                    <td>&nbsp;
				                        <asp:Label ID="UI_lblProblemDesc" runat="server" Text="025_Problem Description"></asp:Label>
                                    </td>
                                    <td>:
				                        <asp:Label ID="UI_lblProblemDescText" runat="server"></asp:Label>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>

                                <tr>
                                    <td>&nbsp;
				                        <asp:Label ID="UI_lblRepairDesc" runat="server" Text="053_Repair Description"></asp:Label>
                                    </td>
                                    <td>:
						                <asp:Label ID="UI_lblRepairDescText" runat="server"></asp:Label>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>

                                <tr>
                                    <td>&nbsp;
				                        <asp:Label ID="UI_lblRepairMemo" runat="server" Text="054_Repair Memo"></asp:Label>
                                    </td>
                                    <td>:
				                        <asp:Label ID="UI_lblRepairMemoText" runat="server"></asp:Label>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td valign="middle">&nbsp;
				                        <asp:Label ID="UI_lblReportAttachment" runat="server" Text="182_Report Attachment"></asp:Label>
                                    </td>
                                    <td colspan="3" align="left">
                                        <asp:GridView ID="UI_dvRetailUpload" runat="server" border="1" Width="85%" CellSpacing="0" CellPadding="0" CssClass="default" BorderColor="#C0C0C0" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric">
                                            <Columns>
                                                <asp:TemplateField HeaderText="117_File Name">
                                                    <HeaderStyle Width="20%" Height="15px" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="left" Height="15px"></ItemStyle>
                                                    <ItemTemplate>
                                                        &nbsp;
                                                        <asp:HyperLink runat="server" ID="UI_RepairUpload" Target="_self"></asp:HyperLink>
                                                        <asp:Label ID="UI_lblRepairUpload" runat="server" Text='<%# Eval("RMARU_UPLOADFILE") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMARUID" runat="server" Text='<%# Eval("RMARU_ID") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="RMARU_DESC" HeaderText="118_File Description" HeaderStyle-Width="40%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="RMARU_LUSTMP" HeaderText="097_Date" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Center" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd hh:ss}"></asp:BoundField>

                                            </Columns>
                                            <HeaderStyle CssClass="Text_Head" />
                                            <RowStyle CssClass="TR_1" />
                                            <AlternatingRowStyle CssClass="ListRowEven" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>&nbsp;</td>
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
                                        <asp:Label ID="UI_lblInformationTittle" runat="server" Text="082_Replace Component" Font-Bold="true"></asp:Label>
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
                        <table class="default" style="border-collapse: collapse" bordercolor="#111111" cellspacing="0" cellpadding="5" width="100%" border="0">
                            <tr>
                                <td>
                                    <!-- [Begin] Search Export -->
                                    <asp:DataList ID="UI_dvRepairDetail" runat="server" ExtractTemplateRows="true" BorderColor="#c0c0c0" CellSpacing="0" CellPadding="3" Width="100%" border="1" bordercolorlight="#c0c0c0">
                                        <HeaderTemplate>
                                            <asp:Table ID="oTableHeader" runat="server">
                                                <asp:TableHeaderRow bgcolor="#fff4d0">
                                                    <asp:TableHeaderCell Width="15%">
                                                        <asp:Label ID="lblHNewPart" runat="server" Text="184_New Part No"></asp:Label>
                                                    </asp:TableHeaderCell>
                                                    <asp:TableHeaderCell Width="15%">
                                                        <asp:Label ID="lblHNewSerial" runat="server" Text="185_New Serial No"></asp:Label>
                                                    </asp:TableHeaderCell>
                                                    <asp:TableHeaderCell Width="10%" RowSpan="2">
                                                        <asp:Label ID="lblHDuty" runat="server" Text="051_Duty"></asp:Label>
                                                    </asp:TableHeaderCell>
                                                    <asp:TableHeaderCell Width="30%" RowSpan="2">
                                                        <asp:Label ID="lblHDescription" runat="server" Text="099_Description"></asp:Label>
                                                    </asp:TableHeaderCell>
                                                    <asp:TableHeaderCell Width="10%" RowSpan="2">
                                                        <asp:Label ID="lblHLocation" runat="server" Text="084_Location"></asp:Label>
                                                    </asp:TableHeaderCell>
                                                    <asp:TableHeaderCell Width="10%" RowSpan="2">
                                                        <asp:Label ID="lblHQty" runat="server" Text="103_Qty"></asp:Label>
                                                    </asp:TableHeaderCell>
                                                    <asp:TableHeaderCell Width="10%" RowSpan="2">
                                                        <asp:Label ID="lblHPrice" runat="server" Text="104_Price"></asp:Label>
                                                    </asp:TableHeaderCell>
                                                    <asp:TableHeaderCell Width="10%" RowSpan="2">
                                                        <asp:Label ID="lblHOption" runat="server" Text="032_Option"></asp:Label>
                                                    </asp:TableHeaderCell>
                                                </asp:TableHeaderRow>
                                                <asp:TableHeaderRow bgcolor="#fff4d0">
                                                    <asp:TableHeaderCell>
                                                        <asp:Label ID="lblHPart" runat="server" Text="083_Part's No"></asp:Label>
                                                    </asp:TableHeaderCell>
                                                    <asp:TableHeaderCell>
                                                        <asp:Label ID="lblHSerial" runat="server" Text="098_Serial No"></asp:Label>
                                                    </asp:TableHeaderCell>
                                                </asp:TableHeaderRow>
                                            </asp:Table>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Table ID="oTableRow" runat="server">
                                                <asp:TableRow>
                                                    <asp:TableCell>
                                                        <asp:Label ID="lblNewPart" runat="server" Text='<%# Eval("RMARED_NPARTNO") %>'></asp:Label>
                                                    </asp:TableCell>
                                                    <asp:TableCell>
                                                        <asp:Label ID="lblNewSerial" runat="server" Text='<%# Eval("RMARED_NSERIALNO") %>'></asp:Label>
                                                    </asp:TableCell>
                                                    <asp:TableCell RowSpan="2">
                                                        <asp:Label ID="lblDuty" runat="server" Text='<%# Eval("DEFECTIVE") %>'></asp:Label>
                                                    </asp:TableCell>
                                                    <asp:TableCell RowSpan="2">
                                                        <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("RMARED_DESC") %>'></asp:Label>
                                                    </asp:TableCell>
                                                    <asp:TableCell RowSpan="2">
                                                        <asp:Label ID="lblLocation" runat="server" Text='<%# Eval("RMARED_LOCATION") %>'></asp:Label>
                                                    </asp:TableCell>
                                                    <asp:TableCell RowSpan="2">
                                                        <asp:Label ID="lblQty" runat="server" Text='<%# Eval("RMARED_QTY") %>'></asp:Label>
                                                        <asp:Label ID="lblMaterialCost" runat="server" Text='<%# Eval("RMARED_MATERIALCOST") %>' Visible="false"></asp:Label>
                                                    </asp:TableCell>
                                                    <asp:TableCell RowSpan="2">
                                                        <asp:Label ID="lblPrice" runat="server" Text='<%# Eval("PRICE") %>'></asp:Label>
                                                    </asp:TableCell>
                                                    <asp:TableCell RowSpan="2">
                                                        <asp:Label ID="lblOption" runat="server" Text='<%# Eval("RMARED_OPTION") %>'></asp:Label>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell>
                                                        &nbsp;
                                                <asp:Label ID="lblPart" runat="server" Text='<%# Eval("RMARED_OPARTNO") %>'></asp:Label>
                                                    </asp:TableCell>
                                                    <asp:TableCell>
                                                        &nbsp;
                                                <asp:Label ID="lblSerial" runat="server" Text='<%# Eval("RMARED_OSERIALNO") %>'></asp:Label>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>
                                        </ItemTemplate>
                                    </asp:DataList>
                                    <!-- [End] Search Export -->

                                    <table id="oTable" runat="server" class="default" visible="false" bordercolor="#c0c0c0" cellspacing="0" cellpadding="3" width="100%" border="0">
                                        <tr>
                                            <td bgcolor="#ffeeb2" align="left" width="25%">
                                                <asp:Label ID="UI_lblLaborCost" runat="server" Text="125_Labor Cost"></asp:Label>
                                                <asp:Label ID="UI_lblLaborCost_Delimited" runat="server" Text=" : "></asp:Label>

                                                <asp:Label ID="UI_lblCurrencySymbol1" runat="server"></asp:Label>
                                                <asp:Label ID="UI_lblLaborTotal" runat="server" Text="0"></asp:Label>
                                            </td>

                                            <td bgcolor="#ffeeb2" align="left" width="25%">
                                                <asp:Label ID="UI_lblMaterialCost" runat="server" Text="126_Material Cost"></asp:Label>
                                                <asp:Label ID="UI_lblMaterialCost_Delimited" runat="server" Text=" : "></asp:Label>

                                                <asp:Label ID="UI_lblCurrencySymbol2" runat="server"></asp:Label>
                                                <asp:Label ID="UI_lblMaterialTotal" runat="server" Text="0"></asp:Label>
                                            </td>

                                            <td bgcolor="#ffeeb2" align="right">
                                                <asp:Label ID="UI_lblTotalText" runat="server" Text="088_Total Amount"></asp:Label>
                                                <asp:Label ID="UI_lblTotalText_Delimited" runat="server" Text=" : "></asp:Label>
                                            </td>

                                            <td bgcolor="#ffeeb2" align="left" width="20%">
                                                <asp:Label ID="UI_lblCurrencyCode1" runat="server" Font-Bold="true"></asp:Label>
                                                <asp:Label ID="UI_lblTotal" runat="server" Text="0" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>

                                    <p align="center" style="margin-top: 10px; margin-bottom: 0">
                                        <div align="left">
                                            <asp:Label ID="UI_lblStatusHistory" runat="server" Text="183_Status History" Font-Bold="true"></asp:Label>
                                        </div>
                                </td>
                            </tr>

                            <!-- 簽核 START -->
                            <tr>
                                <td>
                                    <table id="table1" class="default" bordercolor="#c0c0c0" cellspacing="0" cellpadding="3" width="100%" border="1" bordercolorlight="#FFFFFF">
                                        <tr bgcolor="#fff4d0">
                                            <td align="center" width="10%">&nbsp;</td>
                                            <td align="center" width="15%">
                                                <asp:Label ID="UI_lblReceived" runat="server" Text="090_Received"></asp:Label></td>
                                            <td align="center" width="15%">
                                                <asp:Label ID="UI_lblRepair" runat="server" Text="091_Repair Quoted"></asp:Label></td>
                                            <td align="center" width="15%">
                                                <asp:Label ID="UI_lblSales" runat="server" Text="092_Sales Confirmed"></asp:Label></td>
                                            <td align="center" width="15%">
                                                <asp:Label ID="UI_lblClient" runat="server" Text="093_Client Confirmed"></asp:Label></td>
                                            <td align="center" width="15%">
                                                <asp:Label ID="UI_lblRepaired" runat="server" Text="094_Repaired"></asp:Label></td>
                                            <td align="center" width="15%">
                                                <asp:Label ID="UI_lblClose" runat="server" Text="095_Close" Font-Bold="true" ForeColor="red"></asp:Label></td>
                                            <td align="center" width="15%">
                                                <asp:Label ID="UI_lblCancel" runat="server" Text="041_Cancel"></asp:Label></td>
                                        </tr>


                                        <tr>
                                            <td align="center" bgcolor="#fff4d0">
                                                <asp:Label ID="UI_lblApprover" runat="server" Text="096_Approver"></asp:Label></td>
                                            <td align="center">
                                                <asp:Label ID="UI_lblReceivedUser" runat="server"></asp:Label>&nbsp;</td>
                                            <td align="center">
                                                <asp:Label ID="UI_lblRepairQuotedUser" runat="server"></asp:Label>&nbsp;</td>
                                            <td align="center">
                                                <asp:Label ID="UI_lblSalesUser" runat="server"></asp:Label>&nbsp;</td>
                                            <td align="center">
                                                <asp:Label ID="UI_lblClientUser" runat="server"></asp:Label>&nbsp;</td>
                                            <td align="center">
                                                <asp:Label ID="UI_lblRepairedUser" runat="server"></asp:Label>&nbsp;</td>
                                            <td align="center">
                                                <asp:Label ID="UI_lblCloseUser" runat="server"></asp:Label>&nbsp;</td>
                                            <td align="center">
                                                <asp:Label ID="UI_lblCancelUser" runat="server"></asp:Label>&nbsp;</td>
                                        </tr>


                                        <tr>
                                            <td align="center" bgcolor="#fff4d0">
                                                <asp:Label ID="UI_lblDate" runat="server" Text="097_Date"></asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="UI_lblReceivedDate" runat="server"></asp:Label>&nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="UI_lblRepairQuotedDate" runat="server"></asp:Label>&nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="UI_lblSalesDate" runat="server"></asp:Label>&nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="UI_lblClientDate" runat="server"></asp:Label>&nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="UI_lblRepairedDate" runat="server"></asp:Label>&nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="UI_lblCloseDate" runat="server"></asp:Label>&nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="UI_lblCancelDate" runat="server"></asp:Label>&nbsp;</td>
                                        </tr>
                                    </table>
                                    <p>
                                </td>
                            </tr>
                            <!-- 簽核 END -->
                            <tr align="left" style="font: 10px;">
                                <td>
                                    <uc10:UcSDCViewG ID="UcSDCViewG" runat="server" />
                                </td>
                            </tr>

                        </table>
                    </fieldset>
                </div>
            </td>
        </tr>

        <tr height="10%">
            <td background="Images/pic_20.gif">&nbsp;</td>
            <td valign="top" bgcolor="#E3D8BE" align="center">
                <!--[Begin]頁數-->
                <table border="0" width="100%" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="1%">&nbsp;</td>
                        <td width="99%" align="center" class="default">
                            <input id="UI_cmdBack" runat="server" type="button" value="006_Back" class="Problem_Edit" onclick="javascript:history.back();" />&nbsp;
                        </td>
                    </tr>
                </table>
                <!--[End]頁數-->
            </td>
        </tr>
    </table>

    <asp:Label ID="UI_lblPreviousPage_RMADID" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="UI_lblPreviousPage_RMANO" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="UI_lblMaterial" runat="server" Text="0" Visible="false"></asp:Label>
    <asp:Label ID="UI_Status" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="UI_flowCase" runat="server" Visible="false"></asp:Label>

</asp:Content>

