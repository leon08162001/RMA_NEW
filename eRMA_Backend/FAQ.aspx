<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="FAQ.aspx.vb" Inherits="FAQ" Title="Untitled Page" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <script type="text/javascript">
        function Validate_Category1(oSrc, args) {
            var sValue1 = document.getElementById('<%=UI_cboCategory1.ClientID %>').value;

            if (sValue1 == -1) {
                args.IsValid = false;
            } else {
                args.IsValid = true;
            }
        }

        function Validate_Category2(oSrc, args) {
            var sValue = document.getElementById('<%=UI_cboCategory2.ClientID %>').value;

            if (sValue == -1) {
                args.IsValid = false;
            } else {
                args.IsValid = true;
            }
        }


        function FrmDelete() {
            if (confirm(delMsg)) {
                onProgress('Delete')
                return true;
            }
            return false;
        }
    </script>

    <table border="0" width="100%" id="table4" cellspacing="0" cellpadding="0" height="100%">
        <tr>
            <td width="24">&nbsp;</td>
            <td valign="top" align="left">
                <table border="0" width="100%" cellspacing="0" cellpadding="0" height="100%">
                    <!--[Begin]Tittle-->
                    <tr>
                        <td width="3%">&nbsp;</td>
                        <td width="94%" align="left">
                            <asp:Label ID="UI_lblTittle" runat="server" Text="001_Setting - FAQ" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
                        </td>
                        <td width="3%">&nbsp;</td>
                    </tr>
                    <!--[End]Tittle-->
                    <!--[Begin]新增資料-->
                    <tr>
                        <td>&nbsp;</td>
                        <td align="left">
                            <table width="100%" align="center" border="0" cellspacing="1" cellpadding="0" class="default">
                                <tr>
                                    <td colspan="2">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td width="15%">&nbsp;
				                        <asp:Label ID="UI_lblCategory1" runat="server" Text="010_Category(1)"></asp:Label>
                                    </td>
                                    <td width="85%">
                                        <asp:DropDownList ID="UI_cboCategory1" runat="server" AutoPostBack="true" ValidationGroup="vsFAQ"></asp:DropDownList>
                                        <asp:Button ID="UI_cmdAdd_Class" runat="server" Text="_Add" CssClass="Pick" Width="50" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
				                        <asp:Label ID="UI_lblCategory2" runat="server" Text="011_Category(2)"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="UI_cboCategory2" runat="server" ValidationGroup="vsFAQ"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr height="80" valign="top">
                                    <td>&nbsp;
				                        <asp:Label ID="UI_lblQuestion" runat="server" Text="004_Question"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="UI_txtQuestion" runat="server" TextMode="MultiLine" Rows="3" Columns="38" Width="100%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">&nbsp;
				                        <asp:Label ID="UI_lblAnswer" runat="server" Text="007_Answer"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="UI_txtAnswer" runat="server" TextMode="MultiLine" Rows="12" Columns="38" Width="100%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
				                        <asp:Label ID="UI_lblISSUEDATE" runat="server" Text="015_IssueDate"></asp:Label>
                                    </td>
                                    <td>:<asp:TextBox ID="UI_txtISSUEDATE" runat="server" Width="100" ValidationGroup="vsFAQ"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
				                        <asp:Label ID="UI_lblStatus" runat="server" Text="012_Status"></asp:Label>
                                    </td>
                                    <td>:
				                        <asp:RadioButtonList ID="UI_opgVisible" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="1" Text="013_Open" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="0" Text="014_Close"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr id="UI_TrFAQluad" runat="server">
                                    <td>&nbsp;
				                        <asp:Label ID="UI_lblFAQluad" runat="server" Text="016_Last Editor"></asp:Label>
                                    </td>
                                    <td>:<asp:Label ID="UI_lblFAQluadText" runat="server"></asp:Label>
                                    </td>
                                </tr>

                                <tr id="UI_TrFAQlustmp" runat="server">
                                    <td>&nbsp;
				                        <asp:Label ID="UI_lblFAQlustmp" runat="server" Text="017_Last Edit Time"></asp:Label>
                                    </td>
                                    <td>:<asp:Label ID="UI_lblFAQlustmpText" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td align="center">
                                        <asp:Button ID="UI_cmdSave" runat="server" Text="_Save" CssClass="Confirm_l" ValidationGroup="vsFAQ" OnClientClick="onProgress('Save')" OnClick="UI_cmdSave_Click" />
                                        <asp:Button ID="UI_cmdDel" runat="server" Text="_Delete" CssClass="Confirm_l" OnClientClick="return FrmDelete()" OnClick="UI_cmdDel_Click" />
                                        <input id="UI_cmdBack1" runat="server" type="button" value="_back" class="Confirm_l" onclick="javascript:history.back();" />
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
            <td width="24" height="27">&nbsp;</td>
            <td height="27">
                <table border="0" width="100%" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="3%">&nbsp;</td>
                        <td width="94%">&nbsp;</td>
                        <td width="3%">&nbsp;</td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td height="75">&nbsp;</td>
            <td valign="top" align="center">&nbsp;
			    <asp:Label ID="UI_lblPreviousPage_FAQID" runat="server" Visible="false"></asp:Label>
            </td>
        </tr>
    </table>

    <asp:CustomValidator ID="cv_cboCategory1" runat="server" ClientValidationFunction="Validate_Category1" ErrorMessage="025_Select Category1" Display="None" ValidationGroup="vsFAQ"></asp:CustomValidator>
    <asp:CustomValidator ID="cv_cboCategory2" runat="server" ClientValidationFunction="Validate_Category2" ErrorMessage="026_Select Category2" Display="None" ValidationGroup="vsFAQ"></asp:CustomValidator>
    <asp:RequiredFieldValidator ID="rfv_txtISSUEDATE" runat="server" ErrorMessage="027_必須填寫上架日期" Display="None" ControlToValidate="UI_txtISSUEDATE" ValidationGroup="vsFAQ"></asp:RequiredFieldValidator>
    <asp:CompareValidator ID="cv_txtISSUEDATE" runat="server" ErrorMessage="028_日期格式錯誤" Display="None" ControlToValidate="UI_txtISSUEDATE" Type="Date" Operator="DataTypeCheck" ValidationGroup="vsFAQ"></asp:CompareValidator>
    <asp:ValidationSummary ID="vsFAQ" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="vsFAQ" />

</asp:Content>

