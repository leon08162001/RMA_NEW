<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="WarrantyData.aspx.vb" Inherits="WarrantyData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/site.css" rel="stylesheet" />
    <link href="Content/themes/base/all.css" rel="stylesheet" />
    <%-- <link href="Content/SideMenu/main.css" rel="stylesheet"/>
<link href="Content/SideMenu/plugins.css" rel="stylesheet"/>
<link href="Content/SideMenu/responsive.css" rel="stylesheet"/>
<link href="Content/SideMenu/icons.css" rel="stylesheet"/>
<link href="Content/SideMenu/login.css" rel="stylesheet"/>--%>
    <link href="Content/all.min.css" rel="stylesheet" />
    <link href="Content/themes/base/jquery-ui.min.css" rel="stylesheet" />
    <link href="Content/DataTables/css/jquery.dataTables.min.css" rel="stylesheet" />
    <link href="Content/DataTables/css/dataTables.bootstrap.min.css" rel="stylesheet" />
    <link href="Content/DataTables/css/buttons.dataTables.min.css" rel="stylesheet" />
    <%--  <link href="Content/Pivot/Pivot.min.css" rel="stylesheet"/>--%>

    <script src="scripts/modernizr-2.8.3.js"></script>
    <script src="scripts/jquery-3.6.0.min.js"></script>
    <script src="scripts/jquery-ui-1.13.1.js"></script>
    <script src="scripts/jquery.unobtrusive-ajax.js"></script>
    <script src="scripts/DataTables/jquery.dataTables.min.js"></script>
    <script src="scripts/DataTables/dataTables.bootstrap4.min.js"></script>
    <script src="scripts/DataTables/dataTables.buttons.min.js"></script>
    <script src="scripts/DataTables/buttons.html5.min.js"></script>
    <script src="scripts/DataTables/buttons.print.min.js"></script>
    <script src="scripts/DataTables/jszip.min.js"></script>
    <script src="scripts/pdfmake/vfs_fonts.js"></script>
    <script src="scripts/bootstrap.min.js"></script>

    <style>
        /*.ui-datepicker {
            background: #333;
            border: 1px solid #555;
            color: #EEE;
        }*/

        .dataTables_wrapper .dataTables_paginate .paginate_button {
            border-radius: 0;
            /*background: #ff1493;*/
            padding: 1px;
        }

        .ui-datepicker-week-end {
            background-color: #d9edf7;
            color: #333;
        }

        .ui-datepicker select.ui-datepicker-month,
        .ui-datepicker select.ui-datepicker-year {
            color: black;
        }
    </style>

    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            <%--$("#RepairCenter").empty();
            var arr1 = '<%=Session("_RepairCenter") %>'.split(',')
            $.each(arr1, function (i, val)
            {
                $('#RepairCenter').append(new Option(val, val));
            });--%>

            // 使用attr()
            //$("select option[value='xxx']").attr("selected", "selected");

            //var optionText = "aaa";
            //var optionValue = "bbb";
            //$("#RepairCenter").empty();
            //var o = new Option("option text", "111");
            ///// jquerify the DOM object 'o' so we can use the html method
            //$(o).html("option text");
            //$("#RepairCenter").append(o);
            //$('#RepairCenter').append(new Option(optionText, optionValue));

            //var Optioinhtml = ""
            //Optioinhtml += '<option value="111"></option>';
            //Optioinhtml += '<option value="222"></option>';
            //$('#RepairCenter').html(Optioinhtml);

            $.ajax({
                type: 'post',
                /*url: 'Default.aspx/GetSnData',*/
                url: 'ashx/GetWarrantyTypeHandler.ashx', //請求的目標頁面
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                async: false,//加入這行代表關閉非同步
                //data: JSON.stringify({ sn: $("#sn_txt").val() }),
                success: function (result) {
                    $("#Wa_Type").empty();
                    $.each(result, function (key, item) {

                        $('#Wa_Type').append(new Option(item.WARRSET_TYPE_NAME, item.WARRSET_TYPE));

                    });

                }
            });

            $("#Sdatepicker").datepicker({
                dateFormat: "yy/mm/dd",
                changeMonth: true,
                changeYear: true,
                //dayNamesMin: ["日", "月", "火", "水", "木", "金", "土"],
                //monthNames: ["1 月", "2 月", "3 月", "4 月", "5 月", "6 月", "7 月", "8 月", "9 月", "10 月", "11 月", "12 月"],
            });

            $("#Edatepicker").datepicker({
                dateFormat: "yy/mm/dd",
                changeMonth: true,
                changeYear: true,
            });

            $("#btnPerformAdvancedSearch").on("click", function () {

                /*$('#RMAGrid').parents('div.dataTables_wrapper').first().show();*/

                //if ($("#RepairCenter").val() == "")
                //{
                //    alert("Please enter Repair Center. in the form");
                //    $("#RepairCenter").focus();
                //    e.preventDefault();
                //    return;
                //}
                //if ($("#Sdatepicker").val() == "")
                //{
                //    alert("Please enter Start Date in the form");
                //    $$("#Sdatepicker").focus();
                //    e.preventDefault();
                //    return;
                //}
                //if ($("#Edatepicker").val() == "")
                //{
                //    alert("Please enter End Date in the form");
                //    $$("#Edatepicker").focus();
                //    e.preventDefault();
                //    return;
                //}

                initMyTable();

                if (Mytable != null) {
                    //Mytable.api().ajax.reload();
                    //Mytable.ajax.reload();
                    //$('#RMAGrid').DataTable().draw();

                };
            });
        });

        function initMyTable() {
            //var sJson = { repair_no: $("#RepairCenter").val(), sDate: $("#Sdatepicker").val(), eDate: $("#Edatepicker").val() };
            //var sJson = { "repair_no": 0, "sDate": "zhangsan", "eDate": "zzzzzz" };
            //alert(sJson);
            //var sJson = JSON.stringify({ repair_no: $("#RepairCenter").val(), sDate: $("#Sdatepicker").val(), eDate: $("#Edatepicker").val() });
            Mytable = $("#RMAGrid").DataTable({
                "dom": 'Bfrtip',
                /*"deferLoading": 0, //(畫面一開始載入不發出ajax撈資料)*/
                "destroy": true, //<-- here
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                /*"retrieve": true,  //只回傳實例*/
                /*"filter": true,*/ //無用
                "searching": true,
                "orderMulti": false, // for disable multiple column at once
                "pageLength": 10,
                "scrollX": true,
                "scrollY": true,
                "scrollCollapse": true,
                "buttons": ['copyHtml5', 'excelHtml5', 'csvHtml5'],
                "autoWidth": false,
                /*"pagingType": $(window).width() < 768 ? "simple" : "simple_numbers",*/
                "sPaginationType": "full_numbers",
                "ajax": {
                    /*"url": "/JQueryDataTable/LoadData",*/
                    "url": "ashx/GetWarrantyDataHandler.ashx",
                    //"data": sJson,
                    "data": function (d) {
                        return JSON.stringify({
                            Wa_sDate: $("#Sdatepicker").val(), Wa_eDate: $("#Edatepicker").val(), Wa_Cust: $("#Wa_Cust").val(),
                            Wa_InvNo: $("#Wa_InvNo").val(), Wa_SaleID: $("#Wa_SaleID").val(), Wa_Type: $("#Wa_Type").val(),
                            Wa_SN: $("#Wa_SN").val(), Wa_Model: ""
                        });
                    },
                    "type": "POST",
                    "datatype": "json",
                    "contentType": "application/json; charset=utf-8",
                    "dataSrc": function (json) {
                        if (json.length == 0) {
                            alert('No data available in table');
                        }

                        return json;
                    }
                },
                "columns": [
                    { "title": "WATY_NO", "data": "WATY_NO", "name": "WATY_NO", "autoWidth": true, "searchable": true },
                    { "title": "WATY_CUST", "data": "WATY_CUST", "name": "WATY_CUST", "autoWidth": true, "searchable": true },
                    { "title": "CUST_NM", "data": "CUST_NM", "name": "CUST_NM", "autoWidth": true, "searchable": true },
                    { "title": "WATY_DATE", "data": "WATY_DATE", "name": "WATY_DATE", "autoWidth": true, "searchable": true },
                    { "title": "WATY_CURR", "data": "WATY_CURR", "name": "WATY_CURR", "autoWidth": true, "searchable": true },
                    { "title": "WATY_ERPNO", "data": "WATY_ERPNO", "name": "WATY_ERPNO", "autoWidth": true, "searchable": true },
                    { "title": "WATY_SALESID", "data": "WATY_SALESID", "name": "WATY_SALESID", "autoWidth": true, "searchable": true },
                    { "title": "SALE_NM", "data": "SALE_NM", "name": "SALE_NM", "autoWidth": true, "searchable": true },
                    { "title": "WATY_CUST_PO", "data": "WATY_CUST_PO", "name": "WATY_CUST_PO", "autoWidth": true, "searchable": true },
                    { "title": "WATI_SEQ", "data": "WATI_SEQ", "name": "WATI_SEQ", "autoWidth": true, "searchable": true },
                    { "title": "WATI_ORDER", "data": "WATI_ORDER", "name": "WATI_ORDER", "autoWidth": true, "searchable": true },
                    { "title": "WATI_ORDSEQ", "data": "WATI_ORDSEQ", "name": "WATI_ORDSEQ", "autoWidth": true, "searchable": true },
                    { "title": "WATI_SKUNO", "data": "WATI_SKUNO", "name": "WATI_SKUNO", "autoWidth": true, "searchable": true },
                    { "title": "WATI_SKUDESC", "data": "WATI_SKUDESC", "name": "WATI_SKUDESC", "autoWidth": true, "searchable": true },
                    { "title": "WATI_TYPE", "data": "WATI_TYPE", "name": "WATI_TYPE", "autoWidth": true, "searchable": true },
                    { "title": "WATI_MODEL", "data": "WATI_MODEL", "name": "WATI_MODEL", "autoWidth": true, "searchable": true },
                    { "title": "WATI_VER", "data": "WATI_VER", "name": "WATI_VER", "autoWidth": true, "searchable": true },
                    { "title": "WATI_VER_ACT", "data": "WATI_VER_ACT", "name": "WATI_VER_ACT", "autoWidth": true, "searchable": true },
                    { "title": "WATI_QTY", "data": "WATI_QTY", "name": "WATI_QTY", "autoWidth": true, "searchable": true },
                    { "title": "WATI_YEAR", "data": "WATI_YEAR", "name": "WATI_YEAR", "autoWidth": true, "searchable": true },
                    { "title": "WATI_PRICE", "data": "WATI_PRICE", "name": "WATI_PRICE", "autoWidth": true, "searchable": true },
                    { "title": "WATS_SN", "data": "WATS_SN", "name": "WATS_SN", "autoWidth": true, "searchable": true },
                    { "title": "WATS_WARRNSTART", "data": "WATS_WARRNSTART", "name": "WATS_WARRNSTART", "autoWidth": true, "searchable": true },
                    { "title": "WATS_WARRNEND", "data": "WATS_WARRNEND", "name": "WATS_WARRNEND", "autoWidth": true, "searchable": true },
                ],
                "language": {
                    "search": "Search :"   //在表格中搜索："
                },
            });
        };

    </script>

    <div class="container">
        <h2>Warranty Search</h2>
        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-primary list-panel" id="list-panel">

                    <div class="panel-heading list-panel-heading">
                        <div class="form-group">

                            <div class="col-md-2">
                                <p>
                                    <asp:Label ID="UI_lblStartDate" runat="server" Text="Start_Date"></asp:Label>：<input type="text" class="form-control text-box single-line" id="Sdatepicker" />
                                </p>

                            </div>
                            <div class="col-md-2">
                                <p>
                                    <asp:Label ID="UI_lblEndDate" runat="server" Text="End_Date"></asp:Label>：<input type="text" class="form-control text-box single-line" id="Edatepicker" />
                                </p>
                            </div>
                            <div class="col-md-2">
                                <p>
                                    <asp:Label ID="UI_lblSerial" runat="server" Text="SerialNO"></asp:Label>：<input type="text" class="form-control text-box single-line" id="Wa_SN" />
                                </p>
                            </div>
                            <div class="col-md-2">
                                <p>
                                    <asp:Label ID="UI_lblCust" runat="server" Text="Customer"></asp:Label>：<input type="text" class="form-control text-box single-line" id="Wa_Cust" />
                                </p>
                            </div>
                            <div class="col-md-2">
                                <p>
                                    <asp:Label ID="UI_lblInvoice" runat="server" Text="Invoice"></asp:Label>：<input type="text" class="form-control text-box single-line" id="Wa_InvNo" />
                                </p>
                            </div>
                            <div class="col-md-2">
                                <p>
                                    <asp:Label ID="UI_lblSale" runat="server" Text="Sale"></asp:Label>：<input type="text" class="form-control text-box single-line" id="Wa_SaleID" />
                                </p>
                            </div>
                            <div class="col-md-2">
                                <p>
                                    <asp:Label ID="UI_lblWarrantyType" runat="server" Text="Warranty Type"></asp:Label>：
                                <select class="form-control" id="Wa_Type" name="WarrantyType" style="font-size: Large; width: 150px;">
                                </select>
                                </p>
                            </div>
                            <%--<div class="col-md-2">
                            <p><asp:Label ID="UI_lblModel" runat="server" Text="Model" ></asp:Label>：
                                <select class="form-control" id="Wa_Model" name="WarrantyType" style="font-size:Large;width:150px;">
                                </select>
                        </div>--%>
                        </div>

                        <br />
                        <div class="row">
                            <div class="form-group col-sm-offset-9">
                                <button type="button" class="btn btn-default btn-md" id="btnPerformAdvancedSearch">
                                    <span class="glyphicon glyphicon-search" aria-hidden="true"></span>Search
                                </button>
                            </div>
                        </div>

                    </div>

                    <div class="panel-body">
                        <table id="RMAGrid" class="table table-striped table-bordered dt-responsive nowrap row-border hover order-column" width="100%" cellspacing="0">
                        </table>
                        <br />
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>



