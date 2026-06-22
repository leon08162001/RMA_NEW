<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ucImportRepairDetail.ascx.vb" Inherits="ascx_ucImportRepairDetail" %>

<asp:Panel ID="UI_panel" runat="server" Width="700px" style="display:none;position:absolute">
<fieldset class = "form_div" style="width: 100%">

<table border="0" bgcolor="#E3D8BE" width="100%" id="table2" cellspacing="1" cellpadding="0" class="TableListdownright" align="center" >
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
            <asp:TextBox ID="UI_txtSerial" runat="server" Width ="150"></asp:TextBox>
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
        <td align="left" class="default">&nbsp;<%--<font class="default_Red">*</font>--%>
            <asp:Label ID="UI_lblDescription" runat="server" Text=""></asp:Label>
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