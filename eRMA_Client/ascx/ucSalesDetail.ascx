<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ucSalesDetail.ascx.vb" Inherits="ascx_ucSalesDetail" %>


<asp:Panel ID="UI_panel" runat="server" Width="750px" style="height:620px;display:none;position:absolute;overflow-x: hidden; overflow-y: auto;border:3px solid #808080;background:#ffffff;">


<script type="text/javascript" language="javascript">
function FrmCancel(){
    if (confirm(CancelItemMsg))   {
        onProgress('Cancel')
        return true;
    }
    return false;
}


function calTotalAMT(){
    var blnFlag_Cal=false;

    var iTotalManAmt = 0;
    var iMaterialAmt = 0;
    var iLABORCOST = window.document.getElementById('<%=UI_txtSaleLaborCost.ClientID %>').value;
    var iMaterial = window.document.getElementById('<%=UI_txtSaleMaterialCost.ClientID %>').value;
    var UI_txtCU_DISCOUNT_OFF = window.document.getElementById('<%=UI_txtCU_DISCOUNT_OFF.ClientID %>').value;

    if (iLABORCOST!=""){
        if (isNaN(iLABORCOST)!=true){
            iTotalManAmt = parseFloat(iLABORCOST) ;
            blnFlag_Cal=true;
        }
    }

    if (iMaterial!=""){
        if (isNaN(iMaterial)!=true){
            //DISCOUNT_OFF = parseFloat(UI_txtCU_DISCOUNT_OFF) ;
            iMaterialAmt = parseFloat(iMaterial);
            blnFlag_Cal=true;
        }
    }

    var iTotalQuote=0;
    var UI_txtSaleQuote = window.document.getElementById('<%=UI_txtSaleQuote.ClientID %>');
    if (blnFlag_Cal==true){
        iTotalQuote = (iTotalManAmt + iMaterialAmt);
        iTotalQuote = Round(iTotalQuote,2);
        UI_txtSaleQuote.innerText=iTotalQuote;
    }
}


function  Round(value,dights)  
{  
	var nNewValue = Math.round (value*Math.pow(10,dights))/Math.pow(10,dights);  
	return  nNewValue;  
}  

