<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html
        PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head>
    <title>
        <%=Session("_Title")%>
    </title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.css">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
    <link rel="stylesheet" href="css/font-awesome.min.css" />
    <link rel="stylesheet" href="css/ionicons.min.css" />
    <link href="css/commonStyle.css" rel="stylesheet" />
    <link href="css/login.css" rel="stylesheet" />
    <script src="js/jquery-3.5.0.js"></script>

    <script type="text/javascript">

        var baseLang = "";

        $(document).ready(function ($) {

            /* get browser default lang */
            if (navigator.userLanguage) {
                baseLang = navigator.userLanguage.substring(0, 2).toLowerCase();
            } else {
                baseLang = navigator.language.substring(0, 2).toLowerCase();
            }
            $('.Lang-en').hide();
            $('.Lang-zh').hide();
            $('.Lang-' + baseLang).show();
            filterGlobalLoc = function () {
                var countryCode = $('#loc-' + baseLang).val();
                if (countryCode && countryCode.length > 1) {
                    $('.All-filter_location').hide();
                    $('.location-' + baseLang + '_' + countryCode).show();
                } else {
                    $('.All-filter_location').hide();
                }
            }
            filterGlobalLoc();
            $('#loc-en').change(filterGlobalLoc);
            $('#loc-zh').change(filterGlobalLoc);
        });


        function PayPal() {

            $.ajax({
                type: 'post',
                /*url: 'Default.aspx/GetSnData',*/
                url: 'http://localhost:9001/RMAPaypal/RMAPaypalCreatePaymentSelf', //請求的目標頁面
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ sn: $("#sn_txt").val() }),
                //data: {
                //    num: num //將num post至後臺
                //},
                /*data: { array: JSON.stringify(Users) }, //將陣列轉成Json*/
                success: function (result) {
                    //將傳回的JSON資料轉成array
                    //var dataArray = JSON.parse(result);
                    //var dataArray = result;
                    alert("ok");
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert('error');
                }
            });

        }

        //GetRMA
        function GetRMAData(className) {

            $('#RMA_Server').html("");
            $('#RMA_Server').html("");

            let displayType = $(className)[0].style.display;
            $(className).css("display", "block");
            var userData = "{'sn':'" + $("#RMA_TXT").val() + "'}";

            if ($("#RMA_TXT").val() == null || $("#RMA_TXT").val() == "") {
                alert("Please Input RMA Number");
                return;
            }

            $.ajax({
                type: 'post',
                /*url: 'Default.aspx/GetSnData',*/
                url: 'ashx/GetRMADataHandler.ashx', //請求的目標頁面
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ sn: $("#RMA_TXT").val(), CustomerID: $("#RMA_CustomerID_TXT").val() }),
                //data: {
                //    num: num //將num post至後臺
                //},
                /*data: { array: JSON.stringify(Users) }, //將陣列轉成Json*/
                success: function (result) {
                    //將傳回的JSON資料轉成array
                    //var dataArray = JSON.parse(result);
                    var dataArray = result;
                    var Warhtml = ""

                    if (result.toString() == "") {

                        $('#serialNumber_No_Found_Data').css("display", "block");
                        $(className).css("display", "none");

                    }
                    else {
                        $('#serialNumber_No_Found_Data').css("display", "none");


                        var index = 0;
                        $.each(result, function (key, item) {

                            if (item["RMAD_ID"] != null) {
                                index = 0;
                            }
                            if (item["RECEIVED_DATE"] != null) {
                                index = 1;
                            }
                            if (item["REPAIRQUOTED_DATE"] != null) {
                                index = 2;
                            }
                            if (item["REPAIRED_DATE"] != null) {
                                index = 3;
                            }

                            if (item["CLOSE_DATE"] != null) {
                                index = 4;
                            }

                            if (item["CANCEL_DATE"] != null) {
                                index = 4;
                            }



                        });

                        $.each(result, function (key, item) {

                            if (index == 0) {
                                Warhtml += '<div class="erma-card  active">';
                            }
                            else {
                                Warhtml += '<div class="erma-card">';
                            }
                            Warhtml += '<div class="erma-repairStatus-circle">';
                            Warhtml += '<img src="../images_new/Requested.png" alt="">';
                            Warhtml += '</div>';
                            Warhtml += '<div class="erma-text-content">';
                            Warhtml += '<p>建立</p>';
                            Warhtml += '<p>Requested</p>';

                            if (item["RMAD_ID"] != null) {

                                Warhtml += '<p>' + item["RMAD_ID"] + '</p>';
                            }
                            Warhtml += '</div>';
                            Warhtml += '</div>';

                            Warhtml += '<div class="erma-arrow-block">';
                            Warhtml += '<img src="../images_new/Arrow.svg" alt="" class="erma-arrow">';
                            Warhtml += '</div>';

                            if (index == 1) {
                                Warhtml += '<div class="erma-card  active">';
                            }
                            else {
                                Warhtml += '<div class="erma-card">';
                            }
                            Warhtml += '<div class="erma-repairStatus-circle">';
                            Warhtml += '<img src="../images_new/Received.png" alt="">';
                            Warhtml += '</div>';
                            Warhtml += '<div class="erma-text-content">';
                            Warhtml += '<p>收貨</p>';
                            Warhtml += '<p>Received</p>';
                            if (item["RECEIVED_DATE"] != null) {
                                const date = new Date(item["RECEIVED_DATE"])
                                if (date.toLocaleDateString() != "Invalid Date" && date.toLocaleDateString() != "1970/1/1") {

                                    Warhtml += '<p>' + date.toLocaleDateString() + '</p>';
                                }
                            }
                            Warhtml += '</div>';
                            Warhtml += '</div>';


                            Warhtml += '<div class="erma-arrow-block">';
                            Warhtml += '<img src="../images_new/Arrow.svg" alt="" class="erma-arrow">';
                            Warhtml += '</div>';
                            if (index == 2) {
                                Warhtml += '<div class="erma-card  active">';
                            }
                            else {
                                Warhtml += '<div class="erma-card">';
                            }
                            Warhtml += '<div class="erma-repairStatus-circle">';
                            Warhtml += '<img src="../images_new/Quoted.png" alt="">';
                            Warhtml += '</div>';
                            Warhtml += '<div class="erma-text-content">';
                            Warhtml += '<p>報價</p>';
                            Warhtml += '<p>Quoted</p>';
                            if (item["REPAIRQUOTED_DATE"] != null) {
                                const date = new Date(item["REPAIRQUOTED_DATE"])
                                if (date.toLocaleDateString() != "Invalid Date" && date.toLocaleDateString() != "1970/1/1") {

                                    Warhtml += '<p>' + date.toLocaleDateString() + '</p>';
                                }
                            }
                            Warhtml += '</div>';
                            Warhtml += '</div>';


                            Warhtml += '<div class="erma-arrow-block">';
                            Warhtml += '<img src="../images_new/Arrow.svg" alt="" class="erma-arrow">';
                            Warhtml += '</div>';
                            if (index == 3) {
                                Warhtml += '<div class="erma-card  active">';
                            }
                            else {
                                Warhtml += '<div class="erma-card">';
                            }
                            Warhtml += '<div class="erma-repairStatus-circle">';
                            Warhtml += '<img src="../images_new/Repaired.png" alt="">';
                            Warhtml += '</div>';
                            Warhtml += '<div class="erma-text-content">';
                            Warhtml += '<p>維修</p>';
                            Warhtml += '<p>Repaired</p>';
                            if (item["REPAIRED_DATE"] != null) {
                                const date = new Date(item["REPAIRED_DATE"])
                                if (date.toLocaleDateString() != "Invalid Date" && date.toLocaleDateString() != "1970/1/1") {

                                    Warhtml += '<p>' + date.toLocaleDateString() + '</p>';;
                                }
                            }
                            Warhtml += '</div>';
                            Warhtml += '</div>'

                            Warhtml += '<div class="erma-arrow-block">';
                            Warhtml += '<img src="../images_new/Arrow.svg" alt="" class="erma-arrow">';
                            Warhtml += '</div>';
                            if (index == 4) {
                                Warhtml += '<div class="erma-card  active">';
                            }
                            else {
                                Warhtml += '<div class="erma-card">';
                            }
                            Warhtml += '<div class="erma-repairStatus-circle">';
                            Warhtml += '<img src="../images_new/Noticed.png" alt="">';
                            Warhtml += '</div>';
                            Warhtml += '<div class="erma-text-content">';
                            Warhtml += '<p>寄貨</p>';
                            Warhtml += '<p>Shipping</p>';

                            if (item["CLOSE_DATE"] != null) {

                                Warhtml += '<p>' + item["CLOSE_DATE"].substring(0, 9) + '</p>';;

                            }
                            else {

                                if (item["CANCEL_DATE"] != null) {

                                    Warhtml += '<p>' + item["CANCEL_DATE"].substring(0, 9) + '</p>';;
                                }
                            }
                            Warhtml += '</div>';
                            Warhtml += '</div>';


                        });
                        $('#RMA_Server').html(Warhtml);
                        $('#RMA_Server').html(Warhtml);
                        $('#Currently_Status_RMA_Server').html("Currently Status");
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Can't find the RMA data you input");
                    var Warhtml = ""
                    $('#RMA_Server').html(Warhtml);
                    $('#Currently_Status_RMA_Server').html("");
                }
            });

        }
        //GetRMA



        function GetSnData(className) {


            let displayType = $(className)[0].style.display;
            $(className).css("display", "block");

            $('.rma-service-searchResult').css("display", "block");
            var userData = "{'sn':'" + $("#sn_txt").val() + "'}";
            if ($("#sn_txt").val() == null) return;

            $.ajax({
                type: 'post',
                url: 'ashx/GetSnDataHandler.ashx', //請求的目標頁面
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ sn: $("#sn_txt").val() }),
                success: function (result) {

                    if (result.toString() == "") {
                        serialNumber_No_Found_Data
                        $('#serialNumber_No_Found_Data').css("display", "block");
                        $(className).css("display", "none");

                    }
                    else {
                        $('#serialNumber_No_Found_Data').css("display", "none");

                        //將傳回的JSON資料轉成array
                        var Warhtml = ""
                        var Conhtml = ""
                        var Headhtml = ""

                        Headhtml += '<tr class="erma-datagrid-header erma-service-header" >';
                        Headhtml += '<td style="border-bottom:1px solid #D8D8D8" >Product Name</td>';
                        Headhtml += '<td style="border-bottom:1px solid #D8D8D8" >Hardware Warranty Start Date</td>';
                        Headhtml += '<td style="border-bottom:1px solid #D8D8D8" >Hardware Warranty End Date</td>';
                        Headhtml += '</tr>';

                        var Headhtml_ = ""
                        Headhtml_ += '<tr class="erma-datagrid-header erma-service-header" >';
                        Headhtml_ += '<td style="border-bottom:1px solid #D8D8D8" >WARRANTY Name</td>';
                        Headhtml_ += '<td style="border-bottom:1px solid #D8D8D8" >Hardware Warranty Start Date</td>';
                        Headhtml_ += '<td style="border-bottom:1px solid #D8D8D8" >Hardware Warranty End Date</td>';
                        Headhtml_ += '</tr>';


                        Warhtml += Headhtml;
                        Conhtml += Headhtml_;

                        $.each(result, function (key, item) {

                            if (item["TYPE"] == "0") {
                                Warhtml += '<tr class="erma-datagrid-line erma-service-line">';
                                Warhtml += '<td>' + item["EXPORT_PARTNO"] + '</td>';
                                Warhtml += '<td>' + item["SDATE"] + '</td>';
                                Warhtml += '<td>' + item["EDATE"] + '</td>';
                                Warhtml += '</tr>';
                            }
                            else if (item["TYPE"] == "1") {

                                if (item["TYPENAME"] != null) {
                                    Conhtml += '<tr class="erma-datagrid-line erma-service-line">';
                                    Conhtml += '<td>' + item["TYPENAME"] + '</td>';
                                    Conhtml += '<td>' + item["SDATE"] + '</td>';
                                    Conhtml += '<td>' + item["EDATE"] + '</td>';
                                    Conhtml += '</tr>';
                                }
                                else {

                                    Conhtml += '<tr class="erma-datagrid-line erma-service-line">';
                                    Conhtml += '<td>PARTS WARRANTY</td>';
                                    Conhtml += '<td>' + item["SDATE"] + '</td>';
                                    Conhtml += '<td>' + item["EDATE"] + '</td>';
                                    Conhtml += '</tr>';
                                }


                            }

                        });

                        $('#rma_warranty_tbody').html(Warhtml);

                        $('#rma_contract_tbody').html(Conhtml);

                        $('.loader-inner').css("display", "none");
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert('error');
                }
            });

            /*$.unblockUI();*/


        }

        function dateReviver(key, value) {
            var a;
            if (typeof value === 'string') {
                //UTC
                a = /^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*)?)Z$/.exec(value);
                if (a) {
                    return new Date(Date.UTC(+a[1], +a[2] - 1, +a[3], +a[4], +a[5], +a[6]));
                }
                //Unspecified
                a = /^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*)?)$/.exec(value);
                if (a) {
                    return new Date(+a[1], +a[2] - 1, +a[3], +a[4], +a[5], +a[6]);
                }
                //with Timezone
                a = /^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*)?)([+-])(\d{2}):(\d{2})$/.exec(value);
                if (a) {
                    var dir = a[7] == "+" ? -1 : 1;
                    return new Date(Date.UTC(+a[1], +a[2] - 1, +a[3], +a[4] + dir * a[8], +a[5] + dir * a[9], +a[6]));
                }
            }
            return value;
        }




        function GetRegular() {
            var UI_txtAccountID_txt = document.getElementById('<%= UI_txtAccountID.ClientID %>');
            var UI_txtPassword_txt = document.getElementById('<%= UI_txtPassword.ClientID %>');
            var CheckBox1_Check = document.getElementById('<%= CheckBox1.ClientID %>');

            var art = "";
            if (UI_txtAccountID_txt.value == "") {
                art += "AccountID This is required. \r\n";
            }

            if (UI_txtPassword_txt.value == "") {
                art += "Password This is required. \r\n";
            }

            if (CheckBox1_Check.checked == false) {
                art += "CipherLab's Privacy Policy  This is required. \r\n";
            }

            if (art == "") {
                return true;
            }
            else {

                alert(art);
                return false;
            }
        }

    </script>


    <style type="text/css">
        .img_login {
            position: absolute;
            left: 600px;
            top: 15px;
        }


        .erma-login-left-image-close {
            height: 100vh;
            width: 30%;
            display: inline-block;
            vertical-align: top;
            position: fixed;
        }

        .erma-datagrid-serviceLocation td {
            vertical-align: middle;
        }

        .erma-mail-icon {
            font-size: 1.8rem;
            color: #0d6efd;
            cursor: pointer;
        }

            .erma-mail-icon:hover {
                color: #0a58ca;
            }
    </style>
