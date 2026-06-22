<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="MaintainAccount.aspx.vb" Inherits="MaintainAccount" Title="Untitled Page" %>

<%@ Register Src="ascx/ucProgressStatus.ascx" TagName="ucProgressStatus" TagPrefix="uc1" %>
<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">

    <script type="text/javascript" language="javascript">
        function Validate_AuthorityLevel(oSrc, args) {
            var sValue1 = document.getElementById('<%=UI_cboAuthorityLevel.ClientID %>').value;

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

            <table border="0" width="100%" id="table1" cellspacing="0" cellpadding="0" height="100%">
                <tr height="60%">
                    <td width="24" background="Images/pic_12.gif">&nbsp;</td>
                    <td valign="top" align="left">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0" height="100%">
                            <!--[Begin]Tittle-->
                            <tr>
                                <td width="3%">&nbsp;</td>
                                <td width="94%" align="left">
                                    <asp:Label ID="UI_lblTittle" runat="server" Text="001_Setting - Maintain Account" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
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
                                            <td colspan="4">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td width="15%" align="left">
                                                <asp:Label ID="UI_lblAccountID1" runat="server" Text="013_Account ID."></asp:Label>
                                            </td>
                                            <td width="1" align="left">:</td>
                                            <td width="40%" align="left">
                                                <asp:Label ID="UI_lblAccountID" runat="server" Visible="false"></asp:Label>
                                                <asp:TextBox ID="UI_txtAccountID" runat="server" Width="150" Visible="false"></asp:TextBox>
                                            </td>
                                            <td width="20%" align="right">
                                                <asp:Label ID="UI_lblAuthorityLevel" runat="server" Text="014_Authority Level"></asp:Label>
                                            </td>
                                            <td width="30%" align="left">:
		                                    <asp:DropDownList ID="UI_cboAuthorityLevel" runat="server">
                                                <asp:ListItem Value="-1" Text="015_-Select-" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="0" Text="016_by Center"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="017_All"></asp:ListItem>
                                            </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="UI_lblName" runat="server" Text="006_Name"></asp:Label>
                                            </td>
                                            <td width="1" align="left">:</td>
                                            <td align="left">
                                                <asp:TextBox ID="UI_txtName" runat="server" Width="150"></asp:TextBox>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="UI_lblUpperMail" runat="server" Text="018_Upper Supervisor's Mail"></asp:Label>
                                            </td>
                                            <td align="left">:
		                                    <asp:TextBox ID="UI_txtUpperMail" runat="server" Width="150"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="UI_lblPassword" runat="server" Text="019_Password"></asp:Label>
                                            </td>
                                            <td width="1" align="left">:</td>
                                            <td align="left">
                                                <asp:TextBox ID="UI_txtPassword" runat="server" TextMode="Password" Width="150" MaxLength="8"></asp:TextBox>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="UI_lblMail" runat="server" Text="011_EMail"></asp:Label>
                                            </td>
                                            <td align="left">:
				                            <asp:TextBox ID="UI_txtMail" runat="server" Width="150"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="UI_lblRepairCenter" runat="server" Text="002_Repair Center" ForeColor="red"></asp:Label>
                                            </td>
                                            <td width="1" align="left" valign="bottom">:</td>
                                            <td align="left" colspan="3">
                                                <div style="width: 250px; height: 90px; overflow-y: auto; background: #ffffff; border-style: groove; border-width: thin">
                                                    <asp:CheckBoxList ID="UI_chkRepairCenter" runat="server" RepeatDirection="Vertical" RepeatLayout="Flow"></asp:CheckBoxList>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="UI_lblRole" runat="server" Text="020_Role"></asp:Label>
                                            </td>
                                            <td width="1" align="left">:</td>
                                            <td align="left" colspan="3">
                                                <asp:CheckBoxList ID="UI_chkRole" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                    <asp:ListItem Value="1" Text="021_Receiver"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="002_Repair center"></asp:ListItem>
                                                    <asp:ListItem Value="3" Text="022_Sales"></asp:ListItem>
                                                    <asp:ListItem Value="4" Text="023_Shipping"></asp:ListItem>
                                                    <asp:ListItem Value="9" Text="024_Admin"></asp:ListItem>
                                                    <asp:ListItem Value="8" Text="036_Upload"></asp:ListItem>
                                                    <asp:ListItem Value="6" Text="038_Warranty_Setting"></asp:ListItem>
                                                    <asp:ListItem Value="7" Text="037_Warranty_Selling"></asp:ListItem>
                                                    <asp:ListItem Value="C" Text="039_Cost"></asp:ListItem>
                                                    <asp:ListItem Value="A" Text="034_Assistant"></asp:ListItem>
                                                </asp:CheckBoxList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="UI_lblStatus" runat="server" Text="010_Status"></asp:Label>
                                            </td>
                                            <td width="1" align="left">:</td>
                                            <td align="left" colspan="3">
                                                <asp:RadioButtonList ID="UI_opgStatus" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                    <asp:ListItem Value="1" Text="025_Open" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Value="0" Text="026_Close"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr id="UI_TrAdminluad" runat="server" visible="false">
                                            <td align="left">
                                                <asp:Label ID="UI_lblLastEditor" runat="server" Text="027_Last Editor"></asp:Label>
                                            </td>
                                            <td width="1" align="left">:</td>
                                            <td align="left" colspan="3">
                                                <asp:Label ID="UI_lblLastEditorText" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="UI_TrAdminlustmp" runat="server" visible="false">
                                            <td align="left">
                                                <asp:Label ID="UI_lblLastDate" runat="server" Text="028_Last Edit Time"></asp:Label>
                                            </td>
                                            <td width="1" align="left">:</td>
                                            <td align="left" colspan="3">
                                                <asp:Label ID="UI_lblLastDateText" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5" align="center">
                                                <input id="UI_cmdBack" runat="server" type="button" value="_Back" class="Problem_Edit" onclick="javascript:history.back();" />
                                                <asp:Button ID="UI_cmdSave" runat="server" Text="_Save" CssClass="Confirm_l" ValidationGroup="AccountGroup" OnClientClick="onProgress('Save')" />
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
                    <td height="27" background="Images/pic_15.gif">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="3%">&nbsp;</td>
                                <td width="94%" class="default">
                                    <table border="0" width="100%" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td width="60%" align="left">&nbsp;</td>
                                            <td align="right">&nbsp;</td>
                                        </tr>
                                    </table>
                                </td>
                                <td width="3%">&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <tr height="40%">
                    <td background="Images/pic_20.gif">&nbsp;</td>
                    <td valign="top" bgcolor="#E3D8BE" align="center">
                        <table border="0" cellspacing="0" cellpadding="0" height="100%" width="100%">
                            <tr>
                                <td width="3%">&nbsp;</td>
                                <td width="94%">&nbsp;</td>
                                <td width="3%">&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <asp:Label runat="server" ID="UI_lblPreviousPage_adID" Visible="false"></asp:Label>
            <uc2:ucMessage ID="ucMessage" runat="server" />

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="UI_cmdSave" EventName="Click" />
            <asp:PostBackTrigger ControlID="ucMessage" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:RequiredFieldValidator ID="rfvAccountID" runat="server" ErrorMessage="029_請輸入帳號" ControlToValidate="UI_txtAccountID" Display="None" TabIndex="0" ValidationGroup="AccountGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="rfvName" runat="server" ErrorMessage="030_請輸入名稱" ControlToValidate="UI_txtName" Display="None" TabIndex="0" ValidationGroup="AccountGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ErrorMessage="031_請輸入密碼" ControlToValidate="UI_txtPassword" Display="None" TabIndex="0" ValidationGroup="AccountGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
    <asp:CustomValidator ID="cvAuthorityLevel" runat="server" ClientValidationFunction="Validate_AuthorityLevel" ErrorMessage="032_請選取權限範圍等級" Display="None" Operator="DataTypeCheck" ValidationGroup="AccountGroup"></asp:CustomValidator>
    <%--<asp:CustomValidator ID="cvRole" runat="server" ClientValidationFunction="Validate_Role" ErrorMessage="034_請勾取角色" Display ="None" Operator="DataTypeCheck" ValidationGroup ="AccountGroup"></asp:CustomValidator>--%>

    <asp:RegularExpressionValidator ID="revUpperMail" runat="server" ErrorMessage="033_電子郵件輸入格式有誤" ControlToValidate="UI_txtUpperMail" Display="None" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="AccountGroup" SetFocusOnError="true"></asp:RegularExpressionValidator>

    <asp:RequiredFieldValidator ID="rfvMail" runat="server" ErrorMessage="035_請輸入電子郵件" ControlToValidate="UI_txtMail" Display="None" TabIndex="0" ValidationGroup="AccountGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator ID="revMail" runat="server" ErrorMessage="033_電子郵件輸入格式有誤" ControlToValidate="UI_txtMail" Display="None" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="AccountGroup" SetFocusOnError="true"></asp:RegularExpressionValidator>

    <asp:ValidationSummary ID="vsAccount" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="AccountGroup" />

</asp:Content>

