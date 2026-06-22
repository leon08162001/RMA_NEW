<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Report1_Search.aspx.vb" Inherits="Report1_Search" Title="Untitled Page" %>

<%@ Register Src="ascx/ucSubstitute.ascx" TagName="ucSubstitute" TagPrefix="uc1" %>
<%@ Register Src="ascx/ucPartPic.ascx" TagName="ucPartPic" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">

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

        .UI_cmdReset_btn {
            background: #EAEAEA;
            border: 1px solid #D8D8D8;
            border-radius: 5px;
        }

        .Report_Cancel {
            background-image: url(../images_new/error.png);
            background-size: 16px 16px;
            background-repeat: no-repeat;
            background-position: center;
            border: none;
            width: 100%;
            height: 100%;
            background-color: transparent;
        }
    </style>
    <script type="text/javascript">
        function ImageClick(window) {
            document.getElementById("Report1_img").src = window.src;
            var mpe = $find('Report1_Search_ModalPopupExtender');
            mpe.show();
            return false;
        }

        function Close_windows() {
            //所有視窗程式 關閉
            $find("mpe").hide();
            //$find("ajModalProgress_mpe").hide();
            $find("Flympe").hide();
            return false;
        }

        function normalImg(UI) {
            const collection = UI.children;
            for (let i = 0; i < collection.length; i++) {
                if (collection[i].tagName = "img") {
                    document.getElementById("Report1_img").src = collection[i].src;
                }
            }
            var mpe = $find('Report1_Search_ModalPopupExtender');
            mpe.show();
        }
    </script>

    <div class="erma-window-background erma-window-image-background" style="display: none;">
        <div class="erma-zoom-image-block">
            <img src="../images/demo.png" />
            <div class="erma-cancle-block" onclick="displayFun('.erma-window-image-background')">
                <img src="../images/error.png" class="erma-cancelIcon" />
            </div>
        </div>
    </div>

    <div class="erma-information-view erma-information-parts-view" style="width: 100%;">
        <!-- tabs -->
        <div class="erma-infomation-area2">
            <div class="erma-infomation-text">
                <div class="erma-infomation-block">
                    <img src="../images/info.svg" />
                    <asp:Label ID="UI_lblTittle" runat="server" Text="001_Part's Query" CssClass="title_span erma-information-title"></asp:Label>
                </div>
                <div class="erma-date-block">
                </div>
            </div>
            <div class="erma-infomation-resultView erma-infomation-productNoView">

                <div class="erma-infomation-number-area">
                    <div class="erma-combobox-control">
                        <asp:DropDownList ID="Down" runat="server" CssClass="erma-combobox-choose" AutoPostBack="true"></asp:DropDownList>
                    </div>

                    <asp:Panel ID="UI_cboPrimalModelNo_Panel" CssClass="erma-combobox-choose-server" runat="server" Width="100%">
                        <asp:DropDownList runat="server" ID="UI_cboPrimalModelNo" CssClass="erma-combobox-choose"></asp:DropDownList>
                    </asp:Panel>

                    <%--        
                <asp:Panel ID="UI_txtSerialNo_Panel" runat="server">
                <asp:TextBox ID="UI_txtSerialNo" runat="server" CssClass="erma-input-input"  Width="810"  Height="36" ></asp:TextBox>
                </asp:Panel>
                    --%>

                    <asp:Panel ID="UI_txtProduct_SerialNo_Panel" runat="server" Width="100%">
                        <asp:TextBox ID="UI_txtProduct_SerialNo" runat="server" CssClass="erma-input-input" placeholder="Please enter Serial Number" Width="98%"></asp:TextBox>
                    </asp:Panel>

                    <asp:Panel ID="UI_txtPartsNo_Panel" runat="server" Width="100%">
                        <asp:TextBox ID="UI_txtPartsNo" runat="server" CssClass="erma-input-input" placeholder="Please enter Part's Number" Width="98%"></asp:TextBox>
                    </asp:Panel>

                    <div class="erma-number-buttons">
                        <asp:Button ID="UI_cmdSearch" runat="server" Text="004_Search" CssClass="erma-search-button" />
                        <asp:Button ID="UI_cmdReset" runat="server" Text="Reset" CssClass="erma-search-button erma-reset-button" />
                    </div>
                </div>

                <div class="erma-noData-view" style="display: block;">
                    <div class="erma-information-block">
                        <div class="erma-left-block">
                            <asp:Label ID="lblInformation" runat="server" Text="" CssClass="erma-infomation-text"></asp:Label>

                            <div class="erma-infomation-number" runat="server" id="erma_infomation_number_count_lab">
                                <asp:Label ID="Count_Lab_" runat="server" Text=""></asp:Label>
                                Items
                            </div>
                        </div>
                        <div id="right_component" runat="server" class="erma-right-block">
                            <asp:Button ID="UI_cmdExport" runat="server" Text="076_Export" CssClass="Export_Edit" OnClick="UI_cmdExport_Click"></asp:Button>
                            <asp:Button ID="UI_ExportWithPic" runat="server" Text="Export" CssClass="Export_Edit" />
                            <asp:Button ID="UI_ExportAll" runat="server" Text="Exp All Model" CssClass="Export_Edit" />
                            <div class="erma-changePage-component">
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

                                            <span>/</span>
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
                </div>

                <!--[Begin]資料列表表單-->
                <asp:GridView ID="UI_dvReport" runat="server" GridLines="None" CellPadding="1" class="erma-table-components erma-table-reportSearch" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric">
                    <HeaderStyle CssClass="erma-table-header" />
                    <RowStyle CssClass="erma-table-list" />
                    <Columns>
                        <asp:TemplateField Visible="false">

                            <ItemTemplate>
                                <asp:Label ID="UI_SeqID" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="UI_bmb03" runat="server" Text='<%# Eval("bmb03") %>' Visible="false"></asp:Label>
                                <asp:Label ID="UI_Substitute1" runat="server" Text='<%# Eval("Substitute1") %>' Visible="false"></asp:Label>
                                <asp:Label ID="UI_Substitute2" runat="server" Text='<%# Eval("Substitute2") %>' Visible="false"></asp:Label>
                                <asp:Label ID="UI_imgFile" runat="server" Text='<%# Eval("imgfile") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Pic">
                            <ItemStyle CssClass="erma-table-pic" />
                            <ItemTemplate>
                                <div class="erma-image-box" onclick="normalImg(this);">
                                    <asp:Image ID="Image1" runat="server" ImageUrl='<%#  "https://e-rma.cipherlab.com.tw/object/PartsPic/" + Eval("imgfile") %>' Length="100px" Width="100%" onclick="ImageClick(this);" />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="EXPORT_MODELNO" HeaderText="101_Model No" Visible="false"></asp:BoundField>

                        <asp:BoundField DataField="bmb03" HeaderText="152_Part’s No" ItemStyle-CssClass="erma-table-serialNo"></asp:BoundField>
                        <asp:BoundField DataField="RPBOM_DESC" ItemStyle-CssClass="erma-table-description"></asp:BoundField>
                        <asp:BoundField DataField="RPBOM_MATERIALCOST" ItemStyle-CssClass="erma-table-listPrice"></asp:BoundField>
                        <asp:TemplateField HeaderText="162_取代">
                            <ItemStyle CssClass="erma-table-replacement" />
                            <ItemTemplate>
                                <asp:Button ID="UI_cmdSubstitute1" runat="server" Text="073_Edit" CommandName="cmdSubstitute1" CommandArgument='<%# me.UI_dvReport.Rows.Count%>' Visible="false" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="163_替代">
                            <ItemStyle CssClass="erma-table-replacement" />
                            <ItemTemplate>
                                <asp:Button ID="UI_cmdSubstitute2" runat="server" Text="074_Edit" CommandName="cmdSubstitute2" CommandArgument='<%# me.UI_dvReport.Rows.Count%>' Visible="false" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="" Visible="false">
                            <ItemTemplate>
                                <asp:Button ID="UI_cmdPic" runat="server" Text="075_Edit" CommandName="cmdPic" CommandArgument='<%# me.UI_dvReport.Rows.Count%>' Visible="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>

                        <asp:Label runat="server" ID="UI_NotFound" Text='<%# Session("_isShowNotFound").ToString()%>' />

                    </EmptyDataTemplate>
                    <AlternatingRowStyle />
                    <PagerSettings Visible="false"></PagerSettings>
                    <PagerStyle />
                </asp:GridView>
                <!--[End]資料列表表單-->
            </div>
        </div>
    </div>

    <uc1:ucSubstitute ID="ucSubstitute" runat="server" />
    <uc2:ucPartPic ID="ucPartPic" runat="server" />

    <div style="visibility: hidden">
        <asp:Label ID="BookMark_Label" runat="server" Text=""></asp:Label>
    </div>

    <%--修改RMA 開始--%>
    <div style="visibility: hidden;">
        <asp:Button ID="Report_Btn" runat="server" Text="" />
    </div>
    <asp:Panel ID="Report_panel" runat="server" CssClass="erma-window-background erma-window-image-background" Style="display: none; position: absolute; border: none; background: rgb(0 0 0 / 60%);">
        <div class="erma-zoom-image-block">
            <div class="erma-cancle-block">
                <asp:Button ID="Report_cmdCancel" runat="server" Text="" Font-Size="13" CssClass="Report_Cancel" />
            </div>
            <img id="Report1_img" alt="" src="" style="width: 100%; height: auto; max-height: 780px;" />
        </div>
    </asp:Panel>
    <ajaxToolkit:ModalPopupExtender
        ID="Report_ModalPopupExtender"
        TargetControlID="Report_Btn"
        BehaviorID="Report1_Search_ModalPopupExtender"
        PopupControlID="Report_panel"
        CancelControlID="Report_cmdCancel"
        BackgroundCssClass="Report_Modal"
        runat="server">
    </ajaxToolkit:ModalPopupExtender>
    <%--修改RMA 結束--%>
</asp:Content>
