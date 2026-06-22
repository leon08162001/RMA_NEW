<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ucFAQClass.ascx.vb" Inherits="ascx_ucFAQClass" %>


<script type="text/javascript">
function Validate_FAQSubClass( oSrc, args ) {
    var sValue = document.getElementById('<%=UI_drpCategoryName1.ClientID %>').value; 
    
    if (sValue==-1){
        args.IsValid = false;
    }else{
        args.IsValid = true;
    }
}
</script>


<asp:Panel ID="UI_panel" runat="server"  style="display:none;position:absolute;width:70%;border:3px solid #808080;background:#ffffff;" >

<table border="0" cellspacing="0" cellpadding="0"  class="TableListdownright">
		<tr>
			<td  >
		        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="default">
		            <tr>
		                <td align="center">
		                    <asp:Label ID="UI_lblFAQTittle" runat="server" Text="018_FAQ Class" Font-Size ="Larger" Font-Bold="true"></asp:Label>
		                </td>
		            </tr>
		        </table>
			</td>
		</tr>
		
		<tr>
			<td valign="top"  align="center">
                <table  border="0" cellspacing="1" cellpadding="0" class="table" >
                    <tr>
                        <td width="10%" align="right">
                            <asp:Label ID="UI_lblFAQC_ClassName" runat="server" Text="019_Category1"></asp:Label>:
                        </td>
                        <td width="30%" align ="left">
                            <asp:TextBox ID="UI_txtFAQC_ClassName" runat="server" Width="150px"></asp:TextBox>
                        </td>
                        <td width="10%" align="right">
                            <asp:Label ID="UI_lblFAQC_Visible" runat="server" Text="020_Status"></asp:Label>:
                        </td>
                        <td width="20%" align ="left">
                            <asp:RadioButtonList ID="UI_opgFAQC_Visible" runat ="server" RepeatLayout ="Flow" RepeatDirection="Horizontal" >
                                <asp:ListItem Value="1" Text ="013_Open" Selected="True" ></asp:ListItem>
                                <asp:ListItem Value="0" Text ="014_Close"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td width="15%" align="center">
                            <asp:Button ID="UI_cmdAdd1" runat ="server" Text ="_Add" CssClass ="Add" Width="50" ValidationGroup="vsFAQClass_Add" />
                        </td>
                    </tr>
                    
                    <tr>
                        <td colspan="5">
                            <asp:GridView ID="UI_dvFAQClass" runat ="server" Width = "100%" CellPadding ="0" CellSpacing ="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric">
                                <Columns>
                                  <asp:TemplateField>
                                     <HeaderStyle Width="5%" Height ="20px" HorizontalAlign="Center"></HeaderStyle>
                                     <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                     <ItemTemplate>
                                            <%--<asp:Label ID ="lblSeqID" runat = "server" text='<%#Me.UI_dvFAQClass.Rows.Count + %>'></asp:Label>--%>
                                            <asp:Label ID ="lblFAQC_ID" runat = "server" text='<%# Eval("FAQC_ID") %>' Visible ="false" ></asp:Label>
                                     </ItemTemplate>
                                  </asp:TemplateField>

                                  <asp:TemplateField HeaderText="021_Category Name1">
                                     <HeaderStyle Width="43%" HorizontalAlign="Center"></HeaderStyle>
                                     <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                     <ItemTemplate>
                                            <asp:Label ID ="lblFAQC_CLASSNAME" runat = "server" text='<%# Eval("FAQC_CLASSNAME") %>'></asp:Label>
                                            <asp:TextBox ID="txtFAQC_CLASSNAME" runat="server" Width="150px" text='<%# Eval("FAQC_CLASSNAME") %>' Visible="false"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfv_dvFAQClass_txtFAQC_CLASSNAME" runat="server" ControlToValidate="txtFAQC_CLASSNAME" ErrorMessage = "029_Category1" Display ="None" SetFocusOnError ="true"></asp:RequiredFieldValidator>
                                            <asp:ValidationSummary ID="vsFAQClass_Edit" runat ="server" ShowMessageBox ="true" ShowSummary ="false"/>
                                     </ItemTemplate>
                                  </asp:TemplateField>

                                  <asp:TemplateField HeaderText="020_Status">
                                     <HeaderStyle Width="10%" HorizontalAlign="Center"></HeaderStyle>
                                     <ItemStyle HorizontalAlign="center"></ItemStyle>
                                     <ItemTemplate>
                                            <asp:Label ID ="lblFAQC_VISIBLE" runat = "server" text='<%# Eval("Visible") %>'></asp:Label>
                                            <asp:DropDownList runat="server" ID="drpFAQC_VISIBLE" Visible="false">
                                                <asp:ListItem Text="013_Open" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="014_Close" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID ="hidFAQC_VISIBLE" runat = "server" text='<%# Eval("FAQC_VISIBLE") %>' Visible="false"></asp:Label>
                                     </ItemTemplate>
                                  </asp:TemplateField>

                                  <asp:BoundField DataField="FAQC_LUADNAME" HeaderText="016_Last Editor" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="center"></asp:BoundField>
                                  <asp:BoundField DataField="FAQC_LUSTMP" HeaderText="031_Last Time" HeaderStyle-Width="15%" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd}" ></asp:BoundField>

                                  <asp:TemplateField HeaderText="009_Edit">
                                     <HeaderStyle Width="12%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                     <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                     <ItemTemplate>
                                            <asp:Button ID="imgEdit" runat ="server" Text ="_Edit" CssClass ="Problem_Edit" CommandName="cmdEdit" CommandArgument='<%#Me.UI_dvFAQClass.Rows.Count%>'/>
                                            <asp:Button ID="imgSave" runat ="server" Text ="_Save" CssClass ="Problem_Edit" CommandName="cmdSave" CommandArgument='<%#Me.UI_dvFAQClass.Rows.Count%>' Visible="false"/>
                                     </ItemTemplate>
                                  </asp:TemplateField>
                                </Columns> 
                                
                                <HeaderStyle CssClass="Text_Head"/>
                                <RowStyle CssClass="TR_1" />
                                <AlternatingRowStyle CssClass="ListRowEven" />
                                <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Center" />
                            </asp:GridView>
                        </td>
                    </tr>

                </table>			
			</td>
        </tr>			

		<tr>
			<td  >
		        <table border="0"  cellspacing="0" cellpadding="0" class="table">
		            <tr>
		                <td align="center">
		                    <asp:Label ID="Label3" runat="server" Text="022_FAQ Sub Class" Font-Size="Larger" Font-Bold="true"></asp:Label>
		                </td>
		            </tr>
		        </table>
			</td>
		</tr>
		
		<tr>
			<td valign="top"  align="center" height="20px">
                <table  border="0" cellspacing="1" cellpadding="0" class="table" >
                    <tr>
                        <td width="10%" align="right">
                            <asp:Label ID="UI_lblCategoryName1" runat="server" Text="019_Category1"></asp:Label>:
                        </td>
                        <td width="20%" align ="left">
                            <asp:DropDownList runat="server" ID="UI_drpCategoryName1"></asp:DropDownList>
                        </td>

                        <td width="10%" align="right">
                            <asp:Label ID="UI_lblFAQSC_ClassName" runat="server" Text="023_Category2"></asp:Label>:
                        </td>
                        <td width="30%" align ="left">
                            <asp:TextBox ID="UI_txtFAQSC_ClassName" runat="server" Width="150px"></asp:TextBox>
                        </td>
                        <td width="10%" align="right">
                            <asp:Label ID="UI_lblFAQSC_Visible" runat="server" Text="020_Status"></asp:Label>:
                        </td>
                        <td width="10%" align ="left">
                            <asp:RadioButtonList ID="UI_opgFAQSC_Visible" runat ="server" RepeatLayout ="Flow" RepeatDirection="Horizontal" >
                                <asp:ListItem Value="1" Text ="013_Open" Selected="True" ></asp:ListItem>
                                <asp:ListItem Value="0" Text ="014_Close"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td width="10%" align="center">
                            <asp:Button ID="UI_cmdAdd2" runat ="server" Text ="_Add" CssClass ="Add" Width="50" ValidationGroup="vsFAQSubClass_Add" />
                        </td>
                    </tr>
                    
                    <tr>
                        <td colspan="7">
                            <asp:GridView ID="UI_dvFAQSubClass" runat ="server" Width = "100%" CellPadding ="0" CellSpacing ="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric" >
                                <Columns>
                                  <asp:TemplateField>
                                     <HeaderStyle Width="5%" Height ="20px" HorizontalAlign="Center"></HeaderStyle>
                                     <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                     <ItemTemplate>
                                            <%--<asp:Label ID ="lblSeqID" runat = "server" text='<%#Me.UI_dvFAQSubClass.Rows.Count + %>'></asp:Label>--%>
                                            <asp:Label ID ="lblFAQSC_ID" runat = "server" text='<%# Eval("FAQSC_ID") %>' Visible ="false" ></asp:Label>
                                     </ItemTemplate>
                                  </asp:TemplateField>
                                  
                                  <asp:TemplateField HeaderText="021_Category Name1">
                                     <HeaderStyle Width="25%" HorizontalAlign="Center"></HeaderStyle>
                                     <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                     <ItemTemplate>
                                            <asp:Label ID ="lblFAQC_CLASSNAME" runat = "server" text='<%# Eval("FAQC_CLASSNAME") %>'></asp:Label>
                                            <asp:DropDownList runat="server" ID="drpCategoryName1" Visible="false"></asp:DropDownList>
                                            <asp:Label ID ="hidFAQSC_FAQCID" runat = "server" text='<%# Eval("FAQSC_FAQCID") %>' Visible="false"></asp:Label>
                                     </ItemTemplate>
                                  </asp:TemplateField>

                                  <asp:TemplateField HeaderText="024_Category Name2">
                                     <HeaderStyle Width="25%" HorizontalAlign="Center"></HeaderStyle>
                                     <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                     <ItemTemplate>
                                            <asp:Label ID ="lblFAQSC_CLASSNAME" runat = "server" text='<%# Eval("FAQSC_CLASSNAME") %>'></asp:Label>
                                            <asp:TextBox ID="txtFAQSC_CLASSNAME" runat="server" Width="150px" text='<%# Eval("FAQSC_CLASSNAME") %>' Visible="false"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfv_dvFAQSubClass_txtFAQSC_CLASSNAME" runat="server" ControlToValidate="txtFAQSC_CLASSNAME" ErrorMessage = "030_Category2" Display ="None" SetFocusOnError ="true"></asp:RequiredFieldValidator>
                                            <asp:ValidationSummary ID="vsFAQSubClass_Edit" runat ="server" ShowMessageBox ="true" ShowSummary ="false"/>
                                     </ItemTemplate>
                                  </asp:TemplateField>
                                  
                                  <asp:TemplateField HeaderText="020_Status">
                                     <HeaderStyle Width="10%" HorizontalAlign="Center"></HeaderStyle>
                                     <ItemStyle HorizontalAlign="center"></ItemStyle>
                                     <ItemTemplate>
                                            <asp:Label ID ="lblFAQSC_VISIBLE" runat = "server" text='<%# Eval("Visible") %>'></asp:Label>
                                            <asp:DropDownList runat="server" ID="drpFAQSC_VISIBLE" Visible="false">
                                                <asp:ListItem Text="013_Open" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="014_Close" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID ="hidFAQSC_VISIBLE" runat = "server" text='<%# Eval("FAQSC_VISIBLE") %>' Visible="false"></asp:Label>
                                     </ItemTemplate>
                                  </asp:TemplateField>
                                  
                                  <asp:BoundField DataField="FAQSC_LUADNAME" HeaderText="016_Last Editor" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="center"></asp:BoundField>
                                  <asp:BoundField DataField="FAQSC_LUSTMP" HeaderText="031_Last Time" HeaderStyle-Width="15%" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd}" ></asp:BoundField>

                                  <asp:TemplateField HeaderText="009_Edit">
                                     <HeaderStyle Width="12%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                     <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                     <ItemTemplate>
                                            <asp:Button ID="imgEdit" runat ="server" Text ="_Edit" CssClass ="Problem_Edit" CommandName="cmdEdit" CommandArgument='<%#Me.UI_dvFAQSubClass.Rows.Count%>'/>
                                            <asp:Button ID="imgSave" runat ="server" Text ="_Save" CssClass ="Problem_Edit" CommandName="cmdSave" CommandArgument='<%#Me.UI_dvFAQSubClass.Rows.Count%>' Visible="false"/>
                                     </ItemTemplate>
                                  </asp:TemplateField>
                                </Columns> 
                                
                                <HeaderStyle CssClass="Text_Head"/>
                                <RowStyle CssClass="TR_1" />
                                <AlternatingRowStyle CssClass="ListRowEven" />
                                <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Center" />
                            </asp:GridView>
                        </td>
                    </tr>

                </table>
			
			</td>
		</tr>

        <tr>
			<td align="center" align="center" height="35px">
                <asp:Button ID="UI_butClose" runat ="server" Text ="_Close" CssClass ="Close_l" Width="80" />
            </td>
        </tr>
    </table>


