<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="CUSTOMER_RMA_Data_Search.aspx.vb" Inherits="CUSTOMER_RMA_Data_Search" %>

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
    <script src="scripts/pdfmake/pdfmake.min.js"></script>
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
                if ($("#Sdatepicker").val() == "" && $("#rma_no").val() == "") {
                    alert("日期或RMA單號要輸入!");
                }

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
                "buttons": ['copyHtml5', 'excelHtml5', 'csvHtml5', 'pdfHtml5'],
                "autoWidth": false,
                "sPaginationType": "full_numbers",
                "ajax": {
                    "url": "RMA_Data_Search.aspx/GetRMAData",
                    "contentType": "application/json;charset=utf-8",
                    //data: { cust_no: $("#cust_no").val(), salesid: $("#cu_salesid").val(), assistantid: $("#cu_assistantid").val() },
                    /*contentType: "application/x-www-form-urlencoded; charset=utf-8",*/
                    //"data": {
                    //    "__RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val(),
                    //},
                    "data": function (d) {
                        return JSON.stringify({
                            sDate: $("#Sdatepicker").val(), eDate: $("#Edatepicker").val(), rma_no: $("#rma_no").val(),
                        });
                    },
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
                    { "title": "RMA_CSTMP", "data": "RMA_CSTMP", "name": "RMA_CSTMP", "autoWidth": true, "searchable": true },
                    { "title": "RMA_NO", "data": "RMA_NO", "name": "RMA_NO", "autoWidth": true, "searchable": true },
                    { "title": "RMA_CUNO", "data": "RMA_CUNO", "name": "RMA_CUNO", "autoWidth": true, "searchable": true },
                    { "title": "CU_NAME", "data": "CU_NAME", "name": "CU_NAME", "autoWidth": true, "searchable": true },
                    { "title": "RMA_ADDRESS", "data": "RMA_ADDRESS", "name": "RMA_ADDRESS", "autoWidth": true, "searchable": true },
                    { "title": "RMA_COMPNO", "data": "RMA_COMPNO", "name": "RMA_COMPNO", "autoWidth": true, "searchable": true },
                    { "title": "RMA_STATUS", "data": "RMA_STATUS", "name": "RMA_STATUS", "autoWidth": true, "searchable": true },
                    { "title": "RMA_MARK", "data": "RMA_MARK", "name": "RMA_MARK", "autoWidth": true, "searchable": true },
                    { "title": "RMA_INVNO", "data": "RMA_INVNO", "name": "RMA_INVNO", "autoWidth": true, "searchable": true },
                    { "title": "RMA_ARNO", "data": "RMA_ARNO", "name": "RMA_ARNO", "autoWidth": true, "searchable": true },
                    { "title": "RMAD_MODELNO", "data": "RMAD_MODELNO", "name": "RMAD_MODELNO", "autoWidth": true, "searchable": true },
                    { "title": "RMAD_SERIALNO", "data": "RMAD_SERIALNO", "name": "RMAD_SERIALNO", "autoWidth": true, "searchable": true },
                    { "title": "RMAD_WARRANTY", "data": "RMAD_WARRANTY", "name": "RMAD_WARRANTY", "autoWidth": true, "searchable": true },
                    { "title": "RMAD_FARNO", "data": "RMAD_FARNO", "name": "RMAD_FARNO", "autoWidth": true, "searchable": true },
                    { "title": "FAR_REASON", "data": "FAR_REASON", "name": "FAR_REASON", "autoWidth": true, "searchable": true },
                    { "title": "RMAD_FARFARCNO", "data": "RMAD_FARFARCNO", "name": "RMAD_FARFARCNO", "autoWidth": true, "searchable": true },
                    { "title": "FARC_NAME", "data": "FARC_NAME", "name": "FARC_NAME", "autoWidth": true, "searchable": true },
                    { "title": "RMAD_PROBLEMDESC", "data": "RMAD_PROBLEMDESC", "name": "RMAD_PROBLEMDESC", "autoWidth": true, "searchable": true },
                    { "title": "RMAD_PRODUCTDESC", "data": "RMAD_PRODUCTDESC", "name": "RMAD_PRODUCTDESC", "autoWidth": true, "searchable": true },
                    { "title": "RMAD_STATUS", "data": "RMAD_STATUS", "name": "RMAD_STATUS", "autoWidth": true, "searchable": true },
                    { "title": "RMAD_RECEVSTATUS", "data": "RMAD_RECEVSTATUS", "name": "RMAD_RECEVSTATUS", "autoWidth": true, "searchable": true },
                    { "title": "RMAD_ISWARRANTY", "data": "RMAD_ISWARRANTY", "name": "RMAD_ISWARRANTY", "autoWidth": true, "searchable": true },
                    { "title": "RMAD_ISCW", "data": "RMAD_ISCW", "name": "RMAD_ISCW", "autoWidth": true, "searchable": true },
                    { "title": "RMAD_PARTSN", "data": "RMAD_PARTSN", "name": "RMAD_PARTSN", "autoWidth": true, "searchable": true },
                    { "title": "RMARQ_IMPROPERUSAGE", "data": "RMARQ_IMPROPERUSAGE", "name": "RMARQ_IMPROPERUSAGE", "autoWidth": true, "searchable": true },
                    { "title": "RMARQ_LABORHOUR", "data": "RMARQ_LABORHOUR", "name": "RMARQ_LABORHOUR", "autoWidth": true, "searchable": true },
                    { "title": "RMARQ_LABORPRICE", "data": "RMARQ_LABORPRICE", "name": "RMARQ_LABORPRICE", "autoWidth": true, "searchable": true },
                    { "title": "RMARQ_MATERIALCOST", "data": "RMARQ_MATERIALCOST", "name": "RMARQ_MATERIALCOST", "autoWidth": true, "searchable": true },
                    { "title": "RMARQ_QUOTE", "data": "RMARQ_QUOTE", "name": "RMARQ_QUOTE", "autoWidth": true, "searchable": true },
                    { "title": "RMARQ_ASSIGLABORCOST", "data": "RMARQ_ASSIGLABORCOST", "name": "RMARQ_ASSIGLABORCOST", "autoWidth": true, "searchable": true },
                    { "title": "RMARQ_ASSIGMATERIALCOST", "data": "RMARQ_ASSIGMATERIALCOST", "name": "RMARQ_ASSIGMATERIALCOST", "autoWidth": true, "searchable": true },
                    { "title": "RMARQ_ASSIGEQUOTE", "data": "RMARQ_ASSIGEQUOTE", "name": "RMARQ_ASSIGEQUOTE", "autoWidth": true, "searchable": true },
                    { "title": "RMARQD_OPARTNO", "data": "RMARQD_OPARTNO", "name": "RMARQD_OPARTNO", "autoWidth": true, "searchable": true },
                    { "title": "RMARQD_NPARTNO", "data": "RMARQD_NPARTNO", "name": "RMARQD_NPARTNO", "autoWidth": true, "searchable": true },
                    { "title": "RMARQD_OSERIALNO", "data": "RMARQD_OSERIALNO", "name": "RMARQD_OSERIALNO", "autoWidth": true, "searchable": true },
                    { "title": "RMARQD_NSERIALNO", "data": "RMARQD_NSERIALNO", "name": "RMARQD_NSERIALNO", "autoWidth": true, "searchable": true },
                    { "title": "RMASQ_LABORCOST", "data": "RMASQ_LABORCOST", "name": "RMASQ_LABORCOST", "autoWidth": true, "searchable": true },
                    { "title": "RMASQ_MATERIALCOST", "data": "RMASQ_MATERIALCOST", "name": "RMASQ_MATERIALCOST", "autoWidth": true, "searchable": true },
                    { "title": "RMASQ_QUOTE", "data": "RMASQ_QUOTE", "name": "RMASQ_QUOTE", "autoWidth": true, "searchable": true },
                    { "title": "RMASQ_CURRENCYCODE", "data": "RMASQ_CURRENCYCODE", "name": "RMASQ_CURRENCYCODE", "autoWidth": true, "searchable": true },
                    { "title": "RMASQ_CURRENCYRATE", "data": "RMASQ_CURRENCYRATE", "name": "RMASQ_CURRENCYRATE", "autoWidth": true, "searchable": true },
                    { "title": "RMASM_SHIPMEMO", "data": "RMASM_SHIPMEMO", "name": "RMASM_SHIPMEMO", "autoWidth": true, "searchable": true },
                    { "title": "RMASM_SHIPNO", "data": "RMASM_SHIPNO", "name": "RMASM_SHIPNO", "autoWidth": true, "searchable": true },
                    { "title": "RMASH_PACKINGLIST", "data": "RMASH_PACKINGLIST", "name": "RMASH_PACKINGLIST", "autoWidth": true, "searchable": true },
                    { "title": "RMASH_EXPRESSCO", "data": "RMASH_TRACKINGNO", "name": "RMASH_TRACKINGNO", "autoWidth": true, "searchable": true },
                    { "title": "RMASH_MEMO", "data": "RMASH_MEMO", "name": "RMASH_MEMO", "autoWidth": true, "searchable": true },
                ],
                "language": {
                    "search": "Search :"   //在表格中搜索："
                },
            });
        };

    </script>


    <div class="container">
        <h2>RMA Data</h2>

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
                                    <asp:Label ID="UI_lblRMANO" runat="server" Text="RMA NO"></asp:Label>：<input type="text" class="form-control text-box single-line" id="rma_no" />
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

