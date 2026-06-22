<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ucSpecialSetting.ascx.vb" Inherits="ascx_ucSpecialSetting" %>

<asp:Panel ID="UI_panel" runat="server"  style="display:none;position:absolute;width:70%;border:3px solid #808080;background:#ffffff;">
    <div align="center" style="padding-left: 2px;height:600px; overflow-y: scroll;">
    <fieldset class = "form_div" align="top" style="width: 100%">
    <table id="table2" class="table"  style="width:100%;" align="center" border="0" cellspacing="1" > 
        <tr class="default" valign="top">
            <td align="center">
            <table border="0" width="100%" id="table1" cellspacing="0" cellpadding="0">
                <tr>
                    <td valign="top" align="left">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0" height="100%">
                            <!--[Begin]Tittle-->
                            <tr>
                                <td width="3%">
                                    &nbsp;
                                </td>
                                <td width="94%" align="left">
                                    <asp:Label ID="UI_lblTittle" runat="server" Text="035_Warrenty Setting- Special"
                                        CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
                                </td>
                                <td width="3%">
                                    &nbsp;
                                </td>
                            </tr>
                            <!--[End]Tittle-->
                            <!--[Begin]新增資料-->
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td align="left">
                                    <table width="100%" align="center" border="0" cellspacing="1" class="default">
                                        <tr>
                                            <td width="100px" align="left">
                                                <asp:Label ID="UI_lblOperationCenter" runat="server" Text="026_Operation Center"></asp:Label>
                                            </td>
                                            <td width="1" align="left">
                                                :
                                            </td>
                                            <td width="18%" align="left">
                                                <asp:DropDownList ID="UI_cboOperationCenter" runat="server" Width="150px">
                                                </asp:DropDownList>
                                            </td>
                                            <td width="12%" align="right">
                                                <asp:Label ID="UI_lblWarrantyType" runat="server" Text="024_Warranty Type"></asp:Label>
                                            </td>
                                            <td align="left">
                                                :
                                                <asp:DropDownList ID="UI_cboWarrantyType" runat="server" Width="120px" AutoPostBack="true">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="UI_cboWarrantyTypeoth" runat="server" Width="80px">
                                                    <asp:ListItem Value="1" Text="Exclusive"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="Inclusive"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td width="10%" align="right">
                                                <asp:Label ID="UI_lblExtraMonths" runat="server" Text="031_Extra Months"></asp:Label>
                                            </td>
                                            <td width="12%" align="left">
                                                :
                                                <asp:TextBox ID="UI_ExtraMonths" runat="server" Width="80" Enabled="False"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="UI_lblProductGroup" runat="server" Text="Model"></asp:Label>
                                            </td>
                                            <td width="1" align="left">
                                                :
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="UI_txtProductGroup" ReadOnly="true"  Enabled="False" runat="server" Width="150px"></asp:TextBox>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="UI_lblWarrantyName" runat="server" Text="029_Warranty Name"></asp:Label>
                                            </td>
                                            <td align="left">
                                                :
                                                <asp:TextBox ID="UI_txtWarrantyName" runat="server" Width="200" Enabled="False"></asp:TextBox>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="UI_lblYears" runat="server" Text="032_Years"></asp:Label>
                                            </td>
                                            <td align="left">
                                                :
                                                <asp:TextBox ID="UI_Years" runat="server" Width="80" Enabled="False"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="UI_lblVersion" runat="server" Text="028_Version"></asp:Label>
                                            </td>
                                            <td width="1" align="left">
                                                :
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="UI_txtVersion" ReadOnly="true"  Enabled="False"  runat="server" Width="150px" MaxLength="8"></asp:TextBox>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="UI_lblDescription" runat="server" Text="036_Description"></asp:Label>
                                            </td>
                                            <td align="left">
                                                :
                                                <asp:TextBox ID="UI_txtDescription" runat="server" Width="200" Enabled="False"></asp:TextBox>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="UI_lblUnitPrice" runat="server" Text="037_Unit Price"></asp:Label>
                                            </td>
                                            <td align="left">
                                                :
                                                <asp:TextBox ID="UI_UnitPrice" runat="server" Width="80" Enabled="False"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="lblExpDate" runat="server" Text="Expired Date"></asp:Label>
                                            </td>
                                            <td width="1" align="left">
                                                :
                                            </td>
                                            <td align="left" colspan="5">
                                                <asp:TextBox ID="txtExpDate"  runat="server"  Width="150px" Enabled="False"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <!--[End]新增資料-->
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="left">
                        <asp:Panel ID="pnlFile" runat="server" Width="100%">
                            <table border="0" width="100%" id="Step2" cellspacing="0" cellpadding="0" style="vertical-align: top;">
                                <tr align="left">
                                    <td height="27" class="default">
                                        <b><asp:Label ID="lblPartsInform" runat="server" Text=" Parts Information"></asp:Label></b>
                                   </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="center">
                                        <table width="100%" align="center" border="0" cellspacing="1" class="default">
                                            <tr>
                                                <td align="left" colspan="4">
                                                       <asp:DataList ID="lstFile" runat="server" Width="100%" HorizontalAlign="left"
                                                            CellPadding="0" CellSpacing="0" RepeatColumns="10" RepeatDirection="Horizontal">
                                                            <ItemTemplate>
                                                                <table width="10%" border="0" cellspacing="0" cellpadding="0" class="box_table">
                                                                <tr>
                                                                    <td>
                                                                        <a target="_blank" href="<%=sUploadUrl %>/<%#DataBinder.Eval(Container.DataItem, "SWF_FILE")%>"><%#DataBinder.Eval(Container.DataItem, "SWF_FILE")%></a>
                                                                    </td>
                                                                </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:DataList>
                                                </td>
                                            </tr>
                                           <asp:Panel ID="pnParts" runat="server" Width="100%">
                                            <tr valign="top">
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td valign="top" width="30%">
                                                    <table width="100%" border="1" cellspacing="0" cellpadding="0" class="box_table" style="vertical-align:top;">
                                                        <tr class="Text_Head">
                                                            <td>
                                                                <asp:Label ID="lblPartAllPart" runat="server" Text="Parts No"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblPartAllDesc" runat="server" Text="Description"></asp:Label> 
                                                            </td>
                                                        </tr>
                                                        <asp:DataList ID="lstAllPartNo" runat="server" Width="100%" HorizontalAlign="left"
                                                            CellPadding="0" CellSpacing="0" RepeatLayout="Flow" RepeatColumns="1000000">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td>
                                                                        <%#DataBinder.Eval(Container.DataItem, "swp_partno")%>
                                                                    </td>
                                                                    <td>
                                                                        <%#DataBinder.Eval(Container.DataItem, "swp_desc")%>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:DataList>
                                                    </table>
                                                </td>
                                                <td align="center" valign="top">
                                                    To
                                                </td>
                                                <td align="left" valign="top" width="65%">
                                                    <table width="100%" border="1" cellspacing="0" cellpadding="0" class=" box_table">
                                                        <tr class="Text_Head">
                                                            <td width="30%">
                                                                 <asp:Label ID="lblPartOKPart" runat="server" Text="Parts No"></asp:Label>
                                                            </td>
                                                            <td width="35%">
                                                                <asp:Label ID="lblPartOKDesc" runat="server" Text="Description"></asp:Label>
                                                            </td>
                                                            <td width="20%">
                                                                <asp:Label ID="lblPartOKYears" runat="server" Text="Years"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <asp:DataList ID="lstOKPartNo" runat="server" Width="100%" HorizontalAlign="left"
                                                            CellPadding="0" CellSpacing="0" RepeatLayout="Flow" RepeatColumns="1000000">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td>
                                                                        <%# DataBinder.Eval(Container.DataItem, "swp_partno")%>
                                                                    </td>
                                                                    <td>
                                                                       <%# DataBinder.Eval(Container.DataItem, "swp_desc")%>
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:TextBox ID="txtYears" runat="server"  Enabled="False" Width="50px" Text='<%# DataBinder.Eval(Container.DataItem, "swp_year")%>'></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:DataList>
                                                    </table>
                                                </td>
                                            </tr>
                                            </asp:Panel>
                                            <asp:Panel ID="pnDesc" runat="server" Width="100%">
                                                <tr>
                                                        <td align="right">
                                                            <asp:Label ID="lblDescription" runat="server" Text="Description"></asp:Label>
                                                        </td>
                                                        <td align="left" colspan="3">
                                                            :
                                                            <asp:TextBox ID="txtDescription" runat="server" Width="650" height="250" TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                    </tr> 
                                                </asp:Panel>                                
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>                    
                    </td> 
                </tr> 
            </table>
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


<asp:button id="UI_butTarget" Width="0px" Height="0px" runat="server" style="display:none"></asp:button>
<asp:button id="UI_butOK" Width="0px" Height="0px" runat="server" style="display:none"></asp:button>


</asp:Panel>

<ajaxToolkit:ModalPopupExtender ID="ajModalProgress" TargetControlID="UI_butTarget"  
PopupControlID="UI_panel"  
OkControlID="UI_butOK" 
CancelControlID="UI_cmdClose"
BackgroundCssClass = "modalBackground"
runat="server">
</ajaxToolkit:ModalPopupExtender>
