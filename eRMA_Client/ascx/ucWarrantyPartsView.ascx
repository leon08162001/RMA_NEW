<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ucWarrantyPartsView.ascx.vb" Inherits="ascx_ucWarrantyPartsView" %>

<asp:Panel ID="UI_panel" runat="server"  style="display:none;position:absolute;width:70%;border:3px solid #808080;background:#ffffff;">
    <div align="center">
        <fieldset class="form_div" align="top" style="width: 100%">
            <table id="table2" class="table"  style="width:100%;" align="center" border="0" cellspacing="1" >
                <tr class="default">
                    <td align="left">
                        <asp:Label ID="lblTitle" runat="server" Text="Warranty Parts List"></asp:Label>
						<asp:Label ID="lblTitle2" runat="server" Text="test"></asp:Label>
                    </td>
                </tr>
                <tr class="default" valign="top">
                    <td align="center">
                        <div align="center">
                            <table width="95%" border="1" cellspacing="0" cellpadding="0" class=" box_table">
                                <tr class="Text_Head">
                                    <td width="5%">
                                        <asp:Label ID="lblPartSeq" runat="server" Text="#"></asp:Label>
                                    </td>
                                    <td width="15%">
                                        <asp:Label ID="lblPartPO" runat="server" Text="PO Date"></asp:Label>
                                    </td>
                                    <td width="20%">
                                        <asp:Label ID="lblPartContent" runat="server" Text="Warranty Content"></asp:Label>
                                    </td>
                                    <td width="15%">
                                        <asp:Label ID="lblPartMonth" runat="server" Text="Warranty Month"></asp:Label>
                                    </td>
                                    <td width="15%">
                                        <asp:Label ID="lblPartExtra" runat="server" Text="Extra Month"></asp:Label>
                                    </td>
                                    <td width="15%">
                                        <asp:Label ID="lblPartMemo" runat="server" Text="Memo"></asp:Label>
                                    </td>
                                    <td width="15%">
                                        <asp:Label ID="lblPartEndDate" runat="server" Text="Warranty End Date"></asp:Label>
                                    </td>
                                </tr>
                                <asp:DataList ID="lstParts" runat="server" Width="100%" HorizontalAlign="left"
                                    CellPadding="0" CellSpacing="0" RepeatLayout="Flow" RepeatColumns="1000000">
                                    <ItemTemplate>
                                        <tr>
                                            <td style="text-align: center">
                                                <%# (Container.ItemIndex + 1).ToString("00") + "." %>
                                            </td>
                                            <td align="center">
                                                <%# DataBinder.Eval(Container.DataItem, "PODate")%>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "WAP_NAME")%>
                                            </td>
                                            <td align="center">
                                                <%# DataBinder.Eval(Container.DataItem, "WAP_MON")%>
                                            </td>
                                            <td align="center">
                                                <%# DataBinder.Eval(Container.DataItem, "WAP_EMON")%>
                                            </td>
                                            <td align="center">
                                                <%# DataBinder.Eval(Container.DataItem, "WAP_DESC")%>
                                            </td>
                                            <td align="center">
                                                <font color="red"><%# DataBinder.Eval(Container.DataItem, "WarrEndDate")%></font>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:DataList>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="center">&nbsp;&nbsp;
                <asp:Button ID="UI_cmdClose" runat="server" Text="_Close" CssClass="Problem_Edit" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>


    <asp:Button ID="UI_butTarget" Width="0px" Height="0px" runat="server" Style="display: none"></asp:Button>
    <asp:Button ID="UI_butOK" Width="0px" Height="0px" runat="server" Style="display: none"></asp:Button>
	


</asp:Panel>

<ajaxToolkit:ModalPopupExtender ID="ajModalProgress" TargetControlID="UI_butTarget"
    PopupControlID="UI_panel"
    OkControlID="UI_butOK"
    CancelControlID="UI_cmdClose"
    BackgroundCssClass="modalBackground"
    runat="server">
</ajaxToolkit:ModalPopupExtender>
