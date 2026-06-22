<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="RMA_Repair_Company.aspx.vb" Inherits="RMA_Repair_Company" %>

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

            $.ajax({
                type: 'post',
                /*url: 'Default.aspx/GetSnData',*/
                url: 'ashx/GetCompanyHandler.ashx', //請求的目標頁面
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                async: false,//加入這行代表關閉非同步
                //data: JSON.stringify({ sn: $("#sn_txt").val() }),
                success: function (result) {
                    $("#Repair_No").empty();
                    $.each(result, function (key, item) {

                        $('#Repair_No').append(new Option(item.COMP_NAME, item.COMP_NO));

                    });

                }
            });

            $("#btnPerformAdvancedSearch").on("click", function () {
                /*$('#RMAGrid').parents('div.dataTables_wrapper').first().show();*/
                initMyTable();
            });

            $("#btnInsCompanyCountry").on("click", function () {
                $("#Inscompany_no").val("");
                $("#Incountry_id").val("");

                $.ajax({
                    type: 'post',
                    /*url: 'Default.aspx/GetSnData',*/
                    url: 'ashx/GetCompanyHandler.ashx', //請求的目標頁面
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                    async: false,//加入這行代表關閉非同步
                    //data: JSON.stringify({ sn: $("#sn_txt").val() }),
                    success: function (result) {
                        $("#Inscompany_no").empty();
                        $.each(result, function (key, item) {

                            $("#Inscompany_no").append(new Option(item.COMP_NAME, item.COMP_NO));

                        });

                    }
                });

                event.preventDefault();
                $('#InsCompanyCountryModal').modal('show');
            });


            $("#btnInsert").on("click", function () {
                var comp = {};
                comp.company_no = $("#Inscompany_no").val();
                comp.country_id = $("#Inscountry_id").val();
                $.ajax({
                    type: "POST",
                    url: "RMA_Repair_Company.aspx/SaveCompanyCountry",
                    data: '{objComp: ' + JSON.stringify(comp) + '}',
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function () {
                        alert("CompanyCountry has been added successfully.");
                        initMyTable();
                    },
                    error: function () {
                        alert("Error while inserting data");
                    }
                });
            });

        });


        $(document).on("click", ".deleteButton", function () {
            if (!confirm("Are you sure you want to delete !"))
                return false;

            var company_no = $(this).attr("data-company_no");
            var country_id = $(this).attr("data-country_id");
            $.ajax({
                type: "Post",
                contentType: "application/json; charset=utf-8",
                url: "RMA_Repair_Company.aspx/DeleteData",
                data: '{company_no: "' + company_no + '",country_id: "' + country_id + '"}',
                dataType: "json",
                success: function () {
                    initMyTable();
                },
                error: function (data) {
                    alert("Error while Updating data of :" + id);
                }
            });

        });

        function detectChange(selectOS) {
            //alert(selectOS.value);
            $("#Incountry_id").val("");
            $.ajax({
                type: 'post',
                /*url: 'Default.aspx/GetSnData',*/
                url: 'ashx/GetCounrtyHandler.ashx', //請求的目標頁面
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                async: false,//加入這行代表關閉非同步
                data: JSON.stringify({ Repair_No: selectOS.value }),
                success: function (result) {
                    $("#Inscountry_id").empty();
                    $.each(result, function (key, item) {

                        $("#Inscountry_id").append(new Option(item.COUNTRY_NAME, item.COUNTRY_ID));

                    });

                }
            });
        }

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
                    "url": "RMA_Repair_Company.aspx/GetCompanyCountryData",
                    "contentType": "application/json;charset=utf-8",
                    "data": function (d) {
                        return JSON.stringify({ Repair_no: $("#Repair_No").val() });
                    },
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
                    { "title": "COMP_NO", "data": "COMP_NO", "name": "COMP_NO", "autoWidth": true, "searchable": true },
                    { "title": "COUNTRY_ID", "data": "COUNTRY_ID", "name": "COUNTRY_ID", "autoWidth": true, "searchable": true },
                    { "title": "COUNTRY_NAME", "data": "COUNTRY_NAME", "name": "COUNTRY_NAME", "autoWidth": true, "searchable": true },
                    {
                        /*"render": function (data, type, full, meta) { return '<a class="btn btn-info" href="/JQueryDataTable/CustomerEdit/' + full.CustomerID + '">Edit</a>'; }*/
                        "title": "Actions",
                        "data": null,
                        "searchable": false,
                        "sortable": false,
                        "width": 100,
                        "render": function (data, type, full, meta) {
                            //return "<input type='button' class='btn btn-primary editButton' data-id='" + data + "' data-toggle='modal' data-target='#EditSalesRelateModal' name='EditButton' id='btnEditModal' value='Edit'/><a href='#' class='btn btn-primary SalesRelateDel' onclick=DeleteSalesRelateData(" + data + ")>Delete</a>";

                            return "<input type='button' style='margin-left:10px;' class='btn btn-primary deleteButton' data-company_no='" + data.COMP_NO + "' data-country_id='" + data.COUNTRY_ID + "' name='submitButton' id='btnDelete' value='Delete'/></a>";


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
        <h2>Repair control Country</h2>

        <div class="row">
            <button type="button" id="btnInsCompanyCountry" class="btn btn-primary" style="float: left; margin-left: 20px;">
                Add New Repair control country
            </button>

        </div>

        </br>

        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-primary list-panel" id="list-panel">

                    <div class="panel-heading list-panel-heading">
                        <div class="form-group">
                            <div class="col-md-2">
                                <p>
                                    <asp:Label ID="UI_lblRepair" runat="server" Text="Repair No"></asp:Label>：
                                <select class="form-control" id="Repair_No" name="RepairNo" style="font-size: Large; width: 150px;">
                                </select>
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

        <!-- For Modal Popup Insert  -->
        <div class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" id="InsCompanyCountryModal">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h2 class="modal-title" id="myModalLabel">Repair control Country</h2>
                    </div>
                    <div class="modal-body">
                        <div class="panel-body">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-md-1">Repair No</label>
                                    <div class="col-md-10">
                                        <p>
                                            <select class="form-control" id="Inscompany_no" name="Inscompany_no" style="font-size: Large; width: 150px;" onchange="detectChange(this)">
                                            </select>
                                        </p>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-md-1">counrty id</label>
                                    <div class="col-md-10">
                                        <p>
                                            <select class="form-control" id="Inscountry_id" name="Inscountry_id" style="font-size: Large; width: 150px;">
                                            </select>
                                        </p>
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
    </div>

</asp:Content>

