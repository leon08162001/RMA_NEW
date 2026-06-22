<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ucCustomerUser.ascx.vb" Inherits="ascx_ucCustomerUser" %>
<%@ Register Src="ucProgressStatus.ascx" TagName="ucProgressStatus" TagPrefix="uc1" %>
<%@ Register Src="ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>

<uc1:ucProgressStatus ID="ucProgressStatus" runat="server" />

<asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
    <ContentTemplate> 

<table border="0" width="100%" id="table4" cellspacing="0" cellpadding="0" height="100%">
	<tr>
	    <td width="24" background="Images/pic_12.gif">&nbsp;</td>
	    <td valign="top" align="left">
	        <table border="0" width="100%" cellspacing="0" cellpadding="0" height="100%">
	            <!--[Begin]Tittle-->
	            <tr>
	                <td width="3%">&nbsp;</td>
	                <td width="94%" align="left">
	                    <asp:Label ID="UI_lblTittle" runat="server" Text="001_Setting - Customer" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
			        </td>
	                <td width="3%">&nbsp;</td>
	            </tr>
	            <!--[End]Tittle-->
	            <!--[Begin]新增資料-->
	            <tr>
	                <td>&nbsp;</td>
	                <td align="left">
	                    <table width="100%" align="center" border="0" cellspacing="1" class="default" >
			                <tr>
			                    <td colspan="5">&nbsp;</td>
			                </tr>
			                <tr>
			                    <td width="15%">&nbsp;
			                        <asp:Label ID="UI_lblCustomerName" runat="server" Text="004_Customer Name."></asp:Label>
			                    </td>
			                    <td width="35%">:
			                        <asp:Label ID="UI_lblCustomerNameText" runat="server"></asp:Label>
			                    </td>
			                    <td width="12%" align="right">
			                        <asp:Label ID="UI_lblCustomerID" runat="server" Text="003_Customer ID"></asp:Label>
			                    </td>
			                    <td width="20%" align="left">:
			                        <asp:Label ID="UI_lblCustomerIDText" runat="server"></asp:Label>
			                    </td>
			                    <td width="20%" align="left"></td>
			                </tr>
				            <tr>
			                    <td>&nbsp;
			                        <asp:Label ID="UI_lblCountry" runat="server" Text="013_Country"></asp:Label>
			                    </td>
			                    <td colspan="4">:
			                        <asp:Label ID="UI_lblCountryText" runat="server"></asp:Label>
                                </td>
			                </tr>
				            <tr>
			                    <td>&nbsp;
			                        <asp:Label ID="UI_lblRepairCenter" runat="server" Text="009_Repair Center"></asp:Label>
			                    </td>
			                    <td colspan="4">:
			                        <asp:Label ID="UI_lblRepairCenterText" runat="server"></asp:Label>
			                    </td>
			                </tr>
			                <tr>
			                    <td>&nbsp;
			                        <asp:Label ID="UI_lblContactPerson" runat="server" Text="016_Contact Person"></asp:Label>
			                    </td>
			                    <td colspan="4">:
			                        <asp:Label ID="UI_lblContactPersonText" runat="server"></asp:Label>
					            </td>
			                </tr>
			                <tr>
			                    <td>&nbsp;
			                        <asp:Label ID="UI_lblTEL" runat="server" Text="011_TEL"></asp:Label>
			                    </td>
			                    <td colspan="4">:
			                        <asp:Label ID="UI_lblTELText" runat="server"></asp:Label>
					            </td>
			                </tr>
				            <tr>
			                    <td>&nbsp;
			                        <asp:Label ID="UI_lblAddress1" runat="server" Text="017_Address (1)"></asp:Label>
			                    </td>
			                    <td colspan="4"> : 
					                <asp:Label ID="UI_lblAddress1Text" runat="server"></asp:Label>
					            </td>
			                </tr>
				            <tr>
			                    <td>&nbsp;
			                        <asp:Label ID="UI_lblAddress2" runat="server" Text="018_Address (2)"></asp:Label>
			                    </td>
			                    <td colspan="4"> : 
					                <asp:Label ID="UI_lblAddress2Text" runat="server"></asp:Label>
					            </td>
			                </tr>
				            <tr>
			                    <td>&nbsp;
			                        <asp:Label ID="UI_lblAddress3" runat="server" Text="019_Address (3)"></asp:Label>
			                    </td>
			                    <td colspan="4"> : 
					                <asp:Label ID="UI_lblAddress3Text" runat="server"></asp:Label>
					            </td>
			                </tr>
				            <tr>
			                    <td>&nbsp;
			                        <asp:Label ID="UI_lblAddress4" runat="server" Text="020_Address (4)"></asp:Label>
			                    </td>
			                    <td colspan="4"> : 
					                <asp:Label ID="UI_lblAddress4Text" runat="server"></asp:Label>
					            </td>
			                </tr>
			                <tr>
			                    <td>&nbsp;
			                        <asp:Label ID="UI_lblEMail" runat="server" Text="021_eMail"></asp:Label>
			                    </td>
			                    <td colspan="4"> : 
					                <asp:Label ID="UI_lblEMailText" runat="server"></asp:Label>
					            </td>
			                </tr>
				            <tr>
			                    <td>&nbsp;
			                        <asp:Label ID="UI_lblStatus" runat="server" Text="022_Account status"></asp:Label>
			                    </td>
			                    <td colspan="4">:
			                        <asp:Label ID="UI_lblStatusText" runat="server"></asp:Label>
			                    </td>
			                </tr>
			            </table>
	                </td>
	                <td>&nbsp;</td>
	            </tr>
	            <!--[End]新增資料-->
	        </table>
		</td>
	</tr>
	<tr>
	    <td width="24" height="27" background="Images/pic_14.gif">&nbsp;</td>
		<td height="27" background="Images/pic_15.gif" >
	        <table border="0" width="100%" cellspacing="0" cellpadding="0">
	            <tr>
	                <td width="1%">&nbsp;</td>
	                <td width="96%" class="default">
	                    <table border="0" width="100%" cellspacing="0" cellpadding="0">
	                        <tr>
	                            <td width="60%" align="left">
	                                <asp:Label ID="UI_lblInformationTittle" runat="server" Text="023_User Information" Font-Bold ="true"></asp:Label>
	                            </td>
	                            <td align="right">&nbsp;</td>
	                        </tr>
	                    </table>
			        </td>
			        <td width="3%">&nbsp;</td>
	            </tr>
	        </table>
		</td>
	</tr>
	<tr>
	    <td background="Images/pic_20.gif">&nbsp;</td>
		<td valign="top" bgcolor="#E3D8BE" align="center">
		    <!--[Begin]新增資料列表-->
            <div align="center">
            <asp:GridView ID="UI_CustomerUser" runat ="server" Width = "100%" CellPadding ="0" CellSpacing ="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric"  >
                <Columns>
                  <asp:TemplateField>
                     <HeaderStyle Width="3%" Height ="20px" HorizontalAlign="Center"></HeaderStyle>
                     <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                     <ItemTemplate>
                            <asp:Label ID ="UI_SeqID" runat = "server" text='<%# me.UI_CustomerUser.Rows.Count +1 %>'></asp:Label>
                            <asp:Label ID ="UI_Cuusad" runat = "server" text='<%# Eval("CUUS_AD") %>' Visible ="false" ></asp:Label>
                            <asp:Label ID ="UI_oldAccountID" runat = "server" text='<%# Eval("CUUS_oldAccountID") %>' Visible ="false" ></asp:Label>
                            <asp:Label ID ="UI_ISMANAGER" runat = "server" text='<%# Eval("CUUS_ISMANAGER") %>' Visible ="false"></asp:Label>
                            <asp:Label ID ="UI_ISStatus" runat = "server" text='<%# Eval("CUUS_STATUS") %>' Visible ="false"></asp:Label>
                     </ItemTemplate>
                  </asp:TemplateField>
                  
                  <asp:TemplateField HeaderText="024_User ID">
                     <HeaderStyle Width="15%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                     <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                     <ItemTemplate>
                            <asp:TextBox ID="UI_AccountID" runat ="server" Width ="100px" Text='<%# Eval("CUUS_ACCOUNTID") %>'></asp:TextBox>
                            <asp:Label ID="UI_lblAccountID" runat ="server" Text='<%# Eval("CUUS_ACCOUNTID") %>' Visible="false"></asp:Label>
                            <asp:RequiredFieldValidator ID="rfv_AccountID" runat="server" ErrorMessage="036_請輸入帳號" Display="None" ValidationGroup="CustomerGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
                     </ItemTemplate>
                  </asp:TemplateField>
                  
                  <asp:TemplateField HeaderText="025_Password">
                     <HeaderStyle Width="15%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                     <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                     <ItemTemplate>
                            <asp:TextBox ID="UI_Pwd" runat ="server" Width ="80px" Text='<%# Eval("CUUS_PWD") %>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfv_Password" runat="server" ErrorMessage="037_請輸入密碼" Display="None" ValidationGroup="CustomerGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
                     </ItemTemplate>
                  </asp:TemplateField>
                  
                  <asp:TemplateField HeaderText="011_TEL">
                     <HeaderStyle Width="15%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                     <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                     <ItemTemplate>
                            <asp:TextBox ID="UI_Tel" runat ="server" Width ="120px" Text='<%# Eval("CUUS_TEL") %>'></asp:TextBox>
                     </ItemTemplate>
                  </asp:TemplateField>
                  
                  <asp:TemplateField HeaderText="026_Address">
                     <HeaderStyle Width="30%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                     <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                     <ItemTemplate>
                            <asp:TextBox ID="UI_EMail" runat ="server" Width ="300px" Text='<%# Eval("CUUS_EMAIL") %>'></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revUIEMail_1" runat="server" ErrorMessage="035_EMail輸入格式有誤"  Display="None" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="UserGroup" SetFocusOnError="true"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="revUIEMail_2" runat="server" ErrorMessage="035_EMail輸入格式有誤" Display="None" ValidationGroup="UserGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
                     </ItemTemplate>
                  </asp:TemplateField>           
               
                  <asp:TemplateField HeaderText="005_Status">
                     <HeaderStyle Width="5%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                     <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                     <ItemTemplate>
                            <asp:Label ID ="UI_lblStatus" runat = "server" Visible ="false"></asp:Label>
                            <asp:DropDownList ID="UI_Status" runat="server" Visible="false">
		                        <asp:ListItem Value ="1" Text ="007_Open" Selected ="True"></asp:ListItem>
		                        <asp:ListItem Value ="0" Text ="008_Close"></asp:ListItem>
		                    </asp:DropDownList>
                     </ItemTemplate>
                  </asp:TemplateField>
                  
                  <asp:TemplateField HeaderText="012_Manager">
                     <HeaderStyle Width="8%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                     <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                     <ItemTemplate>
                            <asp:Label ID ="UI_Manager" runat = "server" text=""></asp:Label>
                     </ItemTemplate>
                  </asp:TemplateField>
                </Columns> 
                <HeaderStyle CssClass="Text_Head"/>
                <RowStyle CssClass="TR_1" />
                <AlternatingRowStyle CssClass="ListRowEven" />
                <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Center" />
            </asp:GridView>
            </div>
            <!--[End]新增資料列表-->
		</td>
    </tr>
	<tr>
	    <td background="Images/pic_20.gif" height ="130">&nbsp;</td>
		<td valign="top" bgcolor="#E3D8BE" align="center">
		    <!--[Begin]Submit-->
		    <table border="0" cellspacing="0" cellpadding="0" width="100%" height="100%">
	            <tr>
	                <td width="3%">&nbsp;</td>
	                <td width="94%" align="center" class="default">
	                    <br>
	                    <asp:Button ID="UI_cmdCancel" runat="server" Text="_Cancel" CssClass="Problem_Edit" ValidationGroup="UserGroup" OnClick="UI_cmdCancel_Click" OnClientClick="onProgress('Process')" CausesValidation="false" />&nbsp;
	                    <asp:Button ID="UI_cmdSubmit" runat="server" Text="_Submit" CssClass="Confirm_l" ValidationGroup="UserGroup" OnClick="UI_cmdSubmit_Click" OnClientClick="onProgress('Save')" />
					</td>
	                <td width="3%">&nbsp;</td>
	            </tr>
	        </table>
	        <!--[End]Submit-->
        </td>
    </tr>
</table>

        <uc2:ucMessage ID="ucMessage" runat="server" />

    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="UI_cmdCancel" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="UI_cmdSubmit" EventName="Click" />
        <asp:PostBackTrigger ControlID="ucMessage" />
    </Triggers>
</asp:UpdatePanel>

<asp:Label ID="UI_lblPreviousPage_CuNo" runat="server" Visible="false"></asp:Label>
<asp:Label ID="UI_lblPreviousPage_CuusID" runat="server" Visible="false"></asp:Label>

<asp:ValidationSummary ID="vsCustomer" runat ="server" ShowMessageBox ="true" ShowSummary ="false" ValidationGroup="CustomerGroup"/>
 