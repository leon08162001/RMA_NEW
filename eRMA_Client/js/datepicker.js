
function initDatePickers() {
    function setupDatePicker(txtId, hiddenId) {
        try { $("#" + txtId).datepicker("destroy"); } catch (e) { /* ignore */ }

        $("#" + txtId).datepicker({
            dateFormat: "yy-mm-dd",
            changeMonth: true,
            changeYear: true,
            yearRange: "1900:c",  // 1900 ~ 今年
            maxDate: 0,           // 不允許選未來日期
            beforeShow: function (input, inst) {
                setTimeout(function () {
                    inst.dpDiv.css({ zIndex: 9999 });
                }, 0);
            },
            onSelect: function (dateText) {
                $("#" + hiddenId).val(dateText);
                $("#" + txtId).val(dateText);
            }
        });

        // 初始回填
        var hv = $("#" + hiddenId).val();
        if (hv) {
            $("#" + txtId).val(hv);
        }
    }

    // 你可以根據需要呼叫多組 TextBox + HiddenField
    if (typeof datePickerControls !== "undefined") {
        for (var i = 0; i < datePickerControls.length; i++) {
            setupDatePicker(datePickerControls[i].textBoxId, datePickerControls[i].hiddenId);
        }
    }
}

// 頁面載入
$(function () {
    initDatePickers();
});

// UpdatePanel 或 partial postback
if (typeof (Sys) !== "undefined" && Sys.WebForms && Sys.WebForms.PageRequestManager) {
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
        initDatePickers();
    });
}