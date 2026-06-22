<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ucPopProblem_new.ascx.vb" Inherits="ascx_ucPopProblem_new" %>


<asp:Panel ID="UI_panel" runat="server"  style="display:none;position:absolute;width:70%;border:3px solid #808080;background:#ffffff;" >


<link href="../css/system.css" rel="stylesheet" />
<link href="../css/commonStyle.css" rel="stylesheet" />
<link href="../css/system-header.css" rel="stylesheet" />
<link href="../NeatUpload/default.css" rel="stylesheet" />
<script src="../js/jquery-3.5.0.js"></script>

<script type="text/javascript">
    function ClearFile() {

    var html_File  = window.document.getElementById("<%=Me.html_File.ClientID%>");
    var html_FullFile  = window.document.getElementById("<%=Me.html_FullFile.ClientID%>");

	if (!confirm(delMsg))   {
		return false;

	}else{
        if (html_File!=null){
            html_File.value="";
        }
        
        if (html_FullFile!=null){
            html_FullFile.value="";
        }
	}

    return false;
}


function DownLoadFile(){
    var UI_WEBURL  = window.document.getElementById("<%=Me.UI_WEBURL.ClientID%>").value;
    var UI_VisualPath  = window.document.getElementById("<%=Me.UI_VisualPath.ClientID%>").value;
    var html_FullFile  = window.document.getElementById("<%=Me.html_FullFile.ClientID%>").value;

    if (Trim(html_FullFile)!=''){
        var FileName = html_FullFile.split(",");
        var sURL = UI_WEBURL + UI_VisualPath + FileName[1];
        var sFeatures="width=300,height=200,scrollbars=yes";
        OpenWin(sURL,"winDownLoad",sFeatures);
    }
    
    return false;

}


    
function Validate_FailureClass( oSrc, args ) {
    var sValue1 = document.getElementById('<%=UI_cboFailureClass.ClientID %>').value; 

    if (sValue1==-1){
        args.IsValid = false;
    }else{
        args.IsValid = true;
    }
}

function Validate_Failure( oSrc, args ) {
    var sValue1 = document.getElementById('<%=UI_cboFailure.ClientID %>').value; 

    if (sValue1==-1){
        args.IsValid = false;
    }else{
        args.IsValid = true;
    }
}

function chkWarranty(objName){
    objName.checked=false;
}
    
    </script>

    <div  style="background:#ffffff;padding:25px;border-radius:10px;" >
        <div class="erma-window-box"   >
            <div class="erma-box-div"    >
                <h7>Specification / Problem</h7>
                <div class="erma-image-cancel" onclick="displayClose('.erma-window-background')">
                    <img src="../images_new/cancel.svg" />
                </div>
            </div>
            <div class="erma-problem-content">
                <div class="erma-fault-block">
                    <div class="erma-fault-left">
                        <p class="erma-title">Serial Number:</p>
                             <asp:Label ID="UI_SHOWSERIAL" runat="server" Text="Label"></asp:Label>
            
                    </div>
                    <div  "erma-fault-right" >
                          <p class="erma-title"   style="color:#ff0000" >Customer Product Number:</p>
                           <asp:TextBox ID="Customer_Product_Txt" runat="server"   CssClass="erma-input-input" ></asp:TextBox>
                    </div>

                </div>
                <div class="erma-fault-block">
                    <div class="erma-fault-left">
                        <p class="erma-title">* Fault</p>
                        <div class="erma-number-input erma-fault-option">
                            <div class="erma-fault erma-combobox-control">
                                        <asp:DropDownList ID="UI_cboFailureClass" runat ="server" AutoPostBack="true" CssClass="erma-combobox-choose" ></asp:DropDownList>
                            </div>
                            <div class="erma-fault erma-combobox-control">
                                     <asp:DropDownList ID="UI_cboFailure" runat ="server"  CssClass="erma-combobox-choose" ></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="erma-fault-right">
                        <p class="erma-title">Model No:</p>
                        <div class="erma-modelNo-option">
                            <div class="erma-modelNo erma-combobox-control">
           
                                <asp:DropDownList runat="server" ID="UI_cboModel"  CssClass="erma-combobox-choose"  ></asp:DropDownList>
                                <ajaxToolkit:ListSearchExtender id="LSE" runat="server"
                                TargetControlID="UI_cboModel"
                                PromptText="Model search"
                                PromptCssClass="ListSearchExtenderPrompt"
                                PromptPosition="Top" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="erma-number-input erma-uploadFile-input erma-number-undefined">
                    <div class="erma-number-box">
                        <div class="erma-title">
         
                        </div>
                        <div class="erma-uploadFile-selectFileDiv">
                            <div class="erma-selectFile-button">
                                <p>Select File</p>
                            </div>
                            <input class="erma-upload-button" type="submit">
                            <input class="erma-upload-clear" type="reset">
                        </div>
                    </div>
                </div>
                <div class="erma-problemDescription-block">
                    <div class="erma-description-block">
                      
                            <asp:Label ID="UI_lblDescription" CssClass="erma-title" runat="server" Text="025_Problem Description"></asp:Label>
                            <asp:TextBox ID="UI_txtDescription" runat="server" CssClass="erma-form-control" TextMode="MultiLine" Rows="5" Columns="45" MaxLength="2000"></asp:TextBox>
                     
                    </div>
                </div>
                
   
                      <asp:Button ID="UI_cmdConfirmed" runat ="server" Text ="Submit" CssClass ="erma-infomation-submitButton notButton" ValidationGroup ="vsUCSerial" />


            </div>
        </div>
        </div>

    <fieldset class = "form_div"  style="display:none;" >
   