</script>




    <%--<table id="table1" width="100%" height="80%" border="0"  align="center" border="0" cellspacing="1" bgcolor="#ffffff">
        <tr valign="top">
            <td valign="top">--%>
            <div>
                <fieldset class="form_div" valign="top" style="width:100%">

                <table id="table2" width="100%" align="center" border="0" cellspacing="1" >
	                <tr class="default">
	                    <td align="left" colspan="4">
	                        <asp:Label ID="UI_lblChargeInformation" runat="server" Text ="130_Charge Information" Font-Bold ="true" ForeColor ="red"></asp:Label>
	                    </td>
	                </tr>
	                <tr class="default">
	                    <td width="25%" align="left">
	                        <asp:Label ID="UI_lblSerial" runat="server" Text="013_Serial Number" Font-Bold="true"></asp:Label>.
	                    </td>
	                    <td width="30%" align="left">:
	                        <asp:Label ID="UI_lblSerialText" runat="server"></asp:Label>
	                    </td>
	                    <td width="15%" align="right">
	                        <asp:Label ID="UI_lblModel" runat="server" Text="035_Model" Font-Bold="true"></asp:Label>
	                    </td>
	                    <td width="30%" align="left">:
	                        <asp:Label ID="UI_lblModelText" runat="server"></asp:Label>
	                    </td>
	                </tr>
		            <tr class="default">
	                    <td align="left">
	                        <asp:Label ID="UI_lblFailure" runat="server" Text="023_Failure Reason" Font-Bold="true"></asp:Label>
	                    </td>
	                    <td align="left" colspan="3">:
	                        <asp:Label ID="UI_lblFailureText" runat="server"></asp:Label>
	                    </td>
	                </tr>
	                
		            <tr class="default">
	                    <td align="left">
                	        <asp:Label ID="UI_lblImproperUsage" runat ="server" Text="064_Improper Usage" Font-Bold="true"></asp:Label>
	                    </td>
	                    <td align="left" colspan="3">:
	                        <asp:Label ID="UI_lblImproperUsageText" runat="server"></asp:Label>
	                    </td>
	                </tr>
	                
		            <tr class="default">
	                    <td align="left">
	                        <asp:Label ID="UI_lblProductDesc" runat="server" Text="196_Product Desc" Font-Bold="true" ></asp:Label>.
	                    </td>
	                    <td align="left" colspan="3">:
	                        <asp:Label ID="UI_lblProductDescText" runat="server"></asp:Label>
	                    </td>
	                </tr>
	                
		            <tr class="default">
	                    <td align="left">
	                        <asp:Label ID="UI_lblProblemDesc" runat="server" Text="122_Problem Desc" Font-Bold="true" ></asp:Label>.
	                    </td>
	                    <td align="left" colspan="3">:
	                        <asp:Label ID="UI_lblProblemDescText" runat="server"></asp:Label>
	                    </td>
	                </tr>
	                
		            <tr class="default">
	                    <td align="left">
	                        <asp:Label ID="UI_lblDescription" runat="server" Text="099_Description" Font-Bold="true"></asp:Label>
	                    </td>
	                    <td align="left" colspan="3">:
	                        <asp:Label ID="UI_lblDescriptionText" runat="server"></asp:Label>
	                    </td>
	                </tr>

		            <tr class="default">
	                    <td align="left">
	                        <asp:Label ID="UI_lblDiscount" runat="server" Text="240_Discount" Font-Bold="true"></asp:Label>
	                    </td>
	                    <td align="left" colspan="3">:
	                        <asp:Label ID="UI_lblDiscountText" runat="server"></asp:Label>
	                        <asp:TextBox ID="UI_txtCU_DISCOUNT_OFF" Style="display: none; width: 1px" runat="server" ></asp:TextBox>
	                    </td>
	                </tr>

		            <tr class="default">
	                    <td align="left">
	                        <asp:Label ID="UI_lblCustomerFile" runat="server" Text="123_Customer File" Font-Bold="true"></asp:Label>
	                    </td>
	                    <td align="left" colspan="3">:
	                        <asp:HyperLink ID="UI_DownloadFile" runat="server" Target="_blank" ></asp:HyperLink>
	                    </td>
	                </tr>
	                
	                <tr class="default">
	                    <td align="left" colspan="4" background="images/pic_15.gif">
	                        <asp:Label ID="UI_lblRepairedCharge" runat="server" Text="131_Repaired Charge" Font-Bold="true"></asp:Label>
	                    </td>
	                </tr>
	                
	                
