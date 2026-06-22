$(document).ready(function () {
    selectInformationFun();
});



$(".rma-service-tabs li").click(function () {
    $(".rma-service-tabs li").removeClass("active");
    $(this).addClass("active");

    let globalService = "global-Service";
    let warrantyCheck = "warranty-Check";
    const tabType = $(this)[0].className;
    if (tabType.indexOf(globalService) >= 0) {
        $(".rma-service-information").css("display", "block");
        $(".rma-service-warrantyCheck").css("display", "none");
    } else if (tabType.indexOf(warrantyCheck) >= 0) {
        $(".rma-service-warrantyCheck").css("display", "block");
        $(".rma-service-information").css("display", "none");
    }
});


function displayFun(className) {
    let displayType = $(className)[0].style.display;
    if (displayType != "none")
        $(className).css("display", "none");
    else
        $(className).css("display", "block");
}

function alertMessage() {
    alert("交由怡仁連接資料庫")
}

$('#select_g1').on('change', function (e) {
    let optionSelected = $("option:selected", this);
    let valueSelected = this.value;
    serviceLocationDisplay(valueSelected);
});

function selectInformationFun() {
    let valueSelected = $("#select_g1 option:selected")[0].value;
    serviceLocationDisplay(valueSelected);
}

function serviceLocationDisplay(selected) {
    $(".rma-datagrid-serviceLocation").css("display", "none");
    $("#" + selected).css("display", "table-row");
}