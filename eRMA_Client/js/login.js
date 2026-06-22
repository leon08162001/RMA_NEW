$(document).ready(function () {
    selectInformationFun();
});


$(".erma-login-tabs ul li").click(function () {
    const activeTab = $(this)[0].classList[0];

  

    switch (activeTab) {
        case 'erma-login-tab':
		  $(".erma-login-tabs ul li").removeClass("active");
    $(".erma-contents").css("display", "none");

    $('.' + activeTab).addClass('active');
            $('.erma-login-content').css("display", "block");
            break;
        case 'erma-globalService-tab':
		  $(".erma-login-tabs ul li").removeClass("active");
    $(".erma-contents").css("display", "none");

    $('.' + activeTab).addClass('active');
            $('.erma-globalService-content').css("display", "block");
            break;
        case 'erma-warrantyStatus-tab':
		  $(".erma-login-tabs ul li").removeClass("active");
    $(".erma-contents").css("display", "none");

    $('.' + activeTab).addClass('active');
            $('.erma-warrantyStatus-content').css("display", "block");

            break;
        case 'erma-repairStatus-tab':
		  $(".erma-login-tabs ul li").removeClass("active");
    $(".erma-contents").css("display", "none");

    $('.' + activeTab).addClass('active');
            $('.erma-repairStatus-content').css("display", "block");
            break;
        default:
            break;
    }
})


function displayFun(className) {
    let displayType = $(className)[0].style.display;
    if (displayType != "none")
        $(className).css("display", "none");
    else
        $(className).css("display", "block");
}


$('.erma-contents #select_g1').on('change', function (e) {
    let optionSelected = $("option:selected", this);
    let valueSelected = this.value;
    serviceLocationDisplay(valueSelected);
});

function selectInformationFun() {
    let valueSelected = $("#select_g1 option:selected")[0].value;
    serviceLocationDisplay(valueSelected);
}

function serviceLocationDisplay(selected) {
    $(".erma-datagrid-serviceLocation").css("display", "none");
    $("#" + selected).css("display", "table-row");
}
