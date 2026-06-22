var _gMessage = "";
var _gShowMessage = "";
var ProgressLoading = "";
var ProgressStatus = "";
var ProgressLoadingElementID = "";
var blnProgressLoading = false;

//_gShowMessage = Progress_ProcessMsg;

function onProgress(sMesage){
    _gMessage = sMesage;

    switch (_gMessage.toLowerCase()){
        case "Save".toLowerCase():
            _gShowMessage = Progress_SaveMsg;
            break;

        case "Load".toLowerCase():
            _gShowMessage = Progress_LoadMsg;
            break;

        case "Delete".toLowerCase():
            _gShowMessage = Progress_DelMsg;
            break;
            
        case "Export".toLowerCase():
            _gShowMessage = Progress_ExportMsg;
            break;
            
        case "Process".toLowerCase():
            _gShowMessage = Progress_ProcessMsg;
            break;

        case "Cancel".toLowerCase():
            _gShowMessage = Progress_CancelMsg;
            break;

    }
}


function beginReq(sender, args) {
    if (ProgressLoading!=null && ProgressLoading!=""){
        showProgressLoading(args)
    }
    
    if (window.document.getElementById(ProgressStatus)!=null){
        showProgressStatus(args)
    }
}




function showProgressLoading(args){
    var arrProgress=ProgressLoading.split(",");
    for (var i=0;i<arrProgress.length;i++){
        sProgress = arrProgress[i];

        var oProgressLoading = window.document.getElementById(sProgress)
        
        var innerHTML = "";
        innerHTML = innerHTML + "<img id='imgDataLoading' src='images/ProgressLoading.gif' border=0 />";
        innerHTML = innerHTML + "<label id='lblDataLoading' style='color: DarkRed;'>006_資料處理中,請稍後...</label>";
        oProgressLoading.innerHTML = innerHTML;

        if (ProgressLoadingElementID!=""){
            var arrNotProgress = ProgressLoadingElementID.split("$");
            for (var j=0;j<arrNotProgress.length;j++){
    
                var arrProgressElement = arrNotProgress[j].split("#");
                if (arrProgressElement[0]==sProgress){

                    var arrElement = arrProgressElement[1].split(",");
                    for (var k=0;k<arrElement.length;k++){
                        if (args.get_postBackElement().id==arrElement[k]){
                            oProgressLoading.innerHTML = "";
                            break;
                        }
                    }
                
                }
                
            }
        }
    }        
}



function showProgressStatus(args){
    if (postBackElementID!=""){
        var arrElement = postBackElementID.split(",");
        for (var i=0;i<arrElement.length;i++){
            if (args.get_postBackElement().id==arrElement[i]){
                return false;
            }
        }
    }
    
    

	// shows the Popup 
//    var iWidth = window.screen.availWidth;    
//    var iHeight = window.screen.availHeight;

//    var iWidth = document.body.offsetWidth;
//    var iHeight = document.body.offsetHeight;
    var iWidth = document.body.offsetWidth +50;
    var iHeight = document.body.offsetHeight+50;
           
	var DivBox = window.document.getElementById("DivBox");

	DivBox.style.width=iWidth + "px";
	DivBox.style.height=iHeight + "px";
	DivBox.style.display="block";
	
    if (_gShowMessage==""){
	    _gShowMessage = Progress_ProcessMsg;
    }
	
	var oProgressStatus = window.document.getElementById(ProgressStatus);
	var innerHTML = "<div class='Prog_container'>";
	innerHTML = innerHTML + "<div class='Prog_header'>" + _gShowMessage + "</div>";
	innerHTML = innerHTML + "<div class='Prog_body'><img src='images/activity.gif' /></div>";
	innerHTML = innerHTML + "</div>";
	
	oProgressStatus.innerHTML = innerHTML;

	$find(ModalProgress).show();
}



function endReq(sender, args) {
	//  shows the Popup 
    if (window.document.getElementById(ProgressStatus)!=null){
	    var DivBox = window.document.getElementById("DivBox");
	    DivBox.style.display="none";
	    $find(ModalProgress).hide();
    }
} 
