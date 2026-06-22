<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ucCustomerAdmin.ascx.vb" Inherits="ascx_ucCustomerAdmin" %>
<%@ Register Src="ucProgressStatus.ascx" TagName="ucProgressStatus" TagPrefix="uc1" %>
<%@ Register Src="ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>

<script type="text/javascript" language="javascript">
    function Validate_Country(oSrc, args) {
        var sValue1 = document.getElementById('<%=UI_cboCountry.ClientID %>').value;

        if (sValue1 == -1) {
            args.IsValid = false;
        } else {
            args.IsValid = true;
        }
    }

    function Validate_RepairCenter(oSrc, args) {
        var sValue1 = document.getElementById('<%=UI_cboRepairCenter.ClientID %>').value;

        if (sValue1 == -1) {
            args.IsValid = false;
        } else {
            args.IsValid = true;
        }
    }
</script>


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
                                <table width="100%" align="center" border="0" cellspacing="1" class="default">
                                    <tr>
                                        <td colspan="5">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td width="15%">&nbsp;
			                        <asp:Label ID="UI_lblCustomerName" runat="server" Text="004_Customer Name."></asp:Label>
                                        </td>
                                        <td width="35%">:
			                        <asp:Label ID="UI_lblCustomerNameText" runat="server" Visible="false"></asp:Label>
                                            <asp:TextBox ID="UI_txtCusName" runat="server" Width="400" Visible="false"></asp:TextBox>
                                        </td>

                                        <td width="15%" align="right">
                                            <asp:Label ID="UI_lblCustomerID" runat="server" Text="003_Customer ID"></asp:Label>
                                        </td>
                                        <td width="20%" align="left">:
			                        <asp:Label ID="UI_lblCustomerIDText" runat="server" Visible="false"></asp:Label>
                                            <asp:TextBox ID="UI_txtCustomerID" runat="server" Width="100" MaxLength="6" Visible="false"></asp:TextBox>
                                        </td>
                                        <td width="20%" align="left"></td>
                                    </tr>


                                    <tr>
                                        <td>&nbsp;
			                        <asp:Label ID="UI_lblCountry" runat="server" Text="013_Country"></asp:Label>
                                        </td>
                                        <td>:
			                        <asp:DropDownList ID="UI_cboCountry" runat="server"></asp:DropDownList>
                                        </td>
                                        <td align="right" runat="server" id="UI_trSalesID1" visible="false">
                                            <asp:Label ID="UI_lblSalesID" runat="server" Text="014_Sales ID"></asp:Label>
                                        </td>
                                        <td align="left" runat="server" id="UI_trSalesID2" visible="false">:
			                        <asp:TextBox ID="UI_txtSalesID" runat="server" Width="100"></asp:TextBox>
                                        </td>
                                        <td align="right">&nbsp;</td>
                                    </tr>


                                    <tr>
                                        <td>&nbsp;
			                        <asp:Label ID="UI_lblRepairCenter" runat="server" Text="009_Repair Center"></asp:Label>
                                        </td>
                                        <td>:
			                        <asp:Label ID="UI_lblRepairCenterText" runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="UI_lblRepairCenterValue" runat="server" Visible="false"></asp:Label>
                                            <asp:DropDownList ID="UI_cboRepairCenter" runat="server" Visible="false"></asp:DropDownList>
                                        </td>
                                        <td align="right" runat="server" id="UI_trAssistantID1" visible="false">
                                            <asp:Label ID="UI_lblAssistantID" runat="server" Text="015_Assistant ID"></asp:Label>
                                        </td>
                                        <td align="left" runat="server" id="UI_trAssistantID2" visible="false">:
			                        <asp:TextBox ID="UI_txtAssistantID" runat="server" Width="100"></asp:TextBox>
                                        </td>
                                        <td align="right">&nbsp;</td>
                                    </tr>


                                    <tr>
                                        <td>&nbsp;
			                        <asp:Label ID="UI_lblContactPerson" runat="server" Text="016_Contact Person"></asp:Label>
                                        </td>
                                        <td>:
			                        <asp:TextBox ID="UI_txtContactPerson" runat="server" Width="200"></asp:TextBox>
                                        </td>

                                        <td align="right" runat="server">
                                            <asp:Label ID="UI_lblisChoice" runat="server" Text="042_isChoice"></asp:Label>
                                        </td>
                                        <td align="left" runat="server">:
			                        <asp:RadioButtonList runat="server" ID="UI_radisChoice" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                        <asp:ListItem Text="028_No" Value="0" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="027_Yes" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                        </td>
                                        <td align="right">&nbsp;</td>
                                    </tr>


                                    <tr>
                                        <td>&nbsp;
			                        <asp:Label ID="UI_lblTEL" runat="server" Text="011_TEL"></asp:Label>
                                        </td>
                                        <td>:
			                        <asp:TextBox ID="UI_txtTEL" runat="server" Width="400" MaxLength="50"></asp:TextBox>
                                        </td>

                                        <td align="right" >&nbsp;
			                        <asp:Label ID="UI_CU_TIPTOP_ID" runat="server" Text="TIPTOP CutomerID"></asp:Label>
                                        </td>
                                        <td>:
			                        <asp:TextBox ID="UITxt_CU_TIPTOP_ID" runat="server" Width="100" MaxLength="20" ReadOnly="true"></asp:TextBox>
                                            <%--style="background-color: #787878; color:#BDC0BA;"--%></td>
                                        <td align="right">&nbsp;</td>
                                    </tr>

                                    <tr>
                                        <td>&nbsp;
			                        <asp:Label ID="UI_lblAddress1" runat="server" Text="017_Address (1)"></asp:Label>
                                        </td>
                                        <td>: 
					                <asp:TextBox ID="UI_txtAddress1" runat="server" Width="400"></asp:TextBox>
                                        </td>
                                        <td align="right" >&nbsp;
			                        <asp:Label ID="UI_CU_SERVICE_CHG" runat="server" Text="Service Charge"></asp:Label>
                                        </td>
                                        <td>:
			                        <asp:TextBox ID="UITxt_CU_SERVICE_CHG" runat="server" Width="100" MaxLength="20"></asp:TextBox>
                                        </td>
                                        <td align="right">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
			                        <asp:Label ID="UI_lblAddress2" runat="server" Text="018_Address (2)"></asp:Label>
                                        </td>
                                        <td>: 
					                <asp:TextBox ID="UI_txtAddress2" runat="server" Width="400"></asp:TextBox>
                                        </td>
                                        <td align="right" >&nbsp;
                                            <asp:Label ID="UI_CU_SERVICE_CHG_DISCOUNT" runat="server" Text="Service Charge Discount"></asp:Label>
                                        </td>
                                        <td>:
                                            <asp:RadioButtonList ID="UITxt_CU_SERVICE_CHG_DISCOUNT" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="1" Text="007_On" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="0" Text="008_Off"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
			                        <asp:Label ID="UI_lblAddress3" runat="server" Text="019_Address (3)"></asp:Label>
                                        </td>
                                        <td>: 
					                <asp:TextBox ID="UI_txtAddress3" runat="server" Width="400"></asp:TextBox>
                                        </td>
                                        <!--'新增服務費折讓開關功能 by buck Add 20260427 begin-->
                                        <td align="right" >&nbsp;
                                            <asp:Label ID="UI_CU_DISCOUNT_OFF" runat="server" Text="DISCOUNT OFF"></asp:Label>
                                        </td>
                                        <td>:
                                            <asp:TextBox ID="UITxt_CU_DISCOUNT_OFF" runat="server" Width="100" MaxLength="20"></asp:TextBox>
                                        </td>
                                        <!--'新增服務費折讓開關功能 by buck Add 20260427 end-->
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
			                        <asp:Label ID="UI_lblAddress4" runat="server" Text="020_Address (4)"></asp:Label>
                                        </td>
                                        <td colspan="4">: 
					                <asp:TextBox ID="UI_txtAddress4" runat="server" Width="400"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
			                        <asp:Label ID="UI_lblEMail" runat="server" Text="021_eMail"></asp:Label>
                                        </td>
                                        <td colspan="4">: 
					                <asp:TextBox ID="UI_txtEMail" runat="server" Width="800"></asp:TextBox>
                                            <span style="color: red; font-size: 14px;">※信箱請用逗號分隔</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
			                        <asp:Label ID="UI_lblFinanceEMail" runat="server" Text="043_FinanceEMail"></asp:Label>
                                        </td>
                                        <td colspan="4">: 
					                <asp:TextBox ID="UI_txtFinanceEMail" runat="server" Width="1140px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
			                        <asp:Label ID="UI_lblStatus" runat="server" Text="022_Account status"></asp:Label>
                                        </td>
                                        <td colspan="4">:
			                        <asp:Label ID="UI_lblStatusText" runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="UI_lblStatusValue" runat="server" Visible="false"></asp:Label>
                                            <asp:RadioButtonList ID="UI_opgStatus" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" Visible="false">
                                                <asp:ListItem Value="1" Text="007_Open" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="0" Text="008_Close"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="5" align="center">
                                            <asp:Button ID="UI_cmdSubmit_Company" runat="server" Text="_Submit" CssClass="Confirm_l" ValidationGroup="CustomerGroup" OnClientClick="onProgress('Save')" />
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


            <tr runat="server" id="UI_trInformationTittle" visible="false">
                <td width="24" height="27" background="Images/pic_14.gif">&nbsp;</td>
                <td height="27" background="Images/pic_15.gif">
                    <table border="0" width="100%" cellspacing="0" cellpadding="0">
                        <tr>
                            <td width="3%">&nbsp;</td>
                            <td width="94%" class="default">
                                <table border="0" width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="60%" align="left" class="default">
                                            <asp:Label ID="UI_lblInformationTittle" runat="server" Text="023_User Information" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="UI_cmdAdd" runat="server" Text="_Add Line" CssClass="Search" OnClick="UI_cmdAdd_Click" OnClientClick="onProgress('Process')" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td width="3%">&nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>

            <tr runat="server" id="UI_trCustomerUser" visible="false">
                <td background="Images/pic_20.gif">&nbsp;</td>
                <td valign="top" bgcolor="#E3D8BE" align="center">
                    <table border="0" cellspacing="0" cellpadding="0" height="100%" width="100%">
                        <tr>
                            <td width="3%">&nbsp;</td>
                            <td width="97%" align="left" class="default">
                                <!--[Begin]新增資料列表-->
                                <div align="center">
                                    <asp:GridView ID="UI_CustomerUser" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="UI_SeqID" runat="server" Text='<%# Eval("SeqID") %>'></asp:Label>
                                                    <asp:Label ID="UI_Cuusad" runat="server" Text='<%# Eval("CUUS_AD") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="UI_oldAccountID" runat="server" Text='<%# Eval("CUUS_oldAccountID") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="UI_ISMANAGER" runat="server" Text='<%# Eval("CUUS_ISMANAGER") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="UI_ISStatus" runat="server" Text='<%# Eval("CUUS_STATUS") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="024_User ID">
                                                <HeaderStyle Width="15%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="UI_AccountID" runat="server" Width="100" Text='<%# Eval("CUUS_ACCOUNTID") %>' Visible="false"></asp:TextBox>
                                                    <asp:Label ID="UI_lblAccountID" runat="server" Text='<%# Eval("CUUS_ACCOUNTID") %>' Visible="false"></asp:Label>
                                                    <asp:RequiredFieldValidator ID="rfv_AccountID" runat="server" ErrorMessage="036_請輸入帳號" Display="None" ValidationGroup="UserGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="025_Password">
                                                <HeaderStyle Width="15%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="UI_Pwd" runat="server" Width="80" Text='<%# Eval("CUUS_PWD") %>' MaxLength="20"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfv_Password" runat="server" ErrorMessage="037_請輸入密碼" Display="None" ValidationGroup="UserGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="011_TEL">
                                                <HeaderStyle Width="15%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="UI_Tel" runat="server" Width="120" Text='<%# Eval("CUUS_TEL") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="026_Email">
                                                <HeaderStyle Width="30%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="UI_EMail" runat="server" Width="300px" Text='<%# Eval("CUUS_EMAIL") %>'></asp:TextBox>
                                                    <%--<asp:RegularExpressionValidator ID="revUIEMail_1" runat="server" ErrorMessage="035_EMail輸入格式有誤"  Display="None" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="UserGroup" SetFocusOnError="true"></asp:RegularExpressionValidator>--%>
                                                    <asp:RegularExpressionValidator ID="revUIEMail_1" runat="server" ErrorMessage="035_EMail輸入格式有誤" Display="None" ValidationExpression="^([^@\s,]+@[^@\s,]+\.[^@\s,]+)(\s*,\s*[^@\s,]+@[^@\s,]+\.[^@\s,]+)*$" ValidationGroup="UserGroup" SetFocusOnError="true"></asp:RegularExpressionValidator>
                                                    <asp:RequiredFieldValidator ID="revUIEMail_2" runat="server" ErrorMessage="035_EMail輸入格式有誤" Display="None" ValidationGroup="UserGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="005_Status">
                                                <HeaderStyle Width="5%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="UI_Status" runat="server">
                                                        <asp:ListItem Value="1" Text="007_Open"></asp:ListItem>
                                                        <asp:ListItem Value="0" Text="008_Close"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="012_Manager">
                                                <HeaderStyle Width="15%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="UI_lblManager"></asp:Label>
                                                    <asp:DropDownList ID="UI_Manager" runat="server" Visible="false">
                                                        <asp:ListItem Value="1" Text="027_Yes"></asp:ListItem>
                                                        <asp:ListItem Value="0" Text="028_No"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="Text_Head" />
                                        <RowStyle CssClass="TR_1" />
                                        <AlternatingRowStyle CssClass="ListRowEven" />
                                        <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Center" />
                                    </asp:GridView>
                                </div>
                                <!--[End]新增資料列表-->
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>

            <tr runat="server" id="UI_trSubmit" visible="false">
                <td background="Images/pic_20.gif" height="130">&nbsp;</td>
                <td valign="top" bgcolor="#E3D8BE" align="center">
                    <!--[Begin]Submit-->
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" height="50px">
                        <tr>
                            <td width="3%">&nbsp;</td>
                            <td width="94%" align="center" class="default">
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
        <asp:AsyncPostBackTrigger ControlID="UI_cmdSubmit_Company" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="UI_cmdAdd" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="UI_cmdCancel" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="UI_cmdSubmit" EventName="Click" />
        <asp:PostBackTrigger ControlID="ucMessage" />
    </Triggers>
