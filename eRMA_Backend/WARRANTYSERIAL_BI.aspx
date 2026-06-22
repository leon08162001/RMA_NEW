<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="WARRANTYSERIAL_BI.aspx.vb" Inherits="WARRANTYSERIAL_BI" %>

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
                if (Mytable != null) {

                };
            });
        });

        function initMyTable() {
            Mytable = $("#RMAGrid").DataTable({
                "dom": 'Bfrtip',
                "destroy": true, //<-- here
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "searching": true,
                "orderMulti": false, // for disable multiple column at once
                "pageLength": 10,
                "scrollX": true,
                "scrollY": true,
                "scrollCollapse": true,
                "buttons": ['copyHtml5', 'excelHtml5', 'csvHtml5'],
                "autoWidth": false,
                "sPaginationType": "full_numbers",
                "ajax": {
                    "url": "ashx/GetWARRANTYSERIAL_BIHandler.ashx?RMAD_RMANO=" + $('#RMAD_RMANO').val(),
                    "type": "GET",
                    "contentType": "application/json; charset=utf-8",
                    "dataSrc": function (json) {
                        if (json.length == 0) {
                            alert('No data available in table');
                        }

                        return json;
                    }
                },
                "columns": [
                    { "title": "RMAD_RMANO", "data": "RMAD_RMANO", "name": "RMAD_RMANO", "autoWidth": true, "searchable": true },
                    { "title": "RMAD_SEQ", "data": "RMAD_SEQ", "name": "RMAD_SEQ", "autoWidth": true, "searchable": true },
                    { "title": "Total", "data": "REMAINING_CONSUMPTION", "name": "REMAINING_CONSUMPTION", "autoWidth": true, "searchable": true },
                    { "title": "WATS_WATYNO", "data": "WATS_WATYNO", "name": "WATS_WATYNO", "autoWidth": true, "searchable": true },
                    { "title": "WATS_WATYSEQ", "data": "WATS_WATYSEQ", "name": "WATS_WATYSEQ", "autoWidth": true, "searchable": true },
                ],
                "language": {
                    "search": "Search :"   //在表格中搜索："
                },
            });
        };
    </script>

    <div class="container">
        <h2>RMA耗用表 Search</h2>
        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-primary list-panel" id="list-panel">
                    <div class="panel-heading list-panel-heading">
                        <div class="form-group">
                            <div class="col-md-2">
                                <p>
                                    <asp:Label ID="UI_lblRMAD_RMANO" runat="server" Text="RMAD_RMANO"></asp:Label>：<input type="text" class="form-control text-box single-line" id="RMAD_RMANO" />
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