<!-- 維修報價零件資料 START -->
		            <tr class="default">
	                    <td colspan="4" align="center">
    
        <asp:DataList ID="UI_dvRepairDetail" runat="server" ExtractTemplateRows="true" BorderColor="#c0c0c0"
            CellSpacing="0" CellPadding="3" Width="95%" border="1" bordercolorlight="#c0c0c0">
            <HeaderTemplate>
                <asp:Table ID="oTableHeader" runat="server">
                    <asp:TableHeaderRow bgcolor="#fff4d0">
                        <asp:TableHeaderCell Width="2%" Visible="false">&nbsp;</asp:TableHeaderCell>
                        
                        <asp:TableHeaderCell Width="10%">
                            <asp:Label ID="lblHPart" runat="server" Text="083_Part'sNo"></asp:Label>
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="10%" Visible="false">
                            <asp:Label ID="lblHSerial" runat="server" Text="098_Serial No"></asp:Label>
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="28%">
                            <asp:Label ID="lblHDescription" runat="server" Text="099_Description"></asp:Label>
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="10%" Visible="false">
                            <asp:Label ID="lblHLocation" runat="server" Text="100_SMT Location"></asp:Label>
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="8%" Visible="false">
                            <asp:Label ID="lblHImproper" runat="server" Text="101_Improper Usage"></asp:Label>
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="19%" Visible="false">
                            <asp:Label ID="lblHReason" runat="server" Text="102_Defective Reason"></asp:Label>
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
                        <asp:TableCell Visible="false">
                            <asp:Label ID="lblNew" runat="server" Text="_New" class="default"></asp:Label>
                            <asp:Label ID="lblRMARQDID" runat="server" Text='<%# Eval("RMARQD_ID") %>' Visible="false" class="default"></asp:Label>
                            <asp:Label ID="lblIMPROPERUSAGE" runat="server" Text='<%# Eval("RMARQD_IMPROPERUSAGE") %>' class="default"></asp:Label>
                            <asp:Label ID="lblDEFECTIVE" runat="server" Text='<%# Eval("RMARQD_DEFECTIVE") %>' Visible="false" class="default"></asp:Label>
                            <asp:TextBox ID="UI_txtMaterialCost" runat="server" Text='<%# Eval("RMARQD_MATERIALCOST") %>' Style="display: none; width: 1px" class="default"></asp:TextBox>
                        </asp:TableCell>
                        
                        <asp:TableCell>
                            <asp:Label ID="txtNewPart" runat="server" Width="100" MaxLength="13" Text='<%# Eval("RMARQD_NPARTNO") %>' class="default"></asp:Label>
                        </asp:TableCell>
                        
                        <asp:TableCell Visible="false">
                            <asp:TextBox ID="txtNewSerial" runat="server" Width="80px" Text='<%# Eval("RMARQD_NSERIALNO") %>' class="default"></asp:TextBox>
                        </asp:TableCell>
                        
                        <asp:TableCell> <!--RowSpan="2"-->
                            <asp:Label ID="txtDescription" runat="server" Text='<%# Eval("RMARQD_DESC") %>' class="default"></asp:Label>
                        </asp:TableCell>
                        
                        <asp:TableCell HorizontalAlign="Center" Visible="false"> <!--RowSpan="2"-->
                            <asp:TextBox ID="txtLocation" runat="server" Width="80px" Text='<%# Eval("RMARQD_LOCATION") %>' class="default"></asp:TextBox>
                        </asp:TableCell>

                        <asp:TableCell HorizontalAlign="Center" Visible="false"> <!--RowSpan="2"-->
                            <asp:DropDownList ID="cboImproper" runat="server" class="default">
                                <asp:ListItem Value="1" Text="065_Yes"></asp:ListItem>
                                <asp:ListItem Value="0" Text="066_No" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                        </asp:TableCell>
                        
                        <asp:TableCell Visible="false"> <!--RowSpan="2"-->
                            <asp:DropDownList ID="UI_cboDefective" runat="server" class="default">
                            </asp:DropDownList>
                        </asp:TableCell>
                        
                        <asp:TableCell HorizontalAlign="Center"> <!--RowSpan="2"-->
                            <asp:Label ID="txtQty" runat="server" Width="20px" Text='<%# Eval("RMARQD_QTY") %>' class="default"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell> <!--RowSpan="2"-->
                            <asp:Label ID="txtPrice" runat="server" Width="50px" Text='<%# Eval("RMARQD_PRICE") %>' class="default"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                    
                    <asp:TableRow Visible="false">
                        <asp:TableCell Visible="false">
                            <asp:Label ID="lblOld" runat="server" Text="_Old" class="default"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell Visible="false">
                            <asp:Label ID="txtOldPart" runat="server" Width="100" MaxLength="13" Text='<%# Eval("RMARQD_OPARTNO") %>' class="default"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell Visible="false">
                            <asp:TextBox ID="txtOldSerial" runat="server" Width="80px" Text='<%# Eval("RMARQD_OSERIALNO") %>' class="default"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    
                </asp:Table>
            </ItemTemplate>
        </asp:DataList>
    
	                    
	                    </td>
                    </tr>
