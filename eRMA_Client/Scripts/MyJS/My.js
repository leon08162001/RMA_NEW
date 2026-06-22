let roundDecimal = function (val, precision) {
    return Math.round(Math.round(val * Math.pow(10, (precision || 0) + 1)) / 10) / Math.pow(10, (precision || 0));
}

function ToJavaScriptDate(value) {
    var pattern = /Date(([^)]+))/;
    var results = pattern.exec(value);
    var dt = new Date(parseFloat(results[1]));
    return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
}

//let parsed = JSON.parse(data, function (key, value) {
//    if (typeof value === 'string') {
//        var d = /\/Date\((\d*)\)\//.exec(value);
//        return (d) ? new Date(+d[1]) : value;
//    }
//    return value;
//});

function buttonClick(url) {
    //const locationHref = location.href;
    //alert("locationHref : " + locationHref);
    //const findLast = locationHref.substring(locationHref.lastIndexOf("/"), locationHref.length);
    //let locationURL = locationHref.replace(findLast, "/" + url);
    //alert("locationURL : " + locationURL);
    //window.location.href = locationURL;

    let locationURL = location.origin + "/" + url;
    window.location.href = locationURL;
}