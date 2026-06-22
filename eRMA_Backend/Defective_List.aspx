<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Defective_List.aspx.vb" Inherits="Defective_List" Title="Untitled Page" %>

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
                                <td width="3%">&nbsp;</td>
                                <td width="94%" align="left">
                                    <asp:Label ID="UI_lblTittle" runat="server" Text="001_Setting - Defective" CssClass="text_tittle" ForeColor="#326B9B"></asp:Label>
                                </td>
                                <td width="3%">&nbsp;</td>
                            </tr>
                            <!--[End]Tittle-->
                            <!--[Begin]資料查詢條件區-->
                            <tr>
                                <td>&nbsp;</td>
                                <td align="left">
                                    <table width="95%" border="0" cellspacing="1" cellpadding="0" class="default">
                                        <tr>
                                            <td>
                                                <asp:Label ID="UI_lblLanguage" runat="server" Text="008_Language"></asp:Label>:
		                                <asp:DropDownList ID="UI_cboLanguage" runat="server"></asp:DropDownList>&nbsp;
				                        <asp:Label ID="UI_lblDefectiveName" runat="server" Text="004_Defective Name"></asp:Label>:
				                        <asp:TextBox ID="UI_txtDefectiveName" runat="server" Width="150px"></asp:TextBox>
                                                <asp:Button ID="UI_cmdSearch" runat="server" Text="004_Search" CssClass="Search" OnClientClick="onProgress('Process')" />
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
                <tr>
                    <td width="24" background="Images/pic_14.gif">&nbsp;</td>
                    <td background="Images/pic_15.gif">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="default">
                            <tr>
                                <td width="1%">&nbsp;</td>
                                <td width="20%" align="left" class="default">
                                    <asp:Label ID="UI_lblDefectiveTittle" runat="server" Text="002_Defective Information" Font-Bold="true"></asp:Label>
                                </td>
                                <td width="79%" align="left">
                                    <asp:Label ID="UI_lblLanguageAdd" runat="server" Text="008_Language"></asp:Label>:
		                    <asp:DropDownList ID="UI_cboLanguageAdd" runat="server"></asp:DropDownList>
                                    <asp:Label ID="UI_lblDefectiveNoAdd" runat="server" Text="003_Defective No"></asp:Label>:
				            <asp:TextBox ID="UI_txtDefectiveNoAdd" runat="server" Width="120px"></asp:TextBox>
                                    <asp:Label ID="UI_lblDefectiveNameAdd" runat="server" Text="004_Defective Name"></asp:Label>:
				            <asp:TextBox ID="UI_txtDefectiveNameAdd" runat="server" Width="120px"></asp:TextBox>
                                    <asp:Button ID="UI_cmdAdd" runat="server" Text="003_Add" CssClass="Problem_Edit" ValidationGroup="DefectiveGroup" OnClientClick="onProgress('Save')" />&nbsp;&nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td background="Images/pic_20.gif">&nbsp;</td>
                    <td valign="top" bgcolor="#E3D8BE" align="center">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0" height="100%">
                            <tr>
                                <td width="1%">&nbsp;</td>
                                <td width="99%" align="left" class="default" valign="top">
                                    <!--[Begin]資料列表表單-->
                                    <div align="center">
                                        <asp:GridView ID="UI_dvDefective" runat="server" Width="100%" CellPadding="0" CellSpacing="1" border="0" CssClass="default" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric" AllowSorting="true">
                                            <Columns>
                                                <asp:BoundField DataField="DFL_NAME" HeaderText="010_Language" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>

                                                <asp:TemplateField HeaderText="003_Defective No" SortExpression="DEFECTIVE_NO">
                                                    <HeaderStyle Width="10%" Height="20px" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt" Height="25px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="UI_SeqID" runat="server" Text='<%# Eval("DEFECTIVE_NO") %>'></asp:Label>
                                                        <asp:Label ID="UI_DefectiveNo" runat="server" Text='<%# Eval("DEFECTIVE_NO") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_lblVisible" runat="server" Text='<%# Eval("DEFECTIVE_VISIBLE") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="UI_DefectiveDFLNO" runat="server" Text='<%# Eval("DEFECTIVE_DFLNO") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="004_Defective Name" SortExpression="DEFECTIVE_NAME">
                                                    <HeaderStyle Width="31%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="UI_lblDefecticeName" runat="server" Text='<%# Eval("DEFECTIVE_NAME") %>'></asp:Label>
                                                        <asp:TextBox ID="UI_txtDefecticeName" runat="server" Width="350px" Text='<%# Eval("DEFECTIVE_NAME") %>' Visible="false"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfv_DefecticeName" runat="server" ErrorMessage="013_請輸入Duty名稱" Display="None" TabIndex="0" ValidationGroup="DefectiveListGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="005_Status" SortExpression="Status">
                                                    <HeaderStyle Width="10%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label ID="UI_lblDefecticeStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                                        <asp:DropDownList ID="UI_cboStatus" runat="server" Visible="false">
                                                            <asp:ListItem Text="016_Open" Value="1" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="017_Close" Value="0"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="LUSTMP" SortExpression="LUSTMP" HeaderText="008_Edit Date" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                <asp:BoundField DataField="DEFECTIVE_LUADNAME" SortExpression="DEFECTIVE_LUADNAME" HeaderText="009_Last Editor" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundField>

                                                <asp:TemplateField HeaderText="016_Edit">
                                                    <HeaderStyle Width="8%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Button ID="UI_cmdEdit" runat="server" Text="005_Edit" CssClass="Problem_Edit" CommandName="cmdEdit" CommandArgument='<%#Me.UI_dvDefective.Rows.Count%>' />
                                                        <asp:Button ID="UI_cmdSave" runat="server" Text="002_Save" CssClass="Problem_Edit" CommandName="cmdSave" CommandArgument='<%#Me.UI_dvDefective.Rows.Count%>' Visible="false" ValidationGroup="FailureListGroup" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="017_Delete">
                                                    <HeaderStyle Width="8%" CssClass="ListTitle" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="text9pt"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Button ID="UI_cmdDel" runat="server" Text="007_Delete" CssClass="Problem_Edit" CommandName="cmdDel" CommandArgument='<%#Me.UI_dvDefective.Rows.Count%>' OnClientClick="return FrmDelete()" />
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
                        </table>
                    </td>
                </tr>
                <tr>
                    <td background="Images/pic_20.gif">&nbsp;</td>
                    <td valign="top" bgcolor="#E3D8BE" align="center">
                        <!--[Begin]頁數-->
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr valign="middle">
                                <td width="3%">&nbsp;</td>
                                <td width="94%">&nbsp</td>
                                <td width="3%">&nbsp;
		                    <asp:Label runat="server" ID="UI_lblPreviousPage_DefectiveNo" Visible="false"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <!--[End]頁數-->
                    </td>
                </tr>
            </table>

            <uc2:ucMessage ID="ucMessage" runat="server" />

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="UI_cmdAdd" EventName="Click" />
            <asp:PostBackTrigger ControlID="ucMessage" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:RequiredFieldValidator ID="rfvDefectiveNo" runat="server" ControlToValidate="UI_txtDefectiveNoAdd" ErrorMessage="015_請輸入Duty代碼" Display="None" TabIndex="0" ValidationGroup="DefectiveGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="rfvDefectiveName" runat="server" ControlToValidate="UI_txtDefectiveNameAdd" ErrorMessage="013_請輸入Duty名稱" Display="None" TabIndex="0" ValidationGroup="DefectiveGroup" SetFocusOnError="true"></asp:RequiredFieldValidator>

    <asp:ValidationSummary ID="vsDefective" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="DefectiveGroup" />

</asp:Content>

