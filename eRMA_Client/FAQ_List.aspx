<%@ Page Language="VB" MasterPageFile="~/MasterPageLeft.master" AutoEventWireup="false" CodeFile="FAQ_List.aspx.vb" Inherits="FAQ_List" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <link href="css/rmaFaq.css" rel="stylesheet" />
            <script src="../js/system.js"></script>

            <style>
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
            </style>

            <style>
                .erma-table-components-FAQ th {
                    display: none;
                }

                .erma-table-components-FAQ td {
                    border: 1px solid #D8D8D8;
                    border-radius: 5px;
                    padding-left: 20px;
                    padding-right: 20px;
                    padding-top: 15px;
                    padding-bottom: 15px;
                }

                .erma-table-components-FAQ tr {
                    height: 78px;
                }

                .erma-table-components-FAQ {
                    width: 100%;
                    border-collapse: separate;
                    border-spacing: 0px 10px;
                }

                    .erma-table-components-FAQ tr:first-child {
                        display: none;
                    }

                .div-erma-table-components-FAQ {
                    width: 100%;
                    height: 520px;
                    overflow-y: scroll;
                    padding-top: 5px;
                    position: relative;
                    top: -50px;
                    padding-top: 10px;
                }

                    .div-erma-table-components-FAQ table:first-child {
                        position: relative;
                        top: -50px;
                    }


                .FAQ_title {
                    color: var(--black, #000);
                    font-family: Source Sans Pro;
                    font-size: 16px;
                    font-style: normal;
                    font-weight: 700;
                    line-height: normal;
                    padding-left: 20px;
                }

                .FAQ_context {
                    Browse: plugins please refer the SOP - color: var(--black, #000);
                    font-family: Source Sans Pro;
                    font-size: 14px;
                    font-style: normal;
                    font-weight: 400;
                    line-height: normal;
                    padding-left: 20px;
                }

                .FAQ_btn {
                    display: inline-flex;
                    padding: 3px 25px;
                    align-items: flex-start;
                    gap: 10px;
                    border-radius: 5px;
                    border: 1px solid #000;
                    background: #ffffff;
                    color: #000;
                    font-family: Source Sans Pro;
                    font-size: 14px;
                    font-style: normal;
                    font-weight: 400;
                    line-height: normal;
                }

                    .FAQ_btn:hover {
                        display: inline-flex;
                        padding: 3px 25px;
                        align-items: flex-start;
                        gap: 10px;
                        border-radius: 5px;
                        border: 1px solid #4F4F4F;
                        background: #4F4F4F;
                        color: #ffffff;
                        font-family: Source Sans Pro;
                        font-size: 14px;
                        font-style: normal;
                        font-weight: 400;
                        line-height: normal;
                    }
            </style>

            <div class="erma-faq-content">
                <div class="erma-top-bar">
                    <a href="Client_FlowCase01_Worklist.aspx">
                        <div class="erma-button-back">
                            <img src="../images_new/previous.svg" />
                            <asp:Label ID="UI_lblBack" runat="server" Text="Back"></asp:Label>
                        </div>
                    </a>

                    <div class="erma-personalInformation-title">
                        <center>
                            <asp:Label ID="UI_lblTittle" runat="server" Text="032_Return Merchanise Authorization (RMA) FAQ" Font-Size="Larger"></asp:Label>
                        </center>
                    </div>
                </div>
                <div class="erma-catogory-div">
                    <div class="erma-category-combobox erma-combobox-control">
                        <asp:DropDownList ID="UI_cboCategory1" CssClass="erma-combobox-choose" Width="189" Height="36" runat="server" AutoPostBack="true"></asp:DropDownList>
                    </div>
                    <div class="erma-subCategory-combobox erma-combobox-control">
                        <asp:DropDownList ID="UI_cboCategory2" CssClass="erma-combobox-choose" Width="189" Height="36" runat="server"></asp:DropDownList>
                    </div>
                    <asp:TextBox ID="UI_txtQuestion" runat="server" class="erma-search-input erma-input-input" placeholder="Search"></asp:TextBox>
                    <asp:Button ID="UI_cmdSearch" runat="server" Width="91" Height="36" Text="_Search" CssClass="FAQ_btn" />

                </div>

                <asp:GridView ID="UI_dvFAQ" runat="server" PagerSettings-Visible="false" CellPadding="0" CellSpacing="1" border="0" GridLines="None" CssClass="erma-table-components-FAQ" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric">

                    <Columns>
                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Left" CssClass="erma-table-components-FAQ-tr"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="UI_QUESTION" runat="server" Text='<%# Eval("FAQ_QUESTION") %>' CssClass="FAQ_title"></asp:Label><br />
                                <asp:Label ID="UI_ANSWER" runat="server" Text='<%# Eval("FAQ_ANSWER") %>' CssClass="FAQ_context"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                    <%--<RowStyle CssClass="TR_1" />--%>
                    <AlternatingRowStyle CssClass="ListRowEven" />
                    <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Center" />
                </asp:GridView>

                <center>
                    <div class="erma-changePage-component" style="width: 396px; height: 36px;">
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
                </center>
                <asp:HiddenField ID="BookMark_Label" runat="server" Value="" />
            </div>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>