<table border="0" bgcolor="#E3D8BE"  id="table2" cellspacing="1" cellpadding="0" class="TableListdownright" align="center" style="display:none;" >
    <tr>
        <td align="left" colspan ="4" style="background:url(Images/pic_15.gif);">
            <asp:Label ID="UI_lblProblemTittle" runat="server" Text="022_Specification / Problem" Font-Bold="true" class="default"></asp:Label>
        </td>
    </tr>

    <tr>
        <td align="left" width ="25%" class="default">&nbsp;&nbsp;&nbsp;
            <asp:Label ID="UI_lblSerial" runat="server" Text="013_Serial Number"></asp:Label>
        </td>
        <td align="left" width ="40%" class="default">:&nbsp;
            <asp:TextBox ID="UI_txtSerial" runat="server" Width ="150" Visible="false"></asp:TextBox>
    
        </td>
        <td align="left" width ="10%" class="default">
            <asp:Label ID="UI_lblModel" runat="server" Text="012_Model No."></asp:Label>
        </td>
        <td align="left" width ="25%" class="default">:
      
        </td>
    </tr>

    <tr style="display:none">
        <td align="left" class="default">&nbsp;<%--<font class="default_Red">*</font>--%><asp:Label ID="UI_lblProductDesc" runat="server" Text="196_Product Description"></asp:Label>
        </td>
        <td align="left" colspan ="3" class="default">:&nbsp;
            <asp:TextBox ID="UI_txtProductDesc" runat="server" Width ="300px" MaxLength="2000"></asp:TextBox>
        </td>
    </tr>

    <tr style="display:none">
        <td align="left" class="default">&nbsp;&nbsp;&nbsp;
            <asp:Label ID="UI_lblCustomerName" runat="server" Text="014_Customer Product Name"></asp:Label>
        </td>
        <td align="left" colspan ="3" class="default">:&nbsp;
            <asp:TextBox ID="UI_txtCustomerName" runat="server" Width ="200"></asp:TextBox>
        </td>
    </tr>

    <tr runat="server" id="UI_trWarranty" visible="false">
        <td align="left" class="default">&nbsp;&nbsp;&nbsp;
            <asp:Label ID="UI_lblWarranty" runat="server" Text="015_Warranty"></asp:Label>
        </td>
        <td align="left" colspan ="3" class="default">:&nbsp;
            <asp:CheckBox runat="server" ID="UI_chkWarranty_0" Text="066_No" />
            <asp:CheckBox runat="server" ID="UI_chkWarranty_1" Text="065_Yes" />
        </td>
    </tr>
    
    <tr runat="server" id="UI_trCWarranty" visible="false">
        <td align="left" class="default">&nbsp;&nbsp;&nbsp;
            <asp:Label ID="Label1" runat="server" Text="CW Warranty"></asp:Label>
        </td>
        <td align="left" colspan ="3" class="default">:&nbsp;
            <asp:CheckBox runat="server" ID="UI_chkWarranty_C0" Text="066_No" />
            <asp:CheckBox runat="server" ID="UI_chkWarranty_C1" Text="065_Yes" />
        </td>
    </tr>
    <tr runat="server" id="UI_trSWarranty" visible="false">
        <td align="left" class="default">&nbsp;&nbsp;&nbsp;
            <asp:Label ID="Label2" runat="server" Text="SW Warranty"></asp:Label>
        </td>
        <td align="left" colspan ="3" class="default">:&nbsp;
            <asp:CheckBox runat="server" ID="UI_chkWarranty_S0" Text="066_No" />
            <asp:CheckBox runat="server" ID="UI_chkWarranty_S1" Text="065_Yes" />
        </td>
    </tr>
    <tr>
        <td align="left" class="default">&nbsp;<font class="default_Red">*</font>
            <asp:Label ID="UI_lblFailure" runat="server" Text="023_Failure Reason"></asp:Label>
        </td>
        <td align="left" colspan ="3" class="default">:&nbsp;
            
                        <asp:Label ID="UI_lblFile" runat="server" Text="024_Upload File"></asp:Label>
                        <asp:Label runat="server" id="lblFileUpload_Title" Text="_048目前已上傳檔案"></asp:Label>:
                        <asp:TextBox runat="server" ID="html_File" ReadOnly="true" BackColor="#ededed"></asp:TextBox>
                        <span   class="erma-infomation-addButton"  onclick="return ClearFile();"  style="width:65px" > Clear File</span>
                        <asp:Button runat="server" id="html_FileUpload_Clear" Visible="false" Text="046_Clear File" CssClass="Message_Alert" Width="60px" height="19px" OnClientClick="return ClearFile();"/>
                        <asp:TextBox runat="server" ID="html_FullFile" Enabled="false" BackColor="#ededed" Width="0px" Height="0px" style="visibility:hidden"></asp:TextBox>
                        <asp:Button runat="server" id="UI_butDownLoad" Visible="false" Text="052_DownLoad" CssClass="Message_Alert" Width="70px" height="19px"  OnClientClick="return DownLoadFile();"/>
                        <span    class="erma-infomation-addButton"  onclick="return DownLoadFile();"  style="width:65px"  > DownLoad</span>
                        <asp:Label runat="server" ID="UI_UploadFileDesc" ForeColor="red" Text="186_上傳容量(最大5MB)   這是新頁面" ></asp:Label>
        </td>
    </tr>
    <tr>
        <td align="left" class="default">&nbsp;&nbsp;&nbsp;
         <iframe runat="server" id="ifrmFileUpload" width="70%" height="40px" frameborder="0" scrolling="no" ></iframe>
        </td>
        <td align="left" colspan="3" class="default"></td>
    </tr>
    <tr>
        <td>&nbsp;</td>
        <td align="left" colspan="3" class="default">
          
        </td>
    </tr>

    <tr>
        <td align="left" class="default">&nbsp;<%--<font class="default_Red">*</font>--%></td>
        <td align="left" colspan="3" class="default">&nbsp;&nbsp;
        
        </td>
    </tr>
    <tr>
        <td align="center" colspan="4" class="default">
            <asp:Button ID="UI_cmdCancel" runat ="server" Text ="_Cancel" CssClass ="Problem_Edit" CausesValidation="false" />
      
        </td>
    </tr>