</asp:UpdatePanel>



<asp:Label ID="UI_lblSalesIDText" runat="server" Visible="false"></asp:Label>
<asp:Label ID="UI_lblPreviousPage_CuNo" runat="server" Visible="false"></asp:Label>
<asp:Label ID="UI_lblPreviousPage_CuusID" runat="server" Visible="false"></asp:Label>


<asp:RequiredFieldValidator ID="rfvCusName" runat="server" ErrorMessage="030_請輸入客戶名稱" ControlToValidate="UI_txtCusName" Display="None" TabIndex="0" ValidationGroup="CustomerGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
<asp:RequiredFieldValidator ID="rfvCustomerID" runat="server" ErrorMessage="031_請輸入客戶編號" ControlToValidate="UI_txtCustomerID" Display="None" TabIndex="0" ValidationGroup="CustomerGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
<asp:RequiredFieldValidator ID="rfvSalesID" runat="server" ErrorMessage="032_請輸入業務代碼" ControlToValidate="UI_txtSalesID" Display="None" TabIndex="0" ValidationGroup="CustomerGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
<asp:CustomValidator ID="cvCountry" runat="server" ClientValidationFunction="Validate_Country" ErrorMessage="033_請選取國家名稱" Display="None" Operator="DataTypeCheck" ValidationGroup="CustomerGroup"></asp:CustomValidator>
<asp:CustomValidator ID="cvRepairCenter" runat="server" ClientValidationFunction="Validate_RepairCenter" ErrorMessage="034_請選取維修中心" Display="None" Operator="DataTypeCheck" ValidationGroup="CustomerGroup"></asp:CustomValidator>
<%--<asp:RegularExpressionValidator ID="revEMail" runat="server" ErrorMessage="035_EMail輸入格式有誤" ControlToValidate="UI_txtEMail" Display="None" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="CustomerGroup" SetFocusOnError="true"></asp:RegularExpressionValidator>--%>


<asp:ValidationSummary ID="vsCompany" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="CustomerGroup" />
<asp:ValidationSummary ID="vsCustomer" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="UserGroup" />
