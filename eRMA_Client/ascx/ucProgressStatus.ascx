<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ucProgressStatus.ascx.vb" Inherits="ascx_ucProgressStatus" %>

<style>
.progress
{
    display: block;
    position: absolute;
    padding: 2px 3px;
}
.Prog_container
{
    border: solid 1px #808080;
    border-width: 1px 0px;
}
.Prog_header
{
    background: url(images/sprite.png) repeat-x 0px 0px;
    border-color: #808080 #808080 #ccc;
    border-style: solid;
    border-width: 0px 1px 1px;
    padding: 0px 10px;
    color: #000000;
    font-size: 9pt;
    font-weight: bold;
    line-height: 1.9;  
    font-family: arial,helvetica,clean,sans-serif;
}
.Prog_body
{
    background-color: #f2f2f2;
    border-color: #808080;
    border-style: solid;
    border-width: 0px 1px;
    padding: 10px;
}
</style>



<script type="text/javascript" language="javascript">
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginReq);
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);

	var ModalProgress = '<%= ModalProgress.ClientID %>';         
	var ProgressStatus = '<%= ProgressStatus.ClientID %>';  
	var postBackElementID ='<%= NotpostBackElementID.Text %>';  
</script>

<div id="DivBox" style="z-index: 101; left: 0px; margin: 0px; display:none;
         position: absolute; top: 0px; width: 0px; height: 0px" class="modalBackground">
</div>


<!-- 提示訊息 STRAT -->
<asp:Panel ID="panelUpdateProgress" runat="server" Style="display:none" width="480">
    <table border="0" cellpadding="0" cellspacing="0" CssClass="progress">
        <tr>
            <td>
                <asp:UpdateProgress ID="ProgressStatus" runat="server">
                    <ProgressTemplate>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </td>
        </tr>
    </table>
</asp:Panel>


<!-- 提示訊息 END -->
<asp:button id="alarmButton" Width="0px" Height="0px" runat="server" style="display:none"></asp:button>
<asp:textbox id="NotpostBackElementID" Width="0px" Height="0px" runat="server"  style="display:none"></asp:textbox>

<ajaxToolkit:ModalPopupExtender ID="ModalProgress" TargetControlID="alarmButton"  
PopupControlID="panelUpdateProgress" 
runat="server" DropShadow="false">
</ajaxToolkit:ModalPopupExtender>
	


