<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ucMessageSave.ascx.vb" Inherits="ascx_ucMessageSave" %>

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
            <img src="../images_new/notice.svg" />
            <br />
            <p style="font-size:15px;font-weight:bold;">
            <asp:Label ID="lblTitleMsg" runat ="server" Text="訊息提示"></asp:Label>
                <br />
                <span>
                <%= getoLanguage("RMA2", "119")   %> 
                </span>
                </br>
            </p>
            </center>  
            <center>
            <asp:Label ID="html_Success" CssClass="text12pt" runat ="server" ForeColor="#03a9f4" Text="" Font-Bold="true" Font-Names="Arial"></asp:Label>
            <asp:Label ID="html_Failed" CssClass="text12pt" runat ="server" ForeColor="#ff5c5c" Text="" Font-Bold="true" Font-Names="Arial"></asp:Label>

            <input id="Button1" type="button" value= <%=  getoLanguageTag()   %>  class="AlertBtn" />
            <asp:Panel runat="server" id="Panel_OK" ><asp:Button ID="UI_butOK"  Visible="false"  runat ="server" Text ="014_OK" CssClass ="AlertBtn"  CausesValidation="false" /></asp:Panel>
            </center>    
            <center>
            <asp:Panel runat="server" id="Panel_Cencel"><asp:Button ID="UI_butClose"  Visible="false"  runat ="server" Text ="008_Close" CssClass ="AlertBtn"   CausesValidation="false" /></asp:Panel>
            <asp:Panel runat="server" id="Panel_Alert"><asp:Button ID="UI_butAlert"  Visible="false"  runat ="server" Text ="015_Confirm" CssClass ="AlertBtn"  CausesValidation="false" /></asp:Panel>
            </center>
            <center>
            <asp:button id="alarmButton" CssClass="AlertBtn"  runat="server" style="display:none"></asp:button>
            </center>                                    
        </div>
        </td>
        </tr>


        </table>

        </asp:Panel>
<!-- 提示訊息 END -->

<ajaxToolkit:ModalPopupExtender ID="ModalMessage" TargetControlID="alarmButton"   BehaviorID="Alert_Save"
PopupControlID="panelMessage"  BackgroundCssClass="Modal"
		
runat="server" DropShadow="false">
</ajaxToolkit:ModalPopupExtender>
