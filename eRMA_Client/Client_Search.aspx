<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Client_Search.aspx.vb" Inherits="Client_Client_Search" Title="Untitled Page" %>

<%@ Register Src="ascx/ucClientDetail.ascx" TagName="ucClientDetail" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <script type="text/javascript">
        function Open_Client_FlowCase01_Worklist() {
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

    <style>
        .UI_cmdCancel_UI {
            width: 20px;
            padding: 5px;
            position: absolute;
            display: table;
            right: 10px;
            top: 5px;
            cursor: pointer;
            border: none;
            background: transparent;
            color: black;
        }


        .UI_cmdDetail_UI {
            border: none;
            background: none;
            text-decoration: underline;
            color: #1C218C;
        }

        .erma-search-button-001 {
            background: #4F4F4F;
            border: 1px solid #4F4F4F;
            border-radius: 5px;
            color: #fff;
        }
    </style>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="erma-information-view" style="width: 100%;">
                <div class='erma-infomation-text'>
                    <div class="erma-infomation-block">
                        <img src="../images_new/info.svg" />
                        <asp:Label ID="UI_lblTittle" CssClass="erma-information-title" runat="server" Text="027_Status Query"></asp:Label>
                        <asp:Label ID="UI_lblRequestInformation" runat="server" Text="028_Search for Request Items Information" Font-Bold="true" Visible="false"></asp:Label>
                    </div>
                    <div class="erma-date-block">
                        <div class="erma-calendar-component">
                            *<asp:Label ID="UI_lblRequestDate" runat="server" Text="033_Request Date"></asp:Label>
                            <div class="erma-calendar-input">
                                <div class="erma-input-inputStyle">
                                     <%--'RMA 前端日曆選項故障 by buck modify 20250925 begin--%>
                                    <asp:TextBox ID="txtStart" runat="server" ReadOnly="true" />
                                    <asp:HiddenField ID="hfStartDate" runat="server" />
                                    <%--<input id="txtStart" runat="server" onclick="calendar.show({ id: this })" readonly="true" type="text" />--%>                                    
                                    ~ 
                                    <asp:TextBox ID="txtEnd" runat="server" ReadOnly="true" />
                                    <asp:HiddenField ID="hfEndDate" runat="server" />
                                    <%-- <input id="txtEnd" runat="server" onclick="calendar.show({ id: this })" readonly="true" type="text" />--%>
									<%--'RMA 前端日曆選項故障 by buck modify 20250925 end--%>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>


                <table style="background: transparent; border: none; width: 100%;">
                    <tr style="background: transparent; border: none;">

                        <td style="background: transparent; border: none;">
                            <asp:Label ID="UI_lblRMANo" runat="server" Text="029_RMA No."></asp:Label>
                        </td>


                        <td style="background: transparent; border: none;">
                            <asp:TextBox ID="UI_txtRMANo" runat="server" class="erma-input-input" />
                        </td>


                        <td style="background: transparent; border: none;">
                            <asp:Label ID="UI_lblStatus" runat="server" Text="032_Status"></asp:Label>
                        </td>

                        <td style="background: transparent; border: none;">
                            <asp:DropDownList ID="UI_cboStatus" runat="server" CssClass="erma-combobox-choose"></asp:DropDownList>
                        </td>

                        <td style="background: transparent; border: none;">
                            <asp:Label ID="UI_lblModelNo" runat="server" Text="012_Model No."></asp:Label>
                        </td>


                        <td style="background: transparent; border: none;">
                            <asp:TextBox ID="UI_txtModelNo" runat="server" CssClass="erma-input-input"></asp:TextBox>
                        </td>

                        <td style="background: transparent; border: none;">
                            <asp:Label ID="UI_lblSerialNumber" runat="server" Text="013_Serial Number"></asp:Label>
                        </td>

                        <td style="background: transparent; border: none;">
                            <asp:TextBox ID="UI_txtSerialNumber" runat="server" CssClass="erma-input-input"></asp:TextBox>
                        </td>
                        <td style="background: transparent; border: none;">
                            <asp:Button ID="UI_cmdSearch" runat="server" Text="_Search" CssClass="erma-search-button-001" Width="91" Height="36" />
                        </td>
                    </tr>
                </table>

                <asp:Panel runat="server" ID="UI_panSearch" DefaultButton="UI_cmdSearch"></asp:Panel>

                <div class="erma-infomation-resultView erma-information">
                    <div class="erma-information-block">
                        <div class="erma-left-block">
                            <asp:Label ID="UI_lblRequestedTittle" runat="server" Text="034_Requested Information" Font-Bold="true"></asp:Label>
                        </div>
                        <div class="erma-right-block" runat="server" id="UI_cmdSearch_td">
                            <div id="Div1" runat="server" class="erma-changePage-component">
                            </div>
                            <div id="right_component" runat="server" class="erma-changePage-component">
                                <ul>


                                    <li>
                                        <asp:ImageButton ID="first_ImageBtn" runat="server" ImageUrl="~/images_new/first.svg" CssClass="imgbutton" />
                                    </li>
                                    <li>
                                        <asp:ImageButton ID="previous_ImageBtn" runat="server" ImageUrl="~/images_new/previous.svg" CssClass="imgbutton" />
                                    </li>
                                    <li>

                                        <div class="erma-pages-block">
                                            <asp:TextBox ID="Current_Page_Label" runat="server" AutoPostBack="true"></asp:TextBox>
                                            <span class="erma-pages-mark">/</span>
                                            <asp:Label ID="Total_Page_Label" runat="server" Text=""></asp:Label>
                                        </div>

                                    </li>
                                    <li>
                                        <asp:ImageButton ID="next_ImageBtn" runat="server" ImageUrl="~/images_new/next.svg" CssClass="imgbutton" />
                                    </li>
                                    <li>
                                        <asp:ImageButton ID="last_ImageBtn" runat="server" ImageUrl="~/images_new/last.svg" CssClass="imgbutton" />
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </div>

                    <asp:Panel runat="server" ID="panel_BookMark_Label" Visible="false">
                        <div style="visibility: hidden">
                            <asp:Label ID="BookMark_Label" runat="server" Text=""></asp:Label>
                        </div>
                    </asp:Panel>

                    <div class="erma-infomation-resultView erma-information">
                        <!--[Begin]資料列表表單-->

                        <asp:GridView ID="UI_dvRequest" runat="server" GridLines="None" class="erma-table-components" AutoGenerateColumns="False" AllowPaging="true">
                            <HeaderStyle CssClass="erma-table-header" />

                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <%--<asp:Label ID="UI_SeqID" runat="server" Text='<%# Eval("SeqID") %>'></asp:Label>--%>
                                        <asp:Label ID="UI_RMAID" runat="server" Text='<%# Eval("RMA_ID") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="UI_RMANO" runat="server" Text='<%# Eval("RMA_NO") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="UI_RMADID" runat="server" Text='<%# Eval("RMAD_ID") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="UI_RMASTATUS" runat="server" Text='<%# Eval("RMA_STATUS") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="UI_RMADSTATUS" runat="server" Text='<%# Eval("RMAD_STATUS") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="UI_RMADWARRANTY" runat="server" Text='<%# Eval("RMAD_WARRANTY") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="UI_RMADCSTMP" runat="server" Text='<%# Eval("RMAD_CSTMP") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="UI_SERIALNO" runat="server" Text='<%# Eval("RMAD_SERIALNO") %>' Visible="false"></asp:Label>
                                        <%-- 修改Client端上方Search會出現錯誤bug by buck modify 20250923 begin--%>
                                        <asp:Label ID="UI_RECEVSTATUS" runat="server" Text='<%# Eval("RMAD_RECEVSTATUS") %>' Visible="false"></asp:Label>
                                        <%-- 修改Client端上方Search會出現錯誤bug by buck modify 20250923 end--%>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%-- RMA單號 RMA_NO > 序號 RMAD_SERIALNO > 客戶產品編號 CUSTOMER_PRODUCT_NUMBER > 型號 RMAD_MODELNO > 保固日期 WARRANTY> 需求建立日期 RMA_CSTMP > 業務爆價總和 Amount> 狀態 Status  > 內容--%>
                                <%-- 修改Client端上方Search會出現錯誤bug by buck modify 20250923 begin--%>
                                <%-- <asp:BoundField DataField="RMA_NO" HeaderText="_Serial Number" ></asp:BoundField>
                                <asp:BoundField DataField="RMAD_SERIALNO" HeaderText="CUSTOMER_PRODUCT_NUMBER" ></asp:BoundField>
                                <asp:BoundField DataField="CUSTOMER_PRODUCT_NUMBER" HeaderText="_Model" ></asp:BoundField>
                                <asp:BoundField DataField="RMAD_MODELNO" HeaderText="_Warranty" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd}" ></asp:BoundField>
                                <asp:BoundField DataField="WARRANTY" HeaderText="_Request Date"  HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd}" ></asp:BoundField>
                                <asp:BoundField DataField="RMA_CSTMP" HeaderText="_RMA No"  DataFormatString="{0:yyyy/MM/dd}" ></asp:BoundField>
                                <asp:BoundField DataField="Amount" HeaderText="_Amount" ></asp:BoundField>
                                <asp:BoundField DataField="Status" HeaderText="_Status" ></asp:BoundField>--%>
                                <asp:BoundField DataField="RMA_NO" HeaderText="_RMA_NO"></asp:BoundField>
                                <asp:BoundField DataField="RMAD_SERIALNO" HeaderText="_RMAD_SERIALNO"></asp:BoundField>
                                <asp:BoundField DataField="CUSTOMER_PRODUCT_NUMBER" HeaderText="_CUSTOMER_PRODUCT_NUMBER" ></asp:BoundField>
                                <asp:BoundField DataField="RMAD_MODELNO" HeaderText="_RMAD_MODELNO" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd}"></asp:BoundField>
                                <asp:BoundField DataField="RMAD_WARRANTY" HeaderText="_RMAD_WARRANTY" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd}"></asp:BoundField>
                                <asp:BoundField DataField="RMA_CSTMP" HeaderText="_RMA_CSTMP" DataFormatString="{0:yyyy/MM/dd}"></asp:BoundField>
                                <asp:BoundField DataField="RMASQ_QUOTE" HeaderText="_Amount"></asp:BoundField>
                                <asp:BoundField DataField="RMAD_Status" HeaderText="_Status"></asp:BoundField>
                                <%-- 修改Client端上方Search會出現錯誤bug by buck modify 20250923 end--%>
                                <asp:TemplateField HeaderText="_Detail">
                                    <HeaderStyle></HeaderStyle>
                                    <ItemStyle></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Button ID="UI_cmdDetail" CssClass="UI_cmdDetail_UI" runat="server" CommandName="cmdDetail" CommandArgument='<%#Me.UI_dvRequest.Rows.Count%>' Visible="false" />
                                        <asp:Button ID="UI_cmdEdit" CssClass="UI_cmdDetail_UI" runat="server" CommandName="cmdDetail" CommandArgument='<%#Me.UI_dvRequest.Rows.Count%>' Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>

                            <EmptyDataTemplate>
                                <asp:Label runat="server" ID="UI_NotFound" Text='<%# Session("_isShowNotFound").ToString()%>'></asp:Label>
                            </EmptyDataTemplate>
                            <EmptyDataRowStyle HorizontalAlign="Center" />

                            <HeaderStyle />
                            <RowStyle />
                            <AlternatingRowStyle />
                            <PagerSettings Visible="False"></PagerSettings>

                        </asp:GridView>
                        <!--[End]資料列表表單-->
                    </div>

                    <%--修改RMA 開始--%>
                    <div style="visibility: hidden;">
                        <asp:Button ID="UI_Up_RMA_Btn" runat="server" Text="" />
                    </div>
                    <asp:Panel ID="UI_Up_RMA_panel" runat="server" CssClass="ucSubstitute_div erma-fly-panel" Style="display: none; position: absolute;">


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

                    <uc1:ucClientDetail ID="ucClientDetail" runat="server" />

                    <asp:Label ID="UI_lblPreviousPage_RMAID" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="UI_lblPreviousPage_RMANO" runat="server" Visible="false"></asp:Label>
                </div>

                <asp:Button ID="UI_Up_cmdCancel" runat="server" Text="" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
