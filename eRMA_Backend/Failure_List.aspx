<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Failure_List.aspx.vb" Inherits="Failure_List" Title="Untitled Page" %>

<%@ Register Src="ascx/ucProgressStatus.ascx" TagName="ucProgressStatus" TagPrefix="uc1" %>
<%@ Register Src="ascx/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">

    <script type="text/javascript" language="javascript">
        function FrmDelete() {
            if (confirm(delMsg)) {
                onProgress('Delete')
                return true;
            }
            return false;
        }
    </script>

    <uc1:ucProgressStatus ID="ucProgressStatus" runat="server" />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>

            <table border="0" cellspacing="0" cellpadding="0" width="100%" height="100%">
                <tr height="10%">
                    <td width="24" background="Images/pic_12.gif">&nbsp;</td>
                    <td valign="top" align="left">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <!--[Begin]Tittle-->
                            <tr>
                                <td width="1%">&nbsp;</td>
                                <td width="94%" align="left">
                                    <asp:Label ID="UI_lblTittle" runat="server" Text="001_Setting - Failure Reason" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
                                </td>
                                <td width="1%">&nbsp;</td>
                            </tr>
                            <!--[End]Tittle-->
                            <!--[Begin]資料查詢條件區-->
                            <tr>
                                <td>&nbsp;</td>
                                <td align="left">
                                    <table width="95%" border="0" cellspacing="1" cellpadding="0" class="default">
                                        <tr height="40px">
                                            <td width="15%" align="left">
                                                <asp:Label ID="UI_lblCategory" runat="server" Text="006_Category Searching"></asp:Label>
                                            </td>
                                            <td width="85%" align="left">
                                                <asp:Label ID="UI_lblLanguage_Search" runat="server" Text="024_Language"></asp:Label>:
		                                <asp:DropDownList ID="UI_cboLanguage_Search" runat="server" AutoPostBack="true" OnSelectedIndexChanged="UI_cboLanguage_Search_SelectedIndexChanged"></asp:DropDownList>
                                                <asp:DropDownList ID="UI_cboFarcNameSearch" runat="server"></asp:DropDownList>
                                                <asp:Button ID="UI_cmdSearch" runat="server" Text="_Search" CssClass="Search" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">&nbsp;</td>
                                        </tr>
                                    </table>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <!--[End]資料查詢條件區-->
                        </table>
                    </td>
                </tr>
                <tr height="10%">
                    <td width="24" background="Images/pic_14.gif">&nbsp;</td>
                    <td background="Images/pic_15.gif">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="default">
                            <tr height="28" valign="middle">
                                <td width="1%">&nbsp;</td>
                                <td width="30%" align="left" class="default">
                                    <asp:Label ID="UI_lblFailureTittle" runat="server" Text="008_Failure Reason Information" Font-Bold="true"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label ID="UI_lblLanguage" runat="server" Text="024_Language"></asp:Label>:
                            <asp:DropDownList ID="UI_cboLanguage" runat="server"></asp:DropDownList>
                                    <asp:Label ID="UI_lblFarcNo" runat="server" Text="004_Failure No"></asp:Label>:
	                        <asp:TextBox ID="UI_txtFarcNo" runat="server" Width="60"></asp:TextBox>
                                    <asp:Label ID="UI_lblFarcName" runat="server" Text="005_Failure Name"></asp:Label>:
	                        <asp:TextBox ID="UI_txtFarcName" runat="server" Width="120px"></asp:TextBox>
                                    <asp:Button ID="UI_cmdAddClass" runat="server" Text="003_Add" CssClass="Problem_Edit" ValidationGroup="FailureReasonClassGroup" OnClientClick="onProgress('Save')" />&nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>


                <tr height="350">
                    <td background="Images/pic_20.gif">&nbsp;</td>
                    <td valign="top" bgcolor="#E3D8BE" align="center">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="1%">&nbsp;</td>
                                <td width="99%" align="left" class="default">
                                    <!--[Begin]資料列表表單-->
                                    <div align="center">
                                        <asp:GridView ID="UI_FailureReason" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric" AllowSorting="true">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderStyle Width="3%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="UI_SeqID" runat="server"></asp:Label>
                                                        <asp:Label ID="UI_FARC_DFLNO" runat="server" Text='<%# Eval("FARC_DFLNO") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_FARC_NO" runat="server" Text='<%# Eval("FARC_NO") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_FARC_VISIBLE" runat="server" Text='<%# Eval("FARC_VISIBLE") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="024_LvName" SortExpression="LvName">
                                                    <HeaderStyle Width="10%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="UI_lblLvName" runat="server" Text='<%# Eval("LvName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="004_Failure No" SortExpression="FARC_NO">
                                                    <HeaderStyle Width="12%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="UI_lblFarcNO" runat="server" Text='<%# Eval("FARC_NO") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="005_Failure Name" SortExpression="FARC_NAME">
                                                    <HeaderStyle Width="31%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="UI_lblFarcName" runat="server" Text='<%# Eval("FARC_NAME") %>'></asp:Label>
                                                        <asp:TextBox ID="UI_txtFarcName" runat="server" Width="350px" Text='<%# Eval("FARC_NAME") %>' Visible="false"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfv_FarcName" runat="server" ErrorMessage="019_請輸入不良原因類別名稱" Display="None" TabIndex="0" ValidationGroup="FailureListGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="015_Status" SortExpression="Status">
                                                    <HeaderStyle Width="10%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="UI_lblFarcStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                                        <asp:DropDownList ID="UI_Status" runat="server" Visible="false">
                                                            <asp:ListItem Text="016_Open" Value="1" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="017_Close" Value="0"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="FARC_LUSTMP" SortExpression="FARC_LUSTMP" HeaderText="013_Edit Date" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center" HtmlEncode="false" DataFormatString="{0:yyyy/MM/dd}"></asp:BoundField>
                                                <asp:BoundField DataField="FARC_LUAD" SortExpression="FARC_LUAD" HeaderText="014_Last Editor" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>

                                                <asp:TemplateField HeaderText="027_Edit">
                                                    <HeaderStyle Width="8%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Button ID="UI_cmdEdit" runat="server" Text="005_Edit" CssClass="Problem_Edit" CommandName="cmdEdit" CommandArgument='<%#Me.UI_FailureReason.Rows.Count%>' />
                                                        <asp:Button ID="UI_cmdSave" runat="server" Text="002_Save" CssClass="Problem_Edit" CommandName="cmdSave" CommandArgument='<%#Me.UI_FailureReason.Rows.Count%>' Visible="false" ValidationGroup="FailureListGroup" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="028_Detail">
                                                    <HeaderStyle Width="8%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Button ID="UI_cmdDetail" runat="server" Text="049_Detail" CssClass="Problem_Edit" CommandName="cmdDetail" CommandArgument='<%#Me.UI_FailureReason.Rows.Count%>' PostBackUrl="Failure.aspx" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="029_Delete">
                                                    <HeaderStyle Width="8%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Button ID="UI_cmdDel" runat="server" Text="007_Delete" CssClass="Problem_Edit" CommandName="cmdDel" CommandArgument='<%#Me.UI_FailureReason.Rows.Count%>' OnClientClick="return FrmDelete()" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <HeaderStyle CssClass="Text_Head" />
                                            <RowStyle CssClass="TR_1" />
                                            <AlternatingRowStyle CssClass="ListRowEven" />
                                            <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Center" />
                                        </asp:GridView>
                                    </div>
                                    <!--[End]資料列表表單-->
                                </td>
                            </tr>
                            <tr height="30" valign="bottom">
                                <td colspan="2" align="center">&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <asp:Label runat="server" ID="UI_lblPreviousPage_FARCNO" Visible="false"></asp:Label>
            <asp:Label runat="server" ID="UI_lblPreviousPage_DFLNO" Visible="false"></asp:Label>
            <uc2:ucMessage ID="ucMessage" runat="server" />

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="UI_cmdAddClass" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="UI_cmdSearch" EventName="Click" />
            <asp:PostBackTrigger ControlID="ucMessage" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:RequiredFieldValidator ID="rfvFarNo" runat="server" ControlToValidate="UI_txtFarcNo" ErrorMessage="018_請輸入不良原因類別代碼" Display="None" TabIndex="0" ValidationGroup="FailureReasonClassGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="rfvFarcName" runat="server" ControlToValidate="UI_txtFarcName" ErrorMessage="019_請輸入不良原因類別名稱" Display="None" TabIndex="0" ValidationGroup="FailureReasonClassGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
    <%--<asp:CustomValidator ID="cvLanguage" runat="server" ClientValidationFunction="Validate_Language" ErrorMessage="023_請選取語系" Display ="None" Operator="DataTypeCheck" ValidationGroup ="FailureReasonClassGroup"></asp:CustomValidator>--%>

    <asp:ValidationSummary ID="vsFailureReasonClass" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="FailureReasonClassGroup" />

    <asp:ValidationSummary ID="vsFailureList" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="FailureListGroup" />

</asp:Content>

