<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Client_FlowCase01_Worklist.aspx.vb" Inherits="Client_FlowCase01_Worklist" Title="Untitled Page" %>

<%@ Register Assembly="CrystalDecisions.Web" Namespace="CrystalDecisions.Reporting.WebControls" TagPrefix="cc1" %>

<%@ Register Src="ascx/ucProgressStatus.ascx" TagName="ucProgressStatus" TagPrefix="uc1" %>
<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>
<%@ Register Src="ascx/ucMessage_UI_cmdBankTransfer.ascx" TagName="ucMessage_UI_cmdBankTransfer" TagPrefix="uc2" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <style type="text/css">
        .Details_Btn {
            background: #fff;
            color: #5E7FF4;
            font-size: 15px;
            border: 0px solid #F1F3FF;
        }

        .erma-information-view .erma-tabs .erma-tab-wait.active a {
            color: #496FF2;
        }

        .erma-information-view .erma-tabs .erma-tab-wait.active::after {
            content: "";
            width: 100%;
            height: 2px;
            background: #496FF2;
            display: block;
            position: absolute;
            right: 0;
            left: 0;
            bottom: -2px;
        }

        .Modal {
            background-color: black;
            opacity: 0.5;
        }

        .erma-table-components-byWait .erma-table-header th {
            background: #fff;
        }

        .erma-table-components-RequestDetail .erma-table-header th, .erma-table-components-RequestDetail td {
            width: auto;
            min-width: 40px;
        }

        .erma-table-components-RequestDetail th, .erma-table-components-RequestDetail td {
            text-align: start;
        }

        .WaitingforProcessing_20240220 {
            width: 150vh;
            height: 85vh;
            border: none;
            border-radius: 10px;
        }

        .UI_totalLabel {
            margin-right: 5px;
        }

        @media screen and (max-width: 1366px) {

            .WaitingforProcessing_20240220 {
                width: 167vh;
                height: 92vh;
            }
        }
    </style>

    <script type="text/javascript">			   
        function Open_Client_FlowCase01_Worklist() {
            $find("ajModalProgress_mpe").show();
            $find("UI_Up_RMA_panel_ModalPopupExtender").hide();
            location.reload();
        }

        function Close_windows() {
            //所有視窗程式 關閉
            $find("mpe").hide();
            $find("ajModalProgress_mpe").hide();
            $find("UI_Up_RMA_panel_ModalPopupExtender").hide();
            $find("Flympe").hide();
            return false;
        }

        function Reload_windows() {
            $find("mpe").hide();
            $find("ajModalProgress_mpe").hide();
            $find("UI_Up_RMA_panel_ModalPopupExtender").hide();
            $find("Flympe").hide();
            document.URL = location.href;
            //location.reload();
        }

        function Show_Lab() {
            if ($(window).height() <= 768) { $('#ProductInformation_Lab').animate({ height: $(window).height() - 30 }); }
        }


        function setCookie(cname, cvalue, exdays) {
            var d = new Date();
            d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
            var expires = "expires=" + d.toGMTString();
            document.cookie = cname + "=" + cvalue + "; " + expires;
        }
        setCookie("windowhigh", window.innerHeight, 30);
        setCookie("windowWidth", window.innerWidth, 30);
        //RMA 前端日曆選項故障 by buck modify 20250925 begin
        var datePickerControls = [
            { textBoxId: "<%= txtStart.ClientID %>", hiddenId: "<%= hfStartDate.ClientID %>" },
                { textBoxId: "<%= txtEnd.ClientID %>", hiddenId: "<%= hfEndDate.ClientID %>" }
        ];
        //RMA 前端日曆選項故障 by buck modify 20250925 end
    </script>

    <uc1:ucProgressStatus ID="ucProgressStatus" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="UI_lblRequestedTittle" runat="server" Text="034_Requested Information" Font-Bold="true" Visible="false" />
            <asp:Label ID="UI_lblQuickTittle" runat="server" Text="171_Please click check boxes and click 'confirm' button to start repairing." Visible="false" />

            <div class='erma-infomation-text'>
                <div class="erma-infomation-block">
                    <img src="../images_new/info.svg" />
                    <asp:Label ID="UI_lblTittle" runat="server" Text="067_Wait for Processing" Visible="false" />
                    <asp:Label ID="UI_lblRequestInformation" CssClass="erma-information-title" runat="server" Text="028_Search for Request Items Information" />
                </div>

                <div class="erma-date-block">
                    <div class="erma-calendar-component">
                        <asp:Label ID="UI_lblRequestDate" runat="server" Text="033_Request Date" />
                        : 	
						<div class="erma-calendar-input">
                            <div class="erma-input-inputStyle">
                                <%--'RMA 前端日曆選項故障 by buck modify 20250925 begin--%>
                                <asp:TextBox ID="txtStart" runat="server" ReadOnly="true" />
                                <asp:HiddenField ID="hfStartDate" runat="server" />
                                <%--<input id="txtStart" runat="server" onclick="calendar.show({ id: this })" readonly="true" type="text" />--%>
                                ~ 
                                <asp:TextBox ID="txtEnd" runat="server" ReadOnly="true" />
                                <asp:HiddenField ID="hfEndDate" runat="server" />
								<%--<input id="txtEnd" runat="server" onclick="calendar.show({ id: this })" readonly="true" type="text" />--%>
                                 <%--'RMA 前端日曆選項故障 by buck modify 20250925 end--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="erma-infomation-resultView">
                <div class="erma-infomation-number-area">
                    <div class="erma-infomation-options">
                        <asp:DropDownList ID="UI_cboStatus" runat="server" class="erma-combobox-choose"></asp:DropDownList>
                    </div>
                    <asp:TextBox ID="UI_txtRMANo" runat="server" class="erma-input-input" placeholder="Please enter Any Search" />
                    <div class="erma-number-buttons">
                        <asp:Button ID="UI_cmdSearch" runat="server" Text="_Search" class="erma-search-button" />
                        <asp:Button ID="ResetBtn" runat="server" Text="Reset" CssClass="erma-search-button erma-reset-button" />
                    </div>
                    <asp:Label ID="UI_lblStatus" runat="server" Text="032_Status" Visible="false" />
                    <asp:Label ID="UI_lblRMANo" runat="server" Text="029_RMA No." Visible="false" />
                </div>
                <div class="erma-information-block">
                    <div class="erma-left-block">
                        <asp:Label ID="lblInformation" runat="server" Text="" CssClass="erma-infomation-text"></asp:Label>
                        <div class="erma-infomation-number">
                            <asp:Label ID="Count_Lab_" runat="server" Text="" />
                            Items
                        </div>
                    </div>
                </div>
                <!--[Begin]資料列表表單-->
                <asp:GridView ID="UI_dvCustomer" runat="server" GridLines="None" CellPadding="1" class="erma-table-components erma-table-components-byWait" AutoGenerateColumns="False" AllowSorting="true">
                    <HeaderStyle CssClass="erma-table-header" />
                    <RowStyle CssClass="erma-table-list" />
                    <Columns>
                        <asp:TemplateField Visible="false">
                            <HeaderStyle CssClass="erma-table-header" />
                            <ItemStyle CssClass="erma-table-list" />
                            <HeaderTemplate>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="UI_SeqNo" runat="server" Text='<%# Eval("SeqNo") %>' />
                                <asp:Label ID="UI_RMANO" runat="server" Text='<%# Eval("RMA_NO") %>' Visible="false" />
                                <asp:Label ID="UI_RMAID" runat="server" Text='<%# Eval("RMA_ID") %>' Visible="false" />
                                <asp:Label ID="UI_RMASTATUS" runat="server" Text='<%# Eval("RMAD_STATUS") %>' Visible="false" />
                                <asp:Label ID="UI_RMADID" runat="server" Text='<%# Eval("RMAD_ID") %>' Visible="false" />
                                <asp:Label ID="UI_RMASQID" runat="server" Text='<%# Eval("RMASQ_ID") %>' Visible="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="RMA_NO" HeaderText="029_RMA No" SortExpression="RMA_NO" ItemStyle-CssClass="erma-RMA-NO" />
                        <asp:BoundField DataField="RequestDate" HeaderText="033_Request Date" SortExpression="RequestDate" ItemStyle-CssClass="erma-RequestDate" />
                        <asp:BoundField DataField="Applicant" HeaderText="006_Applicant" HtmlEncode="False" SortExpression="Applicant" ItemStyle-CssClass="erma-Applicant" />
                        <asp:TemplateField HeaderText="127_Total Amount" SortExpression="QUOTE" ItemStyle-CssClass="erma-Quote">
                            <ItemTemplate>
                                <asp:Label ID="UI_CurrencyCode" runat="server" Text='<%# Eval("CurrencyCode") %>' Visible="false" />&nbsp;
								<asp:Label ID="UI_Quote" runat="server" Text='<%# Eval("QUOTE") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Net Charge" SortExpression="Net_Charge_QUOTE" ItemStyle-CssClass="erma-Net-Charge">
                            <ItemTemplate>
                                <asp:Label ID="Net_Charge_QUOTE_UI_CurrencyCode" runat="server" Text='<%# Eval("CurrencyCode") %>' Visible="false" />&nbsp;
								<asp:Label ID="Net_Charge_QUOTE_UI_Quote" runat="server" Text='<%# Eval("Net_Charge_QUOTE") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="RequestQty" HeaderText="213_Request Qty" SortExpression="RequestQty" ItemStyle-CssClass="erma-RequestQty" />
                        <asp:BoundField DataField="ShippedQty" HeaderText="214_Shipped Qty" Visible="false" SortExpression="ShippedQty" ItemStyle-CssClass="erma-ShippedQty" />
                        <asp:BoundField DataField="Remark" HeaderText="134_Remark" SortExpression="Remark" Visible="false" />
                        <asp:TemplateField HeaderText="032_Status" SortExpression="RMAD_STATUS" ItemStyle-CssClass="erma-Status">
                            <ItemTemplate>
                                <asp:Label ID="UI_Status" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-CssClass="erma-Content">

                            <ItemTemplate>

                                <asp:Button ID="UI_imgDetail" runat="server" Text="Quotation" CommandName="cmdDetail" CommandArgument='<%#Me.UI_dvCustomer.Rows.Count%>' OnClientClick="ShowProgressBar();" Visible="false" />

                                <asp:Button ID="UI_cmdEdit" runat="server" Text="_Edit" CommandName="cmdEdit" CommandArgument='<%#Me.UI_dvCustomer.Rows.Count%>' Visible="false" />

                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Label runat="server" ID="UI_NotFound" Text='<%# Session("_isShowNotFound").ToString()%>' />
                    </EmptyDataTemplate>
                    <AlternatingRowStyle />
                </asp:GridView>
                <!--[End]資料列表表單-->

                <asp:Panel ID="Panel1" runat="server" Visible="false">
                    <div class="erma-calendar-component">
                        <span><%--<asp:Label ID="UI_lblRequestDate" runat="server" Text="033_Request Date"></asp:Label> --%>:</span>
                        <div class="erma-calendar-input">
                            <div class="erma-input-inputStyle">
                                <asp:DropDownList ID="UI_cboBYear" runat="server" CssClass="select" />
                                <asp:DropDownList ID="UI_cboBMonth" runat="server" CssClass="select" />
                                <asp:DropDownList ID="UI_cboBDay" runat="server" CssClass="select" />&nbsp;~&nbsp;
								<asp:DropDownList ID="UI_cboEYear" runat="server" CssClass="select" />
                                <asp:DropDownList ID="UI_cboEMonth" runat="server" CssClass="select" />
                                <asp:DropDownList ID="UI_cboEDay" runat="server" CssClass="select" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <asp:Label runat="server" ID="UI_Total_TotalAmount" Text="0" Visible="false"></asp:Label>
                <asp:Label ID="UI_lblPreviousPage_RMANO" runat="server" Visible="false" />
                <asp:Label ID="UI_lblPreviousPage_RMAID" runat="server" Visible="false" />
                <asp:Label ID="UI_lblPreviousPage_RMADID" runat="server" Visible="false" />
                <asp:Label ID="UI_lblRMAD_STATUS" runat="server" Visible="false" />

                <asp:Panel runat="server" ID="UI_panSearch" DefaultButton="UI_cmdSearch">
                </asp:Panel>

                <asp:Panel runat="server" ID="UIPanel_RMADetail" Visible="false">
                </asp:Panel>

                <!--這邊顯示要跳出框 開始-->
                <!--客戶確認RMA 開始-->
                <!-- 新增前端客戶選擇Reject折讓服務費(跑版處理) by buck Add 20260427 begin -->
                <asp:Panel ID="UI_panel" runat="server" Style="display: none; position: absolute; top: 10px; width: 80%; height: 75%; border: 1px solid black;">
                    <div style="background: #fff; border: 1px solid black; border-radius: 10px; padding: 15px; width: 100%; height: 100%; box-sizing: border-box; display: flex; flex-direction: column;">
                        <div class="erma-quotation-box" style="display: flex; flex-direction: column; flex: 1; overflow: hidden;">
                            <!-- 新增前端客戶選擇Reject折讓服務費(跑版處理) by buck Add 20260427 end -->
                            <div class="erma-box-div">
                                <asp:Label ID="QuotationLabel" runat="server" Text=""></asp:Label>
                                <div class="erma-image-cancel">
                                    <asp:ImageButton ID="Deatail_Cancel_Btn" runat="server" ImageUrl="../images/cancel.svg" />
                                </div>
                            </div>
                            <div class="erma-quotation-topInfo">
                                <table>
                                    <tr>
                                        <td>
                                            <div>
                                                <asp:Label ID="UI_lblRMANo_Detail" runat="server" Text="029_RMA No." />&nbsp;:&nbsp;											
                                            </div>
                                        </td>
                                        <td>
                                            <asp:Label ID="UI_RMANo" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <asp:Label ID="UI_lblRequestDate_Detail" runat="server" Text="033_Request Date" />&nbsp;:&nbsp;
                                            </div>
                                        </td>
                                        <td>
                                            <asp:Label ID="UI_RequestDate" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <asp:Label ID="UI_lblRepairCenter" runat="server" Text="009_Repair Center" />&nbsp;:&nbsp;
                                            </div>
                                        </td>
                                        <td>
                                            <asp:Label ID="UI_RepairCenter" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <asp:Label ID="UI_lblApplicant" runat="server" Text="006_Applicant" />&nbsp;:&nbsp;
                                            </div>
                                        </td>
                                        <td>
                                            <asp:Label ID="UI_Applicant" runat="server" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <asp:Label ID="UI_lblRequestedTittle_Detail" runat="server" Text="Requested Information Detail" Visible="false" />
                                            <asp:Label ID="Quotation_titile_Label" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>

                                </table>
                            </div>
                            <!-- 新增前端客戶選擇Reject折讓服務費(跑版處理) by buck Add 20260427 begin -->
                            <div class="erma-warrantyRepair-content" style="padding: 5px; flex-grow: 1; overflow-y: auto; min-height: 0;">
                                <!--[Begin]資料列表表單-->
                                <!-- PagerSettings-Mode="Numeric" -->
                                <asp:GridView ID="UI_dvRequestDetail" runat="server" AutoGenerateColumns="False" class="erma-table-components erma-table-components-RequestDetail" GridLines="None" Width="100%" Height="360px">                                    
                                    <HeaderStyle CssClass="erma-table-header" />
                                    <Columns>
                                        <%--勾選開始 --%>
                                        <asp:TemplateField>
                                            <HeaderStyle CssClass="erma-Content-Accept" HorizontalAlign="Center" />
                                            <ItemStyle CssClass="erma-Content-Accept" HorizontalAlign="Center" />
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="UI_CheckGroup_Accept" runat="server" AutoPostBack="true" OnCheckedChanged="UI_checkGroup_Accept_CheckedChanged" />
                                                <asp:Label ID="UI_lblAccept" runat="server" Text="Accept" />
                                            </HeaderTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="80" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="UI_Check_Accept" runat="server" AutoPostBack="true" Font-Size="Small" ForeColor="Transparent" OnCheckedChanged="UI_check_Accept_CheckedChanged" Text='<%# Eval("RMAD_SERIALNO") %>' />
                                                <asp:Label ID="UI_Accept" runat="server" Text="V" Visible="false" />
                                                <asp:Label ID="UI_SeqID" runat="server" Text='<%# Eval("SeqID") %>' Visible="false" />
                                                <asp:Label ID="UI_RMADWARRANTY" runat="server" Text='<%# Eval("RMAD_WARRANTY") %>' Visible="false" />
                                                <asp:Label ID="UI_RMADCSTMP" runat="server" Text='<%# Eval("RMAD_CSTMP") %>' Visible="false" />
                                                <asp:Label ID="UI_RMANO" runat="server" Text='<%# Eval("RMAD_RMANO") %>' Visible="false" />
                                                <asp:Label ID="UI_RMADID" runat="server" Text='<%# Eval("RMAD_ID") %>' Visible="false" />
                                                <asp:Label ID="UI_RMAD_STATUS" runat="server" Text='<%# Eval("RMAD_STATUS") %>' Visible="false" />
                                                <asp:Label ID="UI_SERIALNO" runat="server" Text='<%# Eval("RMAD_SERIALNO") %>' Visible="false" />
                                                <asp:Label ID="UI_PARTSN" runat="server" Text='<%# Eval("RMAD_PARTSN") %>' Visible="false" />
                                                <asp:Label ID="UI_RMAD_ISWARRANTY" runat="server" Text='<%# Eval("RMAD_ISWARRANTY") %>' Visible="false" />
                                                <asp:Label ID="UI_RMARQ_ID" runat="server" Text='<%# Eval("RMARQ_ID") %>' Visible="false" />
                                                <asp:Label ID="UI_RMARQ_IMPROPERUSAGE" runat="server" Text='<%# Eval("RMARQ_IMPROPERUSAGE") %>' Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle CssClass="erma-Content-Accept" HorizontalAlign="Center" />
                                            <ItemStyle CssClass="erma-Content-Accept" HorizontalAlign="Right" />
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="UI_CheckGroup_Reject" runat="server" AutoPostBack="true" OnCheckedChanged="UI_checkGroup_Reject_CheckedChanged" />
                                                <asp:Label ID="UI_lblReject" runat="server" Text="Reject" />
                                            </HeaderTemplate>
                                            <ItemStyle HorizontalAlign="Right" Width="80" />
                                            <ItemTemplate>
                                                <asp:Label ID="UI_Reject" runat="server" Text="V" Visible="false" />
                                                <asp:CheckBox ID="UI_Check_Reject" runat="server" AutoPostBack="true" Font-Size="Small" ForeColor="Transparent" OnCheckedChanged="UI_check_Reject_CheckedChanged" Text='<%# Eval("RMAD_SERIALNO") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--勾選結束--%>
                                        <asp:BoundField DataField="RMAD_SERIALNO" HeaderText="013_Serial Number" />
                                        <asp:TemplateField HeaderText="Clinet product No">
                                            <ItemStyle />
                                            <ItemTemplate>
                                                <asp:Label ID="UI_QueryCUSTOMER_PRODUCT_NUMBER" runat="server" Text='<%# QueryCUSTOMER_PRODUCT_NUMBER(Eval("RMAD_ID")) %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="RMAD_MODELNO" HeaderText="035_Model" />
                                        <asp:BoundField DataField="WARRANTY" DataFormatString="{0:yyyy/MM/dd}" HeaderText="015_Warranty" HtmlEncode="false" />
                                        <asp:BoundField DataField="IMPROPERUSAGE" HeaderText="064_Improper Usage" />
                                        <asp:BoundField DataField="FailureReason" DataFormatString="{0:yyyy/MM/dd}" HeaderText="023_Failure Reason" HtmlEncode="false" />
                                        <asp:TemplateField HeaderText="125_Labor Cost">
                                            <HeaderStyle />
                                            <ItemStyle />
                                            <ItemTemplate>
                                                <!-- 新增前端客戶選擇Reject折讓服務費 by buck Add 20260427 begin -->
                                                <asp:Label ID="UI_ServiceCharge" runat="server" Text='<%# Eval("LaborCost") %>' />
                                                <asp:Label ID="UI_ServiceCharge_Reject" runat="server" Font-Strikeout="true" ForeColor="Red" Text='<%# Eval("LaborCost") %>' Visible="false" />
                                                <asp:Label ID="UI_ServiceCharge_Cancel" runat="server" ForeColor="Black" Text="0" Visible="false" />
                                                <!-- 新增前端客戶選擇Reject折讓服務費 by buck Add 20260427 end -->
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="126_Material Cost">
                                            <HeaderStyle />
                                            <ItemStyle />
                                            <ItemTemplate>
                                                <asp:Label ID="UI_MaterialCost_Agree" runat="server" Text='<%# Eval("MaterialCost") %>' />
                                                <asp:Label ID="UI_MaterialCost" runat="server" Font-Strikeout="true" ForeColor="Red" Text='<%# Eval("MaterialCost") %>' Visible="false" />
                                                <asp:Label ID="UI_MaterialCost_Cancel" runat="server" ForeColor="Black" Text="0" Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="127_Total Amount">
                                            <HeaderStyle />
                                            <ItemStyle />
                                            <ItemTemplate>
                                                <asp:Label ID="UI_TotalAmount_Agree" runat="server" Text='<%# Eval("TotalAmount") %>' />
                                                <asp:Label ID="UI_TotalAmount" runat="server" Font-Strikeout="true" ForeColor="Red" Text='<%# Eval("TotalAmount") %>' Visible="false" />
                                                <asp:Label ID="UI_TotalAmount_Cancel" runat="server" ForeColor="Black" Text='<%# Eval("LaborCost") %>' Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Status" HeaderStyle-CssClass="RMAD_RMANO_001" HeaderText="032_Status" Visible="false" />
                                        <asp:TemplateField>
                                            <HeaderStyle />
                                            <ItemStyle />
                                            <ItemTemplate>
                                                <asp:Button ID="UI_cmdDetail" runat="server" CommandArgument="<%#Me.UI_dvRequestDetail.Rows.Count%>" CommandName="cmdDetail" CssClass="Details_Btn" Text="View" />
                                                <asp:Button ID="UI_Test_Report" runat="server" CommandArgument="<%#Me.UI_dvRequestDetail.Rows.Count%>" CommandName="Test_Report" CssClass="Details_Btn" Text="Test Report" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <!--[End]資料列表表單-->
                                <div class="erma-warrantyRepair-calc-content">
                                    <!--總數-->
                                    <div class="erma-calc-number">
                                        <img src="images/info-black.svg" />
                                        <asp:Label ID="ServiceChargeLabel" runat="server" Text=""></asp:Label>
                                        <asp:Label ID="UI_Total_ServiceCharge" runat="server" CssClass="UI_totalLabel" Text="0"></asp:Label>
                                        <asp:Label ID="MaterialLabel" runat="server" Text=""></asp:Label>
                                        <asp:Label ID="UI_Total_MaterialCost" runat="server" CssClass="UI_totalLabel" Text="0"></asp:Label>
                                        <asp:Label ID="TotalAmount" runat="server" CssClass="UI_totalLabel" Text=""></asp:Label>
                                    </div>
                                </div>
                                <div class="erma-buttons-div">
                                    <asp:Button ID="UI_client_cmdPaypal" runat="server" CssClass="erma-button-next" Height="40px" Text="Client Paypal" Width="90px" />
                                    <asp:Button ID="UI_cmdBankTransfer" runat="server" CssClass="erma-button-cancel" Height="40px" OnClientClick=" onProgress('Save');" Text="Bank transfer" Width="100px" />
                                    <asp:Button ID="UI_cmdPaypal" runat="server" CssClass="erma-button-cancel" Height="40px" OnClientClick="" Text="Paypal" Width="90px" />
                                    <asp:Button ID="UI_cmdPreview" runat="server" CausesValidation="false" CssClass="erma-button-next" Height="40px" Text="_Preview" Visible="false" Width="90px" />
                                    <asp:Button ID="UI_cmdCancel" runat="server" CssClass="erma-button-cancel" Height="40px" Text="008_Cancel" Width="90px" />
                                    <asp:Button ID="UI_cmdConfirm" runat="server" CssClass="erma-button-next" Height="40px" OnClientClick="onProgress('Save')" Text="_Confirm" Width="90px" />
                                </div>
                            </div>
                        </div>
                    </div>


                    <!-- Pay提示訊息 STRAT -->
                    <asp:Panel ID="panel_Message_Paypal" runat="server" Style="display: none; position: absolute;">
                        <style>
                            .Modal {
                                background-color: black;
                                opacity: 0.5;
                            }

                            .message-div-class center {
                                padding: 7px;
                            }

                                .message-div-class center .AlertBtn {
                                    padding: 9px 18px;
                                    min-width: 90px;
                                }

                                    .message-div-class center .AlertBtn:hover {
                                        background: #191919;
                                        color: #fff;
                                    }
                        </style>
                        <table style="background: #fff; border-radius: 10px; width: 400px; height: 350px; border: none; padding: 10px;">
                            <tr>
                                <td>
                                    <div class="message-div-class">
                                        <center>
                                            <img class="message-div-image" src="../images_new/notice.svg" />
                                            <br />

                                            <p style="font-size: 15px; font-weight: bold; display: none;">
                                                <asp:Label ID="lblTitleMsg" runat="server" Text="訊息提示"></asp:Label>
                                            </p>
                                        </center>
                                        <center class="html_box">
                                            <asp:Label ID="html_Success" CssClass="text12pt" runat="server" ForeColor="#03a9f4" Text="" Font-Bold="true" Font-Names="Arial"></asp:Label>
                                            <asp:Label ID="html_Failed" CssClass="text12pt" runat="server" ForeColor="" Text="" Font-Names="Arial"></asp:Label>

                                        </center>
                                        <center class="UI_button">
                                            <asp:Button ID="UI_butClose" runat="server" Text="008_Close" CssClass="AlertBtn" CausesValidation="false" />
                                            <asp:Button ID="UI_butAlert" runat="server" Text="015_Confirm" CssClass="AlertBtn" CausesValidation="false" OnClientClick=" onProgress('Save');" />
                                        </center>
                                        <br />

                                        <center>
                                            <asp:Button ID="alarmButton" CssClass="AlertBtn" runat="server" Style="display: none"></asp:Button>
                                        </center>
                                    </div>
                                </td>
                            </tr>
                        </table>

                    </asp:Panel>
                    <!-- Pay提示訊息 END -->

                    <ajaxToolkit:ModalPopupExtender
                        ID="ModalMessage"
                        TargetControlID="UI_cmdPaypal"
                        PopupControlID="panel_Message_Paypal"
                        BackgroundCssClass="Modal"
                        runat="server"
                        DropShadow="false">
                    </ajaxToolkit:ModalPopupExtender>

                </asp:Panel>

                <div style="visibility: hidden">
                    <asp:Button ID="Deatail_Btn" runat="server" Text="" />
                </div>

                <ajaxToolkit:ModalPopupExtender ID="ajModalProgress" TargetControlID="Deatail_Btn" BehaviorID="ajModalProgress_mpe" PopupControlID="UI_panel" CancelControlID="UI_cmdCancel" BackgroundCssClass="Model" runat="server">
                </ajaxToolkit:ModalPopupExtender>

                <!--客戶確認RMA 結束-->

                <%--修改RMA 開始--%>
                <div style="visibility: hidden;">
                    <asp:Button ID="UI_Up_RMA_Btn" runat="server" Text="" />
                </div>
                <asp:Panel ID="UI_Up_RMA_panel" runat="server" Style="display: none; position: absolute;">

                    <asp:Button ID="UI_Up_cmdCancel" runat="server" Text="X" Font-Size="0" CssClass="Cancel" />
                    <asp:Label ID="windowLab" runat="server" Text=""></asp:Label>

                </asp:Panel>
                <ajaxToolkit:ModalPopupExtender
                    ID="UI_Up_RMA_panel_ModalPopupExtender"
                    TargetControlID="UI_Up_RMA_Btn"
                    BehaviorID="UI_Up_RMA_panel_ModalPopupExtender"
                    PopupControlID="UI_Up_RMA_panel"
                    CancelControlID="UI_Up_cmdCancel"
                    BackgroundCssClass="Modal"
                    runat="server">
                </ajaxToolkit:ModalPopupExtender>
                <%--修改RMA 結束--%>

                <!--這邊顯示要跳出框 結束-->
                <uc2:ucMessage ID="ucMessage" runat="server" />
                <uc2:ucMessage_UI_cmdBankTransfer ID="ucMessage1" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ucMessage" />
            <asp:PostBackTrigger ControlID="UI_cmdSearch" />
            <asp:PostBackTrigger ControlID="UI_dvCustomer" />
            <asp:PostBackTrigger ControlID="UI_cmdPreview" />
            <asp:PostBackTrigger ControlID="UI_cmdConfirm" />
        </Triggers>
    </asp:UpdatePanel>
    <%--<CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true"/>--%>
</asp:Content>
