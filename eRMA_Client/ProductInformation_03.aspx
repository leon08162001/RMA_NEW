<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ProductInformation_03.aspx.vb" Inherits="ProductInformation_03" %>

<%@ Register Src="ascx/ucProgressStatus.ascx" TagName="ucProgressStatus" TagPrefix="uc1" %>
<%@ Register Src="ascx/ucMessageSave.ascx" TagName="ucMessageSave" TagPrefix="uc3" %>
<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc3" %>
<%@ Register Src="ascx/ucMessageOpen.ascx" TagName="ucMessageOpen" TagPrefix="uc3" %>
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

    <style>
        * {
            font-size: 14px;
        }

        .erma-table-components-list {
            border: 0px solid #ffffff;
            width: 100%;
            height: 100%;
            overflow: auto;
        }

        .erma-download-btn {
            border: 1px solid #000000;
            color: #000000;
            background: #ffffff;
            padding: 9px 25px;
            border-radius: 5px;
            display: table-cell;
        }


        .Head_UI {
            z-index: -50000;
        }

        .Add_btn {
            border-radius: 20px;
            border: 1px solid #496FF2;
            color: #496FF2;
            padding: 6px 18px;
            background: #fff;
            margin-top: 15px;
            margin-right: 1.5%;
            cursor: pointer;
        }

        .modalBackground {
            background-color: black;
            opacity: 0.5;
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


        .Problem_Edit {
            background: #F1F3FF;
            color: #5E7FF4;
            font-size: 15px;
            border: 0px solid #F1F3FF;
        }


        .erma-button-back-003 {
            border: 1px solid #000000;
            background: #fff;
            color: #000000;
            border-radius: 5px;
            margin-left: 5px;
            width: 125px;
            height: 40px;
        }


        .ProductInformation_03-cancel {
            float: right;
        }

        .erma-button-next-div-new {
            border: 1px solid #496FF2;
            background: #496FF2;
            color: #ffffff;
            border-radius: 5px;
            margin-left: 5px;
            cursor: pointer;
        }

        .erma-button-cancel-div,
        .erma-button-next-div,
        .erma-button-save-div {
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

        .erma-info-uploadFile-message-001 {
            position: fixed;
            background: #fff;
            box-shadow: 0 0 5px 0 #47474761;
            padding: 15px 30px;
            border-radius: 10px;
            z-index: 100;
            display: none;
        }

        .erma-title:hover .erma-info-uploadFile-message-001 {
            display: block;
        }

        .erma-title img {
            width: 15px;
            ;
            height: 15px;
            ;
        }


        .erma-image-cancel-001 {
            width: 16px;
            padding: 5px;
            position: absolute;
            display: table;
            right: 10px;
            top: 5px;
            cursor: pointer;
        }

        .erma-problem-content {
            padding: 0 7%;
        }

        .erma-number-box {
            width: 100%;
        }
    </style>

    <script>
        function SavedBtn_Alert() {
            $find("Alert_Save").show();
            return false;
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

    <script type="text/javascript">
        function btnDownload(url, filename) {
            var a = document.createElement('a');
            a.href = url;
            a.download = filename;
            a.click();
            window.URL.revokeObjectURL(url);
        }
    </script>

    <script type="text/javascript">
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

        function Clean_SerialNumberTxt() {

            $("#<%= SerialNumberTxt.ClientID %>").val("");
            $("#<%= UploadFileLabel.ClientID %>").val("");

        }

        function Show_Windows() {
            $find('UI_Add_panel_UI').show();
        }
        $("#ucMessageOpen_panelMessage").animate({ 'margin-left': '-190px', 'margin-top': '-200px', height: $(window).height() * 0.96, width: $(window).width() * 0.95 });
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <div class="erma-window-background erma-window-newRequest-addProduct-background" style="background: #fff;">
            <div class="erma-window-box" style="width: 100%; height: 100%; background: #fff;">
                <h7>
                    <asp:Label ID="Product_InformationLabel" runat="server" Text="" Font-Size=""></asp:Label>
                </h7>
                <div class="erma-image-cancel" onclick="window.parent.Close_windows();">
                    <img src="../images_new/cancel.svg" />
                </div>
                <div class="erma-addBar-div">
                    <asp:Button ID="Add_Btn" runat="server" Text="Add" CssClass="Add_btn" OnClientClick="Clean_SerialNumberTxt();" />

                </div>

                <%-- ListView 資料呈現後--%>

                <div class="erma-addList-box" style="overflow: auto;">
                    <ul class="erma-table-components-list">

                        <asp:ListView ID="UI_dvRMAListView" runat="server" DataKeyNames="SeqID,RMAD_ID,RMAD_SERIALNO">
                            <LayoutTemplate>
                                <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                            </LayoutTemplate>
                            <ItemTemplate>

                                <li runat="server">
                                    <div class="erma-list-left">
                                        <table width="100%">
                                            <tbody>
                                                <tr>
                                                    <td rowspan="2" class="erma-list-ranking">

                                                        <%#DataBinder.Eval(Container.DataItem, "SeqID_Look")%>
                                                        <asp:Panel ID="ViewPanel" runat="server" Visible="false">
                                                            <%#DataBinder.Eval(Container.DataItem, "RMAD_ID")%>
                                                            <%#DataBinder.Eval(Container.DataItem, "RMAD_ISFILL")%>
                                                            <%#DataBinder.Eval(Container.DataItem, "RMAD_WARRANTY")%>
                                                            <%#DataBinder.Eval(Container.DataItem, "RMAD_CSTMP")%>
                                                            <%#DataBinder.Eval(Container.DataItem, "CWEndWarr")%>
                                                            <%#DataBinder.Eval(Container.DataItem, "RMAD_MODELNO")%>
                                                            <%#DataBinder.Eval(Container.DataItem, "RMAD_SERIALNO")%>
                                                        </asp:Panel>
                                                    </td>
                                                    <td class="erma-list-key"><%= getoLanguage("RMA", "012") %></td>
                                                    <td class="erma-list-key"><%= getoLanguage("RMA", "098") %></td>
                                                    <%--   
            <td class="erma-list-key"><%= getoLanguage("RMA2", "105") %></td>
            <td class="erma-list-key"><%= getoLanguage("RMA2", "104") %></td>
                                                    --%>
                                                </tr>
                                                <tr>
                                                    <td class="erma-list-value">
                                                        <%# Eval("RMAD_MODELNO") %> 

                                                    </td>
                                                    <td class="erma-list-value">
                                                        <%# Eval("RMAD_SERIALNO") %> 
                                                    </td>
                                                    <%--     
            <td class="erma-list-value">
            <%# Eval("WarrantyType") %> 
            </td>
                  <td class="erma-list-value">
            <%# Eval("WarrantyDate") %> 
            </td>
                                                    --%>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <div class="erma-list-right">
                                        <table width="100%">
                                            <tbody>
                                                <tr>
                                                    <td class="erma-list-key"><%= getoLanguageword("Transfer", "029") %></td>
                                                    <td class="erma-list-key"><%= getoLanguageword("Transfer", "030") %></td>


                                                    <td class="erma-list-key erma-list-warrantyDetail">
                                                        <a href="#">
                                                            <span style="z-index: -50000;">
                                                                <%# Eval("Warranty_Detail") %> 
                                                            </span>
                                                        </a>
                                                        <div class="erma-warrantyDetail-content" style="display: none; z-index: 50000;">
                                                            <h5><%= getoLanguageword("Transfer", "036") %> <%# Eval("wati_ver") %></h5>
                                                            <table cellpadding="0" cellspacing="0">
                                                                <thead>
                                                                    <tr>
                                                                        <td>＃</td>
                                                                        <td><%= getoLanguageword("Transfer", "004") %></td>
                                                                        <td><%= getoLanguageword("Transfer", "005") %></td>
                                                                        <td><%= getoLanguageword("Transfer", "006") %></td>
                                                                        <td><%= getoLanguageword("Transfer", "007") %></td>
                                                                        <td><%= getoLanguageword("Transfer", "008") %></td>
                                                                        <td><%= getoLanguageword("Transfer", "009") %></td>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>

                                                                    <%# Eval("Warranty_Detail_Context") %>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </td>
                                                    <td rowspan="2" class="erma-list-delete">
                                                        <asp:ImageButton ID="imgDel" runat="server" ImageAlign="AbsBottom" ImageUrl="../images_new/delete.svg" CommandName="cmdDel" OnClientClick="return ShowProgressBar();; FrmDelete();" CommandArgument='<%# UI_dvRMAListView.Items.Count%>' />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="erma-list-value">
                                                        <%# Eval("RMAD_sWARRANTY") %>
                                                    </td>
                                                    <td class="erma-list-value">
                                                        <%# Eval("CWEndWarr") %>
                                                    </td>
                                                    <td class="erma-list-value">

                                                        <asp:Button ID="UI_cmdEdit" runat="server" Text='<%# Eval("CmdEdit") %>' CssClass="Problem_Edit" CommandName="cmdEdit" CommandArgument='<%# UI_dvRMAListView.Items.Count%>' />
                                                        <%-- <a href="javascript:;" class="erma-list-edit" onclick="displayFun('.erma-window-newRequest-problem-background')">Edit</a>--%>
                                                    </td>

                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </li>

                            </ItemTemplate>
                        </asp:ListView>



                    </ul>

                </div>

                <%-- ListView 資料呈現後--%>

                <div class="erma-buttons-div">
                    <asp:Button ID="CancelBtn" CssClass="erma-button-cancel-div" runat="server" Text="Cancel" Width="90" Height="40" />
                    <asp:Button ID="SavedBtn" CssClass="erma-button-save-div" runat="server" Text="Save" Width="90" Height="40" />
                    <asp:Button ID="AddBtn" CssClass="erma-button-next-div-new" runat="server" Text="Next" Width="90" Height="40" OnClientClick="ShowProgressBar();" />
                </div>
            </div>
        </div>

        <asp:HiddenField ID="UI_lblRepairCenterValue_View" runat="server" Value="" />
        <asp:Label ID="UI_txtSerialParts" Visible="false" runat="server" class="default" Text=""></asp:Label>
        <uc3:ucMessage ID="ucMessage" runat="server" />
        <uc3:ucMessageSave ID="ucMessageSave" runat="server" />
        <uc4:ucPopProblem ID="UcPopProblem" runat="server" />

        <%--這邊顯示要跳出框 開始--%>
        <div style="visibility: hidden;">
            <asp:Button ID="Deatail_Btn" runat="server" Text="XXXXXXXX" />
        </div>
        <%--存檔 開始--%>
        <!-- 存檔 STRAT -->
        <asp:Panel ID="Save_panelMessage" runat="server" Style="display: none; position: absolute;">
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
                    }
            </style>
            <table style="background: #fff; border-radius: 10px; width: 400px; height: 380px; border: none; padding: 10px; border: 1px solid black;">
                <tr>
                    <td>
                        <div class="message-div-class">
                            <center>
                                <img src="../images_new/notice.svg" />
                                <br />

                                <p style="font-size: 15px; font-weight: bold;">
                                    <!-- <asp:Label ID="Save_lblTitleMsg" runat ="server" Text="訊息提示"></asp:Label> -->
                                </p>
                            </center>
                            <br />
                            <center>
                                <asp:Label ID="Save_html_Success" CssClass="text12pt" runat="server" Text="" Font-Bold="true" Font-Names="Arial"></asp:Label>
                                <asp:Label ID="Save_html_Failed" CssClass="text12pt" runat="server" ForeColor="#ff5c5c" Text="" Font-Bold="true" Font-Names="Arial"></asp:Label>

                            </center>
                            <center class="UI_button">
                                <asp:Panel runat="server" id="Save_Panel_OK">
                                    <asp:Button ID="UI_butOK" runat="server" Text="014_OK" CssClass="AlertBtn" CausesValidation="false" OnClientClick="ShowProgressBar();" />
                                </asp:Panel>

                            </center>
                            <br />


                        </div>
                    </td>
                </tr>


            </table>

        </asp:Panel>
        <!-- 存檔 END -->

        <ajaxToolkit:ModalPopupExtender ID="SaveModalMessage" TargetControlID="SavedBtn"
            PopupControlID="Save_panelMessage" BackgroundCssClass="Modal" runat="server" DropShadow="false">
        </ajaxToolkit:ModalPopupExtender>
        <%--存檔 結束--%>

        <%--新增序號 開始--%>
        <asp:Panel ID="UI_Add_panel" runat="server" CssClass="ucSubstitute_div" Style="display: none; position: absolute; width: 940px; height: 550px;">
            <fieldset id="UI_Add_RMA_panel_iframe" class="UI_Add_RMA_panel_fieldset" style="width: 100%; height: 100%; border: none;">
                <div class="erma-window-box">
                    <uc3:ucMessageOpen ID="ucMessageOpen" runat="server" />
                    <div class="erma-box-div" style="text-align: center; padding-top: -5px;">
                        <h5>
                            <asp:Label ID="Product_Information_Lab" runat="server" Text="Product Information" Font-Size="Large"></asp:Label></h5>
                        <p>
                            <asp:Label ID="ContextLab" runat="server" Text="Please pick a model you are going to request repair or key in barcode number."></asp:Label>
                        </p>

                        <asp:ImageButton ID="UI_Add_panel_cmdCancel" runat="server" ImageUrl="../images_new/cancel.svg" CssClass="erma-image-cancel" />
                    </div>
                    <div class="erma-number-input erma-serialNumber-input">
                        <div class="erma-number-box">
                            *<asp:Label ID="SerialLab" runat="server" Text="Serial Number"></asp:Label>
                            <asp:TextBox ID="SerialNumberTxt" runat="server" CssClass="erma-input-input" placeholder="Please enter RMA Number"></asp:TextBox>
                        </div>
                    </div>
                    <hr />
                    <div class="erma-number-input erma-uploadFile-input erma-number-undefined">
                        <div class="erma-number-box">
                            <div class="erma-title">
                                <asp:Label ID="UploadFileLabel" runat="server" Text=""></asp:Label>
                                <span>

                                    <table>
                                        <tr>
                                            <td>
                                                <img src="../images_new/info.svg" style="list-style: initial;" />
                                            </td>
                                            <td>
                                                <div id="RMA_Server" class="erma-repairStatus-searchResult" style="color: blue;"></div>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="erma-info-uploadFile-message" style="position: absolute; top: 30px;">
                                        <ul>
                                            <li>
                                                <asp:Label ID="Upload_File_SizeLabel" runat="server" Text=""></asp:Label>
                                            </li>
                                            <li>
                                                <asp:Label ID="Upload_File_CSV_SizeLabel" runat="server" Text=""></asp:Label>
                                            </li>
                                        </ul>
                                    </div>
                                </span>
                                <asp:HyperLink ID="Sample_hyperlink" runat="server" NavigateUrl="https://e-rma.cipherlab.com.tw/object/Sample/001/Sample.csv" Style="float: right;" Text="Download Sample" download="Sample.csv"></asp:HyperLink>
                            </div>
                            <div class="erma-uploadFile-selectFileDiv" style="width: 100%;">
                                <label class="file-upload" style="width: 100%;">
                                    <asp:Label ID="SelectFileLabe" runat="server" Text="" Height="12" Style="display: table;"></asp:Label>
                                    <asp:FileUpload ID="html_FileUpload" runat="server" Width="100%"></asp:FileUpload>
                                </label>
                            </div>
                        </div>
                    </div>

                    <script>

                        var inputFile = document.getElementById('<%= html_FileUpload.ClientID %>');
                        inputFile.addEventListener('change', function (event) {

                            var fileData = event.target.files[0]; // 檔案資訊
                            var finalName = event.target.files[0].name;
                            $('#RMA_Server').html(finalName);
                        });

                    </script>
                    <%--<button class="erma-infomation-notButton" style="display:block";>Add</button>--%>
                    <asp:Button ID="Add_Rma_Btn" CssClass="erma-infomation-addButton" runat="server" Text="Add" OnClientClick="ShowProgressBar();" />
                </div>
                <div style="visibility: hidden;">
                    <asp:Button ID="UI_Add_cmdCancel" runat="server" Text="Button" />
                </div>
            </fieldset>
        </asp:Panel>

        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" TargetControlID="Add_Btn"
            BehaviorID="UI_Add_panel_UI"
            PopupControlID="UI_Add_panel"
            CancelControlID="UI_Add_panel_cmdCancel"
            BackgroundCssClass="modalBackground"
            runat="server">
        </ajaxToolkit:ModalPopupExtender>


        <%--新增序號 結束--%>

        <%--新增RMA維修明細 開始--%>
        <asp:Panel ID="UI_Endit_panel" runat="server">
            <fieldset style="background: #ffffff; width: 940px; height: 550px; border-radius: 10px;">
                <div class="erma-window-box">

                    <div class="erma-box-div">
                        <h7>
                            <asp:Label ID="SpecificationProblemLab" runat="server" Text="Specification / Problem" Font-Size=""></asp:Label>
                        </h7>
                        <asp:ImageButton ID="UI_Endit_cmdCancel" runat="server" ImageUrl="../images_new/cancel.svg" CssClass="erma-image-cancel-001" />

                    </div>
                    <div class="erma-problem-content">
                        <div class="erma-fault-block">

                            <div class="erma-fault-left">
                                <p class="erma-title">
                                    <asp:Label ID="UI_Serial_Number" runat="server" Text=""></asp:Label>:
                                </p>
                                <asp:Label ID="UI_txtSerial_UP" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="erma-fault-right">
                                <p class="erma-title" style="color: #ff0000;">
                                    <asp:Label ID="CustomerProductNumberLabel" runat="server" Text="Label"></asp:Label>:
                                </p>
                                <asp:TextBox ID="CUSTOMER_Txt" runat="server" CssClass="erma-input-input" placeholder="客戶產品編號(客戶自行輸入)" MaxLength="20"></asp:TextBox>
                            </div>


                        </div>
                        <div class="erma-fault-block">
                            <div class="erma-fault-left">
                                <p class="erma-title">
                                    <span style="color: red">*</span><asp:Label ID="FaultLabel" runat="server" Text="" Width="200"></asp:Label>
                                    <span style="color: red">*</span>&nbsp;<asp:Label ID="FaultDLab" runat="server" Text="明細資料"></asp:Label>
                                </p>


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
                                    <asp:Label ID="UI_Model_Lab" runat="server" Text=""></asp:Label>:
                                </p>
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
                                        <asp:Label ID="UploadTitleLabel" runat="server" Text=""></asp:Label>:</span>
                                    <span>
                                        <img src="../images_new/info.svg" /><asp:Label ID="UploadLabel" runat="server" Text="" ForeColor="Red" CssClass="UploadLabel-Text"></asp:Label>
                                        <div class="erma-info-uploadFile-message-001">
                                            <ul>
                                                <li style="display: none;">
                                                    <asp:Label ID="UI_Upload_CSV" runat="server" Text=""></asp:Label>

                                                </li>
                                                <li>
                                                    <asp:Label ID="UI_Upload_Size" runat="server" Text=""></asp:Label>
                                                </li>

                                            </ul>
                                        </div>
                                    </span>
                                </div>
                                <div class="erma-uploadFile-selectFileDiv">
                                    <div class="erma-uploadFile-left">
                                        <label class="file-upload" style="width: 98%;">
                                            <asp:Label ID="SelectFileLabel" runat="server" Text="" Height="12" Style="display: table;"></asp:Label>
                                            <asp:FileUpload ID="FileUpload1" runat="server" Height="0.1" multiple="multiple"></asp:FileUpload>
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
                                    <asp:HiddenField ID="CurrentDataKey_HiddenField" runat="server" Value="" />

                                    <asp:HiddenField ID="HiddenField_RMANO" runat="server" Value="" />
                                    <%-- 檔案暫存--%>
                                </div>
                            </div>
                        </div>
                        <div class="erma-problemDescription-block">
                            <div class="erma-description-block">
                                <p class="erma-title">
                                    <asp:Label ID="Problem_DescriptionLabel" runat="server" Text="Label"></asp:Label>:
                                </p>
                                </p>
                                    <asp:TextBox ID="Message_Box" TextMode="MultiLine" runat="server" mode="multiline" CssClass="erma-form-control" MaxLength="1200" lines="10" cols="10" Wrap="true" />
                            </div>
                        </div>
                        <asp:Button ID="ProductInformation_02_Btn" CssClass="erma-infomation-submitButton notButton" runat="server" Text="Submit" OnClientClick="if (!GetRegular()) return false;" />
                    </div>
                </div>

                <div style="visibility: hidden;">
                </div>
            </fieldset>
        </asp:Panel>
        <ajaxToolkit:ModalPopupExtender ID="ajModalProgress" TargetControlID="Deatail_Btn"
            PopupControlID="UI_Endit_panel"
            CancelControlID="UI_Endit_cmdCancel"
            BackgroundCssClass="modalBackground"
            runat="server">
        </ajaxToolkit:ModalPopupExtender>
        <%--新增RMA維修明細 結束--%>

        <%--您的這個動作，這筆維修單會被刪除取消 開始--%>
        <asp:Panel ID="UI_Add_panel_understand" runat="server" CssClass="ucSubstitute_div" Style="display: none; position: absolute;">

            <table style="background: #fff; border-radius: 10px; width: 400px; height: 380px; padding: 20px;">
                <tr>
                    <td>
                        <center>
                            <img src="../images_new/notice.svg" />
                            <br />
                            <br />
                            <p style="font-size: 15px; font-weight: bold;">
                                <asp:Label ID="ModalPopupExtender_understand_Label" runat="server" Text=""></asp:Label>
                            </p>
                        </center>
                        <tr>
                            <td>

                                <center>
                                    <button class="erma-button-back-003" onclick="displayFun('.erma-window-newRequest-cancel-background')"><%= getoLanguageCommand("Common", "070") %></button>

                                    <asp:Button ID="UI_cmdCancel" runat="server" Text="_Cancel" CssClass="erma-button-back-003" OnClientClick="window.parent.Close_windows();" />

                                </center>
                            </td>
                        </tr>
            </table>

        </asp:Panel>
        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender_understand" TargetControlID="CancelBtn"
            PopupControlID="UI_Add_panel_understand"
            CancelControlID="UI_Add_cmdCancel"
            BackgroundCssClass="modalBackground"
            runat="server">
        </ajaxToolkit:ModalPopupExtender>
        <%--您的這個動作，這筆維修單會被刪除取消 結束--%>

        <%--Saved 開始--%>
        <asp:Button ID="UI_Add_panel_Saved_btn" runat="server" Text="" />
        <asp:Panel ID="UI_Add_panel_Saved" runat="server" CssClass="ucSubstitute_div" Style="display: none; position: absolute;">
            <table style="background: #fff; border-radius: 5px; width: 41vh; height: 280px; border: 1px solid black;">
                <tr>
                    <td>
                        <center>

                            <asp:ImageButton ID="SaveImageBtn" runat="server" ImageUrl="../images_new/save.svg" OnClientClick="alet('TEST');" />
                            <br />
                            <br />
                            <p style="font-size: 15px; font-weight: bold;">
                                Saved
                            </p>
                        </center>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender_Saved" TargetControlID="UI_Add_panel_Saved_btn"
            PopupControlID="UI_Add_panel_Saved"
            CancelControlID="UI_Add_cmdCancel"
            BackgroundCssClass="modalBackground"
            runat="server">
        </ajaxToolkit:ModalPopupExtender>
        <%--Saved 結束--%>

        <%--這邊顯示要跳出框 結束--%>
        <div style="visibility: hidden">
            <asp:GridView ID="UI_dvRMADetail" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="table" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric">
                <Columns>
                    <asp:TemplateField>
                        <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                        <ItemTemplate>
                            <asp:Label ID="UI_SeqID" runat="server" Text='<%# Eval("SeqID") %>'></asp:Label>
                            <asp:Label ID="UI_RMADID" runat="server" Text='<%# Eval("RMAD_ID") %>' Visible="false"></asp:Label>
                            <asp:Label ID="UI_ISFILL" runat="server" Text='<%# Eval("RMAD_ISFILL") %>' Visible="false"></asp:Label>
                            <asp:Label ID="UI_RMADWARRANTY" runat="server" Text='<%# Eval("RMAD_WARRANTY") %>' Visible="false"></asp:Label>
                            <asp:Label ID="UI_RMADCSTMP" runat="server" Text='<%# Eval("RMAD_CSTMP") %>' Visible="false"></asp:Label>
                            <asp:Label ID="UI_CWEndWarr" runat="server" Text='<%# Eval("CWEndWarr")%>' Visible="false"></asp:Label>
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
                        <HeaderStyle Width="10%" HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="UI_WARRANTY" Text='<%# Eval("RMAD_sWARRANTY") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CWEndWarr" HeaderText="CW Warranty">
                        <HeaderStyle Width="10%" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:TemplateField>
                        <HeaderStyle Width="10%" CssClass="Head_UI" HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                        <ItemTemplate>
                            <asp:ImageButton ID="UI_cmdWarrDetail" runat="server" ImageUrl="images/icon-detail.gif" CommandName="cmdWarrDetail" CommandArgument='<%#Me.UI_dvRMADetail.Rows.Count%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="016_Problem">
                        <HeaderStyle Width="12%" HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                        <ItemTemplate>
                            <asp:Button ID="UI_cmdEdit" runat="server" Text="_Edit" CssClass="Problem_Edit" CommandName="cmdEdit" CommandArgument='<%#Me.UI_dvRMADetail.Rows.Count%>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="017_Delete">
                        <HeaderStyle Width="12%" HorizontalAlign="Center"></HeaderStyle>
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

            <asp:Label ID="UI_lblPreviousPage_RMAID" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_lblPreviousPage_RMANO" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UI_RMA_STATUS" runat="server" Visible="false"></asp:Label>
        </div>

        <uc1:uc_Wait runat="server" ID="uc_Wait" />
    </form>
</body>
</html>
