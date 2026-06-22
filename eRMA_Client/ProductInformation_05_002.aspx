<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ProductInformation_05_002.aspx.vb" Inherits="ProductInformation_05_002" %>

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
        .btn_print {
            background: #1C218C;
            border: 1px solid #1C218C;
            border-radius: 20px;
            color: #fff;
        }

        .erma-close-button-001 {
            background: #ffffff;
            border: 1px solid #325CED;
            border-radius: 20px;
            color: #325CED;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <div class="erma-window-background erma-window-newRequest-printRepair-background">
            <div class="erma-window-box" style="width: 100%; height: 100%;">

                <div id="UI_dvTittle" runat="server" class="erma-note-serviceCenter">
                    <div onclick="window.parent.Close_windows();window.parent.Location_Reload_windows();" style="float: right;">
                        <img src="../images_new/cancel.svg" />
                    </div>
                    <div class="erma-serviceCenter-content">
                        <table>
                            <tr>
                                <td>
                                    <img src="../images_new/info-msg.svg" />
                                </td>
                                <td>
                                    <asp:Label ID="UI_Tittle" runat="server" Text="170_After save this form , Are you going to print out this form?."></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="erma-success-contacting-content">
                    <img src="../images_new/success-printRepair.svg" />
                    <h6>Success！</h6>
                    <p>Thanks you for contacting us</p>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="RMALabel" runat="server" Text="" Font-Size="12"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="RMA_ADDRESSLabel" runat="server" Text="" Font-Size="12"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center;">
                                <asp:Button ID="UI_cmdPrint" runat="server" Text="045_Print Now" CssClass="btn_print" Width="206" Height="36" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center;">
                                <asp:Button ID="UI_cmdClose" runat="server" Text="010_Cancel" CssClass="erma-close-button-001" Width="206" Height="36" OnClientClick="window.parent.Close_windows();window.parent.Location_Reload_windows();" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>

        <div style="visibility: hidden;">
            <%--<CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />--%>
            <asp:Label ID="UI_lblPreviousPage_RMAID" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblPreviousPage_ISCW" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_RedirectURL" runat="server" Visible="false"></asp:Label>
        </div>

    </form>
</body>
</html>
