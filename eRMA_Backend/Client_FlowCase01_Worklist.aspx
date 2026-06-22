<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Client_FlowCase01_Worklist.aspx.vb" Inherits="Client_FlowCase01_Worklist" Title="Untitled Page" %>

<%@ Register Src="ascx/ucProgressStatus.ascx" TagName="ucProgressStatus" TagPrefix="uc1" %>
<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>
<%@ Register Src="ascx/ucMessageLage.ascx" TagName="ucMessageLage" TagPrefix="uc2" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <asp:Button ID="Button1" runat="server" Text="Button" Visible="false" />
    <uc1:ucProgressStatus ID="ucProgressStatus" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table border="0" cellspacing="0" cellpadding="0" width="100%" height="410px">
                <tr height="90px">
                    <td width="24" background="Images/pic_12.gif">&nbsp;</td>
                    <td valign="top" align="left">

                        <asp:Panel runat="server" ID="UI_panSearch" DefaultButton="UI_cmdSearch">
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
                                                <td colspan="4">
                                                    <asp:Label ID="UI_lblRequestInformation" runat="server" Text="028_Search for Request Items Information"></asp:Label>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td width="10%">
                                                    <asp:Label ID="UI_lblRMANo" runat="server" Text="029_RMA No."></asp:Label>
                                                </td>
                                                <td width="40%">:
			                            <asp:TextBox ID="UI_txtRMANo" runat="server" Width="150"></asp:TextBox>
                                                </td>


                                                <td width="12%" align="right">&nbsp;
			                            <asp:Label ID="UI_lblStatus" runat="server" Text="032_Status" Visible="false"></asp:Label>
                                                </td>
                                                <td width="30%" align="left">&nbsp;<!--:-->
                                                    <asp:DropDownList ID="UI_cboStatus" runat="server" Visible="false">
                                                        <asp:ListItem Text="-All-" Value="-1"></asp:ListItem>
                                                        <asp:ListItem Text="Request" Value="10"></asp:ListItem>
                                                        <asp:ListItem Text="Processing" Value="20"></asp:ListItem>
                                                        <asp:ListItem Text="Shipped" Value="90"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>

                                            </tr>

                                            <tr>
                                                <td>
                                                    <asp:Label ID="UI_lblRequestDate" runat="server" Text="033_Request Date"></asp:Label>
                                                </td>
                                                <td align="left" colspan="3">:
			                            <asp:DropDownList ID="UI_cboBYear" runat="server"></asp:DropDownList>
                                                    <asp:DropDownList ID="UI_cboBMonth" runat="server"></asp:DropDownList>
                                                    <asp:DropDownList ID="UI_cboBDay" runat="server"></asp:DropDownList>&nbsp;~&nbsp;
                                        <asp:DropDownList ID="UI_cboEYear" runat="server"></asp:DropDownList>
                                                    <asp:DropDownList ID="UI_cboEMonth" runat="server"></asp:DropDownList>
                                                    <asp:DropDownList ID="UI_cboEDay" runat="server"></asp:DropDownList>
                                                    <asp:Button ID="UI_cmdSearch" runat="server" Text="_Search" CssClass="Search" Style="height: 18px" />
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
                        </asp:Panel>

                    </td>
                </tr>

                <tr height="28px">
                    <td width="24" background="Images/pic_14.gif">&nbsp;</td>
                    <td background="Images/pic_15.gif">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="1%">&nbsp;</td>
                                <td width="20%" align="left" class="default">
                                    <asp:Label ID="UI_lblRequestedTittle" runat="server" Text="034_Requested Information" Font-Bold="true"></asp:Label>
                                </td>
                                <td width="79%" align="left" class="default">
                                    <asp:Label ID="UI_lblQuickTittle" runat="server" Text="171_Please click check boxes and click 'confirm' button to start repairing."></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <tr valign="top">
                    <td background="Images/pic_20.gif">&nbsp;</td>
                    <td valign="top" bgcolor="#E3D8BE" align="center">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="1%">&nbsp;</td>
                                <td width="99%" align="left" class="default">
                                    <!--[Begin]資料列表表單-->
                                    <div align="center">


                                        <asp:GridView ID="UI_dvCustomer" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowSorting="true">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                                    <HeaderTemplate>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="UI_SeqNo" runat="server" Text='<%# Eval("SeqNo") %>'></asp:Label>
                                                        <asp:Label ID="UI_RMANO" runat="server" Text='<%# Eval("RMA_NO") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMAID" runat="server" Text='<%# Eval("RMA_ID") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMASTATUS" runat="server" Text='<%# Eval("RMAD_STATUS") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMADID" runat="server" Text='<%# Eval("RMAD_ID") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMASQID" runat="server" Text='<%# Eval("RMASQ_ID") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="RMA_NO" HeaderText="029_RMA No" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center" SortExpression="RMA_NO"></asp:BoundField>
                                                <asp:BoundField DataField="RequestDate" HeaderText="033_Request Date" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center" SortExpression="RequestDate"></asp:BoundField>
                                                <asp:BoundField DataField="Applicant" HeaderText="006_Applicant" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center" HtmlEncode="False" SortExpression="Applicant"></asp:BoundField>

                                                <asp:TemplateField HeaderText="127_Total Amount" SortExpression="QUOTE">
                                                    <HeaderStyle Width="17%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="UI_CurrencyCode" runat="server" Text='<%# Eval("CurrencyCode") %>' Visible="false"></asp:Label>&nbsp;
                                                <asp:Label ID="UI_Quote" runat="server" Text='<%# Eval("QUOTE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Net Charge" SortExpression="Net_Charge">
                                                    <HeaderStyle Width="17%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Net_Charge_QUOTE_UI_CurrencyCode" runat="server" Text='<%# Eval("CurrencyCode") %>' Visible="false"></asp:Label>&nbsp;
                                                <asp:Label ID="Net_Charge_QUOTE_UI_Quote" runat="server" Text='<%# Eval("Net_Charge_QUOTE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:BoundField DataField="RequestQty" HeaderText="213_Request Qty" HeaderStyle-Width="12%" ItemStyle-HorizontalAlign="Center" SortExpression="RequestQty"></asp:BoundField>
                                                <asp:BoundField DataField="ShippedQty" HeaderText="214_Shipped Qty" HeaderStyle-Width="12%" ItemStyle-HorizontalAlign="Center" Visible="false" SortExpression="ShippedQty"></asp:BoundField>
                                                <asp:BoundField DataField="Remark" HeaderText="134_Remark" HeaderStyle-Width="25%" ItemStyle-HorizontalAlign="Center" SortExpression="Remark" Visible="false"></asp:BoundField>

                                                <asp:TemplateField HeaderText="032_Status" SortExpression="RMAD_STATUS">
                                                    <HeaderStyle Width="7%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="UI_Status" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="038_Detail">
                                                    <HeaderStyle Width="7%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="UI_imgDetail" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/icon-detail.gif" CommandArgument='<%#Me.UI_dvCustomer.Rows.Count%>' CommandName="cmdDetail" />
                                                        <asp:Button ID="UI_cmdEdit" runat="server" Text="_Edit" CssClass="Pick" Width="50" CommandName="cmdEdit" CommandArgument='<%#Me.UI_dvCustomer.Rows.Count%>' PostBackUrl="~/Request_New.aspx" Visible="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>

                                            <EmptyDataTemplate>
                                                <asp:Label runat="server" ID="UI_NotFound" Text='<%# Session("_isShowNotFound").tostring()%>'></asp:Label>
                                            </EmptyDataTemplate>

                                            <FooterStyle CssClass="TR_1" HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="Text_Head" />
                                            <RowStyle CssClass="TR_1" />
                                            <AlternatingRowStyle CssClass="ListRowEven" />
                                            <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Center" />
                                        </asp:GridView>

                                    </div>
                                    <!--[End]資料列表表單-->
                                </td>
                            </tr>

                        </table>
                    </td>
                </tr>

                <tr style="height: 20px">
                    <td>&nbsp;</td>
                </tr>

                <asp:Panel runat="server" ID="UIPanel_RMADetail" Visible="false">
                    <!-- style="background-color:Silver"-->
                    <tr style="background-color: Silver">
                        <td>&nbsp;</td>
                        <td align="left">
                            <table width="95%" border="0" cellspacing="1" cellpadding="0" class="default">
                                <tr>
                                    <td width="15%">
                                        <asp:Label ID="UI_lblRMANo_Detail" runat="server" Text="029_RMA No."></asp:Label></td>
                                    <td width="35%">:&nbsp;<asp:Label ID="UI_RMANo" runat="server"></asp:Label></td>
                                    <td width="12%" align="right">
                                        <asp:Label ID="UI_lblRequestDate_Detail" runat="server" Text="033_Request Date"></asp:Label></td>
                                    <td width="30%" align="left">:&nbsp;<asp:Label ID="UI_RequestDate" runat="server"></asp:Label></td>
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

                    <tr height="10px">
                        <td width="24" background="Images/pic_14.gif">&nbsp;</td>
                        <td background="Images/pic_15.gif">
                            <table border="0" width="100%" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td width="1%">&nbsp;</td>
                                    <td width="99%" align="left" class="default">
                                        <asp:Label ID="UI_lblRequestedTittle_Detail" runat="server" Text="Requested Information Detail" Font-Bold="true"></asp:Label>
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
                                                    <HeaderTemplate>
                                                        <asp:Label runat="server" ID="UI_lblAccept" Text="Accept"></asp:Label><br />
                                                        <asp:CheckBox ID="UI_CheckGroup_Accept" runat="server" AutoPostBack="true" OnCheckedChanged="UI_checkGroup_Accept_CheckedChanged" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="UI_Check_Accept" runat="server" AutoPostBack="true" OnCheckedChanged="UI_check_Accept_CheckedChanged" />
                                                        <asp:Label runat="server" ID="UI_Accept" Text="V" Visible="false"></asp:Label>

                                                        <asp:Label ID="UI_SeqID" runat="server" Text='<%# Eval("SeqID") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMADWARRANTY" runat="server" Text='<%# Eval("RMAD_WARRANTY") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMADCSTMP" runat="server" Text='<%# Eval("RMAD_CSTMP") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMANO" runat="server" Text='<%# Eval("RMAD_RMANO") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMADID" runat="server" Text='<%# Eval("RMAD_ID") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMAD_STATUS" runat="server" Text='<%# Eval("RMAD_STATUS") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_SERIALNO" runat="server" Text='<%# Eval("RMAD_SERIALNO") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_PARTSN" runat="server" Text='<%# Eval("RMAD_PARTSN") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMAD_ISWARRANTY" runat="server" Text='<%# Eval("RMAD_ISWARRANTY") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMARQ_ID" runat="server" Text='<%# Eval("RMARQ_ID") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_RMARQ_IMPROPERUSAGE" runat="server" Text='<%# Eval("RMARQ_IMPROPERUSAGE") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <asp:Label runat="server" ID="UI_lblReject" Text="Reject"></asp:Label><br />
                                                        <asp:CheckBox ID="UI_CheckGroup_Reject" runat="server" AutoPostBack="true" OnCheckedChanged="UI_checkGroup_Reject_CheckedChanged" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="UI_Check_Reject" runat="server" AutoPostBack="true" OnCheckedChanged="UI_check_Reject_CheckedChanged" />
                                                        <asp:Label runat="server" ID="UI_Reject" Text="V" Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="RMAD_SERIALNO" HeaderText="013_Serial Number" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="RMAD_MODELNO" HeaderText="035_Model" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="WARRANTY" HeaderText="015_Warranty" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd}"></asp:BoundField>

                                                <asp:BoundField DataField="IMPROPERUSAGE" HeaderText="064_Improper Usage" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="FailureReason" HeaderText="023_Failure Reason" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd}"></asp:BoundField>

                                                <asp:TemplateField HeaderText="125_Labor Cost">
                                                    <HeaderStyle Width="8%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <FooterStyle HorizontalAlign="Center" CssClass="text9pt" />
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="UI_ServiceCharge" Text='<%# Eval("LaborCost") %>'></asp:Label>
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
                                                        <asp:Label runat="server" ID="UI_MaterialCost" Text='<%# Eval("MaterialCost") %>'></asp:Label>
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
                                                        <asp:Label runat="server" ID="UI_TotalAmount" Text='<%# Eval("TotalAmount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="UI_Footer_TotalAmount" Text=""></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>


                                                <asp:BoundField DataField="Status" HeaderText="032_Status" HeaderStyle-Width="9%" ItemStyle-HorizontalAlign="Center" Visible="false"></asp:BoundField>

                                                <asp:TemplateField HeaderText="038_Detail">
                                                    <HeaderStyle Width="8%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="UI_cmdDetail" ImageUrl="images/icon-detail.gif" runat="server" CommandName="cmdDetail" CommandArgument='<%# me.UI_dvRequestDetail.Rows.Count%>' PostBackUrl="~/Client_FlowCase01_Worklist_Item.aspx" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>

                                            <HeaderStyle CssClass="Text_Head" />
                                            <RowStyle CssClass="TR_1" />
                                            <AlternatingRowStyle CssClass="ListRowEven" />
                                            <%--<PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Center" />--%>
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
                            &nbsp;&nbsp;&nbsp;&nbsp;
	    <asp:Button ID="UI_cmdBankTransfer" runat="server" Text="Bank transfer" CssClass="Confirm_l" OnClientClick=" onProgress('Save');" />
                        </td>
                    </tr>

                    <tr>
                        <td colspan="2" align="center">
                            <br />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                 <div style="visibility: hidden;">
                     <asp:Button ID="UI_client_cmdPaypal" runat="server" Text="Paypal" CssClass="Confirm_l" OnClientClick=" return confirm('Please be aware that if you refund, credit back or cancel your credit card payment transaction, CipherLab will charge you a 20% processing and administrative fee which will be applied directly to your account.'); onProgress('Save');" />
                 </div>
                        </td>
                    </tr>

                    <tr>
                        <td colspan="2" align="center">
                            <br />
                            <asp:Button ID="UI_cmdPreview" runat="server" Text="_Preview" CssClass="Confirm_l" CausesValidation="false" Visible="false" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="UI_cmdConfirm" runat="server" Text="_Confirm" CssClass="Confirm_l" OnClientClick="onProgress('Save')" />
                            <asp:Button ID="UI_cmdPaypal" runat="server" Text="Paypal" CssClass="Confirm_l" OnClientClick=" return confirm('Please be aware that if you refund, credit back or cancel your credit card payment transaction, CipherLab will charge you a 20% processing and administrative fee which will be applied directly to your account.'); onProgress('Save');" />
                        </td>
                    </tr>
                </asp:Panel>
                <asp:Label runat="server" ID="UI_Total_ServiceCharge" Text="0" Visible="false"></asp:Label>
                <asp:Label runat="server" ID="UI_Total_MaterialCost" Text="0" Visible="false"></asp:Label>
                <asp:Label runat="server" ID="UI_Total_TotalAmount" Text="0" Visible="false"></asp:Label>
                <asp:Label ID="UI_lblPreviousPage_RMANO" runat="server" Visible="false"></asp:Label>
                <asp:Label ID="UI_lblPreviousPage_RMAID" runat="server" Visible="false"></asp:Label>
                <asp:Label ID="UI_lblPreviousPage_RMADID" runat="server" Visible="false"></asp:Label>
                <asp:Label ID="UI_lblRMAD_STATUS" runat="server" Visible="false"></asp:Label>
            </table>
            <uc2:ucMessageLage ID="ucMessageLage" runat="server" />

            <uc2:ucMessage ID="ucMessage" runat="server" />

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ucMessage" />
            <asp:PostBackTrigger ControlID="UI_cmdSearch" />
            <asp:PostBackTrigger ControlID="UI_dvCustomer" />
            <asp:PostBackTrigger ControlID="UI_cmdPreview" />
            <asp:PostBackTrigger ControlID="UI_cmdConfirm" />
        </Triggers>
    </asp:UpdatePanel>
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
</asp:Content>





