<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ProductInformation_01.aspx.vb" Inherits="ProductInformation_01" %>

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
    <script src="/script/jsUpdateProgress.js"></script>

    <script type="text/javascript">

</script>

    <style>
        u {
            font-size: 8.0pt;
        }

        .SpellE {
            font-size: 8.0pt;
            font-family: 新細明體;
        }

        .GramE {
            font-size: 8.0pt;
            font-family: 新細明體;
        }

        .Product_Information_Lab {
            display: block;
            font-size: 0.83em;
            margin-block-start: 1.67em;
            margin-block-end: 1.67em;
            margin-inline-start: 0px;
            margin-inline-end: 0px;
            font-weight: bold;
        }
    </style>
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
            right: -80px;
            top: 5px;
            cursor: pointer;
        }

        .erma-number-box #Sample_hyperlink {
            color: #496FF2;
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

        .erma-buttons-Agree {
            border: 1px solid #000000;
            background: #fff;
            color: #000000;
            border-radius: 5px;
            margin-left: 5px;
            width: 100px;
            height: 35px;
        }

        .erma-buttons-table {
            position: absolute;
            right: 2%;
            bottom: 2.5%;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <asp:Panel ID="Panel1" runat="server" Visible="false">

            <div style="height: 480px; overflow: auto; width: 100%; overflow-x: scroll; scroll-padding-bottom: 10px; padding: 25px; box-sizing: border-box;">
                <asp:Label ID="UI_lblPolicy" runat="server" CssClass="menuframe"></asp:Label>
            </div>

            <br />
            <center class="erma-buttons-center">
                <table class="erma-buttons-table">
                    <tr>
                        <td>
                            <asp:Button ID="UI_cmdDisagree" runat="server" Text="_Disagree" CssClass="erma-buttons-Agree" OnClientClick="window.parent.Close_windows();" />
                        </td>
                        <td>
                            <asp:Button ID="UI_cmdAgree" runat="server" Text="_Agree" CssClass="erma-buttons-Agree erma-button-Agree" />
                        </td>
                    </tr>
                </table>
            </center>
        </asp:Panel>

        <asp:Panel ID="Panel2" runat="server" Visible="false">
            <div class="erma-window-background erma-window-newRequest-productInfo-background">
                <div class="erma-window-box" style="width: 100%; height: 100%;">
                    <div class="erma-box-div">
                        <h5>
                            <asp:Label ID="Product_Information_Lab" runat="server" Text="Product Information" Font-Size="Large"></asp:Label></h5>

                        <p>
                            <asp:Label ID="ContextLab" runat="server" Text="Please pick a model you are going to request repair or key in barcode number."></asp:Label>
                        </p>

                        <div class="erma-image-cancel" onclick="window.parent.Close_windows();">
                            <img src="../images_new/cancel.svg" />
                        </div>
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
                                                <div id="RMA_Server" class="erma-repairStatus-searchResult" style="color: blue; list-style: initial;"></div>
                                            </td>
                                        </tr>
                                    </table>

                                    <div class="erma-info-uploadFile-message" style="position: absolute; top: 30px; left: 100px;">
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

                    <asp:Button ID="AddBtn" CssClass="erma-infomation-addButton" runat="server" Text="Add" OnClientClick="ShowProgressBar();" />
                    <uc3:ucMessage ID="ucMessage" runat="server" />
                    <script>
                        var inputFile = document.getElementById('<%= html_FileUpload.ClientID %>');
                        inputFile.addEventListener('change', function (event) {

                            var fileData = event.target.files[0]; // 檔案資訊
                            var finalName = event.target.files[0].name;
                            $('#RMA_Server').html(finalName);
                        });

                    </script>
                </div>
            </div>
            <uc1:uc_Wait runat="server" ID="uc_Wait" />
        </asp:Panel>
    </form>

</body>
</html>
