<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Warranty_SpecialSetting_add.aspx.vb" Inherits="Warranty_SpecialSetting_add" Title="Untitled Page" %>

<%@ Register Src="ascx/ucProgressStatus.ascx" TagName="ucProgressStatus" TagPrefix="uc1" %>
<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>
<%@ Register Src="ascx/ucProductGroup.ascx" TagName="ucProductGroup" TagPrefix="uc6" %>
<%@ Register Src="ascx/ucParts.ascx" TagName="ucParts" TagPrefix="uc7" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <uc1:ucProgressStatus ID="ucProgressStatus" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table border="0" width="100%" id="table1" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="24" background="Images/pic_12.gif">&nbsp;
                    </td>
                    <td valign="top" align="left">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0" height="100%">
                            <!--[Begin]Tittle-->
                            <tr>
                                <td width="3%">&nbsp;
                                </td>
                                <td width="94%" align="left">
                                    <asp:Label ID="UI_lblTittle" runat="server" Text="035_Warrenty Setting- Special"
                                        CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
                                </td>
                                <td width="3%">&nbsp;
                                </td>
                            </tr>
                            <!--[End]Tittle-->
                            <!--[Begin]新增資料-->
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td align="left">
                                    <table width="100%" align="center" border="0" cellspacing="1" class="default">
                                        <tr>
                                            <td colspan="7">&nbsp;<asp:Label ID="UI_lblSubTittle" runat="server" Text="038_Please fill in the header below and click button Go, system will list the price table."></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="100px" align="left">
                                                <asp:Label ID="UI_lblOperationCenter" runat="server" Text="026_Operation Center"></asp:Label>
                                            </td>
                                            <td width="1" align="left">:
                                            </td>
                                            <td width="20%" align="left">
                                                <asp:DropDownList ID="UI_cboOperationCenter" runat="server" Width="150px">
                                                </asp:DropDownList>
                                            </td>
                                            <td width="10%" align="right">
                                                <asp:Label ID="UI_lblWarrantyType" runat="server" Text="024_Warranty Type"></asp:Label>
                                            </td>
                                            <td align="left">:
                                                <asp:DropDownList ID="UI_cboWarrantyType" runat="server" Width="120px" AutoPostBack="true">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="UI_cboWarrantyTypeoth" runat="server" Width="80px">
                                                    <asp:ListItem Value="1" Text="Exclusive"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="Inclusive"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Button ID="UI_cmdPartPick" runat="server" Text="_Pick" CssClass="Pick"></asp:Button>
                                            </td>
                                            <td width="10%" align="right">
                                                <asp:Label ID="UI_lblExtraMonths" runat="server" Text="031_Extra Months"></asp:Label>
                                            </td>
                                            <td width="20%" align="left">:
                                                <asp:TextBox ID="UI_ExtraMonths" runat="server" Width="150"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="UI_lblProductGroup" runat="server" Text="Model"></asp:Label>
                                            </td>
                                            <td width="1" align="left">:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="UI_txtProductGroup" ReadOnly="true" CssClass="readyonly" runat="server" Width="90px"></asp:TextBox>
                                                <asp:Button ID="UI_cmdProductGroupPick" runat="server" Text="_Pick" CssClass="Pick"></asp:Button>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="UI_lblWarrantyName" runat="server" Text="029_Warranty Name"></asp:Label>
                                            </td>
                                            <td align="left">:
                                                <asp:TextBox ID="UI_txtWarrantyName" runat="server" Width="250"></asp:TextBox>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="UI_lblYears" runat="server" Text="032_Years"></asp:Label>
                                            </td>
                                            <td align="left">:
                                                <asp:TextBox ID="UI_Years" runat="server" Width="150"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="UI_lblVersion" runat="server" Text="028_Version"></asp:Label>
                                            </td>
                                            <td width="1" align="left">:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="UI_txtVersion" ReadOnly="true" CssClass="readyonly" runat="server" Width="150px" MaxLength="8"></asp:TextBox>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="UI_lblDescription" runat="server" Text="036_Description"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="UI_txtDescription" runat="server" Width="250"></asp:TextBox>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="UI_lblUnitPrice" runat="server" Text="037_Unit Price"></asp:Label>
                                            </td>
                                            <td align="left">:
                                                <asp:TextBox ID="UI_UnitPrice" runat="server" Width="150" Style="text-align: right;"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="lblExpDate" runat="server" Text="Expired Date"></asp:Label>
                                            </td>
                                            <td width="1" align="left">:
                                            </td>
                                            <td align="left" colspan="5">
                                                <asp:TextBox ID="txtExpDate" runat="server" Width="100"></asp:TextBox>
                                                <img style="vertical-align: middle" alt="" src="Images/icon_cal.gif" onclick="window.showModalDialog('Calendar.aspx',document.elementFromPoint(event.x-60,event.y),'dialogHeight:195px;dialogWidth:224px;dialogTop:'+(parseInt(window.event.screenY)+10)+';dialogLeft:'+window.event.screenX+';help:0;resizable:0;status:0;')" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="7" align="center">
                                                <asp:Label ID="lblSwID" runat="server" Visible="false"></asp:Label>
                                                <asp:Button ID="UI_cmdSave" ValidationGroup="AddGroup" runat="server" Text="_Save" CssClass="Search" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <!--[End]新增資料-->
                        </table>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlFile" runat="server" Width="100%">
                <table border="0" width="100%" id="Step2" cellspacing="0" cellpadding="0" style="vertical-align: top;">
                    <tr align="left">
                        <td height="27" background="images/pic_15.gif" class="default">
                            <b>
                                <asp:Label ID="lblPartsInform" runat="server" Text=" Parts Information"></asp:Label></b>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="center">
                            <table border="0" width="100%" id="table5" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td class="text_tittle">&nbsp;
                                    </td>
                                </tr>
                            </table>
                            <table width="100%" align="center" border="0" cellspacing="1" class="default">
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblAttachments" runat="server" Text="Attachments"></asp:Label>
                                    </td>
                                    <td align="left">:
                                        <input name="FileUp" type="file" id="FileUp" runat="server" class="input_upload" size="30" />
                                        <asp:Button ID="btnFileAdd" runat="server" Text="_Add" CssClass="Search" />
                                    </td>
                                    <td align="left" colspan="3">
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
                                    <tr>
                                        <td width="5%" align="right">&nbsp;
                                        </td>
                                        <td width="40%">
                                            <asp:Label ID="lblPartNoQuery" runat="server" Text="Part No"></asp:Label>:
                                        <asp:TextBox ID="txtQueryPartNo" runat="server" Width="100"></asp:TextBox>
                                            <asp:Label ID="lblPartDescQuery" runat="server" Text="Desciption"></asp:Label>:
                                        <asp:TextBox ID="txtQueryPartDesc" runat="server" Width="120"></asp:TextBox>
                                            <asp:Button ID="UI_cmdSearchPart" runat="server" Text="_Search" CssClass="Search" />
                                        </td>
                                        <td width="10%" align="right">&nbsp;
                                        </td>
                                        <td width="40%" align="left">&nbsp;
                                        </td>
                                        <td width="5%" align="right">&nbsp;
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td>&nbsp;
                                        </td>
                                        <td valign="top" width="30%">
                                            <table width="100%" border="1" cellspacing="0" cellpadding="0" class="box_table" style="vertical-align: top;">
                                                <tr class="Text_Head">
                                                    <td>
                                                        <asp:CheckBox ID="chkPartAll" runat="server" OnCheckedChanged="chkPartAll_CheckedChanged" AutoPostBack="true" />All
                                                    </td>
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
                                                            <td align="center">
                                                                <asp:CheckBox ID="chkExcelPart" runat="server" />
                                                                <asp:Label ID="lblPartNo" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "swp_partno")%>'></asp:Label>
                                                                <asp:Label ID="lblPartDesc" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "swp_desc")%>'></asp:Label>
                                                            </td>
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
                                            <asp:Button ID="UI_cmdToPart" runat="server" Text="_To" CssClass="Search" />
                                        </td>
                                        <td align="left" valign="top" width="65%">
                                            <table width="100%" border="1" cellspacing="0" cellpadding="0" class=" box_table">
                                                <tr class="Text_Head">
                                                    <td width="100px">
                                                        <asp:CheckBox ID="chkOKAll" runat="server" OnCheckedChanged="chkOKAll_CheckedChanged" AutoPostBack="true" />All
                                                    </td>
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
                                                            <td style="text-align: center">
                                                                <asp:CheckBox ID="chkOKPart" runat="server" />
                                                                <asp:Label ID="lblPartNo" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "swp_partno")%>'></asp:Label>
                                                            </td>
                                                            <td>
                                                                <%# DataBinder.Eval(Container.DataItem, "swp_partno")%>
                                                            </td>
                                                            <td>
                                                                <%# DataBinder.Eval(Container.DataItem, "swp_desc")%>
                                                            </td>
                                                            <td align="center">
                                                                <asp:TextBox ID="txtYears" runat="server" Width="50px" onblur="SpecialSettingYearsEnter(this);" Text='<%# DataBinder.Eval(Container.DataItem, "swp_year")%>'></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:DataList>
                                            </table>
                                        </td>
                                        <td align="right">&nbsp;
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <asp:Panel ID="pnDesc" runat="server" Width="100%">
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblDescription" runat="server" Text="Description"></asp:Label>
                                        </td>
                                        <td align="left">:
                                                <asp:TextBox ID="txtDescription" runat="server" Width="650" Height="250" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <tr>
                                    <td colspan="5" align="center">
                                        <p>
                                            <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="Problem_Edit" />
                                            <asp:Button ID="btnValid" runat="server" OnClientClick="return confirm('Are your sure?')" Text="Cancel" CssClass="Confirm" />
                                            <asp:Button ID="UI_cmdDelete" runat="server" Text="_Delete" CssClass="Confirm" />
                                            <asp:Button ID="UI_cmdSavePart" ValidationGroup="AddGroup" runat="server" Text="_Save" CssClass="Confirm" />
                                            <asp:Button ID="UI_cmdSubmit" ValidationGroup="AddGroup" runat="server" Text="_Submit" CssClass="Confirm" />
                                            <asp:Button ID="btnInvalid" runat="server" OnClientClick="return confirm('Are your sure?')" Text="Invalid" CssClass="Confirm" />
                                        </p>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <uc2:ucMessage ID="ucMessage" runat="server" />
            <uc6:ucProductGroup ID="ucProductGroup" runat="server"></uc6:ucProductGroup>
            <uc7:ucParts ID="ucParts" runat="server"></uc7:ucParts>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="UI_cmdProductGroupPick" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="UI_cmdPartPick" EventName="Click" />
            <asp:PostBackTrigger ControlID="UI_cmdSave" />
            <asp:PostBackTrigger ControlID="ucMessage" />
            <asp:PostBackTrigger ControlID="btnFileAdd" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:RequiredFieldValidator ID="rfvMonth" runat="server" ErrorMessage="001_Extra Months not allow null" ControlToValidate="UI_ExtraMonths" Display="None" TabIndex="0" ValidationGroup="AddGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="rfvProductGroup" runat="server" ErrorMessage="002_Product Group allow null" ControlToValidate="UI_txtProductGroup" Display="None" TabIndex="0" ValidationGroup="AddGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="rfvWarrName" runat="server" ErrorMessage="003_Warranty Name not allow null" ControlToValidate="UI_txtWarrantyName" Display="None" TabIndex="0" ValidationGroup="AddGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="rfvYears" runat="server" ErrorMessage="004_Years not allow null" ControlToValidate="UI_Years" Display="None" TabIndex="0" ValidationGroup="AddGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="rfvPrice" runat="server" ErrorMessage="005_Unit Price not allow null" ControlToValidate="UI_UnitPrice" Display="None" TabIndex="0" ValidationGroup="AddGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
    <asp:ValidationSummary ID="vsadd" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="AddGroup" />
</asp:Content>
