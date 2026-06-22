<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MainMenu.ascx.vb" Inherits="ascx_MainMenu" %>
<td>
    <asp:HyperLink runat="server" id="UI_linButton01">
    <asp:Image runat="server" ID="button_01" name="button_01" ImageUrl="~/images/pic_04.gif"  onMouseOver="img_act('button_01');return" onMouseOut="img_inact('button_01')"></asp:Image>
    </asp:HyperLink>    

    <asp:HyperLink runat="server" id="UI_linButton02">
    <asp:Image runat="server" ID="button_02" name="button_02" ImageUrl="~/images/pic_05.gif"  onMouseOver="img_act('button_02');return" onMouseOut="img_inact('button_02')"></asp:Image>
    </asp:HyperLink>    

    <asp:HyperLink runat="server" id="UI_linButton03">
    <asp:Image runat="server" ID="Image1" name="button_03" ImageUrl="~/images/pic_06.gif"  onMouseOver="img_act('button_03');return" onMouseOut="img_inact('button_03')"></asp:Image>
    </asp:HyperLink>    


    <asp:HyperLink runat="server" id="UI_linButton07">
    <asp:Image runat="server" ID="button_07" name="button_07" ImageUrl="~/images/pic_menu_06.gif"  onMouseOver="img_act('button_07');return" onMouseOut="img_inact('button_07')"></asp:Image>
    </asp:HyperLink>    


    <asp:HyperLink runat="server" id="UI_linButton04">
    <asp:Image runat="server" ID="button_04" name="button_04" ImageUrl="~/images/pic_07.gif"  onMouseOver="img_act('button_04');return" onMouseOut="img_inact('button_04')"></asp:Image>
    </asp:HyperLink>    


    <asp:HyperLink runat="server" id="UI_linButton05">
    <asp:Image runat="server" ID="button_05" name="button_05" ImageUrl="~/images/pic_08.gif"  onMouseOver="img_act('button_05');return" onMouseOut="img_inact('button_05')"></asp:Image>
    </asp:HyperLink>    

    <asp:HyperLink runat="server" id="UI_linButton06">
    <asp:Image runat="server" ID="button_06" name="button_06" ImageUrl="~/images/pic_24.gif"  onMouseOver="img_act('button_06');return" onMouseOut="img_inact('button_06')"></asp:Image>
    </asp:HyperLink>  
    
    <!--JW/shaili-->
    <asp:hyperlink runat="server" id="UI_linButton08">
    <asp:Image runat="server" ID="button_08" name="button_08" ImageUrl="~/images/pic_menu_07.gif"  onMouseOver="img_act('button_08');return" onMouseOut="img_inact('button_08')"></asp:Image>
    </asp:hyperlink>  
    <%--逾期超過1個月的RMA報價單進行強制取消作業 by buck add 20251201 begin--%>
    <asp:hyperlink runat="server" id="UI_linButton09">
    <asp:Image runat="server" ID="button_09" name="button_09" ImageUrl="~/images/pic_menu_07.gif"  onMouseOver="img_act('button_09');return" onMouseOut="img_inact('button_09')"></asp:Image>
    </asp:hyperlink>  
    <%--逾期超過1個月的RMA報價單進行強制取消作業 by buck add 20251201 end--%>
</td>

    
<td align="right" class="default">
    <b><img border="0" src="Images/icon-arrow.gif" width="5" height="9"></b>
    <asp:Label ID="UI_lblLanguage" runat="server" Text="Language:" CssClass="default"></asp:Label>
	<asp:DropDownList ID="UI_cboLanguage" runat ="server" AutoPostBack="true">
    </asp:DropDownList>&nbsp;&nbsp;&nbsp; 
</td>


