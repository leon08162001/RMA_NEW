<%@ Page Language="VB" AutoEventWireup="false" CodeFile="WarrantyStatusSearch.aspx.vb" Inherits="_WarrantyStatusSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>login</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">

    <link rel="stylesheet" href="css/font-awesome.min.css">
    <link rel="stylesheet" href="css/ionicons.min.css">
    <link href="css/commonStyle.css" rel="stylesheet" />
    <link href="css/login.css" rel="stylesheet" />
    <script src="js/jquery-3.5.0.js"></script>

    <style>
        .erma-serialNumber-button {
            background: #486ff2;
            padding: 9px 25px;
            margin-left: 5px;
            color: #ffffff;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            text-align: center;
        }
    </style>

    <script type="text/javascript">


        function GetSnData() {


            $.ajax({
                type: 'post',
                url: 'ashx/GetSnDataHandlerNew.ashx', //請求的目標頁面
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ sn: $("#sn_txt").val() }),
                success: function (result) {



                    if (result.toString() == "") {
                        $('#Serial_Number_Search_Div').css("display", "none");
                        alert("No information found");
                    }
                    else {

                        //將傳回的JSON資料轉成array
                        var Warhtml = ""
                        var Conhtml = ""
                        var Headhtml = ""

                        Headhtml += '<tr class="erma-datagrid-header erma-service-header" >';
                        Headhtml += '<td style="border-bottom:2px solid #D8D8D8;width:25%" >Product Name</td>';
                        Headhtml += '<td style="border-bottom:2px solid #D8D8D8;width:45%" >Product Description</td>';
                        Headhtml += '<td style="border-bottom:2px solid #D8D8D8;width:15%" >Warranty Start</td>';
                        Headhtml += '<td style="border-bottom:2px solid #D8D8D8;width:15%" >Warranty End</td>';
                        Headhtml += '</tr>';

                        Warhtml += Headhtml;

                        $.each(result, function (key, item) {


                            Warhtml += '<tr class="erma-datagrid-line erma-service-line">';
                            Warhtml += '<td style="width:25%">' + item["ItemCode"] + '</td>';
                            Warhtml += '<td style="width:45%">' + item["Dscription"] + '</td>';
                            Warhtml += '<td style="width:15%" >' + item["GrnStart"] + '</td>';
                            Warhtml += '<td style="width:15%"  >' + item["GrntExp_Cal"] + '</td>';
                            Warhtml += '</tr>';


                        });

                        $('#rma_warranty_tbody').html(Warhtml);


                        $('#Serial_Number_Search_Div').css("display", "block");


                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert('error');
                }
            });
        }

        function Serial_Number_Search_Span_Chage() {
            document.getElementById("Serial_Number_Search_Span").style.color = "white"; document.getElementById("Serial_Number_Search_Span").style.background = "#486FF2";
        }

        function Serial_Number_Search_Span_Closs_Chage() {
            document.getElementById("Serial_Number_Search_Span").style.color = "white"; document.getElementById("Serial_Number_Search_Span").style.background = "#4F4F4F";
        }
    </script>

</head>
<body>

    <form id="form1" runat="server">

        <div class="erma-login-view">
            <div class="erma-login-left-image">
            </div>
            <div class="erma-login-right-content">
                <asp:Panel ID="Panel2" runat="server">
                    <center>
                        <table style="padding-top: 25%;">
                            <tr>
                                <td colspan="2" style="text-align: center; padding: 15px;">
                                    <h4 style="font-size: 26px;">Login
                                    </h4>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center; padding: 15px;">Account ID</td>
                                <td style="text-align: center; padding: 15px;">
                                    <asp:TextBox ID="Account_Txt" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center; padding: 15px;">Password</td>
                                <td style="text-align: center; padding: 15px;">
                                    <asp:TextBox ID="Password_Txt" runat="server" TextMode="Password"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: center; padding: 15px;">
                                    <asp:Button ID="Button1" runat="server" Text="login " Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </center>
                </asp:Panel>

                <asp:Panel ID="Panel1" runat="server">
                    <div class="erma-contents erma-login-content">
                        <h4 style="margin-left: -250px;">USA Warranty Status</h4>
                        <div class="erma-input-control">

                            <span class="erma-serialNumber">Serial Number Search :</span>
                            <br />

                            <div style="display: inline-block; width: 80%;">
                                <span class="erma-serialNumber-input" style="width: 100%;">
                                    <input class="erma-input-input" type="text" id="sn_txt" placeholder="Please enter Serial Number" onblur="Serial_Number_Search_Span_Closs_Chage();" onfocus="Serial_Number_Search_Span_Chage();">
                                </span>
                            </div>
                            <div style="display: inline-block;">
                                <input id="Serial_Number_Search_Span" class="erma-serialNumber-button" onclick="GetSnData();" value="Search"></input>

                            </div>

                            <div id="serialNumber_No_Found_Data" style="display: none; margin-top: 250px;">
                                <center>No information found</center>
                            </div>

                            <div class="erma-query-status erma-service-searchResult-Area">
                                <div class="erma-service-searchResult" id="Serial_Number_Search_Div" style="display: none;">
                                    <p class="erma-inquire-TypeName">Warranty Information</p>
                                    <table width="100%" id="rma_warranty" class="erma-datagrid-table erma-service-table">
                                        <tr class="erma-datagrid-line erma-service-line">
                                            <td colspan="5"></td>
                                        </tr>

                                        <tbody id="rma_warranty_tbody">
                                        </tbody>
                                    </table>
                                </div>
                            </div>

                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>


    </form>
</body>
</html>
