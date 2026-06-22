<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ucPopProblem.ascx.vb" Inherits="ascx_ucPopProblem" %>

    
<asp:Panel ID="UI_panel" runat="server"  >

    <script type="text/javascript">
function ClearFile(){
    var html_File  = window.document.getElementById("<%=me.html_File.ClientID%>");
    var html_FullFile  = window.document.getElementById("<%=me.html_FullFile.ClientID%>");

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
    var UI_WEBURL  = window.document.getElementById("<%=me.UI_WEBURL.ClientID%>").value;
    var UI_VisualPath  = window.document.getElementById("<%=me.UI_VisualPath.ClientID%>").value;
    var html_FullFile  = window.document.getElementById("<%=me.html_FullFile.ClientID%>").value;

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

    <fieldset class = "form_div" style="width:100%">

<table border="0" class="table"  style="width:100%;background:#ffffff;border:3px solid #808080;" align="center" border="0" cellspacing="1"  >
    <tr>
        <td align="left" colspan ="4" >
            <asp:Label ID="UI_lblProblemTittle" runat="server" Text="022_Specification / Problem" Font-Bold="true" class="default"></asp:Label>
        </td>
    </tr>

    <tr>
        <td align="left" width ="25%" class="default">&nbsp;&nbsp;&nbsp;
            <asp:Label ID="UI_lblSerial" runat="server" Text="013_Serial Number"></asp:Label>
        </td>
        <td align="left" width ="40%" class="default">:&nbsp;
            <asp:TextBox ID="UI_txtSerial" runat="server" Width ="150" Visible="false"></asp:TextBox>
            <asp:Label ID="UI_SHOWSERIAL" runat="server" Text="Label"></asp:Label>
        </td>
        <td align="left" width ="10%" class="default">
            <asp:Label ID="UI_lblModel" runat="server" Text="012_Model No."></asp:Label>
        </td>
        <td align="left" width ="25%" class="default">:
            <asp:DropDownList runat="server" ID="UI_cboModel" Width="100px"></asp:DropDownList>
            <ajaxToolkit:ListSearchExtender id="LSE" runat="server"
                TargetControlID="UI_cboModel"
                PromptText="Model search"
                PromptCssClass="ListSearchExtenderPrompt"
                PromptPosition="Top" />
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
            <asp:DropDownList ID="UI_cboFailureClass" runat ="server" AutoPostBack="true" ></asp:DropDownList>
            <asp:DropDownList ID="UI_cboFailure" runat ="server"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td align="left" class="default">&nbsp;&nbsp;&nbsp;
            <asp:Label ID="UI_lblFile" runat="server" Text="024_Upload File"></asp:Label>
        </td>
        <td align="left" colspan="3" class="default"><iframe runat="server" id="ifrmFileUpload" width="70%" height="40px" frameborder="0" scrolling="no" ></iframe></td>
    </tr>
    <tr>
        <td>&nbsp;</td>
        <td align="left" colspan="3" class="default">
            <asp:Label runat="server" id="lblFileUpload_Title" Text="_048目前已上傳檔案"></asp:Label>:&nbsp;
            <asp:TextBox runat="server" ID="html_File" ReadOnly="true" BackColor="#ededed"></asp:TextBox>
            <asp:Button runat="server" id="html_FileUpload_Clear" Text="046_Clear File" CssClass="Message_Alert" Width="90px" height="30px" OnClientClick="return ClearFile();"/>
            <asp:TextBox runat="server" ID="html_FullFile" Enabled="false" BackColor="#ededed" Width="0px" Height="0px" style="visibility:hidden"></asp:TextBox>
            <asp:Button runat="server" id="UI_butDownLoad" Text="052_DownLoad" CssClass="Message_Alert" Width="90px" height="30px"  OnClientClick="return DownLoadFile();"/>
            <br />&nbsp;&nbsp;
            <asp:Label runat="server" ID="UI_UploadFileDesc" ForeColor="red" Text="186_上傳容量(最大5MB)" ></asp:Label>
        </td>
    </tr>

    <tr>
        <td align="left" class="default">&nbsp;<%--<font class="default_Red">*</font>--%><asp:Label ID="UI_lblDescription" runat="server" Text="025_Problem Description"></asp:Label>
        </td>
        <td align="left" colspan="3" class="default">&nbsp;&nbsp;
            <asp:TextBox ID="UI_txtDescription" runat="server" TextMode="MultiLine" Rows="5" Columns="45" MaxLength="2000"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align="center" colspan="4" class="default">
            <asp:Button ID="UI_cmdCancel" runat ="server" Text ="_Cancel" CssClass ="Problem_Edit" CausesValidation="false" />
            <asp:Button ID="UI_cmdConfirmed" runat ="server" Text ="_Confirmed" CssClass ="Confirm_l" ValidationGroup ="vsUCSerial" />
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