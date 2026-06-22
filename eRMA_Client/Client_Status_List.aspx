<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Client_Status_List.aspx.vb" Inherits="Client_Status_List" %>

<%@ Register Assembly="CrystalDecisions.Web" Namespace="CrystalDecisions.Reporting.WebControls" TagPrefix="cc1" %>
<%@ Register Src="ascx/ucProgressStatus.ascx" TagName="ucProgressStatus" TagPrefix="uc1" %>
<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <style>
        .RequestedProcessedLabel {
            background: #0c66f5;
            border-radius: 25px;
            width: 120px;
            height: 25px;
            color: #fff;
            padding: 3px;
        }

        .erma-image-cancel-001 {
            width: 32px;
            padding: 5px;
            position: absolute;
            display: table;
            right: 5px;
            top: 5px;
            cursor: pointer;
            background: transparent;
            border: none;
        }
    </style>
    <style>
        .erma-information-view .erma-tabs .erma-tab-query.active {
            color: #496FF2;
        }

            .erma-information-view .erma-tabs .erma-tab-query.active::before {
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

        .erma-content .UI_cmdDetail {
            border: none;
            background: none;
            color: #1C218C;
            text-decoration: underline;
        }

        .btn_clean {
            border: 0px solid transparent;
            background: transparent;
            color: #353535;
            font-weight: bold;
            font-family: "Arial", "Helvetica", 微軟正黑體;
        }
    </style>

    <script type="text/javascript">

        function ShowInfoPage(urles) {
            window.open(urles);
        }

        function Close_Client_Status_List() {
            $find("Alert_mpe").hide();
            $find("UI_Up_RMA_panel_ModalPopupExtender").hide();

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

    <asp:Panel ID="Panel1" runat="server" Visible="false">
        <uc1:ucProgressStatus ID="ucProgressStatus" runat="server" />
    </asp:Panel>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="erma-information-view erma-information-status-view" style="width: 100%;">
                <!-- tabs -->

                <div class="erma-infomation-area2">
                    <div class="erma-infomation-text">
                        <div class="erma-infomation-block">
                            <img src="../images/info.svg" />
                            <asp:Label ID="UI_lblRequestInformation" CssClass="erma-information-title" runat="server"
                                Text="028_Search for Request Items Information"></asp:Label>
                        </div>
                        <div class="erma-date-block">
                            <div class="erma-calendar-component">
                                <asp:Label ID="UI_lblRequestDate" runat="server"
                                    Text="033_Request Date" />
                                :
                                                   
                                <div class="erma-calendar-input">
                                    <div class="erma-input-inputStyle">
                                        <%--'RMA 前端日曆選項故障 by buck modify 20250925 begin--%>
                                        <asp:TextBox ID="txtStart" runat="server" ReadOnly="true" />
                                        <asp:HiddenField ID="hfStartDate" runat="server" />
                                        <%--<input id="txtStart" runat="server" onclick="calendar.show({ id: this })" type="text" readonly="true" style="border: none;" />--%>
                                        ~
                                        <asp:TextBox ID="txtEnd" runat="server" ReadOnly="true" />
                                        <asp:HiddenField ID="hfEndDate" runat="server" />
                                        <%--<input id="txtEnd" runat="server" onclick="calendar.show({ id: this })" type="text" readonly="true" style="border: none;" />--%>
                                        <%--'RMA 前端日曆選項故障 by buck modify 20250925 end--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>


                    <div class="erma-infomation-number-area">

                        <div class="erma-infomation-options">
                            <asp:DropDownList ID="UI_cboStatus_Change" runat="server" class="erma-combobox-choose"></asp:DropDownList>
                        </div>

                        <asp:TextBox ID="UI_txtRMANo" runat="server" CssClass="erma-input-input"
                            placeholder="Please enter RMA Number"></asp:TextBox>
                        <div class="erma-number-buttons">
                            <asp:Button ID="UI_cmdSearch" runat="server" Text="_Search" CssClass="erma-search-button" OnClientClick="ShowProgressBar();" />
                            <asp:Button ID="ResetBtn" runat="server" Text="Reset" CssClass="erma-search-button erma-reset-button" OnClientClick="location.reload();" />
                        </div>
                    </div>
                    <div class="erma-infomation-resultView erma-information">
                        <div class="erma-information-block">
                            <div class="erma-left-block">
                                <asp:Label ID="lblInformation" runat="server" Text="" CssClass="erma-infomation-text"></asp:Label>

                                <div class="erma-infomation-number">
                                    <asp:Label ID="count_Lab" runat="server" Text=""></asp:Label>
                                    <span>Items</span>
                                </div>
                            </div>
                            <div class="erma-right-block">
                                <div class="erma-status-block">
                                    <div class="erma-combobox-control">
                                        <asp:Label ID="Status_Lab" runat="server" Text=""></asp:Label>&nbsp;:&nbsp;
                                                           
                                        <asp:DropDownList ID="UI_cboStatus" runat="server"
                                            CssClass="erma-combobox-choose">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="erma-changePage-component" style="float: right;">

                                    <ul>


                                        <li>
                                            <asp:ImageButton ID="first_ImageBtn" runat="server"
                                                ImageUrl="~/images_new/first.svg"
                                                CssClass="imgbutton" />
                                        </li>
                                        <li>
                                            <asp:ImageButton ID="previous_ImageBtn" runat="server"
                                                ImageUrl="~/images_new/previous.svg"
                                                CssClass="imgbutton" />
                                        </li>
                                        <li>
                                            <div class="erma-pages-block">

                                                <asp:TextBox ID="Current_Page_Label" runat="server"
                                                    AutoPostBack="true"></asp:TextBox>

                                                <span class="erma-pages-mark">/</span>
                                                <asp:Label ID="Total_Page_Label" runat="server"
                                                    Text=""></asp:Label>
                                            </div>
                                        </li>
                                        <li>
                                            <asp:ImageButton ID="next_ImageBtn" runat="server"
                                                ImageUrl="~/images_new/next.svg"
                                                CssClass="imgbutton" />
                                        </li>
                                        <li>
                                            <asp:ImageButton ID="last_ImageBtn" runat="server"
                                                ImageUrl="~/images_new/last.svg"
                                                CssClass="imgbutton" />

                                        </li>
                                    </ul>
                                </div>
                            </div>

                        </div>
                        <%--頁簽--%>
                        <asp:HiddenField ID="BookMark_Label" runat="server" Value="" />
                        <%--頁簽--%>

                        <div class="erma-table-components" cellspacing="0" cellpadding="0">


                            <div class="erma-table-header">
                                <div class="erma-rmaNo">

                                    <div style="float: initial;">
                                        <asp:ImageButton ID="RMA_NoLab_ImageBtn" runat="server" AlternateText="TESTESTE" ImageUrl="../CipherPG/Content/DataTables/images/sort_desc_.png" />
                                    </div>
                                    <div style="float: initial;">
                                        <asp:Button ID="RMA_NoLab" runat="server" Text="Button" CssClass="btn_clean" />
                                    </div>


                                </div>
                                <div class="erma-date">
                                    <div style="float: initial;">
                                        <asp:ImageButton ID="Request_DateLab_ImageBtn" runat="server" ImageUrl="../CipherPG/Content/DataTables/images/sort_desc_.png" />
                                    </div>
                                    <div style="float: initial;">
                                        <asp:Button ID="Request_DateLab" runat="server" Text="Button" CssClass="btn_clean" />
                                    </div>

                                </div>
                                <div class="erma-person">
                                    <div style="float: initial;">
                                        <asp:ImageButton ID="Contact_PersonLab_ImageBtn" runat="server" ImageUrl="../CipherPG/Content/DataTables/images/sort_desc_.png" />
                                    </div>
                                    <div style="float: initial;">
                                        <asp:Button ID="Contact_PersonLab" runat="server" Text="Button" CssClass="btn_clean" />
                                    </div>
                                </div>
                                <div class="erma-original">
                                    <div style="float: initial;">
                                        <asp:ImageButton ID="Original_ChargeLab_ImageBtn" runat="server" ImageUrl="../CipherPG/Content/DataTables/images/sort_desc_.png" Visible="false" />
                                    </div>
                                    <div style="float: initial;">
                                        <asp:Button ID="Original_ChargeLab" runat="server" Text="Button" CssClass="btn_clean" />
                                    </div>
                                </div>
                                <div class="erma-null">
                                    <span></span>
                                </div>
                                <div class="erma-content">

                                    <asp:Button ID="ContentLab" runat="server" Text="Button" CssClass="btn_clean" />

                                </div>
                                <div class="erma-print">
                                    <asp:Button ID="PrintLab" runat="server" Text="Button" CssClass="btn_clean" />

                                </div>
                            </div>


                            <asp:ListView ID="UI_dvRMAListView" runat="server"
                                GroupPlaceholderID="groupPlaceHolder1"
                                ItemPlaceholderID="itemPlaceHolder1"
                                OnPagePropertiesChanging="UI_dvRMAListView_PagePropertiesChanging">
                                <LayoutTemplate>


                                    <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr style="visibility: hidden;">
                                            <td colspan="3">
                                                <asp:DataPager ID="DataPager1" runat="server" PagedControlID="UI_dvRMAListView" PageSize="10">
                                                    <Fields>
                                                        <asp:NextPreviousPagerField ButtonType="Link" ShowFirstPageButton="false" ShowPreviousPageButton="true"
                                                            ShowNextPageButton="false" />
                                                        <asp:NumericPagerField ButtonType="Link" />
                                                        <asp:NextPreviousPagerField ButtonType="Link" ShowNextPageButton="true" ShowLastPageButton="false"
                                                            ShowPreviousPageButton="false" />
                                                    </Fields>
                                                </asp:DataPager>
                                            </td>
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <GroupTemplate>
                                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                                </GroupTemplate>
                                <ItemTemplate>


                                    <div class="erma-table-listCard">
                                        <div class="erma-table-topCard-first">
                                            <div class="erma-first-div erma-rmaNo">
                                                <span>
                                                    <%# Eval("RMA_NO") %>
                                                                            </span>
                                            </div>
                                            <div class="erma-first-div erma-date">
                                                <span>
                                                    <%# Eval("RequestDate") %>
                                                                            </span>
                                            </div>
                                            <div class="erma-first-div erma-person">
                                                <span>
                                                    <%# Eval("Applicant") %>
                                                                            </span>
                                            </div>
                                            <div class="erma-first-div erma-original">
                                                <span>
                                                    <%# Eval("CurrencyCode") %> &nbsp; <%#
                                                                                        Eval("QUOTE") %>
                                                                            </span>
                                            </div>
                                            <div class="erma-first-div erma-null">
                                                <span>&nbsp;</span>
                                            </div>
                                            <div class="erma-first-div erma-content">
                                                <span>
                                                    <asp:Button ID="UI_cmdDetail"
                                                        runat="server"
                                                        CommandName="cmdDetail"
                                                        CssClass="UI_cmdDetail"
                                                        CommandArgument='<%# Eval("RMA_NO") %>'
                                                        Text="Detail" />

                                                    <asp:HiddenField
                                                        ID="UI_RMAD_STATUS_ES_HiddenField"
                                                        runat="server"
                                                        Value='<%#   Eval("RMAD_STATUS")  %>' />

                                                    <asp:HiddenField
                                                        ID="UI_RMAD_STATUS_HiddenField"
                                                        runat="server"
                                                        Value='<%#   Select_RMA_STATUS(Eval("RMA_NO"))  %>' />
                                                    <asp:HiddenField
                                                        ID="UI_Quote_HiddenField"
                                                        runat="server"
                                                        Value='<%# Eval("QUOTE") %>' />
                                                </span>
                                            </div>
                                            <div class="erma-first-div erma-print">
                                                <asp:Button ID="UI_cmdPrintForm"
                                                    runat="server" Text="維修單"
                                                    CssClass="erma-button-btn"
                                                    ForeColor="Black"
                                                    BackColor="White" Style="width: auto"
                                                    CommandName="cmdPrintForm"
                                                    CommandArgument='<%# Eval("RMA_ID") & "|" & Eval("RMA_NO") %>' />
                                                <asp:Button ID="UI_cmdPrintQuotedFRBH"
                                                    runat="server" Text="報價單"
                                                    CssClass="erma-button-btn"
                                                    ForeColor="Black"
                                                    BackColor="White" Style="width: auto"
                                                    CommandName="cmdPrintQuotedFRBH"
                                                    CommandArgument='<%# Eval("RMA_NO") %>' />
                                                <asp:HiddenField ID="UI_Quote" runat="server" Value='<%# Eval("QUOTE") %>' />
                                            </div>
                                        </div>
                                        <div class="erma-table-listCard-second">
                                            <div class="erma-status-blcok">
                                                <div class="erma-other-content">
                                                    <div
                                                        class="erma-button-status status-closed">
                                                        <p>
                                                            <%--狀態--%>



                                                            <asp:Label
                                                                ID="RequestedProcessedLabel"
                                                                runat="server"
                                                                Text="Requested"
                                                                CssClass="status-closed erma-status-buttons erma-status-Requested" BackColor="#008cff">
                                                                                            </asp:Label>


                                                            <asp:Label
                                                                ID="NotProcessedLabel"
                                                                runat="server"
                                                                Text="Requested"
                                                                CssClass="status-closed erma-status-buttons erma-status-Requested">
                                                                                            </asp:Label>
                                                            <asp:Label
                                                                ID="ProcessingLabel"
                                                                runat="server"
                                                                Text="in Progress"
                                                                CssClass="ProcessingLabel erma-status-buttons erma-status-InProgress">
                                                                                            </asp:Label>
                                                            <asp:Label ID="ClosedLabel"
                                                                runat="server"
                                                                Text="Closed"
                                                                CssClass="ClosedLabel erma-status-buttons erma-status-Close">
                                                                                            </asp:Label>
                                                            <asp:Label
                                                                ID="CanceledLabel"
                                                                runat="server"
                                                                Text="Canceled"
                                                                CssClass="CanceledLabel erma-status-buttons erma-status-Cancel">
                                                                                            </asp:Label>

                                                            <%--狀態--%>
                                                        </p>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="erma-qty-blcok">
                                                <div class="erma-RMAQty">
                                                    <span class="erma-key"><%= getoLanguage("RMA","213") %>:</span>
                                                    <span class="erma-value">
                                                        <%# Eval("RequestQty") %>
                                                                                </span>
                                                </div>
                                                <div class="erma-ProcessQty">
                                                    <span class="erma-key"><%= getoLanguage("RMA","215") %>
                                                                                    :</span>
                                                    <span class="erma-value">
                                                        <%# Eval("ProcessingQty") %>
                                                                                </span>
                                                </div>
                                                <div class="erma-ShippedQty">
                                                    <span class="erma-key"><%= getoLanguage("RMA","214") %>
                                                                                    :</span>
                                                    <span class="erma-value">
                                                        <%# Eval("ShippedQty") %>
                                                                                </span>
                                                </div>
                                            </div>
                                            <div class="erma-description-blcok">
                                                <div class="erma-description-content">
                                                    <asp:Label ID="RemarkLab" runat="server"
                                                        Text="" CssClass="erma-input-span1"></asp:Label>
                                                    <span style="padding: 0 2px;">: </span>
                                                    <span class="erma-input-span2">
                                                        <%# Eval("Remark") %>
                                                                                </span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                </ItemTemplate>
                            </asp:ListView>
                        </div>
                    </div>
                </div>
            </div>


            <asp:Label ID="UI_lblPreviousPage_RMAID" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblPreviousPage_RMANO" runat="server" Visible="false"></asp:Label>

            <%--這邊顯示要跳出框 開始--%>

            <%--修改RMA 開始--%>
            <div style="visibility: hidden;">
                <asp:Button ID="UI_Up_RMA_Btn" runat="server" Text="" />
            </div>

            <asp:Panel ID="UI_Up_RMA_panel" runat="server" CssClass="ucSubstitute_div"
                Style="display: none; position: absolute;">

                <div class="erma-image-cancel">
                    <asp:ImageButton ID="UI_Up_cmdCancel" runat="server" ImageUrl="../images/cancel.svg" />
                </div>
                <asp:Label ID="windowLab" runat="server" Text=""></asp:Label>

            </asp:Panel>
            <ajaxToolkit:ModalPopupExtender ID="UI_Up_RMA_panel_ModalPopupExtender"
                TargetControlID="UI_Up_RMA_Btn"
                BehaviorID="UI_Up_RMA_panel_ModalPopupExtender"
                PopupControlID="UI_Up_RMA_panel" CancelControlID="UI_Up_cmdCancel"
                BackgroundCssClass="Modal" runat="server">
            </ajaxToolkit:ModalPopupExtender>
            <%--修改RMA 結束--%>


            <div class="erma-infomation-number-area" style="display: none;">
                <asp:Label ID="UI_lblRMANo" runat="server" Text="029_RMA No.">
                                                </asp:Label>
                <asp:Label ID="UI_lblStatus" runat="server" Text="032_Status">
                                                </asp:Label>
                <asp:Label ID="UI_lblTittle" runat="server" Text="027_Status Query"
                    CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
                <asp:Label ID="UI_lblRequestedTittle" runat="server"
                    Text="034_Requested Information" Font-Bold="true"></asp:Label>
            </div>

            <!--這邊顯示要跳出框 結束-->
            <uc2:ucMessage ID="ucMessage" runat="server" />

            <script src="../js/system.js"></script>

        </ContentTemplate>

    </asp:UpdatePanel>
    <%--<CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />--%>
</asp:Content>
