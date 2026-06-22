<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="RMA_CostData.aspx.vb" Inherits="RMA_CostData" %>

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
    <script src="scripts/my.js"></script>
    <script src="scripts/Chart.min.js"></script>
    <script src="scripts/Chart/js/Colors.js"></script>
    <script src="scripts/Chart/js/chartjs-plugin-datalabels.min.js"></script>
    <script src="scripts/Chart/js/jspdf.umd.min.js"></script>
    <script src="scripts/DataTables/jquery.dataTables.min.js"></script>
    <script src="scripts/DataTables/dataTables.bootstrap4.min.js"></script>
    <script src="scripts/DataTables/dataTables.buttons.min.js"></script>
    <script src="scripts/DataTables/buttons.html5.min.js"></script>
    <script src="scripts/DataTables/buttons.print.min.js"></script>
    <script src="scripts/DataTables/jszip.min.js"></script>
    <script src="scripts/pdfmake/vfs_fonts.js"></script>
    <%--    <script src="scripts/bootstrap.min.js"></script>   --%>

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
        var Mytable;
        var RMA_REPAIR_Labes = ["RMA_REPAIR_AMT", "RMA_REPAIR_DISCOUNT"];
        var RMA_REPAIR_Data;

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

            //$.ajax({
            //    type: 'post',
            //    /*url: 'Default.aspx/GetSnData',*/
            //    url: 'ashx/GetSnDataHandler.ashx', //請求的目標頁面
            //    dataType: 'json',
            //    contentType: 'application/json; charset=utf-8',
            //    data: JSON.stringify({ sn: $("#sn_txt").val() }),
            //    success: function (result) {
            //        //將傳回的JSON資料轉成array
            //        //var dataArray = JSON.parse(result);
            //        //var dataArray = result;
            //        var Optioinhtml = ""
            //        $.each(result, function (key, item) {

            //            Warhtml += '<td>' + item["EXPORT_PARTNO"] + '</td>';

            //        });
            //        $('#rma_warranty tbody').html(Warhtml);
            //    }
            //});

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

        function resetCanvas() {
            $('#peiChart').remove(); // this is my <canvas> element
            $('#graph-container1').append('<canvas id="peiChart"><canvas>');
            $('#doughnutChart').remove(); // this is my <canvas> element
            $('#graph-container2').append('<canvas id="doughnutChart"><canvas>');
            //canvas = document.querySelector('#results-graph');
            //ctx = canvas.getContext('2d');
            //ctx.canvas.width = $('#graph').width(); // resize to parent width
            //ctx.canvas.height = $('#graph').height(); // resize to parent height
            //var x = canvas.width / 2;
            //var y = canvas.height / 2;
            //ctx.font = '10pt Verdana';
            //ctx.textAlign = 'center';
            //ctx.fillText('This text is centered on the canvas', x, y);
        };

        function initChart() {
            // Register the plugin to all charts:
            Chart.register(ChartDataLabels);

            var chartRadarOptions = {
                plugins: {             //額外項目
                    legend: {       //角色的標籤
                        display: true,       //要不要顯示標籤
                        labels: {
                            color: 'rgba(0,0,0,1)'       //黑色
                        }
                    },
                    datalabels: {
                        display: 'auto',        //要不要顯示數值以及方式，*註1
                        color: '#005599',      //深藍色
                        backgroundColor: 'rgb(210,210,210,0.8)',     //淺灰色，微透明
                        labels: {
                            title: {
                                font: {
                                    weight: 'bold'       //粗體
                                },
                                text: '維修中心折讓比重%'
                            }
                        },
                        anchor: 'end',      //錨點，在畫完圖的後方，*註2
                        align: 'end',          //對齊，在最末端，*註2
                        offset: 4,        //位置調整，*註2
                        formatter: (value, ctx) => {
                            let sum = 0;
                            let dataArr = ctx.chart.data.datasets[0].data;
                            dataArr.map(data => {
                                sum += data;
                            });
                            let percentage = (value * 100 / sum).toFixed(2) + "%";
                            return percentage;
                        },
                    }
                }
            };

            //Pie Chart圓餅圖
            var ctxPie = document.getElementById("peiChart");
            var pieChart = new Chart(ctxPie, {
                type: 'pie',
                data: {
                    labels: RMA_REPAIR_Labes,
                    datasets: [{
                        data: RMA_REPAIR_Data,
                        backgroundColor: [
                            window.chartColors.blue,
                            window.chartColors.red,
                            //window.chartColors.orange,
                            //window.chartColors.yellow,
                            //window.chartColors.green,
                            //window.chartColors.purple
                        ]
                    }],
                    datalabels: {
                        color: '#005599',
                    },
                },
                /* plugins: [ChartDataLabels],*/
                options: chartRadarOptions,
            });

            //Doughnut Chart甜甜圈圖
            var ctxDoughnut = document.getElementById("doughnutChart");
            var DpieChart = new Chart(ctxDoughnut, {
                type: 'doughnut',
                data: {
                    labels: RMA_REPAIR_Labes, /*['中國', '日本', '韓國', '越南', '泰國', '新加坡'],*/
                    datasets: [{
                        data: RMA_REPAIR_Data, /*[45, 11, 14, 8, 10, 12],*/
                        backgroundColor: [
                            window.chartColors.blue,
                            window.chartColors.red,
                            //window.chartColors.blue,
                            //window.chartColors.red,
                            //window.chartColors.green,
                            //window.chartColors.purple
                        ],
                    }],
                },
                options: {
                    showTooltips: true,
                    responsive: true,
                    tooltips: {
                        mode: 'point',
                        intersect: true,
                    },
                    title: {
                        display: true,
                        /*fontSize: 26,*/
                        text: '維修中心折讓比重%'
                    },
                    legend: {
                        position: 'bottom',
                        labels: {
                            fontColor: 'black',
                        }
                    }
                }
            });

        };

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
                    "url": "ashx/GetRMACostHandler.ashx",
                    //"data": sJson,
                    "data": function (d) {
                        return JSON.stringify({ repair_no: $("#RepairCenter").val(), sDate: $("#Sdatepicker").val(), eDate: $("#Edatepicker").val() });
                    },
                    "type": "POST",
                    "datatype": "json",
                    //"contentType": "application/json; charset=utf-8",
                    "dataSrc": function (json) {
                        if (json.length == 0) {
                            alert('No data available in table');
                        }

                        let RMA_REPAIR_AMT = 0;
                        let RMA_REPAIR_DISCOUNT = 0;
                        let RMA_RECEIVABLE_AMT = 0;
                        $.each(json, function (i, item) {
                            RMA_REPAIR_AMT += item.REPAIR_AMT;
                            RMA_RECEIVABLE_AMT += item.RECEIVABLE_AMT;
                            RMA_REPAIR_DISCOUNT += 0 - item.SALE_QUOTED_AMT;
                        });

                        //alert(RMA_REPAIR_AMT);
                        //alert(RMA_REPAIR_DISCOUNT);
                        RMA_REPAIR_Data = [];

                        RMA_REPAIR_Data = [roundDecimal(RMA_RECEIVABLE_AMT / RMA_REPAIR_AMT * 100, 2), roundDecimal(RMA_REPAIR_DISCOUNT / RMA_REPAIR_AMT * 100, 2)];

                        resetCanvas();
                        initChart();

                        return json;
                    }
                },
                "columns": [
                    { "title": "年", "data": "YEAR", "name": "YEAR", "autoWidth": true, "searchable": true },
                    { "title": "月", "data": "MONTH", "name": "MONTH", "autoWidth": true, "searchable": true },
                    { "title": "日", "data": "DAY", "name": "DAY", "autoWidth": true, "searchable": true },
                    { "title": "出貨日期", "data": "SHIPPING_DATE", "name": "SHIPPING_DATE", "autoWidth": true, "searchable": true },
                    { "title": "RMA單號", "data": "RMA_NO", "name": "RMA_NO", "autoWidth": true, "searchable": true },
                    { "title": "客戶代號", "data": "CUST_NO", "name": "CUST_NO", "autoWidth": true, "searchable": true },
                    { "title": "客戶名稱", "data": "CU_NAME", "name": "CU_NAME", "autoWidth": true, "searchable": true },
                    { "title": "國別", "data": "CU_COUNTRYID", "name": "CU_COUNTRYID", "autoWidth": true, "searchable": true },
                    { "title": "維修中心", "data": "REPAIR_NO", "name": "REPAIR_NO", "autoWidth": true, "searchable": true },
                    { "title": "業務員代碼", "data": "SALE_NO", "name": "SALE_NO", "autoWidth": true, "searchable": true },
                    { "title": "業務員名稱", "data": "SALE_NM", "name": "SALE_NM", "autoWidth": true, "searchable": true },
                    { "title": "人工費用", "data": "LABORCOST_AMT", "name": "LABORCOST_AMT", "autoWidth": true, "searchable": true },
                    { "title": "材料金額", "data": "MATERIAL_AMT", "name": "MATERIAL_AMT", "autoWidth": true, "searchable": true },
                    { "title": "RMA系統報價應收", "data": "REPAIR_AMT", "name": "REPAIR_AMT", "autoWidth": true, "searchable": true },
                    { "title": "應收金額", "data": "RECEIVABLE_AMT", "name": "RECEIVABLE_AMT", "autoWidth": true, "searchable": true },
                    { "title": "業務折讓金額", "data": "SALE_QUOTED_AMT", "name": "SALE_QUOTED_AMT", "autoWidth": true, "searchable": true },
                ],
                "language": {
                    "search": "Search :"   //在表格中搜索："
                },
            });
        };

    </script>

    <div class="container">
        <h2>Cost Data Report</h2>
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
                        <div class="row">
                            <div id="graph-container1" class="col-md-6" style="width: 400px; height: 400px">
                                <canvas id="peiChart"></canvas>
                            </div>
                            <div id="graph-container2" class="col-md-6" style="width: 400px; height: 400px">
                                <canvas id="doughnutChart"></canvas>
                            </div>
                        </div>

                        <br />

                        <table id="RMAGrid" class="table table-striped table-bordered dt-responsive nowrap row-border hover order-column" width="100%" cellspacing="0">
                        </table>
                        <br />
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>



