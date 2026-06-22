$(document).ready(function () {
    selectInformationFun();
});



$('#erma-select_parts').on('change', function (e) {
    debugger
    let optionSelected = $("option:selected", this);
    let valueSelected = this.value;
    serviceLocationDisplay(valueSelected);
});

function selectInformationFun() {
    let valueSelected = $("#erma-select_parts option:selected")[0].value;
    serviceLocationDisplay(valueSelected);
}

function serviceLocationDisplay(selected) {
    $(".erma-number-view").css("display", "none");
    let informationDisplay = $('.erma-infomation-productNoView .erma-number-view')[0].style.display;
    if (informationDisplay == "none") {
        $('.erma-infomation-productNoView .erma-noData-view').css("display","block");
    }
}