<!-- 維修報價零件資料 END -->
                


	                
		            <tr class="default">
	                    <td colspan="4" align="center">
	                        <table border="1" width="95%" id="table3" class="default" cellspacing="0" cellpadding="0" bordercolorlight="#FFFFFF" style="border-right-color: #C0C0C0; border-bottom-color: #C0C0C0; margin-top:10px; margin-bottom:10px">
			                    <tr bgcolor="#fff4d0">
			                        <td align="center" width="20%">&nbsp;</td>
			                        <td align="center">
			                            <asp:Label ID="UI_lblLaborCost" runat="server" Text="125_Labor Cost" Font-Bold="true"></asp:Label>
			                        </td>
			                        <td align="center">
			                            <asp:Label ID="UI_lblMaterialCost" runat="server" Text="126_Material Cost" Font-Bold="true"></asp:Label>
			                        </td>
			                        <td align="center">
			                            <asp:Label ID="UI_lblTotalAmount" runat="server" Text="127_Total Amount" Font-Bold="true"></asp:Label>
			                        </td>
			                    </tr>
			                    <tr>
			                        <td align="left" bgcolor="#fff4d0">
			                            <asp:Label ID="UI_lblRepairedQuote" runat="server" Text="132_Repaired Quote" Font-Bold="true"></asp:Label>
			                        </td>
			                        <td align="center">
			                            <asp:Label ID="UI_lblLaborCostText" runat="server"></asp:Label>
			                            <asp:TextBox ID="UI_txtLaborCost" runat="server" Visible="false"></asp:TextBox>
			                        </td>
			                        <td align="center">
			                            <asp:Label ID="UI_lblMaterialCostText" runat="server"></asp:Label>
			                            <asp:TextBox ID="UI_txtMaterialCost" runat="server" Visible="false"></asp:TextBox>
			                        </td>
			                        <td align="center">
			                            <asp:Label ID="UI_lblTotalAmountText" runat="server"></asp:Label>
			                        </td>
			                    </tr>
			                    <tr>
			                        <td align="left" bgcolor="#fff4d0">
			                            <asp:Label ID="UI_lblSalesQuote" runat="server" Text="133_Sales Quote" Font-Bold="true"></asp:Label>
			                        </td>
			                        <td align="center">
			                            <asp:Label ID="UI_lblSaleLaborCost_Code" runat="server"></asp:Label>
			                            <asp:TextBox ID="UI_txtSaleLaborCost" runat="server" Width="50px"></asp:TextBox>
			                        </td>
			                        <td align="center">
			                            <asp:Label ID="UI_lblSaleMaterialCost_Code" runat="server"></asp:Label>
			                            <asp:TextBox ID="UI_txtSaleMaterialCost" runat="server" Width="50px"></asp:TextBox>
			                        </td>
			                        <td align="center">
			                            <asp:Label ID="UI_lblSaleQuote_Code" runat="server"></asp:Label><asp:Label ID="UI_txtSaleQuote" runat="server"></asp:Label>
			                        </td>
			                    </tr>
			                    <tr>
			                        <td align="left" bgcolor="#fff4d0">
			                            <asp:Label ID="UI_lblRemark" runat="server" Text="134_Remark" Font-Bold="true"></asp:Label>
			                        </td>
			                        <td align="left" colspan="3">
			                            <asp:TextBox ID="UI_txtRemark" runat="server" TextMode="MultiLine" Rows ="3" Columns ="100"></asp:TextBox>
			                        </td>
			                    </tr>
			                </table>
			            </td>
	                  </tr>



<asp:Panel runat="server" ID="UI_panReportFile" Visible="false">
	                <tr class="default">
	                    <td align="left" colspan="4" background="images/pic_15.gif">
	                        <asp:Label ID="UI_lblTittel" runat="server" Text="128_RMA Report Attachment" Font-Bold="true"></asp:Label>
	                    </td>
	                </tr>
	                
	                <tr class="default" valign="top">
	                    <td align="left" colspan="4" valign="top">
	                        <asp:datalist id="UI_dvRepairUpload" runat="server" width="100%" border="1" cellspacing="0" cellpadding="0">
                                <ItemTemplate>
                                    <table width="100%" border="0" cellspacing="5" cellpadding="1">
                                        <tr valign="top" align ="left" height ="20px">
                                            <td width="33%" align="left">
                                                <asp:Label ID="UI_SeqID1" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="UI_lblRepair1" runat="server" Text="129_維修報告-" Visible="false"></asp:Label>
                                                <asp:HyperLink ID="UI_DownloadRepair1" runat="server" Target="_blank" Visible="false"></asp:HyperLink>
                                                <asp:Label ID="UI_UPLOADFILE1" runat="server" Text='<%# Eval("RMARU_UPLOADFILE_1") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="UI_lblSeqID1" runat="server" Text='<%# Eval("SeqID_1") %>' Visible="false"></asp:Label>
                                            </td>
                                            <td width="33%" align="left">
                                                <asp:Label ID="UI_SeqID2" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="UI_lblRepair2" runat="server" Text="129_維修報告-" Visible="false"></asp:Label>
                                                <asp:HyperLink ID="UI_DownloadRepair2" runat="server" Target="_blank" Visible="false"></asp:HyperLink>
                                                <asp:Label ID="UI_UPLOADFILE2" runat="server" Text='<%# Eval("RMARU_UPLOADFILE_2") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="UI_lblSeqID2" runat="server" Text='<%# Eval("SeqID_2") %>' Visible="false"></asp:Label>
                                            </td>
                                             <td width="34%" align="left">
                                                <asp:Label ID="UI_SeqID3" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="UI_lblRepair3" runat="server" Text="129_維修報告-" Visible="false"></asp:Label>
                                                <asp:HyperLink ID="UI_DownloadRepair3" runat="server" Target="_blank" Visible="false"></asp:HyperLink>
                                                <asp:Label ID="UI_UPLOADFILE3" runat="server" Text='<%# Eval("RMARU_UPLOADFILE_3") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="UI_lblSeqID3" runat="server" Text='<%# Eval("SeqID_3") %>' Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:datalist>             
                        </td>
	                </tr>