</head>

<body>

    <form id="form1" runat="server">

        <div class="erma-login-view">
            <div id="img" class="erma-login-left-image">
            </div>



            <script type="text/javascript">
                (function ($) {

                    var previousDimensions = {
                        width: $(window).width(),
                        height: $(window).height()
                    }

                    $(window).resize(function () {
                        var newDimensions = {
                            width: $(window).width(),
                            height: $(window).height()
                        }

                        document.getElementById("img").innerHTML = "";

                        if (newDimensions.width > previousDimensions.width) {

                            // resize UP event

                            if ($(window).width() > 1000) {
                                document.getElementById("Login_li").innerHTML = "<ul   ><li  class='erma-login-tab active'>Login</li><li class='erma-globalService-tab'>Global Service</li><li class='erma-warrantyStatus-tab'>Warranty Status</li><li class='erma-repairStatus-tab'>Repair Status</li></ul>";
                                document.getElementById("img").innerHTML = " <div style='position:absolute;right:-250px;top:15px;' ><img  src='images/20240815.png'  style='position:absolute;right:" + (1503 - $(window).width()) / 8 + "px;' /></div>";

                            }
                            else {
                                document.getElementById("Login_li").innerHTML = "<ul style='min-width: 750px;' ><li class='erma-login' style='background:#ffffff;border:0px solid #ffffff;height:41px;'><img  src='images/20240815.png'     /></li><li  class='erma-login-tab active'>Login</li><li class='erma-globalService-tab'>Global Service</li><li class='erma-warrantyStatus-tab'>Warranty Status</li><li class='erma-repairStatus-tab'>Repair Status</li></ul>";
                            }


                            if ($(window).width() > 1300) {
                                //$("#img").attr('class', 'erma-login-left-image');
                            }
                            else {
                                //$("#img").attr('class', 'erma-login-left-image-close');
                            }

                        } else {

                            // resize Down event
                            if ($(window).width() > 1000) {
                                document.getElementById("Login_li").innerHTML = "<ul   ><li  class='erma-login-tab active'>Login</li><li class='erma-globalService-tab'>Global Service</li><li class='erma-warrantyStatus-tab'>Warranty Status</li><li class='erma-repairStatus-tab'>Repair Status</li></ul>";
                                document.getElementById("img").innerHTML = " <div style='position:absolute;right:-250px;top:15px;' ><img  src='images/20240815.png'  style='position:absolute;right:" + (1503 - $(window).width()) / 8 + "px;' /></div>";
                            }
                            else {
                                document.getElementById("Login_li").innerHTML = "<ul style='min-width: 750px;' ><li class='erma-login' style='background:#ffffff;border:0px solid #ffffff;height:41px;'><img  src='images/20240815.png'     /></li><li  class='erma-login-tab active'>Login</li><li class='erma-globalService-tab'>Global Service</li><li class='erma-warrantyStatus-tab'>Warranty Status</li><li class='erma-repairStatus-tab'>Repair Status</li></ul>";
                            }

                            if ($(window).width() > 1300) {
                                //$("#img").attr('class', 'erma-login-left-image');
                            }
                            else {
                                //$("#img").attr('class', 'erma-login-left-image-close');
                            }

                        }

                        previousDimensions = newDimensions


                        $(".erma-login-tabs ul li").click(function () {
                            const activeTab = $(this)[0].classList[0];

                            switch (activeTab) {
                                case 'erma-login-tab':
                                    $(".erma-login-tabs ul li").removeClass("active");
                                    $(".erma-contents").css("display", "none");
                                    $('.' + activeTab).addClass('active');
                                    $('.erma-login-content').css("display", "block");
                                    break;
                                case 'erma-globalService-tab':
                                    $(".erma-login-tabs ul li").removeClass("active");
                                    $(".erma-contents").css("display", "none");
                                    $('.' + activeTab).addClass('active');
                                    $('.erma-globalService-content').css("display", "block");
                                    break;
                                case 'erma-warrantyStatus-tab':
                                    $(".erma-login-tabs ul li").removeClass("active");
                                    $(".erma-contents").css("display", "none");
                                    $('.' + activeTab).addClass('active');
                                    $('.erma-warrantyStatus-content').css("display", "block");

                                    break;
                                case 'erma-repairStatus-tab':
                                    $(".erma-login-tabs ul li").removeClass("active");
                                    $(".erma-contents").css("display", "none");
                                    $('.' + activeTab).addClass('active');
                                    $('.erma-repairStatus-content').css("display", "block");
                                    break;


                                default:
                                    break;
                            }
                        });


                    })
                })(jQuery)

                function RMA_Number_Search_Span_Chage() {
                    document.getElementById("RMA_Number_Search_Span").style.color = "white"; document.getElementById("RMA_Number_Search_Span").style.background = "#486FF2";
                }

                function RMA_Number_Search_Span_Closs_Chage() {
                    document.getElementById("RMA_Number_Search_Span").style.color = "white"; document.getElementById("RMA_Number_Search_Span").style.background = "#4F4F4F";
                }

            </script>

            <div class="erma-login-right-content">
                <div class="erma-login-switch">
                    <div class="erma-login-tabs" id="Login_li">
                    </div>
                    <div class="erma-login-language erma-language erma-combobox-control">
                        <img src="../images_new/language.svg" alt="" style="display: none;">
                        <span>

                            <asp:DropDownList ID="UI_cboLanguage" runat="server" CssClass="erma-combobox-choose" Visible="false">
                            </asp:DropDownList>


                        </span>
                    </div>
                </div>
                <div style="visibility: hidden;">
                    <asp:Label ID="UI_lblMessage" runat="server" ForeColor="red" Visible="false" Text="050_051_Message"></asp:Label>
                </div>

                <!-- 使用區塊 -->
                <div class="erma-content">
                    <div class="erma-contents erma-login-content">
                        <h4>Login</h4>
                        <script>
                            if ($(window).width() > 1000) {
                                document.getElementById("Login_li").innerHTML = "<ul  ><li  class='erma-login-tab active'>Login</li><li class='erma-globalService-tab'>Global Service</li><li class='erma-warrantyStatus-tab'>Warranty Status</li><li class='erma-repairStatus-tab'>Repair Status</li></ul>";
                                document.getElementById("img").innerHTML = " <div style='position:absolute;right:-250px;top:15px;' ><img  src='images/20240815.png'  style='position:absolute;right:" + (1503 - $(window).width()) / 8 + "px;' /></div>";

                            }
                            else {
                                document.getElementById("Login_li").innerHTML = "<ul  style='min-width: 750px;'><li class='erma-login' style='background:#ffffff;border:0px solid #ffffff;height:41px;'><img  src='images/20240815.png'     /></li><li  class='erma-login-tab active'>Login</li><li class='erma-globalService-tab'>Global Service</li><li class='erma-warrantyStatus-tab'>Warranty Status</li><li class='erma-repairStatus-tab'>Repair Status</li></ul>";
                            }


                            if ($(window).width() > 1300) {
                                //$("#img").attr('class', 'erma-login-left-image');
                            }
                            else {
                                //$("#img").attr('class', 'erma-login-left-image-close');
                            }
                        </script>

                        <div class="erma-login-block">

                            <div class="erma-input-components">
                                <p class="erma-input-p">
                                    Account ID
                                </p>
                                <asp:TextBox ID="UI_txtAccountID" runat="server" CssClass="erma-input-input"
                                    placeholder="Please enter Account ID"></asp:TextBox>


                            </div>
                            <div class="erma-input-components">
                                <p class="erma-input-p">Password</p>
                                <asp:TextBox ID="UI_txtPassword" runat="server" CssClass="erma-input-input" AutoCompleteType="Disabled"
                                    TextMode="Password" placeholder="Please enter Password"></asp:TextBox>
                            </div>
                            <div class="erma-checkBox-components">
                                <div class="erma-checkBox-div">
                                    <asp:CheckBox ID="CheckBox1" runat="server" CssClass="erma-checkBox" />

                                </div>
                                <div class="erma-checkBox-text">
                                    <p>
                                        By signing up CipherLab E-RMA system, you acknowledge that you consent to
                                            CipherLab's
                                            <asp:HyperLink ID="UI_linkPrivacy_policy1" runat="server" Target="_blank"
                                                NavigateUrl="~/Privacy_policy.aspx" Font-Size="Small">Privacy Policy
                                            </asp:HyperLink>
                                        You can withdraw your consent at any time.
                                    </p>
                                </div>
                            </div>
                            <asp:Button ID="UI_cmdLogin" runat="server" CssClass="erma-button erma-button-login" Text="Login" OnClientClick="if (!GetRegular()) return false;" OnClick="UI_cmdLogin_Click" />
                            <div class="erma-orBlock">
                                <hr>
                                <span>
                                    <p>or</p>
                                </span>
                                <hr>
                            </div>
                            <div class="erma-other-block">
                                <table>
                                    <tr>
                                        <td>


                                            <a href=""></a>
                                            <script>
                                                function myFunction() {
                                                    let text = "Your CipherLab device must have been registered with comprehensive warranty before you can apply for an Reseller/End User Account for direct RMA service.";
                                                    if (confirm(text) == true) {
                                                        location.href = "https://e-rma.cipherlab.com.tw/RMA/View/RMA/RegisterUser.aspx";
                                                    } else {

                                                    }
                                                    document.getElementById("demo").innerHTML = text;
                                                }

                                                function myFunctionErr() {

                                                    location.href = "https://e-rma.cipherlab.com.tw/forgetpassword.aspx";

                                                }

                                            </script>

                                            <a class="erma-other-button erma-register-button" onclick='myFunction();'>Register for Reseller/End User Account</a>
                                            <asp:LinkButton runat="server" ID="UI_linkRigister"
                                                Text="Register for Reseller/End User Account" Visible="false"
                                                CssClass="erma-other-button erma-register-button" Font-Size="Small"
                                                OnClientClick='return confirm("Your CipherLab device must have been registered with comprehensive warranty before you can apply for an Reseller/End User Account for direct RMA service.")'>
                                            </asp:LinkButton>

                                            <p>(Only for Comprehensive Warranty User)</p>
                                        </td>
                                        <td>

                                            <a class="erma-other-button erma-forgot-button" onclick='myFunctionErr();'>Forgot Password</a>
                                            <asp:LinkButton runat="server" ID="UI_linkForget"
                                                Text="053_Forget Password" Visible="false"
                                                CssClass="erma-other-button erma-forgot-button"
                                                PostBackUrl="~/forgetpassword.aspx"></asp:LinkButton>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">

                                            <div style="display: inline-block;">
                                                <img src="../images_new/manual.svg" alt="">
                                            </div>
                                            <div style="display: inline-block;">
                                                <a href="https://e-rma-admin.cipherlab.com.tw/FILE/Sample/EndUser_UserGuide_2015.pdf" style="color: black;" download>User Guide for Reseller/End User</a>
                                                <asp:LinkButton runat="server" ForeColor="Black" ID="UI_linkEndUser" Visible="false"
                                                    Text="User Guide for Reseller/End User" Font-Size="Small">
                                                </asp:LinkButton>
                                            </div>


                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">

                                            <div style="display: inline-block;">
                                                <img src="../images_new/manual.svg" alt="">
                                            </div>
                                            <div style="display: inline-block;">

                                                <a href="https://e-rma-admin.cipherlab.com.tw/FILE/Sample/EndUser_RMA_New_account_Registration_User_Guide.pdf" style="color: black;" download>User Guide for Distributor</a>

                                                <asp:LinkButton runat="server" ForeColor="Black" ID="UI_linkDist" Visible="false"
                                                    Text="User Guide for Distributor" Font-Size="Small">
                                                </asp:LinkButton>
                                            </div>

                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div class="erma-contents erma-globalService-content" style="display: none;">
                        <h4>Global Service</h4>
                        <div class="erma-combobox-control">
                            <span>Select Your Location:</span>
                            <span>
                                <select name="g1" id="select_g1" class="erma-combobox-choose">

                                    <option value="TW">TW Service Center (HQ)</option>
                                    <option value="EU">EU Service Center</option>
                                    <option value="UK">UK Service Center</option>
                                    <option value="AU">AU Service Center</option>
                                    <option value="NZ">NZ Service Center</option>
                                    <option value="CN">CN Service Center</option>
                                    <option value="US">US Service Center</option>
                                    <option value="JP">JP Service Center</option>


                                </select>
                            </span>
                        </div>
                        <div>
                            <table width="100%" class="erma-datagrid-table">
                                <tr class="erma-datagrid-header erma-service-header">
                                    <td>Service Center</td>
                                    <td>Address</td>
                                    <td>Map</td>
                                    <td>Contact Information</td>
                                </tr>
                                <tr class="erma-datagrid-line erma-service-line">
                                    <td colspan="5"></td>
                                </tr>
                                <tr id="TW" class="erma-datagrid-serviceLocation">
                                    <td>TW Service Center (HQ)</td>
                                    <td>5F., No.196-3, Sec. 3, Datong Rd., Xizhi Dist., New Taipei City 22103, Taiwan (R.O.C.)</br>
									新北市汐止區大同路3段196-3號5樓
                                    </td>
                                    <td>
                                        <a href="https://www.google.com/maps/place/221%E6%96%B0%E5%8C%97%E5%B8%82%E6%B1%90%E6%AD%A2%E5%8D%80%E5%A4%A7%E5%90%8C%E8%B7%AF%E4%B8%89%E6%AE%B5198%E8%99%9F/@25.0732668,121.6715551,17z/data=!3m1!4b1!4m6!3m5!1s0x345d53a78b21bb2d:0x2111bb1f8e70635c!8m2!3d25.073262!4d121.67413!16s%2Fg%2F11c239ssql?hl=zh-TW&entry=ttu"
                                            target="_blank">地圖</a>
                                    </td>
                                    <td><a href="mailto:e-rma@cipherlab.com.tw"><i class="bi bi-envelope-fill fs-4 erma-mail-icon"></i></a></td>
                                </tr>
                                <tr id="EU" class="erma-datagrid-serviceLocation">
                                    <td>EU Service Center</td>
                                    <td>4 Rue de Cadorago, 21310 Belleneuve, France
                                    </td>
                                    <td>
                                        <a href="https://www.google.com/maps/place/4+Rue+de+Cadorago,+21310+Belleneuve,+%E6%B3%95%E5%9C%8B/@47.3634286,5.257474,17z/data=!3m1!4b1!4m6!3m5!1s0x47ed58d9678a5511:0x608948bad9ea4a88!8m2!3d47.363425!4d5.2600489!16s%2Fg%2F11lh3hjsrj?hl=zh-TW&entry=ttu"
                                            target="_blank">地圖</a>
                                    </td>
                                    <td><a href="mailto:cipherlabrma@adac-electronique.fr"><i class="bi bi-envelope-fill fs-4 erma-mail-icon"></i></a></td>
                                </tr>
                                <tr id="UK" class="erma-datagrid-serviceLocation">
                                    <td>UK Service Center</td>
                                    <td>14 Eelmoor Road,  Farnborough,  Hants,  GU14 7QN  UK
                                    </td>
                                    <td>
                                        <a href="https://www.google.com/maps/place/14+Eelmoor+Rd,+Farnborough+GU14+7QN%E8%8B%B1%E5%9C%8B/@51.2847935,-0.773566,17z/data=!3m1!4b1!4m6!3m5!1s0x48742b3ef819ffa1:0x3e4764778f60d216!8m2!3d51.2847902!4d-0.7709911!16s%2Fg%2F11bw3z5_lt?hl=zh-TW&entry=ttu"
                                            target="_blank">地圖</a>
                                    </td>
                                    <td><a href="mailto:helpdesk@barcodetrade.co.uk"><i class="bi bi-envelope-fill fs-4 erma-mail-icon"></i></a></td>
                                </tr>
                                <tr id="AU" class="erma-datagrid-serviceLocation">
                                    <td>AU Service Center</td>
                                    <td>U3 17-19 Miles Street, Mulgrave  VIC 3170
                                    </td>
                                    <td>
                                        <a href="https://www.google.com/maps/place/u3%2F17-19+Miles+St,+Mulgrave+VIC+3170%E6%BE%B3%E6%B4%B2/@-37.9228124,145.1500579,17z/data=!3m1!4b1!4m5!3m4!1s0x6ad6153b38c0ba4b:0xdf057b738d8005dd!8m2!3d-37.9228167!4d145.1526328?hl=zh-TW&entry=ttu&g_ep=EgoyMDI0MTAwMi4xIKXMDSoASAFQAw%3D%3D"
                                            target="_blank">地圖</a>
                                    </td>
                                    <td><a href="mailto:cipherlabrma@clauservice.com.au"><i class="bi bi-envelope-fill fs-4 erma-mail-icon"></i></a></td>
                                </tr>
                                <tr id="NZ" class="erma-datagrid-serviceLocation">
                                    <td>NZ Service Center</td>
                                    <td>Unit G03/656 Great South Road, Ellerslie, Auckland 1051, New Zealand
                                    </td>
                                    <td>
                                        <a href="https://www.google.com/maps/place/Bluechip+Infotech+New+Zealand+Ltd/@-36.9008456,174.8046568,17z/data=!3m1!4b1!4m6!3m5!1s0x6d0d487680f6b2a7:0x3224a3e5859af74f!8m2!3d-36.9008456!4d174.8072317!16s%2Fg%2F11c2j8yjyz?entry=ttu&g_ep=EgoyMDI1MTEwOS4wIKXMDSoASAFQAw%3D%3D"
                                            target="_blank">地圖</a>
                                    </td>
                                    <td><a href="mailto:cipherlabrmanz@clauservice.com.au"><i class="bi bi-envelope-fill fs-4 erma-mail-icon"></i></a></td>
                                </tr>
                                <tr id="CN" class="erma-datagrid-serviceLocation">
                                    <td>CN Service Center</td>
                                    <td>上海市长宁区延安西路726号9楼E室
                                    </td>
                                    <td>
                                        <a href="https://www.google.com/maps/search/%E4%B8%8A%E6%B5%B7%E5%B8%82%E9%95%BF%E5%AE%81%E5%8C%BA%E5%BB%B6%E5%AE%89%E8%A5%BF%E8%B7%AF726%E5%8F%B79%E6%A5%BC/@31.2101187,121.3943123,14z/data=!3m1!4b1?hl=zh-TW&entry=ttu"
                                            target="_blank">地圖</a>
                                    </td>
                                    <td><a href="mailto:77267899@qq.com"><i class="bi bi-envelope-fill fs-4 erma-mail-icon"></i></a></td>
                                </tr>
                                <tr id="US" class="erma-datagrid-serviceLocation">
                                    <td>US Service Center</td>
                                    <td>2552 Summit Avenue Plano Texas USA 75074
                                    </td>
                                    <td>
                                        <a href="https://www.google.com/maps/place/2552+Summit+Ave,+Plano,+TX+75074%E7%BE%8E%E5%9C%8B/@33.0088806,-96.6827167,17z/data=!3m1!4b1!4m6!3m5!1s0x864c194164d6d533:0xecf1feb2d882551c!8m2!3d33.0088761!4d-96.6801418!16s%2Fg%2F11c1xhq4b2?hl=zh-TW&entry=ttu"
                                            target="_blank">地圖</a>
                                    </td>
                                    <td><a href="mailto:rma@cipherlab.com"><i class="bi bi-envelope-fill fs-4 erma-mail-icon"></i></a></td>
                                </tr>
                                <tr id="JP" class="erma-datagrid-serviceLocation">
                                    <td>JP Service Center</td>
                                    <td>3F, 753-2, Nakadate, Chuo-shi, Yamanashi Prefecture, Japan 409-3801
                                    </td>
                                    <td>
                                        <a href="https://www.google.com/maps/search/3F,+753-2,+Nakadate,+Chuo-shi,+Yamanashi+Prefecture,+Japan/@35.6118196,138.551091,17z/data=!3m1!4b1?hl=zh-TW&entry=ttu"
                                            target="_blank">地圖</a>
                                    </td>
                                    <td><a href="mailto:johnny1.tsai@bestyield.com"><i class="bi bi-envelope-fill fs-4 erma-mail-icon"></i></a></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="erma-contents erma-warrantyStatus-content" style="display: none;">
                        <h4>Warranty Status</h4>
                        <div class="erma-input-control">
                            <span class="erma-serialNumber">Serial Number Search :</span>
                            <span class="erma-serialNumber-input">
                                <input class="erma-input-input" type="text" id="sn_txt"
                                    placeholder="Please enter Serial Number"
                                    onblur="Serial_Number_Search_Span_Closs_Chage();"
                                    onfocus="Serial_Number_Search_Span_Chage();">
                                <script>


                                    function Serial_Number_Search_Span_Chage() {
                                        document.getElementById("Serial_Number_Search_Span").style.color = "white"; document.getElementById("Serial_Number_Search_Span").style.background = "#486FF2";
                                    }

                                    function Serial_Number_Search_Span_Closs_Chage() {
                                        document.getElementById("Serial_Number_Search_Span").style.color = "white"; document.getElementById("Serial_Number_Search_Span").style.background = "#4F4F4F";
                                    }

                                </script>


                                <p class="erma-service-where">
                                    <img src="images_new/info.svg" style="width: 15px; height: 15px;" alt="">
                                    <a href="NeedHelp.html" target="_blank">How to Find the Serial Number of Your Cipherlab Product?
                                    </a>
                                </p>
                            </span>

                            <span id="Serial_Number_Search_Span" class="erma-serialNumber-button"
                                onclick="GetSnData('.erma-service-searchResult-Area');"
                                style="background: #4F4F4F; color: white;">Search</span>

                            <div id="serialNumber_No_Found_Data" style="display: none; margin-top: 250px;">
                                <center>No information found</center>

                            </div>

                            <div class="erma-query-status erma-service-searchResult-Area" style="display: none;">
                                <p>The following is the query result.</p>

                                <div class="erma-service-searchResult">
                                    <p class="erma-inquire-TypeName">Product Information</p>
                                    <table width="100%" id="rma_warranty"
                                        class="erma-datagrid-table erma-service-table">
                                        <tr class="erma-datagrid-line erma-service-line">
                                            <td colspan="5"></td>
                                        </tr>

                                        <tbody id="rma_warranty_tbody" class="tbody">
                                        </tbody>
                                    </table>

                                    <p class="erma-inquire-TypeName">Active Contract Information</p>

                                    <table width="100%" id="rma_contract"
                                        class="erma-datagrid-table erma-service-table">
                                        <tr class="erma-datagrid-line erma-service-line">
                                            <td colspan="5"></td>
                                        </tr>

                                        <tbody id="rma_contract_tbody" class="tbody">
                                        </tbody>
                                    </table>

                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="erma-contents erma-repairStatus-content" style="display: none;">
                        <h4>Repair Status</h4>

                        <div class="erma-input-control">



                            <span class="erma-serialNumber">RMA Number Search :</span>
                            <span class="erma-serialNumber-input">
                                <input class="erma-input-input" id="RMA_TXT" type="text"
                                    placeholder="Please enter RMA Number"
                                    onblur="RMA_Number_Search_Span_Closs_Chage();"
                                    onfocus="RMA_Number_Search_Span_Chage();">
                            </span>
                            <span class="erma-serialNumber-button" id="RMA_Number_Search_Span"
                                onclick="GetRMAData('.erma-repairStatus-searchResult-Area');"
                                style="background: #4F4F4F; color: white;">Search</span>






                            <div class="erma-query-status  erma-repairStatus-searchResult-Area"
                                style="display: none;">
                                <p id="Currently_Status_RMA_Server"></p>
                                <%--RMA Server--%>
                                <div id="RMA_Server" class="erma-repairStatus-searchResult">
                                </div>
                                <%--RMA Server--%>
                            </div>
                        </div>

                        <div class="erma-input-control" style="visibility: hidden;">
                            <span class="erma-serialNumber" style="margin-left: -105px;">Account ID
                                                :</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span class="erma-serialNumber-input">
                                                <input class="erma-input-input" id="RMA_CustomerID_TXT" type="text"
                                                    placeholder="Account ID">
                                            </span>

                        </div>

                    </div>
                </div>
            </div>
        </div>

        <script src="js/login.js"></script>
    </form>
</body>

</html>
