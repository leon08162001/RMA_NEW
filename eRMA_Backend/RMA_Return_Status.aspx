<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="RMA_Return_Status.aspx.vb" Inherits="RMA_Return_Status" %>

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
    <script src="scripts/DataTables/dataTables.select.min.js"></script>
    <script src="scripts/DataTables/buttons.print.min.js"></script>
    <script src="scripts/DataTables/jszip.min.js"></script>
    <script src="scripts/pdfmake/vfs_fonts.js"></script>
    <script src="scripts/bootstrap.min.js"></script>

    <script type="text/javascript" language="javascript">
        var Mytable;
        var CheckList = new Array();

        $(document).ready(function () {

            $("#btnPerformAdvancedSearch").on("click", function () {
                /*$('#RMAGrid').parents('div.dataTables_wrapper').first().show();*/

                //清空陣列
                CheckList.length = 0;

                initMyTable();
            });

            $('#RMAGrid tbody').on('click', 'tr', function () {
                alert("mark");
                $(this).toggleClass('selected');
            });

            $("#RMAGrid tr").click(function () {
                //$(this).addClass('selected').siblings().removeClass('selected');
                var value = $(this).find('td:first').html();
                alert(value);
            });

            $("#btnUpdateRmaStatus").on("click", function () {
                //var rows_selected = Mytable.rows('.selected').data();

                //var rows = Mytable.rows('.selected').data();
                //$.each(rows, function (index, rowId)
                //{
                //    var data = rows.data();

                //    alert(rowId.RMA_NO + "-" + rowId.RMAD_SERIALNO + "-" + rowId.RMAD_STATUS);
                //});

                //table.rows('.selected').data()
                var ins = $('#RMAGrid').find("tbody select").map(function () {

                    return $(this).find(":selected").val() + '@' + $(this).find(":selected").text() // get selected text
                    //return $(this).val() // get selected value
                    //return $(this).find(":selected").text() // get selected text

                }).get()

                //alert(ins);

                //alert(Mytable.rows('.selected').data().length + ' row(s) selected');

                //var form = this;

                // Iterate over all checkboxes in the table
                //Mytable.$('input[type="checkbox"]').each(function ()
                //{
                //    // If checkbox doesn't exist in DOM
                //    //if (!$.contains(document, this))
                //    //{
                //        // If checkbox is checked
                //        if (this.checked)
                //        {
                //            CheckList.push(this.value);
                //        }
                //    //}
                //});


                $.ajax({
                    type: "POST",
                    url: "RMA_Return_Status.aspx/UpdateRMAStatus",
                    data: '{objStatus: "' + ins + '"}',
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.d.length != 0) {
                            alert(data.d);
                        }
                        else {
                            alert("回復完成,請查核資料.");
                        }

                        initMyTable();
                    },
                    error: function () {
                        alert("Error while inserting data");
                    }
                });

            });

            // Handle click on "Select all" control
            $('#RMAGrid-select-all').on('click', function () {

                // Check/uncheck all checkboxes in the table
                var rows = Mytable.rows({ 'search': 'applied' }).nodes();
                $('input[type="checkbox"]', rows).prop('checked', this.checked);
            });

            $('#RMAGrid tr').on('change', function () {
                alret("change");
            })

            // Handle click on checkbox to set state of "Select all" control
            $('#RMAGrid tbody').on('change', 'input[type="checkbox"]', function () {

                // If checkbox is not checked
                if (!this.checked) {
                    var el = $('#RMAGrid-select-all').get(0);
                    // If "Select all" control is checked and has 'indeterminate' property
                    if (el && el.checked && ('indeterminate' in el)) {
                        // Set visual state of "Select all" control
                        // as 'indeterminate'
                        el.indeterminate = true;
                    }
                }
            });

            //Mytable.on('click', 'tr', function ()
            //{
            //    var row = oTable.fnGetData(this); //取得 tr 內容, 傳回陣列
            //    alert(row); //顯示陣列內容
            //    alert(row[2]); //顯示

            //});
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
                //"searching": true,
                "orderMulti": false, // for disable multiple column at once
                //"pageLength": 10,
                "paging": false,
                "scrollX": true,
                "scrollY": true,
                "scrollCollapse": true,
                "buttons": ['copyHtml5', 'excelHtml5', 'csvHtml5'],
                "autoWidth": false,
                "sPaginationType": "full_numbers",
                "ajax": {
                    "url": "RMA_Return_Status.aspx/GetRMAStatus",
                    "contentType": "application/json;charset=utf-8",
                    //"data": JSON.stringify({ sRMA_NO: $("#RMANO").val() }),
                    "data": function (d) {
                        return JSON.stringify({
                            sRMA_NO: $("#RMANO").val()
                        });
                    },
                    /*contentType: "application/x-www-form-urlencoded; charset=utf-8",*/
                    //"data": {
                    //    "__RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val(),
                    //},
                    "type": "POST",
                    "datatype": "json",
                    "dataSrc": function (json) {
                        if (json.length == 0) {
                            alert('此筆資料不適合調整狀態!');
                        }
                        var data = json.d;
                        data = $.parseJSON(data);
                        return data;
                    }
                },
                //"columDefs": [{
                //    'targets': 0,
                //    'searchable': false,
                //    'orderable': false,
                //    'className': 'dt-body-center', //;select-checkbox
                //    'checkboxes': {
                //            'selectRow': true
                //    },
                //    'render': function (data, type, full, meta)
                //    {
                //        return '<input type="checkbox" name="id[]" value="' + $('<div/>').text(data).html() + '">';
                //    }
                //    }],
                "columns": [
                    //{
                    //    "defaultContent": ''
                    //},
                    //{
                    //    width: "0.5%",
                    //    searchable: false,
                    //    orderable: false,
                    //    data: "RMAD_SERIALNO",
                    //    //checkboxes: {
                    //    //    'selectRow': true
                    //    //},
                    //    render: function ( data, type, row ) {
                    //        if ( type === 'display' ) {
                    //            return '<input type="checkbox" name="id[]" value="' + $('<div/>').text(data).html() + '">';
                    //        }
                    //        return data;
                    //    },
                    //    className: "select-checkbox",
                    //},
                    { "title": "RMA_NO", "data": "RMA_NO", "name": "RMA_NO", "autoWidth": true, "searchable": true },
                    { "title": "RMAD_SERIALNO", "data": "RMAD_SERIALNO", "name": "RMA_NO", "autoWidth": true, "searchable": true },
                    //{ "title": "RMA_STATUS", "data": "RMA_STATUS", "name": "RMAD_STATUS", "autoWidth": true, "searchable": true },
                    //{ "title": "RMAD_STATUS", "data": "RMAD_STATUS", "name": "RMAD_STATUS", "autoWidth": true, "searchable": true },
                    {
                        "title": "RMAD_STATUS", "data": "RMAD_STATUS", "className": "text-center",
                        render: function (data, type, row) {
                            var SelectList = "<select id='dropdown'>";
                            if (data == "20") {
                                SelectList += '<option value="' + row.RMA_NO + '@' + row.RMAD_SERIALNO + '@' + data + '"' + (data == "10" ? " selected " : "") + '>10</option>';
                                SelectList += '<option value="' + row.RMA_NO + '@' + row.RMAD_SERIALNO + '@' + data + '"' + (data == "20" ? " selected " : "") + '>20</option>';
                            }
                            if (data == "30") {
                                SelectList += '<option value="' + row.RMA_NO + '@' + row.RMAD_SERIALNO + '@' + data + '"' + (data == "10" ? " selected " : "") + '>10</option>';
                                SelectList += '<option value="' + row.RMA_NO + '@' + row.RMAD_SERIALNO + '@' + data + '"' + (data == "20" ? " selected " : "") + '>20</option>';
                                SelectList += '<option value="' + row.RMA_NO + '@' + row.RMAD_SERIALNO + '@' + data + '"' + (data == "30" ? " selected " : "") + '>30</option>';
                            }
                            if (data == "40") {
                                SelectList += '<option value="' + row.RMA_NO + '@' + row.RMAD_SERIALNO + '@' + data + '"' + (data == "10" ? " selected " : "") + '>10</option>';
                                SelectList += '<option value="' + row.RMA_NO + '@' + row.RMAD_SERIALNO + '@' + data + '"' + (data == "20" ? " selected " : "") + '>20</option>';
                                SelectList += '<option value="' + row.RMA_NO + '@' + row.RMAD_SERIALNO + '@' + data + '"' + (data == "30" ? " selected " : "") + '>30</option>';
                                SelectList += '<option value="' + row.RMA_NO + '@' + row.RMAD_SERIALNO + '@' + data + '"' + (data == "40" ? " selected " : "") + '>40</option>';
                            }
                            if (data == "50") {
                                SelectList += '<option value="' + row.RMA_NO + '@' + row.RMAD_SERIALNO + '@' + data + '"' + (data == "10" ? " selected " : "") + '>10</option>';
                                SelectList += '<option value="' + row.RMA_NO + '@' + row.RMAD_SERIALNO + '@' + data + '"' + (data == "20" ? " selected " : "") + '>20</option>';
                                SelectList += '<option value="' + row.RMA_NO + '@' + row.RMAD_SERIALNO + '@' + data + '"' + (data == "30" ? " selected " : "") + '>30</option>';
                                SelectList += '<option value="' + row.RMA_NO + '@' + row.RMAD_SERIALNO + '@' + data + '"' + (data == "50" ? " selected " : "") + '>50</option>';
                            }
                            if (data == "60") {
                                SelectList += '<option value="' + row.RMA_NO + '@' + row.RMAD_SERIALNO + '@' + data + '"' + (data == "10" ? " selected " : "") + '>10</option>';
                                SelectList += '<option value="' + row.RMA_NO + '@' + row.RMAD_SERIALNO + '@' + data + '"' + (data == "20" ? " selected " : "") + '>20</option>';
                                SelectList += '<option value="' + row.RMA_NO + '@' + row.RMAD_SERIALNO + '@' + data + '"' + (data == "30" ? " selected " : "") + '>30</option>';
                                SelectList += '<option value="' + row.RMA_NO + '@' + row.RMAD_SERIALNO + '@' + data + '"' + (data == "60" ? " selected " : "") + '>60</option>';
                            }

                            return SelectList;
                            //createSelect(data);
                            //return "<select id='dropdown'>" +
                            //        '<option value="' + row.RMA_NO + '@' + row.RMAD_SERIALNO + '"' + (data == "20" ? " selected " : "") + '>20</option>' +
                            //        '<option value="' + row.RMA_NO + '@' + row.RMAD_SERIALNO + '"' + (data == "30" ? " selected " : "") + '>30</option>' +
                            //        '<option value="' + row.RMA_NO + '@' + row.RMAD_SERIALNO + '"' + (data == "40" ? " selected " : "") + '>40</option>' +
                            //        '<option value="' + row.RMA_NO + '@' + row.RMAD_SERIALNO + '"' + (data == "50" ? " selected " : "") + '>50</option>' +
                            //        "</select>";
                        },
                    },
                ],
                "select": {
                    "style": 'multi',
                    "selector": 'td:first-child'
                },
                "language": {
                    "search": "Search :"   //在表格中搜索："
                },
            });

            //Mytable.on('select', function (e, dt, type, indexes)
            //{
            //    if (type === 'rows')
            //    {
            //        var data = Mytable.rows(indexes).data().pluck('id');
            //    }
            //});
        };

    </script>

    <div class="container">
        <h2>改變RMA狀態</h2>

        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-primary list-panel" id="list-panel">

                    <div class="panel-heading list-panel-heading">
                        <div class="form-group">

                            <div class="col-md-3">
                                <p>
                                    <asp:Label ID="UI_lblRMANO" runat="server" Text="RMA NO"></asp:Label>：<input type="text" class="form-control text-box single-line" id="RMANO" value="" />
                                    10 : 開立   20 : 接受   30 : 報價
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
                            <%--<thead>
                                <tr>
                                   <th style="text-align :center"> <input name="select_all" value="1" id="RMAGrid-select-all" type="checkbox" /></th>
                                    <th>RMA NO</th>
                                    <th>SERIAL NO</th>
                                    <th>RMAD STATUS</th>
                                </tr>
                            </thead>--%>
                        </table>
                        <br />
                    </div>
                </div>

                <div class="row">
                    <div class="form-group col-sm-offset-9">
                        <button type="button" id="btnUpdateRmaStatus" class="btn btn-primary" style="float: left; margin-left: 20px;">
                            改變RMA狀態
                        </button>
                    </div>

                </div>
            </div>
        </div>
    </div>

</asp:Content>

