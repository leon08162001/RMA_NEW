<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ProductInformation_02.aspx.vb" Inherits="ProductInformation_02" %>

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

    <style id="inert-style">
        * {
            font-size: 14px;
        }

        .file-upload {
            display: inline-block;
            overflow: hidden;
            text-align: center;
            vertical-align: middle;
            font-family: Arial;
            border: 1px solid #000000;
            background: #fff;
            color: #000000;
            border-radius: 6px;
            -moz-border-radius: 6px;
            cursor: pointer;
            -webkit-border-radius: 6px;
        }

        .UploadLabel-Text {
            overflow: hidden;
            display: -webkit-box;
            -webkit-line-clamp: 2;
            // 行數 -webkit-box-orient: vertical;
            white-space: normal;
            width: 400px;
            overflow: hidden;
        }

        .file-upload:hover {
        }

        /* The button size */
        .file-upload {
            height: 35px;
            position: relative;
        }

            .file-upload input {
                top: 0;
                left: 0;
                margin: 0;
                font-size: 11px;
                font-weight: bold;
                /* Loses tab index in webkit if width is set to 0 */
                opacity: 0;
                filter: alpha(opacity=0);
                width: 100%;
                height: 100%;
                cursor: pointer;
            }

            .file-upload strong {
                font: normal 12px Tahoma,sans-serif;
                text-align: center;
                vertical-align: middle;
            }

            .file-upload span {
                position: absolute;
                right: 0;
                left: 0;
                top: 0;
                bottom: 0;
                margin: auto;
                display: table;
            }
    </style>
    <script type="text/javascript">
     
    <!--
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
     //-->
    </script>
    <script>
        function btnDownload(url, filename) {
            var a = document.createElement('a');
            a.href = url;
            a.download = filename;
            a.click();
            window.URL.revokeObjectURL(url);
        }
    </script>
    <script>
        function GetRegular() {
            var UI_cboFailureClass_Select = document.getElementById('<%= UI_cboFailureClass.ClientID %>');
        var UI_cboFailure_Select = document.getElementById('<%= UI_cboFailure.ClientID %>');

        var UI_cboFailureClass_index = UI_cboFailureClass_Select.selectedIndex;
        var UI_cboFailure_Select_index = UI_cboFailure_Select.selectedIndex;

        var art = "";
        if (UI_cboFailureClass_index == 0) {
            art += "<%= getoLanguage("RMA2", "091") & "\r\n"  %>";
        }

        if (UI_cboFailure_Select_index == 0) {
            art += "<%= getoLanguage("RMA2", "092") & "\r\n" %>";
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

</head>
<body>
    <form id="form1" runat="server">

        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:HiddenField ID="SerialNumberHiddenField" runat="server" Value="" />

        <div class="erma-window-background erma-window-newRequest-problem-background">
            <div class="erma-window-box" style="width: 100%; height: 100%;">


                <div class="erma-problem-content">


                    <div class="erma-fault-block" style="width: 100%; padding-top: 5px;">
                        <div class="erma-fault-left" style="left: 15px;">
                            <asp:Label ID="SpecificationProblemLab" runat="server" Text="Specification / Problem" Font-Size=""></asp:Label>

                        </div>
                        <div class="erma-fault-right">
                            <div onclick="window.parent.Close_windows();" class="erma-image-cancel" style="position: absolute; right: 15px;">
                                <img class="erma-cancel-img" src="../images_new/cancel.svg" />
                            </div>
                        </div>
                    </div>

                    <div class="erma-Problem-box">
                        <div class="erma-number-input erma-serialNumber-input">
                            <div style="display: inline-block; width: 75%;">
                                <p class="erma-title">
                                    <asp:Label ID="UI_Serial_Number" runat="server" Text=""></asp:Label>
                                    :
                                </p>
                                <asp:Label ID="UI_txtSerial" runat="server" Text=""></asp:Label>
                            </div>

                            <div style="display: inline-block; width: 25%;">
                                <p class="erma-title" style="color: #ff0000;">
                                    <asp:Label ID="CustomerProductNumberLab" runat="server" Text=""></asp:Label>:</p>
                                <asp:TextBox ID="CUSTOMER_Txt" runat="server" CssClass="erma-input-input" placeholder="客戶產品編號(客戶自行輸入)" MaxLength="20"></asp:TextBox>
                            </div>
                        </div>
                        <div class="erma-fault-block">
                            <div class="erma-fault-left">
                                <p class="erma-title"><span style="color: red">*</span>
                                    <asp:Label ID="FaultLab" runat="server" Text="" Width="200"></asp:Label>
                                    <span style="color: red">*</span>&nbsp;<asp:Label ID="FaultDLab" runat="server" Text="明細資料"></asp:Label></p>


                                <div class="erma-number-input erma-fault-option">
                                    <div class="erma-fault erma-combobox-control">

                                        <asp:DropDownList ID="UI_cboFailureClass" CssClass="erma-combobox-choose" runat="server" AutoPostBack="true">
                                        </asp:DropDownList>

                                    </div>
                                    <div class="erma-fault erma-combobox-control">

                                        <asp:DropDownList ID="UI_cboFailure" CssClass="erma-combobox-choose" runat="server">
                                        </asp:DropDownList>

                                    </div>
                                </div>
                            </div>
                            <div class="erma-fault-right">
                                <p class="erma-title">
                                    <asp:Label ID="UI_Model_Lab" runat="server" Text=""></asp:Label>:</p>
                                <div class="erma-modelNo-option">
                                    <div class="erma-modelNo erma-combobox-control">

                                        <asp:DropDownList runat="server" ID="UI_cboModel" CssClass="erma-combobox-choose"></asp:DropDownList>

                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="erma-number-input erma-uploadFile-input erma-number-undefined">
                            <div class="erma-number-box">
                                <div class="erma-title">
                                    <span>
                                        <asp:Label ID="UploadFileLab" runat="server" Text=""></asp:Label>:</span>
                                    <span>
                                        <img src="../images_new/info.svg" /><asp:Label ID="UploadLabel" runat="server" Text="" ForeColor="Red" CssClass="UploadLabel-Text"></asp:Label>
                                        <div class="erma-info-uploadFile-message">
                                            <ul>
                                                <li>
                                                    <asp:Label ID="UploadFileSizeLab" runat="server" Text=""></asp:Label>
                                                </li>
                                            </ul>
                                        </div>
                                    </span>
                                </div>
                                <div class="erma-uploadFile-selectFileDiv">

                                    <div class="erma-uploadFile-left">
                                        <label class="file-upload" style="width: 98%;">
                                            <asp:Label ID="Select_FileLab" runat="server" Text="" Height="12" Style="display: table;"></asp:Label>

                                            <asp:FileUpload ID="FileUpload1" runat="server" multiple="multiple"></asp:FileUpload>

                                            <asp:Label runat="server" Text="" ID="CW_Show_OP"></asp:Label>
                                        </label>
                                    </div>
                                    <div class="erma-uploadFile-right">
                                        <asp:Button ID="UI_cmdFileAdd" runat="server" Text="_FileAdd" CssClass="erma-upload-button" ValidationGroup="vsFileUpLoad" />
                                        <asp:Button ID="clear_btn" runat="server" Text="Clear" CssClass="erma-upload-clear" ValidationGroup="vsFileUpLoadClear" />
                                        <asp:Button ID="download_btn" runat="server" Text="Download" CssClass="erma-download-btn" ValidationGroup="vsFileUpLoadDownload" Visible="false" />
                                    </div>

                                    <%-- 檔案暫存--%>
                                    <asp:HiddenField ID="UI_WEBURL" runat="server" Value="" />
                                    <asp:HiddenField ID="UI_VisualPath" runat="server" Value="" />
                                    <asp:HiddenField ID="html_File_" runat="server" Value="" />
                                    <asp:HiddenField ID="File_HiddenField" runat="server" Value="" />
                                    <%-- 檔案暫存--%>
                                </div>
                            </div>
                        </div>

                        <div style="visibility: hidden;">
                            <asp:TextBox runat="server" ID="html_File" Visible="false"></asp:TextBox>
                            <asp:TextBox runat="server" ID="html_FullFile" Visible="false"></asp:TextBox>
                            <asp:TextBox runat="server" ID="html_FilePath" Visible="false"></asp:TextBox>
                            <asp:Label runat="server" ID="lblScript"></asp:Label>

                        </div>

                        <div class="erma-problemDescription-block">
                            <div class="erma-description-block">
                                <p class="erma-title">
                                    <asp:Label ID="ProblemDescriptionLabe" runat="server" Text=""></asp:Label>:</p>

                                <asp:TextBox ID="Message_Box" TextMode="MultiLine" runat="server" mode="multiline" CssClass="erma-form-control" MaxLength="1200" lines="10" cols="10" Wrap="true" Style="resize: none;" />

                            </div>
                        </div>
                        <asp:Button ID="AddBtn" CssClass="erma-infomation-submitButton notButton" runat="server" Text="Submit" OnClientClick="if (!GetRegular()) return false;" />

                    </div>
                </div>
            </div>
        </div>

        <%--對話框元件--%>
        <uc3:ucMessage ID="ucMessage" runat="server" />

        <%--隱藏元件--%>
        <asp:Label ID="UI_lblRepairCenterText" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="UI_lblRepairCenterValue" runat="server" Visible="false"></asp:Label>
        <asp:DropDownList ID="UI_cboRepairCenter" runat="server" Visible="false"></asp:DropDownList>
        <asp:Label ID="UI_lblAccountIDText" runat="server" Visible="false"></asp:Label>
        <asp:TextBox runat="server" ID="UI_txtAccountIDText" Width="50px" Visible="false"></asp:TextBox>
        <asp:Label ID="UI_lblAccountNameText" runat="server" Visible="false"></asp:Label>
        <asp:TextBox ID="UI_txtApplicant" runat="server" CssClass="erma-input-input" Visible="false"></asp:TextBox>
        <asp:TextBox ID="UI_txtMail" runat="server" CssClass="erma-input-input" Visible="false"></asp:TextBox>
        <asp:Label ID="UI_lblUserIDText" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="UI_txtSerialParts" Visible="false" runat="server" class="table" Text=""></asp:Label>


        <uc1:uc_Wait runat="server" ID="uc_Wait" />
    </form>
</body>
</html>
