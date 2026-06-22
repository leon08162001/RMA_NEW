<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ProductInformation_04.aspx.vb" Inherits="ProductInformation_04" %>

<%@ Register Src="ascx/ucProgressStatus.ascx" TagName="ucProgressStatus" TagPrefix="uc1" %>
<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc3" %>
<%@ Register Src="ascx/ucPopProblem_new.ascx" TagName="ucPopProblem" TagPrefix="uc4" %>
<%@ Register Src="ascx/ucModel.ascx" TagName="ucModel" TagPrefix="uc5" %>
<%@ Register Src="ascx/ucCustAddress.ascx" TagName="ucCustAddress" TagPrefix="uc6" %>
<%@ Register Src="ascx/ucCustomer_pick.ascx" TagName="ucCustomer_pick" TagPrefix="uc7" %>
<%@ Register Src="ascx/ucWarrantyPartsView.ascx" TagName="ucWarrantyPartsView" TagPrefix="uc8" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%--<%@ Register TagPrefix="Upload" Namespace="Brettle.Web.NeatUpload" Assembly="Brettle.Web.NeatUpload" %>--%>
<%@ Register Src="~/ascx/uc_Wait.ascx" TagPrefix="uc1" TagName="uc_Wait" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Product Information</title>

    <link href="css/system.css" rel="stylesheet" />
    <link href="css/commonStyle.css" rel="stylesheet" />
    <link href="css/system-header.css" rel="stylesheet" />
    <link href="NeatUpload/default.css" rel="stylesheet" />
    <script src="js/jquery-3.5.0.js"></script>

    <script>
        function Checking_ProductInformation_04() {

            var UI_txtApplicant_txt = document.getElementById('<%= UI_txtApplicant.ClientID %>');
            var UI_txtTel_txt = document.getElementById('<%= UI_txtTel.ClientID %>');
            var UI_txtMail_txt = document.getElementById('<%= UI_txtMail.ClientID %>');
            var UI_txtAddress_txt = document.getElementById('<%= UI_txtAddress.ClientID %>');

            var art = "";

            if (UI_txtApplicant_txt.value == "") {
                art += '<%= CheckingAlert("114") %>  \r\n';
            }

            if (UI_txtTel_txt.value == "") {
                art += '<%= CheckingAlert("115") %>  \r\n';
            }

            if (UI_txtMail_txt.value == "") {
                art += '<%= CheckingAlert("116") %>  \r\n';
            }

            if (UI_txtAddress_txt.value == "") {
                art += '<%= CheckingAlert("117") %>   \r\n';
            }

            if (art == "") {
                ShowProgressBar();
                return true;

            }
            else {

                alert(art);
                return false;
            }
        }

    </script>

    <script type="text/javascript">
        $(function () {
            $("#ddlMenuOne").append(
                "<option value='-1' selected='selected'>請選擇...</option>");
            ddlMenuTwoInit();

            $.getJSON(
                "DDLMenu.ashx",
                { menuOneId: $("#ddlMenuOne").val() },
                function (json) {
                    $.each(json, function (i) {
                        $("#ddlMenuOne").append(
                            "<option value='" + json[i].Id + "'>"
                            + json[i].MenuOneName + "</option>"
                        );
                    });
                });

            $("#ddlMenuOne").change(function (event) {
                if ($("#ddlMenuOne").val() !== "-1") {
                    $.getJSON(
                        "DDLMenu.ashx",
                        { menuOneId: $("#ddlMenuOne").val() },
                        function (json) {
                            ddlMenuTwoInit();
                            $.each(json, function (i) {
                                $("#ddlMenuTwo").append(
                                    "<option value='" + json[i].Id + "'>"
                                    + json[i].MenuTwoName + "</option>"
                                );
                            });
                        });
                }
                else {
                    ddlMenuTwoInit();
                }
            });
        });

        function ddlMenuTwoInit() {
            $("#ddlMenuTwo").html("").append(
                "<option value='-1' selected='selected'>請選擇...</option>");
        }
    </script>

    <style>
        .DelEndUserbtn, .EndUserbtn {
            width: auto;
            padding: 9px 18px;
            border: 1px solid black;
            background: #fff;
            color: black;
            border-radius: 5px;
            display: table;
            cursor: pointer;
        }

            .EndUserbtn:hover {
                border: 1px solid #31346d;
                background: #31346d;
                color: #ffffff;
                border-radius: 5px;
                display: table;
                cursor: pointer;
            }

            .DelEndUserbtn:hover {
                border: 1px solid #f44336;
                background: #f44336;
                color: #ffffff;
                border-radius: 5px;
                display: table;
                cursor: pointer;
            }

        .erma-button-next-div-new {
            border: 1px solid #496FF2;
            background: #496FF2;
            color: #ffffff;
            border-radius: 5px;
            margin-left: 5px;
            cursor: pointer;
        }

        .erma-button-next-div {
            border: 1px solid black;
            background: #fff;
            color: black;
            border-radius: 5px;
            margin-left: 5px;
            cursor: pointer;
        }

            .erma-button-next-div:hover {
                border: 1px solid #496FF2;
                background: #496FF2;
                color: #ffffff;
                border-radius: 5px;
                margin-left: 5px;
                cursor: pointer;
            }

        .remark-text {
            padding-top: 0;
        }
    </style>

