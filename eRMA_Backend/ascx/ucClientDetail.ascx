<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ucClientDetail.ascx.vb" Inherits="ascx_ucClientDetail" %>



<asp:Panel ID="UI_panel" runat="server" Width="700px" style="display:none;position:absolute">
    <table id="table1" width="100%" align="center" border="0" cellspacing="1" class="form_div">
        <tr valign="top">
            <td>

                <div class="form_div" align="center" style="width: 98%" >
                <fieldset>
                <table id="table2" width="100%" height="100%" align="center" border="0" cellspacing="1" >
	                <tr class="default">
	                    <td align="center" colspan="4">
	                        <asp:Label ID="UI_lblRepairDetail" runat="server" Text ="111_Repair Detail" Font-Bold ="true" ForeColor ="red"></asp:Label>
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
	                        <asp:Label ID="UI_lblCustomerFile" runat="server" Text="123_Customer File" Font-Bold="true"></asp:Label>
	                    </td>
	                    <td align="left" colspan="3">:
						
						    <asp:Label ID="UI_Downloadlbl" runat="server" Text=""></asp:Label>
						
	                        <asp:HyperLink ID="UI_DownloadFile" runat="server" Target="_blank"  Visible="false"  ></asp:HyperLink>
	                    </td>
	                </tr>
	                
	                
<asp:Panel runat="server" ID="UI_panRepairedQUOTE" Visible="false">
	                <tr class="default">
	                    <td align="left" colspan="4" background="images/pic_15.gif">
	                        <asp:Label ID="UI_lblCharge" runat="server" Text="124_Charge" Font-Bold="true"></asp:Label>
	                    </td>
	                </tr>

		            <tr class="default">
	                    <td align="left" width="30%">
	                        <asp:Label ID="UI_lblLaborCost" runat="server" Text="125_Labor Cost" Font-Bold="true"></asp:Label>:
			                <asp:Label ID="UI_lblLaborCostText" runat="server" Font-Underline="true"></asp:Label>
			            </td>
			            <td align="left" width="30%">
			                 <asp:Label ID="UI_lblMaterialCost" runat="server" Text="126_Material Cost" Font-Bold="true"></asp:Label>:
			                <asp:Label ID="UI_lblMaterialCostText" runat="server" Font-Underline="true"></asp:Label>
			            </td>
			            <td colspan="2" align="left" width="40%">
			                <asp:Label ID="UI_lblTotalAmount" runat="server" Text="127_Total Amount" Font-Bold="true"></asp:Label>:
			                <asp:Label ID="UI_lblTotalAmountText" runat="server" Font-Underline="true"></asp:Label>
			            </td>
	                  </tr>
</asp:Panel>	                  

	                  
<asp:Panel runat="server" ID="UI_panReport" Visible="false">
	                <tr class="default">
	                    <td align="left" colspan="4" background="images/pic_15.gif">
	                        <asp:Label ID="UI_lblTittel" runat="server" Text="128_RMA Report Attachment" Font-Bold="true"></asp:Label>
	                    </td>
	                </tr>
	                <tr class="default" valign="top">
	                    <td align="left" colspan="4">
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
	                

<asp:Panel runat="server" ID="UI_panRepaired" Visible="false">
	                <tr class="default">
	                    <td align="left" colspan="4" background="images/pic_15.gif">
	                        <asp:Label ID="UI_lblRepairedTitle" runat="server" Text="202_RMA Repaired" Font-Bold="true"></asp:Label>
	                    </td>
	                </tr>
	                <tr class="default" valign="top">
	                    <td align="left" colspan="4">

                            <asp:GridView ID="UI_dvRepairDetail" runat ="server" Width = "100%" CellPadding ="0" CellSpacing ="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric"  >
                                <Columns>
                                  <asp:TemplateField>
                                     <HeaderStyle Width="3%" Height ="20px" HorizontalAlign="Center"></HeaderStyle>
                                     <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                     <ItemTemplate>
                                            <asp:Label ID ="SeqID" runat = "server" text='<%# Eval("SeqID") %>' ></asp:Label>
                                     </ItemTemplate>
                                  </asp:TemplateField>

                                  <asp:BoundField DataField="RMARED_NPARTNO" HeaderText="083_Part's No" HeaderStyle-Width="25%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                  <asp:BoundField DataField="RMARED_DESC" HeaderText="099_Description" HeaderStyle-Width="40%" ItemStyle-HorizontalAlign="left"></asp:BoundField>
                                  <asp:BoundField DataField="RMARED_QTY" HeaderText="103_Qty" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                </Columns> 
                                
                                <HeaderStyle CssClass="Text_Head"/>
                                <RowStyle CssClass="TR_1" />
                                <AlternatingRowStyle CssClass="ListRowEven" />
<%--                                <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Center" />--%>
                            </asp:GridView>

                        </td>
	                </tr>
</asp:Panel>	   
	                <tr class="default">
	                    <td align="left" colspan="4" background="images/pic_15.gif">
	                        <asp:Label ID="Label1" runat="server" Text="Purchasing Records : " Font-Bold="true"></asp:Label>
	                    </td>
	                </tr>
	                <tr class="default" valign="top">
	                    <td align="left" colspan="4">
                                                <asp:GridView ID="dgvPurchasing" runat="server" CssClass="default" Width="100%" PagerSettings-Mode="Numeric" AllowPaging="true" AutoGenerateColumns="False" border="0"
                                                    CellSpacing="1" CellPadding="0" AllowSorting="true">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="UI_SEQID" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="SerialNo" HeaderText="SerialNo"
                                                            HeaderStyle-Width="12%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="PurchaseDate" HeaderText="PurchaseDate"
                                                            HeaderStyle-Width="12%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="WarrantyCode" HeaderText="WarrantyCode"
                                                            HeaderStyle-Width="12%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="StartDate" HeaderText="StartDate"
                                                            HeaderStyle-Width="12%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                        <asp:BoundField DataField="EndDate" HeaderText="EndDate"
                                                            HeaderStyle-Width="12%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
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
                        </td>
	                </tr>
	            </table>
                </fieldset>	
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div align="center"><br />
	                <asp:Button ID="UI_cmdClose" runat ="server" Text = "039_Close Window" CssClass ="Confirm_l" />
	            </div>
            </td>
        </tr>
    </table>

     
    <asp:button id="UI_butTarget" Width="0px" Height="0px" runat="server" style="display:none"></asp:button>
    <asp:button id="UI_butOK" Width="0px" Height="0px" runat="server" style="display:none"></asp:button>

</asp:Panel>


<ajaxToolkit:ModalPopupExtender ID="ajModalProgress" TargetControlID="UI_butTarget"  
PopupControlID="UI_panel"  
OkControlID="UI_butOK" 
CancelControlID="UI_cmdClose"
BackgroundCssClass = "modalBackground"
runat="server">
</ajaxToolkit:ModalPopupExtender>
