<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="SearchDuplicateSN.aspx.vb" Inherits="SearchDuplicateSN" %>

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
            $("#btnPerformAdvancedSearch").on("click", function () {
                initMyTable();
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
                    "url": "SearchDuplicateSN.aspx/GetDuplicateSNData",
                    //"data": sJson,
                    "data": function (d) {
                        return JSON.stringify({
                            Wa_Cust: $("#Wa_Cust").val(), Wa_SN: $("#Wa_SN").val(),
                        });
                    },
                    "type": "POST",
                    "datatype": "json",
                    "contentType": "application/json; charset=utf-8",
                    "dataSrc": function (json) {
                        if (json.d.length == 0) {
                            alert('No data available in table');
                        }
                        var data = json.d;
                        data = $.parseJSON(data);
                        return data;
                    }
                },
                "columns": [
                    { "title": "RMA_NO", "data": "RMA_NO", "name": "RMA_NO", "autoWidth": true, "searchable": true },
                    { "title": "REQUEST_DATE", "data": "REQUEST_DATE", "name": "REQUEST_DATE", "autoWidth": true, "searchable": true },
                    { "title": "CU_NAME", "data": "CU_NAME", "name": "CU_NAME", "autoWidth": true, "searchable": true },
                    { "title": "RMAD_SERIALNO", "data": "RMAD_SERIALNO", "name": "RMAD_SERIALNO", "autoWidth": true, "searchable": true },
                    { "title": "RMAD_MODELNO", "data": "RMAD_MODELNO", "name": "RMAD_MODELNO", "autoWidth": true, "searchable": true },
                    { "title": "RMAD_WARRANTY_DATE", "data": "RMAD_WARRANTY_DATE", "name": "RMAD_WARRANTY_DATE", "autoWidth": true, "searchable": true },
                    { "title": "WARRANTY", "data": "WARRANTY", "name": "WARRANTY", "autoWidth": true, "searchable": true },
                    { "title": "IMPROPER_USAGE", "data": "IMPROPER_USAGE", "name": "IMPROPER_USAGE", "autoWidth": true, "searchable": true },
                    { "title": "STATUS", "data": "STATUS", "name": "STATUS", "autoWidth": true, "searchable": true },
                ],
                "language": {
                    "search": "Search :"   //在表格中搜索："
                },
            });
        };
    </script>

    <div class="container">
        <h2>Duplicate SN Search</h2>
        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-primary list-panel" id="list-panel">

                    <div class="panel-heading list-panel-heading">
                        <div class="form-group">
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