<asp:RequiredFieldValidator ID="rfv_txtFAQC_ClassName" runat="server" ControlToValidate="UI_txtFAQC_ClassName" ErrorMessage = "029_Category1" Display ="None" ValidationGroup ="vsFAQClass_Add" SetFocusOnError ="true"></asp:RequiredFieldValidator>
<asp:ValidationSummary ID="vsFAQClass_Add" runat ="server" ShowMessageBox ="true" ShowSummary ="false" ValidationGroup="vsFAQClass_Add" />

<asp:CustomValidator ID="cv_drpCategoryName1" runat="server" ClientValidationFunction="Validate_FAQSubClass" ErrorMessage = "025_Select Category1" Display ="None" ValidationGroup ="vsFAQSubClass_Add"></asp:CustomValidator>  
<asp:RequiredFieldValidator ID="rfv_txtFAQSC_ClassName" runat="server" ControlToValidate="UI_txtFAQSC_ClassName" ErrorMessage = "030_Category2" Display ="None" ValidationGroup ="vsFAQSubClass_Add" SetFocusOnError ="true"></asp:RequiredFieldValidator>
<asp:ValidationSummary ID="vsFAQSubClass_Add" runat ="server" ShowMessageBox ="true" ShowSummary ="false" ValidationGroup="vsFAQSubClass_Add" />

<asp:button id="UI_butTarget" Width="0px" Height="0px" runat="server" style="display:none"></asp:button>
<asp:button id="UI_butOK" Width="0px" Height="0px" runat="server" style="display:none"></asp:button></asp:Panel>


<ajaxToolkit:ModalPopupExtender ID="ajModalProgress" TargetControlID="UI_butTarget"  
PopupControlID="UI_panel"  
OkControlID="UI_butOK" 
CancelControlID="UI_butClose"
BackgroundCssClass = "modalBackground"
runat="server">
</ajaxToolkit:ModalPopupExtender>