</head>
<body>
    <div style="width: 750px"></div>

    <form id="form1" runat="server" onkeydown="if(event.keyCode==13)return false;">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <%--資料 開始--%>
        <asp:HiddenField ID="UI_lblAccountNameText_HiddenField" runat="server" Value="" />
        <asp:HiddenField ID="UI_txtApplicant_HiddenField" runat="server" Value="" />
        <asp:HiddenField ID="UI_txtTel_HiddenField" runat="server" Value="" />
        <asp:HiddenField ID="UI_txtMail_HiddenField" runat="server" Value="" />
        <asp:HiddenField ID="UI_txtAddress_HiddenField" runat="server" Value="" />
        <%--資料 結束--%>

        <div class="erma-window-background erma-window-newRequest-clientInfo-background" style="width: 100%; background: #fff; overflow: auto;">
            <div class="erma-window-box" style="width: 100%; background: #fff; margin: 0; box-sizing: border-box;">

                <h7>
                    <asp:Label ID="ClientInformationLab" runat="server" Text="Client Information" Visible="false"></asp:Label>
                    <asp:Label ID="RepairProductShippingInformationLab" runat="server" Text="維修物品寄送資訊"></asp:Label>
                </h7>
                <div class="erma-image-cancel" onclick="window.parent.Close_windows();">
                    <img src="../images_new/cancel.svg" />
                </div>

                <div class="erma-box-content">


                    <div class="erma-warranty-divtop">
                        <div class="erma-buttons-div erma-top-buttons-div">
                            <asp:Button ID="AddBtn" CssClass="EndUserbtn" runat="server" Text="Add new contact INFO" OnClientClick="ShowProgressBar();" />
                            <asp:Button ID="DelBtn" CssClass="DelEndUserbtn" runat="server" Text="Delete contact INFO" OnClientClick="ShowProgressBar();" />
                            <asp:HiddenField ID="HiddenField_UPDATA" runat="server" Value="" />
                        </div>
                    </div>

                    <div class="erma-warranty-divcontent">
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Label ID="UI_lblContactInfo" runat="server" Text="002_* Contact INFO" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <div class="erma-newRequest-combobox erma-combobox-control">
                                        <asp:DropDownList ID="Drop_SaveEndUser" runat="server" AutoPostBack="true" CssClass="erma-combobox-choose"></asp:DropDownList>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="UI_lblAccountID" runat="server" Text="003_* Account ID"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="UI_lblAccountIDText" runat="server"></asp:Label>
                                    <asp:TextBox runat="server" ID="UI_txtAccountIDText" Width="50px" Visible="false"></asp:TextBox>
                                    <asp:Button ID="UI_cmdCust_Search" runat="server" Text="016_Search" CssClass="Pick" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="UI_lblUserID" runat="server" Text="005_* User ID"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="UI_lblUserIDText" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="UI_lblAccountName" runat="server" Text="004_* Account Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="UI_lblAccountNameText" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="UI_lblRepairCenter" runat="server" Text="009_* Repair Center"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="UI_lblRepairCenterText" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="UI_lblRepairCenterValue" runat="server" Visible="false"></asp:Label>
                                    <asp:DropDownList ID="UI_cboRepairCenter" runat="server" Visible="false"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span style="color: red">*</span><asp:Label ID="UI_lblApplicant" runat="server" Text="006_* Applicant"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="UI_txtApplicant" runat="server" CssClass="erma-input-input"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span style="color: red">*</span><asp:Label ID="UI_lblTel" runat="server" Text="007_* Tel No."></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="UI_txtTel" runat="server" CssClass="erma-input-input" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td><span style="color: red">*</span><asp:Label ID="UI_lblMail" runat="server" Text="044_Mail"></asp:Label></td>
                                <td>
                                    <asp:TextBox ID="UI_txtMail" runat="server" CssClass="erma-input-input"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td><span style="color: red">*</span><asp:Label ID="UI_lblAddress" runat="server" Text="008_* Address"></asp:Label></td>
                                <td>
                                    <div class="erma-newRequest-combobox erma-combobox-control">



                                        <asp:TextBox ID="UI_txtAddress" runat="server" CssClass="erma-input-input"></asp:TextBox>
                                        <asp:Button ID="UI_cmdAdressPick" runat="server" Text="016_Pick" CssClass="btn btn-primary" Visible="false" />
                                    </div>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="UI_lblRemark" runat="server" Text="134_Remark"></asp:Label>:</td>
                            </tr>
                            <tr>
                                <td colspan="2" class="remark-text">
                                    <asp:TextBox runat="server" ID="UI_txtRemark" TextMode="MultiLine" Rows="4" Columns="50" MaxLength="1500" Style="resize: none;"></asp:TextBox>

                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div class="erma-checkbox-div">
                                        <span>
                                            <asp:CheckBox ID="UI_PartsRequest" runat="server" />
                                        </span>
                                        <span>
                                            <asp:Label ID="lb_PartsRequest" runat="server" Text="RMA Number for Parts Request Only"></asp:Label>
                                        </span>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="erma-buttons-div">
                    <%--<button class="erma-button-back" onclick="displayFun('.erma-window-newRequest-clientInfo-background','.erma-window-newRequest-addProduct-background')">Back</button>--%>
                    <%--     <asp:Button ID="Add_INFO_Btn" CssClass="EndUserbtn"  runat="server" Text="Add new contact INFO"  Width="180"  Height="40"  />--%>
                    <asp:Button ID="BackBtn" CssClass="erma-button-next-div" runat="server" Text="Back" Width="90" Height="40" />
                    <asp:Button ID="SubmitBtn" CssClass="erma-button-next-div-new" runat="server" Text="Submit" Width="90" Height="40" OnClientClick="if (!Checking_ProductInformation_04()) return false;" />

                </div>
            </div>

            <asp:Panel ID="UI_dvRMADetail_Panel" runat="server" Visible="false">
                <asp:GridView ID="UI_dvRMADetail" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="table" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                            <ItemTemplate>
                                <%--      <asp:Label ID="UI_SeqID" runat="server" Text='<%# Eval("SeqID") %>'></asp:Label>--%>
                                <asp:Label ID="UI_RMADID" runat="server" Text='<%# Eval("RMAD_ID") %>' Visible="false"></asp:Label>
                                <asp:Label ID="UI_ISFILL" runat="server" Text='<%# Eval("RMAD_ISFILL") %>' Visible="false"></asp:Label>
                                <asp:Label ID="UI_RMADWARRANTY" runat="server" Text='<%# Eval("RMAD_WARRANTY") %>' Visible="false"></asp:Label>
                                <asp:Label ID="UI_RMADCSTMP" runat="server" Text='<%# Eval("RMAD_CSTMP") %>' Visible="false"></asp:Label>
                                <%--    <asp:Label ID="UI_CWEndWarr" runat="server" Text='<%# Eval("CWEndWarr")%>' Visible="false"></asp:Label>--%>
                                <asp:Label ID="UI_RMAD_MODELNO" runat="server" Text='<%# Eval("RMAD_MODELNO")%>' Visible="false"></asp:Label>
                                <asp:Label ID="UI_RMAD_SERIALNO" runat="server" Text='<%# Eval("RMAD_SERIALNO")%>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Part Number" SortExpression="RMAD_PARTSN">
                            <HeaderStyle Width="15%" CssClass="text9pt" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="UI_PARTNUMBER" runat="server" Text='<%# Eval("RMAD_PARTSN") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="013_Serial" SortExpression="RMAD_SERIALNO">
                            <HeaderStyle Width="15%" CssClass="text9pt" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="UI_SERIALNO" ReadOnly="true" runat="server" Text='<%# Eval("RMAD_SERIALNO") %>' Width="130"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="RMAD_MODELNO" HeaderText="012_Model No" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                        <asp:BoundField DataField="RMAD_PRODUCTDESC" HeaderText="196_PRODUCT DESC" Visible="false" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>

                        <asp:TemplateField HeaderText="EW Warranty">
                            <HeaderStyle Width="10%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="UI_WARRANTY" Text='<%# Eval("RMAD_sWARRANTY") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--          <asp:BoundField DataField="CWEndWarr" HeaderText="CW Warranty">
                                                    <HeaderStyle Width="10%" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>--%>
                        <asp:TemplateField HeaderText="Warranty Detail">
                            <HeaderStyle Width="10%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton ID="UI_cmdWarrDetail" runat="server" ImageUrl="images/icon-detail.gif" CommandName="cmdWarrDetail" CommandArgument='<%# me.UI_dvRMADetail.Rows.Count%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="016_Problem">
                            <HeaderStyle Width="12%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                            <ItemTemplate>
                                <asp:Button ID="UI_cmdEdit" runat="server" Text="_Edit" CssClass="Problem_Edit" CommandName="cmdEdit" CommandArgument='<%# me.UI_dvRMADetail.Rows.Count%>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="017_Delete">
                            <HeaderStyle Width="12%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgDel" runat="server" ImageAlign="AbsBottom" ImageUrl="images/xx.gif" CommandName="cmdDel" OnClientClick="return FrmDelete()" CommandArgument='<%#Me.UI_dvRMADetail.Rows.Count%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="Text_Head" />
                    <RowStyle CssClass="TR_1" />
                    <AlternatingRowStyle CssClass="ListRowEven" />
                    <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Center" />
                </asp:GridView>

            </asp:Panel>

            <asp:Label ID="UI_lblPreviousPage_RMAID" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblPreviousPage_RMANO" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_RMA_STATUS" runat="server" Visible="false"></asp:Label>
            <asp:Panel ID="Panel1" runat="server" Visible="false">
                <%--<CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />--%>
            </asp:Panel>
            <uc4:ucPopProblem ID="UcPopProblem" runat="server" />
            <uc3:ucMessage ID="ucMessage" runat="server" />
            <uc5:ucModel ID="ucModel" runat="server" />
            <uc6:ucCustAddress ID="ucCustAddress" runat="server" />
            <uc7:ucCustomer_pick ID="ucCustomer_pick" runat="server" />
            <uc8:ucWarrantyPartsView ID="UcWarrantyPartsView" runat="server" />
            <uc1:uc_Wait runat="server" ID="uc_Wait" />
    </form>

</body>
<script>
    if ($(window).height() <= 768) {
        $(".erma-warranty-divcontent").css({ 'height': '66vh', 'overflow': 'auto' });
        //$("#ProductInformation_03_iframe").offset({ top: myTop, left: myLeft });
    }
</script>

</html>
