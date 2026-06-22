<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ucMessage.ascx.vb" Inherits="ascx_ucMessage" %>


<!-- 提示訊息 STRAT -->
<asp:Panel ID="panelMessage" runat="server" Style="display:none">


<table border="0" width="480" id="table1" cellspacing="0" cellpadding="0">
<tr>
    <td width="32" height="28"><img border="0" src="images/message_01.gif" width="32" height="28"></td>
    <td background="images/message_02.gif"><b><font color="#FFFFFF">&nbsp;
        <asp:Label ID="lblTitleMsg" runat ="server" Text="訊息提示"></asp:Label>
        </font></b>
    </td>
    <td width="23" height="28"><img border="0" src="images/message_03.gif" width="23" height="28"></td>
</tr>

<tr>
    <td background="images/message_04.gif">&nbsp;</td>
    <td background="images/message_05.gif" height="200px" align="center">
        <asp:Label ID="html_Success" CssClass="text12pt" runat ="server" ForeColor="Blue" Text="" Font-Bold="true" Font-Names="Arial"></asp:Label>
        <asp:Label ID="html_Failed" CssClass="text12pt" runat ="server" ForeColor="Red" Text="" Font-Bold="true" Font-Names="Arial"></asp:Label>
        <br>
        <br>
        <asp:Panel runat="server" id="Panel_OK"><asp:Button ID="UI_butOK" runat ="server" Text ="014_OK" CssClass ="Message_OK" Width="70" CausesValidation="false" /></asp:Panel>
        <asp:Panel runat="server" id="Panel_Cencel"><asp:Button ID="UI_butClose" runat ="server" Text ="008_Close" CssClass ="Message_Cancel" Width="70" CausesValidation="false" /></asp:Panel>
        <asp:Panel runat="server" id="Panel_Alert"><asp:Button ID="UI_butAlert" runat ="server" Text ="015_Confirm" CssClass ="Message_Alert" Width="70" CausesValidation="false" /></asp:Panel>
    </td>
    <td background="images/message_06.gif">&nbsp;</td>
</tr>

<tr>
    <td height="18"><img border="0" src="images/message_07.gif" width="32" height="18"></td>
    <td background="images/message_08.gif"  style="no-repeat;" height="18"><img border="0" src="images/message_10.gif"></td>
    <td width="23" height="18"><img border="0" src="images/message_09.gif" width="23" height="18"></td>
</tr>
</table>

</asp:Panel>
<!-- 提示訊息 END -->

<asp:button id="alarmButton" Width="0px" Height="0px" runat="server" style="display:none"></asp:button>


<ajaxToolkit:ModalPopupExtender ID="ModalMessage" TargetControlID="alarmButton"  
PopupControlID="panelMessage"  BackgroundCssClass="modalBackground"
runat="server" DropShadow="false">
</ajaxToolkit:ModalPopupExtender>
