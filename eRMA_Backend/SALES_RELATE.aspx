<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="SALES_RELATE.aspx.vb" Inherits="SALES_RELATE" %>

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

            $("#btnInsSales_Relate").on("click", function () {
                $("#Inssales_id").val("");
                $("#Inshead_id").val("");
                $("#Insasst_id").val("");

                event.preventDefault();
                $('#InsSalesRelateModal').modal('show');
            });

            $('#RMAGrid').on("click", ".GetSalesRelateOne", function (event) {

                event.preventDefault();

                var url = $(this).attr("href");

                $.get(url, function (data) {
                    $('#editSalesRelateContainer').html(data);

                    $('#editSalesRelateModal').modal('show');
                });

            });

            $("#btnInsert").on("click", function () {
                var sales = {};
                sales.sales_id = $("#Inssales_id").val();
                sales.head_id = $("#Inshead_id").val();
                sales.asst_id = $("#Insasst_id").val();
                $.ajax({
                    type: "POST",
                    url: "Sales_Relate.aspx/SaveSalesRelate",
                    data: '{objSale: ' + JSON.stringify(sales) + '}',
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function () {
                        alert("sales has been added successfully.");
                        initMyTable();
                    },
                    error: function () {
                        alert("Error while inserting data");
                    }
                });
            });

            $("#btnUpdate").click(function () {
                var id = $(this).attr("edit-id");
                var sales = {};
                sales.sales_id = $("#Editsales_id").val();
                sales.head_id = $("#Edithead_id").val();
                sales.asst_id = $("#Editasst_id").val();

                $.ajax({
                    type: "Post",
                    contentType: "application/json; charset=utf-8",
                    url: "Sales_Relate.aspx/UpdateData",
                    data: '{objSale: ' + JSON.stringify(sales) + ' }',
                    //JSON.stringify({ objEmployee: user, eid: id }),
                    //'{objEmployee: ' + JSON.stringify(user) + ', eid : + ' + id + '}',
                    dataType: "json",
                    success: function (data) {
                        alert("sales has been Update successfully.");

                        initMyTable();
                    },
                    error: function (data) {
                        alert("Error while Updating data of :" + id);
                    }
                });
            });
        });

        $(document).on("click", ".editButton", function () {
            $('#EditSalesRelateModal').focus();
            var id = $(this).attr("data-id");
            //alert(id);
            $.ajax({
                type: "Post",
                contentType: "application/json; charset=utf-8",
                url: "Sales_Relate.aspx/EditData",
                data: '{sale_id: "' + id + '"}',
                dataType: "json",
                success: function (data) {
                    var empDetails = $.parseJSON(data.d);
                    $.each(empDetails, function (index, value) {
                        //console.log(v.Fname);
                        $("#Editsales_id").val(value.SALES_ID);
                        $("#Edithead_id").val(value.HEAD_ID);
                        $("#Editasst_id").val(value.ASST_ID);
                    });

                },
                error: function () {
                    alert("Error while retrieving data of :" + id);
                }
            });
        });

        $(document).on("click", ".deleteButton", function () {
            if (!confirm("Are you sure you want to delete !"))
                return false;

            var id = $(this).attr("data-id");
            $.ajax({
                type: "Post",
                contentType: "application/json; charset=utf-8",
                url: "Sales_Relate.aspx/DeleteData",
                data: '{sale_id: "' + id + '"}',
                dataType: "json",
                success: function () {
                    initMyTable();
                },
                error: function (data) {
                    alert("Error while Updating data of :" + id);
                }
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
                "buttons": ['copyHtml5', 'excelHtml5', 'csvHtml5'],
                "autoWidth": false,
                "sPaginationType": "full_numbers",
                "ajax": {
                    "url": "SALES_RELATE.aspx/GetSalesRelateData",
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
                    { "title": "SALES_ID", "data": "SALES_ID", "name": "SALES_ID", "autoWidth": true, "searchable": true },
                    { "title": "HEAD_ID", "data": "HEAD_ID", "name": "HEAD_ID", "autoWidth": true, "searchable": true },
                    { "title": "ASST_ID", "data": "ASST_ID", "name": "ASST_ID", "autoWidth": true, "searchable": true },
                    {
                        /*"render": function (data, type, full, meta) { return '<a class="btn btn-info" href="/JQueryDataTable/CustomerEdit/' + full.CustomerID + '">Edit</a>'; }*/
                        "title": "Actions",
                        "data": "SALES_ID",
                        "searchable": false,
                        "sortable": false,
                        "width": 100,
                        "render": function (data, type, full, meta) {
                            //return "<input type='button' class='btn btn-primary editButton' data-id='" + data + "' data-toggle='modal' data-target='#EditSalesRelateModal' name='EditButton' id='btnEditModal' value='Edit'/><a href='#' class='btn btn-primary SalesRelateDel' onclick=DeleteSalesRelateData(" + data + ")>Delete</a>";

                            return "<input type='button' class='btn btn-primary editButton' data-id='" + data + "' data-toggle='modal' data-target='#EditSalesRelateModal' name='EditButton' id='btnEditModal' value='Edit'/><input type='button' style='margin-left:10px;' class='btn btn-primary deleteButton' data-id='" + data + "' name='submitButton' id='btnDelete' value='Delete'/></a>";


                        },
                    },

                ],
                "language": {
                    "search": "Search :"   //在表格中搜索："
                },
            });
        };

    </script>


    <div class="container">
        <h2>Sales Relate</h2>

        <div class="row">
            <button type="button" id="btnInsSales_Relate" class="btn btn-primary" style="float: left; margin-left: 20px;">
                Add New Sales_Relate
            </button>
        </div>

        </br>

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

        <!-- For Modal Popup Insert  -->
        <div class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" id="InsSalesRelateModal">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h2 class="modal-title" id="myModalLabel">Sales Relate</h2>
                    </div>
                    <div class="modal-body">
                        <div class="panel-body">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-md-1">sales_id</label>
                                    <div class="col-md-10">
                                        <input type="text" name="Inssales_id" id="Inssales_id" class="form-control" placeholder="1234" required="" />
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-md-1">head_id</label>
                                    <div class="col-md-10">
                                        <input type="text" name="Inshead_id" id="Inshead_id" class="form-control" placeholder="1234" required="" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-1">asst_id</label>
                                    <div class="col-md-10">
                                        <input type="text" name="Insasst_id" id="Insasst_id" class="form-control" placeholder="1234" required="" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        <button type="button" id="btnInsert" class="btn btn-primary" edit-id="" data-dismiss="modal">Save</button>
                    </div>
                </div>
            </div>
        </div>
        <!-- For Modal Popup Insert -->


        <!-- For Modal Popup Edit  -->
        <div class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" id="EditSalesRelateModal">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h2 class="modal-title" id="myModalLabel">Sales Relate Edit</h2>
                    </div>
                    <div class="modal-body">
                        <div class="panel-body">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-md-1">sales_id</label>
                                    <div class="col-md-10">
                                        <input type="text" name="Editsales_id" id="Editsales_id" class="form-control" placeholder="1234" required="" disabled="disabled" />
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-md-1">head_id</label>
                                    <div class="col-md-10">
                                        <input type="text" name="Edithead_id" id="Edithead_id" class="form-control" placeholder="1234" required="" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-1">asst_id</label>
                                    <div class="col-md-10">
                                        <input type="text" name="Editasst_id" id="Editasst_id" class="form-control" placeholder="1234" required="" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        <button type="button" id="btnUpdate" class="btn btn-primary" edit-id="" data-dismiss="modal">Save changes</button>
                    </div>
                </div>
            </div>
        </div>
        <!-- For Modal Popup Edit  -->
    </div>

</asp:Content>

