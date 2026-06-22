<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Maintenance_Statement.aspx.vb" Inherits="Maintenance_Statement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <%-- <link href="Content/site.css" rel="stylesheet"/> --%>
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
            $("#RepairCenter").empty();
            var arr1 = '<%=Session("_RepairCenter") %>'.split(',')
            $.each(arr1, function (i, val) {
                $('#RepairCenter').append(new Option(val, val));
            });

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

                if ($("#RepairCenter").val() == "") {
                    alert("Please enter Repair Center. in the form");
                    $("#RepairCenter").focus();
                    e.preventDefault();
                    return;
                }
                if ($("#Sdatepicker").val() == "") {
                    alert("Please enter Start Date in the form");
                    $$("#Sdatepicker").focus();
                    e.preventDefault();
                    return;
                }
                if ($("#Edatepicker").val() == "") {
                    alert("Please enter End Date in the form");
                    $$("#Edatepicker").focus();
                    e.preventDefault();
                    return;
                }

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
                    "url": "ashx/GetMaintenanceStatementHandler.ashx",
                    //"data": sJson,
                    "data": function (d) {
                        return JSON.stringify({ repair_no: $("#Repair_No").val(), sDate: $("#Sdatepicker").val(), eDate: $("#Edatepicker").val() });
                    },
                    "type": "POST",
                    "datatype": "json",
                    //"contentType": "application/json; charset=utf-8",
                    "dataSrc": function (json) {
                        if (json.length == 0) {
                            alert('No data available in table');
                        }

                        return json;
                    }
                },
                "columns": [

                    { "title": "Tracking No", "data": "TRACKINGNO", "name": "TRACKINGNO", "autoWidth": true, "searchable": true },
                    { "title": "Repair No", "data": "RMA_COMPNO", "name": "RMA_COMPNO", "autoWidth": true, "searchable": true },
                    { "title": "Shipped Date", "data": "SHIPPED_DATE", "name": "SHIPPED_DATE", "autoWidth": true, "searchable": true },
                    { "title": "Shipped Year", "data": "SHIPPED_YEAR", "name": "SHIPPED_YEAR", "autoWidth": true, "searchable": true },
                    { "title": "Shipped Month", "data": "SHIPPED_MONTH", "name": "SHIPPED_MONTH", "autoWidth": true, "searchable": true },
                    { "title": "Shipped Day", "data": "SHIPPED_DAY", "name": "SHIPPED_DAY", "autoWidth": true, "searchable": true },
                    { "title": "Customer No", "data": "RMA_CUNO", "name": "RMA_CUNO", "autoWidth": true, "searchable": true },
                    { "title": "Customer Name", "data": "CU_NAME", "name": "CU_NAME", "autoWidth": true, "searchable": true },


                    {
                        data: "CU_COUNTRYID", title: "CU_COUNTRYID",
                        render: function (data, type, row) {
                            const chars = data.split('|');

                            return '<span>' + chars[0] + '</span>'
                        },
                    },



                    { "title": "COUNTRY_NAME", "data": "COUNTRY_NAME", "name": "COUNTRY_NAME", "autoWidth": true, "searchable": true },
                    { "title": "RMA NO", "data": "RMA_NO", "name": "RMA_NO", "autoWidth": true, "searchable": true },
                    { "title": "Serial No", "data": "SERIAL_NO", "name": "SERIAL_NO", "autoWidth": true, "searchable": true },
                    { "title": "Warranty Type", "data": "WARRANTY_TYPE", "name": "WARRANTY_TYPE", "autoWidth": true, "searchable": true },
                    { "title": "Warranty Kind", "data": "WARRANTY_KIND", "name": "WARRANTY_KIND", "autoWidth": true, "searchable": true },
                    { "title": "Final Status", "data": "FINAL_STATUS", "name": "FINAL_STATUS", "autoWidth": true, "searchable": true },
                    { "title": "MODELNO", "data": "MODELNO", "name": "MODELNO", "autoWidth": true, "searchable": true },
                    { "title": "WARRANTY", "data": "WARRANTY", "name": "WARRANTY", "autoWidth": true, "searchable": true },
                    { "title": "RMAD_RECVAD", "data": "RMAD_RECVAD", "name": "RMAD_RECVAD", "autoWidth": true, "searchable": true },



                    {
                        data: "CU_COUNTRYID", title: "RMAR_REPAIRADNAME",
                        render: function (data, type, row) {

                            const chars = data.split('|');

                            return '<span>' + chars[1] + '</span>'
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
        <h2>Maintenance Report</h2>
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

                                <%--                            Repair No.：<select class="form-control" id="RepairCenter" name="repair_no" style="font-size:Large;width:150px;"><option value="JP_BYTE">BYTE</option>
<option value="CEAT">CEAT</option>
<option value="AU_LAPTOP_KINGS">Laptop Kings</option>
<option value="CL_USA">CipherLab USA</option>
<option value="JP_BYTE_MPLUS">JP_MPLUS</option>
<option value="US_CL_MPLUS">US_MPLUS</option>
<option value="NZ_PB_TECH">PB Tech</option>
<option value="UK_FALA">FALA</option>
</select>--%>
                            </div>
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

