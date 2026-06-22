<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ucMessage.ascx.vb" Inherits="ascx_ucMessage" %>

<!-- 提示訊息 STRAT -->
<asp:Panel ID="panelMessage" runat="server"  style="display:none;position:absolute;">
   <style>     
       .Modal {
        background-color:black;
        opacity:0.5;   
       }

       .message-div-class center{
        padding: 7px;        
       }
       .message-div-class center .AlertBtn{
        padding: 9px 18px;
       }
   </style>
        <table style="background:#fff;border-radius:10px;width:400px;height:350px;border:none;padding:10px;">
        <tr>
        <td>
        <div class="message-div-class">
            <center>
            <img class="message-div-image"  src="../images_new/notice.svg" />

            <p style="font-size:15px;font-weight:bold;display:none;">
            <asp:Label ID="lblTitleMsg" runat ="server" Text="訊息提示"></asp:Label>
            </p>
            </center>  
            <center class="html_box">
            <asp:Label ID="html_Success" CssClass="text12pt" runat ="server" Text="" Font-Names="Arial"></asp:Label>
            <asp:Label ID="html_Failed" CssClass="text12pt" runat ="server" Text=""  Font-Names="Arial"></asp:Label>

            </center>    
            <center  class="UI_button">
            <asp:Panel runat="server" id="Panel_OK"><asp:Button ID="UI_butOK" runat ="server" Text ="014_OK" CssClass ="AlertBtn"  CausesValidation="false" /></asp:Panel>
            <asp:Panel runat="server" id="Panel_Cencel"><asp:Button ID="UI_butClose" runat ="server" Text ="008_Close" CssClass ="AlertBtn"  CausesValidation="false" /></asp:Panel>
            <asp:Panel runat="server" id="Panel_Alert"><asp:Button ID="UI_butAlert" runat ="server" Text ="015_Confirm" CssClass ="AlertBtn"  CausesValidation="false" /></asp:Panel>
            </center>
            <br />

            <center>
            <asp:button id="alarmButton" CssClass="AlertBtn"  runat="server" style="display:none"></asp:button>
            </center>                                    
        </div>
        </td>
        </tr>


        </table>

        </asp:Panel>
<!-- 提示訊息 END -->

<ajaxToolkit:ModalPopupExtender ID="ModalMessage" TargetControlID="alarmButton"  
PopupControlID="panelMessage"  BackgroundCssClass="Modal"
		
runat="server" DropShadow="false">
</ajaxToolkit:ModalPopupExtender>
