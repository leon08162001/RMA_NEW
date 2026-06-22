<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="WarrantyQuery.aspx.vb" Inherits="WarrantyQuery" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">

    <link rel="stylesheet" href="~/css/system.css">
    <link rel="stylesheet" href="~/css/commonStyle.css">
    <link rel="stylesheet" href="~/css/system-header.css">

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

        .Select_Data_Not_found {
            position: absolute;
            top: 58%;
            right: 48%;
            left: 58%;
            width: 125px;
        }

        .Show_Name {
            font-size: 18px;
            align-items: center;
        }

        .Information_Title {
            margin-bottom: 5px;
        }
    </style>

    <script type="text/javascript">
        function displayFunID(className1) {
            let displayType = $(className1)[0].style.display;
            if (displayType != "none")
                $(className1).css("display", "none");
            else
                $(className1).css("display", "block");


            if (className2 != null) {
                displayFun(className2);
            }
        }

        function Close_windows() {
            //所有視窗程式 關閉
            $find("mpe").hide();
            $find("Flympe").hide();
            return false;
        }
    </script>

    <div class="erma-information-view erma-information-warrantyQuery-view" style="width: 100%;">
        <div class="erma-infomation-text">
            <div class="erma-infomation-block">
                <img src="../images_new/info.svg" />
                <asp:Label ID="Warranty_Query_Label" CssClass="erma-information-title" runat="server" Text=""></asp:Label>

            </div>
            <div class="erma-date-block">
            </div>
        </div>
        <div class="erma-infomation-number-area">

            <asp:TextBox ID="sRMANo_Txt" runat="server" CssClass="erma-input-input" placeholder="Please enter Product Serial Number"></asp:TextBox>


            <div class="erma-number-buttons">

                <asp:Button ID="SearchBtn" runat="server" Text="Search" CssClass="erma-search-button" OnClientClick="displayFun('.erma-infomation-resultView .erma-information-warrantyQuery-box');" />

            </div>
        </div>


        <div class="erma-information-block">
            <div class="erma-left-block">
                <asp:Label ID="lblInformation" runat="server" Text="" CssClass="Show_Name"></asp:Label>

            </div>

        </div>

        <asp:Label ID="UI_NotFound_Label" runat="server" Text="Data not found" Visible="false" CssClass="Select_Data_Not_found"></asp:Label>
        <asp:Panel ID="ermatablecomponentsPanel" runat="server" Visible="false">
            <div class="erma-table-components" style="background: none; border: none;">

                <div class="erma-infomation-resultView  erma-information-warrantyQueryView">

                    <div class="erma-information-warrantyQuery-box" style="padding: 0;">


                        <asp:Panel ID="found_Panel" runat="server">
                            <center>
                            </center>
                        </asp:Panel>
                        <div class="Information_Title">
                            <asp:Label ID="UI_lblPriceList" runat="server" Text="025_Price List"></asp:Label>
                        </div>
                        <asp:ListView ID="UI_dvRMAListView" runat="server">
                            <ItemTemplate>
                                <div class="erma-warrantyQuery-block">
                                    <table>
                                        <tr runat="server" class="erma-query-cards">
                                            <td style="width: 90%;">
                                                <table class="erma-cards-table">
                                                    <tr class="erma-cards-left">

                                                        <td class="erma-query-text">
                                                            <p class="erma-query-title"><%= getoLanguage("Customer", "004") %> </p>
                                                            <p><%# Eval("EXPORT_CUSTOMERNAME") %> </p>
                                                        </td>


                                                        <td class="erma-query-text">
                                                            <p class="erma-query-title"><%= getoLanguage("Report", "152") %> </p>
                                                            <p><%# Eval("EXPORT_PARTNO") %> </p>
                                                        </td>
                                                        <td class="erma-query-text">
                                                            <p class="erma-query-title" style="width: 125px;"><%= getoLanguage("RMA", "098") %></p>
                                                            <p><%# Eval("EXPORT_SERIALNO") %> </p>
                                                        </td>
                                                        <td class="erma-query-text">
                                                            <p class="erma-query-title"><%= getoLanguage("RMA", "141") %> </p>
                                                            <p><%# If(Eval("EXPORT_SHIPPING_TIME") Is DBNull.Value, "-", Convert.ToDateTime(Eval("EXPORT_SHIPPING_TIME")).ToString("yyyy/MM/dd")) %></p>
                                                        </td>
                                                        <td class="erma-query-text">
                                                            <p class="erma-query-title"><%= getoLanguageword("Transfer", "030") %> </p>
                                                            <p>
                                                                <%# If(Eval("CW_EDATE") Is DBNull.Value, "-", Convert.ToDateTime(Eval("CW_EDATE")).ToString("yyyy/MM/dd")) %>
                                                            </p>
                                                        </td>


                                                        <td class="erma-query-text">
                                                            <p class="erma-query-title" style="width: 125px;"><%= getoLanguage("RMA2", "104") %></p>
                                                            <p>
                                                                <%# If(Eval("EXPORT_WARRANTY_DATE") Is DBNull.Value, "-", Convert.ToDateTime(Eval("EXPORT_WARRANTY_DATE")).ToString("yyyy/MM/dd")) %>
                                                            </p>
                                                        </td>

                                                        <td class="erma-query-text">
                                                            <p class="erma-query-title" style="width: 125px;"><%= getoLanguage("RMA2", "105") %></p>
                                                            <p><%# Eval("WAR_TYPE") %></p>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </td>
                                            <td style="width: 10%;" class="erma-more">
                                                <a href="#" onclick="displayFunID('<%#  "#" + Eval("EXPORT_SERIALNO") %>')">More
                                                </a>
                                                <div class="erma-more-area" id='<%# Eval("EXPORT_SERIALNO") %>' style="display: none;">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <thead>
                                                            <tr>
                                                                <td><%= getoLanguage("Report", "152") %></td>
                                                                <td><%= getoLanguage("RMA", "098") %></td>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <%# Eval("Context") %>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ItemTemplate>

                        </asp:ListView>

                        <asp:Panel ID="AccessoriesInformationPanel" runat="server" Visible="false">
                            <div class="Information_Title">
                                <asp:Label ID="lblPurchasing" runat="server" Text="025_Purchasing Records"></asp:Label>
                            </div>
                            <asp:ListView ID="dgvPurchasing" runat="server">
                                <ItemTemplate>
                                    <div class="erma-warrantyQuery-block">
                                        <table>
                                            <tr runat="server" class="erma-query-cards">
                                                <td width="100%">
                                                    <table class="erma-cards-table">
                                                        <tr class="erma-cards-left">


                                                            <td class="erma-query-text">
                                                                <p class="erma-query-title"><%= getoLanguage("RMA2", "058") %> </p>
                                                                <p><%# Eval("SerialNo") %> </p>
                                                            </td>
                                                            <td class="erma-query-text">
                                                                <p class="erma-query-title"><%= getoLanguage("Warranty", "080") %> </p>
                                                                <p><%# Eval("PurchaseDate") %> </p>
                                                            </td>
                                                            <td class="erma-query-text">
                                                                <p class="erma-query-title"><%= getoLanguage("RMA2", "105") %></p>
                                                                <p><%# Eval("WarrantyCode") %> </p>
                                                            </td>

                                                            <td class="erma-query-text">
                                                                <p class="erma-query-title" style="width: 125px;"><%= getoLanguage("Warranty", "069") %></p>
                                                                <p><%# Eval("StartDate") %> </p>
                                                            </td>
                                                            <td class="erma-query-text">
                                                                <p class="erma-query-title" style="width: 125px;"><%= getoLanguage("Warranty", "070") %></p>
                                                                <p><%# Eval("EndDate") %> </p>
                                                            </td>

                                                            <td class="erma-query-text">
                                                                <p class="erma-query-title" style="width: 125px;"><%= getoLanguage("RMA", "012") %></p>
                                                                <p><%# Eval("Model") %> </p>
                                                            </td>

                                                            <td class="erma-query-text">
                                                                <p class="erma-query-title" style="width: 125px;"><%= getoLanguage("Report", "152") %></p>
                                                                <p><%# Eval("SKU") %> </p>
                                                            </td>

                                                        </tr>

                                                        <tr class="erma-cards-left">
                                                            <td class="erma-query-text" colspan="7">
                                                                <p class="erma-query-title"><%= getoLanguage("Warranty", "074") %></p>
                                                                <p><%# Eval("Description") %> </p>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>

                                            </tr>
                                        </table>
                                    </div>
                                </ItemTemplate>

                            </asp:ListView>
                            <div class="Information_Title">
                                <asp:Label ID="lblAccessoriesInformation" runat="server" Text="Accessories Information"></asp:Label>
                            </div>
                            <%--三大項--%>
                            <table class="erma-table-components" border="0" cellspacing="0" cellpadding="0" style="height: auto;">
                                <tbody>
                                    <tr style="border: 0px solid #ffffff;">

                                        <td style="width: 20%; border: 0px solid #ffffff;">
                                            <asp:Label ID="lblPartPO" runat="server" Text="PO Date"></asp:Label>
                                        </td>
                                        <td style="width: 20%; border: 0px solid #ffffff;">
                                            <p class="erma-query-title"><%= getoLanguage("RMA2", "052") %></p>
                                            <asp:Label ID="lblPartContent" runat="server" Text="Content" Visible="false"></asp:Label>
                                        </td>
                                        <td style="width: 17%; border: 0px solid #ffffff;">
                                            <p class="erma-query-title"><%= getoLanguageword("Transfer", "014") %></p>
                                            <asp:Label ID="lblPartMonth" runat="server" Text="Month" Visible="false"></asp:Label>
                                        </td>
                                        <td style="width: 17%; border: 0px solid #ffffff;">
                                            <p class="erma-query-title"><%= getoLanguage("Warranty", "031") %></p>
                                            <asp:Label ID="lblPartExtra" runat="server" Text="Extra" Visible="false"></asp:Label>
                                        </td>
                                        <td style="width: 17%; border: 0px solid #ffffff;">
                                            <p class="erma-query-title"><%= getoLanguage("RMA", "151") %></p>
                                            <asp:Label ID="lblPartMemo" runat="server" Text="Memo" Visible="false"></asp:Label>
                                        </td>
                                        <td style="width: 17%; border: 0px solid #ffffff;">
                                            <p class="erma-query-title"><%= getoLanguage("Warranty", "070") %></p>
                                            <asp:Label ID="lblPartEndDate" runat="server" Text="End Date" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <asp:DataList ID="lstParts" runat="server" Width="100%" HorizontalAlign="left"
                                        CellPadding="0" CellSpacing="0" RepeatLayout="Flow" RepeatColumns="1000000">
                                        <ItemTemplate>
                                            <tr class="erma-table-list">

                                                <td style="">
                                                    <%# DataBinder.Eval(Container.DataItem, "PODate")%>
                                                </td>
                                                <td style="">
                                                    <%# DataBinder.Eval(Container.DataItem, "TYPE_NAME")%>
                                                </td>
                                                <td style="">
                                                    <%# DataBinder.Eval(Container.DataItem, "WAP_MON")%>
                                                </td>
                                                <td style="">
                                                    <%# DataBinder.Eval(Container.DataItem, "WAP_EMON")%>
                                                </td>
                                                <td style="">
                                                    <%# DataBinder.Eval(Container.DataItem, "WAP_DESC")%>
                                                </td>
                                                <td style="">
                                                    <font color="red"><%# DataBinder.Eval(Container.DataItem, "WarrEndDate")%></font>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:DataList>
                                </tbody>
                            </table>

                            <br />
                            <br />

                            <!-- 版本 START -->
                            <asp:CheckBox ID="UI_CheckVer" runat="server" AutoPostBack="true" Visible="true" OnCheckedChanged="UI_CheckVer_CheckedChanged" /><asp:Label runat="server" ID="lblVerChange" Text="Version Modification"></asp:Label>

                            <br />
                            <br />

                            <asp:Panel ID="pnlVersion" runat="server">
                                <table id="table2" class="default" bordercolor="#c0c0c0" cellspacing="0" cellpadding="3"
                                    width="100%" border="1" bordercolorlight="#FFFFFF">
                                    <tr>
                                        <td style="width: 10%;" class="default">
                                            <asp:Label runat="server" ID="lblColumn"></asp:Label>
                                        </td>
                                        <td style="width: 15%;" class="default">
                                            <asp:Label runat="server" ID="lblVerName"></asp:Label>
                                        </td>
                                        <td style="width: 37%;" class="default">
                                            <asp:Label runat="server" ID="lblVerBefore"></asp:Label>
                                        </td>
                                        <td class="default">
                                            <asp:Label runat="server" ID="lblVerAfter"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="default" align="left">
                                            <asp:Label runat="server" ID="lblC01"></asp:Label></td>
                                        <td class="default" align="left">
                                            <asp:Label ID="lblA01" runat="Server"></asp:Label></td>
                                        <td align="left">
                                            <asp:TextBox ID="txtA01" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblOA01" runat="Server"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtQA01" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblNA01" runat="Server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="default" align="left">
                                            <asp:Label runat="server" ID="lblC02"></asp:Label></td>
                                        <td class="default" align="left">
                                            <asp:Label ID="lblA02" runat="Server"></asp:Label></td>
                                        <td align="left">
                                            <asp:TextBox ID="txtA02" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblOA02" runat="Server"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtQA02" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblNA02" runat="Server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="default" align="left">
                                            <asp:Label runat="server" ID="lblC03"></asp:Label></td>
                                        <td class="default" align="left">
                                            <asp:Label ID="lblA03" runat="Server"></asp:Label></td>
                                        <td align="left">
                                            <asp:TextBox ID="txtA03" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblOA03" runat="Server"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtQA03" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblNA03" runat="Server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="default" align="left">
                                            <asp:Label runat="server" ID="lblC04"></asp:Label></td>
                                        <td class="default" align="left">
                                            <asp:Label ID="lblA04" runat="Server"></asp:Label></td>
                                        <td align="left">
                                            <asp:TextBox ID="txtA04" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblOA04" runat="Server"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtQA04" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblNA04" runat="Server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="default" align="left">
                                            <asp:Label runat="server" ID="lblC05"></asp:Label></td>
                                        <td class="default" align="left">
                                            <asp:Label ID="lblA05" runat="Server"></asp:Label></td>
                                        <td align="left">
                                            <asp:TextBox ID="txtA05" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblOA05" runat="Server"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtQA05" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblNA05" runat="Server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="default" align="left">
                                            <asp:Label runat="server" ID="lblC06"></asp:Label></td>
                                        <td class="default" align="left">
                                            <asp:Label ID="lblA06" runat="Server"></asp:Label></td>
                                        <td align="left">
                                            <asp:TextBox ID="txtA06" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblOA06" runat="Server"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtQA06" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblNA06" runat="Server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="default" align="left">
                                            <asp:Label runat="server" ID="lblC07"></asp:Label></td>
                                        <td class="default" align="left">
                                            <asp:Label ID="lblA07" runat="Server"></asp:Label></td>
                                        <td align="left">
                                            <asp:TextBox ID="txtA07" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblOA07" runat="Server"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtQA07" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblNA07" runat="Server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="default" align="left">
                                            <asp:Label runat="server" ID="lblC08"></asp:Label></td>
                                        <td class="default" align="left">
                                            <asp:Label ID="lblA08" runat="Server"></asp:Label></td>
                                        <td align="left">
                                            <asp:TextBox ID="txtA08" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblOA08" runat="Server"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtQA08" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblNA08" runat="Server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="default" align="left">
                                            <asp:Label runat="server" ID="lblC09"></asp:Label></td>
                                        <td class="default" align="left">
                                            <asp:Label ID="lblA09" runat="Server"></asp:Label></td>
                                        <td align="left">
                                            <asp:TextBox ID="txtA09" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblOA09" runat="Server"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtQA09" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblNA09" runat="Server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="default" align="left">
                                            <asp:Label runat="server" ID="lblC10"></asp:Label></td>
                                        <td class="default" align="left">
                                            <asp:Label ID="lblA10" runat="Server"></asp:Label></td>
                                        <td align="left">
                                            <asp:TextBox ID="txtA10" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblOA10" runat="Server"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtQA10" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblNA10" runat="Server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="default" align="left">
                                            <asp:Label runat="server" ID="lblC11"></asp:Label></td>
                                        <td class="default" align="left">
                                            <asp:Label ID="lblA11" runat="Server"></asp:Label></td>
                                        <td align="left">
                                            <asp:TextBox ID="txtA11" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblOA11" runat="Server"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtQA11" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblNA11" runat="Server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="default" align="left">
                                            <asp:Label runat="server" ID="lblC12"></asp:Label></td>
                                        <td class="default" align="left">
                                            <asp:Label ID="lblA12" runat="Server"></asp:Label></td>
                                        <td align="left">
                                            <asp:TextBox ID="txtA12" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblOA12" runat="Server"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtQA12" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblNA12" runat="Server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="default02" align="left">
                                            <asp:Label runat="server" ID="lblC13"></asp:Label></td>
                                        <td class="default02" align="left">
                                            <asp:Label ID="lblB01" runat="Server"></asp:Label></td>
                                        <td align="left">
                                            <asp:TextBox ID="txtB01" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblOB01" runat="Server"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtQB01" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblNB01" runat="Server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="default02" align="left">
                                            <asp:Label runat="server" ID="lblC14"></asp:Label></td>
                                        <td class="default02" align="left">
                                            <asp:Label ID="lblB02" runat="Server"></asp:Label></td>
                                        <td align="left">
                                            <asp:TextBox ID="txtB02" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblOB02" runat="Server"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtQB02" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblNB02" runat="Server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="default02" align="left">
                                            <asp:Label runat="server" ID="lblC15"></asp:Label></td>
                                        <td class="default02" align="left">
                                            <asp:Label ID="lblB03" runat="Server"></asp:Label></td>
                                        <td align="left">
                                            <asp:TextBox ID="txtB03" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblOB03" runat="Server"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtQB03" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblNB03" runat="Server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="default02" align="left">
                                            <asp:Label runat="server" ID="lblC16"></asp:Label></td>
                                        <td class="default02" align="left">
                                            <asp:Label ID="lblB04" runat="Server"></asp:Label></td>
                                        <td align="left">
                                            <asp:TextBox ID="txtB04" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblOB04" runat="Server"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtQB04" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblNB04" runat="Server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="default02" align="left">
                                            <asp:Label runat="server" ID="lblC17"></asp:Label></td>
                                        <td class="default02" align="left">
                                            <asp:Label ID="lblB05" runat="Server"></asp:Label></td>
                                        <td align="left">
                                            <asp:TextBox ID="txtB05" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblOB05" runat="Server"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtQB05" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblNB05" runat="Server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="default02" align="left">
                                            <asp:Label runat="server" ID="lblC18"></asp:Label></td>
                                        <td class="default02" align="left">
                                            <asp:Label ID="lblB06" runat="Server"></asp:Label></td>
                                        <td align="left">
                                            <asp:TextBox ID="txtB06" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblOB06" runat="Server"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtQB06" Width="140px" runat="server" CssClass="input" ReadOnly="True"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblNB06" runat="Server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <!-- 版本 END -->
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>