</asp:Panel>
	                
	                
	                <tr>
                        <td align="center" colspan="4">
                            <asp:Button ID="UI_cmdBack" runat ="server" Text = "_Back" CssClass ="Problem_Edit" />
                            <asp:Button ID="UI_cmdCancel" runat ="server" Text = "_Cancel" CssClass ="Confirm_l" CausesValidation="false" OnClientClick="return FrmCancel()" />
                            <asp:Button ID="UI_cmdSubmit" runat ="server" Text = "_Confirm" CssClass ="Confirm_l" ValidationGroup="SaleGroup" />
                        </td>
                    </tr>
	                
	            </table>
	            
                </fieldset>	
                </div>
       <%--     </td>
        </tr>
        
        
        
    </table>--%>
    
    <asp:Label ID="UI_lblRMASQID" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="UI_CurrencyCode" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="UI_CurrencyRate" runat="server" Visible="false"></asp:Label>
     
    <asp:button id="UI_butTarget" Width="0px" Height="0px" runat="server" style="display:none"></asp:button>
    <asp:button id="UI_butOK" Width="0px" Height="0px" runat="server" style="display:none"></asp:button>

    <asp:RequiredFieldValidator ID="rfvSaleLaborCost" runat="server" ErrorMessage="074_請輸入工時" ControlToValidate="UI_txtSaleLaborCost" Display="None" TabIndex="0" ValidationGroup="SaleGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="rfvSaleMaterialCost" runat="server" ErrorMessage="075_請輸入零件費用" ControlToValidate="UI_txtSaleMaterialCost" Display="None" TabIndex="0" ValidationGroup="SaleGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>

<%--    <asp:RequiredFieldValidator ID="rfvSaleQuote" runat="server" ErrorMessage="135_請輸入費用加總" ControlToValidate="UI_txtSaleQuote" Display="None" TabIndex="0" ValidationGroup="SaleGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>

    <asp:RangeValidator ID="rvSaleLaborCost" runat="server" ErrorMessage="076_輸入工時型態有誤" ControlToValidate="UI_txtSaleLaborCost" Display="None" MinimumValue="0" MaximumValue="100000" Type="Double" ValidationGroup="SaleGroup" SetFocusOnError="true"></asp:RangeValidator>
    <asp:RangeValidator ID="rvSaleMaterialCost" runat="server" ErrorMessage="077_輸入零件費用型態有誤" ControlToValidate="UI_txtSaleMaterialCost" Display="None" MinimumValue="0" MaximumValue="100000" Type="Double" ValidationGroup="SaleGroup" SetFocusOnError="true"></asp:RangeValidator>
<%--    <asp:RangeValidator ID="rvSaleQuote" runat="server" ErrorMessage="136_輸入費用加總型態有誤" ControlToValidate="UI_txtSaleQuote" Display="None" MinimumValue="0" MaximumValue="10000" Type="Double" ValidationGroup="SaleGroup" SetFocusOnError="true"></asp:RangeValidator>--%>
    
    <asp:ValidationSummary ID="vsSale" runat ="server" ShowMessageBox ="true" ShowSummary ="false" ValidationGroup="SaleGroup"/>
  
</asp:Panel>


<ajaxToolkit:ModalPopupExtender ID="ajModalProgress" TargetControlID="UI_butTarget"  
PopupControlID="UI_panel"  
OkControlID="UI_butOK" 
CancelControlID="UI_cmdBack"
BackgroundCssClass = "modalBackground"
runat="server">
</ajaxToolkit:ModalPopupExtender>
