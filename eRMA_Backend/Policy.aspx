<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Policy.aspx.vb" Inherits="Policy" Title="Untitled Page" ValidateRequest="false" %>

<%@ Register Src="ascx/ucProgressStatus.ascx" TagName="ucProgressStatus" TagPrefix="uc1" %>
<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <script type="text/javascript">
        function Validate_PolicyDFLNO(oSrc, args) {
            var sValue = document.getElementById('<%=UI_cboPolicyDFLNO.ClientID %>').value;

            if (sValue == -1) {
                args.IsValid = false;
            } else {
                args.IsValid = true;
            }
        }
    </script>

    <uc1:ucProgressStatus ID="ucProgressStatus" runat="server" Visible="false" />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>

            <table border="0" width="100%" id="table1" cellspacing="0" cellpadding="0" height="100%">
                <tr>
                    <td width="24" background="Images/pic_12.gif">&nbsp;</td>
                    <td valign="top" align="left">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0" height="100%">
                            <!--[Begin]Tittle-->
                            <tr>
                                <td width="3%">&nbsp;</td>
                                <td width="94%" align="left">
                                    <asp:Label ID="UI_lblTittle" runat="server" Text="001_Setting - Policy" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
                                </td>
                                <td width="3%">&nbsp;</td>
                            </tr>

                            <tr>
                                <td width="3%">&nbsp;</td>
                                <td width="94%" align="left">
                                    <asp:DropDownList ID="UI_cboRepairCenter" runat="server">
                                        <asp:ListItem>Mplus</asp:ListItem>
                                        <asp:ListItem>Cipherlab</asp:ListItem>
                                    </asp:DropDownList>
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
                                            <td>
                                                <asp:Label ID="UI_lblPolicyDFLNO" runat="server" Text="002_PolicyDFLNO"></asp:Label>:
		                                    <asp:DropDownList ID="UI_cboPolicyDFLNO" runat="server" ValidationGroup="vsPolicy" AutoPostBack="true"></asp:DropDownList><br />
                                                <asp:Label ID="UI_lblHTML" runat="server" Text="007_HTMLDesc"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="UI_txtPolicy" runat="server" TextMode="MultiLine" Rows="25" Columns="90"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="UI_cmdSave" runat="server" Text="_Save" CssClass="Confirm_l" ValidationGroup="vsPolicy" OnClientClick="onProgress('Save')" />
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
                <tr>
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
                <tr>
                    <td background="Images/pic_20.gif">&nbsp;</td>
                    <td valign="top" bgcolor="#E3D8BE" align="center">
                        <!--[Begin]Submit-->
                        <table border="0" cellspacing="0" cellpadding="0" width="100%" height="100%">
                            <tr>
                                <td width="3%">&nbsp;</td>
                                <td width="94%" align="center" class="default">&nbsp;</td>
                                <td width="3%">
                                    <asp:Label ID="UI_lblPolicyID" runat="server" Visible="false"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <!--[End]Submit-->
                    </td>
                </tr>
            </table>

            <uc2:ucMessage ID="ucMessage" runat="server" />

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="UI_cmdSave" EventName="Click" />
            <asp:PostBackTrigger ControlID="ucMessage" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:RequiredFieldValidator ID="rfv_txtPolicy" runat="server" ErrorMessage="005_請輸入敘述" Display="None" ControlToValidate="UI_txtPolicy" ValidationGroup="vsPolicy"></asp:RequiredFieldValidator>
    <asp:CustomValidator ID="cv_cboPolicyDFLNO" runat="server" ClientValidationFunction="Validate_PolicyDFLNO" ErrorMessage="004_請選擇語系" Display="None" ValidationGroup="vsPolicy"></asp:CustomValidator>
    <asp:ValidationSummary ID="vsPolicy" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="vsPolicy" />

</asp:Content>