</table>

    </fieldset>	

<asp:TextBox id="UI_WEBURL" Width="0px" Height="0px" runat="server" style="display:none"></asp:TextBox>
<asp:TextBox id="UI_VisualPath" Width="0px" Height="0px" runat="server" style="display:none"></asp:TextBox>

 <asp:Label ID="UI_lblAccountID" runat="server" Visible="false"></asp:Label>
<asp:Label ID="UI_lblRepairNo" runat="server" Visible="false"></asp:Label>
<asp:Label ID="UI_flowCase" runat="server" Visible="false"></asp:Label>

<asp:button id="UI_butTarget" Width="0px" Height="0px" runat="server" style="display:none"></asp:button>
<asp:button id="UI_butOK" Width="0px" Height="0px" runat="server" style="display:none"></asp:button>


<asp:RequiredFieldValidator ID="rfv_ProductDesc" runat="server" ErrorMessage = "198_Product Desc不可空白" Display="None" ControlToValidate="UI_txtProductDesc" ValidationGroup ="vsUCSerial" SetFocusOnError="true" Visible="false"></asp:RequiredFieldValidator>
<asp:RequiredFieldValidator ID="rfv_ProblemDesc" runat="server" ErrorMessage = "199_Problem Desc不可空白" Display="None" ControlToValidate="UI_txtDescription" ValidationGroup ="vsUCSerial" SetFocusOnError="true" Visible="false"></asp:RequiredFieldValidator>

<asp:CustomValidator ID="cv_FailureClass" runat="server" 
ClientValidationFunction="Validate_FailureClass" ErrorMessage="078_請選取不良原因類別名稱" 
Display ="None" Operator="DataTypeCheck" ValidationGroup ="vsUCSerial"></asp:CustomValidator>

<asp:CustomValidator ID="cv_Failure" runat="server" 
ClientValidationFunction="Validate_Failure" ErrorMessage="079_請選取不良原因名稱" 
Display ="None" Operator="DataTypeCheck" ValidationGroup ="vsUCSerial"></asp:CustomValidator>

<asp:ValidationSummary ID="vsUCSerial" runat ="server" ShowMessageBox ="true" ShowSummary ="false" ValidationGroup="vsUCSerial" />  

</asp:Panel>


<ajaxToolkit:ModalPopupExtender ID="ajModalProgress" TargetControlID="UI_butTarget"  
PopupControlID="UI_panel"  
OkControlID="UI_butOK" 
CancelControlID="UI_cmdCancel"
BackgroundCssClass = "modalBackground"
runat="server">
</ajaxToolkit:ModalPopupExtender>
