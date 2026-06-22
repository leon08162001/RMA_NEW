$(document).ready(function () {
});


function displayFun(className1,className2) {
    let displayType = $(className1)[0].style.display;
    if (displayType != "none")
        $(className1).css("display", "none");
    else
        $(className1).css("display", "block");


    if (className2 != null) {
        displayFun(className2);
    }
}


$(".erma-calc-number img").hover(function () {
    $(".erma-calc-number .erma-total-content").css("display", "block")
}, function () {
        $(".erma-calc-number .erma-total-content").css("display", "none")
    });


function displayClose(className) {
    $(className).css("display", "none");
}


function setTimeFun(className) {
    setTimeout(function () {
        displayFun(className);
    }, 2000);
}



$(".erma-versionModification-block input").click(function () {
    var inputProp = $(".erma-versionModification-block input").prop("checked");
        displayFun('.erma-versionModification-table-block');
    
})

$(".erma-serialNumber-input input").click(function () {
    $(this).parent().parent().addClass("select");

    $(".erma-window-newRequest-productInfo-background .erma-window-box").find(".erma-infomation-notButton").css("display", "none");
    $(".erma-window-newRequest-productInfo-background .erma-window-box").find(".erma-infomation-addButton").css("display", "block");
})

$(".erma-table-components-FAQ").parent().addClass("erma-FAQ-div")





