<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPageRWD.master" AutoEventWireup="false" CodeFile="CheckingReport.aspx.vb" Inherits="CheckingReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <style id="inert-style">
        .file-upload {
            display: inline-block;
            overflow: hidden;
            text-align: center;
            vertical-align: middle;
            font-family: Arial;
            border: 1px solid #124d77;
            background: #fff;
            color: #000000;
            border-radius: 6px;
            -moz-border-radius: 6px;
            cursor: pointer;
            -webkit-border-radius: 6px;
        }

            .file-upload:hover {
                background: -webkit-gradient(linear, left top, left bottom, color-stop(0.05, #0061a7), color-stop(1, #007dc1));
                background: -moz-linear-gradient(top, #0061a7 5%, #007dc1 100%);
                background: -webkit-linear-gradient(top, #0061a7 5%, #007dc1 100%);
                background: -o-linear-gradient(top, #0061a7 5%, #007dc1 100%);
                background: -ms-linear-gradient(top, #0061a7 5%, #007dc1 100%);
                background: linear-gradient(to bottom, #0061a7 5%, #007dc1 100%);
                filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#0061a7', endColorstr='#007dc1',GradientType=0);
                background-color: #0061a7;
            }

        /* The button size */
        .file-upload {
            height: 35px;
        }

            .file-upload, .file-upload span {
                width: 400px;
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
                }

                .file-upload strong {
                    font: normal 12px Tahoma,sans-serif;
                    text-align: center;
                    vertical-align: middle;
                }

                .file-upload span {
                    top: 0;
                    left: 0;
                    display: inline-block;
                    /* Adjust button text vertical alignment */
                    padding-top: 5px;
                }

        .erma-download-btn {
            border: 1px solid #000000;
            color: #000000;
            background: #ffffff;
            padding: 9px 25px;
            border-radius: 5px;
            display: table-cell;
        }
    </style>

    <div class="container-fluid p-5 bg-primary text-white text-center">
        <h1>Test OK Report</h1>
    </div>

    <div class="container mt-3">

        <div class="row">
            <div class="col-md">
                Customer 
            </div>
            <div class="col-md">
                <asp:Label ID="CustomerlabTxt" runat="server" Text=""></asp:Label>
            </div>
            <div class="col-md">
                RMA number 
            </div>
            <div class="col-md">
                <asp:Label ID="RMAnumberTxt" runat="server" Text=""></asp:Label>
            </div>
            <div class="col-md">
                Serial number 
            </div>
            <div class="col-md">
                <asp:Label ID="SerialTxt" runat="server" Text=""></asp:Label>
            </div>
        </div>

        <br />

        <%--1.Power on Start--%>
        <div class="row">
            <div class="col-sm">
                <asp:RadioButton ID="Power_on_check" runat="server" />
            </div>
            <div class="col-sm">
                1.Power on
            </div>
            <div class="col-sm">
                <asp:RadioButton ID="Power_on_Y" runat="server" Text="Y" />
                <asp:RadioButton ID="Power_on_N" runat="server" Text="N" />
            </div>
            <div class="col-sm">
                <asp:TextBox ID="Power_on_note" runat="server" CssClass="form-control" placeholder="請輸入 Re-Mark /備註"></asp:TextBox>
            </div>
        </div>
        <%--1.Power on End--%>

        <%--2.Charger Test Start--%>
        <div class="row">
            <div class="col-sm">
                <asp:RadioButton ID="Charger_Test_check" runat="server" />
            </div>
            <div class="col-sm">
                2.Charger Test
            </div>
            <div class="col-sm">
                <asp:RadioButton ID="Charger_Test_Y" runat="server" Text="Y" />
                <asp:RadioButton ID="Charger_Test_N" runat="server" Text="N" />
            </div>
            <div class="col-sm">
                <asp:TextBox ID="Charger_Test_note" runat="server" CssClass="form-control" placeholder="請輸入 Re-Mark /備註"></asp:TextBox>
            </div>
        </div>
        <%--2.Charger Test End--%>

        <%--3.Display Test Start--%>
        <div class="row">
            <div class="col-sm">
                <asp:RadioButton ID="Display_Test_check" runat="server" />
            </div>
            <div class="col-sm">
                3.Display Test
            </div>
            <div class="col-sm">
                <asp:RadioButton ID="Display_Test_Y" runat="server" Text="Y" />
                <asp:RadioButton ID="Display_Test_N" runat="server" Text="N" />
            </div>
            <div class="col-sm">
                <asp:TextBox ID="Display_Test_note" runat="server" CssClass="form-control" placeholder="請輸入 Re-Mark /備註"></asp:TextBox>
            </div>
        </div>
        <%--3.Display Test End--%>

        <%--4.Indicator light Test Start--%>
        <div class="row">
            <div class="col-sm">
                <asp:RadioButton ID="Indicator_light_Test_check" runat="server" />
            </div>
            <div class="col-sm">
                4.Indicator light Test
            </div>
            <div class="col-sm">
                <asp:RadioButton ID="Indicator_light_Test_Y" runat="server" Text="Y" />
                <asp:RadioButton ID="Indicator_light_Test_N" runat="server" Text="N" />
            </div>
            <div class="col-sm">
                <asp:TextBox ID="Indicator_light_Test_note" runat="server" CssClass="form-control" placeholder="請輸入 Re-Mark /備註"></asp:TextBox>
            </div>
        </div>
        <%--4.Indicator light Test End--%>

        <%--5.Keypad Test light Test Start--%>
        <div class="row">
            <div class="col-sm">
                <asp:RadioButton ID="Keypad_Test_check" runat="server" />
            </div>
            <div class="col-sm">
                5.Keypad Test
            </div>
            <div class="col-sm">
                <asp:RadioButton ID="Keypad_Test_Y" runat="server" Text="Y" />
                <asp:RadioButton ID="Keypad_Test_N" runat="server" Text="N" />
            </div>
            <div class="col-sm">
                <asp:TextBox ID="Keypad_Test_note" runat="server" CssClass="form-control" placeholder="請輸入 Re-Mark /備註"></asp:TextBox>
            </div>
        </div>
        <%--5.Keypad Test light Test End--%>

        <%--6.Reader Test Start--%>
        <div class="row">
            <div class="col-sm">
                <asp:RadioButton ID="Reader_Test_check" runat="server" />
            </div>
            <div class="col-sm">
                6.Reader Test
            </div>
            <div class="col-sm">
                <asp:RadioButton ID="Reader_Test_Y" runat="server" Text="Y" />
                <asp:RadioButton ID="Reader_Test_N" runat="server" Text="N" />
            </div>
            <div class="col-sm">
                <asp:TextBox ID="Reader_Test_note" runat="server" CssClass="form-control" placeholder="請輸入 Re-Mark /備註"></asp:TextBox>
            </div>
        </div>
        <%--6.Reader Test End--%>

        <%-- 7.Memory Test Start--%>
        <div class="row">
            <div class="col-sm">
                <asp:RadioButton ID="Memory_Test_check" runat="server" />
            </div>
            <div class="col-sm">
                7.Memory Test
            </div>
            <div class="col-sm">
                <asp:RadioButton ID="Memory_Test_Y" runat="server" Text="Y" />
                <asp:RadioButton ID="Memory_Test_N" runat="server" Text="N" />
            </div>
            <div class="col-sm">
                <asp:TextBox ID="Memory_Test_note" runat="server" CssClass="form-control" placeholder="請輸入 Re-Mark /備註"></asp:TextBox>
            </div>
        </div>
        <%-- 7.Memory Test End--%>

        <%-- 8.Camera Test Start--%>
        <div class="row">
            <div class="col-sm">
                <asp:RadioButton ID="Camera_Test_check" runat="server" />
            </div>
            <div class="col-sm">
                8.Camera Test
            </div>
            <div class="col-sm">
                <asp:RadioButton ID="Camera_Test_Y" runat="server" Text="Y" />
                <asp:RadioButton ID="Camera_Test_N" runat="server" Text="N" />
            </div>
            <div class="col-sm">
                <asp:TextBox ID="Camera_Test_note" runat="server" CssClass="form-control" placeholder="請輸入 Re-Mark /備註"></asp:TextBox>
            </div>
        </div>
        <%-- 8.Camera Test End--%>

        <%-- 9.Sound Test Start--%>
        <div class="row">
            <div class="col-sm">
                <asp:RadioButton ID="Sound_Test_check" runat="server" />
            </div>
            <div class="col-sm">
                9.Sound Test
            </div>
            <div class="col-sm">
                <asp:RadioButton ID="Sound_Test_Y" runat="server" Text="Y" />
                <asp:RadioButton ID="Sound_Test_N" runat="server" Text="N" />
            </div>
            <div class="col-sm">
                <asp:TextBox ID="Sound_Test_note" runat="server" CssClass="form-control" placeholder="請輸入 Re-Mark /備註"></asp:TextBox>
            </div>
        </div>
        <%-- 9.Sound Test End--%>

        <%-- 10.Interface Test Start--%>
        <div class="row">
            <div class="col-sm">
                <asp:RadioButton ID="Interface_Test_check" runat="server" />
            </div>
            <div class="col-sm">
                10.Interface Test
            </div>
            <div class="col-sm">
                <asp:RadioButton ID="Interface_Test_Y" runat="server" Text="Y" />
                <asp:RadioButton ID="Interface_Test_N" runat="server" Text="N" />
            </div>
            <div class="col-sm">
                <asp:TextBox ID="Interface_Test_note" runat="server" CssClass="form-control" placeholder="請輸入 Re-Mark /備註"></asp:TextBox>
            </div>
        </div>
        <%-- 10.Interface Test End--%>

        <%-- 11.SD Card Test Start--%>
        <div class="row">
            <div class="col-sm">
                <asp:RadioButton ID="SD_Card_Test_check" runat="server" />
            </div>
            <div class="col-sm">
                11.SD Card Test
            </div>
            <div class="col-sm">
                <asp:RadioButton ID="SD_Card_Test_Y" runat="server" Text="Y" />
                <asp:RadioButton ID="SD_Card_Test_N" runat="server" Text="N" />
            </div>
            <div class="col-sm">
                <asp:TextBox ID="SD_Card_Test_note" runat="server" CssClass="form-control" placeholder="請輸入 Re-Mark /備註"></asp:TextBox>
            </div>
        </div>
        <%-- 11.SD Card Test End--%>

        <%-- 12.NFC Test Start--%>
        <div class="row">
            <div class="col-sm">
                <asp:RadioButton ID="NFC_Test_check" runat="server" />
            </div>
            <div class="col-sm">
                12.NFC Test
            </div>
            <div class="col-sm">
                <asp:RadioButton ID="NFC_Test_Y" runat="server" Text="Y" />
                <asp:RadioButton ID="NFC_Test_N" runat="server" Text="N" />
            </div>
            <div class="col-sm">
                <asp:TextBox ID="NFC_Test_note" runat="server" CssClass="form-control" placeholder="請輸入 Re-Mark /備註"></asp:TextBox>
            </div>
        </div>
        <%-- 12.NFC Test End--%>

        <%-- 13.RFID Test Start--%>
        <div class="row">
            <div class="col-sm">
                <asp:RadioButton ID="RFID_Test_check" runat="server" />
            </div>
            <div class="col-sm">
                13.RFID Test
            </div>
            <div class="col-sm">
                <asp:RadioButton ID="RFID_Test_Y" runat="server" Text="Y" />
                <asp:RadioButton ID="RFID_Test_N" runat="server" Text="N" />
            </div>
            <div class="col-sm">
                <asp:TextBox ID="RFID_Test_note" runat="server" CssClass="form-control" placeholder="請輸入 Re-Mark /備註"></asp:TextBox>
            </div>
        </div>
        <%-- 13.RFID Test End--%>

        <%-- 14.GMS Test Start--%>
        <div class="row">
            <div class="col-sm">
                <asp:RadioButton ID="GMS_Test_check" runat="server" />
            </div>
            <div class="col-sm">
                14.GMS Test
            </div>
            <div class="col-sm">
                <asp:RadioButton ID="GMS_Test_Y" runat="server" Text="Y" />
                <asp:RadioButton ID="GMS_Test_N" runat="server" Text="N" />
            </div>
            <div class="col-sm">
                <asp:TextBox ID="GMS_Test_note" runat="server" CssClass="form-control" placeholder="請輸入 Re-Mark /備註"></asp:TextBox>
            </div>
        </div>
        <%-- 14.GMS Test End--%>

        <%-- 15.Wireless Test Start--%>
        <div class="row">
            <div class="col-sm">
                <asp:RadioButton ID="Wireless_Test_check" runat="server" />
            </div>
            <div class="col-sm">
                15.Wireless Test
            </div>
            <div class="col-sm">
                <asp:RadioButton ID="Wireless_Test_Y" runat="server" Text="Y" />
                <asp:RadioButton ID="Wireless_Test_N" runat="server" Text="N" />
            </div>
            <div class="col-sm">
                <asp:TextBox ID="Wireless_Test_note" runat="server" CssClass="form-control" placeholder="請輸入 Re-Mark /備註"></asp:TextBox>
            </div>
        </div>
        <%-- 15.Wireless Test End--%>

        <%-- 16.Firmware Version Start--%>
        <div class="row">
            <div class="col-sm">
                <asp:RadioButton ID="Firmware_Version_check" runat="server" />
            </div>
            <div class="col-sm">
                16.Firmware Version
            </div>
            <div class="col-sm">
                <asp:RadioButton ID="Firmware_Version_Y" runat="server" Text="Y" />
                <asp:RadioButton ID="Firmware_Version_N" runat="server" Text="N" />
            </div>
            <div class="col-sm">
                <asp:TextBox ID="Firmware_Version_note" runat="server" CssClass="form-control" placeholder="請輸入 Re-Mark /備註"></asp:TextBox>
            </div>
        </div>
        <%-- 16.Firmware Version End--%>

        <%--   Other Start --%>
        <div class="row">
            <div class="col-md">
                Other 
            </div>
        </div>

        <div class="row">
            <div class="col-md">
                <asp:TextBox ID="Other_Txt" TextMode="MultiLine" mode="multiline" CssClass="form-control" runat="server" Height="250"></asp:TextBox>
            </div>
        </div>
        <%--   Other End --%>


        <%-- Test Method Start --%>
        <div class="row">
            <div class="col-md">
                Test Method
            </div>
        </div>

        <div class="row">
            <div class="col-md">
                <asp:TextBox ID="Test_Method_Txt" TextMode="MultiLine" mode="multiline" CssClass="form-control" runat="server" Height="250"></asp:TextBox>
            </div>
        </div>
        <%-- Test Method End --%>

        <%-- File Start --%>
        <div class="row">
            <div class="col-md">
                Attachment file
            </div>
        </div>

        <div class="row">
            <div class="col-md">

                <div class="erma-uploadFile-selectFileDiv">

                    <label class="file-upload">
                        <span><strong>Select File</strong></span>
                        <asp:FileUpload ID="FileUpload1" runat="server"></asp:FileUpload>
                    </label>
                    &nbsp;
        <asp:Button ID="UI_cmdFileAdd" runat="server" Text="Upload" CssClass="erma-download-btn" ValidationGroup="vsFileUpLoad" />

                    <%-- 檔案暫存--%>
                    <asp:HiddenField ID="UI_WEBURL" runat="server" Value="" />
                    <asp:HiddenField ID="UI_VisualPath" runat="server" Value="" />
                    <asp:HiddenField ID="html_File_" runat="server" Value="" />
                    <asp:HiddenField ID="File_HiddenField" runat="server" Value="" />
                    <%-- 檔案暫存--%>
                </div>

            </div>


        </div>

        <!-- Modal gallery -->
        <section class="">
            <!-- Section: Images -->
            <section class="">
                <div class="row">

                    <%=DisplayStr()%>
                    <asp:Label ID="UploadLabel" runat="server" Text=""></asp:Label>

                </div>
            </section>
            <!-- Section: Images -->


        </section>
        <!-- Modal gallery -->



        <div class="row">
            <div class="col-md">
                Maintenance 
            </div>
            <div class="col-md">
                <asp:Label ID="Maintenance_Lab" runat="server" Text=""></asp:Label>
            </div>
            <div class="col-md">
                Date
            </div>
            <div class="col-md">
                <asp:Label ID="Date_Lab" runat="server" Text=""></asp:Label>
            </div>
        </div>

        <div class="row">

            <div class="col-md">
                <asp:Button ID="SaveBtn" runat="server" Text="Save" />
            </div>
            <div class="col-md">
                <asp:Button ID="CancelBtn" runat="server" Text="Cancel" />
            </div>
        </div>


    </div>
    <%-- File End --%>

    <asp:HiddenField ID="FileHiddenField" runat="server" Value="" />

</asp:Content>

