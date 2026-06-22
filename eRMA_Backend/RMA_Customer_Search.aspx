<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="RMA_Customer_Search.aspx.vb" Inherits="RMA_Customer_Search" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">

    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/site.css" rel="stylesheet" />
    <link href="Content/themes/base/all.css" rel="stylesheet" />
    <link href="Content/all.min.css" rel="stylesheet" />
    <link href="Content/themes/base/jquery-ui.min.css" rel="stylesheet" />
    <link href="Content/DataTables/css/jquery.dataTables.min.css" rel="stylesheet" />
    <link href="Content/DataTables/css/dataTables.bootstrap.min.css" rel="stylesheet" />
    <link href="Content/DataTables/css/buttons.dataTables.min.css" rel="stylesheet" />

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
    </style>

    <script type="text/javascript" language="javascript">

        var Mytable;

        $(document).ready(function () {
            $("#btnPerformAdvancedSearch").on("click", function () {
                /*$('#RMAGrid').parents('div.dataTables_wrapper').first().show();*/
                initMyTable();
            });
        });

        function initMyTable() {
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
                //"buttons": ['copyHtml5', 'excelHtml5', 'csvHtml5'],
                "buttons": [
                    {
                        extend: 'copyHtml5',
                        exportOptions: {
                            columns: [':visible']
                        }
                    },
                    {
                        extend: 'excelHtml5',
                        exportOptions: {
                            columns: [0, 2, 3]
                        }
                    },
                    {
                        extend: 'csvHtml5',
                        exportOptions: {
                            columns: [0, 1, 2, 3]
                        }
                    },
                    //'colvis'
                ],
                "autoWidth": false,
                "sPaginationType": "full_numbers",
                "ajax": {
                    "url": "RMA_Customer_Search.aspx/GetCustomerData",
                    "contentType": "application/json;charset=utf-8",
                    //data: { cust_no: $("#cust_no").val(), salesid: $("#cu_salesid").val(), assistantid: $("#cu_assistantid").val() },
                    /*contentType: "application/x-www-form-urlencoded; charset=utf-8",*/
                    //"data": {
                    //    "__RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val(),
                    //},
                    "type": "POST",
                    "datatype": "json",
                    "dataSrc": function (json) {
                        if (json.length == 0) {
                            alert('No data available in table');
                        }
                        var data = json.d;
                        data = $.parseJSON(data);
                        return data;
                    }
                },
                "columDefs": [{
                    "defaultContent": ''
                }],
                "columns": [
                    { "title": "CU_NO", "data": "CU_NO", "name": "CU_NO", "autoWidth": true, "searchable": true },
                    { "title": "CU_NAME", "data": "CU_NAME", "name": "CU_NAME", "autoWidth": true, "searchable": true },
                    { "title": "CU_SALESID", "data": "CU_SALESID", "name": "CU_SALESID", "autoWidth": true, "searchable": true },
                    { "title": "CU_ASSISTANTID", "data": "CU_ASSISTANTID", "name": "CU_ASSISTANTID", "autoWidth": true, "searchable": true },

                ],
                "language": {
                    "search": "Search :"   //在表格中搜索："
                },
            });
        };
    </script>

    <div class="container">
        <h2>Customer</h2>

        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-primary list-panel" id="list-panel">

                    <div class="panel-heading list-panel-heading">
                        <div class="form-group">
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

